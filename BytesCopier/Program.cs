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
            //args = new string[] { "C:\\Users\\sevans\\source\\repos\\BytesCopier\\BytesCopier\\bin\\Debug\\net5.0\\test\\@.bmp", 36.ToString(), 40.ToString() };
            args = new string[] { "undo" };
#endif
            string undoPath = AppDomain.CurrentDomain.BaseDirectory + ".undoBytes\\";
            if (args[0].ToLower() == "undo")
            {
                //restore all the files from the undo folder
                if (!Directory.Exists(undoPath) || ReadUndoDestination() == "")
                {
                    Console.WriteLine("Error: Undo folder not found at\n" + undoPath);
                    return;
                }
                RestoreFromUndo();
                return;
            }
            ClearUndoFolder();
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

            WriteUndoDestination(Path.GetDirectoryName(filePath));
            foreach (string item in Directory.GetFiles(Path.GetDirectoryName(filePath)))
            {
                if(Path.GetExtension(item) != Path.GetExtension(filePath)) { continue; }

                BackupFile(item);
                byte[] target = File.ReadAllBytes(item);
                copyThis.CopyTo(target, offset);
                File.WriteAllBytes(item, target);
            }
            return;
        }

        //there is probably a better was to implement the Undo Path but this was fastest.
        public static void BackupFile(string file)
        {
            //create undo folder and copy all the affected files of the operation there.
            string undoPath = AppDomain.CurrentDomain.BaseDirectory + ".undoBytes\\";
            File.Copy(file, undoPath + Path.GetFileName(file));
            return;
        }
        public static void ClearUndoFolder()
        {
            //create undo folder and copy all the affected files of the operation there.
            string undoPath = AppDomain.CurrentDomain.BaseDirectory + ".undoBytes\\";
            if (!Directory.Exists(undoPath)) { return; }
            foreach (string file in Directory.GetFiles(Path.GetDirectoryName(undoPath)))
            {
                File.Delete(file);
            }
            return;
        }
        public static void RestoreFromUndo()
        {
            string undoPath = AppDomain.CurrentDomain.BaseDirectory + ".undoBytes\\";
            string destinationPath = ReadUndoDestination();
            foreach (string file in Directory.GetFiles(Path.GetDirectoryName(undoPath)))
            {
                File.Copy(file, destinationPath + Path.GetFileName(file), true);
            }
            return;
        }
        public static void WriteUndoDestination(string workingDirectory)
        {
            string undoPath = AppDomain.CurrentDomain.BaseDirectory + ".undoBytes\\";
            Directory.CreateDirectory(undoPath);
            File.WriteAllText(undoPath + "dest.txt", workingDirectory + "\\");
        }
        public static string ReadUndoDestination()
        {
            string undoPath = AppDomain.CurrentDomain.BaseDirectory + ".undoBytes\\";
            string pathLocation = undoPath + "dest.txt";
            if (!File.Exists(pathLocation))
            {
                return "";
            }
            string workingDirectory = File.ReadAllText(pathLocation);
            return workingDirectory;
        }
    }
}
