![logo](https://raw.githubusercontent.com/xubot-team/xubot/master/docs/xublogo.png)

### Build Status
`AppVeyor`: [![Build status](https://ci.appveyor.com/api/projects/status/vxb9wvryyppa1cc5?svg=true)](https://ci.appveyor.com/project/xubiod/xubot)

`Travis CI`: [![Build status](https://api.travis-ci.org/xubot-team/xubot.svg?branch=master)](https://travis-ci.org/xubot-team/xubot) (doesn't understand WPF at the moment)

## Building
When building xubot, you need to have a code handler! The official build uses an application I've made.

Xubot uses WebSocket4Net to use connectivity on Windows 7, where the bot has been built in.

The application needs to read inputs, specifically language and code to use. A pre-made application (the one the code references) will be provided in the future.

The binary also depends on certain files within its directory. These can be found in the [post build requirements](post-build-requirements) folder.

When forking for your own bot, ***follow the license.***

## Contributing
I rather add code myself, but some pull requests on readability or more efficient code will be accepted. New commands via pull request will more than likely be declined.

## License
The license used is the **MIT License**. More details [here.](LICENSE)
