using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot.src.Commands
{
    public class ApplicationReports : ModuleBase
    {
        [Group("uptime"), Summary("Gets application and/or connection uptime.")]
        public class Uptime : ModuleBase
        {
            readonly TimeSpan _appUptime = DateTime.Now - Program.AppStart;
            readonly TimeSpan _conUptime = DateTime.Now - Program.ConnectStart;

            private readonly TimeSpan _appToRedCli = Program.stepTimes[0] - Program.AppStart;
            private readonly TimeSpan _appToDis = Program.stepTimes[2] - Program.AppStart;

            private readonly TimeSpan _redCliToSub = Program.stepTimes[1] - Program.stepTimes[0];
            private readonly TimeSpan _subToDiscord = Program.stepTimes[2] - Program.stepTimes[1];

            [Command, Summary("Gets application uptime.")]
            public async Task Basic()
            {
                await ReplyAsync($"Uptime (from application start) is **{_appUptime.Days} days, {_appUptime.Hours} hours, {_appUptime.Minutes} minutes, {_appUptime.Seconds} seconds.**");
            }

            [Command("report", RunMode = RunMode.Async), Summary("Gets application and connection uptimes.")]
            public async Task Report()
            {
                await BuildReport(Context, new List<EmbedFieldBuilder>()
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Broad report",
                        Value = $"App uptime: **{_appUptime}**\n" +
                                $"Connection uptime: **{_conUptime}**\n\n" ,
                        IsInline = true
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Specific connections report",
                        Value = $"Connection to Reddit: **{_appToRedCli}**\n" +
                                $"Connection to Discord: **{_appToDis}**\n\n" +
                                $"Reddit Connection to Default Sub: **{_redCliToSub}**\n" +
                                $"Default Sub to Discord: **{_subToDiscord}**\n\n",
                        IsInline = true
                    }
                }
                );

                //await ReplyAsync("Uptime (from application start) is **{uptime.Days } days, {uptime.Hours } hours, {uptime.Minutes } minutes, {uptime.Seconds } seconds.**");
            }

            [Command("report?human", RunMode = RunMode.Async), Summary("Gets application and connection uptimes in a more friendlier layout.")]
            public async Task ReportHuman()
            {
                await BuildReport(Context, new List<EmbedFieldBuilder>()
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Broad report",
                        Value = $"App uptime: **{_appUptime.Days }d, {_appUptime.Hours }h, {_appUptime.Minutes }min, {_appUptime.Seconds }s, {_appUptime.Milliseconds }ms**\n" +
                                $"Connection uptime: **{_conUptime.Days }d, {_conUptime.Hours }h, {_conUptime.Minutes }min, {_conUptime.Seconds }s, {_conUptime.Milliseconds }ms**\n\n" ,
                        IsInline = true
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Specific connections report",
                        Value = $"Connection to Reddit: **{_appToRedCli.Days }d, {_appToRedCli.Hours }h, {_appToRedCli.Minutes }min, {_appToRedCli.Seconds }s, {_appToRedCli.Milliseconds }ms**\n" +
                                $"Connection to Discord: **{_appToDis.Days }d, {_appToDis.Hours }h, {_appToDis.Minutes }min, {_appToDis.Seconds }s, {_appToDis.Milliseconds }ms**\n" +
                                $"Reddit Connection to Default Sub: **{_redCliToSub.Days }d, {_redCliToSub.Hours }h, {_redCliToSub.Minutes }min, {_redCliToSub.Seconds }s, {_redCliToSub.Milliseconds }ms**\n" +
                                $"Default Sub to Discord: **{_subToDiscord.Days }d, {_subToDiscord.Hours }h, {_subToDiscord.Minutes }min, {_subToDiscord.Seconds }s, {_subToDiscord.Milliseconds }ms**\n\n",
                        IsInline = true
                    }
                }
                );

                //await ReplyAsync("Uptime (from application start) is **{uptime.Days } days, {uptime.Hours } hours, {uptime.Minutes } minutes, {uptime.Seconds } seconds.**");
            }

            [Command("report?ticks", RunMode = RunMode.Async), Summary("Gets application and connection uptimes into C# ticks.")]
            public async Task ReportTicks()
            {
                await BuildReport(Context,
                    new List<EmbedFieldBuilder>()
                    {
                        new EmbedFieldBuilder
                        {
                            Name = "Broad report",
                            Value = $"App uptime: **{_appUptime.Ticks } ticks**\n" +
                                    $"Connection uptime: **{_conUptime.Ticks } ticks**\n\n" ,
                            IsInline = true
                        },
                        new EmbedFieldBuilder
                        {
                            Name = "Specific connections report",
                            Value = $"Connection to Reddit: **{_appToRedCli.Ticks } ticks**\n" +
                                    $"Connection to Discord: **{_appToDis.Ticks } ticks**\n" +
                                    $"Reddit Connection to Default Sub: **{_redCliToSub.Ticks } ticks**\n" +
                                    $"Default Sub to Discord: **{_subToDiscord.Ticks } ticks**\n\n",
                            IsInline = true
                        }
                    }
                );

                //await ReplyAsync("Uptime (from application start) is **{uptime.Days } days, {uptime.Hours } hours, {uptime.Minutes } minutes, {uptime.Seconds } seconds.**");
            }

            [Command("report?doom-tics", RunMode = RunMode.Async), Summary("Gets application and connection uptimes into DOOM realtics (1/35ths of a second).")]
            public async Task ReportDoomTics()
            {
                float tic = 35;

                await BuildReport(Context,
                    new List<EmbedFieldBuilder>()
                    {
                        new EmbedFieldBuilder
                        {
                            Name = "Broad report",
                            Value = $"App uptime: **{(System.Math.Round((_appUptime.TotalSeconds / tic)*100)/100) } DOOM realtics**\n" +
                                    $"Connection uptime: **{(System.Math.Round((_conUptime.TotalSeconds / tic)*100)/ 100) } DOOM realtics**\n\n" ,
                            IsInline = true
                        },
                        new EmbedFieldBuilder
                        {
                            Name = "Specific connections report",
                            Value = $"Connection to Reddit: **{(System.Math.Round((_appToRedCli.TotalSeconds / tic)*100)/100) } DOOM realtics**\n" +
                                    $"Connection to Discord: **{(System.Math.Round((_appToDis.TotalSeconds / tic)*100)/100) } DOOM realtics**\n" +
                                    $"Reddit Connection to Default Sub: **{(System.Math.Round((_redCliToSub.TotalSeconds / tic)*100)/100) } DOOM realtics**\n" +
                                    $"Default Sub to Discord: **{(System.Math.Round((_subToDiscord.TotalSeconds / tic)*100)/100) } DOOM realtics**\n\n",
                            IsInline = true
                        }
                    }
                );

                //await ReplyAsync("Uptime (from application start) is **{uptime.Days } days, {uptime.Hours } hours, {uptime.Minutes } minutes, {uptime.Seconds } seconds.**");
            }

            [Command("report?no-span", RunMode = RunMode.Async), Summary("Gets application and connection starting time.")]
            public async Task ReportDate()
            {
                await BuildReport(Context,
                    new List<EmbedFieldBuilder>()
                    {
                        new EmbedFieldBuilder
                        {
                            Name = "Broad report",
                            Value = $"App start time: **{Program.AppStart }**\n" +
                                    $"Connection time: **{Program.ConnectStart }**\n\n" ,
                            IsInline = true
                        },
                        new EmbedFieldBuilder
                        {
                            Name = "Specific connections report",
                            Value = $"Connection to Reddit: **{Program.stepTimes[0] }**\n" +
                                    $"Connection to Default Sub: **{Program.stepTimes[1] }**\n" +
                                    $"Connection to Discord: **{Program.stepTimes[2] }**\n",
                            IsInline = true
                        }
                    });

                //await ReplyAsync("Uptime (from application start) is **{uptime.Days } days, {uptime.Hours } hours, {uptime.Minutes } minutes, {uptime.Seconds } seconds.**");
            }
        }

        [Group("memory"), Summary("Gets memory stats for xubot.")]
        public class Memory : ModuleBase
        {
            [Command, Summary("Gets working set memory for xubot in MB.")]
            public async Task BasicMemory()
            {
                Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                long used = currentProcess.WorkingSet64;

                await ReplyAsync($"Memory used (MB): **{(used / 1000000) }**");
            }

            [Command("report"), Summary("Gets working set memory, virtual memory, paged memory, and their peaks for xubot.")]
            public async Task Report()
            {
                Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                double ws = currentProcess.WorkingSet64 / 1000000;
                double pws = currentProcess.PeakWorkingSet64 / 1000000;

                double vms = currentProcess.VirtualMemorySize64 / 1000000;
                double pvms = currentProcess.PeakVirtualMemorySize64 / 1000000;

                double pm = currentProcess.PagedMemorySize64 / 1000000;
                double ppm = currentProcess.PeakPagedMemorySize64 / 1000000;

                await BuildReport(Context, new List<EmbedFieldBuilder>()
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Working Set",
                        Value = $"Used (MB): **{ws }** | Peak (MB): **{pws }**\n\n",
                        IsInline = true
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Virtual Memory",
                        Value = $"Virtual (MB): **{vms }** | Virtual Peak (MB): **{pvms }**\n\n",
                        IsInline = true
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Paged Memory",
                        Value = $"Paged (MB): **{pm }** | Paged Peak (MB): **{ppm }**\n\n",
                        IsInline = true
                    }
                }
                );
            }
        }

        public static async Task BuildReport(ICommandContext context, List<EmbedFieldBuilder> fields)
        {
            EmbedBuilder embed = Util.Embed.GetDefaultEmbed(context, "Uptime Report", $"Report from {DateTime.Now}", Discord.Color.Red);
            embed.Fields = fields;

            await context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
