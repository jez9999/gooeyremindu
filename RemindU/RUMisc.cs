using System;
using System.Collections.Generic;
using System.Text;

namespace RemindU
{
	// Miscellaneous Remind U functions and structures
	
	public class RUUtilities {
		public static UInt32 GetNextAvailableEventId(SortedDictionary<UInt32, Event> events) {
			// Grab the last event's ID, and increment it for next available ID
			UInt32 newId = 1;
			foreach (KeyValuePair<UInt32, Event> entry in events) {
				newId = entry.Key;
			}
			
			return newId + 1;
		}
		
		public static bool LoadEvents(out string errorMsg) {
			errorMsg = "No error";
			
			try {
				Program.Events = Program.SM.LoadEvents();
			}
			catch (RUDException ex) {
				errorMsg = "Remind U data manager exception occurred whilst loading events: " + ex.Message;
				return false;
			}
			catch (Exception ex) {
				errorMsg = "General exception occured whilst loading events: " + ex.Message;
				return false;
			}
			
			return true;
		}
		
		public static bool UpdateEvents(out string errorMsg) {
			errorMsg = "No error";
			
			try {
				Program.SM.UpdateEvents(Program.Events);
			}
			catch (RUDException ex) {
				errorMsg = "Remind U data manager exception occurred whilst updating events: " + ex.Message;
				return false;
			}
			catch (Exception ex) {
				errorMsg = "Unknown error occurred whilst updating events!  " + ex.Message;
				return false;
			}
			
			return true;
		}
		
		public static bool LoadSettings(out string errorMsg) {
			errorMsg = "No error";
			
			try {
				Program.Settings = Program.SM.LoadSettings();
			}
			catch (RUDException ex) {
				errorMsg = "Remind U data manager exception occurred whilst loading settings: " + ex.Message;
				return false;
			}
			catch (Exception ex) {
				errorMsg = "General exception occured whilst loading settings: " + ex.Message;
				return false;
			}
			
			return true;
		}
		
		public static bool UpdateSettings(out string errorMsg) {
			errorMsg = "No error";
			
			try {
				Program.SM.UpdateSettings(Program.Settings);
			}
			catch (RUDException ex) {
				errorMsg = "Remind U data manager exception occurred whilst updating settings: " + ex.Message;
				return false;
			}
			catch (Exception ex) {
				errorMsg = "Unknown error occurred whilst updating settings!  " + ex.Message;
				return false;
			}
			
			return true;
		}
		
		public static void CheckForNewReminders(DateTime dtNow) {
			// Do a check for new reminders
			string errorMsg;
			DateTime dtNowUtc = dtNow.ToUniversalTime();
//			bool raisedEvents = false;
			
			foreach (UInt32 evId in Program.Events.Keys) {
				Event ev = Program.Events[evId];
				if (
					ev.Acknowledged == false &&
					!Program.EventsOutstanding.Contains(evId) &&
					(
						ev.When.Date.CompareTo(dtNowUtc.Date) < 0 ||
						(
							ev.When.Date.CompareTo(dtNowUtc.Date) == 0 &&
							(
								ev.StartOfDay ||
								(
									ev.When.Hour < dtNowUtc.Hour ||
									(ev.When.Hour == dtNowUtc.Hour && ev.When.Minute <= dtNowUtc.Minute)
								)
							)
						)
					)
				) {
					// Event's not acknowledged, needs to be raised, and should be raised; raise it.
					Program.EventsOutstanding.Add(evId);
//					raisedEvents = true;
				}
			}
			
//			if (raisedEvents) {
				Program.InstanceFrmEvents.RefreshEventsOutstanding();
				if (Program.InstanceFrmEvents.Visible == false) {
					Program.InstanceAppContext.UpdateTrayIconStatus(true, true, false);
				}
//			}
			
			// Update settings (so we store last recorded date/time)
			if (!RUUtilities.UpdateSettings(out errorMsg)) {
				Program.Utils.ShowError(errorMsg);
				return;
			}
		}

		public static string GetReminderSummaryString(Event ev) {
			string reminderDateTime;
			if ( ev.StartOfDay ) {
				reminderDateTime = ev.When.ToLocalTime().ToString("dd MMM yyyy") + "  SoD";
			}
			else {
				reminderDateTime = ev.When.ToLocalTime().ToString("dd MMM yyyy  HH:mm");
			}
			return ("(" + reminderDateTime + ") - " + ev.Title);
		}
	}
	
	/// <summary>
	/// Represents a reminder event.
	/// </summary>
	public class Event {
		private string title;
		private string body;
		
		/// <summary>
		///  *MUST BE UTC!*  The date/time of an event.  If a single event, its date/time.  If a recurring event, the date/time of the first occurance of the event.
		/// </summary>
		public DateTime When { get; set; }
		/// <summary>
		/// An event's title.
		/// </summary>
		public string Title {
			get {
				return this.title;
			}
			set {
				if (value.Replace("	", "").Replace(" ", "") == "") {
					throw new EventValueInvalidException("Reminder title must contain at least one non-whitespace character.");
				}
				if (value.Length > 100) {
					throw new EventValueInvalidException("Reminder title mustn't be longer than 100 characters (is " + value.Length.ToString() + " characters).");
				}
				this.title = value;
			}
		}
		/// <summary>
		/// An event's main body/description.
		/// </summary>
		public string Body {	
			get {
				return this.body;
			}
			set {
				if (value.Length > 10000) {
					throw new EventValueInvalidException("Reminder body mustn't be longer than 10000 characters (is " + value.Length.ToString() + " characters).");
				}
				this.body = value;
			}
		}
		/// <summary>
		/// Whether this reminder is a 'start of day' one, rather than at a specific time.
		/// </summary>
		public bool StartOfDay { get; set; }
		/// <summary>
		/// Whether this reminder has been acknowledged yet, or not.
		/// </summary>
		public bool Acknowledged { get; set; }
	}
	
	/// <summary>
	/// Represents the current program settings.
	/// </summary>
	public class Settings {
		/// <summary>
		/// The date/time of the last recorded new reminder/event check.
		/// </summary>
		public DateTime? LastRecordedDt { get; set; }
	}
	
	/// <summary>
	/// Represents a reminder event in the 'outstanding reminers' listbox
	/// </summary>
	public class ReminderListItem {
		public UInt32 EventId { get; set; }
		public string Description { get; set; }
		
		public override string ToString() {
			return Description;
		}
	}
	
	#region Exceptions
	
	public class EventValueInvalidException : Exception {
		// Thrown if one tries to set a proeprty of an Event to an invalid value.
		
		#region Private vars
		
		private const string defaultMsg = "The value passed to the Event property was invalid.";
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
		
		public EventValueInvalidException(): base(defaultMsg) {
			// No further implementation
		}
		
		public EventValueInvalidException(string message): base(message) {
			// No further implementation
		}
		
		public EventValueInvalidException(string message, Exception innerException): base(message, innerException) {
			// No further implementation
		}
		
		protected EventValueInvalidException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context) {
			// No further implementation
		}
		
		#endregion
	}
	
	#endregion
}
