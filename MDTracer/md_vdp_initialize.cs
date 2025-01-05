using System.Xml.Linq;

namespace MDTracer
{
    internal partial class md_vdp
    {
        public void initialize()
        {
            g_vram = new byte[65536];
            g_cram = new ushort[64];
            g_vsram = new ushort[40];
            g_color = new uint[COLOR_MAX];
            g_color_shadow = new uint[COLOR_MAX];
            g_color_highlight = new uint[COLOR_MAX];

            g_pattern_chk = new bool[PATTERN_MAX];
            g_game_cmap = new uint[DISPLAY_BUFSIZE];
            g_game_primap = new uint[DISPLAY_BUFSIZE];
            g_game_shadowmap = new uint[DISPLAY_BUFSIZE];

            g_game_screen = new uint[DISPLAY_BUFSIZE];
            g_renderer_vram = new uint[VRAM_DATASIZE * 4];

            g_snap_register = new VDP_REGISTER();
            g_line_snap = new VDP_LINE_SNAP[DISPLAY_YSIZE];
            for(int i = 0; i < DISPLAY_YSIZE; i++)
            {
                g_line_snap[i].vscrollA = new int[VSRAM_DATASIZE];
                g_line_snap[i].vscrollB = new int[VSRAM_DATASIZE];
                g_line_snap[i].sprite_left = new int[MAX_SPRITE];
                g_line_snap[i].sprite_right = new int[MAX_SPRITE];
                g_line_snap[i].sprite_top = new int[MAX_SPRITE];
                g_line_snap[i].sprite_bottom = new int[MAX_SPRITE];
                g_line_snap[i].sprite_xcell_size = new int[MAX_SPRITE];
                g_line_snap[i].sprite_ycell_size = new int[MAX_SPRITE];
                g_line_snap[i].sprite_priority = new uint[MAX_SPRITE];
                g_line_snap[i].sprite_palette = new uint[MAX_SPRITE];
                g_line_snap[i].sprite_reverse = new uint[MAX_SPRITE];
                g_line_snap[i].sprite_char = new uint[MAX_SPRITE];
            }
            g_snap_line_snap = new VDP_LINE_SNAP[DISPLAY_YSIZE];
            for (int i = 0; i < DISPLAY_YSIZE; i++)
            {
                g_snap_line_snap[i].vscrollA = new int[VSRAM_DATASIZE];
                g_snap_line_snap[i].vscrollB = new int[VSRAM_DATASIZE];
                g_snap_line_snap[i].sprite_left = new int[MAX_SPRITE];
                g_snap_line_snap[i].sprite_right = new int[MAX_SPRITE];
                g_snap_line_snap[i].sprite_top = new int[MAX_SPRITE];
                g_snap_line_snap[i].sprite_bottom = new int[MAX_SPRITE];
                g_snap_line_snap[i].sprite_xcell_size = new int[MAX_SPRITE];
                g_snap_line_snap[i].sprite_ycell_size = new int[MAX_SPRITE];
                g_snap_line_snap[i].sprite_priority = new uint[MAX_SPRITE];
                g_snap_line_snap[i].sprite_palette = new uint[MAX_SPRITE];
                g_snap_line_snap[i].sprite_reverse = new uint[MAX_SPRITE];
                g_snap_line_snap[i].sprite_char = new uint[MAX_SPRITE];
            }

            g_snap_renderer_vram = new uint[VRAM_DATASIZE * 4];
            g_snap_color = new uint[64];
            g_snap_color_shadow = new uint[64];
            g_snap_color_highlight = new uint[64];

            g_scrollA_bitmap = new Bitmap(1024, 1024);
            g_scrollB_bitmap = new Bitmap(1024, 1024);
            g_scrollW_bitmap = new Bitmap(1024, 1024);
            g_scrollS_bitmap = new Bitmap(512, 512);
            g_pattern_table = new Bitmap(128, 1024);
            g_sprite_enable = new bool[80];
            MONOCOLOR_TABLE = new uint[16];
            for (uint i = 0; i <= 15; i++)
            {
                uint w_clor = i << 4;
                MONOCOLOR_TABLE[i] = (uint)(0xff000000
                                        | w_clor << 16
                                        | w_clor << 8
                                        | w_clor);
            }
            g_display_xsize = 320;
            g_display_ysize = 224;
            g_scroll_xcell = 32;
            g_scroll_ycell = 32;
            g_scroll_xsize = 256;
            g_scroll_ysize = 256;
            g_scroll_xsize_mask = 0x00ff;
            g_scroll_ysize_mask = 0x00ff;
            g_vertical_line_max = 262;

            //VDP control port
            g_vdp_status_9_empl = 1;            //const
            g_vdp_status_8_full = 0;            //const
            g_vdp_status_7_vinterrupt = 0;
            g_vdp_status_6_sprite = 0;
            g_vdp_status_5_collision = 0;
            g_vdp_status_4_frame = 0;
            g_vdp_status_3_vbrank = 0;
            g_vdp_status_2_hbrank = 0;      //const
            g_vdp_status_1_dma = 0;             //const
            g_vdp_status_0_tvmode = 0;

            g_vdp_reg = new byte[24];
            g_vdp_reg_2_scrolla = 0xffff;
            g_vdp_reg_3_windows = 0xffff;
            g_vdp_reg_4_scrollb = 0xffff;

            g_scanline = 0;
            g_hinterrupt_counter = -1;
        }
    }
}
