using System.Diagnostics;
using static MDTracer.md_m68k;

namespace MDTracer
{
    internal partial class md_z80
    {
        private byte[] g_ram;
        private uint g_bank_register;

        //----------------------------------------------------------------
        //read
        //----------------------------------------------------------------
        public byte read8(uint in_address)
        {
            byte w_out = 0;
            in_address &= 0xffff;
            if (in_address < 0x4000)
            {
                in_address &= 0x1fff;
                w_out = g_ram[in_address];
            }
            else
            if (in_address <= 0x5fff)
            {
                w_out = md_main.g_md_music.g_md_ym2612.read8(in_address);
            }   
            else
            if ((in_address >= 0x6000) && (in_address <= 0x7eff))
            {
                w_out = 0xff;
            }
            else
            if (in_address >= 0x8000)
            {
                w_out = md_main.g_md_m68k.read8(g_bank_register + (in_address & 0x7fff));
            }
            else
            {
                MessageBox.Show("md_z80_memory.read8", "error");
            }
            return w_out;
        }
        public ushort read16(uint in_address)
        {
            UNION_UINT w_data;
            w_data.w = 0;
            in_address &= 0xffff;
            w_data.b1 = g_ram[in_address];
            w_data.b0 = g_ram[in_address + 1];
            return w_data.w;
        }
        public uint read32(uint in_address)
        {
            UNION_UINT w_data;
            w_data.l = 0;
            in_address &= 0xffff;
            w_data.b3 = g_ram[in_address];
            w_data.b2 = g_ram[in_address + 1];
            w_data.b1 = g_ram[in_address + 2];
            w_data.b0 = g_ram[in_address + 3];
            return w_data.l;
        }
        //----------------------------------------------------------------
        //write
        //----------------------------------------------------------------
        public void write8(uint in_address, byte in_data)
        {
            in_address &= 0xffff;
            if (in_address < 0x4000)
            {
                in_address &= 0x1fff;
                g_ram[in_address] = in_data;
            }
            else
            if ((0x4000 <= in_address) && (in_address <= 0x5fff))
            {
                md_main.g_md_music.g_md_ym2612.write8(in_address, in_data);
            }
            else
            if ((in_address >= 0x6000) && (in_address <= 0x60ff))
            {
                g_bank_register &= 0x00ff0000;
                g_bank_register >>= 1;

                if ((in_data & 0x01) == 1)
                {
                    g_bank_register = (g_bank_register | 0x00800000);
                }
            }
            else
            if ((in_address >= 0x6100) && (in_address <= 0x7eff))
            {
                //nothing
            }
            else
            if (0x7f11 == in_address)
            {
                md_main.g_md_music.g_md_sn76489.write8(in_data);
            }
            else
            if (in_address >= 0x8000)
            {
                md_main.g_md_m68k.write8(g_bank_register + (in_address & 0x7fff), in_data);
            }
            else
            {
                MessageBox.Show("md_z80_memory.write8", "error");
            }
        }
        public void write16(uint in_address, ushort in_data)
        {
            in_address &= 0xffff;
            g_ram[in_address] = (byte)((in_data >> 8) & 0x00ff);
            g_ram[in_address + 1] = (byte)(in_data & 0x00ff);
        }
        public void write32(uint in_address, uint in_data)
        {
            in_address &= 0xffff;
            g_ram[in_address] = (byte)(in_data >> 24);
            g_ram[in_address + 1] = (byte)((in_data >> 16) & 0x00ff);
            g_ram[in_address + 2] = (byte)((in_data >> 8) & 0x00ff);
            g_ram[in_address + 3] = (byte)(in_data & 0x00ff);
        }
    }
}
