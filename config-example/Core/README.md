# xubot
## post build requirements

- __**Keys.json** - Used for all api tokens and keys used. This is absolutely necessary.__
- **API.json** - Used for `[>about` and `[>credits`. must be edited manually before execution *(loaded once)*.
- **Moods.xml** - Used for the "interaction" commands for having differnet messages relating to the mood value of the user. use the commands in "Mood.cs" to "initalize" this file.
- **Opinions.xml** - Used for `[>opinion`. must be edited manually before/during execution.
- **Pronouns.xml** - Used for the `[>pronouns` command. please use the command to "initalize" the file.
- **Trusted.xml** - Currently used as a pseudo replacement for owner requirement. one current use is `[>markov?flush`
- **Wiki.xml** - Used for `[>wiki`

- **code-handler/code-compiler** - Used for most of `[>interp`. must be compiled and located like this relative to the bot executable: `/code-handler/xubot-code-compiler.exe`

- A copy of "Roboto-Regular.ttf" is provided for text overlays and future text drawings