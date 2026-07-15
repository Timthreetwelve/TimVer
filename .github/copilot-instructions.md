# Copilot instructions for TimVer

## Build, test, and lint

- **SDK/runtime target:** .NET SDK `10.0.200` (`global.json`), WPF on Windows (`net10.0-windows`).
- **Restore:** `dotnet restore TimVer.slnx`
- **Build (solution):** `dotnet build TimVer.slnx -c Debug`
- **Build (single project):** `dotnet build TimVer\TimVer.csproj -c Debug`
- **Run locally:** `dotnet run --project TimVer\TimVer.csproj`
- **Run hidden/history mode (command-line path):** `dotnet run --project TimVer\TimVer.csproj -- --hide`
- **Tests:** there is currently no automated test project in this repository.
- **Linting/analyzers:** there is no separate lint script; code style/analyzer enforcement happens during `dotnet build` (`AnalysisLevel=latest-recommended`, `EnforceCodeStyleInBuild=true` in `TimVer.csproj`).

## High-level architecture

- This is a **single-project WPF desktop app** (`TimVer/TimVer.csproj`) with MVVM.
- `App.xaml` defines shared Material Design theme resources and the DataTemplate mapping from ViewModels to Views. `MainWindow` sets `DataContext = new NavigationViewModel()`.
- Startup flow is concentrated in `App.OnStartup` and `MainWindowHelpers.TimVerStartUp()`:
  - initialize settings (`ConfigHelpers.InitializeSettings`)
  - configure logging (`NLogHelpers.NLogConfig`)
  - select/load localization resources (`Languages/Strings.*.xaml`)
  - process command-line options (`--hide` / `-h`) and optionally update build history before exiting
  - apply UI settings (theme, accent color, scaling, window position)
- Data retrieval is mostly helper-driven (`Helpers/*`): Windows/registry/WMI/environment/disk/video info helpers populate ViewModel collections/dictionaries.
- Runtime state is centralized in static settings objects:
  - persisted settings: `ConfigManager<UserSettings>.Setting` (`usersettings.json` in app directory)
  - temporary state: `ConfigManager<TempSettings>.Setting`
  - `SettingChange.UserSettingChanged` is the side-effect hub for setting updates (theme switch, log level, restart on language change, refreshing drive/viewmodel caches, etc.).
- Logging is centralized through NLog and a global static logger (`_log`) from `NLogHelpers`; log files are written to `%TEMP%\T_K\...`.

## Key repository conventions

- **Localization source of truth:** `TimVer/Languages/Strings.en-US.xaml` is the authoritative key set. Other `Strings.<culture>.xaml` files must stay key-compatible with it.
- **Localization change log is required:** when changing `Strings.en-US.xaml`, add/update the timestamped change-log comments at the bottom using existing `A |`, `U |`, `D |` format.
- **Preserve localization structure exactly:** if a string uses `xml:space="preserve"` and/or encoded line breaks (`&#x0a;`), keep those semantics in translated files.
- **MVVM Toolkit pattern:** ViewModels and settings classes use CommunityToolkit attributes (`[ObservableProperty]`, `[RelayCommand]`) with partial classes; follow the existing generated-property style.
- **Global usings are intentional:** many files rely on `GlobalUsings.cs`, including `using static TimVer.Helpers.NLogHelpers` and `using static TimVer.Helpers.ResourceHelpers`; avoid adding redundant per-file imports unless required (for example, `Octokit` is intentionally local in `GitHubHelpers.cs`).
- **User-facing text should come from resources:** prefer `GetStringResource(...)` keys over inline UI strings so language files remain the single localization surface.
