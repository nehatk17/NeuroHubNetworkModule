namespace OneClient_NetworkTiming_TCP
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.startbutton = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.comPortIn = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.radiotcp = new System.Windows.Forms.RadioButton();
            this.radioudp = new System.Windows.Forms.RadioButton();
            this.checkserial = new System.Windows.Forms.CheckBox();
            this.baudratebox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 122);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "COM in";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 188);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number of Data Points";
            // 
            // startbutton
            // 
            this.startbutton.Location = new System.Drawing.Point(229, 249);
            this.startbutton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.startbutton.Name = "startbutton";
            this.startbutton.Size = new System.Drawing.Size(98, 40);
            this.startbutton.TabIndex = 2;
            this.startbutton.Text = "Start";
            this.startbutton.UseVisualStyleBackColor = true;
            this.startbutton.Click += new System.EventHandler(this.startbutton_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(296, 185);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(231, 26);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // comPortIn
            // 
            this.comPortIn.FormattingEnabled = true;
            this.comPortIn.Location = new System.Drawing.Point(296, 117);
            this.comPortIn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comPortIn.Name = "comPortIn";
            this.comPortIn.Size = new System.Drawing.Size(226, 28);
            this.comPortIn.TabIndex = 5;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(28, 322);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(508, 206);
            this.textBox1.TabIndex = 6;
            // 
            // radiotcp
            // 
            this.radiotcp.AutoSize = true;
            this.radiotcp.Checked = true;
            this.radiotcp.Location = new System.Drawing.Point(58, 43);
            this.radiotcp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radiotcp.Name = "radiotcp";
            this.radiotcp.Size = new System.Drawing.Size(64, 24);
            this.radiotcp.TabIndex = 7;
            this.radiotcp.TabStop = true;
            this.radiotcp.Text = "TCP";
            this.radiotcp.UseVisualStyleBackColor = true;
            // 
            // radioudp
            // 
            this.radioudp.AutoSize = true;
            this.radioudp.Location = new System.Drawing.Point(154, 42);
            this.radioudp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioudp.Name = "radioudp";
            this.radioudp.Size = new System.Drawing.Size(68, 24);
            this.radioudp.TabIndex = 8;
            this.radioudp.TabStop = true;
            this.radioudp.Text = "UDP";
            this.radioudp.UseVisualStyleBackColor = true;
            // 
            // checkserial
            // 
            this.checkserial.AutoSize = true;
            this.checkserial.Location = new System.Drawing.Point(250, 45);
            this.checkserial.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkserial.Name = "checkserial";
            this.checkserial.Size = new System.Drawing.Size(108, 24);
            this.checkserial.TabIndex = 9;
            this.checkserial.Text = "Serial Port";
            this.checkserial.UseVisualStyleBackColor = true;
            this.checkserial.CheckedChanged += new System.EventHandler(this.checkserial_CheckedChanged);
            // 
            // baudratebox
            // 
            this.baudratebox.FormattingEnabled = true;
            this.baudratebox.Location = new System.Drawing.Point(374, 42);
            this.baudratebox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baudratebox.Name = "baudratebox";
            this.baudratebox.Size = new System.Drawing.Size(151, 28);
            this.baudratebox.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 555);
            this.Controls.Add(this.baudratebox);
            this.Controls.Add(this.checkserial);
            this.Controls.Add(this.radioudp);
            this.Controls.Add(this.radiotcp);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comPortIn);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.startbutton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button startbutton;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ComboBox comPortIn;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radiotcp;
        private System.Windows.Forms.RadioButton radioudp;
        private System.Windows.Forms.CheckBox checkserial;
        private System.Windows.Forms.ComboBox baudratebox;
    }
}

