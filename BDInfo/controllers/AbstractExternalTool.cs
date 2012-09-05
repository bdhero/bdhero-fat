using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace BDInfo.controllers
{
    abstract class AbstractExternalTool
    {
        IList<string> paths = new List<string>();

        protected abstract void ExtractResources();
        protected abstract string GetToolName();
        
        public abstract void Test();

        protected string GetTempDirectory()
        {
            string path = Path.Combine(Path.GetTempPath(), Process.GetCurrentProcess().Id.ToString(), GetToolName());
            DirectoryInfo dir = Directory.CreateDirectory(path);
            return path;
        }

        protected string GetResourceName(string filename)
        {
            return "BDInfo.lib.exe." + filename;
        }

        protected string GetTempPath(string filename)
        {
            return Path.Combine(GetTempDirectory(), filename);
        }

        protected string ExtractResource(string filename)
        {
            string resource = GetResourceName(filename);
            string destPath = GetTempPath(filename);
            ExtractResource(resource, destPath);
            paths.Add(destPath);
            return destPath;
        }

        // extracts [resource] into the the file specified by [destPath]
        protected void ExtractResource(string resource, string destPath)
        {
            Stream stream = GetType().Assembly.GetManifestResourceStream(resource);
            byte[] bytes = new byte[(int)stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            File.WriteAllBytes(destPath, bytes);
        }

        protected void Cleanup()
        {
            foreach (string path in paths)
            {
                File.Delete(path);
            }

            DirectoryInfo dir = new DirectoryInfo(GetTempDirectory());

            if (dir.GetFiles().Length == 0 && dir.GetDirectories().Length == 0)
            {
                MessageBox.Show("Temp Directory \"" + GetTempDirectory() + "\" is empty.  Deleting...");
                dir.Delete();
            }
            else
            {
                MessageBox.Show("Temp Directory \"" + GetTempDirectory() + "\" is NOT empty.  Not deleting.");
            }
        }
    }
}
