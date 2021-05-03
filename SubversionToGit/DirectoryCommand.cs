namespace SubversionToGit
{
    static class DirectoryCommand
    {
        public static ShellCommand ListFilesAndFolders(string workingDirectory, string outputPath)
        {
            return new ShellCommand("dir", $"/A /b > \"{outputPath}\"", workingDirectory);
        }
    }
}
