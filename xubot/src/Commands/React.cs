using System.Threading.Tasks;
using Discord.Commands;

namespace xubot.Commands
{
    public class React : ModuleBase
    {
        [Command("$"), RequireOwner]
        public async Task Speak(string echo)
        {
            await Context.Message.DeleteAsync(new Discord.RequestOptions());
            await ReplyAsync(echo);
        }
    }
}
