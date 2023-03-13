using System;

namespace HS.IO
{
    public class IOPath
    {
        public IOPath(string Path, IOPathKind Kind)
        {
            this.Path = Path;
            this.Kind = Kind;
        }

        public string Path { get; private set; }
        public IOPathKind Kind { get; private set; }

        public object Clone() => new IOPath(Path, Kind);

        public override string ToString() => Path;

        public static implicit operator string(IOPath Path) => Path?.ToString();
        public static implicit operator IOPath(string Path) => new IOPath(Path, IOPathKind.Relative);

        public static readonly IOPath Empty = new IOPath(null, IOPathKind.Relative);
    }

    public enum IOPathKind
    {
        /// <summary>
        /// 완전 절대경로
        /// </summary>
        Absolute,
        /// <summary>
        /// 루트 디렉터리로부터의 상대경로
        /// </summary>
        RelativeRoot,
        /// <summary>
        /// 현재 디렉터리로부터 상대경로
        /// </summary>
        Relative
    }
}
