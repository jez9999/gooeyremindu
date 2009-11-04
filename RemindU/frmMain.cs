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

		bool refreshingList;

		#region Constructors

		public frmMain() {
			InitializeComponent();
		}

		#endregion

		private void frmMain_Load(object sender, EventArgs ea) {
			// All of this logic has been moved to Program.
			RefreshCalendarReminders();
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs ea) {
			if (!Program.ReallyCloseFrmMain) {
				// Only hide this form; we don't allow multiple instances of it
				this.Hide();
				ea.Cancel = true;
			}
		}

		private void button1_Click(object sender, EventArgs ea) {
			Program.Utils.ShowInfo("Remind U version: " + Program.Utils.GetVersionString(System.Reflection.Assembly.GetExecutingAssembly(), VersionStringType.FullString));
		}

		private void btnTest_Click(object sender, EventArgs ea) {
			//			DateTime dt = new DateTime(2009, 10, 25, 3, 30, 0, DateTimeKind.Local);
			//			int s=5;
		}

		private void calReminderDates_MouseDown(object sender, MouseEventArgs e) {
			// Trick to avoid the bug occurring when modifying the MonthCalendar BoldedDates into the DateChanged event 
			if (calReminderDates.HitTest(e.X, e.Y).HitArea == MonthCalendar.HitArea.NextMonthButton
				|| calReminderDates.HitTest(e.X, e.Y).HitArea == MonthCalendar.HitArea.PrevMonthButton) {
				RefreshCalendarReminders();
			}
		}

		private DateTime lastSelectedDate;

		private void calReminderDates_DateChanged(object sender, DateRangeEventArgs e) {
			if (lastSelectedDate != e.Start) {
				refreshSelectedDateReminders();
			}
		}

		private void lstReminders_SelectedIndexChanged(object sender, EventArgs e) {
			if (!refreshingList) { refreshSelectedReminderInfo(); }
		}

		private void refreshSelectedReminderInfo() {
			if (lstReminders.SelectedItem != null) {
				Event ev = ((Event)Program.Events[((ReminderListItem)lstReminders.SelectedItem).EventId]);
				tbReminderTitle.Text = ev.Title;
				tbReminderBody.Text = Program.Utils.ConvertToWindowsNewlines(ev.Body);
			}
		}

		public void RefreshCalendarReminders() {
			// Fills in the calendar with current events
			MonthCalendar calendar = calReminderDates;
			DateTime selectionStart = calendar.SelectionStart;
			DateTime selectionEnd = calendar.SelectionStart;
			List<DateTime> boldedDates = new List<DateTime>();

			foreach (Event ev in Program.Events.Values) {
				selectionStart = new DateTime(calendar.SelectionStart.Year, calendar.SelectionStart.Month, 1);
				selectionEnd = selectionStart.AddMonths(1);

				if (ev.When >= selectionStart && ev.When < selectionEnd) { boldedDates.Add(ev.When); }
			}
			calendar.BoldedDates = boldedDates.ToArray();
			refreshSelectedDateReminders();
		}

		private void refreshSelectedDateReminders() {
			// Fills the reminders list according to the selected date
			refreshingList = true;
			lastSelectedDate = calReminderDates.SelectionStart;
			lstReminders.Items.Clear();
			foreach (UInt32 evId in Program.Events.Keys) {
				Event reminder = Program.Events[evId];
				if (reminder.When.ToShortDateString() == calReminderDates.SelectionStart.ToShortDateString()) {
					ReminderListItem listItem = new ReminderListItem();
					listItem.EventId = evId;
					listItem.Description = reminder.Title;
					lstReminders.Items.Add(listItem);
				}
			}
			refreshingList = false;
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
		
		private void btnEdit_Click(object sender, EventArgs e) {

		}
	}
}
