namespace PicoServer
{
    partial class PicoServerWindow
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PicoServerWindow));

            this.pnl_header       = new System.Windows.Forms.Panel();
            this.lbl_title        = new System.Windows.Forms.Label();
            this.lbl_mqttStatus   = new System.Windows.Forms.Label();
            this.grp_stats        = new System.Windows.Forms.GroupBox();
            this.richTextBox1     = new System.Windows.Forms.RichTextBox();
            this.grp_rgb          = new System.Windows.Forms.GroupBox();
            this.rgb_none         = new System.Windows.Forms.RadioButton();
            this.rgb_single_color = new System.Windows.Forms.RadioButton();
            this.rgb_rainbow_line = new System.Windows.Forms.RadioButton();
            this.rgb_rainbow_snake= new System.Windows.Forms.RadioButton();
            this.btn_pick_color   = new System.Windows.Forms.Button();
            this.grp_control      = new System.Windows.Forms.GroupBox();
            this.lbl_gpu_select   = new System.Windows.Forms.Label();
            this.gpuSelectionComboBox = new System.Windows.Forms.ComboBox();
            this.sleep_button     = new System.Windows.Forms.Button();
            this.awake_button     = new System.Windows.Forms.Button();
            this.grp_power        = new System.Windows.Forms.GroupBox();
            this.lbl_psu_header   = new System.Windows.Forms.Label();
            this.lbl_psu_value    = new System.Windows.Forms.Label();
            this.lbl_psu_source   = new System.Windows.Forms.Label();
            this.pnl_command      = new System.Windows.Forms.Panel();
            this.txtBox_command   = new System.Windows.Forms.TextBox();
            this.btn_sendCommand  = new System.Windows.Forms.Button();
            this.btn_exit         = new System.Windows.Forms.Button();
            this.colorDialog1     = new System.Windows.Forms.ColorDialog();

            this.pnl_header.SuspendLayout();
            this.grp_stats.SuspendLayout();
            this.grp_rgb.SuspendLayout();
            this.grp_control.SuspendLayout();
            this.grp_power.SuspendLayout();
            this.pnl_command.SuspendLayout();
            this.SuspendLayout();

            // ── Palette ──────────────────────────────────────────────────────────
            var bg        = System.Drawing.Color.FromArgb(28,  28,  35);
            var bgPanel   = System.Drawing.Color.FromArgb(40,  40,  50);
            var bgHeader  = System.Drawing.Color.FromArgb(18,  18,  24);
            var fgText    = System.Drawing.Color.FromArgb(220, 220, 230);
            var fgDim     = System.Drawing.Color.FromArgb(130, 130, 150);
            var btnNormal = System.Drawing.Color.FromArgb(55,  55,  68);
            var btnBorder = System.Drawing.Color.FromArgb(72,  72,  90);
            var btnBlue   = System.Drawing.Color.FromArgb(0,   120, 180);
            var btnRed    = System.Drawing.Color.FromArgb(180,  50,  50);
            var psuBlue   = System.Drawing.Color.FromArgb(100, 200, 255);
            var mqttRed   = System.Drawing.Color.FromArgb(200,  80,  80);

            // ── Header panel ─────────────────────────────────────────────────────
            this.pnl_header.Location  = new System.Drawing.Point(0, 0);
            this.pnl_header.Size      = new System.Drawing.Size(820, 50);
            this.pnl_header.BackColor = bgHeader;
            this.pnl_header.Name      = "pnl_header";
            this.pnl_header.TabIndex  = 10;
            this.pnl_header.Controls.Add(this.lbl_title);
            this.pnl_header.Controls.Add(this.lbl_mqttStatus);

            this.lbl_title.Text      = "PicoServer";
            this.lbl_title.Font      = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lbl_title.ForeColor = fgText;
            this.lbl_title.BackColor = System.Drawing.Color.Transparent;
            this.lbl_title.Location  = new System.Drawing.Point(14, 10);
            this.lbl_title.AutoSize  = true;
            this.lbl_title.Name      = "lbl_title";
            this.lbl_title.TabIndex  = 0;

            this.lbl_mqttStatus.Text      = "● Disconnected";
            this.lbl_mqttStatus.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.lbl_mqttStatus.ForeColor = mqttRed;
            this.lbl_mqttStatus.BackColor = System.Drawing.Color.Transparent;
            this.lbl_mqttStatus.Location  = new System.Drawing.Point(672, 16);
            this.lbl_mqttStatus.AutoSize  = true;
            this.lbl_mqttStatus.Name      = "lbl_mqttStatus";
            this.lbl_mqttStatus.TabIndex  = 1;

            // ── Live Stats ───────────────────────────────────────────────────────
            this.grp_stats.Text      = "Live Stats";
            this.grp_stats.Font      = new System.Drawing.Font("Segoe UI", 8.5F);
            this.grp_stats.ForeColor = fgDim;
            this.grp_stats.BackColor = bgPanel;
            this.grp_stats.Location  = new System.Drawing.Point(10, 58);
            this.grp_stats.Size      = new System.Drawing.Size(800, 120);
            this.grp_stats.Name      = "grp_stats";
            this.grp_stats.TabIndex  = 11;
            this.grp_stats.Controls.Add(this.richTextBox1);

            this.richTextBox1.ReadOnly    = true;
            this.richTextBox1.BackColor   = bgPanel;
            this.richTextBox1.ForeColor   = fgText;
            this.richTextBox1.Font        = new System.Drawing.Font("Consolas", 10.5F);
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.ScrollBars  = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.Location    = new System.Drawing.Point(10, 20);
            this.richTextBox1.Size        = new System.Drawing.Size(780, 90);
            this.richTextBox1.Name        = "richTextBox1";
            this.richTextBox1.TabIndex    = 0;
            this.richTextBox1.Text        = "";

            // ── RGB Mode ─────────────────────────────────────────────────────────
            this.grp_rgb.Text      = "RGB Mode";
            this.grp_rgb.Font      = new System.Drawing.Font("Segoe UI", 8.5F);
            this.grp_rgb.ForeColor = fgDim;
            this.grp_rgb.BackColor = bgPanel;
            this.grp_rgb.Location  = new System.Drawing.Point(10, 188);
            this.grp_rgb.Size      = new System.Drawing.Size(220, 155);
            this.grp_rgb.Name      = "grp_rgb";
            this.grp_rgb.TabIndex  = 12;
            this.grp_rgb.Controls.Add(this.rgb_none);
            this.grp_rgb.Controls.Add(this.rgb_single_color);
            this.grp_rgb.Controls.Add(this.rgb_rainbow_line);
            this.grp_rgb.Controls.Add(this.rgb_rainbow_snake);
            this.grp_rgb.Controls.Add(this.btn_pick_color);

            this.rgb_none.Text            = "None";
            this.rgb_none.ForeColor       = fgText;
            this.rgb_none.BackColor       = System.Drawing.Color.Transparent;
            this.rgb_none.Font            = new System.Drawing.Font("Segoe UI", 9F);
            this.rgb_none.Location        = new System.Drawing.Point(12, 24);
            this.rgb_none.AutoSize        = true;
            this.rgb_none.Name            = "rgb_none";
            this.rgb_none.TabIndex        = 0;
            this.rgb_none.TabStop         = true;
            this.rgb_none.CheckedChanged += new System.EventHandler(this.rgb_none_CheckedChanged);

            this.rgb_single_color.Text            = "Single Color";
            this.rgb_single_color.ForeColor       = fgText;
            this.rgb_single_color.BackColor       = System.Drawing.Color.Transparent;
            this.rgb_single_color.Font            = new System.Drawing.Font("Segoe UI", 9F);
            this.rgb_single_color.Location        = new System.Drawing.Point(12, 50);
            this.rgb_single_color.AutoSize        = true;
            this.rgb_single_color.Name            = "rgb_single_color";
            this.rgb_single_color.TabIndex        = 1;
            this.rgb_single_color.TabStop         = true;
            this.rgb_single_color.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);

            this.rgb_rainbow_line.Text            = "Rainbow Line";
            this.rgb_rainbow_line.ForeColor       = fgText;
            this.rgb_rainbow_line.BackColor       = System.Drawing.Color.Transparent;
            this.rgb_rainbow_line.Font            = new System.Drawing.Font("Segoe UI", 9F);
            this.rgb_rainbow_line.Location        = new System.Drawing.Point(12, 76);
            this.rgb_rainbow_line.AutoSize        = true;
            this.rgb_rainbow_line.Name            = "rgb_rainbow_line";
            this.rgb_rainbow_line.TabIndex        = 2;
            this.rgb_rainbow_line.TabStop         = true;
            this.rgb_rainbow_line.CheckedChanged += new System.EventHandler(this.rgb_rainbow_line_CheckedChanged);

            this.rgb_rainbow_snake.Text            = "Rainbow Snake";
            this.rgb_rainbow_snake.ForeColor       = fgText;
            this.rgb_rainbow_snake.BackColor       = System.Drawing.Color.Transparent;
            this.rgb_rainbow_snake.Font            = new System.Drawing.Font("Segoe UI", 9F);
            this.rgb_rainbow_snake.Location        = new System.Drawing.Point(12, 102);
            this.rgb_rainbow_snake.AutoSize        = true;
            this.rgb_rainbow_snake.Name            = "rgb_rainbow_snake";
            this.rgb_rainbow_snake.TabIndex        = 3;
            this.rgb_rainbow_snake.TabStop         = true;
            this.rgb_rainbow_snake.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);

            this.btn_pick_color.Text                       = "Pick Color...";
            this.btn_pick_color.ForeColor                  = fgText;
            this.btn_pick_color.BackColor                  = btnNormal;
            this.btn_pick_color.FlatStyle                  = System.Windows.Forms.FlatStyle.Flat;
            this.btn_pick_color.FlatAppearance.BorderColor = btnBorder;
            this.btn_pick_color.Font                       = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btn_pick_color.Location                   = new System.Drawing.Point(12, 124);
            this.btn_pick_color.Size                       = new System.Drawing.Size(194, 24);
            this.btn_pick_color.Name                       = "btn_pick_color";
            this.btn_pick_color.TabIndex                   = 4;
            this.btn_pick_color.Enabled                    = false;
            this.btn_pick_color.UseVisualStyleBackColor    = false;
            this.btn_pick_color.Click                     += new System.EventHandler(this.btn_pick_color_Click);

            // ── PC Control ───────────────────────────────────────────────────────
            this.grp_control.Text      = "PC Control";
            this.grp_control.Font      = new System.Drawing.Font("Segoe UI", 8.5F);
            this.grp_control.ForeColor = fgDim;
            this.grp_control.BackColor = bgPanel;
            this.grp_control.Location  = new System.Drawing.Point(240, 188);
            this.grp_control.Size      = new System.Drawing.Size(260, 145);
            this.grp_control.Name      = "grp_control";
            this.grp_control.TabIndex  = 13;
            this.grp_control.Controls.Add(this.lbl_gpu_select);
            this.grp_control.Controls.Add(this.gpuSelectionComboBox);
            this.grp_control.Controls.Add(this.sleep_button);
            this.grp_control.Controls.Add(this.awake_button);

            this.lbl_gpu_select.Text      = "GPU";
            this.lbl_gpu_select.ForeColor = fgDim;
            this.lbl_gpu_select.BackColor = System.Drawing.Color.Transparent;
            this.lbl_gpu_select.Font      = new System.Drawing.Font("Segoe UI", 8F);
            this.lbl_gpu_select.Location  = new System.Drawing.Point(10, 22);
            this.lbl_gpu_select.AutoSize  = true;
            this.lbl_gpu_select.Name      = "lbl_gpu_select";
            this.lbl_gpu_select.TabIndex  = 0;

            this.gpuSelectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gpuSelectionComboBox.BackColor      = bgPanel;
            this.gpuSelectionComboBox.ForeColor      = fgText;
            this.gpuSelectionComboBox.Font           = new System.Drawing.Font("Segoe UI", 9F);
            this.gpuSelectionComboBox.Location       = new System.Drawing.Point(10, 38);
            this.gpuSelectionComboBox.Size           = new System.Drawing.Size(238, 23);
            this.gpuSelectionComboBox.Name           = "gpuSelectionComboBox";
            this.gpuSelectionComboBox.TabIndex       = 1;

            this.sleep_button.Text                          = "Sleep";
            this.sleep_button.ForeColor                     = fgText;
            this.sleep_button.BackColor                     = btnNormal;
            this.sleep_button.FlatStyle                     = System.Windows.Forms.FlatStyle.Flat;
            this.sleep_button.FlatAppearance.BorderColor    = btnBorder;
            this.sleep_button.Font                          = new System.Drawing.Font("Segoe UI", 9F);
            this.sleep_button.Location                      = new System.Drawing.Point(10, 76);
            this.sleep_button.Size                          = new System.Drawing.Size(116, 30);
            this.sleep_button.Name                          = "sleep_button";
            this.sleep_button.TabIndex                      = 2;
            this.sleep_button.UseVisualStyleBackColor       = false;
            this.sleep_button.Click                        += new System.EventHandler(this.sleep_button_Click);

            this.awake_button.Text                          = "Wake";
            this.awake_button.ForeColor                     = fgText;
            this.awake_button.BackColor                     = btnNormal;
            this.awake_button.FlatStyle                     = System.Windows.Forms.FlatStyle.Flat;
            this.awake_button.FlatAppearance.BorderColor    = btnBorder;
            this.awake_button.Font                          = new System.Drawing.Font("Segoe UI", 9F);
            this.awake_button.Location                      = new System.Drawing.Point(133, 76);
            this.awake_button.Size                          = new System.Drawing.Size(116, 30);
            this.awake_button.Name                          = "awake_button";
            this.awake_button.TabIndex                      = 3;
            this.awake_button.UseVisualStyleBackColor       = false;
            this.awake_button.Click                        += new System.EventHandler(this.awake_button_Click);

            // ── PSU Power Draw ───────────────────────────────────────────────────
            this.grp_power.Text      = "PSU Power Draw";
            this.grp_power.Font      = new System.Drawing.Font("Segoe UI", 8.5F);
            this.grp_power.ForeColor = fgDim;
            this.grp_power.BackColor = bgPanel;
            this.grp_power.Location  = new System.Drawing.Point(510, 188);
            this.grp_power.Size      = new System.Drawing.Size(300, 145);
            this.grp_power.Name      = "grp_power";
            this.grp_power.TabIndex  = 14;
            this.grp_power.Controls.Add(this.lbl_psu_header);
            this.grp_power.Controls.Add(this.lbl_psu_value);
            this.grp_power.Controls.Add(this.lbl_psu_source);

            this.lbl_psu_header.Text      = "Current Draw";
            this.lbl_psu_header.ForeColor = fgDim;
            this.lbl_psu_header.BackColor = System.Drawing.Color.Transparent;
            this.lbl_psu_header.Font      = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lbl_psu_header.Location  = new System.Drawing.Point(10, 22);
            this.lbl_psu_header.AutoSize  = true;
            this.lbl_psu_header.Name      = "lbl_psu_header";
            this.lbl_psu_header.TabIndex  = 0;

            this.lbl_psu_value.Text      = "--- W";
            this.lbl_psu_value.ForeColor = psuBlue;
            this.lbl_psu_value.BackColor = System.Drawing.Color.Transparent;
            this.lbl_psu_value.Font      = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Bold);
            this.lbl_psu_value.Location  = new System.Drawing.Point(8, 38);
            this.lbl_psu_value.AutoSize  = true;
            this.lbl_psu_value.Name      = "lbl_psu_value";
            this.lbl_psu_value.TabIndex  = 1;

            this.lbl_psu_source.Text      = "via LibreHardwareMonitor";
            this.lbl_psu_source.ForeColor = fgDim;
            this.lbl_psu_source.BackColor = System.Drawing.Color.Transparent;
            this.lbl_psu_source.Font      = new System.Drawing.Font("Segoe UI", 7.5F);
            this.lbl_psu_source.Location  = new System.Drawing.Point(10, 118);
            this.lbl_psu_source.AutoSize  = true;
            this.lbl_psu_source.Name      = "lbl_psu_source";
            this.lbl_psu_source.TabIndex  = 2;

            // ── Command bar ──────────────────────────────────────────────────────
            this.pnl_command.Location  = new System.Drawing.Point(0, 343);
            this.pnl_command.Size      = new System.Drawing.Size(820, 52);
            this.pnl_command.BackColor = bgHeader;
            this.pnl_command.Name      = "pnl_command";
            this.pnl_command.TabIndex  = 15;
            this.pnl_command.Controls.Add(this.txtBox_command);
            this.pnl_command.Controls.Add(this.btn_sendCommand);
            this.pnl_command.Controls.Add(this.btn_exit);

            this.txtBox_command.BackColor    = bgPanel;
            this.txtBox_command.ForeColor    = fgText;
            this.txtBox_command.Font         = new System.Drawing.Font("Segoe UI", 9F);
            this.txtBox_command.BorderStyle  = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBox_command.Location     = new System.Drawing.Point(10, 14);
            this.txtBox_command.Size         = new System.Drawing.Size(630, 23);
            this.txtBox_command.Name         = "txtBox_command";
            this.txtBox_command.TabIndex     = 0;

            this.btn_sendCommand.Text                       = "Send";
            this.btn_sendCommand.ForeColor                  = System.Drawing.Color.White;
            this.btn_sendCommand.BackColor                  = btnBlue;
            this.btn_sendCommand.FlatStyle                  = System.Windows.Forms.FlatStyle.Flat;
            this.btn_sendCommand.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(0, 100, 160);
            this.btn_sendCommand.Font                       = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_sendCommand.Location                   = new System.Drawing.Point(650, 13);
            this.btn_sendCommand.Size                       = new System.Drawing.Size(75, 26);
            this.btn_sendCommand.Name                       = "btn_sendCommand";
            this.btn_sendCommand.TabIndex                   = 1;
            this.btn_sendCommand.UseVisualStyleBackColor    = false;
            this.btn_sendCommand.Click                     += new System.EventHandler(this.button1_Click);

            this.btn_exit.Text                       = "Exit";
            this.btn_exit.ForeColor                  = System.Drawing.Color.White;
            this.btn_exit.BackColor                  = btnRed;
            this.btn_exit.FlatStyle                  = System.Windows.Forms.FlatStyle.Flat;
            this.btn_exit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(150, 40, 40);
            this.btn_exit.Font                       = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_exit.Location                   = new System.Drawing.Point(735, 13);
            this.btn_exit.Size                       = new System.Drawing.Size(75, 26);
            this.btn_exit.Name                       = "btn_exit";
            this.btn_exit.TabIndex                   = 2;
            this.btn_exit.UseVisualStyleBackColor    = false;
            this.btn_exit.Click                     += new System.EventHandler(this.btn_exit_Click);

            // ── Form ─────────────────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(820, 395);
            this.BackColor           = bg;
            this.FormBorderStyle     = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox         = false;
            this.Icon                = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name                = "PicoServerWindow";
            this.Text                = "PicoServer";
            this.FormClosing        += new System.Windows.Forms.FormClosingEventHandler(this.PicoServerWindow_FormClosing);
            this.Load               += new System.EventHandler(this.PicoServerWindow_Load);
            this.Controls.Add(this.pnl_header);
            this.Controls.Add(this.grp_stats);
            this.Controls.Add(this.grp_rgb);
            this.Controls.Add(this.grp_control);
            this.Controls.Add(this.grp_power);
            this.Controls.Add(this.pnl_command);

            this.pnl_header.ResumeLayout(false);
            this.pnl_header.PerformLayout();
            this.grp_stats.ResumeLayout(false);
            this.grp_rgb.ResumeLayout(false);
            this.grp_rgb.PerformLayout();
            this.grp_control.ResumeLayout(false);
            this.grp_control.PerformLayout();
            this.grp_power.ResumeLayout(false);
            this.grp_power.PerformLayout();
            this.pnl_command.ResumeLayout(false);
            this.pnl_command.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel      pnl_header;
        private System.Windows.Forms.Label      lbl_title;
        private System.Windows.Forms.Label      lbl_mqttStatus;
        private System.Windows.Forms.GroupBox   grp_stats;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox   grp_rgb;
        private System.Windows.Forms.RadioButton rgb_none;
        private System.Windows.Forms.RadioButton rgb_single_color;
        private System.Windows.Forms.RadioButton rgb_rainbow_line;
        private System.Windows.Forms.RadioButton rgb_rainbow_snake;
        private System.Windows.Forms.Button     btn_pick_color;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.GroupBox   grp_control;
        private System.Windows.Forms.Label      lbl_gpu_select;
        private System.Windows.Forms.ComboBox   gpuSelectionComboBox;
        private System.Windows.Forms.Button     sleep_button;
        private System.Windows.Forms.Button     awake_button;
        private System.Windows.Forms.GroupBox   grp_power;
        private System.Windows.Forms.Label      lbl_psu_header;
        private System.Windows.Forms.Label      lbl_psu_value;
        private System.Windows.Forms.Label      lbl_psu_source;
        private System.Windows.Forms.Panel      pnl_command;
        private System.Windows.Forms.TextBox    txtBox_command;
        private System.Windows.Forms.Button     btn_sendCommand;
        private System.Windows.Forms.Button     btn_exit;
    }
}
