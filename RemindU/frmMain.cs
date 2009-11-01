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

namespace RemindU
{
	public partial class frmMain : Form {
		#region Constructors
		
		public frmMain() {
			InitializeComponent();
		}
		
		#endregion
		
		private void frmMain_Load(object sender, EventArgs ea) {
			// All of this logic has been moved to Program.
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
	}
}
