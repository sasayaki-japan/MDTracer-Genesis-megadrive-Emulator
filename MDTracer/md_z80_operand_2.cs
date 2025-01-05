using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDTracer
{
    internal partial class md_z80
    {
        private void op_ADD_IXH()
        {
            g_reg_A = M_ADD(g_reg_A, g_reg_IXH, 0);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_ADD_IXL()
        {
            g_reg_A = M_ADD(g_reg_A, g_reg_IXL, 0);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_ADD_IYH()
        {
            g_reg_A = M_ADD(g_reg_A, g_reg_IYH, 0);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_ADD_IYL()
        {
            g_reg_A = M_ADD(g_reg_A, g_reg_IYL, 0);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_ADC_IXH()
        {
            g_reg_A = M_ADD(g_reg_A, g_reg_IXH, (byte)g_flag_C);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_ADC_IXL()
        {
            g_reg_A = M_ADD(g_reg_A, g_reg_IXL, (byte)g_flag_C);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_ADC_IYH()
        {
            g_reg_A = M_ADD(g_reg_A, g_reg_IYH, (byte)g_flag_C);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_ADC_IYL()
        {
            g_reg_A = M_ADD(g_reg_A, g_reg_IYL, (byte)g_flag_C);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_SUB_IXH()
        {
            g_reg_A = M_SUB(g_reg_A, g_reg_IXH, 0);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_SUB_IXL()
        {
            g_reg_A = M_SUB(g_reg_A, g_reg_IXL, 0);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_SUB_IYH()
        {
            g_reg_A = M_SUB(g_reg_A, g_reg_IYH, 0);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_SUB_IYL()
        {
            g_reg_A = M_SUB(g_reg_A, g_reg_IYL, 0);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_SBC_IXH()
        {
            g_reg_A = M_SUB(g_reg_A, g_reg_IXH, (byte)g_flag_C);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_SBC_IXL()
        {
            g_reg_A = M_SUB(g_reg_A, g_reg_IXL, (byte)g_flag_C);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_SBC_IYH()
        {
            g_reg_A = M_SUB(g_reg_A, g_reg_IYH, (byte)g_flag_C);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_SBC_IYL()
        {
            g_reg_A = M_SUB(g_reg_A, g_reg_IYL, (byte)g_flag_C);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_AND_IXH()
        {
            M_AND(g_reg_IXH);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_AND_IXL()
        {
            M_AND(g_reg_IXL);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_AND_IYH()
        {
            M_AND(g_reg_IYH);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_AND_IYL()
        {
            M_AND(g_reg_IYL);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_XOR_IXH()
        {
            M_XOR(g_reg_IXH);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_XOR_IXL()
        {
            M_XOR(g_reg_IXL);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_XOR_IYH()
        {
            M_XOR(g_reg_IYH);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_XOR_IYL()
        {
            M_XOR(g_reg_IYL);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_OR_IXH()
        {
            M_OR(g_reg_IXH);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_OR_IXL()
        {
            M_OR(g_reg_IXL);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_OR_IYH()
        {
            M_OR(g_reg_IYH);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_OR_IYL()
        {
            M_OR(g_reg_IYL);
            g_reg_PC += 2;
            g_clock = 4;
        }

        private void op_CP_IXH()
        {
            M_SUB(g_reg_A, g_reg_IXH, 0);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_CP_IXL()
        {
            M_SUB(g_reg_A, g_reg_IXL, 0);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_CP_IYH()
        {
            M_SUB(g_reg_A, g_reg_IYH, 0);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_CP_IYL()
        {
            M_SUB(g_reg_A, g_reg_IYL, 0);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_INC_IXH()
        {
            byte w_result = M_INC(g_reg_IXH);
            g_reg_IX = (ushort)((w_result << 8) + g_reg_IXL);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_INC_IXL()
        {
            byte w_result = M_INC(g_reg_IXL);
            g_reg_IX = (ushort)((g_reg_IXH << 8) + w_result);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_INC_IYH()
        {
            byte w_result = M_INC(g_reg_IYH);
            g_reg_IY = (ushort)((w_result << 8) + g_reg_IYL);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_INC_IYL()
        {
            byte w_result = M_INC(g_reg_IYL);
            g_reg_IY = (ushort)((g_reg_IYH << 8) + w_result);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_DEC_IXH()
        {
            byte w_result = M_DEC(g_reg_IXH);
            g_reg_IX = (ushort)((w_result << 8) + g_reg_IXL);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_DEC_IXL()
        {
            byte w_result = M_DEC(g_reg_IXL);
            g_reg_IX = (ushort)((g_reg_IXH << 8) + w_result);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_DEC_IYH()
        {
            byte w_result = M_DEC(g_reg_IYH);
            g_reg_IY = (ushort)((w_result << 8) + g_reg_IYL);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_DEC_IYL()
        {
            byte w_result = M_DEC(g_reg_IYL);
            g_reg_IY = (ushort)((g_reg_IYH << 8) + w_result);
            g_reg_PC += 2;
            g_clock = 4;
        }

        private void op_LD_IXH_N()
        {
            g_write_IXH(g_opcode3);
            g_reg_PC += 3;
            g_clock = 4;
        }
        private void op_LD_IXL_N()
        {
            g_write_IXL(g_opcode3);
            g_reg_PC += 3;
            g_clock = 4;
        }
        private void op_LD_IYH_N()
        {
            g_write_IYH(g_opcode3);
            g_reg_PC += 3;
            g_clock = 4;
        }
        private void op_LD_IYL_N()
        {
            g_write_IYL(g_opcode3);
            g_reg_PC += 3;
            g_clock = 4;
        }
        private void op_SLL_r()
        {
            byte w_val = read_reg(g_opcode2_210);
            w_val = M_SLL(w_val);
            write_reg(g_opcode2_210, w_val);
            g_reg_PC += 2;
            g_clock = 8;
        }
        private void op_SLL_HL()
        {
            byte w_val = read_byte(g_reg_HL);
            w_val = M_SLL(w_val);
            write_byte(g_reg_HL, w_val);
            g_reg_PC += 2;
            g_clock = 15;
        }
        /*
        private void op_RLC_IX_LD_r()
        {
            g_reg_PC += 4;
            g_clock = 4;
        }
        private void op_RLC_IY_LD_r()
        {
            g_reg_PC += 4;
            g_clock = 4;
        }
        private void op_RRC_IX_LD_r()
        {
            g_reg_PC += 4;
            g_clock = 4;
        }
        private void op_RRC_IY_LD_r()
        {
            g_reg_PC += 4;
            g_clock = 4;
        }
        */
        private void op_SLL_IXD()
        {
            ushort w_addr = (ushort)(g_reg_IX + g_opcode3);
            byte w_val = read_byte(w_addr);
            w_val = M_SLL(w_val);
            write_byte(w_addr, w_val);
            g_reg_PC += 4;
            g_clock = 4;
        }
        private void op_SLL_IYD()
        {
            ushort w_addr = (ushort)(g_reg_IY + g_opcode3);
            byte w_val = read_byte(w_addr);
            w_val = M_SLL(w_val);
            write_byte(w_addr, w_val);
            g_reg_PC += 4;
            g_clock = 4;
        }
        private void op_LD_A_r()
        {
            g_reg_A = read_reg(g_opcode2_210);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_LD_B_r()
        {
            g_reg_B = read_reg(g_opcode2_210);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_LD_C_r()
        {
            g_reg_C = read_reg(g_opcode2_210);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_LD_D_r()
        {
            g_reg_D = read_reg(g_opcode2_210);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_LD_E_r()
        {
            g_reg_E = read_reg(g_opcode2_210);
            g_reg_PC += 2;
            g_clock = 4;
        }
        private void op_LD_r_IXH()
        {
            switch (g_opcode2)
            {
                case 0x7c: g_reg_A = g_reg_IXH; break;
                case 0x44: g_reg_B = g_reg_IXH; break;
                case 0x4c: g_reg_C = g_reg_IXH; break;
                case 0x54: g_reg_D = g_reg_IXH; break;
                case 0x5c: g_reg_E = g_reg_IXH; break;
            }
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_LD_r_IXL()
        {
            switch (g_opcode2)
            {
                case 0x7d: g_reg_A = g_reg_IXL; break;
                case 0x45: g_reg_B = g_reg_IXL; break;
                case 0x4d: g_reg_C = g_reg_IXL; break;
                case 0x55: g_reg_D = g_reg_IXL; break;
                case 0x5d: g_reg_E = g_reg_IXL; break;
            }
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_LD_r_IYH()
        {
            switch (g_opcode2)
            {
                case 0x7c: g_reg_A = g_reg_IYH; break;
                case 0x44: g_reg_B = g_reg_IYH; break;
                case 0x4c: g_reg_C = g_reg_IYH; break;
                case 0x54: g_reg_D = g_reg_IYH; break;
                case 0x5c: g_reg_E = g_reg_IYH; break;
            }
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_LD_r_IYL()
        {
            switch (g_opcode2)
            {
                case 0x7d: g_reg_A = g_reg_IYL; break;
                case 0x45: g_reg_B = g_reg_IYL; break;
                case 0x4d: g_reg_C = g_reg_IYL; break;
                case 0x55: g_reg_D = g_reg_IYL; break;
                case 0x5d: g_reg_E = g_reg_IYL; break;
            }
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_LD_IXH_r()
        {
            switch (g_opcode2)
            {
                case 0x60: g_write_IXH(g_reg_B); break;
                case 0x61: g_write_IXH(g_reg_C); break;
                case 0x62: g_write_IXH(g_reg_D); break;
                case 0x63: g_write_IXH(g_reg_E); break;
                case 0x67: g_write_IXH(g_reg_A); break;
            }
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_LD_IXL_r()
        {
            switch (g_opcode2)
            {
                case 0x68: g_write_IXL(g_reg_B); break;
                case 0x69: g_write_IXL(g_reg_C); break;
                case 0x6a: g_write_IXL(g_reg_D); break;
                case 0x6b: g_write_IXL(g_reg_E); break;
                case 0x6f: g_write_IXL(g_reg_A); break;
            }
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_LD_IYH_r()
        {
            switch (g_opcode2)
            {
                case 0x60: g_write_IYH(g_reg_B); break;
                case 0x61: g_write_IYH(g_reg_C); break;
                case 0x62: g_write_IYH(g_reg_D); break;
                case 0x63: g_write_IYH(g_reg_E); break;
                case 0x67: g_write_IYH(g_reg_A); break;
            }
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_LD_IYL_r()
        {
            switch (g_opcode2)
            {
                case 0x68: g_write_IYL(g_reg_B); break;
                case 0x69: g_write_IYL(g_reg_C); break;
                case 0x6a: g_write_IYL(g_reg_D); break;
                case 0x6b: g_write_IYL(g_reg_E); break;
                case 0x6f: g_write_IYL(g_reg_A); break;
            }
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_LD_IXn_IXn()
        {
            if (g_opcode2 == 0x65)
            {
                g_write_IXH(g_reg_IXL);
            }
            else
            if (g_opcode2 == 0x6c)
            {
                g_write_IXL(g_reg_IXH);
            }
            g_reg_PC += 2;
            g_clock = 7;
        }
        private void op_LD_IYn_IYn()
        {
            if (g_opcode2 == 0x65)
            {
                g_write_IYH(g_reg_IYL);
            }
            else
            if (g_opcode2 == 0x6c)
            {
                g_write_IYL(g_reg_IYH);
            }
            g_reg_PC += 2;
            g_clock = 7;
        }
    }
}
