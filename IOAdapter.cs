using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HS.IO
{
    public abstract class IOAdapter : IDisposable
    {
        public abstract char SeparatorChar { get; }
        public abstract bool CanRead { get; }
        public abstract bool CanWrite { get; }
        public abstract bool CanAppend { get; }
        public abstract bool CanChangeTimsstamp { get; }

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
        public virtual Task<List<string>> GetItemsAsync(string Path, ItemType Type, string Extension = null, CancellationToken cancellationToken = default) => Task.Run(() => GetItems(Path, Type, Extension), cancellationToken);

        public abstract IOItemInfo GetInfo(string Path);
        public virtual Task<IOItemInfo> GetInfoAsync(string Path, CancellationToken cancellationToken = default) => Task.Run(() => GetInfo(Path), cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        public abstract void Delete(string Path);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        public virtual Task DeleteAsync(string Path, CancellationToken cancellationToken = default) => Task.Run(() => Delete(Path), cancellationToken);
        public abstract bool Exist(string Path);
        public virtual Task<bool> ExistAsync(string Path, CancellationToken cancellationToken = default) => Task.Run(() => Exist(Path), cancellationToken);
        public abstract void CreateDirectory(string Path);
        public virtual Task CreateDirectoryAsync(string Path, CancellationToken cancellationToken = default) => Task.Run(() => CreateDirectory(Path), cancellationToken);
        public abstract void Create(string Path);
        public virtual Task CreateAsync(string Path, CancellationToken cancellationToken = default) => Task.Run(() => Create(Path), cancellationToken);
        public abstract void Append(string Path, Stream Data);
        public virtual Task AppendAsync(string Path, Stream Data, CancellationToken cancellationToken = default) => Task.Run(() => Append(Path, Data), cancellationToken);
        public abstract Stream Open(string Path);
        public virtual Task<Stream> OpenAsync(string Path, CancellationToken cancellationToken = default) => Task.Run(() => Open(Path), cancellationToken);
        public virtual void Write(string Path, Stream Data) => Data.CopyTo(Open(Path));
        public virtual Task WriteAsync(string Path, Stream Data, CancellationToken cancellationToken = default) => Task.Run(() => Write(Path, Data), cancellationToken);
        public virtual void Move(string OriginalPath, string DestinationPath) { Copy(OriginalPath, DestinationPath); Delete(OriginalPath); }
        public virtual async Task MoveAsync(string OriginalPath, string DestinationPath, CancellationToken cancellationToken = default) { await CopyAsync(OriginalPath, DestinationPath, cancellationToken); await DeleteAsync(OriginalPath, cancellationToken); }
        public virtual void Copy(string OriginalPath, string DestinationPath) => Copy(this, OriginalPath, this, DestinationPath);
        public virtual Task CopyAsync(string OriginalPath, string DestinationPath, CancellationToken cancellationToken = default) => CopyAsync(this, OriginalPath, this, DestinationPath);
        public abstract void SetTimestamp(string Path, DateTime Timestamp, IOItemKind Kind = IOItemKind.None);
        public virtual Task SetTimestampAsync(string Path, DateTime Timestamp, IOItemKind Kind = IOItemKind.None, CancellationToken cancellationToken = default) => Task.Run(() => SetTimestamp(Path, Timestamp, Kind), cancellationToken);

        public abstract void Dispose();

        public enum ItemType
        {
            All = File | Directory,
            File = 2,
            Directory = 4,
        }

        #region Static Class
        public static void Copy(IOAdapter Original, string OriginalPath, IOAdapter Destination, string DestinationPath)
        {
            var orig = Original.Open(OriginalPath);
            Destination.Create(DestinationPath);
            try { Destination.Write(DestinationPath, orig); }
            catch
            {
                try { Destination.Delete(DestinationPath); } catch { }
                throw;
            }
        }
        public static async Task CopyAsync(IOAdapter Original, string OriginalPath, IOAdapter Destination, string DestinationPath)
        {
            var orig = await Original.OpenAsync(OriginalPath);
            await Destination.CreateAsync(DestinationPath);
            try { await Destination.WriteAsync(DestinationPath, orig); }
            catch
            {
                try { await Destination.DeleteAsync(DestinationPath); } catch { }
                throw;
            }
        }
        #endregion
    }
}
