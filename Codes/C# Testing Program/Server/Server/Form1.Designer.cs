namespace Server
{
    partial class Form1
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
            this.tcpradio = new System.Windows.Forms.RadioButton();
            this.udpradio = new System.Windows.Forms.RadioButton();
            this.datapts = new System.Windows.Forms.NumericUpDown();
            this.startbutton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.datapts)).BeginInit();
            this.SuspendLayout();
            // 
            // tcpradio
            // 
            this.tcpradio.AutoSize = true;
            this.tcpradio.Location = new System.Drawing.Point(54, 45);
            this.tcpradio.Name = "tcpradio";
            this.tcpradio.Size = new System.Drawing.Size(46, 17);
            this.tcpradio.TabIndex = 0;
            this.tcpradio.TabStop = true;
            this.tcpradio.Text = "TCP";
            this.tcpradio.UseVisualStyleBackColor = true;
            // 
            // udpradio
            // 
            this.udpradio.AutoSize = true;
            this.udpradio.Location = new System.Drawing.Point(163, 45);
            this.udpradio.Name = "udpradio";
            this.udpradio.Size = new System.Drawing.Size(48, 17);
            this.udpradio.TabIndex = 1;
            this.udpradio.TabStop = true;
            this.udpradio.Text = "UDP";
            this.udpradio.UseVisualStyleBackColor = true;
            // 
            // datapts
            // 
            this.datapts.Location = new System.Drawing.Point(54, 104);
            this.datapts.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.datapts.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.datapts.Name = "datapts";
            this.datapts.Size = new System.Drawing.Size(157, 20);
            this.datapts.TabIndex = 2;
            this.datapts.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // startbutton
            // 
            this.startbutton.Location = new System.Drawing.Point(86, 161);
            this.startbutton.Name = "startbutton";
            this.startbutton.Size = new System.Drawing.Size(99, 30);
            this.startbutton.TabIndex = 3;
            this.startbutton.Text = "start";
            this.startbutton.UseVisualStyleBackColor = true;
            this.startbutton.Click += new System.EventHandler(this.startbutton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.startbutton);
            this.Controls.Add(this.datapts);
            this.Controls.Add(this.udpradio);
            this.Controls.Add(this.tcpradio);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.datapts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton tcpradio;
        private System.Windows.Forms.RadioButton udpradio;
        private System.Windows.Forms.NumericUpDown datapts;
        private System.Windows.Forms.Button startbutton;
    }
}

