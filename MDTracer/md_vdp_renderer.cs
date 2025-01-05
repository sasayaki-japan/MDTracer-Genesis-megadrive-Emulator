using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using static MDTracer.md_vdp;

namespace MDTracer
{
    internal partial class md_vdp
    {
        private const int PATTERN_MAX = 2048;
        private const int DISPLAY_XSIZE = 320;
        private const int DISPLAY_YSIZE = 240;
        private const int DISPLAY_BUFSIZE = DISPLAY_XSIZE * DISPLAY_YSIZE;
        public int SPRITE_XSIZE = 512;
        public int SPRITE_YSIZE = 512;
        private const int VRAM_DATASIZE = 65536 / 2;
        private const int VSRAM_DATASIZE = 20;
        private const int COLOR_MAX = 64;
        private const int MAX_SPRITE = 20;

        private bool[] g_pattern_chk;
        private uint[] g_renderer_vram;
        private VDP_LINE_SNAP[] g_line_snap;
        private uint[] g_game_cmap;
        private uint[] g_game_primap;
        private uint[] g_game_shadowmap;

        private VDP_REGISTER g_snap_register;
        private uint[] g_snap_renderer_vram;
        private VDP_LINE_SNAP[] g_snap_line_snap;
        private uint[] g_snap_color;
        private uint[] g_snap_color_shadow;
        private uint[] g_snap_color_highlight;

        public uint[] g_game_screen;

        public int g_display_xsize;
        public int g_display_ysize;
        private int g_display_xcell;
        private int g_display_ycell;
        private int g_scroll_xcell;
        private int g_scroll_ycell;
        public int g_scroll_xsize;
        public int g_scroll_ysize;
        private int g_scroll_xsize_mask;
        private int g_scroll_ysize_mask;
        public int g_vertical_line_max;

        private int g_screenA_left_x;
        private int g_screenA_right_x;
        private int g_screenA_top_y;
        private int g_screenA_bottom_y;

        private int g_max_sprite_num;
        private int g_max_sprite_line;
        private int g_max_sprite_cell;
        private int g_sprite_vmask;

        public bool rendering_gpu;
        public ManualResetEvent g_waitHandle;

        private void rendering_line()
        {
            if (g_vdp_reg_1_6_display == 1)
            {
                rendering_line_snap();
                if (rendering_gpu == false)
                {
                    rendering_line_cpu();
                }
            }
            else
            {
                if (rendering_gpu == false)
                {
                    int w_pos = g_scanline * g_display_xsize;
                    for (int wx = 0; wx < g_display_xsize; wx++)
                    {
                        g_game_screen[w_pos] = 0;
                        w_pos += 1;
                    }
                }
            }
        }
        private void rendering_frame()
        {
            if (g_waitHandle.WaitOne(0) == false)
            {
                if (rendering_gpu == true)
                {
                    dx_frame_stack();
                }
                rendering_frame_snap();
                g_waitHandle.Set();
            }
        }
        public void run_event()
        {
            while (true)
            {
                g_waitHandle.WaitOne(Timeout.Infinite);
                g_waitHandle.Reset();
                if (rendering_gpu == true)
                {
                    if (g_snap_register.vdp_reg_1_6_display == 1)
                    {
                        dx_rendering();
                        dx_get_screen_data();
                    }
                    else
                    {
                        for (int w_pos = 0; w_pos < DISPLAY_BUFSIZE; w_pos++)
                        {
                            g_game_screen[w_pos] = 0;
                        }
                    }
                }
                md_main.Screen_Game_Update();
                rendering_data();
                md_main.Screen_Update();
            }
        }
    }
}