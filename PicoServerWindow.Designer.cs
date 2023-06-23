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
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(149, 209);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(501, 171);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            
            // 
            // txtBox_command
            // 
            this.txtBox_command.Location = new System.Drawing.Point(149, 57);
            this.txtBox_command.Name = "txtBox_command";
            this.txtBox_command.Size = new System.Drawing.Size(420, 20);
            this.txtBox_command.TabIndex = 1;
            // 
            // btn_sendCommand
            // 
            this.btn_sendCommand.Location = new System.Drawing.Point(586, 57);
            this.btn_sendCommand.Name = "btn_sendCommand";
            this.btn_sendCommand.Size = new System.Drawing.Size(64, 19);
            this.btn_sendCommand.TabIndex = 2;
            this.btn_sendCommand.Text = "Send";
            this.btn_sendCommand.UseVisualStyleBackColor = true;
            this.btn_sendCommand.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_exit
            // 
            this.btn_exit.Location = new System.Drawing.Point(665, 13);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(118, 23);
            this.btn_exit.TabIndex = 3;
            this.btn_exit.Text = "Exit Application";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // PicoServerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.btn_sendCommand);
            this.Controls.Add(this.txtBox_command);
            this.Controls.Add(this.richTextBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PicoServerWindow";
            this.Text = "PicoServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PicoServerWindow_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox txtBox_command;
        private System.Windows.Forms.Button btn_sendCommand;
        private System.Windows.Forms.Button btn_exit;
    }
}

