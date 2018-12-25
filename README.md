# Anti Virus Scanner for .NET (and COM) [![NuGet Package](https://img.shields.io/nuget/v/AntiVirusScanner.svg)](https://www.nuget.org/packages/AntiVirusScanner/)

## Project Description / 概要

This library allows you to virus-scanning a file by any .NET languages (C#, F#, VB...), PowerShell, F# Script, WSH (JScript, VBScript), and any COM IDispatch client.

 このライブラリは任意の.NET言語 (C#, F#, VB), PowerShell, F# Script, 及び WSH (JScript, VBScript) 等の任意の COM IDispatch クライアントから、ファイルのウィルススキャンを可能にします。

## Who virus-scan a file? / 誰がウィルススキャンを行うのか?

This library is a wrapper of anti virus software product (such as "Microsoft Security Essentials") which you installed on your Windows OS.  
Therefore, this library does not work if you do not install any anti virus software produsts on your Windows OS.

このライブラリは、あなたの Windows OS にあなたがインストールしたウィルス対策ソフトウェア（"Microsoft Security Essentials" のような）のラッパーに過ぎません。  
実際にウィルススキャンを行うのは、そのウィルス対策ソフトウェア製品です。  
そのため、何らかのウィルス対策ソフトウェア製品があなたの Windows OS にインストールされていない場合は、このライブラリは機能しません。

## Why need this library? / なぜこのライブラリが必要なのか?

Windows OS provides the common API to calling the anti virus software which is installed (Of course, the anti virus software required support the API).  
However, the API to calling the anti virus software provide only COM Interface style, not supported IDispatch.  
So, calling this API is too difficult from any .NET languages and script languages.

Windows OS は、インストールされているウィルス対策ソフトウェアを呼び出すための共通の API を提供しています（もちろん、インストールされているウィルス対策ソフトウェアがこの API に対応している必要があります）。  
しかしこの API は COM インターフェースとしてのみ公開されており、COM の IDispatch インターフェースにすら対応していません。  
そのため、各種 .NET 言語やスクリプト言語から呼び出すのが極めて困難です。  
そこで、.NET のアセンブリとして、この API のラッパーを作ることで、各種 .NET 言語やスクリプト言語からこの API を容易に利用可能にしました。

## Quick Start / クイックスタート

C#

```csharp
// You can download and add reference your VS project from "NuGet"!
// https://www.nuget.org/packages/AntiVirusScanner/
var scanner = new AntiVirus.Scanner();
var result = scanner.ScanAndClean(@"c:\some\file\path.txt");
Console.WriteLine(result); // console output is "VirusNotFound".
```

Windows PowerShell

```powershell
PS> [Reflection.Assembly]::LoadFrom("c:\some\folder\AntiVirusScanner.dll")
PS> $scanner = New-Object AntiVirus.Scanner
PS> $scanner.ScanAndClean("c:\some\file\path.txt")
VirusNotFound
```

F# Script

```fsharp
#I "c:\some\folder"
#r "AntiVirusScanner.dll"
let scanner = new AntiVirus.Scanner()
scanner.ScanAndClean(@"c:\some\file\path.txt")
    |> printfn "%O" // console output is "VirusNotFound".
```

VBScript

```vb
Set scanner = CreateObject("AntiVirus.Scanner")
Set result = scanner.ScanAndClean("c:\some\file\path.txt")
WScript.Echo result 
' console output is  0, means "VirusNotFound". 1 is "VirusFound" (may be cleaned), 2 is "FileNotExist".
```

## System Requirements / システム要件

- Windows OS (7 ~)
- .NET Framework (3.5 Client Profile ~) - or - .NET Core (2.0 ~)
- Any anti virus application which supported IAttachmentExecute API. / IAttachmentExecute API に対応したウィルス対策ソフトウェア

### Known list / 既知のリスト

- "Microsoft Security Essentials" on Win7Pro (x64)
- "ESET NOD32 AntiVirus 4.0" on Win7Pro (x86)
- "Windows Defender" on Win10Pro (x64)


## License

[Mozilla Public License Version 2.0](https://github.com/jsakamoto/AntiVirusScanner/blob/master/LICENSE)