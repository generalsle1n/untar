using untar.Model;

namespace untar.Archive;

internal interface IArchiveType : IDisposable
{
    internal Task<bool> IsArchiveTypeAsync(FileInfo fileInfo);
    internal Task<bool> DecompressArchiveAsync(FileInfo fileInfo, DirectoryInfo directoryInfo);

    internal string GetFolderName(FileInfo fileInfo)
    {
        return fileInfo.Name.Replace(fileInfo.Extension, "").Replace(".","_");
    }
    internal string ArchiveExtensions { get; }
    internal ArchiveType Type { get; }
}