namespace MDTracer
{
    public partial class Form_MUSIC : Form
    {
        public float[] KEY_SCALE_LIST = {
            34, 36, 38, 40, 42, 45, 48, 50, 53, 57,
            60, 64, 67, 71, 76, 80, 85, 90, 95, 101,
            107, 113, 120, 127, 135, 143, 151, 160, 170, 180,
            190, 202, 214, 227, 240, 254, 269, 285, 302, 320,
            339, 360, 381, 404, 428, 453, 480, 509, 539, 571,
            605, 641, 679, 719, 762, 807, 855, 906, 960, 1017,
            1078, 1142, 1210, 1282, 1358, 1438, 1524, 1615, 1711, 1812,
            1920, 2034, 2155, 2283, 2419, 2563, 2715, 2877, 3048, 3229,
            3421, 3625, 3840, 4069, 4310, 4567, 4838, 5126, 5431, 5754,
            6096, 6458, 6842, 7249, 7680 };
        public int[] KEY_WIDTH = { 9, 6, 6, 6, 9, 9, 6, 6, 6, 6, 6, 9 };
        public int[] KEY_POS = { 9, 15, 21, 27, 36, 45, 51, 57, 63, 69, 75, 84 };
        public int[] KEY_COLOR = { 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 0 };
        public Bitmap g_orgBitmap;
        public Bitmap g_cpyBitmap;
        public int[] g_freq_out;
        public int g_screen_xpos;
        public int g_screen_ypos;
        //----------------------------------------------------------------
        //form
        //----------------------------------------------------------------
        public Form_MUSIC()
        {
            InitializeComponent();
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            g_orgBitmap = new Bitmap(240, 674);
            g_cpyBitmap = new Bitmap(210, 674);
            g_freq_out = new int[11];
            pictureBox_view.Image = g_orgBitmap;

            //Keyboard display
            using (Graphics g = Graphics.FromImage(g_orgBitmap))
            {
                Brush brush = new SolidBrush(Color.White);
                Rectangle rect = new Rectangle(0, 0, 29, 672);
                g.FillRectangle(brush, rect);
                Pen pen = new Pen(Color.Black);
                for (int i = 0; i < 56; i++)
                {
                    rect = new Rectangle(0, i * 12, 28, 12);
                    g.DrawRectangle(pen, rect);
                }
                brush = new SolidBrush(Color.Black);
                for (int i = 0; i < 8; i++)
                {
                    rect = new Rectangle(0, i * 84 + 8, 15, 8);
                    g.FillRectangle(brush, rect);
                    rect = new Rectangle(0, i * 84 + 20, 15, 8);
                    g.FillRectangle(brush, rect);
                    rect = new Rectangle(0, i * 84 + 32, 15, 8);
                    g.FillRectangle(brush, rect);
                    rect = new Rectangle(0, i * 84 + 56, 15, 8);
                    g.FillRectangle(brush, rect);
                    rect = new Rectangle(0, i * 84 + 68, 15, 8);
                    g.FillRectangle(brush, rect);
                }
                Font font = new Font("Arial", 7);
                for (int i = 0; i < 8; i++)
                {
                    brush = Brushes.Black;
                    PointF point = new PointF(10, 661 - 84 * i);
                    g.DrawString("C" + i, font, brush, point);
                }
                //background display
                int dy = 672;
                for (int i = 0; i < 96; i++)
                {
                    if ((i % 2) == 0)
                    {
                        brush = new SolidBrush(Color.Azure);
                    }
                    else
                    {
                        brush = new SolidBrush(Color.Ivory);
                    }
                    dy -= KEY_WIDTH[i % 12];
                    rect = new Rectangle(29, dy, 211, KEY_WIDTH[i % 12]);
                    g.FillRectangle(brush, rect);
                }
                pictureBox_view.Image = (Bitmap)g_orgBitmap;
            }
        }
        //----------------------------------------------------------------
        //initialize
        //----------------------------------------------------------------
        public void initialize()
        {
            hScrollBar_Fm1.Value = md_main.g_md_music.g_master_vol[0];
            hScrollBar_Fm2.Value = md_main.g_md_music.g_master_vol[1];
            hScrollBar_Fm3.Value = md_main.g_md_music.g_master_vol[2];
            hScrollBar_Fm4.Value = md_main.g_md_music.g_master_vol[3];
            hScrollBar_Fm5.Value = md_main.g_md_music.g_master_vol[4];
            hScrollBar_Fm6.Value = md_main.g_md_music.g_master_vol[5];
            hScrollBar_Psg1.Value = md_main.g_md_music.g_master_vol[6];
            hScrollBar_Psg2.Value = md_main.g_md_music.g_master_vol[7];
            hScrollBar_Psg3.Value = md_main.g_md_music.g_master_vol[8];
            hScrollBar_Psg4.Value = md_main.g_md_music.g_master_vol[9];
            hScrollBar_Master.Value = md_main.g_md_music.g_master_vol[10];
            checkBox_Fm1.Checked = md_main.g_md_music.g_master_chk[0];
            checkBox_Fm2.Checked = md_main.g_md_music.g_master_chk[1];
            checkBox_Fm3.Checked = md_main.g_md_music.g_master_chk[2];
            checkBox_Fm4.Checked = md_main.g_md_music.g_master_chk[3];
            checkBox_Fm5.Checked = md_main.g_md_music.g_master_chk[4];
            checkBox_Fm6.Checked = md_main.g_md_music.g_master_chk[5];
            checkBox_Psg1.Checked = md_main.g_md_music.g_master_chk[6];
            checkBox_Psg2.Checked = md_main.g_md_music.g_master_chk[7];
            checkBox_Psg3.Checked = md_main.g_md_music.g_master_chk[8];
            checkBox_Psg4.Checked = md_main.g_md_music.g_master_chk[9];
            checkBox_Master.Checked = md_main.g_md_music.g_master_chk[10];
        }

        //----------------------------------------------------------------
        //Event Handling: Painting
        //----------------------------------------------------------------
        private void Form_MUSIC_Paint(object sender, PaintEventArgs e)
        {
            pictureBox_view.Invalidate();
        }
        private void pictureBox_view_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics g_cpy = Graphics.FromImage(g_cpyBitmap))
            {
                Rectangle srcRect = new Rectangle(29, 0, 210, 672);
                Rectangle desRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
                g_cpy.DrawImage(g_orgBitmap, desRect, srcRect, GraphicsUnit.Pixel);
            }
            using (Graphics g_org = Graphics.FromImage(g_orgBitmap))
            {
                Rectangle srcRect = new Rectangle(0, 0, 210, 672);
                Rectangle desRect = new Rectangle(30, 0, srcRect.Width, srcRect.Height);
                g_org.DrawImage(g_cpyBitmap, desRect, srcRect, GraphicsUnit.Pixel);
            }
            using (Graphics g_org = Graphics.FromImage(g_orgBitmap))
            {
                for (int i = 0; i <= 9; i++)
                {
                    if (md_main.g_md_music.g_master_chk[i] == false) continue;
                    Brush brush;
                    switch (i)
                    {
                        case 0:
                            brush = new SolidBrush(Color.Salmon);
                            break;
                        case 1:
                            brush = new SolidBrush(Color.Gold);
                            break;
                        case 2:
                            brush = new SolidBrush(Color.Lime);
                            break;
                        case 3:
                            brush = new SolidBrush(Color.Aqua);
                            break;
                        case 4:
                            brush = new SolidBrush(Color.SteelBlue);
                            break;
                        case 5:
                            brush = new SolidBrush(Color.DarkKhaki);
                            break;
                        case 6:
                            brush = new SolidBrush(Color.Orchid);
                            break;
                        case 7:
                            brush = new SolidBrush(Color.DeepPink);
                            break;
                        case 8:
                            brush = new SolidBrush(Color.Pink);
                            break;
                        default:
                            brush = new SolidBrush(Color.DarkViolet);
                            break;
                    }
                    int w_freq = g_freq_out[i];
                    if (0 < w_freq)
                    {
                        int w_ley_number = key_number_chk(w_freq);
                        int dy = 672 - ((int)(w_ley_number / 12) * 84) - KEY_POS[w_ley_number % 12];
                        Rectangle rect = new Rectangle(30, dy, 2, KEY_WIDTH[w_ley_number % 12]);
                        g_org.FillRectangle(brush, rect);
                    }
                }
            }
        }
        private int key_number_chk(float freq)
        {
            int w_out = KEY_SCALE_LIST.Length;
            for (int i = 0; i < KEY_SCALE_LIST.Length; i++)
            {
                if (freq <= KEY_SCALE_LIST[i])
                {
                    w_out = i;
                    break;
                }
            }
            return w_out;
        }
        //----------------------------------------------------------------
        ///Event Handling: Screen Operations
        //----------------------------------------------------------------
        private void hScrollBar_Fm1_Scroll(object sender, ScrollEventArgs e)
        {
            md_main.g_md_music.g_master_vol[0] = hScrollBar_Fm1.Value;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void hScrollBar_Fm2_Scroll(object sender, ScrollEventArgs e)
        {
            md_main.g_md_music.g_master_vol[1] = hScrollBar_Fm2.Value;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void hScrollBar_Fm3_Scroll(object sender, ScrollEventArgs e)
        {
            md_main.g_md_music.g_master_vol[2] = hScrollBar_Fm3.Value;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void hScrollBar_Fm4_Scroll(object sender, ScrollEventArgs e)
        {
            md_main.g_md_music.g_master_vol[3] = hScrollBar_Fm4.Value;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void hScrollBar_Fm5_Scroll(object sender, ScrollEventArgs e)
        {
            md_main.g_md_music.g_master_vol[4] = hScrollBar_Fm5.Value;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void hScrollBar_Fm6_Scroll(object sender, ScrollEventArgs e)
        {
            md_main.g_md_music.g_master_vol[5] = hScrollBar_Fm6.Value;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void hScrollBar_Psg1_Scroll(object sender, ScrollEventArgs e)
        {
            md_main.g_md_music.g_master_vol[6] = hScrollBar_Psg1.Value;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void hScrollBar_Psg2_Scroll(object sender, ScrollEventArgs e)
        {
            md_main.g_md_music.g_master_vol[7] = hScrollBar_Psg2.Value;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void hScrollBar_Psg3_Scroll(object sender, ScrollEventArgs e)
        {
            md_main.g_md_music.g_master_vol[8] = hScrollBar_Psg3.Value;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void hScrollBar_Psg4_Scroll(object sender, ScrollEventArgs e)
        {
            md_main.g_md_music.g_master_vol[9] = hScrollBar_Psg4.Value;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void hScrollBar_Master_Scroll(object sender, ScrollEventArgs e)
        {
            md_main.g_md_music.g_master_vol[10] = hScrollBar_Master.Value;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void checkBox_Fm1_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_md_music.g_master_chk[0] = checkBox_Fm1.Checked;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void checkBox_Fm2_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_md_music.g_master_chk[1] = checkBox_Fm2.Checked;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void checkBox_Fm3_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_md_music.g_master_chk[2] = checkBox_Fm3.Checked;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void checkBox_Fm4_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_md_music.g_master_chk[3] = checkBox_Fm4.Checked;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void checkBox_Fm5_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_md_music.g_master_chk[4] = checkBox_Fm5.Checked;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void checkBox_Fm6_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_md_music.g_master_chk[5] = checkBox_Fm6.Checked;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void checkBox_Psg1_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_md_music.g_master_chk[6] = checkBox_Psg1.Checked;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void checkBox_Psg2_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_md_music.g_master_chk[7] = checkBox_Psg2.Checked;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void checkBox_Psg3_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_md_music.g_master_chk[8] = checkBox_Psg3.Checked;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void checkBox_Psg4_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_md_music.g_master_chk[9] = checkBox_Psg4.Checked;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void checkBox_Master_CheckedChanged(object sender, EventArgs e)
        {
            md_main.g_md_music.g_master_chk[10] = checkBox_Master.Checked;
            md_main.g_md_music.setting();
            md_main.write_setting();
        }
        private void Form_MUSIC_FormClosing(object sender, FormClosingEventArgs e)
        {
            md_main.g_music_enable = false;
            md_main.g_form_setting.update();
            md_main.write_setting();
            e.Cancel = true;
        }
        private void Form_MUSIC_ResizeEnd(object sender, EventArgs e)
        {
            var currentPosition = this.Location;
            g_screen_xpos = currentPosition.X;
            g_screen_ypos = currentPosition.Y;
            md_main.write_setting();
        }
        private void Form_MUSIC_Shown(object sender, EventArgs e)
        {
            this.Location = new System.Drawing.Point(g_screen_xpos, g_screen_ypos);
        }
    }
}
