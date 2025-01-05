using System;
using System.DirectoryServices.ActiveDirectory;

namespace MDTracer
{
    internal partial class md_ym2612
    {
        //const
        private const int NUM_CHANNELS = 6;
        private const int NUM_SLOT = 4;
        private const uint YM2612_CLOCK = 7670454;
        private const int YM2612_SAMPLING = 44100;
        private const double PI = 3.14159265358979323846;
        private const double EMU_CORRECTION = ((double)YM2612_CLOCK / 8000000f) * (8000000f / (double)YM2612_SAMPLING / 144f);
        private const int TIMER_CPU = (int)(EMU_CORRECTION * 4096.0);
        private const int DAC_SHIFT = 6;

        private const int CNT_BIT = 20;
        private const int CNT_HIGH_BIT = 10;
        private const int CNT_LOW_BIT = 10;
        private const int CNT_LENGHT = 1 << CNT_HIGH_BIT;
        private const int CNT_MASK = CNT_LENGHT - 1;


        private const double OUT_MAX_dB = 96.0;
        private const int OUT_DOWN_BIT = 9;
        private const int TL_SIZE = (CNT_LENGHT * 3);
        private const int ENV_LEN_ATTACK = ((CNT_LENGHT * 0) << CNT_LOW_BIT);
        private const int ENV_LEN_DECAY = ((CNT_LENGHT * 1) << CNT_LOW_BIT);
        private const int ENV_LEN_END = ((CNT_LENGHT * 2) << CNT_LOW_BIT);
        private const int OUT_CH_LIMIT = ((int)(((1 << 13) * 1.5) - 1));
        private const double LFO_ENV_AMS_MAX_dB = 11.8;
        private const double ENV_ONE_STEP = (OUT_MAX_dB / CNT_LENGHT);
        private const int ENV_PG_MAX = ((int)(78.0 / ENV_ONE_STEP));

        private int[] SLOT_MAP = { 0, 2, 1, 3 };
        private int[] CH3CSM_MAP = { 2, 1, 3, 0 };
        private uint[] KEYCODE_TABLE = { 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 3, 3, 3, 3, 3, 3 };
        private int[] KEYON_MAP = { 0, 1, 2, 0, 3, 4, 5, 0 };
        private double[] MULTIPLE_TABLE = { 0.5f, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

        private int[,] DT_TABLE = new int[8, 32]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2,  2, 3, 3, 3, 4, 4, 4, 5, 5, 6, 6, 7, 8, 8, 8, 8 },
            { 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5,  5, 6, 6, 7, 8, 8, 9, 10, 11, 12, 13, 14, 16, 16, 16, 16 },
            { 2, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 6, 6, 7,  8 , 8, 9, 10, 11, 12, 13, 14, 16, 17, 19, 20, 22, 22, 22, 22 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2, -2, - 2, -3, -3, -3, -4, -4, -4, -5, -5, -6, -6, -7, -8, -8, -8, -8 },
            { 1, -1, -1, -1, -2, -2, -2, -2, -2, -3, -3, -3, -4, -4, -4, -5, - 5, -6, -6, -7, -8, -8, -9, -10, -11, -12, -13, -14, -16, -16, -16, -16 },
            { 2, -2, -2, -2, -2, -3, -3, -3, -4, -4, -4, -5, -5, -6, -6, -7, - 8 , -8, -9, -10, -11, -12, -13, -14, -16, -17, -19, -20, -22, -22, -22, -22 }
        };
        private int[] LFO_INC_MAP = {(int)(3.98f / 0.053f * EMU_CORRECTION)
                                    ,(int)(5.56f / 0.053f * EMU_CORRECTION)
                                    ,(int)(6.02f / 0.053f * EMU_CORRECTION)
                                    ,(int)(6.37f / 0.053f * EMU_CORRECTION)
                                    ,(int)(6.88f / 0.053f * EMU_CORRECTION)
                                    ,(int)(9.63f / 0.053f * EMU_CORRECTION)
                                    ,(int)(48.1f / 0.053f * EMU_CORRECTION)
                                    ,(int)(72.2f / 0.053f * EMU_CORRECTION)
        };
        private double[] LFO_PMS_MAP = { 0
                                    ,(Math.Pow(2, 3.4 / 1200f) - 1) * EMU_CORRECTION
                                    ,(Math.Pow(2, 6.7 / 1200f) - 1) * EMU_CORRECTION
                                    ,(Math.Pow(2, 10 / 1200f) - 1) * EMU_CORRECTION
                                    ,(Math.Pow(2, 14 / 1200f) - 1) * EMU_CORRECTION
                                    ,(Math.Pow(2, 20 / 1200f) - 1) * EMU_CORRECTION
                                    ,(Math.Pow(2, 40 / 1200f) - 1) * EMU_CORRECTION
                                    ,(Math.Pow(2, 80 / 1200f) - 1) * EMU_CORRECTION
        };

        private uint[] LFO_AMS_MAP = { 31, 4, 1, 0 };
        private enum ENV_COND
        {
            ATTACK,
            DECAY,
            SUBSTAIN,
            RELEASE,
            ENDLESS,
            DECAY2,
            SUBSTAIN2
        };
        //common
        private byte g_reg_addr1;
        private byte g_reg_addr2;
        private byte[,] g_reg;
        private int g_dac_high_level;
        private int g_com_lfo_cnt;
        private int g_com_lfo_env_cnt;
        private int g_com_lfo_freq_cnt;
        private int g_com_timerA;
        private int g_com_timerB;
        private int g_com_timerA_cnt;
        private int g_com_timerB_cnt;
        private byte g_com_status;

        //channel
        private int[] g_ch_out;
        private bool[] g_ch_reg_reflesh;
        private double[] g_ch_pms_cnt;

        //slot
		private int[,] g_slot_key_scale;
        private int[,] g_slot_fnum;
        private int[,] g_slot_keycode;
        private int[,] g_slot_freq_cnt;
        private int[,] g_slot_op_calc;
        private int[,] g_slot_phase_out;
        private int[,] g_slot_phase_inc;
        private ENV_COND[,] g_slot_env_cond;
        private int[,] g_slot_env_incA;
        private int[,] g_slot_env_incD;
        private int[,] g_slot_env_incS;
        private int[,] g_slot_env_incR;
        private int[,] g_slot_env_cnt;
        private int[,] g_slot_env_cmp;
        private int[,] g_slot_env_out;
        private int[,] g_slot_env_indexA;
        private int[,] g_slot_env_indexD;
        private int[,] g_slot_env_indexS;
        private int[,] g_slot_env_indexR;
        private int[,] g_slot_ams;
        private bool[,] g_slot_CNT_MASK;
        //table
        private int[] LFO_FREQ_TABLE;
        private int[] LFO_ENV_TABLE;
        private int[] SIN_TABLE;
        private int[] TL_TABLE;
        private uint[] SL_TABLE;
        private uint[] ENV_RATE_A_TABLE;
        private uint[] ENV_RATE_D_TABLE;
        private uint[] ENV_A_TABLE;
        private uint[] ENV_TABLE;
        private uint[] ENV_D2A;
        public void YM2612_Start()
        {
            //regster
            g_reg_30_multi = new double[NUM_CHANNELS, NUM_SLOT];
            g_reg_30_dt = new int[NUM_CHANNELS, NUM_SLOT];
            g_reg_40_tl = new int[NUM_CHANNELS, NUM_SLOT];
            g_reg_50_key_scale = new int[NUM_CHANNELS, NUM_SLOT];
            g_reg_60_ams_enable = new int[NUM_CHANNELS, NUM_SLOT];
            g_reg_80_sl = new int[NUM_CHANNELS, NUM_SLOT];
            g_reg_90_ssg = new int[NUM_CHANNELS, NUM_SLOT];
            g_reg_a0_fnum = new int[NUM_CHANNELS, NUM_SLOT];
            g_reg_a4_fnum = new int[NUM_CHANNELS, NUM_SLOT];
            g_reg_a4_block = new int[NUM_CHANNELS, NUM_SLOT];
            g_reg_b0_fb = new byte[6];
            g_reg_b0_algo = new int[6];
            g_reg_b4_l = new bool[6];
            g_reg_b4_r = new bool[6];
            g_reg_b4_ams = new int[6];
            g_reg_b4_pms = new int[6];

            //common
            g_reg = new byte[2, 256];

            //channel
            g_ch_out = new int[NUM_CHANNELS];
            g_ch_reg_reflesh = new bool[NUM_CHANNELS];
            g_ch_pms_cnt = new double[NUM_CHANNELS];

            //slot
			g_slot_key_scale = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_fnum = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_keycode = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_freq_cnt = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_op_calc = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_phase_out = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_phase_inc = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_env_cond = new ENV_COND[NUM_CHANNELS, NUM_SLOT];
            g_slot_env_incA = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_env_incD = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_env_incS = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_env_incR = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_env_cnt = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_env_cmp = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_env_out = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_env_indexA = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_env_indexD = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_env_indexS = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_env_indexR = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_ams = new int[NUM_CHANNELS, NUM_SLOT];
            g_slot_CNT_MASK = new bool[NUM_CHANNELS, NUM_SLOT];

            //table
            LFO_FREQ_TABLE = new int[CNT_LENGHT];
            LFO_ENV_TABLE = new int[CNT_LENGHT];
            SIN_TABLE = new int[CNT_LENGHT];
            TL_TABLE = new int[TL_SIZE * 2];
            SL_TABLE = new uint[16];
            ENV_RATE_A_TABLE = new uint[96];
            ENV_RATE_D_TABLE = new uint[96];
            ENV_A_TABLE = new uint[CNT_LENGHT];

            ENV_TABLE = new uint[2 * CNT_LENGHT + 8];
            ENV_D2A = new uint[CNT_LENGHT];
            //---------------------------------------------------------------------
            for (int i = 0; i < CNT_LENGHT; i++)
            {
                double x = Math.Sin(2.0 * PI * (double)i / (double)CNT_LENGHT);
                LFO_FREQ_TABLE[i] = (int)((x * (double)((1 << (CNT_HIGH_BIT - 1)) - 1)));
                LFO_ENV_TABLE[i] = (int)((x + 1) / 2 * (LFO_ENV_AMS_MAX_dB / ENV_ONE_STEP));
            }

            SIN_TABLE[0] = SIN_TABLE[CNT_LENGHT / 2] = (int)ENV_PG_MAX;
            for (int i = 1; i <= CNT_LENGHT / 4; i++)
            {
                double x = Math.Sin(2.0 * PI * (double)(i) / (double)(CNT_LENGHT));
                x = 20 * Math.Log10(1 / x);
                int j = (int)(x / ENV_ONE_STEP);
                if (j > ENV_PG_MAX) j = (int)ENV_PG_MAX;
                SIN_TABLE[i] = SIN_TABLE[(CNT_LENGHT / 2) - i] = j;
                SIN_TABLE[(CNT_LENGHT / 2) + i] = SIN_TABLE[CNT_LENGHT - i] = TL_SIZE + j;
            }
            for (int i = 4; i < 64; i++)
            {
                double y = 1.476751 * Math.Exp(0.17438 * i);
                ENV_RATE_A_TABLE[i] = (uint)(y);
                ENV_RATE_D_TABLE[i] = (uint)(y / 14);
            }
            for (int i = 64; i < 96; i++)
            {
                ENV_RATE_A_TABLE[i] = ENV_RATE_A_TABLE[63];
                ENV_RATE_D_TABLE[i] = ENV_RATE_D_TABLE[63];
            }
            for (int i = 0; i < TL_SIZE; i++)
            {
                if (i < ENV_PG_MAX)
                {
                    double x = ((1 << (CNT_BIT + 2)) - 1);
                    x /= Math.Pow(10, (ENV_ONE_STEP * i) / 20);

                    TL_TABLE[i] = (int)x;
                    TL_TABLE[TL_SIZE + i] = -TL_TABLE[i];
                }
            }
            for (int i = 0; i <= 15; i++)
            {
                int j = (int)(i * 3 / OUT_MAX_dB * CNT_LENGHT) << CNT_LOW_BIT;
                if (i == 15) j = (CNT_LENGHT - 1) << CNT_LOW_BIT;
                SL_TABLE[i] = (uint)(j + ENV_LEN_DECAY);
            }
            for (int i = 0; i < 15; i++)
            {
                double x = i * 3;
                x /= ENV_ONE_STEP;
                int j = (int)x;
                j <<= CNT_LOW_BIT;
                SL_TABLE[i] = (uint)(j + ENV_LEN_DECAY);
            }
            SL_TABLE[15] = (uint)(((CNT_LENGHT - 1) << CNT_LOW_BIT) + ENV_LEN_DECAY);

            for (int i = 0; i < CNT_LENGHT; i++)
            {
                double x = Math.Pow(((double)((CNT_LENGHT - 1) - i) / (double)(CNT_LENGHT)), 8);
                x *= CNT_LENGHT;

                ENV_TABLE[i] = (uint)x;
                ENV_A_TABLE[i] = (uint)x;

                x = Math.Pow(((double)(i) / (double)(CNT_LENGHT)), 1);
                x *= CNT_LENGHT;

                ENV_TABLE[CNT_LENGHT + i] = (uint)x;
            }
            ENV_TABLE[ENV_LEN_END >> CNT_LOW_BIT] = CNT_LENGHT - 1;


            for (int i = 0, j = CNT_LENGHT - 1; i < CNT_LENGHT; i++)
            {
                while (j > 0 && (ENV_A_TABLE[j] < (uint)i)) j--;

                ENV_D2A[i] = (uint)(j << CNT_LOW_BIT);
            }

            //initialization
            for (int w_ch = 0; w_ch < NUM_CHANNELS; w_ch++)
            {
                g_reg_b4_l[w_ch] = true;
                g_reg_b4_r[w_ch] = true;
                g_reg_b0_fb[w_ch] = 31;
                for (int w_slot = 0; w_slot < NUM_SLOT; w_slot++)
                {
                    g_slot_env_cond[w_ch, w_slot] = ENV_COND.RELEASE;
                    g_slot_env_cnt[w_ch, w_slot] = ENV_LEN_END;
                }
            }
            for (int i = 0xB6; i >= 0xB4; i--)
            {
                write8(0, (byte)i);
                write8(2, (byte)i);
                write8(1, 0xC0);
                write8(3, 0xC0);
            }
            write8(0, 0x2A);
            write8(1, 0x80);
        }
    }
}
