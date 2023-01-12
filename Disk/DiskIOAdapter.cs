using System.Collections.Generic;
using System.IO;

namespace HS.IO.Disk
{
    public class DiskIOAdapter : IOAdapter
    {
        public override char SeparatorChar => Path.DirectorySeparatorChar;
        public override bool CanRead => true;
        public override bool CanWrite => true;
        public override bool CanAppend => true;

        public DiskIOAdapter() { }

        public override void CreateDirectory(string Path) => Directory.CreateDirectory(Path);

        public override Stream Create(string Path) => new FileStream(Path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
        public override Stream Append(string Path) => new FileStream(Path, FileMode.Append, FileAccess.ReadWrite, FileShare.Read);

        public override void Delete(string Path, IOItemKind Kind = IOItemKind.None)
        {
            if(Kind == IOItemKind.None)
            {
                var kind = GetKind(Path);
                if (kind == IOItemKind.File) File.Delete(Path);
                else if (kind == IOItemKind.Directory) Directory.Delete(Path, true);
            }
            else if(Kind == IOItemKind.Directory) Directory.Delete(Path, true);
            else File.Delete(Path);
        }
        public override bool Exist(string Path) => File.Exists(Path) || Directory.Exists(Path);
        public override IOItemKind GetKind(string Path)
        {
            if (Directory.Exists(Path)) return IOItemKind.Directory;
            else if (File.Exists(Path)) return IOItemKind.File;
            else return IOItemKind.None;
        }

        public override IOItemInfo GetInfo(string Path) => new DiskIOItemInfo(Path);

        public override Stream Open(string Path) => new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);

        public override List<string> GetItems(string Path, ItemType Type, string Extension = null)
        {
            if (Type == ItemType.File) return new List<string>(Directory.GetFiles(Path, Extension));
            else if(Type == ItemType.Directory) return new List<string>(Directory.GetDirectories(Path, Extension));
            else
            {
                Extension = Extension == null ? "" : Extension;
                var files = Directory.GetFiles(Path, Extension);
                var dirs = Directory.GetDirectories(Path, Extension);
                var all = new List<string>(files.Length + dirs.Length);
                all.AddRange(files);
                all.AddRange(dirs);
                return all;
            }
        }
    }
}
