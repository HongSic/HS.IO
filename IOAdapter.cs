using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HS.IO
{
    public abstract class IOAdapter
    {
        public abstract char SeparatorChar { get; }
        public abstract bool CanRead { get; }
        public abstract bool CanWrite { get; }
        public abstract bool CanAppend { get; }

        public virtual IOItemKind GetKind(string Path) => GetInfo(Path).Kind;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Type"></param>
        /// <param name="Extension">.foo</param>
        /// <returns></returns>
        public abstract List<string> GetItems(string Path, ItemType Type, string Extension = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Type"></param>
        /// <param name="Extension">.foo</param>
        /// <returns></returns>
        public virtual Task<List<string>> GetItemsAsync(string Path, ItemType Type, string Extension = null) => Task.Run(() => GetItems(Path, Type, Extension));

        public abstract IOItemInfo GetInfo(string Path);
        public virtual Task<IOItemInfo> GetInfoAsync(string Path) => Task.Run(() => GetInfo(Path));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Kind">None is Auto</param>
        public abstract void Delete(string Path, IOItemKind Kind = IOItemKind.None);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Kind">None is Auto</param>
        public virtual Task DeleteAsync(string Path, IOItemKind Kind = IOItemKind.None) => Task.Run(() => Delete(Path, Kind));
        public abstract bool Exist(string Path);
        public virtual Task<bool> ExistAsync(string Path) => Task.Run(() => Exist(Path));
        public abstract void CreateDirectory(string Path);
        public virtual Task CreateDirectoryAsync(string Path) => Task.Run(() => CreateDirectory(Path));
        public abstract Stream Create(string Path);
        public virtual Task<Stream> CreateAsync(string Path, bool IsDirectory) => Task.Run(() => Create(Path));
        public abstract Stream Append(string Path);
        public virtual Task<Stream> AppendAsync(string Path, bool IsDirectory) => Task.Run(() => Append(Path));
        public abstract Stream Open(string Path);
        public virtual Task<Stream> OpenAsync(string Path) => Task.Run(() => Open(Path));

        public enum ItemType
        {
            All = File | Directory,
            File = 2,
            Directory = 4,
        }
    }
}
