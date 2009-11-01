namespace RemindU
{
	partial class frmEvents
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
			this.gbOutstanding = new System.Windows.Forms.GroupBox();
			this.lblSodNote = new System.Windows.Forms.Label();
			this.btnAckOutstanding = new System.Windows.Forms.Button();
			this.lstOutstanding = new System.Windows.Forms.ListBox();
			this.btnClose = new System.Windows.Forms.Button();
			this.gbReminderInfo = new System.Windows.Forms.GroupBox();
			this.tbReminderBody = new System.Windows.Forms.TextBox();
			this.lblReminderBody = new System.Windows.Forms.Label();
			this.lblReminderTitle = new System.Windows.Forms.Label();
			this.tbReminderTitle = new System.Windows.Forms.TextBox();
			this.gbOutstanding.SuspendLayout();
			this.gbReminderInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbOutstanding
			// 
			this.gbOutstanding.Controls.Add(this.lblSodNote);
			this.gbOutstanding.Controls.Add(this.btnAckOutstanding);
			this.gbOutstanding.Controls.Add(this.lstOutstanding);
			this.gbOutstanding.Location = new System.Drawing.Point(12, 12);
			this.gbOutstanding.Name = "gbOutstanding";
			this.gbOutstanding.Size = new System.Drawing.Size(315, 273);
			this.gbOutstanding.TabIndex = 0;
			this.gbOutstanding.TabStop = false;
			this.gbOutstanding.Text = "Reminders outstanding";
			// 
			// lblSodNote
			// 
			this.lblSodNote.AutoSize = true;
			this.lblSodNote.Location = new System.Drawing.Point(6, 254);
			this.lblSodNote.Name = "lblSodNote";
			this.lblSodNote.Size = new System.Drawing.Size(223, 13);
			this.lblSodNote.TabIndex = 2;
			this.lblSodNote.Text = "* A reminder time of \'SoD\' means \'Start of Day\'";
			// 
			// btnAckOutstanding
			// 
			this.btnAckOutstanding.Location = new System.Drawing.Point(12, 148);
			this.btnAckOutstanding.Name = "btnAckOutstanding";
			this.btnAckOutstanding.Size = new System.Drawing.Size(297, 23);
			this.btnAckOutstanding.TabIndex = 1;
			this.btnAckOutstanding.Text = "Acknowledge";
			this.btnAckOutstanding.UseVisualStyleBackColor = true;
			this.btnAckOutstanding.Click += new System.EventHandler(this.btnAckOutstanding_Click);
			// 
			// lstOutstanding
			// 
			this.lstOutstanding.FormattingEnabled = true;
			this.lstOutstanding.HorizontalScrollbar = true;
			this.lstOutstanding.Location = new System.Drawing.Point(12, 20);
			this.lstOutstanding.Name = "lstOutstanding";
			this.lstOutstanding.ScrollAlwaysVisible = true;
			this.lstOutstanding.Size = new System.Drawing.Size(297, 121);
			this.lstOutstanding.TabIndex = 0;
			this.lstOutstanding.SelectedIndexChanged += new System.EventHandler(this.lstOutstanding_SelectedIndexChanged);
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(707, 266);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// gbReminderInfo
			// 
			this.gbReminderInfo.Controls.Add(this.tbReminderBody);
			this.gbReminderInfo.Controls.Add(this.lblReminderBody);
			this.gbReminderInfo.Controls.Add(this.lblReminderTitle);
			this.gbReminderInfo.Controls.Add(this.tbReminderTitle);
			this.gbReminderInfo.Location = new System.Drawing.Point(333, 12);
			this.gbReminderInfo.Name = "gbReminderInfo";
			this.gbReminderInfo.Size = new System.Drawing.Size(449, 248);
			this.gbReminderInfo.TabIndex = 3;
			this.gbReminderInfo.TabStop = false;
			this.gbReminderInfo.Text = "Reminder information";
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
			// frmEvents
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(794, 297);
			this.Controls.Add(this.gbReminderInfo);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.gbOutstanding);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmEvents";
			this.Text = "Viewing pending reminders";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEvents_FormClosing);
			this.gbOutstanding.ResumeLayout(false);
			this.gbOutstanding.PerformLayout();
			this.gbReminderInfo.ResumeLayout(false);
			this.gbReminderInfo.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbOutstanding;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.ListBox lstOutstanding;
		private System.Windows.Forms.GroupBox gbReminderInfo;
		private System.Windows.Forms.TextBox tbReminderBody;
		private System.Windows.Forms.Label lblReminderBody;
		private System.Windows.Forms.Label lblReminderTitle;
		private System.Windows.Forms.TextBox tbReminderTitle;
		private System.Windows.Forms.Button btnAckOutstanding;
		private System.Windows.Forms.Label lblSodNote;

	}
}