using System;

namespace MDTracer
{
    internal partial class md_z80
    {
        private void op_LD_r1_r2()
        {
            byte w_val = read_reg(g_opcode1_210);
            write_reg(g_opcode1_543, w_val);
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_LD_r_n()
        {
            byte w_val = g_opcode2;
            write_reg(g_opcode1_543, w_val);
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_LD_r_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            write_reg(g_opcode1_543, w_val);
            g_reg_PC += 1;
            g_clock = 7;
        }
        private void op_LD_r_IXD()
        {
            byte w_val = read_byte((ushort)(g_reg_IX + g_opcode3));
            write_reg(g_opcode2_543, w_val);
            g_reg_PC += 3;
            g_clock = 19;
        }
        private void op_LD_r_IYD()
        {
            byte w_val = read_byte((ushort)(g_reg_IY + g_opcode3));
            write_reg(g_opcode2_543, w_val);
            g_reg_PC += 3;
            g_clock = 19;
        }
        private void op_LD_HL_r()
        {
            byte w_val = read_reg(g_opcode1_210);
            write_byte(g_reg_HL, w_val);
            g_reg_PC += 1;
            g_clock = 7;
        }
        private void op_LD_IXD_r()
        {
            byte w_val = read_reg(g_opcode2_210);
            ushort w_addr = (ushort)(g_reg_IX + g_opcode3);
            write_byte(w_addr, w_val);
            g_reg_PC += 3;
            g_clock = 19;
        }
        private void op_LD_IYD_r()
        {
            byte w_val = read_reg(g_opcode2_210);
            ushort w_addr = (ushort)(g_reg_IY + g_opcode3);
            write_byte(w_addr, w_val);
            g_reg_PC += 3;
            g_clock = 19;
        }
        private void op_LD_HL_n()
        {
            write_byte(g_reg_HL, g_opcode2);
            g_reg_PC += 2;
            g_clock = 10;
        }
        private void op_LD_IXD_n()
        {
            ushort w_addr = (ushort)(g_reg_IX + g_opcode3);
            write_byte(w_addr, g_opcode4);
            g_reg_PC += 4;
            g_clock = 19;
        }
        private void op_LD_IYD_n()
        {
            ushort w_addr = (ushort)(g_reg_IY + g_opcode3);
            write_byte(w_addr, g_opcode4);
            g_reg_PC += 4;
            g_clock = 19;
        }
        private void op_LD_a_BC()
        {
            g_reg_A = read_byte(g_reg_BC);
            g_reg_PC += 1;
            g_clock = 7;
        }
        private void op_LD_a_DE()
        {
            g_reg_A = read_byte(g_reg_DE);
            g_reg_PC += 1;
            g_clock = 7;
        }
        private void op_LD_a_NN()
        {
            g_reg_A = read_byte(g_opcode23);
            g_reg_PC += 3;
            g_clock = 13;
        }
        private void op_LD_BC_a()
        {
            write_byte(g_reg_BC, g_reg_A);
            g_reg_PC += 1;
            g_clock = 7;
        }
        private void op_LD_DE_a()
        {
            write_byte(g_reg_DE, g_reg_A);
            g_reg_PC += 1;
            g_clock = 7;
        }
        private void op_LD_NN_a()
        {
            write_byte(g_opcode23, g_reg_A);
            g_reg_PC += 3;
            g_clock = 13;
        }
        private void op_LD_a_i()
        {
            g_reg_A = g_reg_I;
            set_flag_s(g_reg_A >= 0x80);
            set_flag_z(g_reg_A == 0);
            set_flag_pv(g_IFF2 == true);
            g_flag_H = 0;
            g_flag_N = 0;
            g_reg_PC += 2;
            g_clock = 9;
        }
        private void op_LD_a_r()
        {
            g_reg_A = g_reg_R;
            set_flag_s(g_reg_A >= 0x80);
            set_flag_z(g_reg_A == 0);
            set_flag_pv(g_IFF1 == true);
            g_flag_H = 0;
            g_flag_N = 0;
            g_reg_PC += 2;
            g_clock = 9;
        }
        private void op_LD_i_a()
        {
            g_reg_I = g_reg_A;
            g_reg_PC += 2;
            g_clock = 9;
        }
        private void op_LD_r_a()
        {
            g_reg_R = g_reg_A;
            g_reg_PC += 2;
            g_clock = 9;
        }
        //--------------------------------------
        private void op_LD_rp_nn()
        {
            write_rp(g_opcode1_54, g_opcode23);
            g_reg_PC += 3;
            g_clock = 10;
        }
        private void op_LD_ix_nn()
        {
            g_reg_IX = g_opcode34;
            g_reg_PC += 4;
            g_clock = 14;
        }
        private void op_LD_iy_nn()
        {
            g_reg_IY = g_opcode34;
            g_reg_PC += 4;
            g_clock = 14;
        }
        private void op_LD_hl_NN()
        {
            ushort w_val = read_word(g_opcode23);
            write_rp((byte)RP_TYPE.HL, w_val);
            g_reg_PC += 3;
            g_clock = 16;
        }
        private void op_LD_rp_NN()
        {
            ushort w_val = read_word(g_opcode34);
            write_rp(g_opcode2_54, w_val);
            g_reg_PC += 4;
            g_clock = 20;
        }
        private void op_LD_ix_NN()
        {
            g_reg_IX = read_word(g_opcode34);
            g_reg_PC += 4;
            g_clock = 20;
        }
        private void op_LD_iy_NN()
        {
            g_reg_IY = read_word(g_opcode34);
            g_reg_PC += 4;
            g_clock = 20;
        }
        private void op_LD_NN_hl()
        {
            write_word(g_opcode23, g_reg_HL);
            g_reg_PC += 3;
            g_clock = 16;
        }
        private void op_LD_NN_rp()
        {
            ushort w_val = read_rp(g_opcode2_54);
            write_word(g_opcode34, w_val);
            g_reg_PC += 4;
            g_clock = 20;
        }
        private void op_LD_NN_ix()
        {
            write_word(g_opcode34, g_reg_IX);
            g_reg_PC += 4;
            g_clock = 20;
        }
        private void op_LD_NN_iy()
        {
            write_word(g_opcode34, g_reg_IY);
            g_reg_PC += 4;
            g_clock = 20;
        }
        private void op_LD_sp_hl()
        {
            g_reg_SP = g_reg_HL;
            g_reg_PC += 1;
            g_clock = 6;
        }
        private void op_LD_sp_ix()
        {
            g_reg_SP = g_reg_IX;
            g_reg_PC += 2;
            g_clock = 10;
        }
        private void op_LD_sp_iy()
        {
            g_reg_SP = g_reg_IY;
            g_reg_PC += 2;
            g_clock = 10;
        }
        //--------------------------------------
        private void M_LD_loop(bool in_count, bool in_loop)
        {
            ushort w_bc = g_reg_BC;
            ushort w_de = g_reg_DE;
            ushort w_hl = g_reg_HL;

            do
            {
                byte w_val = read_byte(w_hl);
                write_byte(w_de, w_val);
                w_bc -= 1;
                if (in_count == true)
                {
                    w_de += 1;
                    w_hl += 1;
                }
                else
                {
                    w_de -= 1;
                    w_hl -= 1;
                }
                g_clock += 21;
            } while ((in_loop == true) && (w_bc != 0));

            write_rp((byte)RP_TYPE.BC, w_bc);
            write_rp((byte)RP_TYPE.DE, w_de);
            write_rp((byte)RP_TYPE.HL, w_hl);
            g_flag_H = 0;
            g_flag_N = 0;
            set_flag_pv(false);
            g_clock += 16;
            g_reg_PC += 2;
        }
        private void op_LDI() { M_LD_loop(true, false); }
        private void op_LDIR() { M_LD_loop(true, true); }
        private void op_LDD() { M_LD_loop(false, false); }
        private void op_LDDR() { M_LD_loop(false, true); }
        //--------------------------------------
        private void op_EX_de_hl()
        {
            byte w_val = 0;
            w_val = g_reg_H; g_reg_H = g_reg_D; g_reg_D = w_val;
            w_val = g_reg_L; g_reg_L = g_reg_E; g_reg_E = w_val;
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_EX_af_af2()
        {
            byte w_val = 0;
            int w_val2 = 0;
            w_val = g_reg_Au; g_reg_Au = g_reg_A; g_reg_A = w_val;
            w_val2 = g_flag_Su; g_flag_Su = g_flag_S; g_flag_S = w_val2;
            w_val2 = g_flag_Zu; g_flag_Zu = g_flag_Z; g_flag_Z = w_val2;
            w_val2 = g_flag_Hu; g_flag_Hu = g_flag_H; g_flag_H = w_val2;
            w_val2 = g_flag_PVu; g_flag_PVu = g_flag_PV; g_flag_PV = w_val2;
            w_val2 = g_flag_Nu; g_flag_Nu = g_flag_N; g_flag_N = w_val2;
            w_val2 = g_flag_Cu; g_flag_Cu = g_flag_C; g_flag_C = w_val2;
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_EXX()
        {
            byte w_val = 0;
            w_val = g_reg_Bu; g_reg_Bu = g_reg_B; g_reg_B = w_val;
            w_val = g_reg_Cu; g_reg_Cu = g_reg_C; g_reg_C = w_val;
            w_val = g_reg_Du; g_reg_Du = g_reg_D; g_reg_D = w_val;
            w_val = g_reg_Eu; g_reg_Eu = g_reg_E; g_reg_E = w_val;
            w_val = g_reg_Hu; g_reg_Hu = g_reg_H; g_reg_H = w_val;
            w_val = g_reg_Lu; g_reg_Lu = g_reg_L; g_reg_L = w_val;
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_EX_SP_hl()
        {
            byte w_val1 = read_byte(g_reg_SP);
            byte w_val2 = read_byte((ushort)(g_reg_SP + 1));
            write_byte(g_reg_SP, g_reg_L);
            write_byte((ushort)(g_reg_SP + 1), g_reg_H);
            g_reg_L = w_val1;
            g_reg_H = w_val2;
            g_reg_PC += 1;
            g_clock = 19;
        }
        private void op_EX_SP_ix()
        {
            byte w_val1 = read_byte(g_reg_SP);
            byte w_val2 = read_byte((ushort)(g_reg_SP + 1));
            write_byte(g_reg_SP, g_reg_IXL);
            write_byte((ushort)(g_reg_SP + 1), g_reg_IXH);
            g_write_IXL(w_val1);
            g_write_IXH(w_val2);
            g_reg_PC += 2;
            g_clock = 23;
        }
        private void op_EX_SP_iy()
        {
            byte w_val1 = read_byte(g_reg_SP);
            byte w_val2 = read_byte((ushort)(g_reg_SP + 1));
            write_byte(g_reg_SP, g_reg_IYL);
            write_byte((ushort)(g_reg_SP + 1), g_reg_IYH);
            g_write_IYL(w_val1);
            g_write_IYH(w_val2);
            g_reg_PC += 2;
            g_clock = 23;
        }
        //--------------------------------------
        private void op_PUSH_rp()
        {
            ushort w_val = read_rp(g_opcode1_54);
            stack_push((byte)((w_val >> 8) & 0xff));
            stack_push((byte)(w_val & 0xff));
            g_reg_PC += 1;
            g_clock = 11;
        }
        private void op_PUSH_af()
        {
            stack_push(g_reg_A);
            stack_push(g_status_flag);
            g_reg_PC += 1;
            g_clock = 11;
        }
        private void op_PUSH_ix()
        {
            stack_push(g_reg_IXH);
            stack_push(g_reg_IXL);
            g_reg_PC += 2;
            g_clock = 15;
        }
        private void op_PUSH_iy()
        {
            stack_push(g_reg_IYH);
            stack_push(g_reg_IYL);
            g_reg_PC += 2;
            g_clock = 15;
        }
        private void op_POP_rp()
        {
            ushort w_val = (ushort)(stack_pop() + (stack_pop() << 8));
            write_rp(g_opcode1_54, w_val);
            g_reg_PC += 1;
            g_clock = 10;
        }
        private void op_POP_af()
        {
            g_status_flag = stack_pop();
            g_reg_A = stack_pop();
            g_reg_PC += 1;
            g_clock = 10;
        }
        private void op_POP_ix()
        {
            g_reg_IX = (ushort)(stack_pop() + (stack_pop() << 8));
            g_reg_PC += 2;
            g_clock = 14;
        }
        private void op_POP_iy()
        {
            g_reg_IY = (ushort)(stack_pop() + (stack_pop() << 8));
            g_reg_PC += 2;
            g_clock = 14;
        }
        //--------------------------------------
        private void set_flag_rotate(byte in_val, bool in_mode)
        {
            g_flag_H = 0;
            g_flag_N = 0;
            if (in_mode == true)
            {
                set_flag_s((in_val & 0x80) == 0x80);
                set_flag_z(in_val == 0);
                set_flag_pv_logical(in_val);
            }
        }
        private byte M_RRC(byte in_val, bool in_mode)
        {
            g_flag_C = in_val & 0x01;
            in_val >>= 1;
            in_val &= 0x7f;
            in_val |= (byte)((g_flag_C == 1) ? 0x80 : 0);
            set_flag_rotate(in_val, in_mode);
            return in_val;
        }
        private byte M_RLC(byte in_val, bool in_mode)
        {
            g_flag_C = (in_val & 0x80) >> 7;
            in_val <<= 1;
            in_val = (byte)(in_val | g_flag_C);
            set_flag_rotate(in_val, in_mode);
            return in_val;
        }
        private byte M_RL(byte in_val, bool in_mode)
        {
            int w_b7 = (in_val & 0x80) >> 7;
            in_val <<= 1;
            in_val = (byte)(in_val | g_flag_C);
            g_flag_C = w_b7;
            set_flag_rotate(in_val, in_mode);
            return in_val;
        }
        private byte M_RR(byte in_val, bool in_mode)
        {
            int w_b7 = in_val & 0x01;
            in_val >>= 1;
            in_val &= 0x7f;
            in_val |= (byte)((g_flag_C == 1) ? 0x80 : 0);
            g_flag_C = w_b7;
            set_flag_rotate(in_val, in_mode);
            return in_val;
        }
        private byte M_SLA(byte in_val)
        {
            g_flag_C = (in_val & 0x80) >> 7;
            in_val <<= 1;
            set_flag_rotate(in_val, true);
            return in_val;
        }
        private byte M_SRA(byte in_val)
        {
            byte w_b7 = (byte)(in_val & 0x80);
            g_flag_C = in_val & 1;
            in_val >>= 1;
            in_val &= 0x7f;
            in_val |= w_b7;
            set_flag_rotate(in_val, true);
            return in_val;
        }
        private byte M_SRL(byte in_val)
        {
            g_flag_C = in_val & 1;
            in_val >>= 1;
            in_val &= 0x7f;
            set_flag_rotate(in_val, true);
            return in_val;
        }
        private byte M_SLL(byte in_val)
        {
            g_flag_C = (in_val & 0x80) >> 7;
            in_val <<= 1;
            in_val |= 1;
            set_flag_rotate(in_val, true);
            return in_val;
        }
        //--------------------------------------
        private void op_RRCA()
        {
            g_reg_A = M_RRC(g_reg_A, false);
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_RRC_r()
        {
            byte w_val = read_reg(g_opcode2_210);
            w_val = M_RRC(w_val, true);
            write_reg(g_opcode2_210, w_val);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_RRC_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            w_val = M_RRC(w_val, true);
            write_byte(g_reg_HL, w_val);
            g_reg_PC += 2;
            g_clock = 15;
        }
        private void op_RRC_IXD()
        {
            ushort w_addr = (ushort)(g_reg_IX + g_opcode3);
            byte w_val = read_byte(w_addr);
            w_val = M_RRC(w_val, true);
            write_byte(w_addr, w_val);
            g_reg_PC += 4;
            g_clock = 23;
        }
        private void op_RRC_IYD()
        {
            ushort w_addr = (ushort)(g_reg_IY + g_opcode3);
            byte w_val = read_byte(w_addr);
            w_val = M_RRC(w_val, true);
            write_byte(w_addr, w_val);
            g_reg_PC += 4;
            g_clock = 23;
        }
        //--------------------------------------
        private void op_RLCA()
        {
            g_reg_A = M_RLC(g_reg_A, false);
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_RLC_r()
        {
            byte w_val = read_reg(g_opcode2_210);
            w_val = M_RLC(w_val, true);
            write_reg(g_opcode2_210, w_val);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_RLC_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            w_val = M_RLC(w_val, true);
            write_byte(g_reg_HL, w_val);
            g_reg_PC += 2;
            g_clock = 15;
        }
        private void op_RLC_IXD()
        {
            ushort w_addr = (ushort)(g_reg_IX + g_opcode3);
            byte w_val = read_byte(w_addr);
            w_val = M_RLC(w_val, true);
            write_byte(w_addr, w_val);
            g_reg_PC += 4;
            g_clock = 23;
        }
        private void op_RLC_IYD()
        {
            ushort w_addr = (ushort)(g_reg_IY + g_opcode3);
            byte w_val = read_byte(w_addr);
            w_val = M_RLC(w_val, true);
            write_byte(w_addr, w_val);
            g_reg_PC += 4;
            g_clock = 23;
        }
        //--------------------------------------
        private void op_RLA()
        {
            g_reg_A = M_RL(g_reg_A, false);
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_RL_r()
        {
            byte w_val = read_reg(g_opcode2_210);
            w_val = M_RL(w_val, true);
            write_reg(g_opcode2_210, w_val);
            g_reg_PC += 2;
            g_clock = 8;
        }
        private void op_RL_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            w_val = M_RL(w_val, true);
            write_byte(g_reg_HL, w_val);
            g_reg_PC += 2;
            g_clock = 15;
        }
        private void op_RL_IXD()
        {
            ushort w_addr = (ushort)(g_reg_IX + g_opcode3);
            byte w_val = read_byte(w_addr);
            w_val = M_RL(w_val, true);
            write_byte(w_addr, w_val);
            g_reg_PC += 4;
            g_clock = 23;
        }
        private void op_RL_IYD()
        {
            ushort w_addr = (ushort)(g_reg_IY + g_opcode3);
            byte w_val = read_byte(w_addr);
            w_val = M_RL(w_val, true);
            write_byte(w_addr, w_val);
            g_reg_PC += 4;
            g_clock = 23;
        }
        //--------------------------------------
        private void op_RRA()
        {
            g_reg_A = M_RR(g_reg_A, false);
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_RR_r()
        {
            byte w_val = read_reg(g_opcode2_210);
            w_val = M_RR(w_val, true);
            write_reg(g_opcode2_210, w_val);
            g_reg_PC += 2;
            g_clock = 8;
        }
        private void op_RR_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            w_val = M_RR(w_val, true);
            write_byte(g_reg_HL, w_val);
            g_reg_PC += 2;
            g_clock = 15;
        }
        private void op_RR_IXD()
        {
            ushort w_addr = (ushort)(g_reg_IX + g_opcode3);
            byte w_val = read_byte(w_addr);
            w_val = M_RR(w_val, true);
            write_byte(w_addr, w_val);
            g_reg_PC += 4;
            g_clock = 23;
        }
        private void op_RR_IYD()
        {
            ushort w_addr = (ushort)(g_reg_IY + g_opcode3);
            byte w_val = read_byte(w_addr);
            w_val = M_RR(w_val, true);
            write_byte(w_addr, w_val);
            g_reg_PC += 4;
            g_clock = 23;
        }
        //--------------------------------------
        private void op_SLA_r()
        {
            byte w_val = read_reg(g_opcode2_210);
            w_val = M_SLA(w_val);
            write_reg(g_opcode2_210, w_val);
            g_reg_PC += 2;
            g_clock = 8;
        }
        private void op_SLA_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            w_val = M_SLA(w_val);
            write_byte(g_reg_HL, w_val);
            g_reg_PC += 2;
            g_clock = 15;
        }
        private void op_SLA_IXD()
        {
            ushort w_addr = (ushort)(g_reg_IX + g_opcode3);
            byte w_val = read_byte(w_addr);
            w_val = M_SLA(w_val);
            write_byte(w_addr, w_val);
            g_reg_PC += 4;
            g_clock = 23;
        }
        private void op_SLA_IYD()
        {
            ushort w_addr = (ushort)(g_reg_IY + g_opcode3);
            byte w_val = read_byte(w_addr);
            w_val = M_SLA(w_val);
            write_byte(w_addr, w_val);
            g_reg_PC += 4;
            g_clock = 23;
        }
        //--------------------------------------
        private void op_SRA_r()
        {
            byte w_val = read_reg(g_opcode2_210);
            w_val = M_SRA(w_val);
            write_reg(g_opcode2_210, w_val);
            g_reg_PC += 2;
            g_clock = 8;
        }
        private void op_SRA_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            w_val = M_SRA(w_val);
            write_byte(g_reg_HL, w_val);
            g_reg_PC += 2;
            g_clock = 15;
        }
        private void op_SRA_IXD()
        {
            ushort w_addr = (ushort)(g_reg_IX + g_opcode3);
            byte w_val = read_byte(w_addr);
            w_val = M_SRA(w_val);
            write_byte(w_addr, w_val);
            g_reg_PC += 4;
            g_clock = 23;
        }
        private void op_SRA_IYD()
        {
            ushort w_addr = (ushort)(g_reg_IY + g_opcode3);
            byte w_val = read_byte(w_addr);
            w_val = M_SRA(w_val);
            write_byte(w_addr, w_val);
            g_reg_PC += 4;
            g_clock = 23;
        }
        //--------------------------------------
        private void op_SRL_r()
        {
            byte w_val = read_reg(g_opcode2_210);
            w_val = M_SRL(w_val);
            write_reg(g_opcode2_210, w_val);
            g_reg_PC += 2;
            g_clock = 8;
        }
        private void op_SRL_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            w_val = M_SRL(w_val);
            write_byte(g_reg_HL, w_val);
            g_reg_PC += 2;
            g_clock = 15;
        }
        private void op_SRL_IXD()
        {
            ushort w_addr = (ushort)(g_reg_IX + g_opcode3);
            byte w_val = read_byte(w_addr);
            w_val = M_SRL(w_val);
            write_byte(w_addr, w_val);
            g_reg_PC += 4;
            g_clock = 23;
        }
        private void op_SRL_IYD()
        {
            ushort w_addr = (ushort)(g_reg_IY + g_opcode3);
            byte w_val = read_byte(w_addr);
            w_val = M_SRL(w_val);
            write_byte(w_addr, w_val);
            g_reg_PC += 4;
            g_clock = 23;
        }
        //--------------------------------------
        private byte M_ADD(byte in_val1, byte in_val2, byte in_val3)
        {
            int w_result = in_val1 + in_val2 + in_val3;
            byte w_result2 = (byte)(w_result & 0xff);
            set_flag_s((w_result2 & 0x80) > 0);
            set_flag_z(w_result2 == 0);
            set_flag_h(((in_val1 & 0xf) + (in_val2 & 0xf) + in_val3) > 0xf);
            set_flag_pv(((in_val1 ^ w_result) & 0x80) != 0 && ((in_val1 ^ in_val2) & 0x80) == 0);
            set_flag_c(w_result > 0xff);
            g_flag_N = 0;
            return w_result2;
        }
        private byte M_SUB(byte in_val1, byte in_val2, byte in_val3, bool in_c_up = true)
        {
            int w_result = in_val1 - in_val2 - in_val3;
            byte w_result2 = (byte)w_result;
            set_flag_s((w_result2 & 0x80) > 0);
            set_flag_z(w_result2 == 0);
            set_flag_h(((in_val1 & 0xf) < ((in_val2 & 0xf) + in_val3)));
            set_flag_pv(((in_val1 ^ in_val2) & 0x80) != 0 && ((in_val1 ^ w_result) & 0x80) != 0);
            if (in_c_up == true)
            {
                set_flag_c(w_result < 0);
            }
            g_flag_N = 1;
            return w_result2;
        }
        private byte M_INC(byte in_val1)
        {
            byte w_result = (byte)(in_val1 + 1);
            set_flag_s((w_result & 0x80) > 0);
            set_flag_z(w_result == 0);
            set_flag_h((w_result & 0xf) == 0);
            set_flag_pv(w_result == 0x80);
            g_flag_N = 0;
            return w_result;
        }
        private byte M_DEC(byte in_val1)
        {
            byte w_result = (byte)(in_val1 - 1);
            set_flag_s((w_result & 0x80) > 0);
            set_flag_z(w_result == 0);
            set_flag_h((w_result & 0xf) == 0xf);
            set_flag_pv(w_result == 0x7f);
            g_flag_N = 1;
            return w_result;
        }
        //--------------------------------------
        private void op_ADD_a_r()
        {
            byte w_val = read_reg(g_opcode1_210);
            g_reg_A = M_ADD(g_reg_A, w_val, 0);
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_ADD_a_n()
        {
            byte w_val = g_opcode2;
            g_reg_A = M_ADD(g_reg_A, w_val, 0);
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_ADD_a_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            g_reg_A = M_ADD(g_reg_A, w_val, 0);
            g_reg_PC += 1;
            g_clock = 7;
        }
        private void op_ADD_a_IXD()
        {
            byte w_val = read_byte((ushort)(g_reg_IX + g_opcode3));
            g_reg_A = M_ADD(g_reg_A, w_val, 0);
            g_reg_PC += 3;
            g_clock = 19;
        }
        private void op_ADD_a_IYD()
        {
            byte w_val = read_byte((ushort)(g_reg_IY + g_opcode3));
            g_reg_A = M_ADD(g_reg_A, w_val, 0);
            g_reg_PC += 3;
            g_clock = 19;
        }

        private void op_INC_r()
        {
            byte w_val = read_reg(g_opcode1_543);
            byte w_result = M_INC(w_val);
            write_reg(g_opcode1_543, w_result);
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_INC_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            byte w_result = M_INC(w_val);
            write_byte(g_reg_HL, w_result);
            g_reg_PC += 1;
            g_clock = 11;
        }
        private void op_INC_IXD()
        {
            ushort w_addr = (ushort)(g_reg_IX + g_opcode3);
            byte w_val = read_byte(w_addr);
            byte w_result = M_INC(w_val);
            write_byte(w_addr, w_result);
            g_reg_PC += 3;
            g_clock = 23;
        }
        private void op_INC_IYD()
        {
            ushort w_addr = (ushort)(g_reg_IY + g_opcode3);
            byte w_val = read_byte(w_addr);
            byte w_result = M_INC(w_val);
            write_byte(w_addr, w_result);
            g_reg_PC += 3;
            g_clock = 23;
        }

        //--------------------------------------
        private void op_ADC_a_r()
        {
            byte w_val = read_reg(g_opcode1_210);
            g_reg_A = M_ADD(g_reg_A, w_val, (byte)g_flag_C);
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_ADC_a_n()
        {
            byte w_val = g_opcode2;
            g_reg_A = M_ADD(g_reg_A, w_val, (byte)g_flag_C);
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_ADC_a_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            g_reg_A = M_ADD(g_reg_A, w_val, (byte)g_flag_C);
            g_reg_PC += 1;
            g_clock = 7;
        }
        private void op_ADC_a_IXD()
        {
            byte w_val = read_byte((ushort)(g_reg_IX + g_opcode3));
            g_reg_A = M_ADD(g_reg_A, w_val, (byte)g_flag_C);
            g_reg_PC += 3;
            g_clock = 19;
        }
        private void op_ADC_a_IYD()
        {
            byte w_val = read_byte((ushort)(g_reg_IY + g_opcode3));
            g_reg_A = M_ADD(g_reg_A, w_val, (byte)g_flag_C);
            g_reg_PC += 3;
            g_clock = 19;
        }
        //--------------------------------------
        private void op_SUB_a_r()
        {
            byte w_val = read_reg(g_opcode1_210);
            g_reg_A = M_SUB(g_reg_A, w_val, 0);
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_SUB_a_n()
        {
            byte w_val = g_opcode2;
            g_reg_A = M_SUB(g_reg_A, w_val, 0);
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_SUB_a_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            g_reg_A = M_SUB(g_reg_A, w_val, 0);
            g_reg_PC += 1;
            g_clock = 7;
        }
        private void op_SUB_a_IXD()
        {
            byte w_val = read_byte((ushort)(g_reg_IX + g_opcode3));
            g_reg_A = M_SUB(g_reg_A, w_val, 0);
            g_reg_PC += 3;
            g_clock = 19;
        }
        private void op_SUB_a_IYD()
        {
            byte w_val = read_byte((ushort)(g_reg_IY + g_opcode3));
            g_reg_A = M_SUB(g_reg_A, w_val, 0);
            g_reg_PC += 3;
            g_clock = 19;
        }
        private void op_DEC_r()
        {
            byte w_val = read_reg(g_opcode1_543);
            byte w_result = M_DEC(w_val);
            write_reg(g_opcode1_543, w_result);
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_DEC_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            byte w_result = M_DEC(w_val);
            write_byte(g_reg_HL, w_result);
            g_reg_PC += 1;
            g_clock = 11;
        }
        private void op_DEC_IXD()
        {
            ushort w_addr = (ushort)(g_reg_IX + g_opcode3);
            byte w_val = read_byte(w_addr);
            byte w_result = M_DEC(w_val);
            write_byte(w_addr, w_result);
            g_reg_PC += 3;
            g_clock = 23;
        }
        private void op_DEC_IYD()
        {
            ushort w_addr = (ushort)(g_reg_IY + g_opcode3);
            byte w_val = read_byte(w_addr);
            byte w_result = M_DEC(w_val);
            write_byte(w_addr, w_result);
            g_reg_PC += 3;
            g_clock = 23;
        }
        //--------------------------------------
        private void op_SBC_a_r()
        {
            byte w_val = read_reg(g_opcode1_210);
            g_reg_A = M_SUB(g_reg_A, w_val, (byte)g_flag_C);
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_SBC_a_n()
        {
            byte w_val = g_opcode2;
            g_reg_A = M_SUB(g_reg_A, w_val, (byte)g_flag_C);
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_SBC_a_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            g_reg_A = M_SUB(g_reg_A, w_val, (byte)g_flag_C);
            g_reg_PC += 1;
            g_clock = 7;
        }
        private void op_SBC_a_IXD()
        {
            byte w_val = read_byte((ushort)(g_reg_IX + g_opcode3));
            g_reg_A = M_SUB(g_reg_A, w_val, (byte)g_flag_C);
            g_reg_PC += 3;
            g_clock = 19;
        }
        private void op_SBC_a_IYD()
        {
            byte w_val = read_byte((ushort)(g_reg_IY + g_opcode3));
            g_reg_A = M_SUB(g_reg_A, w_val, (byte)g_flag_C);
            g_reg_PC += 3;
            g_clock = 19;
        }
        //--------------------------------------
        private ushort M_ADD_W(ushort in_val1, ushort in_val2)
        {
            int w_result = in_val1 + in_val2;
            int carrybits = in_val1 ^ in_val2 ^ w_result;
            ushort w_result2 = (ushort)(w_result);
            set_flag_h(((in_val1 & 0xfff) + (in_val2 & 0xfff) + g_flag_C) > 0xfff);
            set_flag_c(w_result > 0xffff);
            g_flag_N = 0;
            return (ushort)w_result2;
        }
        private ushort M_ADC_W(ushort in_val1, ushort in_val2)
        {
            in_val2 = (ushort)(in_val2 + g_flag_C);
            int w_result = in_val1 + in_val2;
            ushort w_result2 = (ushort)(w_result);
            set_flag_s((w_result2 & 0x8000) > 0);
            set_flag_z(0 == w_result2);
            set_flag_h(((in_val1 & 0xfff) + (in_val2 & 0xfff) + g_flag_C) > 0xfff);
            set_flag_pv(((in_val1 ^ w_result) & 0x8000) != 0 && ((in_val1 ^ in_val2) & 0x8000) == 0);
            set_flag_c(w_result > 0xffff);
            g_flag_N = 0;
            return (ushort)w_result2;
        }

        private ushort M_SBC_W(ushort in_val1, ushort in_val2)
        {
            in_val2 = (ushort)(in_val2 + g_flag_C);
            int w_result = in_val1 - in_val2;
            ushort w_result2 = (ushort)(w_result & 0xffff);
            set_flag_s((w_result2 & 0x8000) > 0);
            set_flag_z(w_result2 == 0);
            set_flag_h(((in_val1 & 0xfff) < ((in_val2 & 0xfff) + g_flag_C)));
            set_flag_pv(((in_val1 ^ in_val2) & 0x8000) != 0 && ((in_val1 ^ w_result) & 0x8000) != 0);
            set_flag_c(w_result2 > in_val1);
            g_flag_N = 1;
            return w_result2;
        }
        //--------------------------------------
        private void op_ADD_hl_rp()
        {
            ushort w_val1 = g_reg_HL;
            ushort w_val2 = read_rp(g_opcode1_54);
            ushort w_result = M_ADD_W(w_val1, w_val2);
            write_rp((byte)RP_TYPE.HL, w_result);
            g_reg_PC += 1;
            g_clock = 11;
        }
        private void op_ADC_hl_rp()
        {
            ushort w_val1 = g_reg_HL;
            ushort w_val2 = read_rp(g_opcode2_54);
            ushort w_result = M_ADC_W(w_val1, w_val2);
            write_rp((byte)RP_TYPE.HL, w_result);
            g_reg_PC += 2;
            g_clock = 15;
        }
        private void op_ADD_ix_rp()
        {
            ushort w_val1 = g_reg_IX;
            ushort w_val2 = read_rpix(g_opcode2_54);
            g_reg_IX = M_ADD_W(w_val1, w_val2);
            g_reg_PC += 2;
            g_clock = 15;
        }
        private void op_ADD_iy_rp()
        {
            ushort w_val1 = g_reg_IY;
            ushort w_val2 = read_rpiy(g_opcode2_54);
            g_reg_IY = M_ADD_W(w_val1, w_val2);
            g_reg_PC += 2;
            g_clock = 15;
        }
        private void op_INC_rp()
        {
            byte w_reg = g_opcode1_54;
            write_rp(w_reg, (ushort)(read_rp(w_reg) + 1));
            g_reg_PC += 1;
            g_clock = 6;
        }
        private void op_INC_ix()
        {
            g_reg_IX += 1;
            g_reg_PC += 2;
            g_clock = 10;
        }
        private void op_INC_iy()
        {
            g_reg_IY += 1;
            g_reg_PC += 2;
            g_clock = 13;
        }
        //--------------------------------------
        private void op_SBC_hl_rp()
        {
            ushort w_val1 = g_reg_HL;
            ushort w_val2 = read_rp(g_opcode2_54);
            ushort w_result = M_SBC_W(w_val1, w_val2);
            write_rp((byte)RP_TYPE.HL, w_result);
            g_reg_PC += 2;
            g_clock = 15;
        }
        private void op_DEC_rp()
        {
            ushort w_val1 = read_rp(g_opcode1_54);
            write_rp(g_opcode1_54, (ushort)(w_val1 - 1));
            g_reg_PC += 1;
            g_clock = 6;
        }
        private void op_DEC_ix()
        {
            g_reg_IX -= 1;
            g_reg_PC += 2;
            g_clock = 10;
        }
        private void op_DEC_iy()
        {
            g_reg_IY -= 1;
            g_reg_PC += 2;
            g_clock = 10;
        }
        //--------------------------------------
        private void M_AND(byte in_val)
        {
            g_reg_A = (byte)(g_reg_A & in_val);
            set_flag_s((g_reg_A & 0x80) > 0);
            set_flag_z(g_reg_A == 0);
            set_flag_pv_logical(g_reg_A);
            g_flag_N = 0;
            g_flag_C = 0;
            g_flag_H = 1;
        }
        private void M_OR(byte in_val)
        {
            g_reg_A = (byte)(g_reg_A | in_val);
            set_flag_s((g_reg_A & 0x80) > 0);
            set_flag_z(g_reg_A == 0);
            set_flag_pv_logical(g_reg_A);
            g_flag_N = 0;
            g_flag_C = 0;
            g_flag_H = 0;
        }
        private void M_XOR(byte in_val)
        {
            g_reg_A = (byte)(g_reg_A ^ in_val);
            set_flag_s((g_reg_A & 0x80) > 0);
            set_flag_z(g_reg_A == 0);
            set_flag_pv_logical(g_reg_A);
            g_flag_N = 0;
            g_flag_C = 0;
            g_flag_H = 0;
        }

        //--------------------------------------
        private void op_AND_r()
        {
            byte w_val = read_reg(g_opcode1_210);
            M_AND(w_val);
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_AND_n()
        {
            byte w_val = g_opcode2;
            M_AND(w_val);
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_AND_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            M_AND(w_val);
            g_reg_PC += 1;
            g_clock = 7;
        }
        private void op_AND_IXD()
        {
            byte w_val = read_byte((ushort)(g_reg_IX + g_opcode3));
            M_AND(w_val);
            g_reg_PC += 3;
            g_clock = 19;
        }
        private void op_AND_IYD()
        {
            byte w_val = read_byte((ushort)(g_reg_IY + g_opcode3));
            M_AND(w_val);
            g_reg_PC += 3;
            g_clock = 19;
        }
        //--------------------------------------
        private void op_OR_r()
        {
            byte w_val = read_reg(g_opcode1_210);
            M_OR(w_val);
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_OR_n()
        {
            byte w_val = g_opcode2;
            M_OR(w_val);
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_OR_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            M_OR(w_val);
            g_reg_PC += 1;
            g_clock = 7;
        }
        private void op_OR_IXD()
        {
            byte w_val = read_byte((ushort)(g_reg_IX + g_opcode3));
            M_OR(w_val);
            g_reg_PC += 3;
            g_clock = 19;
        }
        private void op_OR_IYD()
        {
            byte w_val = read_byte((ushort)(g_reg_IY + g_opcode3));
            M_OR(w_val);
            g_reg_PC += 3;
            g_clock = 19;
        }
        //--------------------------------------
        private void op_XOR_r()
        {
            byte w_val = read_reg(g_opcode1_210);
            M_XOR(w_val);
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_XOR_n()
        {
            byte w_val = g_opcode2;
            M_XOR(w_val);
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_XOR_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            M_XOR(w_val);
            g_reg_PC += 1;
            g_clock = 7;
        }
        private void op_XOR_IXD()
        {
            byte w_val = read_byte((ushort)(g_reg_IX + g_opcode3));
            M_XOR(w_val);
            g_reg_PC += 3;
            g_clock = 19;
        }
        private void op_XOR_IYD()
        {
            byte w_val = read_byte((ushort)(g_reg_IY + g_opcode3));
            M_XOR(w_val);
            g_reg_PC += 3;
            g_clock = 19;
        }
        //--------------------------------------
        private void op_CPL()
        {
            g_reg_A = (byte)~g_reg_A;
            g_flag_H = 1;
            g_flag_N = 1;
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_NEG()
        {
            byte w_val = g_reg_A;
            g_reg_A = (byte)-w_val;
            set_flag_s((g_reg_A & 0x80) > 0);
            set_flag_z(g_reg_A == 0);
            set_flag_pv(w_val == 0x80);
            set_flag_h((w_val & 0xf) != 0);
            set_flag_c(g_reg_A != 0);
            g_flag_N = 1;
            g_reg_PC += 2;
            g_clock = 8;
        }
        private void op_CCF()
        {
            g_flag_H = g_flag_C;
            g_flag_C = (g_flag_C == 1) ? 0 : 1;
            g_flag_N = 0;
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_SCF()
        {
            g_flag_C = 1;
            g_flag_H = 0;
            g_flag_N = 0;
            g_reg_PC += 1;
            g_clock = 4;
        }
        //--------------------------------------
        private void M_BIT(byte in_val1, byte in_val2)
        {
            byte w_val2 = (byte)(1 << in_val2);
            if ((in_val1 & w_val2) == 0) g_flag_Z = 1; else g_flag_Z = 0;
            g_flag_H = 1;
            g_flag_N = 0;
        }
        private void op_BIT_b_r()
        {
            byte w_val1 = read_reg(g_opcode2_210);
            M_BIT(w_val1, g_opcode2_543);
            g_reg_PC += 2;
            g_clock = 8;
        }
        private void op_BIT_b_HL()
        {
            byte w_val1 = read_byte(g_reg_HL);
            M_BIT(w_val1, g_opcode2_543);
            g_reg_PC += 2;
            g_clock = 12;
        }
        private void op_BIT_b_IXD()
        {
            byte w_val1 = read_byte((ushort)(g_reg_IX + g_opcode3));
            M_BIT(w_val1, g_opcode4_543);
            g_reg_PC += 4;
            g_clock = 20;
        }
        private void op_BIT_b_IYD()
        {
            byte w_val1 = read_byte((ushort)(g_reg_IY + g_opcode3));
            M_BIT(w_val1, g_opcode4_543);
            g_reg_PC += 4;
            g_clock = 20;
        }
        //--------------------------------------
        private void op_SET_b_r()
        {
            byte w_val1 = read_reg(g_opcode2_210);
            byte w_val2 = (byte)(1 << g_opcode2_543);
            write_reg(g_opcode2_210, (byte)(w_val1 | w_val2));
            g_reg_PC += 2;
            g_clock = 8;
        }
        private void op_SET_b_HL()
        {
            byte w_val1 = read_byte(g_reg_HL);
            byte w_val2 = (byte)(1 << g_opcode2_543);
            write_byte(g_reg_HL, (byte)(w_val1 | w_val2));
            g_reg_PC += 2;
            g_clock = 15;
        }
        private void op_SET_b_IXD()
        {
            ushort w_addr = (ushort)(g_reg_IX + g_opcode3);
            byte w_val1 = read_byte(w_addr);
            byte w_val2 = (byte)(1 << g_opcode4_543);
            write_byte(w_addr, (byte)(w_val1 | w_val2));
            g_reg_PC += 4;
            g_clock = 23;
        }
        private void op_SET_b_IYD()
        {
            ushort w_addr = (ushort)(g_reg_IY + g_opcode3);
            byte w_val1 = read_byte(w_addr);
            byte w_val2 = (byte)(1 << g_opcode4_543);
            write_byte(w_addr, (byte)(w_val1 | w_val2));
            g_reg_PC += 4;
            g_clock = 23;
        }
        private void op_RES_b_r()
        {
            byte w_reg = g_opcode2_210;
            byte w_val1 = read_reg(w_reg);
            byte w_val2 = (byte)~(1 << g_opcode2_543);
            write_reg(w_reg, (byte)(w_val1 & w_val2));
            g_reg_PC += 2;
            g_clock = 8;
        }
        private void op_RES_b_HL()
        {
            byte w_val1 = read_byte(g_reg_HL);
            byte w_val2 = (byte)~(1 << g_opcode2_543);
            write_byte(g_reg_HL, (byte)(w_val1 & w_val2));
            g_reg_PC += 2;
            g_clock = 15;
        }
        private void op_RES_b_IXD()
        {
            ushort w_addr = (ushort)(g_reg_IX + g_opcode3);
            byte w_val1 = read_byte(w_addr);
            byte w_val2 = (byte)~(1 << g_opcode4_543);
            write_byte(w_addr, (byte)(w_val1 & w_val2));
            g_reg_PC += 4;
            g_clock = 23;
        }
        private void op_RES_b_IYD()
        {
            ushort w_addr = (ushort)(g_reg_IY + g_opcode3);
            byte w_val1 = read_byte(w_addr);
            byte w_val2 = (byte)~(1 << g_opcode4_543);
            write_byte(w_addr, (byte)(w_val1 & w_val2));
            g_reg_PC += 4;
            g_clock = 23;
        }
        //--------------------------------------
        private void M_CP_loop(bool in_count, bool in_loop)
        {
            ushort w_bc = g_reg_BC;
            ushort w_hl = g_reg_HL;
            byte w_val = read_byte(w_hl);
            M_SUB(g_reg_A, w_val, 0, false);
            w_bc -= 1;
            if (in_count == true)
            {
                w_hl += 1;
            }
            else
            {
                w_hl -= 1;
            }
            write_rp((byte)RP_TYPE.BC, w_bc);
            write_rp((byte)RP_TYPE.HL, w_hl);
            set_flag_pv(w_bc != 0);
            if ((in_loop == true) && (w_bc != 0) && (g_flag_Z == 0))
            {
                g_clock += 21;
            }
            else
            {
                g_clock += 16;
                g_reg_PC += 2;

            }
        }
        private void op_CPI() { M_CP_loop(true, false); }
        private void op_CPIR() { M_CP_loop(true, true); }
        private void op_CPD() { M_CP_loop(false, false); }
        private void op_CPDR() { M_CP_loop(false, true); }
        //--------------------------------------
        private void op_CP_r()
        {
            byte w_reg = g_opcode1_210;
            byte w_val1 = read_reg(w_reg);
            M_SUB(g_reg_A, w_val1, 0);
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_CP_n()
        {
            byte w_val1 = g_opcode2;
            M_SUB(g_reg_A, w_val1, 0);
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_CP_HL()
        {
            byte w_val1 = read_byte(g_reg_HL);
            M_SUB(g_reg_A, w_val1, 0);
            g_reg_PC += 1;
            g_clock = 7;
        }
        private void op_CP_IXD()
        {
            ushort w_addr = (ushort)(g_reg_IX + g_opcode3);
            byte w_val1 = read_byte(w_addr);
            M_SUB(g_reg_A, w_val1, 0);
            g_reg_PC += 3;
            g_clock = 7;
        }
        private void op_CP_IYD()
        {
            ushort w_addr = (ushort)(g_reg_IY + g_opcode3);
            byte w_val1 = read_byte(w_addr);
            M_SUB(g_reg_A, w_val1, 0);
            g_reg_PC += 3;
            g_clock = 7;
        }
        //--------------------------------------
        private void op_JP_nn()
        {
            g_reg_PC = g_opcode23;
            g_clock = 10;
        }
        private void op_JP_cc_nn()
        {
            ushort w_addr = g_opcode23;
            if (true == chk_condion(g_opcode1_543))
            {
                g_reg_PC = w_addr;
            }
            else
            {
                g_reg_PC += 3;
            }
            g_clock = 10;
        }
        private void op_JR_e()
        {
            g_reg_PC = (ushort)((int)g_reg_PC + (sbyte)g_opcode2);
            g_reg_PC += 2;
            g_clock = 12;
        }
        private void op_JR_c_e()
        {
            if (g_flag_C == 1)
            {
                g_reg_PC = (ushort)((int)g_reg_PC + (sbyte)g_opcode2);
            }
            g_reg_PC += 2;
            g_clock = 12;
        }
        private void op_JR_nc_e()
        {
            if (g_flag_C == 0)
            {
                g_reg_PC = (ushort)((int)g_reg_PC + (sbyte)g_opcode2);
            }
            g_reg_PC += 2;
            g_clock = 12;
        }
        private void op_JR_z_e()
        {
            if (g_flag_Z == 1)
            {
                g_reg_PC = (ushort)((int)g_reg_PC + (sbyte)g_opcode2);
            }
            g_reg_PC += 2;
            g_clock = 12;
        }
        private void op_JR_nz_e()
        {
            if (g_flag_Z == 0)
            {
                g_reg_PC = (ushort)((int)g_reg_PC + (sbyte)g_opcode2);
            }
            g_reg_PC += 2;
            g_clock = 12;
        }
        private void op_JP_HL()
        {
            g_reg_PC = g_reg_HL;
            g_clock = 4;
        }
        private void op_JP_IX()
        {
            g_reg_PC = g_reg_IX;
            g_clock = 8;
        }
        private void op_JP_IY()
        {
            g_reg_PC = g_reg_IY;
            g_clock = 8;
        }
        private void op_DJNZ()
        {
            g_reg_B -= 1;
            if (g_reg_B != 0)
            {
                g_reg_PC = (ushort)((int)g_reg_PC + (sbyte)g_opcode2);
            }
            g_reg_PC += 2;
            g_clock = 13;
        }
        //--------------------------------------
        private void op_CALL_nn()
        {
            ushort w_pc = (ushort)(g_reg_PC + 3);
            stack_push((byte)((w_pc >> 8)& 0xff));
            stack_push((byte)(w_pc & 0xff));
            g_reg_PC = g_opcode23;
            g_clock = 17;
        }
        private void op_CALL_cc_nn()
        {
            if (true == chk_condion(g_opcode1_543))
            {
                ushort w_pc = (ushort)(g_reg_PC + 3);
                stack_push((byte)((w_pc >> 8) & 0xff));
                stack_push((byte)(w_pc & 0xff));
                g_reg_PC = g_opcode23;
                g_clock = 17;
            }
            else
            {
                g_reg_PC += 3;
                g_clock = 10;
            }
        }
        private void op_RET()
        {
            g_write_PCL(stack_pop());
            g_write_PCH(stack_pop());
            g_clock = 10;
        }
        private void op_RET_cc()
        {
            if (chk_condion(g_opcode1_543))
            {
                g_write_PCL(stack_pop());
                g_write_PCH(stack_pop());
                g_clock += 11;
            }
            else
            {
                g_reg_PC += 1;
                g_clock += 5;
            }
        }
        private void op_RETI()
        {
            g_write_PCL(stack_pop());
            g_write_PCH(stack_pop());
            g_IFF1 = g_IFF2;
            g_clock = 15;
        }
        private void op_RETN()
        {
            g_write_PCL(stack_pop());
            g_write_PCH(stack_pop());
            g_IFF1 = g_IFF2;
            g_clock = 14;
        }
        private void op_RST()
        {
            g_reg_SP -= 2;
            ushort w_pc = (ushort)(g_reg_PC + 1);
            write_word(g_reg_SP, w_pc);
            g_reg_PC = (ushort)(g_opcode1_543 << 3);
            g_clock = 12;
        }
        //--------------------------------------
        private void op_NOP()
        {
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_HALT()
        {
            g_clock = 4;
        }
        private void op_DI()
        {
            g_IFF1 = false;
            g_IFF2 = false;
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_EI()
        {
            g_IFF1 = true;
            g_IFF2 = true;
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_IM0()
        {
            g_interruptMode = 0;
            g_reg_PC += 2;
            g_clock = 8;
        }
        private void op_IM1()
        {
            g_interruptMode = 1;
            g_reg_PC += 2;
            g_clock = 8;
        }
        private void op_IM2()
        {
            g_interruptMode = 2;
            g_reg_PC += 2;
            g_clock = 8;
        }
        //--------------------------------------
        private void op_IN_a_N()
        {
            g_reg_A = 0xff;
            g_reg_PC += 2;
            g_clock = 11;
        }
        private void op_IN_r_C()
        {
            g_clock = 12;
        }
        private void op_INI()
        {
            g_clock = 16;
        }
        private void op_INIR()
        {
            g_clock = 21;
            g_clock = 16;
        }
        private void op_IND()
        {
            g_clock = 16;
        }
        private void op_INDR()
        {
            g_clock = 21;
            g_clock = 16;
        }
        //--------------------------------------
        private void op_OUT_N_a()
        {
            Console.Write((char)g_reg_A);
            g_reg_PC += 2;
            g_clock = 11;
        }
        private void op_OUT_C_r()
        {
            g_clock = 12;
        }
        private void op_OUTI()
        {
            g_clock = 16;
        }
        private void op_OUTIR()
        {
            g_clock = 21;
            g_clock = 16;
        }
        private void op_OUTD()
        {
            g_clock = 16;
        }
        private void op_OUTDR()
        {
            g_clock = 21;
            g_clock = 16;
        }
        //--------------------------------------
        private void op_DAA()
        {
            byte w_add = 0;
            if ((g_flag_C == 1) || (g_reg_A > 0x99)) w_add = 0x60;
            if ((g_flag_H == 1) || ((g_reg_A & 0x0f) > 0x09)) w_add += 0x06;
            byte w_val_a = g_reg_A;
            w_val_a += (byte)((g_flag_N == 0) ? w_add : -w_add);
            set_flag_s((w_val_a & 0x80) > 0);
            set_flag_z(w_val_a == 0);
            set_flag_h((((g_reg_A ^ w_val_a) & 0x10) >> 4) == 1);
            set_flag_pv_logical(w_val_a);
            if (g_reg_A > 0x99) g_flag_C = 1;
            g_reg_A = w_val_a;
            g_reg_PC += 1;
            g_clock = 4;
        }
        private void op_RLD()
        {
            byte w_reg_a = g_reg_A;
            byte w_val = read_byte(g_reg_HL);
            g_reg_A = (byte)(((byte)(w_reg_a & 0xf0)) | (((byte)(w_val & 0xf0)) >> 4));
            write_byte(g_reg_HL, (byte)((w_reg_a & 0x0f) | ((w_val & 0x0f) << 4)));
            set_flag_s((g_reg_A & 0x80) > 0);
            set_flag_z(g_reg_A == 0);
            g_flag_H = 0;
            g_flag_N = 0;
            set_flag_pv_logical(g_reg_A);
            g_reg_PC += 2;
            g_clock = 18;
        }
        private void op_RRD()
        {
            byte w_reg_a = g_reg_A;
            byte w_val = read_byte(g_reg_HL);
            g_reg_A = (byte)((w_reg_a & 0xf0) | (w_val & 0x0f));
            write_byte(g_reg_HL, (byte)(((w_reg_a & 0x0f) << 4) | ((w_val & 0xf0) >> 4)));
            set_flag_s((g_reg_A & 0x80) > 0);
            set_flag_z(g_reg_A == 0);
            g_flag_H = 0;
            g_flag_N = 0;
            set_flag_pv_logical(g_reg_A);
            g_reg_PC += 2;
            g_clock = 18;
        }
    }
}

