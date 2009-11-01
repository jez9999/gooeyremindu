namespace RemindU
{
	partial class frmAddEdit
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
			this.calReminderDate = new System.Windows.Forms.MonthCalendar();
			this.pickReminderTime = new System.Windows.Forms.DateTimePicker();
			this.gbReminderDateTime = new System.Windows.Forms.GroupBox();
			this.cbReminderStartDay = new System.Windows.Forms.CheckBox();
			this.gbReminderInfo = new System.Windows.Forms.GroupBox();
			this.tbReminderBody = new System.Windows.Forms.TextBox();
			this.lblReminderBody = new System.Windows.Forms.Label();
			this.lblReminderTitle = new System.Windows.Forms.Label();
			this.tbReminderTitle = new System.Windows.Forms.TextBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblWarnings = new System.Windows.Forms.Label();
			this.btnApply = new System.Windows.Forms.Button();
			this.gbReminderDateTime.SuspendLayout();
			this.gbReminderInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// calReminderDate
			// 
			this.calReminderDate.AnnuallyBoldedDates = new System.DateTime[] {
        new System.DateTime(2008, 2, 29, 0, 0, 0, 0)};
			this.calReminderDate.Location = new System.Drawing.Point(12, 25);
			this.calReminderDate.MaxSelectionCount = 1;
			this.calReminderDate.Name = "calReminderDate";
			this.calReminderDate.ShowToday = false;
			this.calReminderDate.TabIndex = 0;
			this.calReminderDate.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.calReminderDate_DateChanged);
			// 
			// pickReminderTime
			// 
			this.pickReminderTime.CustomFormat = "HH:mm";
			this.pickReminderTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.pickReminderTime.Location = new System.Drawing.Point(12, 192);
			this.pickReminderTime.Name = "pickReminderTime";
			this.pickReminderTime.ShowUpDown = true;
			this.pickReminderTime.Size = new System.Drawing.Size(178, 20);
			this.pickReminderTime.TabIndex = 1;
			this.pickReminderTime.ValueChanged += new System.EventHandler(this.pickReminderTime_ValueChanged);
			// 
			// gbReminderDateTime
			// 
			this.gbReminderDateTime.Controls.Add(this.cbReminderStartDay);
			this.gbReminderDateTime.Controls.Add(this.calReminderDate);
			this.gbReminderDateTime.Controls.Add(this.pickReminderTime);
			this.gbReminderDateTime.Location = new System.Drawing.Point(12, 12);
			this.gbReminderDateTime.Name = "gbReminderDateTime";
			this.gbReminderDateTime.Size = new System.Drawing.Size(202, 248);
			this.gbReminderDateTime.TabIndex = 0;
			this.gbReminderDateTime.TabStop = false;
			this.gbReminderDateTime.Text = "Reminder date/time";
			// 
			// cbReminderStartDay
			// 
			this.cbReminderStartDay.AutoSize = true;
			this.cbReminderStartDay.Location = new System.Drawing.Point(12, 218);
			this.cbReminderStartDay.Name = "cbReminderStartDay";
			this.cbReminderStartDay.Size = new System.Drawing.Size(129, 17);
			this.cbReminderStartDay.TabIndex = 2;
			this.cbReminderStartDay.Text = "Remind at start of day";
			this.cbReminderStartDay.UseVisualStyleBackColor = true;
			this.cbReminderStartDay.CheckedChanged += new System.EventHandler(this.cbReminderStartDay_CheckedChanged);
			// 
			// gbReminderInfo
			// 
			this.gbReminderInfo.Controls.Add(this.tbReminderBody);
			this.gbReminderInfo.Controls.Add(this.lblReminderBody);
			this.gbReminderInfo.Controls.Add(this.lblReminderTitle);
			this.gbReminderInfo.Controls.Add(this.tbReminderTitle);
			this.gbReminderInfo.Location = new System.Drawing.Point(232, 12);
			this.gbReminderInfo.Name = "gbReminderInfo";
			this.gbReminderInfo.Size = new System.Drawing.Size(449, 248);
			this.gbReminderInfo.TabIndex = 0;
			this.gbReminderInfo.TabStop = false;
			this.gbReminderInfo.Text = "Reminder information";
			// 
			// tbReminderBody
			// 
			this.tbReminderBody.Location = new System.Drawing.Point(9, 89);
			this.tbReminderBody.Multiline = true;
			this.tbReminderBody.Name = "tbReminderBody";
			this.tbReminderBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbReminderBody.Size = new System.Drawing.Size(434, 146);
			this.tbReminderBody.TabIndex = 8;
			this.tbReminderBody.TextChanged += new System.EventHandler(this.tbReminderBody_TextChanged);
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
			this.tbReminderTitle.Location = new System.Drawing.Point(9, 41);
			this.tbReminderTitle.Name = "tbReminderTitle";
			this.tbReminderTitle.Size = new System.Drawing.Size(434, 20);
			this.tbReminderTitle.TabIndex = 6;
			this.tbReminderTitle.TextChanged += new System.EventHandler(this.tbReminderTitle_TextChanged);
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(444, 266);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 9;
			this.btnOk.Text = "Add-or-Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(525, 266);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 10;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lblWarnings
			// 
			this.lblWarnings.AutoSize = true;
			this.lblWarnings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblWarnings.ForeColor = System.Drawing.Color.Red;
			this.lblWarnings.Location = new System.Drawing.Point(12, 271);
			this.lblWarnings.Name = "lblWarnings";
			this.lblWarnings.Size = new System.Drawing.Size(198, 13);
			this.lblWarnings.TabIndex = 0;
			this.lblWarnings.Text = "Warnings will be displayed here...";
			// 
			// btnApply
			// 
			this.btnApply.Location = new System.Drawing.Point(606, 266);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(75, 23);
			this.btnApply.TabIndex = 11;
			this.btnApply.Text = "Apply";
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// frmAddEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(693, 299);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.lblWarnings);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.gbReminderInfo);
			this.Controls.Add(this.gbReminderDateTime);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAddEdit";
			this.Text = "Add new reminder -or- Edit reminder";
			this.Load += new System.EventHandler(this.frmAddEdit_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAddEdit_FormClosing);
			this.gbReminderDateTime.ResumeLayout(false);
			this.gbReminderDateTime.PerformLayout();
			this.gbReminderInfo.ResumeLayout(false);
			this.gbReminderInfo.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MonthCalendar calReminderDate;
		private System.Windows.Forms.DateTimePicker pickReminderTime;
		private System.Windows.Forms.GroupBox gbReminderDateTime;
		private System.Windows.Forms.CheckBox cbReminderStartDay;
		private System.Windows.Forms.GroupBox gbReminderInfo;
		private System.Windows.Forms.TextBox tbReminderBody;
		private System.Windows.Forms.Label lblReminderBody;
		private System.Windows.Forms.Label lblReminderTitle;
		private System.Windows.Forms.TextBox tbReminderTitle;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblWarnings;
		private System.Windows.Forms.Button btnApply;
	}
}