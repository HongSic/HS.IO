using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HS.IO
{
    public abstract class IOAdapter
    {
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
        public abstract void Delete(string Path);
        public virtual Task DeleteAsync(string Path) => Task.Run(() => Delete(Path));
        public abstract bool Exist(string Path);
        public virtual Task<bool> ExistAsync(string Path) => Task.Run(() => Exist(Path));
        public abstract void Create(string Path, bool IsDirectory);
        public virtual Task CreateAsync(string Path, bool IsDirectory) => Task.Run(() => Create(Path, IsDirectory));
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
