namespace SubversionToGit
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    static class Shell
    {
        public static int Execute(ShellCommand command, Dictionary<string, string> environmentVariables = null)
        {
            string fullCommand = $"{command.AppName} {command.Args}";
            Logger.WriteInfo(fullCommand);

            var startInfo = new ProcessStartInfo("cmd", $"/c {fullCommand}");
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            
            if (!string.IsNullOrEmpty(command.WorkingFolder))
            {
                startInfo.WorkingDirectory = command.WorkingFolder;
            }

            if (environmentVariables != null)
            {
                var environment = startInfo.EnvironmentVariables;
                foreach (string key in environmentVariables.Keys)
                {
                    environment[key] = environmentVariables[key];
                }
            }

            using (var process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();

                var outputTask = ConsumeReader(process.StandardOutput);
                var errorTask = ConsumeReader(process.StandardError);

                process.WaitForExit();
                return process.ExitCode;
            }
        }

        private async static Task ConsumeReader(TextReader reader)
        {
            string text;
            while ((text = await reader.ReadLineAsync()) != null)
            {
                Logger.WriteInfo(text);
            }
        }
    }
}
