using System.Collections.Generic;
using System.IO;

namespace HS.IO.Disk
{
    public class DiskIOAdapter : IOAdapter
    {
        public DiskIOAdapter() { }

        public override void Create(string Path, bool IsDirectory)
        {
            if (IsDirectory) Directory.CreateDirectory(Path);
            else File.Create(Path).Close();
        }

        public override void Delete(string Path) => File.Delete(Path);
        public override bool Exist(string Path) => File.Exists(Path) || Directory.Exists(Path);
        public override IOItemKind GetKind(string Path) => Directory.Exists(Path) ? IOItemKind.Directory : IOItemKind.File;

        public override IOItemInfo GetInfo(string Path) => new DiskIOItemInfo(Path);

        public override Stream Open(string Path) => new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);

        public override List<string> GetItems(string Path, ItemType Type, string Extension = null)
        {
            if (Type == ItemType.File) return new List<string>(Directory.GetFiles(Path, Extension));
            else if(Type == ItemType.Directory) return new List<string>(Directory.GetDirectories(Path, Extension));
            else
            {
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
