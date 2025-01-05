namespace MDTracer
{
    //----------------------------------------------------------------
    //PSG  : chips: Texas Instruments SN76489
    //----------------------------------------------------------------
    internal partial class md_sn76489
    {
        //const
        private const int PSG_CLOCK = 3579545;
        private const int PSG_SAMPLING = 44100;
        private const float CLOCK_INC = (float)PSG_CLOCK / PSG_SAMPLING / 16;
        private const int CHANNEL_NUM = 4;
        private const int NOISE_CHANNEL = 3;
        //FMは最大96dBで0～65535、PSGは28dBなので1/4さらに4音なので1/4 。よって0～4046
        private int[] VOL_MAP = new int[]{
           4095, 3267, 2594, 2062, 1638, 1301, 1032, 819, 651, 518, 411, 326, 259, 206, 164, 0 };
        private const int FREQ_MIN = 5;     //レート44100の最大周波数は22050Hzなので周波数の最小は5
        private const int WHITENOISE = 0x0009;
        private const int NOISESHIFT = 15;
        private const int NOISEINITIAL = 0xffff;

        //work
        private float[] g_psg_clock;
        private int[] g_channel_out;
        private int[] g_freq;
        private int[] g_vol;
        private bool[] g_duty;
        private bool g_noise_mode;
        private int g_write_num_bk;
        private int g_shift_reg;
        private float g_ch2_clock;

        public void SN76489_Start()
        {
            g_psg_clock = new float[CHANNEL_NUM];
            g_channel_out = new int[CHANNEL_NUM];
            g_freq = new int[CHANNEL_NUM];
            g_vol = new int[CHANNEL_NUM];
            g_duty = new bool[4];
            g_write_num_bk = -1;
            g_shift_reg = NOISEINITIAL;

            for (int w_ch = 0; w_ch <= 3; w_ch++)
            {
                g_freq[w_ch] = 1;
                g_vol[w_ch] = 15;
                g_freq[3] = 0x10;
            }
        }

        public int SN76489_Update()
        {
            int w_out = 0;
            //toon
            for (int w_ch = 0; w_ch <= 2; w_ch++)
            {
                g_channel_out[w_ch] = (g_duty[w_ch] == true) ? g_vol[w_ch] : -g_vol[w_ch];
                g_psg_clock[w_ch] -= CLOCK_INC;
                if (w_ch == 2) g_ch2_clock = g_psg_clock[2];
                if (g_psg_clock[w_ch] <= 0)
                {
                    if (g_freq[w_ch] >= FREQ_MIN)
                    {
                        g_duty[w_ch] = !g_duty[w_ch];
                    }
                    g_psg_clock[w_ch] += g_freq[w_ch];
                }
            }

            //noise
            {
                g_channel_out[NOISE_CHANNEL] = g_vol[NOISE_CHANNEL] * ((g_shift_reg & 0x1) * 2 - 1);
                if (g_noise_mode == true)
                {
                    g_channel_out[NOISE_CHANNEL] >>= 1;
                }
                if (g_freq[3] == 0x80)
                {
                    g_psg_clock[NOISE_CHANNEL] = g_ch2_clock;
                }
                else
                {
                    g_psg_clock[NOISE_CHANNEL] -= CLOCK_INC;
                }
                if (g_psg_clock[NOISE_CHANNEL] <= 0)
                {
                    g_duty[NOISE_CHANNEL] = !g_duty[NOISE_CHANNEL];
                    if (g_freq[3] != 0x80)
                    {
                        g_psg_clock[NOISE_CHANNEL] += g_freq[3];
                    }
                    if (g_duty[NOISE_CHANNEL] == true)
                    {
                        int w_bit1;
                        if (g_noise_mode == true)
                        {
                            w_bit1 = g_shift_reg & WHITENOISE;
                            w_bit1 = (w_bit1 > 0) && (((g_shift_reg & WHITENOISE) ^ WHITENOISE) > 0) ? 1 : 0;
                        }
                        else
                        {
                            w_bit1 = g_shift_reg & 1;
                        }
                        g_shift_reg = (g_shift_reg >> 1) | (w_bit1 << NOISESHIFT);
                    }
                }
            }
            //mix
            for (int w_ch = 0; w_ch <= 3; w_ch++)
            {
                w_out += (int)(g_channel_out[w_ch] * md_main.g_md_music.g_out_vol[w_ch + 6]);
            }
            return w_out;
        }
    }
}
