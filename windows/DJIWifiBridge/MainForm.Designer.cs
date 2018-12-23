namespace DJIWifiBridge
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.JoystickCombo = new System.Windows.Forms.ComboBox();
            this.wifiConnect = new System.Windows.Forms.Button();
            this.takeoff = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.IP = new System.Windows.Forms.TextBox();
            this.labelThr = new System.Windows.Forms.Label();
            this.labelJoy = new System.Windows.Forms.Label();
            this.ThrottleCombo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // JoystickCombo
            // 
            this.JoystickCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.JoystickCombo.FormattingEnabled = true;
            this.JoystickCombo.Location = new System.Drawing.Point(70, 252);
            this.JoystickCombo.Margin = new System.Windows.Forms.Padding(2);
            this.JoystickCombo.Name = "JoystickCombo";
            this.JoystickCombo.Size = new System.Drawing.Size(120, 20);
            this.JoystickCombo.TabIndex = 0;
            this.JoystickCombo.SelectedIndexChanged += new System.EventHandler(this.JoystickCombo_SelectedIndexChanged);
            // 
            // wifiConnect
            // 
            this.wifiConnect.Location = new System.Drawing.Point(422, 325);
            this.wifiConnect.Margin = new System.Windows.Forms.Padding(2);
            this.wifiConnect.Name = "wifiConnect";
            this.wifiConnect.Size = new System.Drawing.Size(38, 20);
            this.wifiConnect.TabIndex = 1;
            this.wifiConnect.Text = "连接";
            this.wifiConnect.UseVisualStyleBackColor = true;
            this.wifiConnect.Click += new System.EventHandler(this.wifiConnect_Click);
            // 
            // takeoff
            // 
            this.takeoff.Location = new System.Drawing.Point(69, 82);
            this.takeoff.Margin = new System.Windows.Forms.Padding(2);
            this.takeoff.Name = "takeoff";
            this.takeoff.Size = new System.Drawing.Size(70, 71);
            this.takeoff.TabIndex = 2;
            this.takeoff.Text = "起飞";
            this.takeoff.UseVisualStyleBackColor = true;
            this.takeoff.Click += new System.EventHandler(this.takeoff_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(312, 82);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 71);
            this.button2.TabIndex = 3;
            this.button2.Text = "降落";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // IP
            // 
            this.IP.Location = new System.Drawing.Point(269, 326);
            this.IP.Margin = new System.Windows.Forms.Padding(2);
            this.IP.Name = "IP";
            this.IP.Size = new System.Drawing.Size(149, 21);
            this.IP.TabIndex = 4;
            // 
            // labelThr
            // 
            this.labelThr.AutoSize = true;
            this.labelThr.Location = new System.Drawing.Point(12, 255);
            this.labelThr.Name = "labelThr";
            this.labelThr.Size = new System.Drawing.Size(53, 12);
            this.labelThr.TabIndex = 5;
            this.labelThr.Text = "Joystick";
            // 
            // labelJoy
            // 
            this.labelJoy.AutoSize = true;
            this.labelJoy.Location = new System.Drawing.Point(12, 299);
            this.labelJoy.Name = "labelJoy";
            this.labelJoy.Size = new System.Drawing.Size(53, 12);
            this.labelJoy.TabIndex = 7;
            this.labelJoy.Text = "Throttle";
            // 
            // ThrottleCombo
            // 
            this.ThrottleCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThrottleCombo.FormattingEnabled = true;
            this.ThrottleCombo.Location = new System.Drawing.Point(70, 296);
            this.ThrottleCombo.Margin = new System.Windows.Forms.Padding(2);
            this.ThrottleCombo.Name = "ThrottleCombo";
            this.ThrottleCombo.Size = new System.Drawing.Size(120, 20);
            this.ThrottleCombo.TabIndex = 6;
            this.ThrottleCombo.SelectedIndexChanged += new System.EventHandler(this.ThrottleCombo_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 382);
            this.Controls.Add(this.labelJoy);
            this.Controls.Add(this.ThrottleCombo);
            this.Controls.Add(this.labelThr);
            this.Controls.Add(this.IP);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.takeoff);
            this.Controls.Add(this.wifiConnect);
            this.Controls.Add(this.JoystickCombo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox JoystickCombo;
        private System.Windows.Forms.Button wifiConnect;
        private System.Windows.Forms.Button takeoff;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox IP;
        private System.Windows.Forms.Label labelThr;
        private System.Windows.Forms.Label labelJoy;
        private System.Windows.Forms.ComboBox ThrottleCombo;
    }
}

