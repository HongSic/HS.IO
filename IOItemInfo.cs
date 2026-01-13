using System;

namespace HS.IO
{
    public abstract class IOItemInfo
    {
        private IOItemInfo() { }
        public IOItemInfo(string Path) { this.Path = Path; }

        public virtual string Path { get; }
        public virtual string Name => System.IO.Path.GetFileName(Path);
        public virtual string Type => System.IO.Path.GetFileName(Path).Replace(".", "");

        public abstract IOItemKind Kind { get; }
        public abstract long Length { get; }
        public abstract DateTime? ModifyTime { get; }
        public abstract DateTime? CreateTime { get; }
    }
}
