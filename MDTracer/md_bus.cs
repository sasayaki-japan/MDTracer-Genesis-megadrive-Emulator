using System.Diagnostics;

namespace MDTracer
{
    //----------------------------------------------------------------
    //Bus arbiter : chips:315-5308
    //----------------------------------------------------------------
    internal class md_bus
    {
        //----------------------------------------------------------------
        //read
        //----------------------------------------------------------------
        public byte read8(uint in_address)
        {
            byte w_out = 0;
            in_address = in_address & 0xffffff;
            if (in_address <= 0x3fffff)
            {
                w_out = md_main.g_md_m68k.read8(in_address);
            }
            else 
            if (0xff0000 <= in_address)
            {
                w_out = md_main.g_md_m68k.read8(in_address);
            }
            else
            if ((0xc00000 <= in_address) && (in_address <= 0xdfffff))
            {
                w_out = md_main.g_md_vdp.read8(in_address);
            }
            else
            if ((0xa10000 <= in_address) && (in_address <= 0xa10fff))
            {
                w_out = md_main.g_md_io.read8(in_address);
            }
            else
            if ((0xa04000 <= in_address) && (in_address <= 0xa04000))
            {
                w_out = md_main.g_md_music.g_md_ym2612.read8(in_address);
            }
            else
            if ((0xa11000 <= in_address) && (in_address <= 0xa1ffff))
            {
                w_out = md_main.g_md_control.read8(in_address);
            }
            else
            if ((0xa00000 <= in_address) && (in_address <= 0xa0ffff))
            {
                w_out = md_main.g_md_z80.read8(in_address);
            }
            else
            {
                MessageBox.Show("md_bus.read8", "error");
            }
            md_main.g_form_code.memory_monitor_check(in_address, w_out, false);
            return w_out;
        }
        public ushort read16(uint in_address)
        {
            ushort w_out = 0;
            in_address = in_address & 0xffffff;
            if (in_address <= 0x3fffff)
            {
                w_out = md_main.g_md_m68k.read16(in_address);
            }
            else
            if (0xff0000 <= in_address)
            {
                w_out = md_main.g_md_m68k.read16(in_address);
            }
            else
            if ((0xc00000 <= in_address) && (in_address <= 0xdfffff))
            {
                w_out = md_main.g_md_vdp.read16(in_address);
            }
            else
            if ((0xa10000 <= in_address) && (in_address <= 0xa10fff))
            {
                w_out = md_main.g_md_io.read16(in_address);
            }
            else
            if ((0xa11000 <= in_address) && (in_address <= 0xa1ffff))
            {
                w_out = md_main.g_md_control.read16(in_address);
            }
            else
            if ((0xa00000 <= in_address) && (in_address <= 0xa0ffff))
            {
                w_out = md_main.g_md_z80.read16(in_address);
            }
            else
            {
                MessageBox.Show("md_bus.read16", "error");
            }
            md_main.g_form_code.memory_monitor_check(in_address, w_out, false);
            return w_out;
        }
        public uint read32(uint in_address)
        {
            uint w_out = 0;
            in_address = in_address & 0xffffff;
            if (in_address <= 0x3fffff)
            {
                w_out = md_main.g_md_m68k.read32(in_address);
            }
            else
            if (0xff0000 <= in_address)
            {
                w_out = md_main.g_md_m68k.read32(in_address);
            }
            else
            if ((0xc00000 <= in_address) && (in_address <= 0xdfffff))
            {
                w_out = md_main.g_md_vdp.read32(in_address);
            }
            else
            if ((0xa10000 <= in_address) && (in_address <= 0xa10fff))
            {
                w_out = md_main.g_md_io.read32(in_address);
            }
            else
            if ((0xa11000 <= in_address) && (in_address <= 0xa1ffff))
            {
                w_out = md_main.g_md_control.read32(in_address);
            }
            else
            if ((0xa00000 <= in_address) && (in_address <= 0xa0ffff))
            {
                w_out = md_main.g_md_z80.read32(in_address);
            }
            else
            {
                MessageBox.Show("md_bus.read32", "error");
            }
            md_main.g_form_code.memory_monitor_check(in_address, w_out, false);
            return w_out;
        }
        //----------------------------------------------------------------
        //write
        //----------------------------------------------------------------
        public void write8(uint in_address, byte in_data)
        {
            in_address = in_address & 0xffffff;
            md_main.g_form_code.memory_monitor_check(in_address, in_data, true);
            if (0xff0000 <= in_address)
            {
                md_main.g_md_m68k.write8(in_address, in_data);
            }
            else
            if ((in_address == 0xc00010)|| (in_address == 0xc00011))
            {
                md_main.g_md_music.g_md_sn76489.write8(in_data);
            }
            else
            if ((0xc00000 <= in_address) && (in_address <= 0xdfffff))
            {
                md_main.g_md_vdp.write8(in_address, in_data);
            }
            else
            if ((0xa10000 <= in_address) && (in_address <= 0xa10fff))
            {
                md_main.g_md_io.write8(in_address, in_data);
            }
            else
            if ((0xa04000 <= in_address) && (in_address <= 0xa04003))
            {
                md_main.g_md_music.g_md_ym2612.write8(in_address, in_data);
            }
            else
            if ((0xa11000 <= in_address) && (in_address <= 0xa1ffff))
            {
                md_main.g_md_control.write8(in_address, in_data);
            }
            else
            if ((0xa00000 <= in_address) && (in_address <= 0xa0ffff))
            {
                md_main.g_md_z80.write8(in_address, in_data);
            }
            else
            {
                //MessageBox.Show("md_bus.write8", "error");
            }
        }
        public void write16(uint in_address, ushort in_data)
        {
            in_address = in_address & 0xffffff;
            md_main.g_form_code.memory_monitor_check(in_address, in_data, true);
            if (0xff0000 <= in_address)
            {
                md_main.g_md_m68k.write16(in_address, in_data);
            }
            else
            if ((0xc00000 <= in_address) && (in_address <= 0xdfffff))
            {
                md_main.g_md_vdp.write16(in_address, in_data);
            }
            else
            if ((0xa10000 <= in_address) && (in_address <= 0xa10fff))
            {
                md_main.g_md_io.write16(in_address, in_data);
            }
            else
            if ((0xa11000 <= in_address) && (in_address <= 0xa1ffff))
            {
                md_main.g_md_control.write16(in_address, in_data);
            }
            else
            if ((0xa00000 <= in_address) && (in_address <= 0xa0ffff))
            {
                md_main.g_md_z80.write16(in_address, in_data);
            }
            else
            {
                MessageBox.Show("md_bus.write16", "error");
            }
        }
        public void write32(uint in_address, uint in_data)
        {
            in_address = in_address & 0xffffff;
            md_main.g_form_code.memory_monitor_check(in_address, in_data, true);
            if (0xff0000 <= in_address)
            {
                md_main.g_md_m68k.write32(in_address, in_data);
            }
            else
            if ((0xc00000 <= in_address) && (in_address <= 0xdfffff))
            {
                md_main.g_md_vdp.write32(in_address, in_data);
            }
            else
            if ((0xa00000 <= in_address) && (in_address <= 0xa0ffff))
            {
                if (in_address == 0xa01ffc) in_address = in_address;
                if (in_address == 0xa01ffd) in_address = in_address;
                if (in_address == 0xa01ffe) in_address = in_address;
                if (in_address == 0xa01fff) in_address = in_address;

                md_main.g_md_z80.write32(in_address, in_data);
            }
            else
            if (0xa14000 == in_address)
            {
                //TMSS
            }
            else
            {
                MessageBox.Show("md_bus.write32", "error");
            }
        }
    }
}
