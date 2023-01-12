using System;
using System.IO;

namespace HS.IO.Disk
{
    public class DiskIOItemInfo : IOItemInfo
    {
        public DiskIOItemInfo(string Path): base(Path)
        {
            if (Directory.Exists(Path))
            {
                DirectoryInfo info = new DirectoryInfo(Path);
                _Kind = IOItemKind.Directory;
                _ModifyTime = info.LastWriteTime;
                _CreateTime = info.CreationTime;
            }
            else
            {
                FileInfo info = new FileInfo(Path);
                _Length = info.Length;
                _ModifyTime = info.LastWriteTime;
                _CreateTime = info.CreationTime;
            }
        }

        readonly IOItemKind _Kind = IOItemKind.File;
        readonly long _Length = -1;
        readonly DateTime _ModifyTime;
        readonly DateTime _CreateTime;
        public override IOItemKind Kind => _Kind;

        public override long Length => _Length;

        public override DateTime ModifyTime => _ModifyTime;
        public override DateTime CreateTime => _CreateTime;
    }
}
