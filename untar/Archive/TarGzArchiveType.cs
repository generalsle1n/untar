using System.Formats.Tar;
using System.IO.Compression;
using untar.Model;

namespace untar.Archive;

public class TarGzArchiveType : IArchiveType
{

    public string ArchiveExtensions { get; } = ".gz";
    public ArchiveType Type { get; } = ArchiveType.Targz;
    
    public void Dispose()
    {
        
    }

    public async Task<bool> IsArchiveTypeAsync(FileInfo fileInfo)
    {
        await using(FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read))
        await using(GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
        await using(TarReader tarReader = new TarReader(gzipStream))
        {
            try
            {
                await tarReader.GetNextEntryAsync();
                return true;
            }
            catch (InvalidDataException)
            {
                return false;
            }
        }
    }

    public async Task<bool> DecompressArchiveAsync(FileInfo fileInfo, DirectoryInfo directoryInfo)
    {
        await using(FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read))
        await using(GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
        await using(TarReader tarReader = new TarReader(gzipStream))
        {
            while (await tarReader.GetNextEntryAsync() is TarEntry singleEntry)
            {
                if (singleEntry.EntryType == TarEntryType.Directory)
                {
                    string folderEntryPath = Path.Combine(directoryInfo.FullName, singleEntry.Name);
                    if (!Path.Exists(folderEntryPath))
                    {
                        Directory.CreateDirectory(folderEntryPath);
                        Console.WriteLine($"unpack: {singleEntry.Name}");
                    }
                }
                else
                {
                    string itemPath = Path.Combine(directoryInfo.FullName, singleEntry.Name);
                    string folderEntryPath = Path.GetDirectoryName(itemPath)!;

                    if (!Path.Exists(folderEntryPath))
                    {
                        Directory.CreateDirectory(folderEntryPath);
                    }
                    
                    await singleEntry.ExtractToFileAsync(itemPath, true);
                    Console.WriteLine($"unpack: {singleEntry.Name}");
                }
            }
        }

        return true;
    }
}