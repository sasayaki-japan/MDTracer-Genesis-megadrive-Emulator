
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MDTracer
{
    internal partial class md_m68k
    {
        public void initialize()
        {
            g_reg_data = new UNION_UINT[8];
            g_reg_addr = new UNION_UINT[8];
            g_memory = new byte[0x1000000];
            g_opcode_info = new OPINFO[65536];
            g_flag_chack = new Func<bool>[]
            {
                g_flag_chack_t,
                g_flag_chack_f,
                g_flag_chack_hi,
                g_flag_chack_ls,
                g_flag_chack_cc,
                g_flag_chack_cs,
                g_flag_chack_ne,
                g_flag_chack_eq,
                g_flag_chack_vc,
                g_flag_chack_vs,
                g_flag_chack_pl,
                g_flag_chack_mi,
                g_flag_chack_ge,
                g_flag_chack_lt,
                g_flag_chack_gt,
                g_flag_chack_le
            };
        }
        public void reset()
        {
            for (int i = 0; i < 0x1000000; i++)
            {
                g_memory[i] = 0;
            }
            for (int i = 0; i < md_main.g_md_cartridge.g_file_size; i++)
            {
                g_memory[i] = md_main.g_md_cartridge.g_file[i];
            }
            g_initial_PC = read32(4);
            g_reg_PC = g_initial_PC;
            g_stack_top = read32(0);
            for (int i = 0; i < 8; i++)
            {
                g_reg_data[i].l = 0;
                g_reg_addr[i].l = 0;
            }
            g_reg_addr[7].l = read32(0);
            g_reg_addr_usp.l = 0;

            g_status_T = false;
            g_status_B1 = false;
            g_status_S = true;
            g_status_B2 = false;
            g_status_B3 = false;
            g_status_interrupt_mask = 7;
            g_status_B4 = false;
            g_status_B5 = false;
            g_status_B6 = false;
            g_status_X = false;
            g_status_N = false;
            g_status_Z = false;
            g_status_V = false;
            g_status_C = false;
            g_interrupt_V_req = false;
            g_interrupt_H_req = false;
            g_interrupt_EXT_req = false;
            g_interrupt_V_act = false;
            g_interrupt_H_act = false;
            g_interrupt_EXT_act = false;
            g_68k_stop = false;
            g_clock_total = 0;
            g_clock_now = 0;
            g_clock = 0;
        }

        private void opcode_add(int in_opnum, Action in_func
                                , string in_opname_org, string in_opname, string in_opname_out
                                , string in_format
                                , int in_opleng, int in_datasize, int in_memaccess)
        {
            int w1 = (byte)((in_opnum >> 9) & 0x07);
            int w2 = (byte)((in_opnum >> 6) & 0x07);
            int w3 = (byte)((in_opnum >> 3) & 0x07);
            int w4 = (byte)(in_opnum & 0x07);
            int w_len = in_opleng;

            g_opcode_info[in_opnum].opcode = in_func;
            g_opcode_info[in_opnum].opname_org = in_opname_org;
            g_opcode_info[in_opnum].opname = in_opname;
            g_opcode_info[in_opnum].opname_out = in_opname_out.ToLower();           
            g_opcode_info[in_opnum].format = in_format;
            g_opcode_info[in_opnum].opleng = w_len;
            g_opcode_info[in_opnum].datasize = in_datasize;
            g_opcode_info[in_opnum].memaccess = in_memaccess;
        }
    }
}
