using System.Diagnostics;

namespace MDTracer
{
    internal partial class md_m68k
    {
        public byte[] g_memory;
        //----------------------------------------------------------------
        //read
        //----------------------------------------------------------------
        public byte read8(uint in_address)
        {
            in_address = in_address & 0xffffff;
            if (0xe00000 <= in_address) in_address = (in_address & 0xffff) | 0xff0000;
            return g_memory[in_address];
        }
        public ushort read16(uint in_address)
        {
            in_address = in_address & 0xffffff;
            if (0xe00000 <= in_address) in_address = (in_address & 0xffff) | 0xff0000;
            UNION_UINT w_data;
            w_data.w = 0;
            w_data.b1 = g_memory[in_address];
            w_data.b0 = g_memory[in_address + 1];
            return w_data.w;
        }
        public uint read32(uint in_address)
        {
            in_address = in_address & 0xffffff;
            if (0xe00000 <= in_address) in_address = (in_address & 0xffff) | 0xff0000;
            UNION_UINT w_data;
            w_data.l = 0;
            w_data.b3 = g_memory[in_address];
            w_data.b2 = g_memory[in_address + 1];
            w_data.b1 = g_memory[in_address + 2];
            w_data.b0 = g_memory[in_address + 3];
            return w_data.l;
        }
        //----------------------------------------------------------------
        //write
        //----------------------------------------------------------------
        public void write8(uint in_address, byte in_data)
        {
            in_address = in_address & 0xffffff;
            if (0xe00000 <= in_address) in_address = (in_address & 0xffff) | 0xff0000;
            g_memory[in_address] = in_data;
        }
        public void write16(uint in_address, ushort in_data)
        {
            in_address = in_address & 0xffffff;
            if (0xe00000 <= in_address) in_address = (in_address & 0xffff) | 0xff0000;
            g_memory[in_address] = (byte)((in_data >> 8) & 0x00ff);
            g_memory[in_address + 1] = (byte)(in_data & 0x00ff);
        }
        public void write32(uint in_address, uint in_data)
        {
            in_address = in_address & 0xffffff;
            if (0xe00000 <= in_address) in_address = (in_address & 0xffff) | 0xff0000;
            g_memory[in_address] = (byte)(in_data >> 24);
            g_memory[in_address + 1] = (byte)((in_data >> 16) & 0x00ff);
            g_memory[in_address + 2] = (byte)((in_data >> 8) & 0x00ff);
            g_memory[in_address + 3] = (byte)(in_data & 0x00ff);
        }
    }
}

