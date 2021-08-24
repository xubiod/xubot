using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using Discord;
using Discord.Commands;

namespace xubot.Commands.Global
{
	public class WikiTools
	{
		public static EmbedBuilder BuildEmbed(string article, string lasteditor, string name)
		{
			return new EmbedBuilder
			{
				Title = "Wiki",
				Color = Color.Gold,
				Description = @"Article: ""**" + name.ToLower() + @"**"". Was last edited by **" + lasteditor + "**",

				Footer = new EmbedFooterBuilder
				{
					Text = Util.Globals.EmbedFooter
				},
				Timestamp = DateTime.UtcNow,
				Fields = new List<EmbedFieldBuilder>()
						{
							new()
							{
								Name = name.ToLower(),
								Value = article,
								IsInline = false
							}
						}
			};
		}

		public static string ReadArticle(string article)
		{
			var xdoc = XDocument.Load("Wiki.xml");

			var items = from i in xdoc.Descendants("article")
						select new
						{
							name = (string)i.Attribute("name"),
							lasteditor = (string)i.Attribute("lasteditor"),
							i.Value
						};

			foreach (var item in items)
			{
				if (item.name.ToLower() == article.ToLower())
				{
					return item.Value;
				}
			}

			return "No article exists! Create one!";
		}
		public static string ReadLastEdit(string article)
		{
			var xdoc = XDocument.Load("Wiki.xml");

			var items = from i in xdoc.Descendants("article")
						select new
						{
							name = (string)i.Attribute("name"),
							lasteditor = (string)i.Attribute("lasteditor"),
							i.Value
						};

			foreach (var item in items)
			{
				if (item.name.ToLower() == article.ToLower())
				{
					return item.lasteditor;
				}
			}

			return "noone";
		}

		public static async void AddEditArticle(ICommandContext context, string article, string content)
		{
			bool exist = false;
			var xdoc = XDocument.Load("Wiki.xml");

			var items = from i in xdoc.Descendants("article")
						select new
						{
							name = i.Attribute("name"),
							lasteditor = i.Attribute("lasteditor"),
							content = i.Value
						};

			foreach (var item in items)
			{
				if (((string)item.name).ToLower() == article.ToLower())
				{
					exist = true;
					try
					{
						xdoc.XPathSelectElement("//article[@name='" + article.ToLower() + "']").Value = content;
						item.lasteditor.Value = context.Message.Author.Username + "#" + context.Message.Author.Discriminator;
						xdoc.Save("Wiki.xml");
					}
					catch (Exception exp)
					{
						await Util.Error.BuildError(exp, context);
					}

					await Task.CompletedTask;
				}
			}

			if (!exist)
			{
				XElement element = new XElement("article");

				XAttribute nameAtt = new XAttribute("name", article);
				XAttribute editorAtt = new XAttribute("lasteditor", context.Message.Author);

				element.Add(nameAtt);
				element.Add(editorAtt);
				element.SetValue(content);

				xdoc.Root.Add(element);
				xdoc.Save("Wiki.xml");

				await Task.CompletedTask;
			}
		}
	}
}
