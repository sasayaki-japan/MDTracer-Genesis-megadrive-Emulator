using System;
using System.Diagnostics;

namespace MDTracer
{
    internal partial class md_vdp
    {
        private byte[] g_vram;
        private ushort[] g_cram;
        public ushort[] g_vsram;
        public uint[] g_color;
        public uint[] g_color_shadow;
        public uint[] g_color_highlight;
        private int g_vdp_reg_code;
        private ushort g_vdp_reg_dest_address;

        //work
        private bool g_command_select;
        private ushort g_command_word;

        private int[] COLOR_NORMAL = { 0, 52, 87, 116, 144, 172, 206, 255 };
        private int[] COLOR_SHADOW = { 0, 29, 52, 70, 87, 101, 116, 130 };
        private int[] COLOR_HIGHLIGHT = { 130, 144, 158, 172, 187, 206, 228, 255 };

        //----------------------------------------------------------------
        //read
        //----------------------------------------------------------------
        public byte read8(uint in_address)
        {
            byte w_out = 0;
            ushort w_data = read16(in_address);
            if ((in_address & 1) == 0)
            {
                w_out = (byte)(w_data >> 8);
            }
            else
            {
                w_out = (byte)(w_data & 0xff);
            }
            return w_out;
        }
        public ushort read16(uint in_address)
        {
            ushort w_out = 0;
            in_address &= 0xfffffe;
            if (in_address <= 0xc00003)
            {
                g_command_select = false;
                switch (g_vdp_reg_code)
                {
                    case 0:
                        w_out = vram_read_w(g_vdp_reg_dest_address);
                        break;
                    case 8:
                        w_out = g_cram[(g_vdp_reg_dest_address >> 1) & 0x3f];
                        break;
                    case 4:
                        w_out = g_vsram[(g_vdp_reg_dest_address >> 1) % 40];
                        break;
                    default:
                        MessageBox.Show("read16_c0000", "error");
                        break;
                }
                g_vdp_reg_dest_address = (ushort)(g_vdp_reg_dest_address + g_vdp_reg_15_autoinc);
            }
            else
            if (in_address <= 0xc00007)
            {
                g_command_select = false;
                w_out = get_vdp_status();
            }
            else
            if (in_address <= 0xc0000e)
            {
                w_out = get_vdp_hvcounter();
            }
            else
            {
                MessageBox.Show("md_vdp.read16", "error");
            }
            return w_out;
        }
        public uint read32(uint in_address)
        {
            uint w_out = 0;
            if (in_address <= 0xc00003)
            {
                w_out = (uint)((read16(in_address) << 16) + read16(in_address));
            }
            else
            {
                MessageBox.Show("md_vdp.read32", "error");
            }
            return w_out;
        }
        //----------------------------------------------------------------
        //write
        //----------------------------------------------------------------
        public void write8(uint in_address, byte in_data)
        {
            ushort w_data = (ushort)((in_data << 8) + in_data);
            write16(in_address, w_data);
        }
        public void write16(uint in_address, ushort in_data)
        {
            in_address &= 0xfffffe;
            if (in_address <= 0xc00003)
            {
                g_command_select = false;
                if (g_dma_fill_req == true)
                {
                    g_dma_fill_req = false;
                    dma_run_fill_req(in_data);
                }
                else
                {
                    switch (g_vdp_reg_code & 0x0f)
                    {
                        case 1:
                            vram_write_w(g_vdp_reg_dest_address, in_data);
                            pattern_chk(g_vdp_reg_dest_address, (byte)(in_data >> 8));
                            pattern_chk(g_vdp_reg_dest_address + 1, (byte)(in_data & 0xff));
                            g_vdp_reg_dest_address = (ushort)((g_vdp_reg_dest_address + g_vdp_reg_15_autoinc) & 0xffff);
                            break;
                        case 3:
                            int wcol_num = (int)((g_vdp_reg_dest_address >> 1) & 0x3f);
                            cram_set(wcol_num, in_data);
                            g_vdp_reg_dest_address = (ushort)((g_vdp_reg_dest_address + g_vdp_reg_15_autoinc) & 0xffff);
                            break;
                        case 5:
                            if (g_vdp_reg_dest_address < 80)
                            {
                                g_vsram[(g_vdp_reg_dest_address >> 1)] = in_data;
                                g_vdp_reg_dest_address = (ushort)((g_vdp_reg_dest_address + g_vdp_reg_15_autoinc) & 0xffff);
                            }
                            break;
                        default:
                            //MessageBox.Show("write16_c0000", "error");
                            break;
                    }
                }
            }
            else
            if (in_address <= 0xc00007)
            {
                if (g_command_select == false)
                {
                    if ((in_data & 0xc000) == 0x8000)
                    {
                        byte w_rs = (byte)((in_data >> 8) & 0x001f);
                        byte w_data = (byte)(in_data & 0x00ff);
                        set_vdp_register(w_rs, w_data);
                    }
                    else
                    {
                        //address set 1st
                        g_command_select = true;
                        g_command_word = in_data;
                    }
                }
                else
                {
                    //address set 2nd
                    g_command_select = false;
                    g_vdp_reg_code = (int)((g_command_word >> 14) | ((in_data >> 2) & 0x3c));
                    g_vdp_reg_dest_address = (ushort)((g_command_word & 0x3fff) | ((in_data & 0x0003) << 14));
                    if ((g_vdp_reg_code & 0x20) == 0x20)
                    {
                        if (g_vdp_reg_1_4_dma == 1)
                        {
                            switch (g_vdp_reg_23_dma_mode)
                            {
                                case 0:
                                case 1:
                                    dma_run_memory_req();
                                    break;
                                case 2:
                                    g_dma_fill_req = true;
                                    break;
                                case 3:
                                    dma_run_copy_req();
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("md_vdp.write16", "error");
            }
        }
        public void write32(uint in_address, uint in_data)
        {
            if (in_address <= 0xc00003)
            {
                write16(in_address, (ushort)(in_data >> 16));
                write16(in_address, (ushort)(in_data & 0xffff));
            }
            else
            if (in_address <= 0xc00007)
            {
                write16(in_address, (ushort)(in_data >> 16));
                write16(in_address, (ushort)(in_data & 0xffff));
            }
            else
            {
                MessageBox.Show("md_vdp.write32", "error");
            }
        }
        //----------------------------------------------------------------
        //sub
        //----------------------------------------------------------------
        private ushort vram_read_w(int in_addr)
        {
            return (ushort)((g_vram[in_addr] << 8) + g_vram[in_addr + 1]);
        }
        private void vram_write_w(int in_addr, ushort in_data)
        {
            g_vram[in_addr] = (byte)(in_data >> 8);
            g_vram[(in_addr ^ 1) & 0xffff] = (byte)(in_data & 0xff);
        }
        private void cram_set(int in_num, ushort in_data)
        {
            g_cram[in_num] = in_data;
            int w_r = (in_data & 0x000e) >> 1;
            int w_g = (in_data & 0x00e0) >> 5;
            int w_b = (in_data & 0x0e00) >> 9;
            g_color[in_num] = (uint)(0xff000000
                                        | (uint)(COLOR_NORMAL[w_r] << 16)
                                        | (uint)(COLOR_NORMAL[w_g] << 8)
                                        | (uint)(COLOR_NORMAL[w_b]));
            g_color_shadow[in_num] = (uint)(0xff000000
                                        | (uint)(COLOR_SHADOW[w_r] << 16)
                                        | (uint)(COLOR_SHADOW[w_g] << 8)
                                        | (uint)(COLOR_SHADOW[w_b]));
            g_color_highlight[in_num] = (uint)(0xff000000
                                        | (uint)(COLOR_HIGHLIGHT[w_r] << 16)
                                        | (uint)(COLOR_HIGHLIGHT[w_g] << 8)
                                        | (uint)(COLOR_HIGHLIGHT[w_b]));
        }

        private void pattern_chk(int in_address, byte in_val)
        {
            int w_address = in_address & 0xfffe;
            uint w_val = vram_read_w(w_address);
            {
                uint w_val_h = ((w_val >> 12) & 0x000f)
                                + ((w_val >> 4) & 0x00f0)
                                + ((w_val << 4) & 0x0f00)
                                + ((w_val << 12) & 0xf000);
                int w_char = (in_address & 0xffe0) >> 5;
                int w_addr = (in_address & 0xffe0) >> 1;
                int wx = (in_address & 0x0002) >> 1;
                int wy = (in_address & 0x001f) >> 2;
                g_renderer_vram[w_address >> 1] = w_val;
                if (wx == 0)
                {
                    g_renderer_vram[VRAM_DATASIZE + w_addr + (wy << 1) + 1] = w_val_h;
                    g_renderer_vram[(VRAM_DATASIZE * 2) + w_addr + ((7 - wy) << 1)] = w_val;
                    g_renderer_vram[(VRAM_DATASIZE * 3) + w_addr + ((7 - wy) << 1) + 1] = w_val_h;
                }
                else
                {
                    g_renderer_vram[VRAM_DATASIZE + w_addr + (wy << 1)] = w_val_h;
                    g_renderer_vram[(VRAM_DATASIZE * 2) + w_addr + ((7 - wy) << 1) + 1] = w_val;
                    g_renderer_vram[(VRAM_DATASIZE * 3) + w_addr + ((7 - wy) << 1)] = w_val_h;
                }
                g_pattern_chk[w_char] = true;
            }
        }
    }
}
