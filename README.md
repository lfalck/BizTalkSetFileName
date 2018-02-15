[![license](https://img.shields.io/github/license/lfalck/BizTalkSetFileName.svg)]()
# BizTalkSetFileName
A BizTalk pipeline component which can change filenames to include custom date formats and information from the message.

Set the filename to **%SourceFileName%**.



| Parameter                    | Description                                                               | Type| Validation|
| -----------------------------|---------------------------------------------------------------------------|-----|--------|
|IncludeOriginalFilename|Original filename excluding extension. {0} in custom format.|String|Optional|
|XPath1|XPath to first value in message to include in filename. {1} in custom format.|String|Optional|
|XPath2|XPath to second value in message to include in filename. {1} in custom format.|String|Optional|
|XPath3|XPath to third value in message to include in filename. {2} in custom format.|String|Optional|
|FileExtension|Specify a file extension if it should be changed, default is to use the one from the original file name.|String|Optional|
|Separator|Specify a separator if it should be changed, default is "_".|String|Optional|
|IncludeDate|Include current date? Default format is yyyy-MM-ddTHHmmss.|String|Optional|
|DateFormat|Specify a [date format](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings) if it should be changed.|String|Optional|
|CustomFormat|Optionally specify a custom format, default is {0}{1}{2}{3}{4}. Constant values can also be included.|String|Optional|
