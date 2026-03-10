![DL Count](https://img.shields.io/github/downloads/turtle-insect/FF9/total.svg)

# FF9
A save editor for the Nintendo Switch version of Final Fantasy IX.

## Fork notice
This repository is a fork of the original `FF9` project by [turtle-insect](https://github.com/turtle-insect).

- Original project: https://github.com/turtle-insect/FF9

## Solution overview
- Solution file: `FF9.sln`
- Projects:
  - `FF9/FF9.csproj` — WPF desktop application (`net10.0-windows`)
  - `FF9.Tests/FF9.Tests.csproj` — MSTest test project (`net10.0-windows`)
- Solution configurations: `Debug|Any CPU`, `Release|Any CPU`

## Tech stack and dependencies
- SDK-style projects using `Microsoft.NET.Sdk`
- UI: WPF (`UseWPF=true`)
- Main package dependency: `Newtonsoft.Json` (`13.0.3`)
- Test packages:
  - `MSTest` (`4.0.1`)
  - `coverlet.collector` (`6.0.4`)

## Project content files
The app and tests rely on data files copied to output at build time:
- `info/ability.txt`
- `info/card.txt`
- `info/item.txt`

## Official links
- Portal: https://www.jp.square-enix.com/game/detail/ff9/
- Nintendo eShop: https://store-jp.nintendo.com/list/software/70010000010543.html

## Runtime requirements
- Windows 10 or later
- .NET 10 (Windows)
- Save data export/import access

## Build requirements
- Windows 10 (64-bit) or later
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/ja/vs/)

## Basic workflow
1. Export save data.
2. Open save data.
3. Edit save data.
4. Save modified data.
5. Import save data.
