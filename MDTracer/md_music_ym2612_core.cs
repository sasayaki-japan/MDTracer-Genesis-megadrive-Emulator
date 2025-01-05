using System.Diagnostics;
using System.Windows.Forms;

namespace MDTracer
{
    //----------------------------------------------------------------
    //FM synthesis  : chips: YAMAHA YM2612
    //----------------------------------------------------------------
    internal partial class md_ym2612
    {
        public (int out1, int out2) YM2612_Update()
        {
            int w_out_l = 0;
            int w_out_r = 0;

            lfo_calc();
            for (int w_ch = 0; w_ch < NUM_CHANNELS; w_ch++)
            {
                register_change(w_ch);
                phase_generator(w_ch);
                envelop_generator(w_ch);
                if ((w_ch != 5) || (g_reg_2b_dac == 0))
                {
                    operator_update(w_ch);
                    if (g_ch_out[w_ch] > OUT_CH_LIMIT) g_ch_out[w_ch] = OUT_CH_LIMIT;
                    else if (g_ch_out[w_ch] < -OUT_CH_LIMIT) g_ch_out[w_ch] = -OUT_CH_LIMIT;
                    if (g_reg_b4_l[w_ch] == true) w_out_l += (int)(g_ch_out[w_ch] * md_main.g_md_music.g_out_vol[w_ch]);
                    if (g_reg_b4_r[w_ch] == true) w_out_r += (int)(g_ch_out[w_ch] * md_main.g_md_music.g_out_vol[w_ch]);
                }
                else
                {
                    int w_dac = dac_control();
                    if (g_reg_b4_l[5] == true) w_out_l += (int)(w_dac * md_main.g_md_music.g_out_vol[5]);
                    if (g_reg_b4_r[5] == true) w_out_r += (int)(w_dac * md_main.g_md_music.g_out_vol[5]);
                }
            }
            timer_control();
            return (w_out_l, w_out_r);
        }
        private void lfo_calc()
        {
            if (g_reg_22_lfo_enable == true)
            {
                g_com_lfo_cnt += g_reg_22_lfo_inc;
                int w_cnt = (g_com_lfo_cnt >> CNT_LOW_BIT) & CNT_MASK;
                g_com_lfo_freq_cnt = LFO_FREQ_TABLE[w_cnt];
                g_com_lfo_env_cnt = LFO_ENV_TABLE[w_cnt];
            }
        }
        private void register_change(int in_ch)
        {
            if (g_ch_reg_reflesh[in_ch] == true)
            {
                g_ch_reg_reflesh[in_ch] = false;
                for (int w_slot = 0; w_slot < NUM_SLOT; w_slot++)
                {
                    //ch3 mode support
                    int w_slot_m = 0;
                    if ((in_ch == 2) && ((g_reg_27_mode & 0x40) > 0))
                    {
                        w_slot_m = CH3CSM_MAP[w_slot];
                    }
                    //phase_generator
                    int finc = (int)((float)g_slot_fnum[in_ch, w_slot_m]
                        * (1 << g_reg_a4_block[in_ch, w_slot_m]))>> 1;
                    int kc = g_slot_keycode[in_ch, w_slot_m];
                    g_slot_phase_inc[in_ch, w_slot] = 
                        (int)((finc + DT_TABLE[g_reg_30_dt[in_ch, w_slot], kc])
                        * EMU_CORRECTION
                        * g_reg_30_multi[in_ch, w_slot]);
                    //envelop_generator
                    int ksr = kc >> g_reg_50_key_scale[in_ch, w_slot];
                    g_slot_env_incA[in_ch, w_slot] = (int)ENV_RATE_A_TABLE[g_slot_env_indexA[in_ch, w_slot] + ksr];
                    g_slot_env_incD[in_ch, w_slot] = (int)ENV_RATE_D_TABLE[g_slot_env_indexD[in_ch, w_slot] + ksr];
                    g_slot_env_incS[in_ch, w_slot] = (int)ENV_RATE_D_TABLE[g_slot_env_indexS[in_ch, w_slot] + ksr];
                    g_slot_env_incR[in_ch, w_slot] = (int)ENV_RATE_D_TABLE[g_slot_env_indexR[in_ch, w_slot] + ksr];
                }
            }
        }
        private void phase_generator(int in_ch)
        {
            for (int w_slot = 0; w_slot < NUM_SLOT; w_slot++)
            {
                int w_lfo_inc = 0;
                if (g_reg_22_lfo_enable == true)
                {
                    w_lfo_inc = (int)(g_slot_phase_inc[in_ch, w_slot] * (g_reg_b4_pms[in_ch] * g_com_lfo_freq_cnt)) >> CNT_LOW_BIT;
                }
                g_slot_op_calc[in_ch, w_slot] = g_slot_freq_cnt[in_ch, w_slot];
                g_slot_freq_cnt[in_ch, w_slot] += g_slot_phase_inc[in_ch, w_slot] + w_lfo_inc;
            }
        }

        private void operator_calc(int in_ch)
        {
            g_slot_op_calc[in_ch, 0] += (g_slot_phase_out[in_ch, 0] + g_slot_phase_out[in_ch, 1]) >> g_reg_b0_fb[in_ch];
            g_slot_phase_out[in_ch, 1] = g_slot_phase_out[in_ch, 0];
            g_slot_phase_out[in_ch, 0] = TL_TABLE[SIN_TABLE[(g_slot_op_calc[in_ch, 0] >> CNT_LOW_BIT) & CNT_MASK] + g_slot_env_out[in_ch, 0]];
        }
        private void slot_mixer(int in_ch, int in_input1, int in_input2, int in_input3, int in_input4)
        {
            g_ch_out[in_ch] = (TL_TABLE[SIN_TABLE[(g_slot_op_calc[in_ch, in_input1] >> CNT_LOW_BIT) & CNT_MASK] + g_slot_env_out[in_ch, in_input1]]);
            if (in_input2 != -1) g_ch_out[in_ch] += TL_TABLE[SIN_TABLE[(g_slot_op_calc[in_ch, in_input2] >> CNT_LOW_BIT) & CNT_MASK] + g_slot_env_out[in_ch, in_input2]];
            if (in_input3 != -1) g_ch_out[in_ch] += (int)TL_TABLE[SIN_TABLE[(g_slot_op_calc[in_ch, in_input3] >> CNT_LOW_BIT) & CNT_MASK] + g_slot_env_out[in_ch, in_input3]];
            if (in_input4 != -1) g_ch_out[in_ch] += (int)TL_TABLE[SIN_TABLE[(g_slot_op_calc[in_ch, in_input4] >> CNT_LOW_BIT) & CNT_MASK] + g_slot_env_out[in_ch, in_input4]];
            g_ch_out[in_ch] >>= OUT_DOWN_BIT;
        }

        private void operator_update(int in_ch)
        {
            switch (g_reg_b0_algo[in_ch])
            {
                case 4:
                    if ((g_slot_env_cnt[in_ch, 2] == ENV_LEN_END) && (g_slot_env_cnt[in_ch, 3] == ENV_LEN_END)) return;
                    break;
                case 5:
                    if ((g_slot_env_cnt[in_ch, 2] == ENV_LEN_END) && (g_slot_env_cnt[in_ch, 1] == ENV_LEN_END) && (g_slot_env_cnt[in_ch, 3] == ENV_LEN_END)) return;
                    break;
                case 6:
                    if ((g_slot_env_cnt[in_ch, 2] == ENV_LEN_END) && (g_slot_env_cnt[in_ch, 1] == ENV_LEN_END) && (g_slot_env_cnt[in_ch, 3] == ENV_LEN_END)) return;
                    break;
                case 7:
                    if ((g_slot_env_cnt[in_ch, 0] == ENV_LEN_END) && (g_slot_env_cnt[in_ch, 2] == ENV_LEN_END) && (g_slot_env_cnt[in_ch, 1] == ENV_LEN_END) && (g_slot_env_cnt[in_ch, 3] == ENV_LEN_END)) return;
                    break;
                default:
                    if (g_slot_env_cnt[in_ch, 3] == ENV_LEN_END) return;
                    break;
            }

            switch (g_reg_b0_algo[in_ch])
            {
                case 0:
                    operator_calc(in_ch);
                    g_slot_op_calc[in_ch, 1] += g_slot_phase_out[in_ch, 1];
                    g_slot_op_calc[in_ch, 2] += TL_TABLE[SIN_TABLE[(g_slot_op_calc[in_ch, 1] >> CNT_LOW_BIT) & CNT_MASK] + g_slot_env_out[in_ch, 1]];
                    g_slot_op_calc[in_ch, 3] += TL_TABLE[SIN_TABLE[(g_slot_op_calc[in_ch, 2] >> CNT_LOW_BIT) & CNT_MASK] + g_slot_env_out[in_ch, 2]];
                    slot_mixer(in_ch, 3, -1, -1, -1);
                    break;
                case 1:
                    operator_calc(in_ch);
                    g_slot_op_calc[in_ch, 2] += g_slot_phase_out[in_ch, 1];
                    g_slot_op_calc[in_ch, 2] += TL_TABLE[SIN_TABLE[(g_slot_op_calc[in_ch, 1] >> CNT_LOW_BIT) & CNT_MASK] + g_slot_env_out[in_ch, 1]];
                    g_slot_op_calc[in_ch, 3] += TL_TABLE[SIN_TABLE[(g_slot_op_calc[in_ch, 2] >> CNT_LOW_BIT) & CNT_MASK] + g_slot_env_out[in_ch, 2]];
                    slot_mixer(in_ch, 3, -1, -1, -1);
                    break;
                case 2:
                    operator_calc(in_ch);
                    g_slot_op_calc[in_ch, 2] += TL_TABLE[SIN_TABLE[(g_slot_op_calc[in_ch, 1] >> CNT_LOW_BIT) & CNT_MASK] + g_slot_env_out[in_ch, 1]];
                    g_slot_op_calc[in_ch, 3] += g_slot_phase_out[in_ch, 1];
                    g_slot_op_calc[in_ch, 3] += TL_TABLE[SIN_TABLE[(g_slot_op_calc[in_ch, 2] >> CNT_LOW_BIT) & CNT_MASK] + g_slot_env_out[in_ch, 2]];
                    slot_mixer(in_ch, 3, -1, -1, -1);
                    break;
                case 3:
                    operator_calc(in_ch);
                    g_slot_op_calc[in_ch, 1] += g_slot_phase_out[in_ch, 1];
                    g_slot_op_calc[in_ch, 3] += TL_TABLE[SIN_TABLE[(g_slot_op_calc[in_ch, 1] >> CNT_LOW_BIT) & CNT_MASK] + g_slot_env_out[in_ch, 1]]
                                              + TL_TABLE[SIN_TABLE[(g_slot_op_calc[in_ch, 2] >> CNT_LOW_BIT) & CNT_MASK] + g_slot_env_out[in_ch, 2]];
                    slot_mixer(in_ch, 3, -1, -1, -1);
                    break;
                case 4:
                    operator_calc(in_ch);
                    g_slot_op_calc[in_ch, 1] += g_slot_phase_out[in_ch, 1];
                    g_slot_op_calc[in_ch, 3] += TL_TABLE[SIN_TABLE[(g_slot_op_calc[in_ch, 2] >> CNT_LOW_BIT) & CNT_MASK] + g_slot_env_out[in_ch, 2]];
                    slot_mixer(in_ch, 1, 3, -1, -1);
                    break;
                case 5:
                    operator_calc(in_ch);
                    g_slot_op_calc[in_ch, 1] += g_slot_phase_out[in_ch, 1];
                    g_slot_op_calc[in_ch, 2] += g_slot_phase_out[in_ch, 1];
                    g_slot_op_calc[in_ch, 3] += g_slot_phase_out[in_ch, 1];
                    slot_mixer(in_ch, 1, 2, 3, -1);
                    break;
                case 6:
                    operator_calc(in_ch);
                    g_slot_op_calc[in_ch, 1] += g_slot_phase_out[in_ch, 1];
                    slot_mixer(in_ch, 1, 2, 3, -1);
                    break;
                case 7:
                    operator_calc(in_ch);
                    slot_mixer(in_ch, 0, 1, 2, 3);
                    break;
            }
        }

        private void envelop_generator(int in_ch)
        {
            for (int w_slot = 0; w_slot < NUM_SLOT; w_slot++)
            {
                int w_lfo_inc = 0;
                if (g_reg_22_lfo_enable == true)
                {
                    w_lfo_inc = (g_com_lfo_env_cnt >> g_slot_ams[in_ch, w_slot]);
                }
                int w_env = g_slot_env_out[in_ch, w_slot];
                if ((g_reg_90_ssg[in_ch, w_slot] & 4) == 0)
                {
                    w_env = (int)ENV_TABLE[(g_slot_env_cnt[in_ch, w_slot] >> CNT_LOW_BIT)] + g_reg_40_tl[in_ch, w_slot] + w_lfo_inc;
                }
                else
                {
                    w_env = (int)ENV_TABLE[(g_slot_env_cnt[in_ch, w_slot] >> CNT_LOW_BIT)] + g_reg_40_tl[in_ch, w_slot];
                    if (w_env > CNT_MASK)
                    {
                        w_env = 0;
                    }
                    else
                    {
                        w_env = (w_env ^ CNT_MASK) + w_lfo_inc;
                    }
                }
                g_slot_env_out[in_ch, w_slot] = w_env;

                int w_inc = 0;
                ENV_COND w_cond = g_slot_env_cond[in_ch, w_slot];
                switch (w_cond)
                {
                    case ENV_COND.ATTACK:
                        w_inc = g_slot_env_incA[in_ch, w_slot];
                        break;
                    case ENV_COND.DECAY:
                        w_inc = g_slot_env_incD[in_ch, w_slot];
                        break;
                    case ENV_COND.SUBSTAIN:
                        if (g_slot_env_cnt[in_ch, w_slot] < ENV_LEN_END)
                        {
                            w_inc = g_slot_env_incS[in_ch, w_slot];
                        }
                        break;
                    case ENV_COND.RELEASE:
                        if (g_slot_env_cnt[in_ch, w_slot] < ENV_LEN_END)
                        {
                            w_inc = g_slot_env_incR[in_ch, w_slot];
                        }
                        break;
                }
                g_slot_env_cnt[in_ch, w_slot] += w_inc;
                if (g_slot_env_cnt[in_ch, w_slot] >= g_slot_env_cmp[in_ch, w_slot])
                {
                    switch (w_cond)
                    {
                        case ENV_COND.ATTACK:
                            g_slot_env_cnt[in_ch, w_slot] = ENV_LEN_DECAY;
                            g_slot_env_cmp[in_ch, w_slot] = g_reg_80_sl[in_ch, w_slot];
                            g_slot_env_cond[in_ch, w_slot] = ENV_COND.DECAY;
                            break;
                        case ENV_COND.DECAY:
                            g_slot_env_cnt[in_ch, w_slot] = g_reg_80_sl[in_ch, w_slot];
                            g_slot_env_cmp[in_ch, w_slot] = ENV_LEN_END;
                            g_slot_env_cond[in_ch, w_slot] = ENV_COND.SUBSTAIN;
                            break;
                        case ENV_COND.SUBSTAIN:
                            if ((g_reg_90_ssg[in_ch, w_slot] & 8) != 0)
                            {
                                if ((g_reg_90_ssg[in_ch, w_slot] & 1) == 0)
                                {
                                    g_slot_env_cnt[in_ch, w_slot] = 0;
                                    g_slot_env_cmp[in_ch, w_slot] = ENV_LEN_DECAY;
                                    g_slot_env_cond[in_ch, w_slot] = ENV_COND.ATTACK;
                                }
                                else
                                {
                                    g_slot_env_cnt[in_ch, w_slot] = ENV_LEN_END;
                                    g_slot_env_cmp[in_ch, w_slot] = ENV_LEN_END + 1;
                                }
                                g_reg_90_ssg[in_ch, w_slot] ^= (g_reg_90_ssg[in_ch, w_slot] & 2) << 1;
                            }
                            else
                            {
                                g_slot_env_cnt[in_ch, w_slot] = ENV_LEN_END;
                                g_slot_env_cmp[in_ch, w_slot] = ENV_LEN_END + 1;
                            }
                            break;
                        case ENV_COND.RELEASE:
                            g_slot_env_cnt[in_ch, w_slot] = ENV_LEN_END;
                            g_slot_env_cmp[in_ch, w_slot] = ENV_LEN_END + 1;
                            break;
                    }
                }
            }
        }
        private void Slot_Key_on(int in_ch, int in_slot)
        {
            if (g_slot_env_cond[in_ch, in_slot] == ENV_COND.RELEASE)
            {
                g_slot_freq_cnt[in_ch, in_slot] = 0;
                if (g_slot_CNT_MASK[in_ch, 0] == true)
                {
                    g_slot_env_cnt[in_ch, in_slot] = (int)(ENV_D2A[ENV_TABLE[g_slot_env_cnt[in_ch, in_slot] >> CNT_LOW_BIT]] + ENV_LEN_ATTACK);
                }
                g_slot_CNT_MASK[in_ch, 0] = true;
                g_slot_env_cmp[in_ch, in_slot] = ENV_LEN_DECAY;
                g_slot_env_cond[in_ch, in_slot] = ENV_COND.ATTACK;
            }
        }
        private void Slot_Key_off(int in_ch, int in_slot)
        {

            if (g_slot_env_cond[in_ch, in_slot] != ENV_COND.RELEASE)
            {
                if (g_slot_env_cnt[in_ch, in_slot] < ENV_LEN_DECAY)
                {
                    g_slot_env_cnt[in_ch, in_slot] = (int)((ENV_TABLE[g_slot_env_cnt[in_ch, in_slot] >> CNT_LOW_BIT] << CNT_LOW_BIT) + ENV_LEN_DECAY);
                }

                g_slot_env_cmp[in_ch, in_slot] = ENV_LEN_END;
                g_slot_env_cond[in_ch, in_slot] = ENV_COND.RELEASE;
            }
        }
        private int dac_control()
        {
            int w_dac = 0;
            if (g_reg_2a_dac_data != 0)
            {
                w_dac = (int)(((uint)g_reg_2a_dac_data << 15) - g_dac_high_level);
                g_dac_high_level += w_dac >> 9;
                w_dac >>= 15;
            }
            return w_dac;
        }
        private void timer_control()
        {
            if (g_reg_27_load_A == true)
            {
                if ((g_com_timerA_cnt -= TIMER_CPU) <= 0)
                {
                    g_com_status |= (byte)((g_reg_27_enable_A == true) ? 1 : 0);
                    g_com_timerA_cnt += g_com_timerA;
                    if ((g_reg_27_mode & 0x80) != 0)
                    {
                        Slot_Key_on(2, 0);
                        Slot_Key_on(2, 1);
                        Slot_Key_on(2, 2);
                        Slot_Key_on(2, 3);
                    }
                }
            }
            if (g_reg_27_load_B == true)
            {
                if ((g_com_timerB_cnt -= TIMER_CPU) <= 0)
                {
                    g_com_status |= (byte)((g_reg_27_enable_B == true) ? 2 : 0);
                    g_com_timerB_cnt += g_com_timerB;
                }
            }
        }
    }
}