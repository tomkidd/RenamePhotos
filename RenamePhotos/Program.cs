using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenamePhotos
{
    class Program
    {
        static void Main(string[] args)
        {
            string unsortedPath = @"E:\tom\photos\_unsorted";
            string sortedPath = @"E:\tom\photos\_sorted";

            DirectoryInfo diUnsorted = new DirectoryInfo(unsortedPath);

            StringBuilder sb = new StringBuilder();

            List<string> filenames = new List<string>();

            foreach (FileInfo fi in diUnsorted.GetFiles())
            {
                string newName = fi.LastWriteTime.ToString("yyyy-MM-dd HH.mm.ss") + fi.Extension.ToLower();

                string logLine = string.Format("{0} -> {1}", fi.Name, newName);

                if (filenames.Contains(newName) || File.Exists(Path.Combine(sortedPath, newName)))
                {
                    int i = 2;

                    string newPath = Path.Combine(sortedPath, "dupes", newName.Replace(fi.Extension.ToLower(), string.Format(" ({0}){1}", i, fi.Extension.ToLower())));

                    while (File.Exists(newPath))
                    {
                        i++;
                        newPath = Path.Combine(sortedPath, "dupes", newName.Replace(fi.Extension.ToLower(), string.Format(" ({0}){1}", i, fi.Extension.ToLower())));
                    }

                    File.Move(fi.FullName, newPath);

                    logLine += " - DUPE!!!";

                }
                else
                {
                    File.Move(fi.FullName, Path.Combine(sortedPath, newName));
                }
                filenames.Add(newName);

                sb.AppendLine(logLine);
            }

            File.WriteAllText("log.txt", sb.ToString());

            System.Diagnostics.Process.Start("log.txt");
        }
    }
}
