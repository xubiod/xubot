using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using GitHub;

namespace xubot
{
    public class GitHub : ModuleBase
    {
        public static GitHub xuGit = new GitHub();
        public static GitHubClient xuGitClient = new GitHubClient(new OAuth2Token(Program.keys.github.ToString()));
        public static GitRepository xuRepo;
        public static GitRepository xuRepo_Comm;
        public static GitCommit xuCommit_Comm;
        public static GitCommit[] xuCommitArr_Comm;

        public async void GetGitHubUpdatesAsync()
        {
            xuRepo = await xuGitClient.GetRepositoryAsync("xubot-team", "xubot");
        }

        [Group("github")]
        public class gitCommands : ModuleBase
        {
            [Command("repo")]
            public async Task repoInfo(string user, string repo)
            {
                xuRepo_Comm = await xuGitClient.GetRepositoryAsync(user, repo);
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Information about: " + user + "/" + repo,
                    Color = Discord.Color.Red,
                    Description = "GitHub repo details",
                    ThumbnailUrl = Context.Guild.IconUrl,

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Created (UTC)",
                                Value = "**" + xuRepo_Comm.CreatedAt.ToUniversalTime() + "**\n",
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Is Fork?",
                                Value = "**" + xuRepo_Comm.Fork + "**\n",
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Url",
                                Value = xuRepo_Comm.HtmlUrl + "\n",
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Latest Push (UTC)",
                                Value = "**" + xuRepo_Comm.PushedAt.ToUniversalTime() + "**\n",
                                IsInline = false
                            }
                        }
                };

                await ReplyAsync("", false, embedd);
            }
            
            [Command("commit")]
            public async Task commitInfo(string user, string repo, string sha)
            {
                xuCommit_Comm = await xuGitClient.GetCommitAsync(user, repo, sha);
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Information about: " + user + "/" + repo + " commit:" + sha,
                    Color = Discord.Color.Red,
                    Description = "GitHub commit details",
                    ThumbnailUrl = Context.Guild.IconUrl,

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Author Name",
                                Value = "**" + xuCommit_Comm.Author.Name + "**\n",
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Commited by",
                                Value = "**" + xuCommit_Comm.Committer.Name + "**\n",
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Message",
                                Value = xuCommit_Comm.Message + "\n",
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Tree Url",
                                Value = "**" + xuCommit_Comm.Tree.Url + "**\n",
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "SHA",
                                Value = "**" + xuCommit_Comm.Sha + "**\n",
                                IsInline = false
                            }
                        }
                };

                await ReplyAsync("", false, embedd);
            }

            [Command("repo-latest-commit"), Alias("rlc")]
            public async Task repoCommInfo(string user, string repo)
            {
                xuCommitArr_Comm = await xuGitClient.GetRepositoryCommitsAsync(user, repo);
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Information about: " + user + "/" + repo,
                    Color = Discord.Color.Red,
                    Description = "GitHub repo latest commit details",
                    ThumbnailUrl = Context.Guild.IconUrl,

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Latest Commit SHA",
                                Value = xuCommitArr_Comm.First<GitCommit>().Sha + "\n",
                                IsInline = false
                            }
                        }
                };

                await ReplyAsync("", false, embedd);
            }

        }
    }
}
