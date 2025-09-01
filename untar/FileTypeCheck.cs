using untar.Archive;

namespace untar;

internal class FileTypeCheck
{
    internal async Task<IArchiveType> GetFileArchiveTypeAsync(FileInfo fileInfo)
    {
        IArchiveType result = null;
        
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
                    result = singleArchiveType;
                    break;
                }
            }
        }

        if (result is null)
        {
            foreach (IArchiveType singleArchiveType in implementations)
            {
                if (await singleArchiveType.IsArchiveTypeAsync(fileInfo))
                {
                    result = singleArchiveType;
                    break;
                }    
            }
        }
        
        return result;
    }

    private IArchiveType CreateInstance(Type type)
    {
        IArchiveType archiveType = (Activator.CreateInstance(type) as IArchiveType)!;
        return archiveType;
    }
}