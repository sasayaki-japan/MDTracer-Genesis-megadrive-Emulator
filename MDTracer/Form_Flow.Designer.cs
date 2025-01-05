namespace MDTracer
{
    partial class Form_Flow
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
            panel1 = new Panel();
            pictureBox_flow = new PictureBox();
            vScrollBar1 = new VScrollBar();
            hScrollBar1 = new HScrollBar();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_flow).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(pictureBox_flow);
            panel1.Controls.Add(vScrollBar1);
            panel1.Controls.Add(hScrollBar1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(529, 529);
            panel1.TabIndex = 0;
            // 
            // pictureBox_flow
            // 
            pictureBox_flow.Dock = DockStyle.Fill;
            pictureBox_flow.Location = new Point(0, 0);
            pictureBox_flow.Name = "pictureBox_flow";
            pictureBox_flow.Size = new Size(512, 512);
            pictureBox_flow.TabIndex = 6;
            pictureBox_flow.TabStop = false;
            pictureBox_flow.SizeChanged += pictureBox_flow_SizeChanged;
            pictureBox_flow.Paint += pictureBox_flow_Paint;
            pictureBox_flow.MouseDown += pictureBox_flow_MouseDown;
            pictureBox_flow.MouseMove += pictureBox_flow_MouseMove;
            pictureBox_flow.MouseUp += pictureBox_flow_MouseUp;
            // 
            // vScrollBar1
            // 
            vScrollBar1.Dock = DockStyle.Right;
            vScrollBar1.Location = new Point(512, 0);
            vScrollBar1.Name = "vScrollBar1";
            vScrollBar1.Size = new Size(17, 512);
            vScrollBar1.TabIndex = 5;
            vScrollBar1.Scroll += vScrollBar1_Scroll;
            // 
            // hScrollBar1
            // 
            hScrollBar1.Dock = DockStyle.Bottom;
            hScrollBar1.Location = new Point(0, 512);
            hScrollBar1.Name = "hScrollBar1";
            hScrollBar1.Size = new Size(529, 17);
            hScrollBar1.TabIndex = 4;
            hScrollBar1.Scroll += hScrollBar1_Scroll;
            // 
            // Form_Flow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(529, 529);
            Controls.Add(panel1);
            Name = "Form_Flow";
            Text = "Flow View";
            FormClosing += Form_Flow_FormClosing;
            Shown += Form_Flow_Shown;
            ResizeEnd += Form_Flow_ResizeEnd;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox_flow).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private VScrollBar vScrollBar1;
        private HScrollBar hScrollBar1;
        private PictureBox pictureBox_flow;
    }
}