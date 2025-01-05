using System;
using System.Diagnostics;

namespace MDTracer
{
    internal partial class md_vdp
    {
        //VDP status register
        public byte g_vdp_status_9_empl;
        public byte g_vdp_status_8_full;
        public byte g_vdp_status_7_vinterrupt;
        public byte g_vdp_status_6_sprite;
        public byte g_vdp_status_5_collision;
        public byte g_vdp_status_4_frame;
        public byte g_vdp_status_3_vbrank;
        public byte g_vdp_status_2_hbrank;
        public byte g_vdp_status_1_dma;
        public byte g_vdp_status_0_tvmode;

        //HV Counter
        public ushort g_vdp_c00008_hvcounter;
        public bool g_vdp_c00008_hvcounter_latched;

        //VDP register
        private byte[] g_vdp_reg;
        public byte g_vdp_reg_0_4_hinterrupt;
        public byte g_vdp_reg_0_1_hvcounter;
        public byte g_vdp_reg_1_6_display;
        public byte g_vdp_reg_1_5_vinterrupt;
        public byte g_vdp_reg_1_4_dma;
        public byte g_vdp_reg_1_3_cellmode;
        public int g_vdp_reg_2_scrolla;
        public int g_vdp_reg_3_windows;
        public int g_vdp_reg_4_scrollb;
        public int g_vdp_reg_5_sprite;
        public byte g_vdp_reg_7_backcolor;
        public byte g_vdp_reg_10_hint;
        public byte g_vdp_reg_11_3_ext;
        public byte g_vdp_reg_11_2_vscroll;
        public byte g_vdp_reg_11_1_hscroll;
        public byte g_vdp_reg_12_7_cellmode1;
        public byte g_vdp_reg_12_3_shadow;
        public byte g_vdp_reg_12_2_interlacemode;
        public byte g_vdp_reg_12_0_cellmode2;
        public int g_vdp_reg_13_hscroll;
        public byte g_vdp_reg_15_autoinc;
        public int g_vdp_reg_16_5_scrollV;
        public int g_vdp_reg_16_1_scrollH;
        public byte g_vdp_reg_17_7_windows;
        public byte g_vdp_reg_17_4_basspointer;
        public byte g_vdp_reg_18_7_windows;
        public byte g_vdp_reg_18_4_basspointer;
        public byte g_vdp_reg_19_dma_counter_low;
        public byte g_vdp_reg_20_dma_counter_high;
        public byte g_vdp_reg_21_dma_source_low;
        public byte g_vdp_reg_22_dma_source_mid;
        public byte g_vdp_reg_23_dma_mode;
        public byte g_vdp_reg_23_5_dma_high;

        private ushort get_vdp_status()
        {
            ushort w_out = 0;
            w_out = g_vdp_status_9_empl;
            w_out = (ushort)((w_out << 1) | g_vdp_status_8_full);
            w_out = (ushort)((w_out << 1) | g_vdp_status_7_vinterrupt);
            w_out = (ushort)((w_out << 1) | g_vdp_status_6_sprite);
            w_out = (ushort)((w_out << 1) | g_vdp_status_5_collision);
            w_out = (ushort)((w_out << 1) | g_vdp_status_4_frame);
            w_out = (ushort)((w_out << 1) | g_vdp_status_3_vbrank);
            w_out = (ushort)((w_out << 1) | g_vdp_status_2_hbrank);
            w_out = (ushort)((w_out << 1) | g_vdp_status_1_dma);
            w_out = (ushort)((w_out << 1) | g_vdp_status_0_tvmode);
            return w_out;
        }
        private ushort get_vdp_hvcounter()
        {
            ushort w_out = g_vdp_c00008_hvcounter;
            if (g_vdp_c00008_hvcounter_latched == false)
            {
                if (g_vdp_reg_12_2_interlacemode == 0)
                {
                    w_out = (ushort)
                        ((g_scanline << 8)
                        + ((g_display_xsize
                           * (md_main.g_md_m68k.g_clock_total - md_main.g_md_m68k.g_clock_now)
                           / md_main.VDL_LINE_RENDER_MC68_CLOCK) & 0xff));
                }
                else
                {
                    w_out = (ushort)
                        (((g_scanline << 7) & 0xff00)
                        + ((g_display_xsize
                           * (md_main.g_md_m68k.g_clock_total - md_main.g_md_m68k.g_clock_now)
                           / md_main.VDL_LINE_RENDER_MC68_CLOCK) & 0x00ff));
                }
                g_vdp_c00008_hvcounter = w_out;
            }
            return w_out;
        }

        
        private void set_vdp_register(uint in_num, byte in_data)
        {
            g_vdp_reg[in_num] = in_data;
            switch (in_num)
            {
                case 0:
                    g_vdp_reg_0_4_hinterrupt = (byte)((in_data >> 4) & 0x01);
                    g_vdp_reg_0_1_hvcounter = (byte)((in_data >> 1) & 0x01);
                    break;
                case 1:
                    g_vdp_reg_1_6_display = (byte)((in_data >> 6) & 0x01);
                    g_vdp_reg_1_5_vinterrupt = (byte)((in_data >> 5) & 0x01);
                    g_vdp_reg_1_4_dma = (byte)((in_data >> 4) & 0x01);
                    g_vdp_reg_1_3_cellmode = (byte)((in_data >> 3) & 0x01);
                    if (g_vdp_reg_1_3_cellmode == 0)
                    {
                        g_display_ysize = 224;
                        g_display_ycell = 28;
                        g_vertical_line_max = 262;
                    }
                    else
                    {
                        g_display_ysize = 240;
                        g_display_ycell = 30;
                        g_vertical_line_max = 312;
                    }
                    break;
                case 2:
                    g_vdp_reg_2_scrolla = (ushort)(in_data << 10);
                    break;
                case 3:
                    if (g_vdp_reg_12_7_cellmode1 == 0)
                    {
                        g_vdp_reg_3_windows = (ushort)((in_data & 0x3e) << 10);
                    }
                    else
                    {
                        g_vdp_reg_3_windows = (ushort)((in_data & 0x3c) << 10);
                    }
                    break;
                case 4:
                    g_vdp_reg_4_scrollb = (ushort)(in_data << 13);
                    break;
                case 5:
                    if (g_vdp_reg_12_7_cellmode1 == 0)
                    {
                        g_vdp_reg_5_sprite = (ushort)((in_data & 0x7f) << 9);
                    }
                    else
                    {
                        g_vdp_reg_5_sprite = (ushort)((in_data & 0x7e) << 9);
                    }
                    break;
                case 7:
                    g_vdp_reg_7_backcolor = (byte)(in_data & 0x3f);
                    break;
                case 10:
                    g_vdp_reg_10_hint = in_data;
                    break;
                case 11:
                    g_vdp_reg_11_3_ext = (byte)((in_data >> 3) & 0x01);
                    g_vdp_reg_11_2_vscroll = (byte)((in_data >> 2) & 0x01);
                    g_vdp_reg_11_1_hscroll = (byte)(in_data & 0x03);
                    break;
                case 12:
                    g_vdp_reg_12_7_cellmode1 = (byte)((in_data >> 7) & 0x01);
                    g_vdp_reg_12_3_shadow = (byte)((in_data >> 3) & 0x01);
                    g_vdp_reg_12_2_interlacemode = (byte)((in_data >> 1) & 0x03);
                    if (g_vdp_reg_12_2_interlacemode != 0) MessageBox.Show("g_vdp_reg_12_2_interlacemode  no support", "error");
                    if(g_vdp_reg_12_2_interlacemode == 0)
                    {
                        g_sprite_vmask = 0x1ff;
                    }
                    else
                    {
                        g_sprite_vmask = 0x3ff;
                    }
                    g_vdp_reg_12_0_cellmode2 = (byte)(in_data & 0x01);
                    if (g_vdp_reg_12_7_cellmode1 == 0)
                    {
                        g_display_xsize = 256;
                        g_display_xcell = 32;
                        g_max_sprite_num = 64;
                        g_max_sprite_line = 16;
                        g_max_sprite_cell = 32;
                    }
                    else
                    {
                        g_display_xsize = 320;
                        g_display_xcell = 40;
                        g_max_sprite_num = 80;
                        g_max_sprite_line = 20;
                        g_max_sprite_cell = 40;
                    }
                    if (g_vdp_reg_12_7_cellmode1 == 0)
                    {
                        g_vdp_reg_3_windows = (ushort)((g_vdp_reg[3] & 0x3e) << 10);
                    }
                    else
                    {
                        g_vdp_reg_3_windows = (ushort)((g_vdp_reg[3] & 0x3c) << 10);
                    }
                    if (g_vdp_reg_12_7_cellmode1 == 0)
                    {
                        g_vdp_reg_5_sprite = (ushort)((g_vdp_reg[5] & 0x7f) << 9);
                    }
                    else
                    {
                        g_vdp_reg_5_sprite = (ushort)((g_vdp_reg[5] & 0x7e) << 9);
                    }
                    break;
                case 13:
                    g_vdp_reg_13_hscroll = (ushort)(in_data << 10);
                    break;
                case 15:
                    g_vdp_reg_15_autoinc = in_data;
                    break;
                case 16:
                    g_vdp_reg_16_5_scrollV = (in_data >> 4) & 0x03;
                    g_vdp_reg_16_1_scrollH = in_data & 0x03;

                    g_scroll_ycell = 32 * (g_vdp_reg_16_5_scrollV + 1);
                    g_scroll_ysize = g_scroll_ycell << 3;
                    g_scroll_ysize_mask = g_scroll_ysize - 1;

                    g_scroll_xcell = 32 * (g_vdp_reg_16_1_scrollH + 1);
                    g_scroll_xsize = g_scroll_xcell << 3;
                    g_scroll_xsize_mask = g_scroll_xsize - 1;
                    break;
                case 17:
                    int w_pos = (in_data & 0x1f) << 4;
                    if ((in_data & 0x80) == 0)
                    {
                        if (w_pos < g_display_xsize)
                        {
                            g_screenA_left_x = w_pos;
                            g_screenA_right_x = g_display_xsize - 1;
                        }
                        else
                        {
                            g_screenA_left_x = 0;
                            g_screenA_right_x = 0;
                        }
                    }
                    else
                    {
                        if (w_pos == 0)
                        {
                            g_screenA_left_x = 0;
                            g_screenA_right_x = 0;
                        }
                        else
                        if (w_pos < g_display_xsize)
                        {
                            g_screenA_left_x = 0;
                            g_screenA_right_x = w_pos - 1;
                        }
                        else
                        if (g_display_xsize <= w_pos)
                        {
                            g_screenA_left_x = 0;
                            g_screenA_right_x = g_display_xsize - 1;
                        }
                    }
                    break;
                case 18:
                    w_pos = (in_data & 0x1f) << 3;
                    if ((in_data & 0x80) == 0)
                    {
                        if (w_pos < g_display_ysize)
                        {
                            g_screenA_top_y = w_pos;
                            g_screenA_bottom_y = g_display_ysize - 1;
                        }
                        else
                        {
                            g_screenA_top_y = 0;
                            g_screenA_bottom_y = 0;
                        }
                    }
                    else
                    {
                        if (w_pos == 0)
                        {
                            g_screenA_top_y = 0;
                            g_screenA_bottom_y = 0;
                        }
                        else
                        if (w_pos < g_display_ysize)
                        {
                            g_screenA_top_y = 0;
                            g_screenA_bottom_y = w_pos - 1;
                        }
                        else
                        if (g_display_ysize <= w_pos)
                        {
                            g_screenA_top_y = 0;
                            g_screenA_bottom_y = g_display_ysize - 1;
                        }
                    }
                    break;
                case 19:
                    g_vdp_reg_19_dma_counter_low = in_data;
                    break;
                case 20:
                    g_vdp_reg_20_dma_counter_high = in_data;
                    break;
                case 21:
                    g_vdp_reg_21_dma_source_low = in_data;
                    break;
                case 22:
                    g_vdp_reg_22_dma_source_mid = in_data;
                    break;
                case 23:
                    g_vdp_reg_23_dma_mode = (byte)((in_data >> 6) & 0x03);
                    if ((in_data & 0x80) == 0)
                    {
                        g_vdp_reg_23_5_dma_high = (byte)(in_data & 0x7f);
                    }
                    else
                    {
                        g_vdp_reg_23_5_dma_high = (byte)(in_data & 0x3f);
                    }
                    break;
            }
        }
    }
}
