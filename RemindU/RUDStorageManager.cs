using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.AccessControl;
using System.Xml;
using System.Xml.XPath;

namespace RemindU
{
	// Remind U data storage code here (loading/saving options, reminder events, etc.)
	
	/// <summary>
	/// Class that allows events to be stored to / loaded from disk.  They will be stored in a different file to config options.
	/// </summary>
	public class RUDStorageManager {
		private string appDataDir;
		
		private bool ensureDirExists(out string errorMsg) {
			errorMsg = "";
			
			// Make sure our app's data dir exists
			try {
				Directory.CreateDirectory(this.appDataDir);
			}
			catch (Exception ex) {
				errorMsg = "Couldn't ensure that program data directory existed.  Please check that you have permission to access this path (and that it is a directory): " + this.appDataDir + "(" + ex.Message + ")";
				return false;
			}
			
			// Make sure no backup files exist (if they do, looks like we were stopped in a move/write/delete
			// operation last time?)
			if (File.Exists(this.appDataDir + "\\events.xml.backup") || File.Exists(this.appDataDir + "\\settings.xml.backup")) {
				errorMsg = "One of the data file's backup files exists already in the data dir.  This shouldn't be the case unless there was an unexpected failure during a previous save operation.";
				return false;
			}
			
			return true;
		}
		
		public RUDStorageManager() {
			this.appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\RemindU";
		}
		
		/// <summary>
		/// Load the sorted dictionary of events stored in our events file, in the current user's app settings dir.
		/// </summary>
		/// <returns>The sorted dictionary of Event objects representing the events we loaded from the events file.</returns>
		public SortedDictionary<UInt32, Event> LoadEvents() {
			string errorMsg;
			if (!ensureDirExists(out errorMsg)) { throw new RUDException(errorMsg); }
			
			SortedDictionary<UInt32, Event> events = new SortedDictionary<UInt32, Event>();
			
			// Read the events XML file from disk
			FileStream fsEvents;
			try {
				fsEvents = File.Open(this.appDataDir + "\\events.xml", FileMode.Open, FileAccess.Read, FileShare.None);
			}
			catch (FileNotFoundException) {
				// This is a special case, and it's OK.  As no file exists we'll simply assume an empty dictionary
				// of events.
				return events;
			}
			catch (Exception ex) {
				throw new RUDException("Couldn't open events.xml file because: " + ex.Message);
			}
			
			// From now on, make sure all code is inside the try block that has the finally that closes the FS.
			XmlDocument doc;
			try {
				// First, create our XmlDocument for storing event values
				doc = new XmlDocument();
				
				// Create the XmlReader that will read the XML document from disk
				XmlReader reader = XmlReader.Create(fsEvents);
				
				// Read and load it, and close the reader
				doc.Load(reader);
				reader.Close();
			}
			catch (XmlException ex) {
				throw new RUDException("Couldn't deserialize data from events.xml - file appears to be corrupted.  Error: " + ex.Message);
			}
			catch (Exception ex) {
				throw new RUDException("Couldn't load data from events.xml because: " + ex.Message);
			}
			finally {
				fsEvents.Close();
			}
			
			// Create an XPathNavigator so we can easily query values from the XmlDocument
			XPathNavigator nav = doc.CreateNavigator();
			
			// Enumerate through all the event nodes we can find
			XPathNodeIterator iterEvent = nav.Select("/events/event");
			while (iterEvent.MoveNext()) {
				UInt32 evId;
				try {
					evId = Convert.ToUInt32(iterEvent.Current.GetAttribute("evId", ""));
				}
				catch (FormatException) {
					throw new RUDException("Couldn't load event from events.xml as its ID (" + iterEvent.Current.GetAttribute("evId", "") + ") is not a valid unsigned int32.");
				}
				catch (OverflowException) {
					throw new RUDException("Couldn't load event from events.xml as its ID (" + iterEvent.Current.GetAttribute("evId", "") + ") has caused int32 underflow/overflow.");
				}
				
				XPathNodeIterator iterWhen = iterEvent.Current.Select("when");
				DateTime when;
				XPathNodeIterator iterTitle = iterEvent.Current.Select("title");
				string title;
				XPathNodeIterator iterBody = iterEvent.Current.Select("body");
				string body;
				XPathNodeIterator iterStartOfDay = iterEvent.Current.Select("startOfDay");
				bool startOfDay;
				XPathNodeIterator iterAcknowledged = iterEvent.Current.Select("acknowledged");
				bool acknowledged;
				
				// Get 'when' value
				if (iterWhen.Count != 1) { throw new RUDException("Couldn't load event from events.xml with ID (" + evId + ") as it doesn't have exactly one 'when' element."); }
				iterWhen.MoveNext();
				string strWhen = iterWhen.Current.Value;
				// We're going to rely on the 'when' having been serialized to a UTC, ISO8601-formatted, string.
				// This is an unambiguous format, so .Parse should just be able to parse it normally.
				when = DateTime.Parse(strWhen).ToUniversalTime();
				
				// Get 'title' value
				if (iterTitle.Count != 1) { throw new RUDException("Couldn't load event from events.xml with ID (" + evId + ") as it doesn't have exactly one 'title' element."); }
				iterTitle.MoveNext();
				title = iterTitle.Current.Value;
				
				// Get 'body' value
				if (iterBody.Count != 1) { throw new RUDException("Couldn't load event from events.xml with ID (" + evId + ") as it doesn't have exactly one 'body' element."); }
				iterBody.MoveNext();
				body = iterBody.Current.Value;
				
				// Get 'startOfDay' value
				if (iterStartOfDay.Count != 1) { throw new RUDException("Couldn't load event from events.xml with ID (" + evId + ") as it doesn't have exactly one 'startOfDay' element."); }
				iterStartOfDay.MoveNext();
				string strStartOfDay = iterStartOfDay.Current.Value;
				// Start of day value must be 'true' or 'false'.
				switch (strStartOfDay) {
					case "true":
					startOfDay = true;
					break;
					
					case "false":
					startOfDay = false;
					break;
					
					default:
					throw new RUDException("Couldn't load event from events.xml with ID (" + evId + ") as 'startOfDay' element doesn't contain a valid boolean value.");
				}
				
				// Get 'acknowledged' value
				if (iterAcknowledged.Count == 0) {
					// Default to not acknowledged
					acknowledged = false;
				}
				else if (iterAcknowledged.Count > 1) { throw new RUDException("Couldn't load event from events.xml with ID (" + evId + ") as it has more than one 'acknowledged' element."); }
				else {
					iterAcknowledged.MoveNext();
					string strAcknowledged = iterAcknowledged.Current.Value;
					// Acknowledged value must be 'true' or 'false'
					switch (strAcknowledged) {
						case "true":
						acknowledged = true;
						break;
						
						case "false":
						acknowledged = false;
						break;
						
						default:
						throw new RUDException("Couldn't load event from events.xml with ID (" + evId + ") as 'acknowledged' element doesn't contain a valid boolean value.");
					}
				}
				
				try {
					events.Add(evId, new Event { When = when, Title = title, Body = body, StartOfDay = startOfDay, Acknowledged = acknowledged });
				}
				catch (ArgumentException) {
					throw new RUDException("Couldn't load events from events.xml - a duplicate ID (" + evId + ") exists!");
				}
				catch (Exception ex) {
					throw new RUDException("Couldn't load event from events.xml with ID (" + evId + ") because: " + ex.Message);
				}
			}
			
			return events;
		}
		
		/// <summary>
		/// Update the sorted dictionary of events stored in our events XML file, in the current user's app settings dir.
		/// </summary>
		/// <param name="events">The sorted dictionary of Event objects representing the events to write to the XML file.</param>
		public void UpdateEvents(SortedDictionary<UInt32, Event> events) {
			string errorMsg;
			if (!ensureDirExists(out errorMsg)) { throw new RUDException(errorMsg); }
			
			// Rename old XML data file if it's there
			try {
				File.Move(this.appDataDir + "\\events.xml", this.appDataDir + "\\events.xml.backup");
			}
			catch (FileNotFoundException) {
				// Well, that's OK.  We're gonna create the source file from scratch.
			}
			catch (IOException) {
				throw new RUDException("The events.xml.backup file exists already in the data dir.  This shouldn't be the case unless there was an unexpected failure during a previous save operation.  Operation aborted.");
			}
			catch (Exception ex) {
				throw new RUDException("Couldn't move existing events.xml file to backup file because: " + ex.Message);
			}
			
			// Write the new XML file to disk
			FileStream fsEvents;
			try {
				fsEvents = File.Create(this.appDataDir + "\\events.xml");
			}
			catch (Exception ex) {
				throw new RUDException("Couldn't create new events.xml file during update because: " + ex.Message);
			}
			
			// From now on, make sure all code is inside the try block that has the finally that closes the FS.
			try {
				// Build up XML document in mem, then write to disk via already-opened stream
				XmlDocument doc = new XmlDocument();
				XmlElement rootElement;
				doc.AppendChild(
					rootElement = doc.CreateElement("events")
				);
				
				foreach (KeyValuePair<UInt32, Event> entry in events) {
					XmlElement eventElement = doc.CreateElement("event");
					XmlElement whenElement = doc.CreateElement("when");
					XmlElement titleElement = doc.CreateElement("title");
					XmlElement bodyElement = doc.CreateElement("body");
					XmlElement startOfDayElement = doc.CreateElement("startOfDay");
					XmlElement acknowledgedElement = doc.CreateElement("acknowledged");
					
					// Insert 'when' value
					// We're going to output the date/time to a UTC, ISO8601-formatted, string.  Also, we don't
					// want to record seconds.  As .ToString("o") outputs seconds and fractions of a second, we'll
					// custom-format the string to get rid of them.
					if (entry.Value.When.Kind != DateTimeKind.Utc) {
						throw new RUDException("Unable to continue updating reminder events - event with ID (" + entry.Key.ToString() + ") doesn't appear to have been stored in memory as a UTC time!");
					}
					XmlText whenText = doc.CreateTextNode(entry.Value.When.ToString(@"yyyy-MM-dd\THH:mm\Z"));
					whenElement.AppendChild(whenText);
					eventElement.AppendChild(whenElement);
					
					// Insert 'title' value
					titleElement.AppendChild(doc.CreateTextNode(entry.Value.Title));
					eventElement.AppendChild(titleElement);
					
					// Insert 'body' value
					bodyElement.AppendChild(doc.CreateTextNode(entry.Value.Body));
					eventElement.AppendChild(bodyElement);
					
					// Insert 'startOfDay' value
					startOfDayElement.AppendChild(doc.CreateTextNode(entry.Value.StartOfDay ? "true" : "false"));
					eventElement.AppendChild(startOfDayElement);
					
					// Insert 'acknowledged' value
					acknowledgedElement.AppendChild(doc.CreateTextNode(entry.Value.Acknowledged ? "true" : "false"));
					eventElement.AppendChild(acknowledgedElement);
					
					XmlAttribute evIdAttribute = doc.CreateAttribute("evId");
					evIdAttribute.Value = entry.Key.ToString();
					eventElement.Attributes.Append(evIdAttribute);
					rootElement.AppendChild(eventElement);
				}
				
				// Grab an XPathNavigator to navigate thru our entire XmlDocument tree
				XPathNavigator nav = doc.CreateNavigator();
				
				// Create the XmlWriter that will write the XML document to disk
				XmlWriterSettings sett = new XmlWriterSettings();
				sett.Indent = true;
				sett.IndentChars = "\t";
				XmlWriter writer = XmlWriter.Create(fsEvents, sett);
				// Finally write it, and close the writer
				nav.WriteSubtree(writer);
				writer.Close();
			}
			finally {
				fsEvents.Close();
			}
			// If we haven't failed by this point and have successfully written the new file, delete the backup.
			File.Delete(this.appDataDir + "\\events.xml.backup");
		}
		
		/// <summary>
		/// Load the settings stored in our settings file, in the current user's app settings dir.
		/// </summary>
		/// <returns>The Settings object representing the settings we loaded from the settings file.</returns>
		public Settings LoadSettings() {
			string errorMsg;
			if (!ensureDirExists(out errorMsg)) { throw new RUDException(errorMsg); }
			
			Settings settings = new Settings();
			
			// Read the settings XML file from disk
			FileStream fsSettings;
			try {
				fsSettings = File.Open(this.appDataDir + "\\settings.xml", FileMode.Open, FileAccess.Read, FileShare.None);
			}
			catch (FileNotFoundException) {
				// This is a special case, and it's OK.  As no file exists we'll simply assume an empty collection
				// of settings.
				return settings;
			}
			catch (Exception ex) {
				throw new RUDException("Couldn't open settings.xml file because: " + ex.Message);
			}
			
			// From now on, make sure all code is inside the try block that has the finally that closes the FS.
			XmlDocument doc;
			try {
				// First, create our XmlDocument for storing settings values
				doc = new XmlDocument();
				
				// Create the XmlReader that will read the XML document from disk
				XmlReader reader = XmlReader.Create(fsSettings);
				
				// Read and load it, and close the reader
				doc.Load(reader);
				reader.Close();
			}
			catch (XmlException ex) {
				throw new RUDException("Couldn't deserialize data from settings.xml - file appears to be corrupted.  Error: " + ex.Message);
			}
			catch (Exception ex) {
				throw new RUDException("Couldn't load data from settings.xml because: " + ex.Message);
			}
			finally {
				fsSettings.Close();
			}
			
			// Create an XPathNavigator so we can easily query values from the XmlDocument
			XPathNavigator nav = doc.CreateNavigator();
			
			// Grab the settings values
			XPathNodeIterator iterLastRecordedDt = nav.Select("/settings/lastRecordedDt");
			
			// Get 'lastRecordedDt' value (if count's 0, no problem; this is nullable)
			if (iterLastRecordedDt.Count > 1) { throw new RUDException("Couldn't load lastRecordedDt setting from settings.xml as its element appears more than once."); }
			else if (iterLastRecordedDt.Count == 1) {
				iterLastRecordedDt.MoveNext();
				string strLastRecordedDt = iterLastRecordedDt.Current.Value;
				// We're going to rely on the 'when' having been serialized to a local, ISO8601-formatted, string.
				// This is an unambiguous format, so .Parse should just be able to parse it normally.
				settings.LastRecordedDt = DateTime.Parse(strLastRecordedDt);
			}
			
			return settings;
		}
		
		/// <summary>
		/// Update the settings stored in our settings XML file, in the current user's app settings dir.
		/// </summary>
		/// <param name="settings">The Settings object representing the settings to write to the XML file.</param>
		public void UpdateSettings(Settings settings) {
			string errorMsg;
			if (!ensureDirExists(out errorMsg)) { throw new RUDException(errorMsg); }
			
			// Rename old XML data file if it's there
			try {
				File.Move(this.appDataDir + "\\settings.xml", this.appDataDir + "\\settings.xml.backup");
			}
			catch (FileNotFoundException) {
				// Well, that's OK.  We're gonna create the source file from scratch.
			}
			catch (IOException) {
				throw new RUDException("The settings.xml.backup file exists already in the data dir.  This shouldn't be the case unless there was an unexpected failure during a previous save operation.  Operation aborted.");
			}
			catch (Exception ex) {
				throw new RUDException("Couldn't move existing settings.xml file to backup file because: " + ex.Message);
			}
			
			// Write the new XML file to disk
			FileStream fsSettings;
			try {
				fsSettings = File.Create(this.appDataDir + "\\settings.xml");
			}
			catch (Exception ex) {
				throw new RUDException("Couldn't create new settings.xml file during update because: " + ex.Message);
			}
			
			// From now on, make sure all code is inside the try block that has the finally that closes the FS.
			try {
				// Build up XML document in mem, then write to disk via already-opened stream
				XmlDocument doc = new XmlDocument();
				XmlElement rootElement;
				doc.AppendChild(
					rootElement = doc.CreateElement("settings")
				);
				
				XmlElement lastRecordedDtElement = doc.CreateElement("lastRecordedDt");
				
				// Insert 'lastRecordedDt' value
				if (settings.LastRecordedDt != null) {
					// We're going to output the date/time to a local, ISO8601-formatted, string.  As we're
					// purposely outputting this date/time as a local, rather than a UTC, we can use
					// .ToString("o") - it should format this properly.
					if (settings.LastRecordedDt.Value.Kind != DateTimeKind.Local) {
						throw new RUDException("Unable to continue updating settings - last recorded date/time doesn't appear to have been stored in memory as a local time!");
					}
					XmlText lastRecordedDtText = doc.CreateTextNode(settings.LastRecordedDt.Value.ToString("o"));
					lastRecordedDtElement.AppendChild(lastRecordedDtText);
					rootElement.AppendChild(lastRecordedDtElement);
				}
				
				// Grab an XPathNavigator to navigate thru our entire XmlDocument tree
				XPathNavigator nav = doc.CreateNavigator();
				
				// Create the XmlWriter that will write the XML document to disk
				XmlWriterSettings sett = new XmlWriterSettings();
				sett.Indent = true;
				sett.IndentChars = "\t";
				XmlWriter writer = XmlWriter.Create(fsSettings, sett);
				// Finally write it, and close the writer
				nav.WriteSubtree(writer);
				writer.Close();
			}
			finally {
				fsSettings.Close();
			}
			// If we haven't failed by this point and have successfully written the new file, delete the backup.
			File.Delete(this.appDataDir + "\\settings.xml.backup");
		}
	}
	
	#region Exceptions
	
	public class RUDException : Exception {
		// General Remind U data exception class
		
		#region Private vars
		
		private const string defaultMsg = "There was a miscellaneous error with the Remind U data engine.";
		// ^ As any 'const' is made a compile-time constant, this text will obviously be
		// available to the constructors before the object has been instantiated, as is
		// necessary.
		
		#endregion
		
		#region Constructors
		// Note that the 'public ClassName(...): base() {' notation is explicitly telling
		// the compiler to call this class's base class's empty constructor.  A constructor
		// HAS to call either a base() or this() constructor before its own body, and the
		// '(...): base()' notation (with the colon) is the way to do it explicitly.  If you
		// don't use this notation, base() will be implicitly called.  Therefore, this:
		// public ClassName(...) {...}
		// is identical to this:
		// public ClassName(...): base() {...}
		// 
		// For more information, see:
		// http://msdn2.microsoft.com/en-us/library/aa645603.aspx
		// http://www.jaggersoft.com/csharp_standard/17.10.1.htm
		
		public RUDException(): base(defaultMsg) {
			// No further implementation
		}
		
		public RUDException(string message): base(message) {
			// No further implementation
		}
		
		public RUDException(string message, Exception innerException): base(message, innerException) {
			// No further implementation
		}
		
		protected RUDException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context) {
			// No further implementation
		}
		
		#endregion
	}
	
	#endregion
}
