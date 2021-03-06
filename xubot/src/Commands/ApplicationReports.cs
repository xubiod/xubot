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
            TimeSpan app_uptime = DateTime.Now - Program.appStart;
            TimeSpan con_uptime = DateTime.Now - Program.connectStart;

            TimeSpan app_to_red_cli = Program.stepTimes[0] - Program.appStart;
            TimeSpan app_to_dis = Program.stepTimes[2] - Program.appStart;

            TimeSpan red_cli_to_sub = Program.stepTimes[1] - Program.stepTimes[0];
            TimeSpan sub_to_discord = Program.stepTimes[2] - Program.stepTimes[1];

            [Command, Summary("Gets application uptime.")]
            public async Task Basic()
            {
                await ReplyAsync($"Uptime (from application start) is **{app_uptime.Days} days, {app_uptime.Hours} hours, {app_uptime.Minutes} minutes, {app_uptime.Seconds} seconds.**");
            }

            [Command("report", RunMode = RunMode.Async), Summary("Gets application and connection uptimes.")]
            public async Task Report()
            {
                await BuildReport(Context, new List<EmbedFieldBuilder>()
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Broad report",
                        Value = $"App uptime: **{app_uptime}**\n" +
                                $"Connection uptime: **{con_uptime}**\n\n" ,
                        IsInline = true
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Specific connections report",
                        Value = $"Connection to Reddit: **{app_to_red_cli}**\n" +
                                $"Connection to Discord: **{app_to_dis}**\n\n" +
                                $"Reddit Connection to Default Sub: **{red_cli_to_sub}**\n" +
                                $"Default Sub to Discord: **{sub_to_discord}**\n\n",
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
                        Value = $"App uptime: **{app_uptime.Days }d, {app_uptime.Hours }h, {app_uptime.Minutes }min, {app_uptime.Seconds }s, {app_uptime.Milliseconds }ms**\n" +
                                $"Connection uptime: **{con_uptime.Days }d, {con_uptime.Hours }h, {con_uptime.Minutes }min, {con_uptime.Seconds }s, {con_uptime.Milliseconds }ms**\n\n" ,
                        IsInline = true
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Specific connections report",
                        Value = $"Connection to Reddit: **{app_to_red_cli.Days }d, {app_to_red_cli.Hours }h, {app_to_red_cli.Minutes }min, {app_to_red_cli.Seconds }s, {app_to_red_cli.Milliseconds }ms**\n" +
                                $"Connection to Discord: **{app_to_dis.Days }d, {app_to_dis.Hours }h, {app_to_dis.Minutes }min, {app_to_dis.Seconds }s, {app_to_dis.Milliseconds }ms**\n" +
                                $"Reddit Connection to Default Sub: **{red_cli_to_sub.Days }d, {red_cli_to_sub.Hours }h, {red_cli_to_sub.Minutes }min, {red_cli_to_sub.Seconds }s, {red_cli_to_sub.Milliseconds }ms**\n" +
                                $"Default Sub to Discord: **{sub_to_discord.Days }d, {sub_to_discord.Hours }h, {sub_to_discord.Minutes }min, {sub_to_discord.Seconds }s, {sub_to_discord.Milliseconds }ms**\n\n",
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
                            Value = $"App uptime: **{app_uptime.Ticks } ticks**\n" +
                                    $"Connection uptime: **{con_uptime.Ticks } ticks**\n\n" ,
                            IsInline = true
                        },
                        new EmbedFieldBuilder
                        {
                            Name = "Specific connections report",
                            Value = $"Connection to Reddit: **{app_to_red_cli.Ticks } ticks**\n" +
                                    $"Connection to Discord: **{app_to_dis.Ticks } ticks**\n" +
                                    $"Reddit Connection to Default Sub: **{red_cli_to_sub.Ticks } ticks**\n" +
                                    $"Default Sub to Discord: **{sub_to_discord.Ticks } ticks**\n\n",
                            IsInline = true
                        }
                    }
                );

                //await ReplyAsync("Uptime (from application start) is **{uptime.Days } days, {uptime.Hours } hours, {uptime.Minutes } minutes, {uptime.Seconds } seconds.**");
            }

            [Command("report?doom-tics", RunMode = RunMode.Async), Summary("Gets application and connection uptimes into DOOM realtics (1/35ths of a second).")]
            public async Task ReportDOOMTics()
            {
                float _tic = 35;

                await BuildReport(Context,
                    new List<EmbedFieldBuilder>()
                    {
                        new EmbedFieldBuilder
                        {
                            Name = "Broad report",
                            Value = $"App uptime: **{(System.Math.Round((app_uptime.TotalSeconds / _tic)*100)/100) } DOOM realtics**\n" +
                                    $"Connection uptime: **{(System.Math.Round((con_uptime.TotalSeconds / _tic)*100)/ 100) } DOOM realtics**\n\n" ,
                            IsInline = true
                        },
                        new EmbedFieldBuilder
                        {
                            Name = "Specific connections report",
                            Value = $"Connection to Reddit: **{(System.Math.Round((app_to_red_cli.TotalSeconds / _tic)*100)/100) } DOOM realtics**\n" +
                                    $"Connection to Discord: **{(System.Math.Round((app_to_dis.TotalSeconds / _tic)*100)/100) } DOOM realtics**\n" +
                                    $"Reddit Connection to Default Sub: **{(System.Math.Round((red_cli_to_sub.TotalSeconds / _tic)*100)/100) } DOOM realtics**\n" +
                                    $"Default Sub to Discord: **{(System.Math.Round((sub_to_discord.TotalSeconds / _tic)*100)/100) } DOOM realtics**\n\n",
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
                            Value = $"App start time: **{Program.appStart }**\n" +
                                    $"Connection time: **{Program.connectStart }**\n\n" ,
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
                long _used = currentProcess.WorkingSet64;

                await ReplyAsync($"Memory used (MB): **{(_used / 1000000) }**");
            }

            [Command("report"), Summary("Gets working set memory, virtual memory, paged memory, and their peaks for xubot.")]
            public async Task Report()
            {
                Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                double _ws = currentProcess.WorkingSet64 / 1000000;
                double _pws = currentProcess.PeakWorkingSet64 / 1000000;

                double _vms = currentProcess.VirtualMemorySize64 / 1000000;
                double _pvms = currentProcess.PeakVirtualMemorySize64 / 1000000;

                double _pm = currentProcess.PagedMemorySize64 / 1000000;
                double _ppm = currentProcess.PeakPagedMemorySize64 / 1000000;

                await BuildReport(Context, new List<EmbedFieldBuilder>()
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Working Set",
                        Value = $"Used (MB): **{_ws }** | Peak (MB): **{_pws }**\n\n",
                        IsInline = true
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Virtual Memory",
                        Value = $"Virutal (MB): **{_vms }** | Virutal Peak (MB): **{_pvms }**\n\n",
                        IsInline = true
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Paged Memory",
                        Value = $"Paged (MB): **{_pm }** | Paged Peak (MB): **{_ppm }**\n\n",
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
