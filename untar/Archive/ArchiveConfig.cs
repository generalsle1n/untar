namespace untar.Archive;

internal class ArchiveConfig
{
    internal static readonly Type[] ArchiveTypes = {
        typeof(TarArchiveType),
        typeof(TarGzArchiveType)
    };
}