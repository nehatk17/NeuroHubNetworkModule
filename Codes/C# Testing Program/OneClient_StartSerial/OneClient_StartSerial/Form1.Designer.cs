namespace OneClient_StartSerial
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
            this.radiotcp = new System.Windows.Forms.RadioButton();
            this.radioudp = new System.Windows.Forms.RadioButton();
            this.comPortOut = new System.Windows.Forms.ComboBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.startbutton = new System.Windows.Forms.Button();
            this.radioserial = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.comPortIn = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.baudratebox = new System.Windows.Forms.ComboBox();
            this.checkser1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // radiotcp
            // 
            this.radiotcp.AutoSize = true;
            this.radiotcp.Checked = true;
            this.radiotcp.Location = new System.Drawing.Point(26, 42);
            this.radiotcp.Name = "radiotcp";
            this.radiotcp.Size = new System.Drawing.Size(46, 17);
            this.radiotcp.TabIndex = 0;
            this.radiotcp.TabStop = true;
            this.radiotcp.Text = "TCP";
            this.radiotcp.UseVisualStyleBackColor = true;
            // 
            // radioudp
            // 
            this.radioudp.AutoSize = true;
            this.radioudp.Location = new System.Drawing.Point(78, 42);
            this.radioudp.Name = "radioudp";
            this.radioudp.Size = new System.Drawing.Size(48, 17);
            this.radioudp.TabIndex = 1;
            this.radioudp.Text = "UDP";
            this.radioudp.UseVisualStyleBackColor = true;
            // 
            // comPortOut
            // 
            this.comPortOut.FormattingEnabled = true;
            this.comPortOut.Location = new System.Drawing.Point(242, 82);
            this.comPortOut.Name = "comPortOut";
            this.comPortOut.Size = new System.Drawing.Size(121, 21);
            this.comPortOut.TabIndex = 3;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(242, 152);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1010,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // textBox1
            // 
            this.textBox1.AcceptsReturn = true;
            this.textBox1.Location = new System.Drawing.Point(26, 237);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(370, 144);
            this.textBox1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Com Port Out";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Number of data points";
            // 
            // startbutton
            // 
            this.startbutton.Location = new System.Drawing.Point(168, 194);
            this.startbutton.Name = "startbutton";
            this.startbutton.Size = new System.Drawing.Size(75, 23);
            this.startbutton.TabIndex = 8;
            this.startbutton.Text = "start";
            this.startbutton.UseVisualStyleBackColor = true;
            this.startbutton.Click += new System.EventHandler(this.startbutton_Click);
            // 
            // radioserial
            // 
            this.radioserial.AutoSize = true;
            this.radioserial.Location = new System.Drawing.Point(132, 42);
            this.radioserial.Name = "radioserial";
            this.radioserial.Size = new System.Drawing.Size(71, 17);
            this.radioserial.TabIndex = 9;
            this.radioserial.TabStop = true;
            this.radioserial.Text = "serial only";
            this.radioserial.UseVisualStyleBackColor = true;
            this.radioserial.CheckedChanged += new System.EventHandler(this.radioserial_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(69, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(268, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Sends via serial port, receives through ethernet or serial";
            // 
            // comPortIn
            // 
            this.comPortIn.FormattingEnabled = true;
            this.comPortIn.Location = new System.Drawing.Point(242, 114);
            this.comPortIn.Name = "comPortIn";
            this.comPortIn.Size = new System.Drawing.Size(121, 21);
            this.comPortIn.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Com Port In";
            // 
            // baudratebox
            // 
            this.baudratebox.DisplayMember = "9600; 115200";
            this.baudratebox.FormattingEnabled = true;
            this.baudratebox.Location = new System.Drawing.Point(285, 38);
            this.baudratebox.Name = "baudratebox";
            this.baudratebox.Size = new System.Drawing.Size(87, 21);
            this.baudratebox.TabIndex = 13;
            this.baudratebox.ValueMember = "9600; 115200";
            // 
            // checkser1
            // 
            this.checkser1.AutoSize = true;
            this.checkser1.Location = new System.Drawing.Point(209, 43);
            this.checkser1.Name = "checkser1";
            this.checkser1.Size = new System.Drawing.Size(70, 17);
            this.checkser1.TabIndex = 15;
            this.checkser1.Text = "1 port ser";
            this.checkser1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 393);
            this.Controls.Add(this.checkser1);
            this.Controls.Add(this.baudratebox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comPortIn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.radioserial);
            this.Controls.Add(this.startbutton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.comPortOut);
            this.Controls.Add(this.radioudp);
            this.Controls.Add(this.radiotcp);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radiotcp;
        private System.Windows.Forms.RadioButton radioudp;
        private System.Windows.Forms.ComboBox comPortOut;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button startbutton;
        private System.Windows.Forms.RadioButton radioserial;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comPortIn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox baudratebox;
        private System.Windows.Forms.CheckBox checkser1;
    }
}

