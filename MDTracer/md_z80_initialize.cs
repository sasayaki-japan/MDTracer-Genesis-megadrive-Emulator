using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MDTracer
{
    internal partial class md_z80
    {
        private Action[] g_operand;
        private Action[] g_operand_dd;
        private Action[] g_operand_ddcb;
        private Action[] g_operand_fd;
        private Action[] g_operand_fdcb;
        private Action[] g_operand_ed;
        private Action[] g_operand_cb;
        public void initialize()
        {
            g_ram = new byte[65536];
            reset();

            g_operand = new Action[]
            {
                //0x00
                op_NOP, op_LD_rp_nn, op_LD_BC_a, op_INC_rp, op_INC_r, op_DEC_r, op_LD_r_n, op_RLCA,
                op_EX_af_af2, op_ADD_hl_rp, op_LD_a_BC, op_DEC_rp, op_INC_r, op_DEC_r, op_LD_r_n, op_RRCA,
                //0x10
                op_DJNZ, op_LD_rp_nn, op_LD_DE_a, op_INC_rp, op_INC_r, op_DEC_r, op_LD_r_n, op_RLA,
                op_JR_e, op_ADD_hl_rp, op_LD_a_DE, op_DEC_rp, op_INC_r, op_DEC_r, op_LD_r_n, op_RRA,
                //0x20
                op_JR_nz_e, op_LD_rp_nn, op_LD_NN_hl, op_INC_rp, op_INC_r, op_DEC_r, op_LD_r_n, op_DAA,
                op_JR_z_e, op_ADD_hl_rp, op_LD_hl_NN, op_DEC_rp, op_INC_r, op_DEC_r, op_LD_r_n, op_CPL,
                //0x30
                op_JR_nc_e, op_LD_rp_nn, op_LD_NN_a, op_INC_rp, op_INC_HL, op_DEC_HL, op_LD_HL_n, op_SCF,
                op_JR_c_e, op_ADD_hl_rp, op_LD_a_NN, op_DEC_rp, op_INC_r, op_DEC_r, op_LD_r_n, op_CCF,
                //0x40
                op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r_HL, op_LD_r1_r2,
                op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r_HL, op_LD_r1_r2,
                //0x50
                op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r_HL, op_LD_r1_r2,
                op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r_HL, op_LD_r1_r2,
                //0x60
                op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r_HL, op_LD_r1_r2,
                op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r_HL, op_LD_r1_r2,
                //0x70
                op_LD_HL_r, op_LD_HL_r, op_LD_HL_r, op_LD_HL_r, op_LD_HL_r, op_LD_HL_r, op_HALT, op_LD_HL_r,
                op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r1_r2, op_LD_r_HL, op_LD_r1_r2,
                //0x80
                op_ADD_a_r, op_ADD_a_r, op_ADD_a_r, op_ADD_a_r, op_ADD_a_r, op_ADD_a_r, op_ADD_a_HL, op_ADD_a_r,
                op_ADC_a_r, op_ADC_a_r, op_ADC_a_r, op_ADC_a_r, op_ADC_a_r, op_ADC_a_r, op_ADC_a_HL, op_ADC_a_r,
                //0x90
                op_SUB_a_r, op_SUB_a_r, op_SUB_a_r, op_SUB_a_r, op_SUB_a_r, op_SUB_a_r, op_SUB_a_HL, op_SUB_a_r,
                op_SBC_a_r, op_SBC_a_r, op_SBC_a_r, op_SBC_a_r, op_SBC_a_r, op_SBC_a_r, op_SBC_a_HL, op_SBC_a_r,
                //0xa0
                op_AND_r, op_AND_r, op_AND_r, op_AND_r, op_AND_r, op_AND_r, op_AND_HL, op_AND_r,
                op_XOR_r, op_XOR_r, op_XOR_r, op_XOR_r, op_XOR_r, op_XOR_r, op_XOR_HL, op_XOR_r,
                //0xb0
                op_OR_r, op_OR_r, op_OR_r, op_OR_r, op_OR_r, op_OR_r, op_OR_HL, op_OR_r,
                op_CP_r, op_CP_r, op_CP_r, op_CP_r, op_CP_r, op_CP_r, op_CP_HL, op_CP_r,
                //0xc0
                op_RET_cc, op_POP_rp, op_JP_cc_nn, op_JP_nn, op_CALL_cc_nn, op_PUSH_rp, op_ADD_a_n, op_RST,
                op_RET_cc, op_RET, op_JP_cc_nn, op_cb, op_CALL_cc_nn, op_CALL_nn, op_ADC_a_n, op_RST,
                //0xd0
                op_RET_cc, op_POP_rp, op_JP_cc_nn, op_OUT_N_a, op_CALL_cc_nn, op_PUSH_rp, op_SUB_a_n, op_RST,
                op_RET_cc, op_EXX, op_JP_cc_nn, op_IN_a_N, op_CALL_cc_nn, op_dd, op_SBC_a_n, op_RST,
                //0xe0
                op_RET_cc, op_POP_rp, op_JP_cc_nn, op_EX_SP_hl, op_CALL_cc_nn, op_PUSH_rp, op_AND_n, op_RST,
                op_RET_cc, op_JP_HL, op_JP_cc_nn, op_EX_de_hl, op_CALL_cc_nn, op_ed, op_XOR_n, op_RST,
                //0xf0
                op_RET_cc, op_POP_af, op_JP_cc_nn, op_DI, op_CALL_cc_nn, op_PUSH_af, op_OR_n, op_RST,
                op_RET_cc, op_LD_sp_hl, op_JP_cc_nn, op_EI, op_CALL_cc_nn, op_fd, op_CP_n, op_RST,
            };
            g_operand_dd = new Action[]
            {
                //0x00
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_ADD_ix_rp, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0x10
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_ADD_ix_rp, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0x20
                op_NOP, op_LD_ix_nn, op_LD_NN_ix, op_INC_ix, op_INC_IXH, op_DEC_IXH, op_LD_IXH_N, op_NOP,
                op_NOP, op_ADD_ix_rp, op_LD_ix_NN, op_DEC_ix, op_INC_IXL, op_DEC_IXL, op_LD_IXL_N, op_NOP, 
                //0x30
                op_NOP, op_NOP, op_NOP, op_NOP, op_INC_IXD, op_DEC_IXD, op_LD_IXD_n, op_NOP,
                op_NOP, op_ADD_ix_rp, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0x40
                op_LD_B_r, op_LD_B_r, op_LD_B_r, op_LD_B_r, op_LD_r_IXH, op_LD_r_IXL, op_LD_r_IXD, op_LD_B_r,
                op_LD_C_r, op_LD_C_r, op_LD_C_r, op_LD_C_r, op_LD_r_IXH, op_LD_r_IXL, op_LD_r_IXD, op_LD_C_r, 
                //0x50
                op_LD_D_r, op_LD_D_r, op_LD_D_r, op_LD_D_r, op_LD_r_IXH, op_LD_r_IXL, op_LD_r_IXD, op_LD_D_r,
                op_LD_E_r, op_LD_E_r, op_LD_E_r, op_LD_E_r, op_LD_r_IXH, op_LD_r_IXL, op_LD_r_IXD, op_LD_E_r, 
                //0x60
                op_LD_IXH_r, op_LD_IXH_r, op_LD_IXH_r, op_LD_IXH_r, op_LD_IXn_IXn, op_LD_IXn_IXn, op_LD_r_IXD, op_LD_IXH_r,
                op_LD_IXL_r, op_LD_IXL_r, op_LD_IXL_r, op_LD_IXL_r, op_LD_IXn_IXn, op_LD_IXn_IXn, op_LD_r_IXD, op_LD_IXL_r, 
                //0x70
                op_LD_IXD_r, op_LD_IXD_r, op_LD_IXD_r, op_LD_IXD_r, op_LD_IXD_r, op_LD_IXD_r, op_NOP, op_LD_IXD_r,
                op_LD_A_r, op_LD_A_r, op_LD_A_r, op_LD_A_r, op_LD_r_IXH, op_LD_r_IXL, op_LD_r_IXD, op_LD_A_r, 
                //0x80
                op_NOP, op_NOP, op_NOP, op_NOP, op_ADD_IXH, op_ADD_IXL, op_ADD_a_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_ADC_IXH, op_ADC_IXL, op_ADC_a_IXD, op_NOP, 
                //0x90
                op_NOP, op_NOP, op_NOP, op_NOP, op_SUB_IXH, op_SUB_IXL, op_SUB_a_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_SBC_IXH, op_SBC_IXL, op_SBC_a_IXD, op_NOP, 
                //0xa0
                op_NOP, op_NOP, op_NOP, op_NOP, op_AND_IXH, op_AND_IXL, op_AND_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_XOR_IXH, op_XOR_IXL, op_XOR_IXD, op_NOP, 
                //0xb0
                op_NOP, op_NOP, op_NOP, op_NOP, op_OR_IXH, op_OR_IXL, op_OR_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_CP_IXH, op_CP_IXL, op_CP_IXD, op_NOP, 
                //0xc0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_NOP, op_NOP, op_ddcb, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0xd0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0xe0
                op_NOP, op_POP_ix, op_NOP, op_EX_SP_ix, op_NOP, op_PUSH_ix, op_NOP, op_NOP,
                op_NOP, op_JP_IX, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0xf0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_LD_sp_ix, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
            };
            g_operand_ddcb = new Action[]
            {
                //0x00
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RLC_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RRC_IXD, op_NOP,
                //0x10
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RL_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RR_IXD, op_NOP, 
                //0x20
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SLA_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SRA_IXD, op_NOP, 
                //0x30
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SLL_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SRL_IXD, op_NOP, 
                //0x40
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_BIT_b_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_BIT_b_IXD, op_NOP, 
                //0x50
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_BIT_b_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_BIT_b_IXD, op_NOP, 
                //0x60
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_BIT_b_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_BIT_b_IXD, op_NOP, 
                //0x70
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_BIT_b_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_BIT_b_IXD, op_NOP, 
                //0x80
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RES_b_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RES_b_IXD, op_NOP, 
                //0x90
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RES_b_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RES_b_IXD, op_NOP, 
                //0xa0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RES_b_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RES_b_IXD, op_NOP, 
                //0xb0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RES_b_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RES_b_IXD, op_NOP, 
                //0xc0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SET_b_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SET_b_IXD, op_NOP, 
                //0xd0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SET_b_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SET_b_IXD, op_NOP, 
                //0xe0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SET_b_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SET_b_IXD, op_NOP, 
                //0xf0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SET_b_IXD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SET_b_IXD, op_NOP,
            };
            g_operand_fd = new Action[]
{
                //0x00
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_ADD_iy_rp, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0x10
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_ADD_iy_rp, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0x20
                op_NOP, op_LD_iy_nn, op_LD_NN_iy, op_INC_iy, op_INC_IYH, op_DEC_IYH, op_LD_IYH_N, op_NOP,
                op_NOP, op_ADD_iy_rp, op_LD_iy_NN, op_DEC_iy, op_INC_IYL, op_DEC_IYL, op_LD_IYL_N, op_NOP, 
                //0x30
                op_NOP, op_NOP, op_NOP, op_NOP, op_INC_IYD, op_DEC_IYD, op_LD_IYD_n, op_NOP,
                op_NOP, op_ADD_iy_rp, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0x40
                op_LD_B_r, op_LD_B_r, op_LD_B_r, op_LD_B_r, op_LD_r_IYH, op_LD_r_IYL, op_LD_r_IYD, op_LD_B_r,
                op_LD_C_r, op_LD_C_r, op_LD_C_r, op_LD_C_r, op_LD_r_IYH, op_LD_r_IYL, op_LD_r_IYD, op_LD_C_r,
                //0x50
                op_LD_D_r, op_LD_D_r, op_LD_D_r, op_LD_D_r, op_LD_r_IYH, op_LD_r_IYL, op_LD_r_IYD, op_LD_D_r,
                op_LD_E_r, op_LD_E_r, op_LD_E_r, op_LD_E_r, op_LD_r_IYH, op_LD_r_IYL, op_LD_r_IYD, op_LD_E_r,
                //0x60
                op_LD_IYH_r, op_LD_IYH_r, op_LD_IYH_r, op_LD_IYH_r, op_LD_IYn_IYn, op_LD_IYn_IYn, op_LD_r_IYD, op_LD_IYH_r,
                op_LD_IYL_r, op_LD_IYL_r, op_LD_IYL_r, op_LD_IYL_r, op_LD_IYn_IYn, op_LD_IYn_IYn, op_LD_r_IYD, op_LD_IYL_r, 
                //0x70
                op_LD_IYD_r, op_LD_IYD_r, op_LD_IYD_r, op_LD_IYD_r, op_LD_IYD_r, op_LD_IYD_r, op_NOP, op_LD_IYD_r,
                op_LD_A_r, op_LD_A_r, op_LD_A_r, op_LD_A_r, op_LD_r_IYH, op_LD_r_IYL, op_LD_r_IYD, op_LD_A_r,
                //0x80
                op_NOP, op_NOP, op_NOP, op_NOP, op_ADD_IYH , op_ADD_IYL, op_ADD_a_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_ADC_IYH, op_ADC_IYH, op_ADC_a_IYD, op_NOP, 
                //0x90
                op_NOP, op_NOP, op_NOP, op_NOP, op_SUB_IYH, op_SUB_IYL, op_SUB_a_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_SBC_IYH, op_SBC_IYL, op_SBC_a_IYD, op_NOP, 
                //0xa0
                op_NOP, op_NOP, op_NOP, op_NOP, op_AND_IYH, op_AND_IYL, op_AND_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_XOR_IYH, op_XOR_IYL, op_XOR_IYD, op_NOP, 
                //0xb0
                op_NOP, op_NOP, op_NOP, op_NOP, op_OR_IYH, op_OR_IYL, op_OR_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_CP_IYH, op_CP_IYL, op_CP_IYD, op_NOP, 
                //0xc0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_NOP, op_NOP, op_fdcb, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0xd0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0xe0
                op_NOP, op_POP_iy, op_NOP, op_EX_SP_iy, op_NOP, op_PUSH_iy, op_NOP, op_NOP,
                op_NOP, op_JP_IY, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0xf0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_LD_sp_iy, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
};
            g_operand_fdcb = new Action[]
            {
                //0x00
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RLC_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RRC_IYD, op_NOP,
                //0x10
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RL_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RR_IYD, op_NOP, 
                //0x20
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SLA_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SRA_IYD, op_NOP, 
                //0x30
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SLL_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SRL_IYD, op_NOP, 
                //0x40
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_BIT_b_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_BIT_b_IYD, op_NOP, 
                //0x50
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_BIT_b_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_BIT_b_IYD, op_NOP, 
                //0x60
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_BIT_b_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_BIT_b_IYD, op_NOP, 
                //0x70
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_BIT_b_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_BIT_b_IYD, op_NOP, 
                //0x80
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RES_b_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RES_b_IYD, op_NOP, 
                //0x90
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RES_b_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RES_b_IYD, op_NOP, 
                //0xa0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RES_b_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RES_b_IYD, op_NOP, 
                //0xb0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RES_b_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_RES_b_IYD, op_NOP, 
                //0xc0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SET_b_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SET_b_IYD, op_NOP, 
                //0xd0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SET_b_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SET_b_IYD, op_NOP, 
                //0xe0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SET_b_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SET_b_IYD, op_NOP, 
                //0xf0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SET_b_IYD, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_SET_b_IYD, op_NOP,
            };
            g_operand_ed = new Action[]
{
                //0x00
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0x10
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0x20
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0x30
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0x40
                op_IN_r_C, op_OUT_C_r, op_SBC_hl_rp, op_LD_NN_rp, op_NEG, op_RETN, op_IM0, op_LD_i_a,
                op_IN_r_C, op_OUT_C_r, op_ADC_hl_rp, op_LD_rp_NN, op_NOP, op_RETI, op_NOP, op_LD_r_a, 
                //0x50
                op_IN_r_C, op_OUT_C_r, op_SBC_hl_rp, op_LD_NN_rp, op_NOP, op_NOP, op_IM1, op_LD_a_i,
                op_IN_r_C, op_OUT_C_r, op_ADC_hl_rp, op_LD_rp_NN, op_NOP, op_NOP, op_IM2, op_LD_a_r, 
                //0x60
                op_IN_r_C, op_OUT_C_r, op_SBC_hl_rp, op_LD_NN_rp, op_NOP, op_NOP, op_NOP, op_RRD,
                op_IN_r_C, op_OUT_C_r, op_ADC_hl_rp, op_LD_rp_NN, op_NOP, op_NOP, op_NOP, op_RLD, 
                //0x70
                op_NOP, op_NOP, op_SBC_hl_rp, op_LD_NN_rp, op_NOP, op_NOP, op_NOP, op_NOP,
                op_IN_r_C, op_OUT_C_r, op_ADC_hl_rp, op_LD_rp_NN, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0x80
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0x90
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0xa0
                op_LDI, op_CPI, op_INI, op_OUTI, op_NOP, op_NOP, op_NOP, op_NOP,
                op_LDD, op_CPD, op_IND, op_OUTD, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0xb0
                op_LDIR, op_CPIR, op_INIR, op_OUTIR, op_NOP, op_NOP, op_NOP, op_NOP,
                op_LDDR, op_CPDR, op_INDR, op_OUTDR, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0xc0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0xd0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0xe0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, 
                //0xf0
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
                op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP, op_NOP,
            };
            g_operand_cb = new Action[]
            {
                //0x00
                op_RLC_r, op_RLC_r, op_RLC_r, op_RLC_r, op_RLC_r, op_RLC_r, op_RLC_HL, op_RLC_r,
                op_RRC_r, op_RRC_r, op_RRC_r, op_RRC_r, op_RRC_r, op_RRC_r, op_RRC_HL, op_RRC_r, 
                //0x10
                op_RL_r, op_RL_r, op_RL_r, op_RL_r, op_RL_r, op_RL_r, op_RL_HL, op_RL_r,
                op_RR_r, op_RR_r, op_RR_r, op_RR_r, op_RR_r, op_RR_r, op_RR_HL, op_RR_r, 
                //0x20
                op_SLA_r, op_SLA_r, op_SLA_r, op_SLA_r, op_SLA_r, op_SLA_r, op_SLA_HL, op_SLA_r,
                op_SRA_r, op_SRA_r, op_SRA_r, op_SRA_r, op_SRA_r, op_SRA_r, op_SRA_HL, op_SRA_r, 
                //0x30
                op_SLL_r, op_SLL_r, op_SLL_r, op_SLL_r, op_SLL_r, op_SLL_r, op_SLL_HL, op_SLL_r,
                op_SRL_r, op_SRL_r, op_SRL_r, op_SRL_r, op_SRL_r, op_SRL_r, op_SRL_HL, op_SRL_r, 
                //0x40
                op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_HL, op_BIT_b_r,
                op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_HL, op_BIT_b_r, 
                //0x50
                op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_HL, op_BIT_b_r,
                op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_HL, op_BIT_b_r, 
                //0x60
                op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_HL, op_BIT_b_r,
                op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_HL, op_BIT_b_r, 
                //0x70
                op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_HL, op_BIT_b_r,
                op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_r, op_BIT_b_HL, op_BIT_b_r, 
                //0x80
                op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_HL, op_RES_b_r,
                op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_HL, op_RES_b_r, 
                //0x90
                op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_HL, op_RES_b_r,
                op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_HL, op_RES_b_r, 
                //0xa0
                op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_HL, op_RES_b_r,
                op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_HL, op_RES_b_r, 
                //0xb0
                op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_HL, op_RES_b_r,
                op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_r, op_RES_b_HL, op_RES_b_r, 
                //0xc0
                op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_HL, op_SET_b_r,
                op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_HL, op_SET_b_r, 
                //0xd0
                op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_HL, op_SET_b_r,
                op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_HL, op_SET_b_r, 
                //0xe0
                op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_HL, op_SET_b_r,
                op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_HL, op_SET_b_r, 
                //0xf0
                op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_HL, op_SET_b_r,
                op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_r, op_SET_b_HL, op_SET_b_r,
            };
        }
    }
}

