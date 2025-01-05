namespace MDTracer
{
    internal partial class md_sn76489
    {
        public void write8(byte in_val)
        {
            if ((in_val & 0x80) == 0x80)
            {
                int w_num = (in_val >> 5) & 0x03;
                if ((in_val & 0x10) == 0)
                {
                    //toon
                    if (w_num <= 2)
                    {
                        g_freq[w_num] = (g_freq[w_num] & 0x03f0) | (in_val & 0x0f);
                        if (g_freq[w_num] == 0) g_freq[w_num] = 1;
                        g_write_num_bk = w_num;
                    }
                    else
                    {
                        g_shift_reg = NOISEINITIAL;
                        g_freq[3] = 0x10 << (in_val & 0x3);
                        g_noise_mode = ((in_val & 0x04) == 0) ? false : true;
                        g_write_num_bk = -1;
                    }
                }
                else
                {
                    //vol
                    g_vol[w_num] = VOL_MAP[in_val & 0x0f];
                    g_write_num_bk = -1;
                }
                if (w_num <= 2)
                {
                    if (g_vol[w_num] == 0)
                    {
                        md_main.g_md_music.g_freq_out[6 + w_num] = 0;
                    }
                    else
                    {
                        md_main.g_md_music.g_freq_out[6 + w_num] = (int)(PSG_CLOCK / ((g_freq[w_num] + 1) << 4));
                    }
                }
            }
            else
            {
                if (g_write_num_bk != -1)
                {
                    g_freq[g_write_num_bk] = (g_freq[g_write_num_bk] & 0x000f) | ((in_val & 0x3f) << 4);
                    g_write_num_bk = -1;
                }
            }
        }
    }
}
