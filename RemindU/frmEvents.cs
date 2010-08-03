using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RemindU
{
	public partial class frmEvents : Form {
		#region Private vars
		
		private bool refreshingList = false;
		private bool formIsClosing = false;
		
		#endregion
		
		#region Constructors
		
		public frmEvents() {
			InitializeComponent();
		}
		
		#endregion
		
		private void btnClose_Click(object sender, EventArgs ea) {
			// Close the form...
			this.Close();
		}
		
		private void frmEvents_FormClosing(object sender, FormClosingEventArgs ea) {
			if (!Program.ReallyCloseFrmEvents) {
				// Only hide this form; we don't allow multiple instances of it
				this.Hide();
				ea.Cancel = true;
				
				Program.EventsOutstanding.Clear();
				RUUtilities.CheckForNewReminders(DateTime.Now);
			}
		}
		
		private void lstOutstanding_SelectedIndexChanged(object sender, EventArgs ea) {
			if (!refreshingList) { refreshSelectedReminderInfo(); }
		}
		
		private void btnAckOutstanding_Click(object sender, EventArgs ea) {
			string errorMsg;
			
			ReminderListItem li = (ReminderListItem)lstOutstanding.SelectedItem;
			Program.Events[li.EventId].Acknowledged = true;
			if (!RUUtilities.UpdateEvents(out errorMsg)) {
				Program.Utils.ShowError(errorMsg);
				return;
			}
			int i = 0;
			foreach (UInt32 evId in Program.EventsOutstanding) {
				if (evId == li.EventId) { break; }
				i++;
			}
			Program.EventsOutstanding.RemoveAt(i);
			
			RefreshEventsOutstanding();
			
			Program.Utils.ShowInfo("Reminder acknowledged.");
		}
		
		private int findPlaceForListItem(Reminder ev) {
			// Find which index to insert this item at.  A return value of -1 means just to .Add it to the end.
			int insertIndex = -1;
			
			DateTime eventTime;
			if (ev.StartOfDay) {
				eventTime = new DateTime(ev.When.Year, ev.When.Month, ev.When.Day, 0, 0, 0);
			}
			else {
				eventTime = new DateTime(ev.When.Year, ev.When.Month, ev.When.Day, ev.When.Hour, ev.When.Minute, 0);
			}
			foreach (ReminderListItem li in lstOutstanding.Items) {
				Reminder liEv = Program.Events[li.EventId];
				DateTime liDt = liEv.When;
				insertIndex++;
				
				DateTime compareTime;
				if (liEv.StartOfDay) {
					compareTime = new DateTime(liDt.Year, liDt.Month, liDt.Day, 0, 0, 0);
				}
				else {
					compareTime = new DateTime(liDt.Year, liDt.Month, liDt.Day, liDt.Hour, liDt.Minute, 0);
				}
				if (eventTime.CompareTo(compareTime) < 0) { return insertIndex; }
			}
			
			// This item's date is higher than all others, or there are no list items yet?  Just add it.
			return -1;
		}
		
		private void refreshSelectedReminderInfo() {
			Reminder ev = ((Reminder)Program.Events[((ReminderListItem)lstOutstanding.SelectedItem).EventId]);
			tbReminderTitle.Text = ev.Title;
			tbReminderBody.Text = Program.Utils.ConvertToWindowsNewlines(ev.Body);
		}
		
		#region Public methods
		
		public void RefreshEventsOutstanding() {
			refreshingList = true;
			try {
				// lstOutstanding.SelectedIndex will be -1 if nothing is selected.
				bool selectFirstEvent = (lstOutstanding.SelectedIndex < 0);
				
				// Remove any events that are now gone, acknowledged, or out of date
				bool didListChange;
				do {
					didListChange = false;
					int itemCount = 0;
					foreach (ReminderListItem li in lstOutstanding.Items) {
						if (!Program.EventsOutstanding.Contains(li.EventId)) {
							// Remove this reminder
							int oldItemSelectedIndex = -1;
							if (lstOutstanding.SelectedItem == li) {
								oldItemSelectedIndex = lstOutstanding.SelectedIndex;
							}
							lstOutstanding.Items.RemoveAt(itemCount);
							if (oldItemSelectedIndex >= 0) {
								if (lstOutstanding.Items.Count > oldItemSelectedIndex) {
									lstOutstanding.SelectedIndex = oldItemSelectedIndex;
								}
								else {
									lstOutstanding.SelectedIndex = oldItemSelectedIndex - 1;
								}
							}
							didListChange = true;
							break;
						}
						
						itemCount++;
					}
				} while (didListChange);
				
				// Add any events that aren't in our listbox, but are outstanding
				foreach (UInt32 evId in Program.EventsOutstanding) {
					bool found = false;
					foreach (ReminderListItem li in lstOutstanding.Items) {
						if (li.EventId == evId) { found = true; break; }
					}
					if (!found) {
						// Add this reminder
						ReminderListItem newItem = new ReminderListItem {
							EventId = evId,
							Description = RUUtilities.GetReminderSummaryString(Program.Events[evId])
						};
						int placeForItem = findPlaceForListItem(Program.Events[evId]);
						if (placeForItem < 0) { lstOutstanding.Items.Add(newItem); }
						else { lstOutstanding.Items.Insert(placeForItem, newItem); }
					}
				}
				
				if (lstOutstanding.Items.Count > 0) {
					// Do we need to select the first event?
					if (selectFirstEvent) {
						lstOutstanding.SelectedIndex = 0;
						selectFirstEvent = false;
					}
					
					// Now that the list's up-to-date, update the reminder info for the selected reminder
					refreshSelectedReminderInfo();
				}
				else if (this.Visible == true) {
					// Close this reminders form if there are no reminders left and we need to
					if (!formIsClosing) {
						formIsClosing = true;
						this.Close();
						formIsClosing = false;
					}
					return;
				}
			}
			finally {
				refreshingList = false;
			}
		}
		
		#endregion
	}
}
