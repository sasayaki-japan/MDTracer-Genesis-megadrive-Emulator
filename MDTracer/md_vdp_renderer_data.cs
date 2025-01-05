using SharpDX;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Runtime.InteropServices;

namespace MDTracer
{
    internal partial class md_vdp
    {
        public Bitmap g_scrollA_bitmap;
        public Bitmap g_scrollB_bitmap;
        public Bitmap g_scrollW_bitmap;
        public Bitmap g_scrollS_bitmap;
        public Bitmap g_pattern_table;
        private uint[] MONOCOLOR_TABLE;
        private bool[] g_sprite_enable;

        //-----------------------------------------------------
        private void rendering_data()
        {
            const int bytesPerPixel = 4;

            //pattern make
            BitmapData pattern_bmpData = g_pattern_table.LockBits(new Rectangle(0, 0, g_pattern_table.Width, g_pattern_table.Height),
                                                    ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr dest_ptr = pattern_bmpData.Scan0;
            int dest_stride = pattern_bmpData.Stride;
            unsafe
            {
                byte* pixels = (byte*)dest_ptr;
                for (int w_char = 0; w_char < PATTERN_MAX; w_char++)
                {
                    if (g_pattern_chk[w_char] == true)
                    {
                        g_pattern_chk[w_char] = false;
                        int wx = (w_char & 0x0f) << 3;
                        int wy = (w_char & 0xfff0) >> 1;
                        int pixelOffset1 = (wy * dest_stride) + (wx * bytesPerPixel);
                        int w_pic_addr = w_char << 4;
                        for (int dy = 0; dy < 8; dy++)
                        {
                            int pixelOffset2 = pixelOffset1;
                            for (int dx = 0; dx < 8; dx++)
                            {
                                uint w_pic_w = g_snap_renderer_vram[w_pic_addr + (dy << 1) + (dx >> 2)];
                                uint w_pic = (w_pic_w >> ((3 - (dx & 3)) << 2)) & 0x0f;
                                uint w_color = MONOCOLOR_TABLE[w_pic];
                                uint* pixel2 = (uint*)(pixels + pixelOffset2);
                                *pixel2 = w_color;
                                pixelOffset2 += bytesPerPixel;
                            }
                            pixelOffset1 += dest_stride;
                        }
                    }
                }
            }
            g_pattern_table.UnlockBits(pattern_bmpData);
            //scrollA make
            BitmapData scrollA_bmpData = g_scrollA_bitmap.LockBits(new Rectangle(0, 0, g_scrollA_bitmap.Width, g_scrollA_bitmap.Height),
                                                    ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            dest_ptr = scrollA_bmpData.Scan0;
            dest_stride = scrollA_bmpData.Stride;
            unsafe
            {
                byte* pixels = (byte*)dest_ptr;
                int w_num = g_vdp_reg_2_scrolla >> 1;
                int pixelOffset1 = 0;
                for (int wy = 0; wy < g_scroll_ycell; wy++)
                {
                    int pixelOffset2 = pixelOffset1;
                    for (int wx = 0; wx < g_scroll_xcell; wx++)
                    {
                        int pixelOffset3 = pixelOffset2;
                        uint w_val = g_snap_renderer_vram[w_num];
                        uint w_priority = ((w_val >> 15) & 0x0001);
                        uint w_palette = (((w_val >> 13) & 0x0003) << 4);
                        uint w_reverse = ((w_val >> 11) & 0x0003);
                        uint w_char = (w_val & 0x07ff);
                        int w_pic_addr = (int)((w_reverse * VRAM_DATASIZE) + (w_char << 4));
                        w_num += 1;
                        for (int dy = 0; dy < 8; dy++)
                        {
                            int pixelOffset4 = pixelOffset3;
                            for (int dx = 0; dx < 8; dx++)
                            {
                                uint w_pic_w = g_snap_renderer_vram[w_pic_addr + (dy << 1) + (dx >> 2)];
                                uint w_pic = (w_pic_w >> ((3 - (dx & 3)) << 2)) & 0x0f;
                                uint color = g_snap_color[w_palette + w_pic];
                                uint* pixel2 = (uint*)(pixels + pixelOffset4);
                                *pixel2 = color;
                                pixelOffset4 += bytesPerPixel;
                            }
                            pixelOffset3 += dest_stride;
                        }
                        pixelOffset2 += (bytesPerPixel << 3);
                    }
                    pixelOffset1 += (dest_stride << 3);
                }
            }
            g_scrollA_bitmap.UnlockBits(scrollA_bmpData);
            //scrollB make
            BitmapData scrollB_bmpData = g_scrollB_bitmap.LockBits(new Rectangle(0, 0, g_scrollB_bitmap.Width, g_scrollB_bitmap.Height),
                                        ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            dest_ptr = scrollB_bmpData.Scan0;
            dest_stride = scrollB_bmpData.Stride;
            unsafe
            {
                byte* pixels = (byte*)dest_ptr;
                int w_num = g_vdp_reg_4_scrollb >> 1;
                int pixelOffset1 = 0;
                for (int wy = 0; wy < g_scroll_ycell; wy++)
                {
                    int pixelOffset2 = pixelOffset1;
                    for (int wx = 0; wx < g_scroll_xcell; wx++)
                    {
                        int pixelOffset3 = pixelOffset2;
                        uint w_val = g_snap_renderer_vram[w_num];
                        uint w_priority = ((w_val >> 15) & 0x0001);
                        uint w_palette = (((w_val >> 13) & 0x0003) << 4);
                        uint w_reverse = ((w_val >> 11) & 0x0003);
                        uint w_char = (w_val & 0x07ff);
                        int w_pic_addr = (int)((w_reverse * VRAM_DATASIZE) + (w_char << 4));
                        w_num += 1;
                        for (int dy = 0; dy < 8; dy++)
                        {
                            int pixelOffset4 = pixelOffset3;
                            for (int dx = 0; dx < 8; dx++)
                            {
                                uint w_pic_w = g_snap_renderer_vram[w_pic_addr + (dy << 1) +(dx >> 2)];
                                uint w_pic = (w_pic_w >> ((3 - (dx & 3)) << 2)) & 0x0f;
                                uint color = g_snap_color[w_palette + w_pic];
                                uint* pixel2 = (uint*)(pixels + pixelOffset4);
                                *pixel2 = color;
                                pixelOffset4 += bytesPerPixel;
                            }
                            pixelOffset3 += dest_stride;
                        }
                        pixelOffset2 += (bytesPerPixel << 3);
                    }
                    pixelOffset1 += (dest_stride << 3);
                }
            }
            g_scrollB_bitmap.UnlockBits(scrollB_bmpData);


            //window make
            BitmapData scrollW_bmpData = g_scrollW_bitmap.LockBits(new Rectangle(0, 0, g_scrollW_bitmap.Width, g_scrollW_bitmap.Height),
                                        ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            dest_ptr = scrollW_bmpData.Scan0;
            dest_stride = scrollW_bmpData.Stride;
            unsafe
            {
                byte* pixels = (byte*)dest_ptr;
                int pixelOffset1 = 0;
                for (int wy = 0; wy < g_scroll_ycell; wy++)
                {
                    int w_num = (g_vdp_reg_3_windows >> 1) + (wy * g_scroll_xcell);
                    int pixelOffset2 = pixelOffset1;
                    for (int wx = 0; wx < g_scroll_xcell; wx++)
                    {
                        int pixelOffset3 = pixelOffset2;
                        uint w_val = g_snap_renderer_vram[w_num];
                        uint w_priority = ((w_val >> 15) & 0x0001);
                        uint w_palette = (((w_val >> 13) & 0x0003) << 4);
                        uint w_reverse = ((w_val >> 11) & 0x0003);
                        uint w_char = (w_val & 0x07ff);
                        int w_pic_addr = (int)((w_reverse * VRAM_DATASIZE) + (w_char << 4));
                        w_num += 1;
                        for (int dy = 0; dy < 8; dy++)
                        {
                            int pixelOffset4 = pixelOffset3;
                            for (int dx = 0; dx < 8; dx++)
                            {
                                uint w_pic_w = g_snap_renderer_vram[w_pic_addr + (dy << 1) + (dx >> 2)];
                                uint w_pic = (w_pic_w >> ((3 - (dx & 3)) << 2)) & 0x0f;
                                uint color = g_snap_color[w_palette + w_pic];
                                uint* pixel2 = (uint*)(pixels + pixelOffset4);
                                *pixel2 = color;

                                pixelOffset4 += bytesPerPixel;
                            }
                            pixelOffset3 += dest_stride;
                        }
                        pixelOffset2 += (bytesPerPixel << 3);
                    }
                    pixelOffset1 += (dest_stride << 3);
                }
            }
            g_scrollW_bitmap.UnlockBits(scrollW_bmpData);

            //rendering the sprite screen

            for (int i = 0; i < g_max_sprite_num; i++)
            {
                g_sprite_enable[i] = false;
            }
            int w_link = 0;
            for (int i = 0; i < g_max_sprite_num; i++)
            {
                int w_addr = (g_vdp_reg_5_sprite >> 1) + (w_link << 2);
                ushort w_val2 = (ushort)g_snap_renderer_vram[w_addr + 1];
                w_link = w_val2 & 0x007f;
                if (w_link >= g_max_sprite_num) break;
                g_sprite_enable[w_link] = true;
            }
            BitmapData scrollS_bmpData = g_scrollS_bitmap.LockBits(new Rectangle(0, 0, g_scrollS_bitmap.Width, g_scrollS_bitmap.Height),
                    ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            dest_ptr = scrollS_bmpData.Scan0;
            dest_stride = scrollS_bmpData.Stride;
            unsafe
            {
                ulong* c_pixels = (ulong*)dest_ptr;
                for (int i = 0; i < 512 * 256; i++)
                {
                    *c_pixels = 0;
                    c_pixels += 1;
                }
                byte* pixels = (byte*)dest_ptr;
                for (int i = g_max_sprite_num - 1; i >= 0; i--)
                {
                    int w_addr = (g_vdp_reg_5_sprite >> 1) + (i << 2);
                    ushort w_val1 = (ushort)g_snap_renderer_vram[w_addr];
                    ushort w_val2 = (ushort)g_snap_renderer_vram[w_addr + 1];
                    ushort w_val3 = (ushort)g_snap_renderer_vram[w_addr + 2];
                    ushort w_val4 = (ushort)g_snap_renderer_vram[w_addr + 3];
                    int w_pos_x = w_val4 & 0x01ff;
                    int w_pos_y = w_val1 & g_sprite_vmask;
                    int w_xcell_size = ((w_val2 >> 10) & 0x0003) + 1;
                    int w_ycell_size = ((w_val2 >> 8) & 0x0003) + 1;
                    int w_pic_size_x = w_xcell_size << 3;
                    int w_pic_size_y = w_ycell_size << 3;
                    int w_priority = ((w_val3 >> 15) & 0x0001) << 2;
                    int w_palette = ((w_val3 >> 13) & 0x0003) << 4;
                    int w_reverse = ((w_val3 >> 11) & 0x0003);
                    int w_char = w_val3 & 0x07ff;

                    for (int cy = 0; cy < w_ycell_size; cy++)
                    {
                        for (int cx = 0; cx < w_xcell_size; cx++)
                        {
                            int w_char_cur = 0;
                            switch (w_reverse)
                            {
                                case 0:
                                    w_char_cur = w_char + (w_ycell_size * cx) + cy;
                                    break;
                                case 1:
                                    w_char_cur = w_char + (w_ycell_size * (w_xcell_size - cx - 1)) + cy;
                                    break;
                                case 2:
                                    w_char_cur = w_char + (w_ycell_size * cx) + (w_ycell_size - cy - 1);
                                    break;
                                default:
                                    w_char_cur = w_char + (w_ycell_size * (w_xcell_size - cx - 1)) + (w_ycell_size - cy - 1);
                                    break;
                            }
                            if(w_char_cur <= 0x7ff)
                            {
                                int w_pic_addr = (int)((w_reverse * VRAM_DATASIZE) + (w_char_cur << 4));
                                int wx = w_pos_x + (cx * 8);
                                int wy = w_pos_y + (cy * 8);
                                int pixelOffset3 = (dest_stride * wy) + (bytesPerPixel * wx);
                                for (int dy = 0; dy < 8; dy++)
                                {
                                    int pixelOffset4 = pixelOffset3;
                                    for (int dx = 0; dx < 8; dx++)
                                    {
                                        uint w_pic_w = g_snap_renderer_vram[w_pic_addr + (dy << 1) + (dx >> 2)];
                                        uint w_pic = (w_pic_w >> ((3 - (dx & 3)) << 2)) & 0x0f;
                                        uint color = g_snap_color[w_palette + w_pic];

                                        if (((wy + dy) < 512) && ((wx + dx) < 512))
                                        {
                                            uint* pixel2 = (uint*)(pixels + pixelOffset4);
                                            *pixel2 = color;
                                            pixelOffset4 += bytesPerPixel;
                                        }
                                    }
                                    pixelOffset3 += dest_stride;
                                }
                            }
                        }
                    }
                    uint w_color = 0;
                    if (g_sprite_enable[i] == true)
                    {
                        w_color = 0xffffff00;
                    }
                    else
                    {
                        w_color = 0xff0000ff;
                    }
                    if (w_pos_y < 512)
                    {
                        for (int dx = 0; dx < w_pic_size_x; dx++)
                        {
                            int wx = w_pos_x + dx;
                            if (wx < 512)
                            {
                                uint* pixel2 = (uint*)(pixels
                                    + (dest_stride * w_pos_y)
                                    + (bytesPerPixel * wx)
                                    );
                                *pixel2 = w_color;
                                if ((w_pos_y + w_pic_size_y - 1) < 512)
                                {
                                    pixel2 = (uint*)(pixels
                                    + (dest_stride * (w_pos_y + w_pic_size_y - 1))
                                    + (bytesPerPixel * wx)
                                    );
                                    *pixel2 = w_color;
                                }
                            }
                        }
                    }
                    if (w_pos_x < 512)
                    {
                        for (int dy = 0; dy < w_pic_size_y; dy++)
                        {
                            int wy = w_pos_y + dy;
                            if (wy < 512)
                            {
                                uint* pixel2 = (uint*)(pixels
                                    + (dest_stride * wy)
                                    + (bytesPerPixel * w_pos_x)
                                    );
                                *pixel2 = w_color;
                                if ((w_pos_x + w_pic_size_x - 1) < 512)
                                {
                                    pixel2 = (uint*)(pixels
                                    + (dest_stride * wy)
                                    + (bytesPerPixel * (w_pos_x + w_pic_size_x - 1))
                                    );
                                    *pixel2 = w_color;
                                }
                            }
                        }
                    }
                    for (int dx = 0; dx < g_display_xsize; dx++)
                    {
                        int wx = dx + 128;
                        uint* pixel2 = (uint*)(pixels
                            + (dest_stride * 128)
                            + (bytesPerPixel * wx)
                            );
                        *pixel2 = 0xff00ff00;
                        pixel2 = (uint*)(pixels
                            + (dest_stride * (127 + g_display_ysize))
                            + (bytesPerPixel * wx)
                            );
                        *pixel2 = 0xff00ff00;
                    }
                    for (int dy = 0; dy < g_display_ysize; dy++)
                    {
                        int wy = dy + 128;
                        uint* pixel2 = (uint*)(pixels
                            + (dest_stride * wy)
                            + (bytesPerPixel * 128)
                            );
                        *pixel2 = 0xff00ff00;
                        pixel2 = (uint*)(pixels
                            + (dest_stride * wy)
                            + (bytesPerPixel * (127 + g_display_xsize))
                            );
                        *pixel2 = 0xff00ff00;
                    }
                }
            }
            g_scrollS_bitmap.UnlockBits(scrollS_bmpData);

            md_main.Screen_Update();
        }
    }
}
