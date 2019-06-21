# xubot
## post build requirements

- **API.json** - used for `[>about` and `[>credits`. must be edited manually before execution *(loaded once)*.
- **Moods.xml** - used for the "interaction" commands for having differnet messages relating to the mood value of the user. use the commands in "Mood.cs" to "initalize" this file.
- **Opinions.xml** - used for `[>opinion`. must be edited manually before/during execution.
- **PerServTrigg.xml** - used for the per server triggers/settings, like sending a message when the bot has connected "onwake". use  `[>servertriggers` to "initalize" the file.
- **Pronouns.xml** - used for the `[>pronouns` command. please use the command to "initalize" the file.
- **Keys.json** - used for all api tokens and keys used.
- **SSHQuickConnect.xml** - used with `[>ssh qc [NICK]`. this is do prevent writing system passwords into chat.
- **Trusted.xml** - currently used as a pseudo replacement for owner requirement. one current use is `[>markov?flush`
- **Wiki.xml** - used for `[>wiki`

- **code-handler/code-compiler** - used for most of `[>interp`. must be compiled and located like this relative to the bot executable: `/code-handler/xubot-code-compiler.exe`
