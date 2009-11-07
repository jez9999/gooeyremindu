namespace RemindU
{
	partial class frmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.button1 = new System.Windows.Forms.Button();
			this.btnTest = new System.Windows.Forms.Button();
			this.gbReminderInfo = new System.Windows.Forms.GroupBox();
			this.tbReminderBody = new System.Windows.Forms.TextBox();
			this.lblReminderBody = new System.Windows.Forms.Label();
			this.lblReminderTitle = new System.Windows.Forms.Label();
			this.tbReminderTitle = new System.Windows.Forms.TextBox();
			this.gbReminderDateTime = new System.Windows.Forms.GroupBox();
			this.calReminderDates = new System.Windows.Forms.MonthCalendar();
			this.lstReminders = new System.Windows.Forms.ListBox();
			this.btnEdit = new System.Windows.Forms.Button();
			this.gbReminderInfo.SuspendLayout();
			this.gbReminderDateTime.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(468, 209);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 39);
			this.button1.TabIndex = 0;
			this.button1.Text = "&About Remind U";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// btnTest
			// 
			this.btnTest.Location = new System.Drawing.Point(468, 429);
			this.btnTest.Name = "btnTest";
			this.btnTest.Size = new System.Drawing.Size(75, 23);
			this.btnTest.TabIndex = 1;
			this.btnTest.Text = "Test";
			this.btnTest.UseVisualStyleBackColor = true;
			this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
			// 
			// gbReminderInfo
			// 
			this.gbReminderInfo.Controls.Add(this.tbReminderBody);
			this.gbReminderInfo.Controls.Add(this.lblReminderBody);
			this.gbReminderInfo.Controls.Add(this.lblReminderTitle);
			this.gbReminderInfo.Controls.Add(this.tbReminderTitle);
			this.gbReminderInfo.Location = new System.Drawing.Point(12, 204);
			this.gbReminderInfo.Name = "gbReminderInfo";
			this.gbReminderInfo.Size = new System.Drawing.Size(449, 248);
			this.gbReminderInfo.TabIndex = 4;
			this.gbReminderInfo.TabStop = false;
			this.gbReminderInfo.Text = "Selected reminder information";
			// 
			// tbReminderBody
			// 
			this.tbReminderBody.BackColor = System.Drawing.SystemColors.Window;
			this.tbReminderBody.Location = new System.Drawing.Point(9, 89);
			this.tbReminderBody.Multiline = true;
			this.tbReminderBody.Name = "tbReminderBody";
			this.tbReminderBody.ReadOnly = true;
			this.tbReminderBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbReminderBody.Size = new System.Drawing.Size(434, 146);
			this.tbReminderBody.TabIndex = 8;
			// 
			// lblReminderBody
			// 
			this.lblReminderBody.AutoSize = true;
			this.lblReminderBody.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblReminderBody.Location = new System.Drawing.Point(6, 73);
			this.lblReminderBody.Name = "lblReminderBody";
			this.lblReminderBody.Size = new System.Drawing.Size(39, 13);
			this.lblReminderBody.TabIndex = 7;
			this.lblReminderBody.Text = "Body:";
			// 
			// lblReminderTitle
			// 
			this.lblReminderTitle.AutoSize = true;
			this.lblReminderTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblReminderTitle.Location = new System.Drawing.Point(6, 25);
			this.lblReminderTitle.Name = "lblReminderTitle";
			this.lblReminderTitle.Size = new System.Drawing.Size(36, 13);
			this.lblReminderTitle.TabIndex = 5;
			this.lblReminderTitle.Text = "Title:";
			// 
			// tbReminderTitle
			// 
			this.tbReminderTitle.BackColor = System.Drawing.SystemColors.Window;
			this.tbReminderTitle.Location = new System.Drawing.Point(9, 41);
			this.tbReminderTitle.Name = "tbReminderTitle";
			this.tbReminderTitle.ReadOnly = true;
			this.tbReminderTitle.Size = new System.Drawing.Size(434, 20);
			this.tbReminderTitle.TabIndex = 6;
			// 
			// gbReminderDateTime
			// 
			this.gbReminderDateTime.Controls.Add(this.calReminderDates);
			this.gbReminderDateTime.Controls.Add(this.lstReminders);
			this.gbReminderDateTime.Location = new System.Drawing.Point(12, 12);
			this.gbReminderDateTime.Name = "gbReminderDateTime";
			this.gbReminderDateTime.Size = new System.Drawing.Size(512, 186);
			this.gbReminderDateTime.TabIndex = 5;
			this.gbReminderDateTime.TabStop = false;
			this.gbReminderDateTime.Text = "Browsing reminders";
			// 
			// calReminderDates
			// 
			this.calReminderDates.AnnuallyBoldedDates = new System.DateTime[] {
        new System.DateTime(2008, 2, 29, 0, 0, 0, 0)};
			this.calReminderDates.Location = new System.Drawing.Point(12, 19);
			this.calReminderDates.MaxSelectionCount = 1;
			this.calReminderDates.Name = "calReminderDates";
			this.calReminderDates.ShowToday = false;
			this.calReminderDates.TabIndex = 0;
			this.calReminderDates.MouseDown += new System.Windows.Forms.MouseEventHandler(this.calReminderDates_MouseDown);
			this.calReminderDates.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.calReminderDates_DateChanged);
			// 
			// lstReminders
			// 
			this.lstReminders.FormattingEnabled = true;
			this.lstReminders.HorizontalScrollbar = true;
			this.lstReminders.IntegralHeight = false;
			this.lstReminders.Location = new System.Drawing.Point(205, 19);
			this.lstReminders.Name = "lstReminders";
			this.lstReminders.ScrollAlwaysVisible = true;
			this.lstReminders.Size = new System.Drawing.Size(297, 155);
			this.lstReminders.TabIndex = 0;
			this.lstReminders.SelectedIndexChanged += new System.EventHandler(this.lstReminders_SelectedIndexChanged);
			// 
			// btnEdit
			// 
			this.btnEdit.Enabled = false;
			this.btnEdit.Location = new System.Drawing.Point(467, 400);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(75, 23);
			this.btnEdit.TabIndex = 7;
			this.btnEdit.Text = "&Edit";
			this.btnEdit.UseVisualStyleBackColor = true;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(550, 464);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.gbReminderDateTime);
			this.Controls.Add(this.gbReminderInfo);
			this.Controls.Add(this.btnTest);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Remind U";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.gbReminderInfo.ResumeLayout(false);
			this.gbReminderInfo.PerformLayout();
			this.gbReminderDateTime.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.GroupBox gbReminderInfo;
		private System.Windows.Forms.TextBox tbReminderBody;
		private System.Windows.Forms.Label lblReminderBody;
		private System.Windows.Forms.Label lblReminderTitle;
		private System.Windows.Forms.TextBox tbReminderTitle;
		private System.Windows.Forms.GroupBox gbReminderDateTime;
		private System.Windows.Forms.MonthCalendar calReminderDates;
		private System.Windows.Forms.ListBox lstReminders;
		private System.Windows.Forms.Button btnEdit;
	}
}

