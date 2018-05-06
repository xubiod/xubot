using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace xubot.src
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
            public async Task get(string article)
            {
                EmbedBuilder embedd = WikiTools.BuildEmbed(WikiTools.ReadArticle(article), WikiTools.ReadLastEdit(article), article);
                await ReplyAsync("", false, embedd.Build());
            }

            [Command("edit")]
            public async Task modify(string article, string content)
            {
                WikiTools.AddEditArticle(Context, article, content);
                await ReplyAsync("Article (hopefully) added.");
            }
        }
    }

    public class WikiTools
    {
        public static EmbedBuilder BuildEmbed(string article, string lasteditor, string name)
        {
            return new EmbedBuilder
            {
                Title = "Wiki",
                Color = Discord.Color.Gold,
                Description = @"Article: ""**" + name.ToLower() + @"**"". Was last edited by **" + lasteditor + "**",
                
                Footer = new EmbedFooterBuilder
                {
                    Text = "xubot :p"
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
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

        public static async void AddEditArticle(ICommandContext Context, string article, string content)
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
                        item.lasteditor.Value = Context.Message.Author.Username + "#" + Context.Message.Author.Discriminator;
                        xdoc.Save("Wiki.xml");
                    } catch (Exception exp)
                    {
                        await GeneralTools.CommHandler.BuildError(exp, Context);
                    }
                    
                    await Task.CompletedTask;
                }
            }

            if (!exist)
            {
                XElement element = new XElement("article");

                XAttribute name_att = new XAttribute("name", article);
                XAttribute editor_att = new XAttribute("lasteditor", Context.Message.Author);

                element.Add(name_att);
                element.Add(editor_att);
                element.SetValue(content);

                xdoc.Root.Add(element);
                xdoc.Save("Wiki.xml");

                await Task.CompletedTask;
            }
        }
    }
}
