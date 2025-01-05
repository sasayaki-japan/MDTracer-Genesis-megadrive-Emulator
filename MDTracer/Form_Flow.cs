using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using static MDTracer.Form_Code_Trace;

namespace MDTracer
{
    public partial class Form_Flow : Form
    {
        private List<STACK_LIST> g_list_chk;
        private Bitmap g_work_bitmap;
        public static int g_screen_size_x;
        public static int g_screen_size_y;
        public int g_screen_xpos;
        public int g_screen_ypos;
        private Point g_screen_offset;
        private Point g_mouse_offset;
        private Point g_work_offset;
        private int g_flow_cur;
        private int g_flow_max_level;
        private Font g_font;
        private bool g_dragging;
        private Point g_startPoint;
        public bool g_update_req;
        private uint g_func_address;
        private uint g_caller_address;
        //----------------------------------------------------------------
        //form
        //----------------------------------------------------------------
        public Form_Flow()
        {
            InitializeComponent();
            g_screen_size_x = pictureBox_flow.Width;
            g_screen_size_y = pictureBox_flow.Height;
            g_work_bitmap = new Bitmap(512, 512);
            pictureBox_flow.Image = g_work_bitmap;
            pictureBox_flow.BackColor = Color.White;
            g_font = new Font("ＭＳ ゴシック", 10);
            g_list_chk = new List<STACK_LIST>();
        }
        //----------------------------------------------------------------
        //Event Handling: Screen Operations
        //----------------------------------------------------------------
        private void Form_Flow_FormClosing(object sender, FormClosingEventArgs e)
        {
            md_main.g_flow_enable = false;
            md_main.g_form_setting.update();
            md_main.write_setting();
            e.Cancel = true;
        }
        private void Form_Flow_ResizeEnd(object sender, EventArgs e)
        {
            var currentPosition = this.Location;
            g_screen_xpos = currentPosition.X;
            g_screen_ypos = currentPosition.Y;
            md_main.write_setting();
        }
        private void Form_Flow_Shown(object sender, EventArgs e)
        {
            this.Location = new System.Drawing.Point(g_screen_xpos, g_screen_ypos);
        }
        private void pictureBox_flow_SizeChanged(object sender, EventArgs e)
        {
            g_screen_size_x = pictureBox_flow.Width;
            g_screen_size_y = pictureBox_flow.Height;
            if((g_screen_size_x != 0) && (g_screen_size_y != 0))
            {
                g_work_bitmap = new Bitmap(g_screen_size_x, g_screen_size_y);
                scrollbar_set();
            }
        }
        //----------------------------------------------------------------
        //Event Handling: mouse operations
        //----------------------------------------------------------------
        private void pictureBox_flow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                g_startPoint = e.Location;
                g_mouse_offset.X = 0;
                g_mouse_offset.Y = 0;
                g_dragging = true;
            }
        }
        private void pictureBox_flow_MouseMove(object sender, MouseEventArgs e)
        {
            if (g_dragging == true)
            {
                g_mouse_offset.X = e.X - g_startPoint.X;
                g_mouse_offset.Y = e.Y - g_startPoint.Y;
                if (g_screen_offset.X - g_mouse_offset.X < 0)
                {
                    g_mouse_offset.X += g_screen_offset.X - g_mouse_offset.X;
                }
                if (g_screen_offset.Y - g_mouse_offset.Y < 0)
                {
                    g_mouse_offset.Y += g_screen_offset.Y - g_mouse_offset.Y;
                }
                g_work_offset.X = g_screen_offset.X - g_mouse_offset.X;
                g_work_offset.Y = g_screen_offset.Y - g_mouse_offset.Y;
                pictureBox_flow.Invalidate();
            }
        }
        private void pictureBox_flow_MouseUp(object sender, MouseEventArgs e)
        {
            g_dragging = false;
            g_screen_offset.X -= g_mouse_offset.X;
            g_screen_offset.Y -= g_mouse_offset.Y;
            g_work_offset.X = g_screen_offset.X;
            g_work_offset.Y = g_screen_offset.Y;
            pictureBox_flow.Invalidate();
            hScrollBar1.Value = g_work_offset.X;
            vScrollBar1.Value = g_work_offset.Y;
        }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type != ScrollEventType.EndScroll)
            {
                g_screen_offset.Y = e.NewValue;
                g_work_offset.Y = g_screen_offset.Y;
                pictureBox_flow.Invalidate();
            }
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type != ScrollEventType.EndScroll)
            {
                g_screen_offset.X = e.NewValue;
                g_work_offset.X = g_screen_offset.X;
                pictureBox_flow.Invalidate();
            }
        }
        //----------------------------------------------------------------
        //Event Handling: Painting
        //----------------------------------------------------------------
        private void pictureBox_flow_Paint(object sender, PaintEventArgs e)
        {
            if(g_update_req == true)
            {
                flow_update();
                g_update_req = false;
            }
            g_flow_max_level = 0;
            using (Graphics g = Graphics.FromImage(g_work_bitmap))
            {
                g.Clear(Color.White);
                g_flow_cur = 0;
                for (int i = 0; i < g_list_chk.Count; i++)
                {
                    if (g_list_chk[i].type == STACK_LIST_TYPE.TOP)
                    {
                        flow_check(g
                            , g_list_chk[i].start_address
                            , g_list_chk[i].end_address
                            , g_list_chk[i].caller_address
                            , 0 
                            , -1);
                        g_flow_cur += 1;
                    }
                }
                pictureBox_flow.Image = g_work_bitmap;
            }
            scrollbar_set();
        }
        private void flow_check(Graphics in_g
            , uint in_start_address
            , uint in_end_address
            , uint in_caller_address
            , int in_level
            , int in_cur_level)
        {
            int w_cur_level = in_cur_level;
            int w_x = 0;
            int w_y = 0;
            Font wfont = new Font("ＭＳ ゴシック", 10);
            Brush wbrush = Brushes.Black;

            if (g_flow_max_level < in_level + 1)
            {
                g_flow_max_level = in_level + 1;
            }
            if ((in_start_address == g_func_address) && (in_caller_address == g_caller_address))
            {
                w_cur_level = in_level;
            }

            for (int j = 0; j <= in_level; j++)
            {
                w_x = 10 + (j * 120);
                w_y = 10 + g_flow_cur * 20;
                if (w_cur_level == j)
                {
                    draw_box(in_g, w_x, w_y, 100, 20);
                }
                draw_line(in_g, w_x, w_y, w_x, w_y + 19);
                draw_line(in_g, w_x + 99, w_y, w_x + 99, w_y + 19);
            }
            w_x = 10 + (in_level * 120);
            w_y = 10 + (g_flow_cur * 20);
            draw_line(in_g, w_x, w_y, w_x + 99, w_y);
            draw_string(in_g, in_start_address.ToString("x6"), w_x + 2, w_y + 4);
            if (in_level > 0)
            {
                draw_line(in_g, w_x - 20, w_y + 10, w_x, w_y + 10);
                draw_string(in_g, in_caller_address.ToString("x6"), w_x + 52 - 120, w_y + 4);
            }
            g_flow_cur += 1;

            bool w_top_call = true;
            for (int i = 0; i < g_list_chk.Count; i++)
            {
                STACK_LIST w_list = g_list_chk[i];
                if (w_list.func_address == in_start_address)
                {
                    if(w_top_call == false)
                    {
                        for (int j = 0; j <= in_level; j++)
                        {
                            w_x = 10 + (j * 120);
                            w_y = 10 + g_flow_cur * 20;
                            if (w_cur_level == j)
                            {
                                draw_box(in_g, w_x, w_y, 100, 20);
                            }
                            draw_line(in_g, w_x, w_y, w_x, w_y + 19);
                            draw_line(in_g, w_x + 99, w_y, w_x + 99, w_y + 19);
                        }
                        g_flow_cur += 1;
                    }
                    w_top_call = false;
                    flow_check(in_g, w_list.start_address, w_list.end_address, w_list.caller_address, in_level + 1, w_cur_level);
                }
            }
            for (int j = 0; j <= in_level; j++)
            {
                w_x = 10 + (j * 120);
                w_y = 10 + g_flow_cur * 20;
                if (w_cur_level == j)
                {
                    draw_box(in_g, w_x, w_y, 100, 20);
                }
                draw_line(in_g, w_x, w_y, w_x, w_y + 19);
                draw_line(in_g, w_x + 99, w_y, w_x + 99, w_y + 19);
            }
            w_x = 10 + (in_level * 120);
            w_y = 10 + g_flow_cur * 20;
            draw_line(in_g, w_x, w_y + 19, w_x + 99, w_y + 19);
            draw_string(in_g, in_end_address.ToString("x6"), w_x + 2, w_y + 4);
            g_flow_cur += 1;
        }
        private void draw_line(Graphics in_g, int in_x1, int in_y1, int in_x2, int in_y2)
        {
            in_x1 -= g_work_offset.X;
            in_x2 -= g_work_offset.X;
            in_y1 -= g_work_offset.Y;
            in_y2 -= g_work_offset.Y;

            if ((-100 < in_x1) && (in_x2 < g_screen_size_x + 100) && (-100 < in_y1) && (in_y2 < g_screen_size_y + 100))
            {
                in_g.DrawLine(Pens.Black, in_x1, in_y1, in_x2, in_y2);
            }
        }
        private void draw_box(Graphics in_g, int in_x, int in_y, int in_wx, int in_wy)
        {
            in_x -= g_work_offset.X;
            in_y -= g_work_offset.Y;
            if ((-100 < in_x) && ((in_x + in_wx) < g_screen_size_x + 100) && (-100 < in_y) && ((in_y + in_wy) < g_screen_size_y + 100))
            {
                using (Brush brush = new SolidBrush(Color.LightBlue))
                {
                    in_g.FillRectangle(brush, new Rectangle(in_x, in_y, in_wx, in_wy));
                }
            }
        }
        private void draw_string(Graphics in_g, string in_str, int in_x, int in_y)
        {
            in_x -= g_work_offset.X;
            in_y -= g_work_offset.Y;
            if ((-100 < in_x) && (in_x < g_screen_size_x + 100) && (-100 < in_y) && (in_y < g_screen_size_y + 100))
            {
                in_g.DrawString(in_str, g_font, Brushes.Black, new PointF(in_x, in_y));
            }
        }
        //----------------------------------------------------------------
        //sub function
        //----------------------------------------------------------------
        public void flow_update_req(uint in_func_address, uint in_caller_address)
        {
            g_func_address = in_func_address;
            g_caller_address = in_caller_address;
            g_update_req = true;
        }
        public void flow_update()
        {
            g_list_chk.Clear();
            int w_line = 0;
            do
            {
                TRACECODE w_code = md_main.g_form_code_trace.g_analyse_code[w_line];
                for (int i = 0; i < w_code.stack.Count; i++)
                {
                    g_list_chk.Add(new STACK_LIST
                    {
                        type = w_code.stack[i].type,
                        func_address = w_code.stack[i].func_address,
                        caller_address = w_code.stack[i].caller_address,
                        ret_address = w_code.stack[i].ret_address,
                        start_address = w_code.stack[i].start_address,
                        end_address = w_code.stack[i].end_address
                    });
                    if ((w_code.stack[i].caller_address <= 256) && (md_main.g_form_code_trace.g_analyse_code[w_code.stack[i].caller_address / 4].stack.Count == 0))
                    {
                        if (false == g_list_chk.Exists(x => x.start_address == w_code.stack[i].caller_address))
                        {
                            g_list_chk.Add(new STACK_LIST
                            {
                                type = STACK_LIST_TYPE.TOP,
                                func_address = 0,
                                ret_address = 0,
                                start_address = w_code.stack[i].caller_address,
                                end_address = 0
                            });
                        }
                    }
                }
                w_line += 1;
            } while (w_line < MEMSIZE);
        }
        private void scrollbar_set()
        {
            vScrollBar1.Maximum = g_flow_cur * 20;
            vScrollBar1.SmallChange = 1;
            vScrollBar1.LargeChange = pictureBox_flow.Height;
            hScrollBar1.Maximum = g_flow_max_level * 120;
            hScrollBar1.SmallChange = 1;
            hScrollBar1.LargeChange = pictureBox_flow.Width;
        }
    }
}
