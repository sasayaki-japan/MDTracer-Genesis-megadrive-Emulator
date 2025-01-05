namespace MDTracer
{
    internal partial class md_z80
    {
        public enum RP_TYPE { BC, DE, HL, SP };

        private byte g_status_flag
        {
            get
            {
                byte value = (byte)(
                (g_flag_S << 7)
                + (g_flag_Z << 6)
                + (g_flag_H << 4)
                + (g_flag_PV << 2)
                + (g_flag_N << 1)
                + g_flag_C);
                return value;
            }
            set
            {
                g_flag_S = (value & 0x80) >> 7;
                g_flag_Z = (value & 0x40) >> 6;
                g_flag_H = (value & 0x10) >> 4;
                g_flag_PV = (value & 0x04) >> 2;
                g_flag_N = (value & 0x02) >> 1;
                g_flag_C = (value & 0x01);
            }
        }

        private byte read_byte(ushort in_addr)
        {
            byte w_out = read8(in_addr);
            return w_out;
        }
        private void write_byte(ushort in_addr, byte in_data)
        {
            write8((uint)in_addr, in_data);
        }
        private ushort read_word(ushort in_addr)
        {
            ushort w_out = (ushort)((read8((uint)in_addr + 1) << 8)
                                        + read8(in_addr));
            return w_out;
        }
        private void write_word(ushort in_addr, ushort in_data)
        {
            byte w_data_h = (byte)((in_data >> 8) & 0xff);
            byte w_data_l = (byte)(in_data & 0xff);
            write8((uint)in_addr, w_data_l);
            write8((uint)in_addr + 1, w_data_h);
        }
        private ushort read_rp(byte in_rp)
        {
            ushort w_out = 0;
            switch (in_rp)
            {
                case 0: w_out = g_reg_BC; break;
                case 1: w_out = g_reg_DE; break;
                case 2: w_out = g_reg_HL; break;
                case 3: w_out = g_reg_SP; break;
            }
            return w_out;
        }
        private ushort read_rpix(byte in_rp)
        {
            ushort w_out = 0;
            switch (in_rp)
            {
                case 0: w_out = g_reg_BC; break;
                case 1: w_out = g_reg_DE; break;
                case 2: w_out = g_reg_IX; break;
                case 3: w_out = g_reg_SP; break;
            }
            return w_out;
        }
        private ushort read_rpiy(byte in_rp)
        {
            ushort w_out = 0;
            switch (in_rp)
            {
                case 0: w_out = g_reg_BC; break;
                case 1: w_out = g_reg_DE; break;
                case 2: w_out = g_reg_IY; break;
                case 3: w_out = g_reg_SP; break;
            }
            return w_out;
        }
        private void write_rp(byte in_rp, ushort in_data)
        {
            byte w_data_h = (byte)((in_data >> 8) & 0xff);
            byte w_data_l = (byte)(in_data & 0xff);
            switch (in_rp)
            {
                case 0:
                    g_reg_B = w_data_h;
                    g_reg_C = w_data_l;
                    break;
                case 1:
                    g_reg_D = w_data_h;
                    g_reg_E = w_data_l;
                    break;
                case 2:
                    g_reg_H = w_data_h;
                    g_reg_L = w_data_l;
                    break;
                case 3:
                    g_reg_SP = in_data;
                    break;
            }
        }
        private void write_reg(byte in_reg, byte in_data)
        {
            switch (in_reg)
            {
                case 7: g_reg_A = in_data; break;
                case 0: g_reg_B = in_data; break;
                case 1: g_reg_C = in_data; break;
                case 2: g_reg_D = in_data; break;
                case 3: g_reg_E = in_data; break;
                case 4: g_reg_H = in_data; break;
                case 5: g_reg_L = in_data; break;
            }
        }
        private byte read_reg(byte in_reg)
        {
            byte w_out = 0;
            switch (in_reg)
            {
                case 0: w_out = g_reg_B; break;
                case 1: w_out = g_reg_C; break;
                case 2: w_out = g_reg_D; break;
                case 3: w_out = g_reg_E; break;
                case 4: w_out = g_reg_H; break;
                case 5: w_out = g_reg_L; break;
                case 6: w_out = g_status_flag; break;
                case 7: w_out = g_reg_A; break;
            }
            return w_out;
        }
        private bool chk_condion(byte in_cond)
        {
            bool w_out = false;
            switch (in_cond)
            {
                case 0:
                    if (g_flag_Z == 0) w_out = true;
                    break;
                case 1:
                    if (g_flag_Z == 1) w_out = true;
                    break;
                case 2:
                    if (g_flag_C == 0) w_out = true;
                    break;
                case 3:
                    if (g_flag_C == 1) w_out = true;
                    break;
                case 4:
                    if (g_flag_PV == 0) w_out = true;
                    break;
                case 5:
                    if (g_flag_PV == 1) w_out = true;
                    break;
                case 6:
                    if (g_flag_S == 0) w_out = true;
                    break;
                case 7:
                    if (g_flag_S == 1) w_out = true;
                    break;
            }
            return w_out;
        }

        private void set_flag_s(bool in_val) { g_flag_S = (in_val == true) ? 1 : 0; }
        private void set_flag_z(bool in_val) { g_flag_Z = (in_val == true) ? 1 : 0; }
        private void set_flag_pv(bool in_val) { g_flag_PV = (in_val == true) ? 1 : 0; }
        private void set_flag_h(bool in_val) { g_flag_H = (in_val == true) ? 1 : 0; }
        private void set_flag_c(bool in_val) { g_flag_C = (in_val == true) ? 1 : 0; }
        private void set_flag_pv_logical(byte in_data)
        {
            int w_bit = 0;
            for (int i = 0; i < 8; i++)
            {
                if ((in_data & 1) == 1)
                {
                    w_bit += 1;
                }
                in_data = (byte)(in_data >> 1);
            }
            g_flag_PV = ((w_bit & 1) == 0) ? 1 : 0;
        }
        private void stack_push(byte in_val)
        {
            g_reg_SP -= 1;
            write_byte(g_reg_SP, in_val);
        }
        private byte stack_pop()
        {
            byte w_val = read_byte(g_reg_SP);
            g_reg_SP += 1;
            return w_val;
        }
    }
}

