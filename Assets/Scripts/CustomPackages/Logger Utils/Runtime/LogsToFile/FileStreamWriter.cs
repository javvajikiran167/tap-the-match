using UnityEngine;
using System;
using System.IO;

namespace Custom.Utils
{
    public class FileStreamWriter : IDisposable
    {
        private readonly FileStream fs;
        private readonly StreamWriter writer;

        public FileStreamWriter(FileStream fs)
        {
            writer = new StreamWriter(this.fs = fs);
        }

        public void Write(string data)
        {
            try
            {
                writer.Write(data);
                writer.Flush();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Dispose()
        {
            try
            {
                fs.Dispose();
                writer.Dispose();
            }
            catch (Exception)
            {
                Debug.Log("fs or writer Dispose failed");
            }
        }

        public void CloseIfNoNull()
        {
            try
            {
                if (fs != null && writer != null)
                {
                    fs.Close();
                    writer.Close();
                }
            }
            catch (Exception)
            {
                Debug.Log("fs or writer close failed");
            }
        }

        public static FileStreamWriter FromFile(string path, FileMode fileMode )
        {
            Debug.Log("Create File called: " + path);

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            FileStream stream = new FileStream(
                path,
                fileMode,
                FileAccess.Write,
                FileShare.ReadWrite);

            return new FileStreamWriter(stream);
        }
    }
}