namespace Beyond_Beyaan
{
	partial class Configuration
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Configuration));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this._datasetComboBox = new System.Windows.Forms.ComboBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this._resolutionComboBox = new System.Windows.Forms.ComboBox();
			this._fullCB = new System.Windows.Forms.CheckBox();
			this._launchButton = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this._datasetComboBox);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(13, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(259, 51);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Data";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(79, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Dataset to use:";
			// 
			// _datasetComboBox
			// 
			this._datasetComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._datasetComboBox.FormattingEnabled = true;
			this._datasetComboBox.Location = new System.Drawing.Point(93, 17);
			this._datasetComboBox.Name = "_datasetComboBox";
			this._datasetComboBox.Size = new System.Drawing.Size(160, 21);
			this._datasetComboBox.TabIndex = 1;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this._fullCB);
			this.groupBox2.Controls.Add(this._resolutionComboBox);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Location = new System.Drawing.Point(13, 71);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(259, 72);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Screen";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(10, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Resolution:";
			// 
			// _resolutionComboBox
			// 
			this._resolutionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._resolutionComboBox.FormattingEnabled = true;
			this._resolutionComboBox.Location = new System.Drawing.Point(93, 17);
			this._resolutionComboBox.Name = "_resolutionComboBox";
			this._resolutionComboBox.Size = new System.Drawing.Size(160, 21);
			this._resolutionComboBox.TabIndex = 1;
			// 
			// _fullCB
			// 
			this._fullCB.AutoSize = true;
			this._fullCB.Location = new System.Drawing.Point(93, 45);
			this._fullCB.Name = "_fullCB";
			this._fullCB.Size = new System.Drawing.Size(74, 17);
			this._fullCB.TabIndex = 2;
			this._fullCB.Text = "Fullscreen";
			this._fullCB.UseVisualStyleBackColor = true;
			// 
			// _launchButton
			// 
			this._launchButton.Location = new System.Drawing.Point(197, 149);
			this._launchButton.Name = "_launchButton";
			this._launchButton.Size = new System.Drawing.Size(75, 23);
			this._launchButton.TabIndex = 2;
			this._launchButton.Text = "Launch";
			this._launchButton.UseVisualStyleBackColor = true;
			this._launchButton.Click += new System.EventHandler(this._launchButton_Click);
			// 
			// Configuration
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 185);
			this.Controls.Add(this._launchButton);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Configuration";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Beyond Beyaan Launcher";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox _datasetComboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox _fullCB;
		private System.Windows.Forms.ComboBox _resolutionComboBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button _launchButton;

	}
}