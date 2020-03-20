![logo](https://raw.githubusercontent.com/xubot-team/xubot/master/docs/xublogo.png)

### CodeFactor Grade
[![CodeFactor](https://www.codefactor.io/repository/github/xubot-team/xubot/badge)](https://www.codefactor.io/repository/github/xubot-team/xubot)

### Other Badges
[![forthebadge](https://forthebadge.com/images/badges/made-with-c-sharp.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/made-with-crayons.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/built-by-developers.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/built-by-codebabes.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/powered-by-electricity.svg)](https://forthebadge.com)

[![forthebadge](https://forthebadge.com/images/badges/uses-badges.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/reading-6th-grade-level.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/no-ragrets.svg)](https://forthebadge.com) 
[![forthebadge](https://forthebadge.com/images/badges/gluten-free.svg)](https://forthebadge.com) 
[![forthebadge](https://forthebadge.com/images/badges/does-not-contain-treenuts.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/60-percent-of-the-time-works-every-time.svg)](https://forthebadge.com)

[![forthebadge](https://forthebadge.com/images/badges/fuck-it-ship-it.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/you-didnt-ask-for-this.svg)](https://forthebadge.com)


## Building (.NET Core)
*Use xubot-core to build with .NET Core. You need .NET Core 2.1 on the target machine.*

In the project directory, run this to compile to DLLs for various platforms:
```
dotnet publish
```

You run it by running this with `xubot-core.dll`:
```
dotnet xubot-core.dll
```

The binary depends on certain files within its directory. These can be found in the [config example](config-example) folder.

For a full runtime ID list, use [Microsoft's catalog.](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog)

**Confirmed working on:** Windows 7 x64, Ubuntu

## Building (.NET Framework) (deprecated)
**Note: The .NET Framework version of xubot is no longer updated and supported.**

Xubot uses WebSocket4Net to use connectivity on Windows 7.

Xubot is currently 64bit only. You may change it when building, however, you may come across memory issues.

Xubot requires some applications for some functionality. These include [a code interpeter (for the interp command)](xubot-code-compiler), and [a launcher and updater (for the update command and updates in general)](xubot-launcher).

The binary also depends on certain files within its directory. These can be found in the [config example](config-example) folder.

## Contributing
To be rewritten.

## License
The license used is the **MIT License**. More details [here.](LICENSE)
