using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace MDTracer
{
    internal partial class md_vdp
    {
        private void rendering_line_cpu()
        {
            int w_vscroll_mask = 0xffff;
            if (g_vdp_reg_11_2_vscroll == 1)
            {
                w_vscroll_mask = 0x000f;
            }
            for (int dx = 0; dx < 320; dx++)
            {
                g_game_cmap[dx] = 0;
                g_game_primap[dx] = 0;
                g_game_shadowmap[dx] = 0;
            }
            //rendering the scroll screenB
            {
                int w_view_x = g_line_snap[g_scanline].hscrollB;
                uint w_priority = 0;
                uint w_palette = 0;
                uint w_reverse = 0;
                uint w_char = 0;
                int w_view_addr = 0;
                int w_view_dx = 8;
                int w_view_dy = 0;
                int w_screen_adrdr = g_vdp_reg_4_scrollb >> 1;
                int w_pic_addr = 0;
                for (int wx = 0; wx < g_display_xsize; wx++)
                {
                    if ((wx & w_vscroll_mask) == 0)
                    {
                        int w_view_y = g_line_snap[g_scanline].vscrollB[wx >> 4];
                        w_view_dy = w_view_y & 7;
                        w_view_addr = w_screen_adrdr + ((w_view_y >> 3) * g_scroll_xcell);
                        w_view_dx = 8;
                    }
                    if (w_view_dx == 8)
                    {
                        w_view_x %= g_scroll_xsize;
                        w_view_dx = w_view_x & 7;
                        uint w_val = g_renderer_vram[w_view_addr + (w_view_x >> 3)];
                        w_priority = ((w_val >> 15) & 0x0001);
                        w_palette = (((w_val >> 13) & 0x0003) << 4);
                        w_reverse = ((w_val >> 11) & 0x0003);
                        w_char = (w_val & 0x07ff);
                        w_pic_addr = (int)((w_reverse * VRAM_DATASIZE) + (w_char << 4) + (w_view_dy << 1));
                    }
                    uint w_pic_w = g_renderer_vram[w_pic_addr + (w_view_dx >> 2)];
                    uint w_pic = (w_pic_w >> ((3 - (w_view_dx & 3)) << 2)) & 0x0f;
                    if (w_pic != 0)
                    {
                        g_game_cmap[wx] = w_palette + w_pic;
                        g_game_primap[wx] = w_priority;
                    }
                    g_game_shadowmap[wx] = w_priority;
                    w_view_x += 1;
                    w_view_dx += 1;
                }
            }
            //rendering the scroll screenA
            {
                if ((g_screenA_bottom_y == 0)
                || (g_scanline < g_screenA_top_y)
                || (g_screenA_bottom_y < g_scanline))
                {
                }
                else
                {
                    int w_view_x = g_line_snap[g_scanline].hscrollA;
                    uint w_priority = 0;
                    uint w_palette = 0;
                    uint w_reverse = 0;
                    uint w_char = 0;
                    int w_view_addr = 0;
                    int w_view_dx = 8;
                    int w_view_dy = 0;
                    int w_screen_adrdr = g_vdp_reg_2_scrolla >> 1;
                    int w_pic_addr = 0;
                    for (int wx = 0; wx < g_display_xsize; wx++)
                    {
                        if ((wx & w_vscroll_mask) == 0)
                        {
                            int w_view_y = g_line_snap[g_scanline].vscrollA[wx >> 4];
                            w_view_dy = w_view_y & 7;
                            w_view_addr = w_screen_adrdr + ((w_view_y >> 3) * g_scroll_xcell);
                            w_view_dx = 8;
                        }
                        if (w_view_dx == 8)
                        {
                            w_view_x %= g_scroll_xsize;
                            w_view_dx = w_view_x & 7;
                            uint w_val = g_renderer_vram[w_view_addr + (w_view_x >> 3)];
                            w_priority = ((w_val >> 15) & 0x0001);
                            w_palette = (((w_val >> 13) & 0x0003) << 4);
                            w_reverse = ((w_val >> 11) & 0x0003);
                            w_char = (w_val & 0x07ff);
                            w_pic_addr = (int)((w_reverse * VRAM_DATASIZE) + (w_char << 4) + (w_view_dy << 1));
                        }
                        if (((g_screenA_right_x != 0)
                            && (g_screenA_left_x <= wx) && (wx <= g_screenA_right_x)
                            && (g_game_primap[wx] <= w_priority)))
                        {
                            uint w_pic_w = g_renderer_vram[w_pic_addr + (w_view_dx >> 2)];
                            uint w_pic = (w_pic_w >> ((3 - (w_view_dx & 3)) << 2)) & 0x0f;
                            if (w_pic != 0)
                            {
                                g_game_cmap[wx] = w_palette + w_pic;
                                g_game_primap[wx] = w_priority;
                            }
                            g_game_shadowmap[wx] |= w_priority;
                        }
                        w_view_x += 1;
                        w_view_dx += 1;
                    }
                }

            }

            //rendering the sprite screen
            for (int i = 0; i < g_line_snap[g_scanline].sprite_rendrere_num; i++)
            {
                int w_sp = g_line_snap[g_scanline].sprite_rendrere_num - i - 1;
                int w_left = g_line_snap[g_scanline].sprite_left[w_sp];
                int w_top = g_line_snap[g_scanline].sprite_top[w_sp];
                int w_xcell_size = g_line_snap[g_scanline].sprite_xcell_size[w_sp];
                int w_ycell_size = g_line_snap[g_scanline].sprite_ycell_size[w_sp];
                uint w_priority = g_line_snap[g_scanline].sprite_priority[w_sp];
                uint w_palette = g_line_snap[g_scanline].sprite_palette[w_sp];
                uint w_reverse = g_line_snap[g_scanline].sprite_reverse[w_sp];
                uint w_reverse_addr = VRAM_DATASIZE * w_reverse;
                int w_char = (int)g_line_snap[g_scanline].sprite_char[w_sp];
                int w_y = g_scanline - w_top;
                int w_ycell = w_y >> 3;
                int w_cy = w_y & 7;
                int w_posx = w_left;
                for (int w_cur_xcell = 0; w_cur_xcell < w_xcell_size; w_cur_xcell++)
                {
                    int w_char_cur = 0;
                    switch (w_reverse)
                    {
                        case 0:
                            w_char_cur = w_char + (w_ycell_size * w_cur_xcell) + w_ycell;
                            break;
                        case 1:
                            w_char_cur = w_char + (w_ycell_size * (w_xcell_size - w_cur_xcell - 1)) + w_ycell;
                            break;
                        case 2:
                            w_char_cur = w_char + (w_ycell_size * w_cur_xcell) + (w_ycell_size - w_ycell - 1);
                            break;
                        default:
                            w_char_cur = w_char + (w_ycell_size * (w_xcell_size - w_cur_xcell - 1)) + (w_ycell_size - w_ycell - 1);
                            break;
                    }
                    for (int w_cx = 0; w_cx < 8; w_cx++)
                    {
                        if ((0 <= w_posx) && (w_posx < g_display_xsize))
                        {
                            if (g_game_primap[w_posx] <= w_priority)
                            {
                                int w_num = (w_char_cur << 4) + (w_cy << 1) + (w_cx >> 2);
                                uint w_pic_w = g_renderer_vram[w_reverse_addr + w_num];
                                uint w_pic = (w_pic_w >> ((3 - (w_cx & 3)) << 2)) & 0x0f;

                                if (w_pic != 0)
                                {
                                    uint w_color = (uint)(w_palette + w_pic);
                                    if (g_vdp_reg_12_3_shadow == 0)
                                    {
                                        g_game_cmap[w_posx] = w_color;
                                        g_game_primap[w_posx] = (uint)w_priority;
                                    }
                                    else if (w_color == 0x3e)
                                    {
                                        uint w_map = g_game_shadowmap[w_posx];
                                        if (w_map < 2)
                                            g_game_shadowmap[w_posx] = (uint)(w_map + 1);
                                    }
                                    else if (w_color == 0x3f)
                                    {
                                        uint w_map = g_game_shadowmap[w_posx];
                                        if (w_map > 0)
                                            g_game_shadowmap[w_posx] = (uint)(w_map - 1);
                                    }
                                    else if ((w_color & 0x0f) == 0x0e)
                                    {
                                        g_game_cmap[w_posx] = w_color;
                                        g_game_primap[w_posx] = (uint)w_priority;
                                        g_game_shadowmap[w_posx] = 0x1000;
                                    }
                                    else
                                    {
                                        g_game_cmap[w_posx] = w_color;
                                        g_game_primap[w_posx] = (uint)w_priority;
                                        g_game_shadowmap[w_posx] |= (uint)w_priority;
                                    }
                                }
                            }
                        }
                        w_posx += 1;
                    }
                }
            }

            //rendering the window screen
            {
                int w_xcell_st = g_line_snap[g_scanline].window_x_st;
                int w_xcell_ed = g_line_snap[g_scanline].window_x_ed;
                if (w_xcell_st != w_xcell_ed)
                {
                    int w_view_dy = g_scanline & 7;
                    int w_addr = (g_vdp_reg_3_windows >> 1) + ((g_scanline >> 3) * g_scroll_xcell) + w_xcell_st;
                    int w_posx = w_xcell_st << 3;
                    for (int w_cx = w_xcell_st; w_cx <= w_xcell_ed; w_cx++)
                    {
                        uint w_val = g_renderer_vram[w_addr];
                        w_addr += 1;
                        uint w_priority = ((w_val >> 15) & 0x0001);
                        uint w_palette = (((w_val >> 13) & 0x0003) << 4);
                        uint w_reverse = ((w_val >> 11) & 0x0003) * VRAM_DATASIZE;
                        uint w_char = (w_val & 0x07ff);
                        for (int w_dx = 0; w_dx < 8; w_dx++)
                        {
                            if ((g_game_cmap[w_posx] == 0) || (g_game_primap[w_posx] <= w_priority))
                            {
                                int w_pic_addr = (int)(w_reverse + (w_char << 4) + (w_view_dy << 1) + (w_dx >> 2));
                                uint w_pic_w = g_renderer_vram[w_pic_addr];
                                uint w_pic = (w_pic_w >> ((3 - (w_dx & 3)) << 2)) & 0x0f;
                                if (w_pic != 0)
                                {
                                    g_game_cmap[w_posx] = w_palette + w_pic;
                                    g_game_primap[w_posx] = w_priority;
                                }
                                g_game_shadowmap[w_posx] |= w_priority;
                            }
                            w_posx += 1;
                        }
                    }
                }
            }

            //rendering the game screen
            {
                uint color = 0;
                int w_base = g_scanline * g_display_xsize;
                for (int wx = 0; wx < g_display_xsize; wx++)
                {
                    uint w_colnum = g_game_cmap[wx];
                    if (w_colnum == 0) w_colnum = g_vdp_reg_7_backcolor;
                    if (g_vdp_reg_12_3_shadow == 0)
                    {
                        color = g_color[w_colnum];
                    }
                    else
                    {
                        uint w_shadow = g_game_shadowmap[wx];
                        if (w_shadow == 0) color = g_color_shadow[w_colnum];
                        else
                        if (w_shadow == 2) color = g_color_highlight[w_colnum];
                        else color = g_color[w_colnum];
                    }
                    g_game_screen[w_base + wx] = color;
                }
            }

        }
    }
}
