using HS.Utils.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HS.IO
{
    public abstract class IOAdapter : IDisposable
    {
        /// <summary>경로 구분 문자.</summary>
        public abstract char SeparatorChar { get; }
        /// <summary>읽기 가능 여부.</summary>
        public abstract bool CanRead { get; }
        /// <summary>쓰기 가능 여부.</summary>
        public abstract bool CanWrite { get; }
        /// <summary>추가 쓰기(append) 가능 여부.</summary>
        public abstract bool CanAppend { get; }
        /// <summary>타임스탬프 변경 가능 여부.</summary>
        public abstract bool CanChangeTimsstamp { get; }

        /// <summary>경로의 항목 종류를 반환합니다.</summary>
        public virtual IOItemKind GetKind(string Path) => GetInfo(Path).Kind;

        /// <summary>경로를 병합하여 IOPath를 생성합니다.</summary>
        public virtual IOPath MergePath(string OriginalPath, string DestinationPath = null, IOPathKind Kind = IOPathKind.Absolute) => new IOPath(StringUtils.PathMaker(OriginalPath, DestinationPath, SeparatorChar), Kind);

        /// <summary>
        /// 목록을 조회합니다.
        /// </summary>
        /// <param name="Path">조회 대상 경로.</param>
        /// <param name="Type">조회할 항목 타입.</param>
        /// <param name="Extension">필터링할 확장자 (예: .foo).</param>
        /// <returns>경로 목록.</returns>
        public abstract List<string> GetItems(string Path, ItemType Type, string Extension = null);
        /// <summary>
        /// 목록을 비동기로 조회합니다.
        /// </summary>
        /// <param name="cancellationToken">작업 취소 토큰.</param>
        public virtual Task<List<string>> GetItemsAsync(string Path, ItemType Type, CancellationToken cancellationToken, string Extension = null) => Task.Run(() => GetItems(Path, Type, Extension), cancellationToken);
        public virtual Task<List<string>> GetItemsAsync(string Path, ItemType Type, string Extension = null) => GetItemsAsync(Path, Type, CancellationToken.None, Extension);

        /// <summary>
        /// 경로 정보를 조회합니다.
        /// </summary>
        public abstract IOItemInfo GetInfo(string Path);
        /// <summary>
        /// 경로 정보를 비동기로 조회합니다.
        /// </summary>
        /// <param name="cancellationToken">작업 취소 토큰.</param>
        public virtual Task<IOItemInfo> GetInfoAsync(string Path, CancellationToken cancellationToken) => Task.Run(() => GetInfo(Path), cancellationToken);
        public virtual Task<IOItemInfo> GetInfoAsync(string Path) => GetInfoAsync(Path, CancellationToken.None);
        #region
        /// <summary>디렉터리 존재 여부를 확인합니다.</summary>
        public abstract bool ExistDirectory(string Path);
        /// <summary>디렉터리 존재 여부를 비동기로 확인합니다.</summary>
        public virtual Task<bool> ExistDirectoryAsync(string Path, CancellationToken cancellationToken) => Task.Run(() => ExistDirectory(Path), cancellationToken);
        public Task<bool> ExistDirectoryAsync(string Path) => ExistDirectoryAsync(Path, CancellationToken.None);
        /// <summary>디렉터리를 생성합니다.</summary>
        public abstract void CreateDirectory(string Path);
        /// <summary>디렉터리를 비동기로 생성합니다.</summary>
        public virtual Task CreateDirectoryAsync(string Path, CancellationToken cancellationToken) => Task.Run(() => CreateDirectory(Path), cancellationToken);
        public Task CreateDirectoryAsync(string Path) => CreateDirectoryAsync(Path, CancellationToken.None);
        /// <summary>디렉터리를 삭제합니다.</summary>
        public abstract void DeleteDirectory(string Path);
        /// <summary>디렉터리를 비동기로 삭제합니다.</summary>
        public virtual Task DeleteDirectoryAsync(string Path, CancellationToken cancellationToken) => Task.Run(() => DeleteDirectory(Path), cancellationToken);
        public Task DeleteDirectoryAsync(string Path) => DeleteDirectoryAsync(Path, CancellationToken.None);
        /// <summary>파일을 생성합니다.</summary>
        public abstract void Create(string Path);
        /// <summary>파일을 비동기로 생성합니다.</summary>
        public virtual Task CreateAsync(string Path, CancellationToken cancellationToken) => Task.Run(() => Create(Path), cancellationToken);
        public Task CreateAsync(string Path) => CreateAsync(Path, CancellationToken.None);
        #endregion
        /// <summary>
        /// 경로의 항목을 삭제합니다.
        /// </summary>
        /// <param name="Path"></param>
        public abstract void Delete(string Path);
        /// <summary>
        /// 경로의 항목을 비동기로 삭제합니다.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task DeleteAsync(string Path, CancellationToken cancellationToken) => Task.Run(() => Delete(Path), cancellationToken);
        public Task DeleteAsync(string Path) => DeleteAsync(Path, CancellationToken.None);
        /// <summary>파일 또는 디렉터리 존재 여부를 확인합니다.</summary>
        public abstract bool Exist(string Path);
        /// <summary>파일 또는 디렉터리 존재 여부를 비동기로 확인합니다.</summary>
        public virtual Task<bool> ExistAsync(string Path, CancellationToken cancellationToken) => Task.Run(() => Exist(Path), cancellationToken);
        public Task<bool> ExistAsync(string Path) => ExistAsync(Path, CancellationToken.None);
        /// <summary>지정 경로 끝에 데이터를 추가합니다.</summary>
        public abstract void Append(string Path, Stream Data);
        /// <summary>데이터를 비동기로 추가합니다.</summary>
        public virtual Task AppendAsync(string Path, Stream Data, CancellationToken cancellationToken) => Task.Run(() => Append(Path, Data), cancellationToken);
        public Task AppendAsync(string Path, Stream Data) => AppendAsync(Path, Data, CancellationToken.None);
        /// <summary>스트림으로 파일을 엽니다.</summary>
        public abstract Stream Open(string Path);
        /// <summary>파일을 비동기로 엽니다.</summary>
        public virtual Task<Stream> OpenAsync(string Path, CancellationToken cancellationToken) => Task.Run(() => Open(Path), cancellationToken);
        public Task<Stream> OpenAsync(string Path) => OpenAsync(Path, CancellationToken.None);
        /// <summary>스트림을 지정된 경로에 기록합니다.</summary>
        public virtual void Write(string Path, Stream Data) => Data.CopyTo(Open(Path));
        /// <summary>스트림을 비동기로 기록합니다.</summary>
        public virtual Task WriteAsync(string Path, Stream Data, CancellationToken cancellationToken) => Task.Run(() => Write(Path, Data), cancellationToken);
        public Task WriteAsync(string Path, Stream Data) => WriteAsync(Path, Data, CancellationToken.None);
        /// <summary>파일을 이동합니다.</summary>
        public virtual void Move(string OriginalPath, string DestinationPath) { Copy(OriginalPath, DestinationPath); Delete(OriginalPath); }
        /// <summary>파일을 비동기로 이동합니다.</summary>
        public virtual async Task MoveAsync(string OriginalPath, string DestinationPath, CancellationToken cancellationToken) { await CopyAsync(OriginalPath, DestinationPath, cancellationToken); await DeleteAsync(OriginalPath, cancellationToken); }
        public Task MoveAsync(string OriginalPath, string DestinationPath) => MoveAsync(OriginalPath, DestinationPath, CancellationToken.None);
        /// <summary>파일을 복사합니다.</summary>
        public virtual void Copy(string OriginalPath, string DestinationPath) => Copy(this, OriginalPath, this, DestinationPath);
        /// <summary>파일을 비동기로 복사합니다.</summary>
        public virtual async Task CopyAsync(string OriginalPath, string DestinationPath, CancellationToken cancellationToken)
        {
            var orig = await OpenAsync(OriginalPath, cancellationToken);
            await CreateAsync(DestinationPath, cancellationToken);
            try { await WriteAsync(DestinationPath, orig, cancellationToken); }
            catch
            {
                try { await DeleteAsync(DestinationPath, cancellationToken); } catch { }
                throw;
            }
        }
        public Task CopyAsync(string OriginalPath, string DestinationPath) => CopyAsync(OriginalPath, DestinationPath, CancellationToken.None);
        /// <summary>파일 또는 디렉터리 타임스탬프를 설정합니다.</summary>
        public abstract void SetTimestamp(string Path, DateTime Timestamp, IOItemKind Kind = IOItemKind.None);
        /// <summary>타임스탬프를 비동기로 설정합니다.</summary>
        public virtual Task SetTimestampAsync(string Path, DateTime Timestamp, CancellationToken cancellationToken, IOItemKind Kind = IOItemKind.None) => Task.Run(() => SetTimestamp(Path, Timestamp, Kind), cancellationToken);
        public Task SetTimestampAsync(string Path, DateTime Timestamp, IOItemKind Kind = IOItemKind.None) => SetTimestampAsync(Path, Timestamp, CancellationToken.None, Kind);

        /// <summary>자원을 해제합니다.</summary>
        public abstract void Dispose();

        /// <summary>경로 조회/필터링에 사용되는 항목 종류.</summary>
        public enum ItemType
        {
            All = File | Directory,
            File = 2,
            Directory = 4,
        }

        #region Static Class
        /// <summary>동기 복사 유틸리티.</summary>
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
        /// <summary>비동기 복사 유틸리티.</summary>
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
