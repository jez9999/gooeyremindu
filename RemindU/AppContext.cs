using System;
using System.Reflection;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace RemindU
{
	public class AppContext : ApplicationContext {
		#region Private vars
		
		private IContainer components = null;
		private NotifyIcon trayIcon = null;
		private Timer tmrEventCheck = null;
		private Timer tmrFlashTray = null;
		private bool trayIconIsTransparent = false;
		private bool trayIconIsFlashing = false;
		private bool trayIconIsActivated = false;
		private bool firstEventCheck = true; // The very first event check tick, we always want to check for new events
		private Icon icoApp = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("RemindU._Icons.ruApp.ico"));
		private Icon icoTrans = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("RemindU._Icons.transparent.ico"));
		private ContextMenu iconMenu;
		
		#endregion
		
		#region Constructors
		
		public AppContext() {
			components = new System.ComponentModel.Container();
			
			string errorMsg;
			
			Program.SM = new RUDStorageManager();
			// First up, load current settings
			if (!RUUtilities.LoadSettings(out errorMsg)) {
				Program.Utils.ShowError(errorMsg);
				throw new Exception("AppContext initialization failed!  Fatal error.");
			}
			// Now, load existing reminders/events
			if (!RUUtilities.LoadEvents(out errorMsg)) {
				Program.Utils.ShowError(errorMsg);
				throw new Exception("AppContext initialization failed!  Fatal error.");
			}
			
			// Put ourselves in the system tray
			trayIcon = new NotifyIcon(components);
			trayIcon.Text = "Remind U";
			trayIcon.MouseDoubleClick += new MouseEventHandler(trayIcon_MouseDoubleClick);
			trayIcon.Icon = icoApp;
			
			iconMenu = new ContextMenu();
			iconMenu.Popup += new EventHandler(iconMenu_Popup);
			
			trayIcon.ContextMenu = iconMenu;
			trayIcon.Visible = true;
			
			// Setup the timer to flash the systray icon
			tmrFlashTray = new Timer(components);
			// TODO: Make this configurable
			tmrFlashTray.Interval = 1000;
			tmrFlashTray.Tick += new EventHandler(tmrFlashTray_Tick);
			
			// Create our singleton Form instances
			Program.InstanceFrmMain = new frmMain();
			Program.InstanceFrmEvents = new frmEvents();
			
			// Setup the timer to check for new reminders/events
			tmrEventCheck = new Timer(components);
			tmrEventCheck.Interval = 1000;
			tmrEventCheck.Tick += new System.EventHandler(tmrEventCheck_Tick);
			tmrEventCheck.Enabled = true;
		}
		
		#endregion
		
		#region Internal
		
		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (components != null) { components.Dispose(); }
			}
			base.Dispose(disposing);
		}
		
		#endregion
		
		#region SysTray icon stuff
		
		/// <summary>
		/// Populates the popup menu to be displayed when the user right-clicks on the system tray icon.  If the menu has been cleared and therefore won't be displayed, returns false to indicate that frmMain should be activated.
		/// </summary>
		/// <param name="defaultMenuItem">A reference to the MenuItem that will be set to the default menu item if the menu was able to be populated and therefore the return value is true.</param>
		/// <returns>If the popup menu was able to be populated, true; otherwise false, to indicate that frmMain should be activated (is probably modal).</returns>
		private bool populateIconMenu(out MenuItem defaultMenuItem) {
			defaultMenuItem = null;
			
			iconMenu.MenuItems.Clear();
			// Popup tray icon right-click menu IIF frmMain is able to focus... otherwise activate it so that the
			// user sees that they need to deal with its modal dialog first.  Also, if the form's hidden, assume
			// that it's not modal and so we can popup the menu.
			if (Program.InstanceFrmMain.CanFocus || Program.InstanceFrmMain.Visible == false) {
				bool showPendingRem = false;
				if (Program.EventsOutstanding.Count > 0 && Program.InstanceFrmEvents.Visible == false) { showPendingRem = true; }
				if (showPendingRem) {
					MenuItem miViewPending = new MenuItem("&View pending reminder(s)", new EventHandler(miViewPending_Click));
					iconMenu.MenuItems.Add(miViewPending);
					miViewPending.DefaultItem = true;
					defaultMenuItem = miViewPending;
					MenuItem miSepOpen = new MenuItem("-");
					iconMenu.MenuItems.Add(miSepOpen);
				}
				MenuItem miOpen = new MenuItem("&Open Remind U", new EventHandler(miOpen_Click));
				iconMenu.MenuItems.Add(miOpen);
				if (!showPendingRem) {
					miOpen.DefaultItem = true;
					defaultMenuItem = miOpen;
				}
				MenuItem miAddNew = new MenuItem("&Add new reminder", new EventHandler(miAddNew_Click));
				iconMenu.MenuItems.Add(miAddNew);
				MenuItem miSepExit = new MenuItem("-");
				iconMenu.MenuItems.Add(miSepExit);
				MenuItem miExit = new MenuItem("E&xit", new EventHandler(miExit_Click));
				iconMenu.MenuItems.Add(miExit);
				
				return true;
			}
			else {
				// Simply don't populate popup menu - if it's Clear'ed, it won't display at all! :-)
				defaultMenuItem = null;
				return false;
			}
		}
		
		private bool populateIconMenu() {
			MenuItem throwAway;
			return populateIconMenu(out throwAway);
		}
		
		private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs ea) {
			if (ea.Button == MouseButtons.Left) {
				// Build what would have been the right-click context menu, and activate the default menu item
				MenuItem defaultMenuItem;
				if (!populateIconMenu(out defaultMenuItem)) { Program.InstanceFrmMain.Activate(); }
				else if (defaultMenuItem != null) { defaultMenuItem.PerformClick(); }
			}
		}
		
		private void iconMenu_Popup(object sender, EventArgs ea) {
			if (!populateIconMenu()) { Program.InstanceFrmMain.Activate(); }
		}
		
		private void miViewPending_Click(object sender, EventArgs ea) {
			// Stop the trayicon flashing
			UpdateTrayIconStatus(false, false, true);
			
			// Open the 'view pending reminders' form
			Program.InstanceFrmEvents.Show();
			Program.InstanceFrmEvents.Activate();
		}
		
		private void miOpen_Click(object sender, EventArgs ea) {
			// Open the main Remind U form
			Program.InstanceFrmMain.Show();
			Program.InstanceFrmMain.Activate();
		}
		
		private void miAddNew_Click(object sender, EventArgs ea) {
			// Open an 'add new reminder' form
			(new frmAddEdit()).Show();
		}
		
		private void miExit_Click(object sender, EventArgs ea) {
			// Using Application.Exit allows any open form to cancel the exit in the FormClosing event; it's the
			// best ('politest') way to try to close the application.
			Program.ReallyCloseFrmMain = true;
			Program.ReallyCloseFrmEvents = true;
			Application.Exit();
		}
		
		private void tmrFlashTray_Tick(object sender, EventArgs ea) {
			if (trayIconIsTransparent) {
				this.trayIcon.Icon = this.icoApp;
			}
			else {
				this.trayIcon.Icon = this.icoTrans;
			}
			
			trayIconIsTransparent = !trayIconIsTransparent;
		}
		
		#endregion
		
		#region Event check timer stuff
		
		private void tmrEventCheck_Tick(object sender, EventArgs ea) {
			string errorMsg;
			
			// Although we tick every second, we only check for new reminders on a new minute (minimum).
			bool checkNew = false;
			DateTime dtNow = DateTime.Now;
			DateTime dtNowUtc = dtNow.ToUniversalTime();
			if (Program.Settings.LastRecordedDt == null) {
				// Maybe first time the program's been run; for some reason, there's no last recorded date/time
				// available.  So, we'll set it to Now.
				Program.Settings.LastRecordedDt = dtNow;
				checkNew = true;
			}
			else {
				if (
					Program.Settings.LastRecordedDt.Value.Year != dtNow.Year ||
					Program.Settings.LastRecordedDt.Value.Month != dtNow.Month ||
					Program.Settings.LastRecordedDt.Value.Day != dtNow.Day ||
					Program.Settings.LastRecordedDt.Value.Hour != dtNow.Hour ||
					Program.Settings.LastRecordedDt.Value.Minute != dtNow.Minute
				) {
					// TODO: Make this DST change notification configurable.
					// We're on the cusp of a new minute.  Do reminder check, and...
					// ... if DST change has occurred, create a new reminder on-the-fly to inform us of this.
					if (Program.Settings.LastRecordedDt.Value.IsDaylightSavingTime() != dtNow.IsDaylightSavingTime()) {
						// Generate the DST reminder event on-the-fly
						try {
							// The length of time by which the offset changed?  We need to get (and represent as a
							// string) the difference in offsets between the previous DateTime, and this DateTime.
							// If it's negative, we need to pretend it's positive, as we always want to display
							// a positive value (eg. clocks went back by 01:00:00, clocks went forward
							// by 01:00:00).
							TimeSpan offsetDiff = new TimeSpan(new DateTimeOffset(dtNow).Offset.Ticks - new DateTimeOffset(Program.Settings.LastRecordedDt.Value).Offset.Ticks);
							if (offsetDiff.Ticks < 0) { offsetDiff = offsetDiff.Negate(); }
							// We'll assume the offset change is less than a day.  If it was more, something
							// really wacky happened.  :-)
							string offsetChangeStr = String.Format("{0:00}:{1:00}:{2:00}", offsetDiff.Hours, offsetDiff.Minutes, offsetDiff.Seconds);
							
							string rmdrBody = String.Format(
@"This is just a quick reminder to inform you that the daylight saving time status on your system has been changed.

The date/time jumped from:
{0} ({1})
... to:
{2} ({3})

... meaning that the clocks jumped {4} by {5}!",
Program.Settings.LastRecordedDt.Value.ToString(""),
(Program.Settings.LastRecordedDt.Value.IsDaylightSavingTime() ? "DST" : "non-DST"),
dtNow.ToString(""),
dtNow.IsDaylightSavingTime() ? "DST" : "non-DST",
dtNow.IsDaylightSavingTime() ? "forwards" : "back",
offsetChangeStr
							);
							Program.Events.Add(
								RUUtilities.GetNextAvailableEventId(Program.Events),
								new Event {
									When = dtNowUtc,
									Title = "Daylight saving time change notification",
									Body = rmdrBody,
									StartOfDay = false,
									Acknowledged = false
								}
							);
						}
						catch (EventValueInvalidException ex) {
							Program.Utils.ShowError(ex.Message);
							return;
						}
						
						if (!RUUtilities.UpdateEvents(out errorMsg)) {
							Program.Utils.ShowError(errorMsg);
							return;
						}
					}
					checkNew = true;
				}
				// Always update last recorded date/time for next check
				Program.Settings.LastRecordedDt = dtNow;
			}
			if (firstEventCheck) {
				firstEventCheck = false;
				checkNew = true;
			}
			
			if (checkNew) { RUUtilities.CheckForNewReminders(dtNow); }
		}
		
		#endregion
		
		#region Public methods
		
		public void UpdateTrayIconStatus(bool flashTrayIcon, bool showBalloon, bool forceStopTrayIconFlashing) {
			if (Program.EventsOutstanding.Count > 0) {
				bool activatedTrayIcon = false;
				if (!this.trayIconIsFlashing && flashTrayIcon) {
					trayIcon.Text = "Remind U - " + Program.EventsOutstanding.Count.ToString() + " reminder" + (Program.EventsOutstanding.Count > 1 ? "s" : "") + " pending";
					
					// Flash tray icon
					tmrFlashTray_Tick(null, null);
					tmrFlashTray.Start();
					this.trayIconIsFlashing = true;
					activatedTrayIcon = true;
				}
				
				if (showBalloon && activatedTrayIcon) {
					// Display balloon tip
					// TODO: make configurable, ie. allow a title, different format, maybe even an icon...
					//trayIcon.BalloonTipTitle = "someTitle";
					if (Program.EventsOutstanding.Count == 1) {
						trayIcon.BalloonTipText = "Reminder - " + Program.Events[Program.EventsOutstanding[0]].Title;
					}
					else {
						trayIcon.BalloonTipText = Program.EventsOutstanding.Count.ToString() + " reminders pending.";
					}
					trayIcon.BalloonTipIcon = ToolTipIcon.None;
					// Bare minimum time a balloon tip will display is generally 10 seconds (10000ms) and even then only
					// if you're actively using the computer; otherwise it will just wait.
					// You can't close a balloon tip programatically; if you show a balloon tip for an app when another
					// is displaying for that app, the other should immediately just be replaced by the new one.
					trayIcon.ShowBalloonTip(10000);
				}
				
				if (activatedTrayIcon) { this.trayIconIsActivated = true; }
			}
			else {
				trayIcon.Text = "Remind U";
				
				if (this.trayIconIsFlashing) {
					// No pending reminders and tray icon is flashing; stop it.
					forceStopTrayIconFlashing = true;
				}
				
			}
			
			if (forceStopTrayIconFlashing) {
				tmrFlashTray.Stop();
				if (trayIconIsTransparent) { tmrFlashTray_Tick(null, null); }
				this.trayIconIsFlashing = false;
				this.trayIconIsActivated = false;
			}
		}
		
		#endregion
	}
}
