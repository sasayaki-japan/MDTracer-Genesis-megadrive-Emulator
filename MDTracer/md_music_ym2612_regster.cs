using System;

namespace MDTracer
{
    internal partial class md_ym2612
    {
        private bool g_reg_22_lfo_enable;
        private int g_reg_22_lfo_inc;
        private int g_reg_24_timerA;
        private int g_reg_26_timerB;
        private byte g_reg_27_mode;
        private bool g_reg_27_enable_A;
        private bool g_reg_27_enable_B;
        private bool g_reg_27_load_B;
        private bool g_reg_27_load_A;
        private int g_reg_2a_dac_data;
        private int g_reg_2b_dac;
        private double[,] g_reg_30_multi;
        private int[,] g_reg_30_dt;
        private int[,] g_reg_40_tl;
        private int[,] g_reg_50_key_scale;
        private int[,] g_reg_60_ams_enable;
        private int[,] g_reg_80_sl;
        private int[,] g_reg_90_ssg;
        private int[,] g_reg_a0_fnum;
        private int[,] g_reg_a4_fnum;
        private int[,] g_reg_a4_block;
        private byte[] g_reg_b0_fb;
        private int[] g_reg_b0_algo;
        private bool[] g_reg_b4_l;
        private bool[] g_reg_b4_r;
        private int[] g_reg_b4_ams;
        public int[] g_reg_b4_pms;

        public byte read8(uint in_address)
        {
            return g_com_status;
        }
        public void write8(uint in_address, byte in_val)
        {
            int w_mode = -1;
            byte w_addr = 0;
            in_address &= 0x0003;
            switch (in_address & 0x00000f)
            {
                case 0:
                    g_reg_addr1 = in_val;
                    break;
                case 1:
                    w_mode = 0;
                    w_addr = g_reg_addr1;
                    break;
                case 2:
                    g_reg_addr2 = in_val;
                    break;
                case 3:
                    w_mode = 1;
                    w_addr = g_reg_addr2;
                    break;
            }
            if (w_mode != -1)
            {

                g_reg[w_mode, w_addr] = in_val;
                if ((0x20 <= w_addr) && (w_addr <= 0x2b))
                {
                    if (w_mode == 0)
                    {
                        switch (w_addr)
                        {
                            case 0x22:
                                if ((in_val & 0x08) == 0x08)
                                {
                                    g_reg_22_lfo_enable = true;
                                    g_reg_22_lfo_inc = LFO_INC_MAP[in_val & 0x07];
                                }
                                else
                                {
                                    g_reg_22_lfo_enable = false;
                                    g_reg_22_lfo_inc = 0;
                                }
                                break;
                            case 0x24:
                                g_reg_24_timerA = (g_reg_24_timerA & 0x003) | (((int)in_val) << 2);

                                if (g_com_timerA != (1024 - g_reg_24_timerA) << 12)
                                {
                                    g_com_timerA_cnt = g_com_timerA = (1024 - g_reg_24_timerA) << 12;
                                }
                                break;

                            case 0x25:
                                g_reg_24_timerA = (g_reg_24_timerA & 0x3fc) | (in_val & 3);

                                if (g_com_timerA != (1024 - g_reg_24_timerA) << 12)
                                {
                                    g_com_timerA_cnt = g_com_timerA = (1024 - g_reg_24_timerA) << 12;
                                }
                                break;

                            case 0x26:
                                g_reg_26_timerB = in_val;

                                if (g_com_timerB != (256 - g_reg_26_timerB) << (4 + 12))
                                {
                                    g_com_timerB = (256 - g_reg_26_timerB) << (4 + 12);
                                    g_com_timerB_cnt = g_com_timerB;
                                }
                                break;
                            case 0x27:
                                if (((in_val ^ g_reg_27_mode) & 0x40) != 0)
                                {
                                    g_ch_reg_reflesh[2] = true;
                                }
                                g_com_status &= (byte)((~in_val >> 4) & (in_val >> 2));

                                g_reg_27_mode = in_val;
                                g_reg_27_enable_B = ((in_val & 0x08) == 0x08) ? true : false;
                                g_reg_27_enable_A = ((in_val & 0x04) == 0x04) ? true : false;
                                g_reg_27_load_B = ((in_val & 0x02) == 0x02) ? true : false;
                                g_reg_27_load_A = ((in_val & 0x01) == 0x01) ? true : false;
                                break;
                            case 0x28:
                                if((in_val & 0x03) != 0x03)
                                {
                                    int w_ch = KEYON_MAP[in_val & 0x07];
                                    if ((in_val & 0x10) != 0) Slot_Key_on(w_ch, 0); else Slot_Key_off(w_ch, 0);
                                    if ((in_val & 0x20) != 0) Slot_Key_on(w_ch, 1); else Slot_Key_off(w_ch, 1);
                                    if ((in_val & 0x40) != 0) Slot_Key_on(w_ch, 2); else Slot_Key_off(w_ch, 2);
                                    if ((in_val & 0x80) != 0) Slot_Key_on(w_ch, 3); else Slot_Key_off(w_ch, 3);
                                }
                                break;
                            //case 0x29:
                            //   Not implemented as processing is not required
                            case 0x2A:
                                g_reg_2a_dac_data = ((int)(uint)in_val - 0x80) << DAC_SHIFT;
                                break;
                            case 0x2B:
                                g_reg_2b_dac = in_val & 0x80;
                                break;
                        }
                    }
                }
                else
                if ((0x30 <= w_addr) && (w_addr <= 0x9e) && ((w_addr & 0x03) != 3))
                {
                    int w_ch = 0;
                    int w_slot = 0;
                    switch (w_addr & 0xf0)
                    {
                        case 0x30:
                            w_ch = ((w_addr - 0x30) & 0x03) + (w_mode * 3);
                            w_slot = (w_addr - 0x30) >> 2;
                            w_slot = SLOT_MAP[w_slot];
                            g_reg_30_multi[w_ch, w_slot] = MULTIPLE_TABLE[in_val & 0x0F];
                            g_reg_30_dt[w_ch, w_slot] = (in_val >> 4) & 7;
                            g_ch_reg_reflesh[w_ch] = true;
                            break;
                        case 0x40:
                            w_ch = ((w_addr - 0x40) & 0x03) + (w_mode * 3);
                            w_slot = (w_addr - 0x40) >> 2;
                            w_slot = SLOT_MAP[w_slot];
                            g_reg_40_tl[w_ch, w_slot] = (int)((in_val & 0x7f) << (CNT_HIGH_BIT - 7));
                            break;
                        case 0x50:
                            w_ch = ((w_addr - 0x50) & 0x03) + (w_mode * 3);
                            w_slot = (w_addr - 0x50) >> 2;
                            w_slot = SLOT_MAP[w_slot];
                            g_reg_50_key_scale[w_ch, w_slot] = 3 - (in_val >> 6);
                            g_slot_env_indexA[w_ch, w_slot] = ((in_val & 0x1f) != 0) ? (in_val &= 0x1f) << 1 : 0;
                            g_slot_env_incA[w_ch, w_slot] = (int)ENV_RATE_A_TABLE[g_slot_env_indexA[w_ch, w_slot] + g_slot_key_scale[w_ch, w_slot]];
                            g_ch_reg_reflesh[w_ch] = true;
                            break;
                        case 0x60:
                            w_ch = ((w_addr - 0x60) & 0x03) + (w_mode * 3);
                            w_slot = (w_addr - 0x60) >> 2;
                            w_slot = SLOT_MAP[w_slot];
                            if ((g_reg_60_ams_enable[w_ch, w_slot] = (in_val & 0x80)) != 0) g_slot_ams[w_ch, w_slot] = g_reg_b4_ams[w_ch];
                            else g_slot_ams[w_ch, w_slot] = 31;
                            g_slot_env_indexD[w_ch, w_slot] = ((in_val & 0x1f) != 0) ? (in_val &= 0x1f) << 1 : 0;
                            g_slot_env_incD[w_ch, w_slot] = (int)ENV_RATE_D_TABLE[g_slot_env_indexD[w_ch, w_slot] + g_slot_key_scale[w_ch, w_slot]];
                            g_ch_reg_reflesh[w_ch] = true;
                            break;
                        case 0x70:
                            w_ch = ((w_addr - 0x70) & 0x03) + (w_mode * 3);
                            w_slot = (w_addr - 0x70) >> 2;
                            w_slot = SLOT_MAP[w_slot];
                            g_slot_env_indexS[w_ch, w_slot] = ((in_val & 0x1f) != 0) ? (in_val &= 0x1f) << 1 : 0;
                            g_slot_env_incS[w_ch, w_slot] = (int)ENV_RATE_D_TABLE[g_slot_env_indexS[w_ch, w_slot] + g_slot_key_scale[w_ch, w_slot]];
                            g_ch_reg_reflesh[w_ch] = true;
                            break;
                        case 0x80:
                            w_ch = ((w_addr - 0x80) & 0x03) + (w_mode * 3);
                            w_slot = (w_addr - 0x80) >> 2;
                            w_slot = SLOT_MAP[w_slot];
                            g_reg_80_sl[w_ch, w_slot] = (int)SL_TABLE[in_val >> 4];
                            g_slot_env_indexR[w_ch, w_slot] = ((in_val & 0xF) << 2) + 2;
                            g_slot_env_incR[w_ch, w_slot] = (int)ENV_RATE_D_TABLE[g_slot_env_indexR[w_ch, w_slot] + g_slot_key_scale[w_ch, w_slot]];
                            g_ch_reg_reflesh[w_ch] = true;
                            break;
                        case 0x90:
                            //ssg_eg no support
                            w_ch = ((w_addr - 0x90) & 0x03) + (w_mode * 3);
                            w_slot = (w_addr - 0x90) >> 2;
                            w_slot = SLOT_MAP[w_slot];
                            if ((in_val & 0x08) != 0) g_reg_90_ssg[w_ch, w_slot] = in_val & 0x0F;
                            else g_reg_90_ssg[w_ch, w_slot] = 0;
                            break;
                    }
                }
                else
                if ((0xa0 <= w_addr) && (w_addr <= 0xb6) && ((w_addr & 0x03) != 3))
                {
                    int w_ch = 0;
                    int w_slot = 0;
                    int wfnum = 0;
                    switch (w_addr & 0xfc)
                    {
                        case 0xa0:
                            w_ch = (w_addr - 0xa0) + (w_mode * 3);
                            wfnum = (g_slot_fnum[w_ch, 0] & 0x700) + in_val;
                            g_slot_fnum[w_ch, 0] = wfnum;
                            g_slot_keycode[w_ch, 0] = (int)(((uint)g_reg_a4_block[w_ch, 0] << 2) | KEYCODE_TABLE[g_slot_fnum[w_ch, 0] >> 7]);
                            g_ch_reg_reflesh[w_ch] = true;
                            md_main.g_md_music.g_freq_out[w_ch] = (int)((wfnum << (g_reg_a4_block[w_ch, 0] - 1)) * 0.0529819f);
                            break;
                        case 0xa4:
                            w_ch = (w_addr - 0xa4) + (w_mode * 3);
                            wfnum = (g_slot_fnum[w_ch, 0] & 0x0FF) + ((int)(in_val & 0x07) << 8);
                            g_slot_fnum[w_ch, 0] = wfnum;
                            g_reg_a4_block[w_ch, 0] = (in_val & 0x38) >> 3;
                            g_slot_keycode[w_ch, 0] = (int)(((uint)g_reg_a4_block[w_ch, 0] << 2) | KEYCODE_TABLE[g_slot_fnum[w_ch, 0] >> 7]);
                            g_ch_reg_reflesh[w_ch] = true;
                            md_main.g_md_music.g_freq_out[w_ch] = (int)((wfnum << (g_reg_a4_block[w_ch, 0] - 1)) * 0.0529819f);
                            break;
                        case 0xa8:
                            w_slot = ((w_addr - 0xa8) & 0x03) + 1;
                            g_slot_fnum[2, w_slot] = (g_slot_fnum[2, w_slot] & 0x700) + in_val;
                            g_slot_keycode[2, w_slot] = (int)(((uint)g_reg_a4_block[2, w_slot] << 2) |
                                KEYCODE_TABLE[g_slot_fnum[2, w_slot] >> 7]);
                            g_ch_reg_reflesh[w_ch] = true;
                            break;
                        case 0xac:
                            w_slot = ((w_addr - 0xac) & 0x03) + 1;
                            g_slot_fnum[2, w_slot] = (g_slot_fnum[2, w_slot] & 0x0FF) +
                                ((int)(in_val & 0x07) << 8);
                            g_reg_a4_block[2, w_slot] = (in_val & 0x38) >> 3;
                            g_slot_keycode[2, w_slot] = (int)(((uint)g_reg_a4_block[2, w_slot] << 2) |
                                KEYCODE_TABLE[g_slot_fnum[2, w_slot] >> 7]);
                            g_ch_reg_reflesh[w_ch] = true;
                            break;
                        case 0xb0:
                            w_ch = (w_addr - 0xb0) + (w_mode * 3);
                            g_reg_b0_fb[w_ch] = (byte)(9 - ((in_val >> 3) & 0x07));
                            if (g_reg_b0_algo[w_ch] != (in_val & 0x07))
                            {
                                g_reg_b0_algo[w_ch] = in_val & 0x07;
                                g_slot_CNT_MASK[w_ch, 0] = false;
                                g_slot_CNT_MASK[w_ch, 1] = false;
                                g_slot_CNT_MASK[w_ch, 2] = false;
                                g_slot_CNT_MASK[w_ch, 3] = false;
                            }
                            break;
                        case 0xb4:
                            w_ch = (w_addr - 0xb4) + (w_mode * 3);
                            g_reg_b4_l[w_ch] = ((in_val & 0x80) > 0) ? true : false;
                            g_reg_b4_r[w_ch] = ((in_val & 0x40) > 0) ? true : false;
                            g_reg_b4_ams[w_ch] = (int)LFO_AMS_MAP[(in_val >> 4) & 3];
                            g_reg_b4_pms[w_ch] = (int)LFO_PMS_MAP[in_val & 7];

                            if (g_reg_60_ams_enable[w_ch, 0] != 0) g_slot_ams[w_ch, 0] = g_reg_b4_ams[w_ch];
                            else g_slot_ams[w_ch, 0] = 31;
                            if (g_reg_60_ams_enable[w_ch, 1] != 0) g_slot_ams[w_ch, 1] = g_reg_b4_ams[w_ch];
                            else g_slot_ams[w_ch, 1] = 31;
                            if (g_reg_60_ams_enable[w_ch, 2] != 0) g_slot_ams[w_ch, 2] = g_reg_b4_ams[w_ch];
                            else g_slot_ams[w_ch, 2] = 31;
                            if (g_reg_60_ams_enable[w_ch, 3] != 0) g_slot_ams[w_ch, 3] = g_reg_b4_ams[w_ch];
                            else g_slot_ams[w_ch, 3] = 31;
                            break;
                    }
                }
            }
        }
    }
}
