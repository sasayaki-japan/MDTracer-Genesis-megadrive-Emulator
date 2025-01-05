namespace MDTracer
{
    partial class Form_VDP_Screen
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
            pictureBox_screen = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox_screen).BeginInit();
            SuspendLayout();
            // 
            // pictureBox_screen
            // 
            pictureBox_screen.BackColor = Color.Black;
            pictureBox_screen.Location = new Point(4, 4);
            pictureBox_screen.Margin = new Padding(4);
            pictureBox_screen.Name = "pictureBox_screen";
            pictureBox_screen.Size = new Size(256, 256);
            pictureBox_screen.TabIndex = 0;
            pictureBox_screen.TabStop = false;
            // 
            // Form_VDP_Screen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(264, 269);
            Controls.Add(pictureBox_screen);
            MinimumSize = new Size(272, 294);
            Name = "Form_VDP_Screen";
            Text = "Form_VDP_Memory";
            FormClosing += Form_VDP_Screen_FormClosing;
            Shown += Form_VDP_Screen_Shown;
            ResizeEnd += Form_VDP_Screen_ResizeEnd;
            Paint += Form_VDP_Memory_Paint;
            ((System.ComponentModel.ISupportInitialize)pictureBox_screen).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox_screen;
    }
}