using untar.Model;

namespace untar.Archive;

internal interface IArchiveType : IDisposable
{
    internal Task<bool> IsArchiveTypeAsync(FileInfo fileInfo);
    internal Task<bool> DecompressArchiveAsync(FileInfo fileInfo, DirectoryInfo directoryInfo);
    internal string ArchiveExtensions { get; }
    internal ArchiveType Type { get; }
}