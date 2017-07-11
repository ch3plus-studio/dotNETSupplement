using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace ch3plusStudio.dotNETSupplement.IO
{
    /// <summary>
    /// Lightweight and immutable class for file status comparsion
    /// </summary>

    public sealed class FileChangesInfoSnapshot
    {
        public static bool operator == (FileChangesInfoSnapshot a, FileChangesInfoSnapshot b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator != (FileChangesInfoSnapshot a, FileChangesInfoSnapshot b)
        {
            return !(a == b);
        }

        private FileChangesInfoSnapshot() { }

        public FileChangesInfoSnapshot(string path)
        {
            InitWithFileInfo(new FileInfo(path));
        }

        public FileChangesInfoSnapshot(FileInfo fileInfo)
        {
            InitWithFileInfo(fileInfo);
        }

        private void InitWithFileInfo(FileInfo fileInfo)
        {
            this.LastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
            this.FileSizeInByte = fileInfo.Exists ? fileInfo.Length : -1;
        }

        public DateTime LastWriteTimeUtc { get; private set; }
        public long FileSizeInByte { get; private set; }
        public bool Exists { get { return !(FileSizeInByte == -1); } }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to this class return false.
            if ((obj as FileChangesInfoSnapshot) == null)
            {
                return false;
            }

            return this.Equals((FileChangesInfoSnapshot)obj);
        }

        public bool Equals(FileChangesInfoSnapshot fmi)
        {
            if ((object)fmi == null)
            {
                return false;
            }

            return
                this.FileSizeInByte == fmi.FileSizeInByte
                && this.LastWriteTimeUtc.Equals(fmi.LastWriteTimeUtc);
        }
    }
}
