namespace SubversionToGit
{
    using System.IO;
    using System.Xml.Linq;

    static class RetrieveRevisions
    {
        private const string filename = "SubversionRevisionHistory.xml";

        public static XDocument Execute()
        {
            string directory = Path.GetDirectoryName(Controls.LogFilePath);
            string path = Path.Combine(directory, filename);

            Logger.WriteInfo("START LOAD REVISION HISTORY");
            var command = SubversionCommand.CreateListRevisionsForTopLevelFolderXml(Controls.SubversionRepository, path);
            if (!File.Exists(path))
            {
                // TODO: AFTER TESTING GET RID OF THE CHECK FOR FILE EXISTENCE
                Shell.Execute(command);
            }

            var doc = XDocument.Load(path);

            if (!Controls.Debug)
            {
                File.Delete(path);
            }

            return doc;
        }
    }
}
