
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZlibDecode
{
    class Program
    {
        static string help =
              "ZlibDecode.exe [-h] [filename]\n"
            + "  [-h]       this text\n"
            + "  [filename] specify Zlib compressed file.\n"
            + "             Decoded txt file will be generated.\n";
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine(help);

                return;
            }
            else
            {
                string opt = args[0];
                if (String.IsNullOrEmpty(opt) || opt == "-h" || opt == "--h" || opt == "-help" || opt == "--help" || opt == "/help")
                {
                    Console.WriteLine(help);
                    return;
                }
            }

            foreach (string fname in args ){
                String fn = fname.Trim();
                using (FileStream fs = new FileStream(fn, FileMode.Open))
                {
                    //2byte進めておく（zlib圧縮で先頭にヘッダ0x78,0xDAがついているが、DeflateStreamはヘッダ不要のため。）
                    fs.ReadByte();
                    fs.ReadByte();
                    using (DeflateStream ds = new DeflateStream(fs, CompressionMode.Decompress))
                    {
                        using (FileStream fso = new FileStream(fn + ".txt", FileMode.Create))
                        {
                            byte[] data = new byte[512];
                            while (true)
                            {
                                int rs = ds.Read(data, 0, data.Length);
                                fso.Write(data, 0, rs);
                                if (rs == 0) break;
                            }
                        }
                    }
                }
            }
        }
    }
}
