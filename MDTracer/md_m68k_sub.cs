namespace MDTracer
{
    internal partial class md_m68k
    {
        private uint[] MASKBIT = new uint[3] { 0xff, 0xffff, 0xffffffff };
        private uint[] MOSTBIT = new uint[3] { 0x80, 0x8000, 0x80000000 };
        private uint[] MASKNOTBIT = new uint[3] { 0xffffff00, 0xffff0000, 0x00000000 };
        private uint[] BITHIT = new uint[32] { 0x00000001, 0x00000002, 0x00000004, 0x00000008,
                                                      0x00000010, 0x00000020, 0x00000040, 0x00000080,
                                                      0x00000100, 0x00000200, 0x00000400, 0x00000800,
                                                      0x00001000, 0x00002000, 0x00004000, 0x00008000,
                                                      0x00010000, 0x00020000, 0x00040000, 0x00080000,
                                                      0x00100000, 0x00200000, 0x00400000, 0x00800000,
                                                      0x01000000, 0x02000000, 0x04000000, 0x08000000,
                                                      0x10000000, 0x20000000, 0x40000000, 0x80000000 };
        private int[,] MOVE_CLOCK = {{ 4,  4,  9,  9,  9, 13, 15, 13, 17 }
                                    ,{ 4,  4,  9,  9,  9, 13, 15, 13, 17 }
                                    ,{ 8,  8, 13, 13, 13, 17, 19, 17, 21 }
                                    ,{ 8,  8, 13, 13, 13, 17, 19, 17, 21 }
                                    ,{10, 10, 15, 15, 15, 19, 21, 19, 23 }
                                    ,{12, 12, 17, 17, 17, 21, 23, 21, 15 }
                                    ,{14, 14, 19, 19, 19, 23, 25, 23, 27 }
                                    ,{12, 12, 17, 17, 17, 21, 23, 21, 25 }
                                    ,{16, 16, 21, 21, 21, 25, 27, 25, 29 }
                                    ,{12, 12, 17, 17, 17, 21, 23, 21, 25 }
                                    ,{14, 14, 19, 19, 19, 23, 25, 23, 27 }
                                    ,{ 8,  8, 13, 13, 13, 17, 19, 17, 21 }
                                };
        private int[,] MOVE_CLOCK_L = {{ 4,  4, 14, 14, 16, 18, 20, 18, 22 }
                                    ,{ 4,  4, 14, 14, 16, 18, 20, 18, 22 }
                                    ,{12, 12, 22, 22, 22, 26, 28, 26, 30 }
                                    ,{12, 12, 22, 22, 22, 26, 28, 26, 30 }
                                    ,{14, 14, 24, 24, 24, 28, 30, 28, 32 }
                                    ,{16, 16, 26, 26, 26, 30, 35, 30, 34 }
                                    ,{18, 18, 28, 28, 28, 32, 34, 32, 36 }
                                    ,{16, 16, 26, 26, 26, 30, 32, 30, 34 }
                                    ,{20, 20, 30, 30, 30, 34, 36, 34, 38 }
                                    ,{16, 16, 26, 26, 26, 30, 32, 30, 34 }
                                    ,{18, 18, 28, 28, 28, 32, 34, 32, 36 }
                                    ,{12, 12, 22, 22, 22, 26, 28, 26, 30 }
                                };
        private ushort g_reg_SR
        {
            get
            {
                ushort value = 0;
                if (g_status_T == true) value |= 0x8000;
                if (g_status_B1 == true) value |= 0x4000;
                if (g_status_S == true) value |= 0x2000;
                if (g_status_B2 == true) value |= 0x1000;
                if (g_status_B3 == true) value |= 0x0800;
                value |= (ushort)((g_status_interrupt_mask & 0x07) << 8);
                if (g_status_B4 == true) value |= 0x0080;
                if (g_status_B5 == true) value |= 0x0040;
                if (g_status_B6 == true) value |= 0x0020;
                if (g_status_X == true) value |= 0x0010;
                if (g_status_N == true) value |= 0x0008;
                if (g_status_Z == true) value |= 0x0004;
                if (g_status_V == true) value |= 0x0002;
                if (g_status_C == true) value |= 0x0001;
                return value;
            }
            set
            {
                if ((value & 0x8000) != 0) g_status_T = true; else g_status_T = false;
                if ((value & 0x4000) != 0) g_status_B1 = true; else g_status_B1 = false;
                if ((value & 0x2000) != 0) g_status_S = true; else g_status_S = false;
                if ((value & 0x1000) != 0) g_status_B2 = true; else g_status_B2 = false;
                if ((value & 0x0800) != 0) g_status_B3 = true; else g_status_B3 = false;
                g_status_interrupt_mask = (int)((value & 0x0700) >> 8);
                if ((value & 0x0080) != 0) g_status_B4 = true; else g_status_B4 = false;
                if ((value & 0x0040) != 0) g_status_B5 = true; else g_status_B5 = false;
                if ((value & 0x0020) != 0) g_status_B6 = true; else g_status_B6 = false;
                if ((value & 0x0010) != 0) g_status_X = true; else g_status_X = false;
                if ((value & 0x0008) != 0) g_status_N = true; else g_status_N = false;
                if ((value & 0x0004) != 0) g_status_Z = true; else g_status_Z = false;
                if ((value & 0x0002) != 0) g_status_V = true; else g_status_V = false;
                if ((value & 0x0001) != 0) g_status_C = true; else g_status_C = false;
            }
        }
        private byte g_status_CCR
        {
            get
            {
                byte value = 0;
                if (g_status_B4 == true) value |= 0x0080;
                if (g_status_B5 == true) value |= 0x0040;
                if (g_status_B6 == true) value |= 0x0020;
                if (g_status_X == true) value |= 0x0010;
                if (g_status_N == true) value |= 0x0008;
                if (g_status_Z == true) value |= 0x0004;
                if (g_status_V == true) value |= 0x0002;
                if (g_status_C == true) value |= 0x0001;
                return value;
            }
            set
            {
                if ((value & 0x0080) != 0) g_status_B4 = true; else g_status_B4 = false;
                if ((value & 0x0040) != 0) g_status_B5 = true; else g_status_B5 = false;
                if ((value & 0x0020) != 0) g_status_B6 = true; else g_status_B6 = false;
                if ((value & 0x0010) != 0) g_status_X = true; else g_status_X = false;
                if ((value & 0x0008) != 0) g_status_N = true; else g_status_N = false;
                if ((value & 0x0004) != 0) g_status_Z = true; else g_status_Z = false;
                if ((value & 0x0002) != 0) g_status_V = true; else g_status_V = false;
                if ((value & 0x0001) != 0) g_status_C = true; else g_status_C = false;
            }
        }
        private Func<bool>[] g_flag_chack;
        private bool g_flag_chack_t() { return true; }
        private bool g_flag_chack_f() { return false; }
        private bool g_flag_chack_hi() { return (!g_status_C && !g_status_Z); }
        private bool g_flag_chack_ls() { return (g_status_C || g_status_Z); }
        private bool g_flag_chack_cc() { return (!g_status_C); }
        private bool g_flag_chack_cs() { return (g_status_C); }
        private bool g_flag_chack_ne() { return (!g_status_Z); }
        private bool g_flag_chack_eq() { return (g_status_Z); }
        private bool g_flag_chack_vc() { return (!g_status_V); }
        private bool g_flag_chack_vs() { return (g_status_V); }
        private bool g_flag_chack_pl() { return (!g_status_N); }
        private bool g_flag_chack_mi() { return (g_status_N); }
        private bool g_flag_chack_ge() { return (!((g_status_N ^ g_status_V) == true)); }
        private bool g_flag_chack_lt() { return ((g_status_N ^ g_status_V) == true); }
        private bool g_flag_chack_gt() { return ((!((g_status_N ^ g_status_V) == true)) && (!g_status_Z)); }
        private bool g_flag_chack_le() { return ((((g_status_N ^ g_status_V) == true)) || (g_status_Z)); }

        //----------------------------------------------------------------
        private uint get_int_cast(uint in_data, int in_size)
        {
            uint w_mostbit = MOSTBIT[in_size];
            uint w_mostnotbit = MASKNOTBIT[in_size];
            uint w_out = in_data;
            if(in_size != 2)
            {
                if ((w_out & w_mostbit) == w_mostbit)
                {
                    w_out |= w_mostnotbit;
                }
                else
                {
                    w_out &= MASKBIT[in_size];
                }
            }
            return w_out;
        }
        private uint read_g_reg_data(int in_num, int in_size)
        {
            uint w_out = 0;
            switch (in_size)
            {
                case 0: w_out = g_reg_data[in_num].b0; break;
                case 1: w_out = g_reg_data[in_num].w; break;
                default: w_out = g_reg_data[in_num].l; break;
            }
            return w_out;
        }
        private void write_g_reg_data(int in_num, int in_size, uint in_val)
        {
            switch (in_size)
            {
                case 0: g_reg_data[in_num].b0 = (byte)in_val; break;
                case 1: g_reg_data[in_num].w = (ushort)in_val; break;
                default: g_reg_data[in_num].l = in_val; break;
            }
        }
        //----------------------------------------------------------------
        private void stack_push32(uint in_val)
        {
            if (g_status_S == true)
            {
                g_reg_addr[7].l = (g_reg_addr[7].l - 4);
                write32(g_reg_addr[7].l, in_val);
            }
            else
            {
                g_reg_addr[g_reg_addr_usp.l].l = (g_reg_addr[g_reg_addr_usp.l].l - 4);
                write32(g_reg_addr[g_reg_addr_usp.l].l, in_val);
            }
        }
        private uint stack_pop32()
        {
            uint w_addr = 0;
            if (g_status_S == true)
            {
                w_addr = read32(g_reg_addr[7].l);
                write32(g_reg_addr[7].l, 0);
                g_reg_addr[7].l += 4;
            }
            else
            {
                w_addr = read32(g_reg_addr[g_reg_addr_usp.l].l);
                write32(g_reg_addr_usp.l, 0);
                g_reg_addr[g_reg_addr_usp.l].l += 4;
            }
            return w_addr;
        }
        private void stack_push16(ushort in_val)
        {
            if (g_status_S == true)
            {
                g_reg_addr[7].l = (g_reg_addr[7].l - 2);
                write16(g_reg_addr[7].l, in_val);
            }
            else
            {
                g_reg_addr[g_reg_addr_usp.l].l = (g_reg_addr[g_reg_addr_usp.l].l - 2);
                write16(g_reg_addr[g_reg_addr_usp.l].l, in_val);
            }
        }
        private ushort stack_pop16()
        {
            ushort w_addr = 0;
            if (g_status_S == true)
            {
                w_addr = read16(g_reg_addr[7].l);
                write16(g_reg_addr[7].l, 0);
                g_reg_addr[7].l += 2;
            }
            else
            {
                w_addr = read16(g_reg_addr[g_reg_addr_usp.l].l);
                write16(g_reg_addr_usp.l, 0);
                g_reg_addr[g_reg_addr_usp.l].l += 2;
            }
            return w_addr;
        }
    }
}
