using System;
using System.IO;
using System.Collections;

namespace BytesCopier
{
    class Program
    {
        static void Main(string[] args)
        {
            //copy bytes from one file to many files
            //use case is to copy a palette from one bitmap to many bitmaps
            //command line args are
            //"file to copy from" StartOffset(hex), ByteCount(hex) 
#if DEBUG
            args = new string[] { "C:\\Users\\sevans\\source\\repos\\BytesCopier\\BytesCopier\\bin\\Debug\\net5.0\\test\\E.bmp", 36.ToString(), 40.ToString() };
#endif
            string filePath = args[0];
            int offset = int.Parse(args[1], System.Globalization.NumberStyles.HexNumber);
            int count = int.Parse(args[2], System.Globalization.NumberStyles.HexNumber);
            byte[] copyfrom = File.ReadAllBytes(filePath);
            byte[] copyThis = new byte[count];
            int y = 0;
            for (int i = offset; i < offset + count; i++)
            {
                copyThis[y] = copyfrom[i];
                y++;
            }
            foreach (string item in Directory.GetFiles(Path.GetDirectoryName(filePath)))
            {
                if(Path.GetExtension(item) != Path.GetExtension(filePath)) { continue; }
                byte[] target = File.ReadAllBytes(item);
                copyThis.CopyTo(target, offset);
                File.WriteAllBytes(item, target);
            }
            return;
        }
    }
}
