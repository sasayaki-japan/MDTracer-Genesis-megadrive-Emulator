namespace MDTracer
{
    internal class md_control
    {
        public byte g_io_a11200_z80reset;
        public byte g_io_a11100_z80active;
        //public byte g_io_a11000_memmode;
        //----------------------------------------------------------------
        //read
        //----------------------------------------------------------------
        public byte read8(uint in_address)
        {
            byte w_out = 0;
            if ((in_address & 0xfffffe) == 0xa11100)
            {
                w_out = (byte)((md_main.g_md_z80.g_active == true) ? 1 : 0);
            }
            else
            {
                MessageBox.Show("md_control.read8", "error");
            }
            return w_out;
        }
        public ushort read16(uint in_address)
        {
            return read8(in_address + 1);
        }
        public uint read32(uint in_address)
        {
            uint w_out = 0;
            w_out = (uint)((read8(in_address + 1) << 8) + read8(in_address + 3));
            return w_out;
        }
        //----------------------------------------------------------------
        //write
        //----------------------------------------------------------------
        public void write8(uint in_address, byte in_data)
        {
            if ((in_address & 0xfffffe) == 0xa11100)
            {
                if (in_data == 1)
                {
                    md_main.g_md_z80.g_active = false;
                }
                else
                {
                    md_main.g_md_z80.g_active = true;
                }
            }
            else
            if (in_address == 0xa11200)
            {
                if (in_data == 0)
                {
                    md_main.g_md_z80.reset();
                }
            }
            else
            if (in_address == 0xa130f1)
            {
            }
            else
            {
                MessageBox.Show("md_control.write8", "error");
            }
        }
        public void write16(uint in_address, ushort in_data)
        {
            /*
            if (in_address == 0xa11000)
            {
                //g_io_a11000_memmode = (byte)((in_data >> 8) & 0x0001);
            }
            else
            */
            if (in_address == 0xa11100)
            {
                write8(in_address, (byte)(in_data >> 8));
            }
            else
            if (in_address == 0xa11200)
            {
                write8(in_address, (byte)(in_data >> 8));
            }
            else
            {
                MessageBox.Show("md_control.write16", "error");
            }
        }
    }
}
