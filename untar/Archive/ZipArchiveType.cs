using System.IO.Compression;
using untar.Model;

namespace untar.Archive;

public class ZipArchiveType : IArchiveType
{
    public string ArchiveExtensions { get; } = ".zip";
    public ArchiveType Type { get; } = ArchiveType.Zip;
    
    public void Dispose()
    {
        
    }

    public async Task<bool> IsArchiveTypeAsync(FileInfo fileInfo)
    {
        using(FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read))
        using (ZipArchive zipArchiveStream = new ZipArchive(fileStream))
        {
            try
            {
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
        using(FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read))
        using (ZipArchive zipArchiveStream = new ZipArchive(fileStream))
        {
            try
            {
                foreach (ZipArchiveEntry zipArchiveEntry in zipArchiveStream.Entries)
                {
                    if (!zipArchiveEntry.Name.Equals(string.Empty))
                    {
                        string itemPath = Path.Combine(directoryInfo.FullName, zipArchiveEntry.FullName);
                        string folderEntryPath = Path.GetDirectoryName(itemPath)!;

                        if (!Path.Exists(folderEntryPath))
                        {
                            Directory.CreateDirectory(folderEntryPath);
                        }
                        
                        zipArchiveEntry.ExtractToFile(Path.Combine(folderEntryPath, zipArchiveEntry.Name));
                        Console.WriteLine($"unpack: {zipArchiveEntry.FullName}");
                    } 
                }
                return true;
            }
            catch (InvalidDataException)
            {
                return false;
            }
        }
    }
}