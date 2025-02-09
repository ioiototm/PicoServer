namespace PicoServer
{
    partial class PicoServerWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PicoServerWindow));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.txtBox_command = new System.Windows.Forms.TextBox();
            this.btn_sendCommand = new System.Windows.Forms.Button();
            this.btn_exit = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.rgb_none = new System.Windows.Forms.RadioButton();
            this.rgb_single_color = new System.Windows.Forms.RadioButton();
            this.rgb_rainbow_line = new System.Windows.Forms.RadioButton();
            this.rgb_rainbow_snake = new System.Windows.Forms.RadioButton();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.sleep_button = new System.Windows.Forms.Button();
            this.awake_button = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(149, 50);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(501, 149);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // txtBox_command
            // 
            this.txtBox_command.Location = new System.Drawing.Point(149, 13);
            this.txtBox_command.Name = "txtBox_command";
            this.txtBox_command.Size = new System.Drawing.Size(420, 20);
            this.txtBox_command.TabIndex = 1;
            // 
            // btn_sendCommand
            // 
            this.btn_sendCommand.Location = new System.Drawing.Point(586, 13);
            this.btn_sendCommand.Name = "btn_sendCommand";
            this.btn_sendCommand.Size = new System.Drawing.Size(64, 20);
            this.btn_sendCommand.TabIndex = 2;
            this.btn_sendCommand.Text = "Send";
            this.btn_sendCommand.UseVisualStyleBackColor = true;
            this.btn_sendCommand.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_exit
            // 
            this.btn_exit.Location = new System.Drawing.Point(670, 13);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(118, 20);
            this.btn_exit.TabIndex = 3;
            this.btn_exit.Text = "Exit Application";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.rgb_none);
            this.flowLayoutPanel1.Controls.Add(this.rgb_single_color);
            this.flowLayoutPanel1.Controls.Add(this.rgb_rainbow_line);
            this.flowLayoutPanel1.Controls.Add(this.rgb_rainbow_snake);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(149, 216);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(127, 175);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // rgb_none
            // 
            this.rgb_none.AutoSize = true;
            this.rgb_none.Location = new System.Drawing.Point(3, 3);
            this.rgb_none.Name = "rgb_none";
            this.rgb_none.Size = new System.Drawing.Size(51, 17);
            this.rgb_none.TabIndex = 0;
            this.rgb_none.TabStop = true;
            this.rgb_none.Text = "None";
            this.rgb_none.UseVisualStyleBackColor = true;
            this.rgb_none.CheckedChanged += new System.EventHandler(this.rgb_none_CheckedChanged);
            // 
            // rgb_single_color
            // 
            this.rgb_single_color.AutoSize = true;
            this.rgb_single_color.Location = new System.Drawing.Point(3, 26);
            this.rgb_single_color.Name = "rgb_single_color";
            this.rgb_single_color.Size = new System.Drawing.Size(81, 17);
            this.rgb_single_color.TabIndex = 1;
            this.rgb_single_color.TabStop = true;
            this.rgb_single_color.Text = "Single Color";
            this.rgb_single_color.UseVisualStyleBackColor = true;
            this.rgb_single_color.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // rgb_rainbow_line
            // 
            this.rgb_rainbow_line.AutoSize = true;
            this.rgb_rainbow_line.Location = new System.Drawing.Point(3, 49);
            this.rgb_rainbow_line.Name = "rgb_rainbow_line";
            this.rgb_rainbow_line.Size = new System.Drawing.Size(90, 17);
            this.rgb_rainbow_line.TabIndex = 2;
            this.rgb_rainbow_line.TabStop = true;
            this.rgb_rainbow_line.Text = "Rainbow Line";
            this.rgb_rainbow_line.UseVisualStyleBackColor = true;
            this.rgb_rainbow_line.CheckedChanged += new System.EventHandler(this.rgb_rainbow_line_CheckedChanged);
            // 
            // rgb_rainbow_snake
            // 
            this.rgb_rainbow_snake.AutoSize = true;
            this.rgb_rainbow_snake.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rgb_rainbow_snake.Location = new System.Drawing.Point(3, 72);
            this.rgb_rainbow_snake.Name = "rgb_rainbow_snake";
            this.rgb_rainbow_snake.Size = new System.Drawing.Size(101, 17);
            this.rgb_rainbow_snake.TabIndex = 3;
            this.rgb_rainbow_snake.TabStop = true;
            this.rgb_rainbow_snake.Text = "Rainbow Snake";
            this.rgb_rainbow_snake.UseVisualStyleBackColor = true;
            this.rgb_rainbow_snake.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.sleep_button);
            this.flowLayoutPanel2.Controls.Add(this.awake_button);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(376, 216);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(126, 168);
            this.flowLayoutPanel2.TabIndex = 5;
            // 
            // sleep_button
            // 
            this.sleep_button.Location = new System.Drawing.Point(3, 3);
            this.sleep_button.Name = "sleep_button";
            this.sleep_button.Size = new System.Drawing.Size(75, 23);
            this.sleep_button.TabIndex = 0;
            this.sleep_button.Text = "Sleep";
            this.sleep_button.UseVisualStyleBackColor = true;
            this.sleep_button.Click += new System.EventHandler(this.sleep_button_Click);
            // 
            // awake_button
            // 
            this.awake_button.Location = new System.Drawing.Point(3, 32);
            this.awake_button.Name = "awake_button";
            this.awake_button.Size = new System.Drawing.Size(75, 23);
            this.awake_button.TabIndex = 1;
            this.awake_button.Text = "Awake";
            this.awake_button.UseVisualStyleBackColor = true;
            this.awake_button.Click += new System.EventHandler(this.awake_button_Click);
            // 
            // PicoServerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.btn_sendCommand);
            this.Controls.Add(this.txtBox_command);
            this.Controls.Add(this.richTextBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PicoServerWindow";
            this.Text = "PicoServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PicoServerWindow_FormClosing);
            this.Load += new System.EventHandler(this.PicoServerWindow_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox txtBox_command;
        private System.Windows.Forms.Button btn_sendCommand;
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton rgb_none;
        private System.Windows.Forms.RadioButton rgb_single_color;
        private System.Windows.Forms.RadioButton rgb_rainbow_line;
        private System.Windows.Forms.RadioButton rgb_rainbow_snake;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button sleep_button;
        private System.Windows.Forms.Button awake_button;
    }
}

