using NAudio.Gui;
using System;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static MDTracer.Form_Code_Trace;

namespace MDTracer
{
    public partial class Form_Code : Form
    {
        public int g_top_line;
        public int g_stop_line;
        public int g_cursole_line;
        public int g_screen_xpos;
        public int g_screen_ypos;

        public enum MEMMONITOR_TYPE : int
        {
            NON,
            RW,
            R,
            W
        }
        public List<MEMMONITOR_TYPE> g_memory_monitor_type;
        public List<uint> g_memory_monitor_addr;
        public List<uint> g_memory_monitor_val;
        public int g_memory_monitor_hit;
        //----------------------------------------------------------------
        //form
        //----------------------------------------------------------------
        public Form_Code()
        {
            InitializeComponent();
            pictureBox_code.MouseWheel += PictureBox_code_MouseWheel;

            scrollbar_set();
            md_main.g_form_code_trace.g_arrow_start_line = -1;
            md_main.g_form_code_trace.g_arrow_end_line = -1;
            g_memory_monitor_type = new List<MEMMONITOR_TYPE>();
            g_memory_monitor_addr = new List<uint>();
            g_memory_monitor_val = new List<uint>();
            g_memory_monitor_hit = -1;
        }
        //----------------------------------------------------------------
        //Event Handling: Painting
        //----------------------------------------------------------------
        private void pictureBox_code_paint(object sender, PaintEventArgs e)
        {
            vScrollBar_code.Value = g_top_line;
            md_main.g_form_code_trace.Code_Paint_Code(e, pictureBox_code.Width
                                            , pictureBox_code.Height
                                            , pictureBox_Code_line_num()
                                            , g_top_line
                                            , g_stop_line
                                            , g_cursole_line
                                            , hScrollBar_code.Value);
        }
        //----------------------------------------------------------------               
        //Event Handling: Screen Operations
        //----------------------------------------------------------------
        private void Form_Code_Resize(object sender, EventArgs e)
        {
            scrollbar_set();
            pictureBox_code.Invalidate();
        }
        private void Form_Code_FormClosing(object sender, FormClosingEventArgs e)
        {
            md_main.g_code_enable = false;
            md_main.g_form_setting.update();
            md_main.write_setting();
            e.Cancel = true;
        }
        private void Form_Code_ResizeEnd(object sender, EventArgs e)
        {
            var currentPosition = this.Location;
            g_screen_xpos = currentPosition.X;
            g_screen_ypos = currentPosition.Y;
            md_main.write_setting();
        }
        private void Form_Code_Shown(object sender, EventArgs e)
        {
            this.Location = new System.Drawing.Point(g_screen_xpos, g_screen_ypos);
        }

        //----------------------------------------------------------------
        //Event Handling: mouse operations
        //----------------------------------------------------------------
        private void vScrollBar_code_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type != ScrollEventType.EndScroll)
            {
                picturebox_scroll(e.NewValue, 0);
            }
        }
        private void hScrollBar_code_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type != ScrollEventType.EndScroll)
            {
                scrollbar_set();
                pictureBox_code.Invalidate();
            }
        }

        private void PictureBox_code_MouseWheel(object sender, MouseEventArgs e)
        {
            int w_cur = 0;
            if (e.Delta > 0) w_cur = -1;
            if (e.Delta < 0) w_cur = 1;
            picturebox_scroll(g_top_line, w_cur);
        }
        private void pictureBox_code_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int w_cur_line = ((e.Y - 20) / 16);
                int w_cur = g_top_line;
                for (int i = 0; i < w_cur_line; i++)
                {
                    w_cur += md_main.g_form_code_trace.g_analyse_code[w_cur].leng2;
                    if (w_cur > Form_Code_Trace.MEMSIZE) return;
                }
                if (e.X < 20)
                {
                    if (md_main.g_form_code_trace.g_analyse_code[w_cur].break_static == false)
                    {
                        md_main.g_form_code_trace.g_analyse_code[w_cur].break_static = true;
                    }
                    else
                    {
                        md_main.g_form_code_trace.g_analyse_code[w_cur].break_static = false;
                    }
                }
                else
                {
                    g_cursole_line = w_cur;
                }
                int w_jmp = md_main.g_form_code_trace.g_analyse_code[w_cur].jmp_address;
                if (w_jmp != 0)
                {
                    md_main.g_form_code_trace.g_arrow_start_line = w_cur;
                    md_main.g_form_code_trace.g_arrow_end_line = w_jmp / 2;
                }
                else
                {
                    md_main.g_form_code_trace.g_arrow_start_line = -1;
                    md_main.g_form_code_trace.g_arrow_end_line = -1;
                }
                pictureBox_code.Invalidate();
            }
        }
        private void pictureBox_code_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if ((240 < e.X) && (e.X < 720))
                {
                    int w_cur_line = ((e.Y - 20) / 16);
                    int w_cur = g_top_line;
                    for (int i = 0; i < w_cur_line; i++)
                    {
                        w_cur += md_main.g_form_code_trace.g_analyse_code[w_cur].leng2;
                    }
                    int w_jmp = md_main.g_form_code_trace.g_analyse_code[w_cur].jmp_address;
                    if (w_jmp != 0)
                    {
                        g_cursole_line = w_jmp / 2;
                        picturebox_scroll(g_cursole_line, -pictureBox_Code_line_num() / 2);
                    }
                }
            }
        }
        //----------------------------------------------------------------
        //Event Handling: key operations
        //----------------------------------------------------------------
        private void Form_Code_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    g_cursole_line += md_main.g_form_code_trace.g_analyse_code[g_cursole_line].leng2;
                    if (g_cursole_line > Form_Code_Trace.MEMSIZE) g_cursole_line = Form_Code_Trace.MEMSIZE - 1;
                    picturebox_scroll(g_top_line, 1);
                    break;
                case Keys.Up:
                    g_cursole_line -= md_main.g_form_code_trace.g_analyse_code[g_cursole_line].front;
                    if (g_cursole_line < 0) g_cursole_line = 0;
                    picturebox_scroll(g_top_line, -1);
                    break;
                case Keys.PageDown: picturebox_scroll(g_top_line, pictureBox_Code_line_num() - 1); break;
                case Keys.PageUp: picturebox_scroll(g_top_line, -(pictureBox_Code_line_num() - 1)); break;
            }
        }
        //----------------------------------------------------------------
        //Event Handling: menu
        //----------------------------------------------------------------
        private void runMenuItem_Click(object sender, EventArgs e)
        {
            md_main.g_form_code_trace.Trace_Start();
        }

        private void stopMenuItem_Click(object sender, EventArgs e)
        {
            md_main.g_form_code_trace.Trace_Stop();
        }
        private void stepOverMenuItem_Click(object sender, EventArgs e)
        {
            md_main.g_form_code_trace.Trace_StepOver();
        }
        private void stepInMenuItem_Click(object sender, EventArgs e)
        {
            md_main.g_form_code_trace.Trace_StepIn();
        }
        private void breakPointMenuItem_Click(object sender, EventArgs e)
        {
            if (md_main.g_form_code_trace.g_analyse_code[g_cursole_line].break_static == false)
            {
                md_main.g_form_code_trace.g_analyse_code[g_cursole_line].break_static = true;
            }
            else
            {
                md_main.g_form_code_trace.g_analyse_code[g_cursole_line].break_static = false;
            }
            pictureBox_code.Invalidate();
        }
        private void codeRefleshMenuItem_Click(object sender, EventArgs e)
        {
            md_main.g_form_code_trace.analyses();
            pictureBox_code.Invalidate();
        }

        private void skipnextframeMenuItem_Click(object sender, EventArgs e)
        {
            md_main.g_trace_nextframe = true;
            md_main.g_form_code_trace.Trace_Start();
        }
        //----------------------------------------------------------------
        //sub function
        //----------------------------------------------------------------
        public int pictureBox_Code_line_num()
        {
            return (pictureBox_code.Height - 4) / 16;
        }
        private void scrollbar_set()
        {
            int w_leng = pictureBox_Code_line_num();
            vScrollBar_code.Maximum = Form_Code_Trace.MEMSIZE;
            vScrollBar_code.SmallChange = 1;
            vScrollBar_code.LargeChange = w_leng;
            hScrollBar_code.Maximum = 1500;
            hScrollBar_code.SmallChange = 1;
            hScrollBar_code.LargeChange = pictureBox_code.Width;
        }
        public void picturebox_scroll(int in_line, int in_line_offset)
        {
            int w_line = in_line;
            if (in_line_offset != 0)
            {
                if (0 < in_line_offset)
                {
                    for (int i = 0; i < in_line_offset; i++)
                    {
                        TRACECODE w_code = md_main.g_form_code_trace.g_analyse_code[w_line];
                        w_line += w_code.leng2;
                        if (w_line >= Form_Code_Trace.MEMSIZE)
                        {
                            w_line = in_line;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < -in_line_offset; i++)
                    {
                        if (w_line <= 0)
                        {
                            w_line = in_line;
                            break;
                        }
                        TRACECODE w_code = md_main.g_form_code_trace.g_analyse_code[w_line - 1];
                        w_line -= w_code.front + 1;
                    }
                }
            }
            else
            {
                TRACECODE w_code = md_main.g_form_code_trace.g_analyse_code[w_line];
                TRACECODE w_code_prev = default;
                if (w_line + w_code.front < Form_Code_Trace.MEMSIZE)
                {
                    w_code_prev = md_main.g_form_code_trace.g_analyse_code[w_line + w_code.front];
                }
                TRACECODE w_code_next = default;
                if (w_line - w_code.leng2 > 0)
                {
                    w_code_next = md_main.g_form_code_trace.g_analyse_code[w_line - w_code.leng2];
                }
                if (g_top_line < w_line)
                {
                    if (w_line - w_code.front > 0)
                    {
                        w_line += w_code_prev.leng2;
                    }
                    else
                    {
                        w_line -= w_code_next.front;
                    }
                }
                else
                if (g_top_line > w_line)
                {
                    if (w_line + w_code.leng2 < Form_Code_Trace.MEMSIZE)
                    {
                        w_line -= w_code_next.front;
                    }
                    else
                    {
                        w_line += w_code_prev.leng2;
                    }
                }
            }
            g_top_line = w_line;
            pictureBox_code.Invalidate();
        }
        //----------------------------------------------------------------
        //Event Handling: search
        //----------------------------------------------------------------
        private void textBoxAddr_TextChanged_1(object sender, EventArgs e)
        {
            if (textBoxAddr.Text.Length > 0)
            {
                if (IsHexadecimal(textBoxAddr.Text))
                {
                    int waddr = int.Parse(textBoxAddr.Text, System.Globalization.NumberStyles.HexNumber);
                    picturebox_scroll(waddr / 2, 0);
                    this.Invalidate();
                }
            }
        }

        private void comboBox_comment2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string wselectedItem = comboBox_comment2.SelectedItem.ToString();
            if (wselectedItem != "")
            {
                md_main.g_form_code_trace.analyses();
                listBox_search.Items.Clear();
                for (int w_line = 0; w_line < Form_Code_Trace.MEMSIZE; w_line++)
                {
                    Form_Code_Trace.TRACECODE w_code = md_main.g_form_code_trace.g_analyse_code[w_line];
                    if (w_code.type == Form_Code_Trace.TRACECODE.TYPE.OPC)
                    {
                        if (w_code.comment1 == wselectedItem)
                        {
                            listBox_search.Items.Add(w_code.address.ToString("x6"));
                        }
                    }
                }
            }
        }

        private void listBox_search_Click(object sender, EventArgs e)
        {
            if (listBox_search.SelectedItem != null)
            {
                int w_addr = int.Parse(listBox_search.SelectedItem.ToString(), System.Globalization.NumberStyles.HexNumber);
                picturebox_scroll(w_addr / 2, 0);
                this.Invalidate();
            }
        }
        private void listBox_search_DoubleClick(object sender, EventArgs e)
        {
            if (listBox_search.SelectedItem != null)
            {
                ushort w_addr = ushort.Parse(listBox_search.SelectedItem.ToString(), System.Globalization.NumberStyles.HexNumber);
                int w_line = md_main.g_form_code_trace.get_code_from_addr(w_addr);
                if (md_main.g_form_code_trace.g_analyse_code[w_line].break_static != true)
                {
                    md_main.g_form_code_trace.g_analyse_code[w_line].break_static = true;
                }
                else
                {
                    md_main.g_form_code_trace.g_analyse_code[w_line].break_static = false;
                }
                picturebox_scroll(w_addr / 2, 0);
                this.Invalidate();
            }
        }
        private void dataGridView_memory_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int w_row = dataGridView_memory.RowCount;
            g_memory_monitor_type.Clear();
            g_memory_monitor_addr.Clear();
            g_memory_monitor_val.Clear();
            for (int i = 0; i < w_row; i++)
            {
                var w_val1 = dataGridView_memory.Rows[i].Cells[0].Value;
                var w_val2 = dataGridView_memory.Rows[i].Cells[1].Value;
                if((w_val1 == null)&& (w_val2 != null))
                {
                    w_val1 = "read/write";
                    dataGridView_memory.Rows[i].Cells[0].Value = w_val1;
                }
                if(w_val2 != null)
                {                    
                    if((IsHexadecimal(w_val2.ToString()) == true))
                    {
                        uint w_addr = uint.Parse(w_val2.ToString(), System.Globalization.NumberStyles.HexNumber);
                        g_memory_monitor_addr.Add(w_addr);
                        switch(w_val1)
                        {
                            case "read/write":
                                g_memory_monitor_type.Add(MEMMONITOR_TYPE.RW);
                                break;
                            case "read":
                                g_memory_monitor_type.Add(MEMMONITOR_TYPE.R);
                                break;
                            case "write":
                                g_memory_monitor_type.Add(MEMMONITOR_TYPE.W);
                                break;
                            default:
                                g_memory_monitor_type.Add(MEMMONITOR_TYPE.NON);
                                break;
                        }
                    }
                }
                g_memory_monitor_val.Add(0);
            }
        }
        public void memory_monitor_check(uint in_addr, uint in_val, bool in_write_enable)
        {
            g_memory_monitor_hit = -1;
            int w_row = g_memory_monitor_type.Count;
            for (int i = 0; i < w_row; i++)
            {
                if(g_memory_monitor_addr[i] == in_addr)
                {
                    switch(g_memory_monitor_type[i])
                    {
                        case MEMMONITOR_TYPE.RW:
                            g_memory_monitor_hit = i;
                            g_memory_monitor_val[i] = in_val;
                            dataGridView_memory.Rows[i].Cells[2].Value = in_val;
                            break;
                        case MEMMONITOR_TYPE.R:
                            if(in_write_enable == false)
                            {
                                g_memory_monitor_hit = i;
                                g_memory_monitor_val[i] = in_val;
                                dataGridView_memory.Rows[i].Cells[2].Value = in_val;
                            }
                            break;
                        case MEMMONITOR_TYPE.W:
                            if (in_write_enable == true)
                            {
                                g_memory_monitor_hit = i;
                                g_memory_monitor_val[i] = in_val;
                                dataGridView_memory.Rows[i].Cells[2].Value = in_val;
                            }
                            break;
                    }
                }
            }
        }

        //----------------------------------------------------------------
        //sub func
        //----------------------------------------------------------------
        private bool IsHexadecimal(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c) && !((c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f')))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

