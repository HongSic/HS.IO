using System;
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
        public override bool CanChangeTimsstamp => true;

        public DiskIOAdapter() { }

        public override bool ExistDirectory(string Path) => Directory.Exists(Path);
        public override void CreateDirectory(string Path) => Directory.CreateDirectory(Path);
        public override void DeleteDirectory(string Path) => Directory.Delete(Path);

        public override void Create(string Path) => File.Create(Path);
        public override void Append(string Path, Stream Data)
        {
            using (var stream = new FileStream(Path, FileMode.Append, FileAccess.ReadWrite, FileShare.Read))
                stream.CopyTo(Data);
        }

        public override void Delete(string Path)
        {
            var kind = GetKind(Path);
            if (kind == IOItemKind.Directory) Directory.Delete(Path, true);
            else File.Delete(Path);
        }
        public override bool Exist(string Path) => File.Exists(Path);
        public override IOItemKind GetKind(string Path)
        {
            if (Directory.Exists(Path)) return IOItemKind.Directory;
            else if (File.Exists(Path)) return IOItemKind.File;
            else return IOItemKind.None;
        }

        public override IOItemInfo GetInfo(string Path) => new DiskIOItemInfo(Path);

        public override Stream Open(string Path) => new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);

        public override void Move(string OriginalPath, string DestinationPath)
        {
            if(File.Exists(OriginalPath)) File.Move(OriginalPath, DestinationPath);
            else if(Directory.Exists(OriginalPath)) Directory.Move(OriginalPath, DestinationPath);
        }

        public override void SetTimestamp(string Path, DateTime Timestamp, IOItemKind Kind = IOItemKind.None)
        {
            if (Kind == IOItemKind.None)
            {
                var kind = GetKind(Path);
                if (kind == IOItemKind.File) File.SetLastWriteTime(Path, Timestamp);
                else if (kind == IOItemKind.Directory) Directory.SetLastWriteTime(Path, Timestamp);
            }
            else if (Kind == IOItemKind.Directory) Directory.SetLastWriteTime(Path, Timestamp);
            else File.SetLastWriteTime(Path, Timestamp);
        }

        public override List<string> GetItems(string Path, ItemType Type, string Extension = null)
        {
            if (Type == ItemType.File) return new List<string>(Directory.GetFiles(Path, Extension));
            else if(Type == ItemType.Directory) return new List<string>(Directory.GetDirectories(Path, Extension));
            else
            {
                Extension = Extension ?? "";
                var files = Directory.GetFiles(Path, Extension);
                var dirs = Directory.GetDirectories(Path, Extension);
                var all = new List<string>(files.Length + dirs.Length);
                all.AddRange(files);
                all.AddRange(dirs);
                return all;
            }
        }

        public override void Dispose() { }
    }
}
