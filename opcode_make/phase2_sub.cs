using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics.Eventing.Reader;

namespace opcode_make
{
    internal partial class Program
    {
        static void outsub2(params object[] in_val)
        {
            string w_test = "";
            foreach (var wval in in_val)
            {
                w_test += wval.ToString();
            }
            System.IO.File.AppendAllText("md_m68k_initialize2.cs", w_test + Environment.NewLine);
        }
    }
}
