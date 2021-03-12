![logo](https://raw.githubusercontent.com/xubiod/xubot/master/docs/xublogo.png)

### Project Badges
[![CodeFactor](https://www.codefactor.io/repository/github/xubiod/xubot/badge)](https://www.codefactor.io/repository/github/xubiod/xubot)
[![Build status](https://ci.appveyor.com/api/projects/status/1gwftwwou8k80gir?svg=true)](https://ci.appveyor.com/project/xubiod/xubot-appveyor)
![Custom badge](https://img.shields.io/endpoint?url=https%3A%2F%2Fxubiod.net%2Fbot-status%2Fbadge_endpoint.php)

![GitHub](https://img.shields.io/github/license/xubiod/xubot)
![GitHub top language](https://img.shields.io/github/languages/top/xubiod/xubot)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/xubiod/xubot)
![GitHub repo size](https://img.shields.io/github/repo-size/xubiod/xubot)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/xubiod/xubot)
![GitHub commit activity](https://img.shields.io/github/commit-activity/w/xubiod/xubot)

#### Joke Badges
[![forthebadge](https://forthebadge.com/images/badges/made-with-c-sharp.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/made-with-crayons.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/built-by-developers.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/built-by-codebabes.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/powered-by-electricity.svg)](https://forthebadge.com)

[![forthebadge](https://forthebadge.com/images/badges/contains-tasty-spaghetti-code.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/it-works-why.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/not-a-bug-a-feature.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/powered-by-black-magic.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/works-on-my-machine.svg)](https://forthebadge.com)

[![forthebadge](https://forthebadge.com/images/badges/uses-badges.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/reading-6th-grade-level.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/no-ragrets.svg)](https://forthebadge.com) 
[![forthebadge](https://forthebadge.com/images/badges/gluten-free.svg)](https://forthebadge.com) 
[![forthebadge](https://forthebadge.com/images/badges/does-not-contain-treenuts.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/60-percent-of-the-time-works-every-time.svg)](https://forthebadge.com)

[![forthebadge](https://forthebadge.com/images/badges/fuck-it-ship-it.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/you-didnt-ask-for-this.svg)](https://forthebadge.com)


## Building (.NET Core)
*Use xubot-core to build with .NET Core. You need .NET Core 3.1 on the target machine.*

In the project directory, run this to compile to DLLs for various platforms:
```
dotnet publish
```

You run it by running this with `xubot.dll`:
```
dotnet xubot.dll
```

If the publish provides it, you can also run the bot with the executable named `xubot` (i.e `xubot.exe`, `./xubot`). For Linux, make sure the execute rights are set on the file.

The binary depends on certain files within its directory. These can be found in the [config example](config-example) folder.

For a full runtime ID list, use [Microsoft's catalog.](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog)

**Confirmed working on:** Windows 10 x64, Ubuntu LTS, Debian (published as `linux-x64`)

## License ![GitHub](https://img.shields.io/github/license/xubiod/xubot)
The source code provided here on GitHub is licensed under **AGPLv3**. You can look at the license [here.](LICENSE)
