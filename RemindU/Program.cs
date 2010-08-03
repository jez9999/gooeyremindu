using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Gooey;

namespace RemindU
{
	static class Program {
		#region Public vars
		
		// Here we store the forms and other data that we only ever want one instance of at a time
		public static frmMain InstanceFrmMain = null;
		public static frmEvents InstanceFrmEvents = null;
		public static AppContext InstanceAppContext = null;
		public static Settings Settings = null;
		public static SortedDictionary<UInt32, Reminder> Events = null;
		public static List<UInt32> EventsOutstanding = new List<UInt32>();
		
		public static RUDStorageManager SM;
		public static Utilities Utils = new Utilities();
		
		// If these is true, these forms will NOT cancel a close performed on them; this will allow the program
		// to fully exit.
		public static bool ReallyCloseFrmMain = false;
		public static bool ReallyCloseFrmEvents = false;
		
		#endregion
		
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			try { InstanceAppContext = new AppContext(); }
			catch (Exception) {
				// Application context threw an exception during init??  This is an unrecoverable error.
				return;
			}
			Application.Run(InstanceAppContext);
		}
	}
}
