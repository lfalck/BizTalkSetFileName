[![license](https://img.shields.io/github/license/lfalck/compare-remoteassemblies.svg)]()
# BizTalkSetFileName
A BizTalk pipeline component to change filenames.

Set the filename to **%SourceFileName%** and configure the following parameters.

**Include original filename? (true/false)**  
{0} in custom format.

**XPath1**  
XPath to value in message to include in filename.  
{1} in custom format.

**XPath2**  
{2} in custom format.

**XPath3**  
{3} in custom format.
    
**File extension**  
Specify a file extension if it should be changed, default is to use the one from the original file name.

**Separator**  
Specify a separator if it should be changed, default is "_".

**Include date?**  
Default format is yyyy-MM-ddTHHmmss.
{2} in custom format.

**Date format**  
Specify a [date format](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings) if it should be changed.

**Custom format**  
Specify a custom format, default is {0}{1}{2}{3}{4}.

        

       
