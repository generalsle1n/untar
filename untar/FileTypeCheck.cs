using untar.Archive;
using untar.Model;

namespace untar;

internal class FileTypeCheck
{
    internal async Task<ArchiveType> GetFileArchiveTypeAsync(FileInfo fileInfo)
    {
        ArchiveType archiveTypeResult = ArchiveType.Unknown;
        
        List<IArchiveType> implementations = new List<IArchiveType>();
        
        foreach (Type singleArchiveType in ArchiveConfig.ArchiveTypes)
        {
            IArchiveType archiveType = CreateInstance(singleArchiveType);
            implementations.Add(archiveType);
        }

        foreach (IArchiveType singleArchiveType in implementations)
        {
            if (singleArchiveType.ArchiveExtensions.Equals(fileInfo.Extension, StringComparison.CurrentCultureIgnoreCase))
            {
                if (await singleArchiveType.IsArchiveTypeAsync(fileInfo))
                {
                    archiveTypeResult = singleArchiveType.Type;
                    break;
                }
            }
        }

        if (archiveTypeResult == ArchiveType.Unknown)
        {
            foreach (IArchiveType singleArchiveType in implementations)
            {
                if (await singleArchiveType.IsArchiveTypeAsync(fileInfo))
                {
                    archiveTypeResult = singleArchiveType.Type;
                    break;
                }    
            }
        }
        
        return archiveTypeResult;
    }

    private IArchiveType CreateInstance(Type type)
    {
        IArchiveType archiveType = (Activator.CreateInstance(type) as IArchiveType)!;
        return archiveType;
    }
}