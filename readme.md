# How to use ProjectCleaner?

## Overview

ProjectCleaner is a utility tool designed to help maintain and clean up project directories by identifying and reporting potential issues such as duplicate files, unnecessary Visual Studio files, and suggesting Git garbage collection commands. This tool is handy for developers and project managers who want to keep their project directories clean and organized.

## Parameters

Build and execute `ProjectCleaner.exe` with these parameters:

```
ProjectsCleaner.exe INPUT_FOLDER_TO_ANALYZE [OUTPUT_FOLDER_FOR_REPORTS]
```

`INPUT_FOLDER_TO_ANALYZE`
  - Required. Path to folder which will be analyzed.

`OUTPUT_FOLDER_FOR_REPORTS`
  - Optional. Path to folder where reports will be saved.
    If not set, input folder will be used.

For example: `ProjectsCleaner.exe c:\Dev\`

## What will happen?

All data in given folder will be read and reports will be generated in output folder.

There are these types of reports:

### GIT GC Clean Script

This report is also CMD script which can be executed. It will run GC command on each GIT repository found inside given location. 
This can reduce number and size of files in repository folder.

### Duplicate Files Report

You can review list of duplicate files in given folder.
Files are compared by binary content and ordered by total size (size multipled by number of files).

### Visual Studio Clean Project Script

This script can delete all output folders (`bin/Release` etc.) and `obj` folder inside project directory.
This feature currently supports CSPROJ, VBPROJ project files in XML format.

# Known issues

* If file/folder is not accessible, application will crash (no error handling)
* If OutputPath in project file contains `../` (by default it is `bin` or `bin/Release` or `bin/Debug`) then script will not delete this folder
