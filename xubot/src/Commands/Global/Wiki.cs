using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace xubot.Commands.Global
{
	/*
	<article name="[article name]" lasteditor="[id]">
		Article content.
	</article>
	*/

	public class Wiki : ModuleBase
	{
		[Group("wiki")]
		public class WikiCmd : ModuleBase
		{
			[Command]
			public async Task Get(string article)
			{
				EmbedBuilder embed = WikiTools.BuildEmbed(WikiTools.ReadArticle(article), WikiTools.ReadLastEdit(article), article);
				await ReplyAsync("", false, embed.Build());
			}

			[Command("edit")]
			public async Task Edit(string article, string content)
			{
				WikiTools.AddEditArticle(Context, article, content);
				await ReplyAsync("Article (hopefully) added.");
			}
		}
	}
}
