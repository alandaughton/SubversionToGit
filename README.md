# SubversionToGit

This is a command line tool that can import a Subversion repository to a local Git repository.  Unlike other similar tools, it will not fail in the face of tree conflicts.  Instead a list of conflicted files is provided at the end of the run.  These will need to be imported manually.  Unfortunately that means the history for those files will be lost.

The following command line options are available:


--help      Print out the help message

--version   Print out version information


-svn <url>            URL to Subversion repository (REQUIRED)
  
-dir <path>           Directory where downloaded files will be placed (REQUIRED)
  
-authors <path>       Location of file that holds a map from Subversion authors to Git authors + emails (REQUIRED)
  
[-history <path>]     Location to store the revision history XML file (OPTIONAL)
  
[-log <path>]         Location where logging data is written (OPTIONAL)
  
[-readme <name>]      Name to use for the README.md file placed in intitial Git commit (OPTIONAL)
  
[--shortlog]          Suppress some logging to keep the log file shorter than it would normally be (OPTIONAL)

[--quiet|--verbose]   Quiet mode does not write to the console.  Verbose writes everything to the console.

Report bugs to alandaughton@gmail.com
