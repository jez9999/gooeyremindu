using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Gooey;

namespace RemindU {
	public partial class frmMain : Form {
		private DateTime prevSelectedDate;
		private bool evenNo = true;
		
		#region Constructors
		
		public frmMain() {
			InitializeComponent();
		}
		
		#endregion
		
		private void frmMain_Load(object sender, EventArgs ea) {
			// As soon as we load, we need to get the appropriate reminder dates bolded on the calendar.
			RefreshCalendarReminders();
		}
		
		private void frmMain_FormClosing(object sender, FormClosingEventArgs ea) {
			if (!Program.ReallyCloseFrmMain) {
				// Only hide this form; we don't allow multiple instances of it
				this.Hide();
				ea.Cancel = true;
			}
		}
		
		private void calReminderDates_DateSelected(object sender, DateRangeEventArgs ea) {
//			textBox1.Text += "DateSelected... ea.Start: " + ea.Start.ToShortDateString() + "\r\n";
			
			if (prevSelectedDate != ea.Start) {
				//RefreshCalendarReminders();
				//refreshSelectedDateReminders();
			}
			
			prevSelectedDate = calReminderDates.SelectionStart;
		}
		
		private void calReminderDates_KeyDown(object sender, KeyEventArgs ea) {
//			textBox1.Text += "KeyDown...\r\n";
			//Program.Utils.ShowInfo("Date now: " + calReminderDates.SelectionStart.ToShortDateString());
		}
		
		private void button1_Click(object sender, EventArgs ea) {
			Program.Utils.ShowInfo("Remind U version: " + Program.Utils.GetVersionString(System.Reflection.Assembly.GetExecutingAssembly(), VersionStringType.FullString));
		}
		
		private void btnTest_Click(object sender, EventArgs ea) {
//			DateTime dt = new DateTime(2009, 10, 25, 3, 30, 0, DateTimeKind.Local);
//			int s=5;
		}
		
		private void btnEdit_Click(object sender, EventArgs ea) {
			
		}
		
		public void RefreshCalendarReminders() {
			// Fills in the calendar 'bolded' list with current events' dates
			MonthCalendar calendar = calReminderDates;
			DateTime selectionStart;
			DateTime selectionEnd;
			List<DateTime> boldedDates = new List<DateTime>();
 			selectionStart = new DateTime(calendar.SelectionStart.Year, calendar.SelectionStart.Month, 1);
			selectionEnd = selectionStart.AddMonths(1);
			
			foreach (Event ev in Program.Events.Values) {
 				if (ev.When >= selectionStart && ev.When < selectionEnd) { boldedDates.Add(ev.When); }
			}
			calendar.BoldedDates = boldedDates.ToArray();
			//calendar.BoldedDates = new DateTime[] { new DateTime(2009, 10, 5) };
			refreshSelectedDateReminders();
		}
		
		private void calReminderDates_MouseDown(object sender, MouseEventArgs ea) {
//			// Trick to avoid the bug occurring when modifying the MonthCalendar BoldedDates into the DateChanged event 
//			if (
//				calReminderDates.HitTest(e.X, e.Y).HitArea == MonthCalendar.HitArea.NextMonthButton ||
//				calReminderDates.HitTest(e.X, e.Y).HitArea == MonthCalendar.HitArea.PrevMonthButton
//			) {
//				RefreshCalendarReminders();
//			}
		}
		
		private void calReminderDates_DateChanged(object sender, DateRangeEventArgs ea) {
//			textBox1.Text += "DateChanged... ea.Start: " + ea.Start.ToShortDateString() + "\r\n";
//			Program.Utils.ShowInfo("DateChanged");
//			
//			if (lastSelectedDate != ea.Start) {
//				RefreshCalendarReminders();
//				//refreshSelectedDateReminders();
//			}
			
			evenNo = !evenNo;
			if (!evenNo || true) {
				//calReminderDates.BoldedDates = new DateTime[] { new DateTime(2009, 10, 5) };
				RefreshCalendarReminders();
//				textBox1.Text += "setting bolded dates\r\n";
			}
		}
		
		private void lstReminders_SelectedIndexChanged(object sender, EventArgs ea) {
			refreshSelectedReminderInfo();
		}
		
		private void refreshSelectedReminderInfo() {
			if (lstReminders.SelectedItem != null) {
				Event ev = ((Event)Program.Events[((ReminderListItem)lstReminders.SelectedItem).EventId]);
				tbReminderTitle.Text = ev.Title;
				tbReminderBody.Text = Program.Utils.ConvertToWindowsNewlines(ev.Body);
			}
		}
		
		private void refreshSelectedDateReminders() {
			// Fills the reminders list according to the selected date
			lstReminders.Items.Clear();
			foreach (UInt32 evId in Program.Events.Keys) {
				Event reminder = Program.Events[evId];
				if (
					reminder.When.Year == calReminderDates.SelectionStart.Year &&
					reminder.When.Month == calReminderDates.SelectionStart.Month &&
					reminder.When.Day == calReminderDates.SelectionStart.Day
				) {
					ReminderListItem listItem = new ReminderListItem();
					listItem.EventId = evId;
					listItem.Description = RUUtilities.GetReminderSummaryString(Program.Events[evId]);
					lstReminders.Items.Add(listItem);
				}
			}
			
			tbReminderBody.Text = "";
			tbReminderTitle.Text = "";
			if (lstReminders.Items.Count > 0) {
				lstReminders.SelectedIndex = 0;
				btnEdit.Enabled = true;
			}
			else {
				btnEdit.Enabled = false;
			}
		}
	}
}
