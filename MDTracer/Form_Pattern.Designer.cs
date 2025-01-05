namespace MDTracer
{
    partial class Form_Pattern
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
            pictureBox_pattern = new PictureBox();
            hScrollBar_picturebox = new VScrollBar();
            label_num = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox_pattern).BeginInit();
            SuspendLayout();
            // 
            // pictureBox_pattern
            // 
            pictureBox_pattern.BackColor = Color.Black;
            pictureBox_pattern.BorderStyle = BorderStyle.FixedSingle;
            pictureBox_pattern.Location = new Point(54, 12);
            pictureBox_pattern.Margin = new Padding(0);
            pictureBox_pattern.Name = "pictureBox_pattern";
            pictureBox_pattern.Size = new Size(256, 256);
            pictureBox_pattern.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox_pattern.TabIndex = 0;
            pictureBox_pattern.TabStop = false;
            // 
            // hScrollBar_picturebox
            // 
            hScrollBar_picturebox.Location = new Point(313, 12);
            hScrollBar_picturebox.Name = "hScrollBar_picturebox";
            hScrollBar_picturebox.Size = new Size(16, 256);
            hScrollBar_picturebox.TabIndex = 1;
            hScrollBar_picturebox.Scroll += hScrollBar_picturebox_Scroll;
            // 
            // label_num
            // 
            label_num.AutoSize = true;
            label_num.Location = new Point(7, 16);
            label_num.Name = "label_num";
            label_num.Size = new Size(38, 15);
            label_num.TabIndex = 2;
            label_num.Text = "label1";
            // 
            // Form_Pattern
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(337, 275);
            Controls.Add(label_num);
            Controls.Add(hScrollBar_picturebox);
            Controls.Add(pictureBox_pattern);
            Name = "Form_Pattern";
            Text = "Pattern View";
            FormClosing += Form_Pattern_FormClosing;
            Shown += Form_Pattern_Shown;
            ResizeEnd += Form_Pattern_ResizeEnd;
            Paint += Form_Pattern_Paint;
            ((System.ComponentModel.ISupportInitialize)pictureBox_pattern).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox_pattern;
        private VScrollBar hScrollBar_picturebox;
        private Label label_num;
    }
}