using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace xubot.src.Commands.Global
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
				EmbedBuilder embedd = WikiTools.BuildEmbed(WikiTools.ReadArticle(article), WikiTools.ReadLastEdit(article), article);
				await ReplyAsync("", false, embedd.Build());
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
