using System.Diagnostics;

namespace MDTracer
{
    public partial class Form_VDP_Screen : Form
    {
        private int g_screen_xsize;
        private int g_screen_ysize;
        private int g_screen_xsize_change;
        private int g_screen_ysize_change;
        public int g_screen_xpos;
        public int g_screen_ypos;
        private string g_screen_type;
        //----------------------------------------------------------------
        //form
        //----------------------------------------------------------------
        public Form_VDP_Screen()
        {
            InitializeComponent();
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }
        //----------------------------------------------------------------
        //initialize
        //----------------------------------------------------------------
        public void initialize(string in_type, int in_screen_xsize, int in_screen_ysize, string in_title)
        {
            g_screen_xsize_change = in_screen_xsize;
            g_screen_ysize_change = in_screen_ysize;
            g_screen_type = in_type;
            this.Text = in_title;
            this.Invalidate();
        }
        //----------------------------------------------------------------
        //Event Handling: Screen Operations
        //----------------------------------------------------------------
        private void Form_VDP_Screen_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (g_screen_type)
            {
                case "A": md_main.g_screenA_enable = false; break;
                case "B": md_main.g_screenB_enable = false; break;
                case "W": md_main.g_screenW_enable = false; break;
                case "S": md_main.g_screenS_enable = false; break;
            }
            md_main.g_form_setting.update();
            md_main.write_setting();
            e.Cancel = true;
        }
        private void Form_VDP_Screen_ResizeEnd(object sender, EventArgs e)
        {
            var currentPosition = this.Location;
            g_screen_xpos = currentPosition.X;
            g_screen_ypos = currentPosition.Y;
            md_main.write_setting();
        }
        private void Form_VDP_Screen_Shown(object sender, EventArgs e)
        {
            this.Location = new System.Drawing.Point(g_screen_xpos, g_screen_ypos);
        }
        //----------------------------------------------------------------
        //Event Handling: Painting
        //----------------------------------------------------------------
        private void Form_VDP_Memory_Paint(object sender, PaintEventArgs e)
        {
            if ((g_screen_xsize != g_screen_xsize_change) || (g_screen_ysize != g_screen_ysize_change))
            {
                g_screen_xsize = g_screen_xsize_change;
                g_screen_ysize = g_screen_ysize_change;
                pictureBox_screen.Size = new Size(g_screen_xsize, g_screen_ysize);
                this.MinimumSize = new Size(g_screen_xsize + 24, g_screen_ysize + 46);
                this.MaximumSize = new Size(g_screen_xsize + 24, g_screen_ysize + 46);
                this.Size = new Size(g_screen_xsize + 24, g_screen_ysize + 46);
            }
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

        public void picture_update(Bitmap in_bitmap, int in_screen_xsize, int in_screen_ysize)
        {
            g_screen_xsize_change = in_screen_xsize;
            g_screen_ysize_change = in_screen_ysize;
            if (this.IsHandleCreated && this.Visible)
            {
                Rectangle rect = new Rectangle(0, 0, in_bitmap.Height, in_bitmap.Height);
                Bitmap bmp_dst = in_bitmap.Clone(rect, in_bitmap.PixelFormat);
                this.Invoke(new UpdatePictureBoxDelegate(this.UpdatePictureBox), new object[] { pictureBox_screen, bmp_dst });
                rect = Rectangle.Empty;
                bmp_dst.Dispose();
            }
            this.Invalidate();
        }
    }
}
