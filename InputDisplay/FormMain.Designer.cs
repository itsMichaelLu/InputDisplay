namespace InputDisplay
{
    partial class FormMain
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
            this.buttonReloadlist = new System.Windows.Forms.Button();
            this.comboBoxInputList = new System.Windows.Forms.ComboBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.checkBoxShowDisplay = new System.Windows.Forms.CheckBox();
            this.labelConstDevice = new System.Windows.Forms.Label();
            this.labelConnectedDevice = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonReloadlist
            // 
            this.buttonReloadlist.Location = new System.Drawing.Point(12, 12);
            this.buttonReloadlist.Name = "buttonReloadlist";
            this.buttonReloadlist.Size = new System.Drawing.Size(260, 23);
            this.buttonReloadlist.TabIndex = 0;
            this.buttonReloadlist.Text = "Refresh List";
            this.buttonReloadlist.UseVisualStyleBackColor = true;
            this.buttonReloadlist.Click += new System.EventHandler(this.buttonReloadlist_Click);
            // 
            // comboBoxInputList
            // 
            this.comboBoxInputList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInputList.FormattingEnabled = true;
            this.comboBoxInputList.Location = new System.Drawing.Point(12, 41);
            this.comboBoxInputList.Name = "comboBoxInputList";
            this.comboBoxInputList.Size = new System.Drawing.Size(222, 21);
            this.comboBoxInputList.TabIndex = 1;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(240, 39);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(32, 23);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "Get";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // checkBoxShowDisplay
            // 
            this.checkBoxShowDisplay.AutoSize = true;
            this.checkBoxShowDisplay.Location = new System.Drawing.Point(12, 232);
            this.checkBoxShowDisplay.Name = "checkBoxShowDisplay";
            this.checkBoxShowDisplay.Size = new System.Drawing.Size(92, 17);
            this.checkBoxShowDisplay.TabIndex = 3;
            this.checkBoxShowDisplay.Text = "Show Overlay";
            this.checkBoxShowDisplay.UseVisualStyleBackColor = true;
            this.checkBoxShowDisplay.CheckedChanged += new System.EventHandler(this.checkBoxShowDisplay_CheckedChanged);
            // 
            // labelConstDevice
            // 
            this.labelConstDevice.AutoSize = true;
            this.labelConstDevice.Location = new System.Drawing.Point(13, 69);
            this.labelConstDevice.Name = "labelConstDevice";
            this.labelConstDevice.Size = new System.Drawing.Size(44, 13);
            this.labelConstDevice.TabIndex = 4;
            this.labelConstDevice.Text = "Device:";
            // 
            // labelConnectedDevice
            // 
            this.labelConnectedDevice.AutoSize = true;
            this.labelConnectedDevice.Location = new System.Drawing.Point(63, 69);
            this.labelConnectedDevice.Name = "labelConnectedDevice";
            this.labelConnectedDevice.Size = new System.Drawing.Size(33, 13);
            this.labelConnectedDevice.TabIndex = 5;
            this.labelConnectedDevice.Text = "None";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.labelConnectedDevice);
            this.Controls.Add(this.labelConstDevice);
            this.Controls.Add(this.checkBoxShowDisplay);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.comboBoxInputList);
            this.Controls.Add(this.buttonReloadlist);
            this.Name = "FormMain";
            this.Text = "Input Display Overlay";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonReloadlist;
        private System.Windows.Forms.ComboBox comboBoxInputList;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.CheckBox checkBoxShowDisplay;
        private System.Windows.Forms.Label labelConstDevice;
        private System.Windows.Forms.Label labelConnectedDevice;
    }
}

