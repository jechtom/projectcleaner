# ProjectCleaner

## Overview

ProjectCleaner is a utility tool designed to help maintain and clean up project directories by identifying and reporting potential issues such as duplicate files, unnecessary Visual Studio files, and suggesting Git garbage collection commands. This tool is handy for developers and project managers who want to keep their project directories clean and organized.

## Features

- **Directory Analysis**: Analyzes a specified directory to identify potential issues.
- **Duplicate File Finder**: Identifies duplicate files within the directory.
- **Visual Studio Cleaner**: Suggests Visual Studio files that can be cleaned up.
- **Git Garbage Collection**: Suggests Git GC commands for identified Git repositories within the directory.
- **Report Generation**: Generates detailed reports for the identified issues.

## How to Use

To use the ProjectCleaner, you need to run the executable with the appropriate parameters. Here is how you can do it:

```sh
ProjectsCleaner.exe INPUT_FOLDER_TO_ANALYZE [OUTPUT_FOLDER_FOR_REPORTS]
```

- `INPUT_FOLDER_TO_ANALYZE`: Required. The path to the folder will be analyzed.
- `OUTPUT_FOLDER_FOR_REPORTS`: Optional. The path to the folder where reports will be saved. If not set, the input folder will be used.

## Note

When you run the ProjectCleaner, it only generates reports based on the analysis of the specified directory. No actions or changes are made to the files or directories during the run. It is up to the user to review the reports and decide whether to execute the suggested actions. Always review the reports carefully to avoid unintended loss of data or changes to your project directories.

# Known issues

* If file/folder is not accessible, application will crash (no error handling)
* If OutputPath in project file contains `../` (by default it is `bin` or `bin/Release` or `bin/Debug`) then script will not delete this folder
