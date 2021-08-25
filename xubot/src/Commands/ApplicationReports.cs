using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace xubot.Commands
{
    public class ApplicationReports : ModuleBase
    {
        [Group("up-time"), Summary("Gets application and/or connection up-time.")]
        public class UpTime : ModuleBase
        {
            private readonly TimeSpan _appUpTime = DateTime.Now - Program.AppStart;
            private readonly TimeSpan _conUpTime = DateTime.Now - Program.ConnectStart;

            private readonly TimeSpan _appToRedCli = Program.stepTimes[0] - Program.AppStart;
            private readonly TimeSpan _appToDis = Program.stepTimes[2] - Program.AppStart;

            private readonly TimeSpan _redCliToSub = Program.stepTimes[1] - Program.stepTimes[0];
            private readonly TimeSpan _subToDiscord = Program.stepTimes[2] - Program.stepTimes[1];

            [Command, Summary("Gets application up-time.")]
            public async Task Basic()
            {
                await ReplyAsync($"Up-time (from application start) is **{_appUpTime.Days} days, {_appUpTime.Hours} hours, {_appUpTime.Minutes} minutes, {_appUpTime.Seconds} seconds.**");
            }

            [Command("report", RunMode = RunMode.Async), Summary("Gets application and connection up-times.")]
            public async Task Report()
            {
                await BuildReport(Context, new List<EmbedFieldBuilder>
                    {
                    new()
                    {
                        Name = "Broad report",
                        Value = $"App up-time: **{_appUpTime}**\n" +
                                $"Connection up-time: **{_conUpTime}**\n\n" ,
                        IsInline = true
                    },
                    new()
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

                //await ReplyAsync("Up-time (from application start) is **{up-time.Days } days, {up-time.Hours } hours, {up-time.Minutes } minutes, {up-time.Seconds } seconds.**");
            }

            [Command("report?human", RunMode = RunMode.Async), Summary("Gets application and connection up-times in a more friendlier layout.")]
            public async Task ReportHuman()
            {
                await BuildReport(Context, new List<EmbedFieldBuilder>
                    {
                    new()
                    {
                        Name = "Broad report",
                        Value = $"App up-time: **{_appUpTime.Days }d, {_appUpTime.Hours }h, {_appUpTime.Minutes }min, {_appUpTime.Seconds }s, {_appUpTime.Milliseconds }ms**\n" +
                                $"Connection up-time: **{_conUpTime.Days }d, {_conUpTime.Hours }h, {_conUpTime.Minutes }min, {_conUpTime.Seconds }s, {_conUpTime.Milliseconds }ms**\n\n" ,
                        IsInline = true
                    },
                    new()
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

                //await ReplyAsync("Up-time (from application start) is **{up-time.Days } days, {up-time.Hours } hours, {up-time.Minutes } minutes, {up-time.Seconds } seconds.**");
            }

            [Command("report?ticks", RunMode = RunMode.Async), Summary("Gets application and connection up-times into C# ticks.")]
            public async Task ReportTicks()
            {
                await BuildReport(Context,
                    new List<EmbedFieldBuilder>
                    {
                        new()
                        {
                            Name = "Broad report",
                            Value = $"App up-time: **{_appUpTime.Ticks } ticks**\n" +
                                    $"Connection up-time: **{_conUpTime.Ticks } ticks**\n\n" ,
                            IsInline = true
                        },
                        new()
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

                //await ReplyAsync("Up-time (from application start) is **{up-time.Days } days, {up-time.Hours } hours, {up-time.Minutes } minutes, {up-time.Seconds } seconds.**");
            }

            [Command("report?doom-tics", RunMode = RunMode.Async), Summary("Gets application and connection up-times into DOOM realtics (1/35ths of a second).")]
            public async Task ReportDoomTics()
            {
                float tic = 35;

                await BuildReport(Context,
                    new List<EmbedFieldBuilder>
                    {
                        new()
                        {
                            Name = "Broad report",
                            Value = $"App up-time: **{System.Math.Round(_appUpTime.TotalSeconds / tic*100)/100 } DOOM real-tics**\n" +
                                    $"Connection up-time: **{System.Math.Round(_conUpTime.TotalSeconds / tic*100)/ 100 } DOOM real-tics**\n\n" ,
                            IsInline = true
                        },
                        new()
                        {
                            Name = "Specific connections report",
                            Value = $"Connection to Reddit: **{System.Math.Round(_appToRedCli.TotalSeconds / tic*100)/100 } DOOM real-tics**\n" +
                                    $"Connection to Discord: **{System.Math.Round(_appToDis.TotalSeconds / tic*100)/100 } DOOM real-tics**\n" +
                                    $"Reddit Connection to Default Sub: **{System.Math.Round(_redCliToSub.TotalSeconds / tic*100)/100 } DOOM real-tics**\n" +
                                    $"Default Sub to Discord: **{System.Math.Round(_subToDiscord.TotalSeconds / tic*100)/100 } DOOM real-tics**\n\n",
                            IsInline = true
                        }
                    }
                );

                //await ReplyAsync("Up-time (from application start) is **{up-time.Days } days, {up-time.Hours } hours, {up-time.Minutes } minutes, {up-time.Seconds } seconds.**");
            }

            [Command("report?no-span", RunMode = RunMode.Async), Summary("Gets application and connection starting time.")]
            public async Task ReportDate()
            {
                await BuildReport(Context,
                    new List<EmbedFieldBuilder>
                    {
                        new()
                        {
                            Name = "Broad report",
                            Value = $"App start time: **{Program.AppStart }**\n" +
                                    $"Connection time: **{Program.ConnectStart }**\n\n" ,
                            IsInline = true
                        },
                        new()
                        {
                            Name = "Specific connections report",
                            Value = $"Connection to Reddit: **{Program.stepTimes[0] }**\n" +
                                    $"Connection to Default Sub: **{Program.stepTimes[1] }**\n" +
                                    $"Connection to Discord: **{Program.stepTimes[2] }**\n",
                            IsInline = true
                        }
                    });

                //await ReplyAsync("Up-time (from application start) is **{up-time.Days } days, {up-time.Hours } hours, {up-time.Minutes } minutes, {up-time.Seconds } seconds.**");
            }
        }

        [Group("memory"), Summary("Gets memory stats for xubot.")]
        public class Memory : ModuleBase
        {
            [Command, Summary("Gets working set memory for xubot in MB.")]
            public async Task BasicMemory()
            {
                Process currentProcess = Process.GetCurrentProcess();
                long used = currentProcess.WorkingSet64;

                await ReplyAsync($"Memory used (MB): **{used / 1000000 }**");
            }

            [Command("report"), Summary("Gets working set memory, virtual memory, paged memory, and their peaks for xubot.")]
            public async Task Report()
            {
                Process currentProcess = Process.GetCurrentProcess();
                double ws = (double)currentProcess.WorkingSet64 / 1000000;
                double pws = (double)currentProcess.PeakWorkingSet64 / 1000000;

                double vms = (double)currentProcess.VirtualMemorySize64 / 1000000;
                double pvms = (double)currentProcess.PeakVirtualMemorySize64 / 1000000;

                double pm = (double)currentProcess.PagedMemorySize64 / 1000000;
                double ppm = (double)currentProcess.PeakPagedMemorySize64 / 1000000;

                await BuildReport(Context, new List<EmbedFieldBuilder>
                    {
                    new()
                    {
                        Name = "Working Set",
                        Value = $"Used (MB): **{ws }** | Peak (MB): **{pws }**\n\n",
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Virtual Memory",
                        Value = $"Virtual (MB): **{vms }** | Virtual Peak (MB): **{pvms }**\n\n",
                        IsInline = true
                    },
                    new()
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
            EmbedBuilder embed = Util.Embed.GetDefaultEmbed(context, "Up-time Report", $"Report from {DateTime.Now}", Color.Red);
            embed.Fields = fields;

            await context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
