namespace MDTracer
{
    //----------------------------------------------------------------
    //VDP : chips:315-5313
    //----------------------------------------------------------------
    internal partial class md_vdp
    {
        public int g_scanline;
        private int g_hinterrupt_counter;
        //----------------------------------------------------------------
        public md_vdp()
        {
            initialize();
            dx_rendering_initialize();
        }
        public void run(int in_vline)
        {
            g_scanline = in_vline;
            if (g_scanline == 0)
            {
                rendering_line();
                set_hinterrupt();
                interrupt_check();
            }
            else
            if (g_scanline < g_display_ysize)
            {
                rendering_line();
                interrupt_check();
            }
            else
            if (g_scanline == g_display_ysize)
            {
                rendering_frame();
                interrupt_check();
                g_vdp_status_3_vbrank = 1;
                md_main.g_md_m68k.g_interrupt_V_req = true;
                md_main.g_md_vdp.g_vdp_status_7_vinterrupt = 1;
                md_main.g_md_z80.irq_request(true);
            }
            else
            if (g_scanline == g_vertical_line_max - 1)
            {
                g_vdp_status_3_vbrank = 0;
                g_vdp_status_4_frame = (byte)((g_vdp_status_4_frame == 0) ? 1 : 0);
                g_vdp_status_5_collision = 0;
                g_vdp_status_6_sprite = 0;
            }
        }
        private void set_hvcounter()
        {
            if (g_vdp_reg_12_2_interlacemode == 0)
            {
                g_vdp_c00008_hvcounter = (ushort)(((Form_Main.g_mouseclick_pos_x >> 1) & 0x00ff)
                                            + (Form_Main.g_mouseclick_pos_y << 8));
            }
            else
            {
                g_vdp_c00008_hvcounter = (ushort)(((Form_Main.g_mouseclick_pos_x >> 1) & 0x00ff)
                                            + ((Form_Main.g_mouseclick_pos_y << 8) & 0xfe00)
                                            + (Form_Main.g_mouseclick_pos_y & 0x0100));
            }
        }
        private void set_hinterrupt()
        {
            g_hinterrupt_counter = g_vdp_reg_10_hint;
        }
        private void interrupt_check()
        {
            g_hinterrupt_counter -= 1;
            if (g_hinterrupt_counter < 0)
            {
                md_main.g_md_m68k.g_interrupt_H_req = true;
                set_hinterrupt();
            }

            if (Form_Main.g_mouseclick_interrupt == true)
            {
                if (g_vdp_reg_11_3_ext == 1)
                {
                    md_main.g_md_m68k.g_interrupt_EXT_req = true;
                    if ((g_vdp_reg_0_1_hvcounter == 1) && (g_vdp_c00008_hvcounter_latched == false))
                    {
                        Form_Main.g_mouseclick_interrupt = true;
                        set_hvcounter();
                    }
                    else
                    {
                        g_vdp_c00008_hvcounter_latched = false;
                    }
                }
                else
                {
                    Form_Main.g_mouseclick_interrupt = false;
                }
            }
        }
    }
}
