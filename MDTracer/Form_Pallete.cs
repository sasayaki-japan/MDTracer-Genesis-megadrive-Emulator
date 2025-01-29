using System.Drawing.Imaging;

namespace MDTracer
{
    public partial class Form_Pallete : Form
    {
        public int g_screen_xpos;
        public int g_screen_ypos;
        //----------------------------------------------------------------
        //form
        //----------------------------------------------------------------
        public Form_Pallete()
        {
            InitializeComponent();
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            pictureBox_color.Image = new Bitmap(256, 128);
            pictureBox_shadow.Image = new Bitmap(256, 128);
            pictureBox_highlight.Image = new Bitmap(256, 128);
        }
        //----------------------------------------------------------------
        //Event Handling: Screen Operations
        //----------------------------------------------------------------
        private void Form_Pallete_FormClosing(object sender, FormClosingEventArgs e)
        {
            md_main.g_pallete_enable = false;
            md_main.g_form_setting.update();
            md_main.write_setting();
            e.Cancel = true;
        }
        private void Form_Pallete_ResizeEnd(object sender, EventArgs e)
        {
            var currentPosition = this.Location;
            g_screen_xpos = currentPosition.X;
            g_screen_ypos = currentPosition.Y;
            md_main.write_setting();
        }
        private void Form_Pallete_Shown(object sender, EventArgs e)
        {
            this.Location = new System.Drawing.Point(g_screen_xpos, g_screen_ypos);
        }
        //----------------------------------------------------------------
        //Event Handling: Painting
        //----------------------------------------------------------------
        private void Form_Pallete_Paint(object sender, PaintEventArgs e)
        {
            pictureBox_color.Invalidate();
            pictureBox_shadow.Invalidate();
            pictureBox_highlight.Invalidate();
        }
        private void pictureBox_color_Paint(object sender, PaintEventArgs e)
        {
            Bitmap dest_bitmap = (Bitmap)((PictureBox)sender).Image;
            BitmapData dest_bmpData = dest_bitmap.LockBits(new Rectangle(0, 0, dest_bitmap.Width, dest_bitmap.Height),
                                                 ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr dest_ptr = dest_bmpData.Scan0;
            int dest_stride = dest_bmpData.Stride;
            int bytesPerPixel = 4;
            unsafe
            {
                for (int wy = 0; wy < 4; wy++)
                {
                    for (int wx = 0; wx < 16; wx++)
                    {
                        uint w_color = md_main.g_md_vdp.g_color[wy * 16 + wx];
                        for (int dy = 0; dy < 32; dy++)
                        {
                            for (int dx = 0; dx < 16; dx++)
                            {
                                uint* pixel = (uint*)(dest_ptr + (((wy * 32) + dy) * dest_stride) + (((wx * 16) + dx) * bytesPerPixel));
                                *pixel = w_color;
                            }
                        }
                    }
                }
            }
            dest_bitmap.UnlockBits(dest_bmpData);
        }

        private void pictureBox_shadow_Paint(object sender, PaintEventArgs e)
        {
            Bitmap dest_bitmap = (Bitmap)((PictureBox)sender).Image;
            BitmapData dest_bmpData = dest_bitmap.LockBits(new Rectangle(0, 0, dest_bitmap.Width, dest_bitmap.Height),
                                                 ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr dest_ptr = dest_bmpData.Scan0;
            int dest_stride = dest_bmpData.Stride;
            int bytesPerPixel = 4;
            unsafe
            {
                for (int wy = 0; wy < 4; wy++)
                {
                    for (int wx = 0; wx < 16; wx++)
                    {
                        uint w_color = md_main.g_md_vdp.g_color_shadow[wy * 16 + wx];
                        for (int dy = 0; dy < 32; dy++)
                        {
                            for (int dx = 0; dx < 16; dx++)
                            {
                                uint* pixel = (uint*)(dest_ptr + (((wy * 32) + dy) * dest_stride) + (((wx * 16) + dx) * bytesPerPixel));
                                *pixel = w_color;
                            }
                        }
                    }
                }
            }
            dest_bitmap.UnlockBits(dest_bmpData);
        }

        private void pictureBox_highlight_Paint(object sender, PaintEventArgs e)
        {
            Bitmap dest_bitmap = (Bitmap)((PictureBox)sender).Image;
            BitmapData dest_bmpData = dest_bitmap.LockBits(new Rectangle(0, 0, dest_bitmap.Width, dest_bitmap.Height),
                                                 ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr dest_ptr = dest_bmpData.Scan0;
            int dest_stride = dest_bmpData.Stride;
            int bytesPerPixel = 4;
            unsafe
            {
                for (int wy = 0; wy < 4; wy++)
                {
                    for (int wx = 0; wx < 16; wx++)
                    {
                        uint w_color = md_main.g_md_vdp.g_color_highlight[wy * 16 + wx];
                        for (int dy = 0; dy < 32; dy++)
                        {
                            for (int dx = 0; dx < 16; dx++)
                            {
                                uint* pixel = (uint*)(dest_ptr + (((wy * 32) + dy) * dest_stride) + (((wx * 16) + dx) * bytesPerPixel));
                                *pixel = w_color;
                            }
                        }
                    }
                }
            }
            dest_bitmap.UnlockBits(dest_bmpData);
        }
    }
}
