using System.CommandLine;
using untar.Archive;

namespace untar;

class Program
{
    private const string MainDescription = "This is an simple tool to extract tar/tar.gz files";
    private const string ArgumentName = "File";

    static async Task<int> Main(string[] args)
    {
        RootCommand root = new RootCommand(MainDescription);
        
        Argument<FileInfo> fileArgument = new Argument<FileInfo>(ArgumentName)
        {
            Arity = ArgumentArity.ExactlyOne
        };
        
        root.Arguments.Add(fileArgument);
        
        root.SetAction(async (parseResult) =>
        {
            FileInfo fileInfo = parseResult.GetRequiredValue(fileArgument);

            FileTypeCheck fileArchiveChecker = new FileTypeCheck();
            IArchiveType type =  await fileArchiveChecker.GetFileArchiveTypeAsync(fileInfo);
            
            string folderName = type.GetFolderName(fileInfo);
            string absoluteFolder = Path.Combine(fileInfo.DirectoryName, folderName);
            
            Directory.CreateDirectory(absoluteFolder);
            DirectoryInfo directoryInfo = new DirectoryInfo(absoluteFolder);
            await type.DecompressArchiveAsync(fileInfo, directoryInfo);
        });
        
        ParseResult result =  root.Parse(args);
        return await result.InvokeAsync();
    }
}
