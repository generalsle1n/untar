using System.Formats.Tar;
using untar.Model;

namespace untar.Archive;

public class TarArchiveType : IArchiveType
{
    public async Task<bool> IsArchiveTypeAsync(FileInfo fileInfo)
    {
        using(FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read))
        using (TarReader tarReader = new TarReader(fileStream))
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
        await using(TarReader tarReader = new TarReader(fileStream))
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

    public string ArchiveExtensions { get; } = ".tar";
    public ArchiveType Type { get; } = ArchiveType.Tar;

    public void Dispose()
    {
        
    }
}