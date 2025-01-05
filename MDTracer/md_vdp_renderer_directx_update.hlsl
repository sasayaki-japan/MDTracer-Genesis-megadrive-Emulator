static const int VRAM_DATASIZE = 65536 / 2;
static const int DISPLAY_XSIZE = 320;
static const int VSRAM_DATASIZE = 20;
static const int MAX_SPRITE = 20;
struct CSInput
{
    uint3 groupId : SV_GroupID;
    uint groupIndex : SV_GroupIndex;
};
cbuffer Input : register(b0)
{
    int display_xsize;
    int display_ysize;
    int scroll_xsize;
    int scroll_xcell;
    int scroll_mask;
    int scrollw_xcell;
    int vdp_reg_1_6_display;
    int vdp_reg_2_scrolla;
    int vdp_reg_4_scrollb;
    int vdp_reg_3_windows;
    uint vdp_reg_7_backcolor;
    uint vdp_reg_12_3_shadow;
    uint screenA_left;
    uint screenA_right;
    uint screenA_top;
    uint screenA_bottom;
};
struct VDP_LINE_SNAP
{
    int hscrollA;
    int hscrollB;
    int vscrollA[VSRAM_DATASIZE];
    int vscrollB[VSRAM_DATASIZE];
    int window_x_st;
    int window_x_ed;
    int sprite_rendrere_num;
    int sprite_left[MAX_SPRITE];
    int sprite_right[MAX_SPRITE];
    int sprite_top[MAX_SPRITE];
    int sprite_bottom[MAX_SPRITE];
    int sprite_xcell_size[MAX_SPRITE];
    int sprite_ycell_size[MAX_SPRITE];
    int sprite_priority[MAX_SPRITE];
    int sprite_palette[MAX_SPRITE];
    int sprite_reverse[MAX_SPRITE];
    int sprite_char[MAX_SPRITE];
};
StructuredBuffer<uint> vram : register(t0);
StructuredBuffer<uint> g_color : register(t1);
StructuredBuffer<uint> g_color_shadow : register(t2);
StructuredBuffer<uint> g_color_highlight : register(t3);
StructuredBuffer<VDP_LINE_SNAP> g_line_snap : register(t4);
RWStructuredBuffer<uint> g_game_screen : register(u0);
RWStructuredBuffer<uint> g_game_cmap : register(u1);
RWStructuredBuffer<uint> g_game_primap : register(u2);
RWStructuredBuffer<uint> g_game_shadowmap : register(u3);

//rendering the scroll screenB
[numthreads(1, 1, 1)]
void CS_SCREENB(CSInput in_param)
{
    int wy = in_param.groupId.x;
    int w_base = wy * display_xsize;
    for (int i = 0; i < DISPLAY_XSIZE; i++)
    {
        g_game_cmap[w_base + i] = 0;
        g_game_primap[w_base + i] = 0;
        g_game_shadowmap[w_base + i] = 0;
    }
    int w_view_x = g_line_snap[wy].hscrollB;
    uint w_priority = 0;
    uint w_palette = 0;
    uint w_reverse = 0;
    uint w_char = 0;
    int w_view_addr = 0;
    int w_view_dx = 8;
    int w_view_dy = 0;
    int w_screen_addr = vdp_reg_4_scrollb;
    int w_pic_addr = 0;
    int w_out_addr = w_base;
    for (int wx = 0; wx < display_xsize; wx++)
    {
        if ((wx & scroll_mask) == 0)
        {
            int w_view_y = g_line_snap[wy].vscrollB[wx >> 4];
            w_view_dy = w_view_y & 7;
            w_view_addr = w_screen_addr + ((w_view_y >> 3) * scroll_xcell);
            w_view_dx = 8;
        }
        if (w_view_dx == 8)
        {
            w_view_x = (int)((uint) w_view_x % (uint) scroll_xsize);
            w_view_dx = w_view_x & 7;
            uint w_val = vram[w_view_addr + (w_view_x >> 3)];
            w_priority = ((w_val >> 15) & 0x0001);
            w_palette = (((w_val >> 13) & 0x0003) << 4);
            w_reverse = ((w_val >> 11) & 0x0003);
            w_char = (w_val & 0x07ff);
            w_pic_addr = (int) ((w_reverse * VRAM_DATASIZE) + (w_char << 4) + (w_view_dy << 1));
        }
        uint w_pic_w = vram[w_pic_addr + (w_view_dx >> 2)];
        uint w_pic = (w_pic_w >> ((3 - (w_view_dx & 3)) << 2)) & 0x0f;
        if (w_pic != 0)
        {
            g_game_cmap[w_out_addr] = w_palette + w_pic;
            g_game_primap[w_out_addr] = w_priority;
        }
        g_game_shadowmap[w_out_addr] = w_priority;
        w_view_x += 1;
        w_view_dx += 1;
        w_out_addr += 1;
    }

}
//rendering the scroll screenA
[numthreads(1, 1, 1)]
void CS_SCREENA(CSInput in_param)
{
    int wy = in_param.groupId.x;
    int w_base = wy * display_xsize;
    if ((screenA_bottom == 0)
                    || (wy < screenA_top)
                    || (screenA_bottom < wy))
    {
    }
    else
    {
        int w_view_x = g_line_snap[wy].hscrollA;
        uint w_priority = 0;
        uint w_palette = 0;
        uint w_reverse = 0;
        uint w_char = 0;
        int w_view_addr = 0;
        int w_view_dx = 8;
        int w_view_dy = 0;
        int w_screen_addr = vdp_reg_2_scrolla;
        int w_pic_addr = 0;
        int w_out_addr = w_base;
        for (int wx = 0; wx < display_xsize; wx++)
        {
            if ((wx & scroll_mask) == 0)
            {
                int w_view_y = g_line_snap[wy].vscrollA[wx >> 4];
                w_view_dy = w_view_y & 7;
                w_view_addr = w_screen_addr + ((w_view_y >> 3) * scroll_xcell);
                w_view_dx = 8;
            }
            if (w_view_dx == 8)
            {
                w_view_x = (int) ((uint) w_view_x % (uint) scroll_xsize);
                w_view_dx = w_view_x & 7;
                uint w_val = vram[w_view_addr + (w_view_x >> 3)];
                w_priority = ((w_val >> 15) & 0x0001);
                w_palette = (((w_val >> 13) & 0x0003) << 4);
                w_reverse = ((w_val >> 11) & 0x0003);
                w_char = (w_val & 0x07ff);
                w_pic_addr = (int) ((w_reverse * VRAM_DATASIZE) + (w_char << 4) + (w_view_dy << 1));
            }
            if (((screenA_right != 0)
                                && (screenA_left <= wx) && (wx <= screenA_right)
                                && (g_game_primap[w_out_addr] <= w_priority)))
            {
                uint w_pic_w = vram[w_pic_addr + (w_view_dx >> 2)];
                uint w_pic = (w_pic_w >> ((3 - (w_view_dx & 3)) << 2)) & 0x0f;
                if (w_pic != 0)
                {
                    g_game_cmap[w_out_addr] = w_palette + w_pic;
                    g_game_primap[w_out_addr] = w_priority;
                }
                g_game_shadowmap[w_out_addr] |= w_priority;
            }
            w_view_x += 1;
            w_view_dx += 1;
            w_out_addr += 1;
        }
    }
}
//rendering the sprite screen
[numthreads(1, 1, 1)]
void CS_SPRITE(CSInput in_param)
{
    int wy = in_param.groupId.x;
    int w_base = wy * display_xsize;
    for (int i = 0; i < g_line_snap[wy].sprite_rendrere_num; i++)
    {
        int w_sp = g_line_snap[wy].sprite_rendrere_num - i - 1;
        int w_left = g_line_snap[wy].sprite_left[w_sp];
        int w_right = g_line_snap[wy].sprite_right[w_sp];
        int w_top = g_line_snap[wy].sprite_top[w_sp];
        int w_bottom = g_line_snap[wy].sprite_bottom[w_sp];
        int w_xcell_size = g_line_snap[wy].sprite_xcell_size[w_sp];
        int w_ycell_size = g_line_snap[wy].sprite_ycell_size[w_sp];
        uint w_priority = g_line_snap[wy].sprite_priority[w_sp];
        uint w_palette = g_line_snap[wy].sprite_palette[w_sp];
        uint w_reverse = g_line_snap[wy].sprite_reverse[w_sp];
        uint w_reverse_addr = VRAM_DATASIZE * w_reverse;
        int w_char = (int) g_line_snap[wy].sprite_char[w_sp];
        int w_y = wy - w_top;
        int w_ycell = w_y >> 3;
        int w_cy = w_y & 7;
        int w_posx = w_left;
        int w_out_addr = w_base + w_left;
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
                if ((0 <= w_posx) && (w_posx < display_xsize))
                {
                    if (g_game_primap[w_out_addr] <= w_priority)
                    {
                        int w_num = (w_char_cur << 4) + (w_cy << 1) + (w_cx >> 2);
                        uint w_pic_w = vram[w_reverse_addr + w_num];
                        uint w_pic = (w_pic_w >> ((3 - (w_cx & 3)) << 2)) & 0x0f;

                        if (w_pic != 0)
                        {
                            uint w_color = (uint) (w_palette + w_pic);
                            if (vdp_reg_12_3_shadow == 0)
                            {
                                g_game_cmap[w_out_addr] = w_color;
                                g_game_primap[w_out_addr] = (uint) w_priority;
                            }
                            else if (w_color == 0x3e)
                            {
                                uint w_map = g_game_shadowmap[w_out_addr];
                                if (w_map < 2)
                                    g_game_shadowmap[w_out_addr] = (uint) (w_map + 1);
                            }
                            else if (w_color == 0x3f)
                            {
                                uint w_map = g_game_shadowmap[w_out_addr];
                                if (w_map > 0)
                                    g_game_shadowmap[w_out_addr] = (uint) (w_map - 1);
                            }
                            else if ((w_color & 0x0f) == 0x0e)
                            {
                                g_game_cmap[w_out_addr] = w_color;
                                g_game_primap[w_out_addr] = (uint) w_priority;
                                g_game_shadowmap[w_out_addr] = 0x1000;
                            }
                            else
                            {
                                g_game_cmap[w_out_addr] = w_color;
                                g_game_primap[w_out_addr] = (uint) w_priority;
                                g_game_shadowmap[w_out_addr] |= (uint) w_priority;
                            }
                        }
                    }
                }
                w_posx += 1;
                w_out_addr += 1;

            }
        }
    }
}
//rendering the window screen
[numthreads(1, 1, 1)]
void CS_WINDOW(CSInput in_param)
{

    int wy = in_param.groupId.x;
    int w_base = wy * display_xsize;
    int w_xcell_st = g_line_snap[wy].window_x_st;
    int w_xcell_ed = g_line_snap[wy].window_x_ed;
    if (w_xcell_st != w_xcell_ed)
    {
        int w_view_dy = wy & 7;
        int w_addr = vdp_reg_3_windows + ((wy >> 3) * scrollw_xcell) + w_xcell_st;
        int w_posx = screenA_left;
        int w_out_addr = w_base;
        for (int w_cx = w_xcell_st; w_cx <= w_xcell_ed; w_cx++)
        {
            uint w_val = vram[w_addr];
            w_addr += 1;
            uint w_priority = ((w_val >> 15) & 0x0001);
            uint w_palette = (((w_val >> 13) & 0x0003) << 4);
            uint w_reverse = ((w_val >> 11) & 0x0003) * VRAM_DATASIZE;
            uint w_char = (w_val & 0x07ff);
            for (int w_dx = 0; w_dx < 8; w_dx++)
            {
                if (g_game_primap[w_out_addr] <= w_priority)
                {
                    int w_pic_addr = (int) (w_reverse + (w_char << 4) + (w_view_dy << 1) + (w_dx >> 2));
                    uint w_pic_w = vram[w_pic_addr];
                    uint w_pic = (w_pic_w >> ((3 - (w_dx & 3)) << 2)) & 0x0f;
                    if (w_pic != 0)
                    {
                        g_game_cmap[w_out_addr] = w_palette + w_pic;
                        g_game_primap[w_out_addr] = w_priority;
                    }
                    g_game_shadowmap[w_out_addr] |= w_priority;
                }
                w_posx += 1;
                w_out_addr += 1;
            }
        }
    }
}

//rendering the game screen
[numthreads(1, 1, 1)]
void CS_FINAL(CSInput in_param)
{    
    int wy = in_param.groupId.x;
    int w_base = wy * display_xsize;
    uint color = 0;
    for (int wx = 0; wx <display_xsize; wx++)
    {
        uint w_colnum = g_game_cmap[w_base];
        if (w_colnum == 0) w_colnum = vdp_reg_7_backcolor;
        if (vdp_reg_12_3_shadow == 0)
        {
            color = g_color[w_colnum];
        }
        else
        {
            uint w_shadow = g_game_shadowmap[w_base];
            if (w_shadow == 0)
                color = g_color_shadow[w_colnum];
            else if (w_shadow == 2)
                color = g_color_highlight[w_colnum];
            else
                color = g_color[w_colnum];
        }  
        g_game_screen[w_base] = color;
        w_base += 1;

    }
}
