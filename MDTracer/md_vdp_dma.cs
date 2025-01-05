using System.Diagnostics;
using System.Xml.Linq;

namespace MDTracer
{
    internal partial class md_vdp
    {
        private int g_dma_mode;
        private uint g_dma_src_addr;
        private int g_dma_leng;
        private bool g_dma_fill_req;
        private ushort g_dma_fill_data;

        public int dma_status_update()
        {
            int w_clock = 0;
            int w_tran = 0;
            if(0 < g_dma_leng)
            {
                switch (g_dma_mode)
                {
                    case 1:
                        w_tran = (g_vdp_status_3_vbrank == 0) ? 18 : 205;
                        w_clock = 488;
                        break;
                    case 2:
                        w_tran = (g_vdp_status_3_vbrank == 0) ? 17 : 204;
                        break;
                    case 3:
                        w_tran = (g_vdp_status_3_vbrank == 0) ? 9 : 102;
                        break;
                }
                g_dma_leng -= w_tran;
                if (g_dma_leng <= 0)
                {
                    g_dma_mode = 0;
                    g_dma_leng = 0;
                    g_vdp_status_1_dma = 0;
                    g_vdp_status_8_full = 0;
                    write_dma_leng();
                    switch (g_dma_mode)
                    {
                        case 1:
                            write_dma_src_addr(g_dma_src_addr >> 1);
                            break;
                        case 3:
                            write_dma_src_addr(g_dma_src_addr);
                            break;
                    }
                }
            }
            return w_clock;
        }

        private void dma_run_memory_req()
        {
            g_dma_src_addr = read_dma_src_addr() << 1;
            g_dma_leng = read_dma_leng();
            g_dma_mode = 1;
            g_vdp_status_1_dma = 1;
            g_vdp_status_8_full = 1;
            int w_loop_cnt = g_dma_leng;
            switch (g_vdp_reg_code & 0x0f)
            {
                case 1:
                    do
                    {
                        ushort w_val = md_main.g_md_m68k.read16(g_dma_src_addr);
                        vram_write_w(g_vdp_reg_dest_address, w_val);
                        pattern_chk(g_vdp_reg_dest_address, (byte)(w_val >> 8));
                        pattern_chk(g_vdp_reg_dest_address + 1, (byte)(w_val & 0xff));
                        g_dma_src_addr += 2;
                        g_vdp_reg_dest_address = (ushort)(g_vdp_reg_dest_address + g_vdp_reg_15_autoinc);
                    } while (--w_loop_cnt > 0);
                    break;
                case 3:
                    do
                    {
                        ushort w_val = md_main.g_md_m68k.read16(g_dma_src_addr);
                        int wcol_num = (int)((g_vdp_reg_dest_address >> 1) & 0x3f);
                        cram_set(wcol_num, w_val);
                        g_dma_src_addr += 2;
                        g_vdp_reg_dest_address = (ushort)(g_vdp_reg_dest_address + g_vdp_reg_15_autoinc);
                    } while (--w_loop_cnt > 0);
                    break;
                case 5:
                    do
                    {
                        ushort w_val = md_main.g_md_m68k.read16(g_dma_src_addr);
                        int wcol_num = (int)((g_vdp_reg_dest_address >> 1) % 40);
                        g_vsram[wcol_num] = w_val; g_dma_src_addr += 2;
                        g_vdp_reg_dest_address = (ushort)(g_vdp_reg_dest_address + g_vdp_reg_15_autoinc);
                    } while (--w_loop_cnt > 0);
                    break;
            }

        }
        private void dma_run_fill_req(ushort in_data)
        {
            g_dma_leng = read_dma_leng();
            g_dma_fill_data = in_data;
            g_dma_mode = 2;
            g_vdp_status_1_dma = 1;
            g_vdp_status_8_full = 1;
            int w_loop_cnt = g_dma_leng;
            switch (g_vdp_reg_code & 0x0f)
            {
                case 1:
                    byte w_data = (byte)(g_dma_fill_data & 0x00ff);
                    g_vram[g_vdp_reg_dest_address] = w_data;
                    pattern_chk(g_vdp_reg_dest_address, w_data);
                    w_data = (byte)((g_dma_fill_data >> 8) & 0x00ff);
                    do
                    {
                        g_vram[g_vdp_reg_dest_address ^ 1] = w_data;
                        pattern_chk((g_vdp_reg_dest_address ^ 1), w_data);
                        g_vdp_reg_dest_address = (ushort)(g_vdp_reg_dest_address + g_vdp_reg_15_autoinc);
                    } while (--w_loop_cnt > 0);
                    break;
                case 3:
                    do
                    {
                        int wcol_num = (int)((g_vdp_reg_dest_address >> 1) & 0x3f);
                        cram_set(wcol_num, g_dma_fill_data);
                        g_vdp_reg_dest_address = (ushort)((g_vdp_reg_dest_address + g_vdp_reg_15_autoinc) & 0xffff);
                    } while (--w_loop_cnt > 0);
                    break;
                case 5:
                    do
                    {
                        g_vsram[(g_vdp_reg_dest_address >> 1) % 40] = g_dma_fill_data;
                        g_dma_src_addr += 1;
                        g_vdp_reg_dest_address = (ushort)(g_vdp_reg_dest_address + g_vdp_reg_15_autoinc);
                    } while (--w_loop_cnt > 0);
                    break;
            }
        }
        private void dma_run_copy_req()
        {
            g_dma_src_addr = read_dma_src_addr() & 0xffff;
            g_dma_leng = read_dma_leng();
            g_dma_mode = 3;
            g_vdp_status_1_dma = 1;
            g_vdp_status_8_full = 1;
            int w_loop_cnt = g_dma_leng;
            switch (g_vdp_reg_code & 0x0f)
            {
                case 1:
                    do
                    {
                        byte w_val = g_vram[g_dma_src_addr];
                        g_vram[g_vdp_reg_dest_address] = w_val;
                        pattern_chk(g_vdp_reg_dest_address, w_val);
                        g_dma_src_addr = (g_dma_src_addr + 1) & 0xffff;
                        g_vdp_reg_dest_address = (ushort)((g_vdp_reg_dest_address + g_vdp_reg_15_autoinc) & 0xffff);
                    } while (--w_loop_cnt > 0);
                    break;
                case 3:
                    MessageBox.Show("md_vdp.dma_run_copy", "error");
                    break;
                case 5:
                    MessageBox.Show("md_vdp.dma_run_copy", "error");
                    break;
            }
        }
        //--------------------------------------------------
        private uint read_dma_src_addr()
        {
            return (uint)(g_vdp_reg_21_dma_source_low
                        + (g_vdp_reg_22_dma_source_mid << 8)
                        + (g_vdp_reg_23_5_dma_high << 16));
        }
        private void write_dma_src_addr(uint in_addr)
        {
            g_vdp_reg_21_dma_source_low = (byte)(in_addr & 0x00ff);
            g_vdp_reg_22_dma_source_mid = (byte)(in_addr >> 8);
            g_vdp_reg_23_5_dma_high = (byte)(in_addr >> 16);
        }
        private int read_dma_leng()
        {
            int out_ling = (g_vdp_reg_19_dma_counter_low
                    + (g_vdp_reg_20_dma_counter_high << 8));
            if (out_ling == 0) out_ling = 0xffff;
            return out_ling;
        }
        private void write_dma_leng()
        {
            g_vdp_reg_19_dma_counter_low = (byte)(g_dma_leng & 0x00ff);
            g_vdp_reg_20_dma_counter_high = (byte)(g_dma_leng >> 8);
        }
    }
}
