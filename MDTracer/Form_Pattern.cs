using System.Threading;

namespace MDTracer
{
    public partial class Form_Pattern : Form
    {
        public int g_screen_xpos;
        public int g_screen_ypos;
        public static int CHAR_MAX = 112;
        public static int g_cur_char;
        //----------------------------------------------------------------
        //form
        //----------------------------------------------------------------
        public Form_Pattern()
        {
            InitializeComponent();

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            label_num.Text = "0";

            hScrollBar_picturebox.Minimum = 0;
            hScrollBar_picturebox.Maximum = CHAR_MAX - 1;
            hScrollBar_picturebox.LargeChange = 1;
        }
        //----------------------------------------------------------------
        //Event Handling: Screen Operations
        //----------------------------------------------------------------
        private void hScrollBar_picturebox_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type != ScrollEventType.EndScroll)
            {
                int w_cur = e.NewValue;
                if (w_cur < 0)
                {
                    w_cur = 0;
                }
                else
                if (w_cur >= CHAR_MAX)
                {
                    w_cur = CHAR_MAX - 1;
                }
                g_cur_char = w_cur;
                label_num.Text = g_cur_char.ToString();
                this.Invalidate();
            }
        }
        private void Form_Pattern_FormClosing(object sender, FormClosingEventArgs e)
        {
            md_main.g_pattern_enable = false;
            md_main.g_form_setting.update();
            md_main.write_setting();
            e.Cancel = true;
        }
        private void Form_Pattern_ResizeEnd(object sender, EventArgs e)
        {
            var currentPosition = this.Location;
            g_screen_xpos = currentPosition.X;
            g_screen_ypos = currentPosition.Y;
            md_main.write_setting();
        }
        private void Form_Pattern_Shown(object sender, EventArgs e)
        {
            this.Location = new System.Drawing.Point(g_screen_xpos, g_screen_ypos);
        }
        //----------------------------------------------------------------
        //Event Handling: Painting
        //----------------------------------------------------------------
        private void Form_Pattern_Paint(object sender, PaintEventArgs e)
        {
            pictureBox_pattern.Invalidate();
        }
        public delegate void UpdatePictureBoxDelegate(PictureBox in_pic, Bitmap in_bitmap);
        private void UpdatePictureBox(PictureBox in_pic, Bitmap in_bitmap)
        {
            if (in_pic.InvokeRequired)
            {
                in_pic.Invoke(new UpdatePictureBoxDelegate(UpdatePictureBox), new object[] { in_bitmap.Clone() });
            }
            else
            {
                in_pic.Image = (Bitmap)in_bitmap.Clone();
            }
        }

        public void picture_update(Bitmap in_bitmap)
        {
            if (this.IsHandleCreated && this.Visible)
            {
                Rectangle rect = new Rectangle(0, g_cur_char << 3, 128, 128);
                Bitmap bmp_dst = in_bitmap.Clone(rect, in_bitmap.PixelFormat);
                this.Invoke(new UpdatePictureBoxDelegate(this.UpdatePictureBox), new object[] { pictureBox_pattern, bmp_dst });
                rect = Rectangle.Empty;
                bmp_dst.Dispose();
            }
        }
    }
}
