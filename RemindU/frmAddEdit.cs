using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gooey;

namespace RemindU
{
	public partial class frmAddEdit : Form {
		#region Private vars
		
		// The second some data is changed, we consider the dialog 'tainted'; that is, we give a prompt to the
		// user if they try to cancel or close the form, as they will lose data.
		private bool tainted = false;
		// If isAddDialog, is an add reminder dialog; else, is an edit reminder dialog.
		private bool isAddDialog;
		
		#endregion
		
		public frmAddEdit() {
			InitializeComponent();
		}
		
		private void taint() {
			tainted = true;
			
			// TODO: If this is an 'edit' form, enable the Apply button.
		}
		
		private void frmAddEdit_FormClosing(object sender, FormClosingEventArgs ea) {
			if (tainted) {
				this.Activate();
				string dialogType;
				if (isAddDialog) { dialogType = "add new reminder"; }
				else { dialogType = "edit reminder"; }
				if (Program.Utils.ShowYesNo("Close reminder dialog", "Do you want to close this " + dialogType + " dialog?  If you do, you'll lose any data you've entered.") != DialogResult.Yes) {
					ea.Cancel = true;
				}
			}
		}
		
		private void frmAddEdit_Load(object sender, EventArgs ea) {
			// Initialize time picker size/location; centred underneath date calendar picker
			//pickReminderTime.Width = Convert.ToInt32(Math.Round(calReminderDate.Width / 2.5, MidpointRounding.AwayFromZero));
			pickReminderTime.Width = calReminderDate.Width / 2;
			pickReminderTime.Left += calReminderDate.Width / 2 - pickReminderTime.Width / 2;
			cbReminderStartDay.Left += calReminderDate.Width / 2 - cbReminderStartDay.Width / 2;
			
			// We're going to be ignoring the pickReminderTime's date and seconds value; it's just used to
			// select time, and seconds will always be 00.
			
			// For now, always disable warnings label on load.  When we start using this, we'll immediately want
			// to apply the logic here as to whether to display warning(s).
			// TODO: We'll use this to warn about 4-yearly recurring Feb 29th's - leapyears.
			lblWarnings.Visible = false;
			
			// TODO: For now, we always hide the apply button.  When this is an edit form, not add, don't hide it.
			isAddDialog = true;
			
			if (isAddDialog) {
				this.Text = "Add new reminder";
				btnOk.Text = "Add";
				btnOk.Left = btnCancel.Left;
				btnCancel.Left = btnApply.Left;
				btnApply.Visible = false;
			}
			else {
				this.Text = "Edit reminder";
				btnOk.Text = "OK";
				btnApply.Enabled = false;
			}
		}
		
		private void cbReminderStartDay_CheckedChanged(object sender, EventArgs ea) {
			taint();
			
			if (((CheckBox)sender).Checked) { pickReminderTime.Enabled = false; }
			else { pickReminderTime.Enabled = true; }
		}
		
		private void btnOk_Click(object sender, EventArgs ea) {
			// TODO: This can also be an 'Apply and close' if we're an edit form, not add...
			string errorMsg;
			
			// Try to add the reminder to events in memory (if it's valid)
			DateTime dt = calReminderDate.SelectionStart;
			DateTime tm = pickReminderTime.Value;
			DateTime localWhen = new DateTime(
				dt.Year,
				dt.Month,
				dt.Day,
				tm.Hour,
				tm.Minute,
				0,
				DateTimeKind.Local
			);
			
			try {
				Program.Events.Add(
					RUUtilities.GetNextAvailableEventId(Program.Events),
					new Reminder {
						When = localWhen.ToUniversalTime(),
						Title = tbReminderTitle.Text,
						Body = tbReminderBody.Text,
						StartOfDay = (cbReminderStartDay.Checked ? true : false),
						Acknowledged = false
					}
				);
				Program.InstanceFrmMain.RefreshCalendarReminders();
			}
			catch (EventValueInvalidException ex) {
				Program.Utils.ShowError(ex.Message);
				return;
			}
			
			// Important notes on Daylight Savings Time:
			// What we just did above was use .NET to convert a Local datetime (localWhen) to a universal (UTC)
			// datetime.  Most of the time, this is a straightforward procedure; just take the local datetime,
			// add/subtract the offset with UTC, and you have the UTC time.  But, there are 2 times in the year,
			// in areas of the world that observe Daylight Savings Time, where there is a serious ambiguity in
			// this process: the conversion of Local datetimes to UTC datetimes for the hour when the clocks are
			// going forward, or going back.  Here's the behaviour of .NET's DateTime functionality for the two
			// cases...
			// 
			// For going forward, you have an hour of the year which 'doesn't exist'.  When the clocks go forward
			// on some date, the Local time will jump from 00:59:59 to 02:00:00.  That means there's an hour that,
			// in Local time, doesn't exist.  So what happens if you try to set a DateTime to some point within
			// that 'phantom' hour?  Our GUI allows the user to do that.  Well, DateTime does allow it.  What
			// will happen is that you will get a DateTime object with the date/time set to, say, 01:30:00, and
			// its Kind will be Local.  When you try to convert it to UTC, the time will stay the same and NOT
			// go back an hour.  This means that DateTime considers ANY time BEFORE 02:00:00 to be BEFORE the
			// clocks go forward, and will not pull the time an hour back upon conversion.  So although that
			// 01:30:00 doesn't actually exist, you can create a DateTime object set to that time, and it will be
			// treated as (in the UK) a GMT, rather than a BST, offset; the clocks "haven't gone forward yet".
			// 
			// The other case, of course, is when the clocks are going back.  In this instance, in Local time, the
			// same hour is 'played out twice'.  On some date, you'll go through 01:00:00 to 01:59:59, and then
			// the clocks go back so you'll jump back to 01:00:00 and play that hour out again, this time without
			// the BST offset.  Now, if a Local time is set to a time within that hour, the problem here is that
			// that setting is ambiguous.  It could refer to the first 'play through' of the hour, or the second
			// 'play through'.  So what happens if we create a DateTime object with its time set to a time
			// within that hour?  Well, it assumes that we're talking about the SECOND 'play through' of that hour;
			// so in the UK, for example, you would get the GMT version of the 01:30:00 on that date, which would
			// equate to 01:30:00 UTC on that date.  The BST offset would not apply.  This is just the way that
			// DateTime works, no particular rhyme or reason for it!
			// 
			// In conclusion, I don't think we need to worry about this.  We can allow the user to set the
			// date/time to whatever, and our mechanism will just check whether that reminder is earlier than .Now
			// when both are converted to UTC.  The worst problem that may happen is if someone sets a reminder
			// to 01:59:00 in the 'phantom hour' that shouldn't exist as the clocks are going forward.  Then, it
			// will take until 02:59:00 Local time for that reminder to pop up, so an extra 59 minutes.
			// No big deal.
			
			if (!RUUtilities.UpdateEvents(out errorMsg)) {
				Program.Utils.ShowError(errorMsg);
				return;
			}
			
			// Successfully added reminder
			RUUtilities.CheckForNewReminders(DateTime.Now);
			Program.Utils.ShowInfo("Reminder added.");
			
			tainted = false;
			this.Close();
		}
		
		private void btnCancel_Click(object sender, EventArgs ea) {
			// Don't want to add the reminder anymore?  Just close the form...
			this.Close();
		}
		
		private void btnApply_Click(object sender, EventArgs ea) {
			// This button only appears if we're editing, not adding.  We want to apply the changes made in the
			// dialog.
		}
		
		private void calReminderDate_DateChanged(object sender, DateRangeEventArgs ea) {
			taint();
		}
		
		private void pickReminderTime_ValueChanged(object sender, EventArgs ea) {
			taint();
		}
		
		private void tbReminderTitle_TextChanged(object sender, EventArgs ea) {
			taint();
		}
		
		private void tbReminderBody_TextChanged(object sender, EventArgs ea) {
			taint();
		}
	}
}
