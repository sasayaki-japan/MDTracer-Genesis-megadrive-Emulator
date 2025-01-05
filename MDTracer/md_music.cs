using Microsoft.VisualBasic.Devices;
using NAudio.Wave;
using System;
using System.Diagnostics;
using System.Xml.Linq;


namespace MDTracer
{
    internal partial class md_music
    {
        //const
        const int SAMPLING = 44100;
        const int BIT = 16;
        const int CHANNELS = 2;
        const int BUFSIZE = 1024;
        public bool[] g_master_chk;
        public int[] g_master_vol;
        public float[] g_out_vol;
        public int[] g_freq_out;

        private BufferedWaveProvider g_bufferedwaveprovider;
        private WaveOut g_waveOut;

        private byte[] g_buffer;
        private int g_buffer_cur = 0;
        private float g_clock_total;
        private int g_wait = 174;
        public md_sn76489 g_md_sn76489;
        public md_ym2612 g_md_ym2612;
        //----------------------------------------------------------------
        public md_music()
        {
            initialize();
        }
        public void initialize()
        {
            g_master_chk = new bool[11];
            g_master_vol = new int[11];
            g_out_vol = new float[11];
            g_freq_out = new int[11];

            g_bufferedwaveprovider = new BufferedWaveProvider(new WaveFormat(SAMPLING, BIT, CHANNELS));
            g_waveOut = new WaveOut();
            g_waveOut.Init(g_bufferedwaveprovider);
            g_buffer = new byte[BUFSIZE];

            g_md_sn76489 = new md_sn76489();
            g_md_ym2612 = new md_ym2612();
            g_md_sn76489.SN76489_Start();
            g_md_ym2612.YM2612_Start();
            g_waveOut.Play();
        }
        public void setting()
        {
            for(int i = 0; i <= 9; i++)
            {
                g_out_vol[i] = 0;
            }
            if (g_master_chk[10] == true)
            {
                float w_master = g_master_vol[10] / 100.0f;
                for (int i = 0; i <= 9; i++)
                {
                    if (g_master_chk[i] == true) g_out_vol[i] = (g_master_vol[i] / 100.0f) * w_master;
                }
            }
        }

        public void run(int in_clock)
        {
            g_clock_total += in_clock;
            while (g_wait <= g_clock_total)  //7670453(cpu) / 44100
            {
                g_clock_total -= g_wait;

                var result = g_md_ym2612.YM2612_Update();
                short result2 = (short)g_md_sn76489.SN76489_Update();
                short w_mix_total1 = (short)(result.out1 + result2);
                short w_mix_total2 = (short)(result.out2 + result2);

                w_mix_total1 = (short)Math.Max((short)-32768, (short)Math.Min((short)32767, w_mix_total1));
                w_mix_total2 = (short)Math.Max((short)-32768, (short)Math.Min((short)32767, w_mix_total2));

                g_buffer[g_buffer_cur + 1] = (byte)((short)w_mix_total1 >> 8);
                g_buffer[g_buffer_cur + 0] = (byte)((short)w_mix_total1 & 0xff);
                g_buffer[g_buffer_cur + 3] = (byte)((short)w_mix_total2 >> 8);
                g_buffer[g_buffer_cur + 2] = (byte)((short)w_mix_total2 & 0xff);
                g_buffer_cur += 4;
                if (BUFSIZE <= g_buffer_cur)
                {
                    g_bufferedwaveprovider.AddSamples(g_buffer, 0, BUFSIZE);
                    g_buffer_cur = 0;
                }

                if (g_bufferedwaveprovider.BufferedBytes < 10000)
                {
                    g_wait = 172;
                }
                else
                {
                    g_wait = 174;
                }

            }
        }

    }
}
