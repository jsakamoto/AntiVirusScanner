# Anti Virus Scanner for .NET (and COM) [![NuGet Package](https://img.shields.io/nuget/v/AntiVirusScanner.svg)](https://www.nuget.org/packages/AntiVirusScanner/)

## Project Description

This library allows you to scan files for viruses by any .NET languages (C#, F#, VB...), PowerShell, F# Script, WSH (JScript, VBScript), and any COM IDispatch client.


## Quick Start

### C#

1. Download and add reference of this library to your C# project from "NuGet",

- https://www.nuget.org/packages/AntiVirusScanner/

From Package Manager Console,

```powershell
PM> Install-Package AntiVirusScanner
```

or dotnet CLI.

```sh
> dotnet add package AntiVirusScanner
```

2. After installing this library, you can scan a file by C# code like this:

```csharp
var scanner = new AntiVirus.Scanner();
var result = scanner.ScanAndClean(@"c:\some\file\path.txt");
Console.WriteLine(result); // console output is "VirusNotFound".
```

### PowerShell

1. Donwload `AntiVirusScanner.dll` from [download page](https://github.com/jsakamoto/AntiVirusScanner/releases).

2. Enter commands in the PowerShell console, like this:

```powershell
PS> [Reflection.Assembly]::LoadFrom("c:\some\folder\AntiVirusScanner.dll")
PS> $scanner = New-Object AntiVirus.Scanner
PS> $scanner.ScanAndClean("c:\some\file\path.txt")
VirusNotFound
```

### F# Script

1. Donwload `AntiVirusScanner.dll` from [download page](https://github.com/jsakamoto/AntiVirusScanner/releases).
2. Write F# Script code like this, and run it.

```fsharp
// sample.fsx
#I "c:\some\folder"
#r "AntiVirusScanner.dll"
let scanner = new AntiVirus.Scanner()
scanner.ScanAndClean(@"c:\some\file\path.txt")
    |> printfn "%O" // console output is "VirusNotFound".
```

### VBScript

1. Donwload `AntiVirusScanner.dll` from [download page](https://github.com/jsakamoto/AntiVirusScanner/releases), and run `Install as a COM server.bat` as adminstrator.

2. Write VBScript code like this, and run it.

```vb
rem sample.vbs
Set scanner = CreateObject("AntiVirus.Scanner")
Set result = scanner.ScanAndClean("c:\some\file\path.txt")
WScript.Echo result 
' console output is  0, means "VirusNotFound". 1 is "VirusFound" (may be cleaned), 2 is "FileNotExist".
```

## System Requirements

- Windows OS (7 ~)
- .NET Framework (3.5 Client Profile ~) or .NET Core (2.0 ~) or .NET (5.0 ~)
- Any antivirus applications that support the ["IAttachmentExecute"](https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-iattachmentexecute) API.

### Known list

- "Microsoft Security Essentials" on Win7Pro (x64)
- "ESET NOD32 AntiVirus 4.0" on Win7Pro (x86)
- "Windows Defender" on Win10Pro (x64)


## Who virus-scan a file?

This library is a wrapper of an antivirus software product (such as "Microsoft Security Essentials") that you installed on your Windows OS.  
Therefore, this library does not work if you do not install any antivirus software products on your Windows OS.

## Why need this library?

Windows OS provides the standard API to call the antivirus software installed (Of course, the installed antivirus software has to support that API).

However, the Windows OS provided API exposes only COM Interface style, not "IDispatch".

So, using that API from any .NET languages and script languages is too tricky.

The reason why this library developed is to make it easier to use that Windows OS API from any .NET languages and any script languages.

## License

[Mozilla Public License Version 2.0](https://github.com/jsakamoto/AntiVirusScanner/blob/master/LICENSE)
