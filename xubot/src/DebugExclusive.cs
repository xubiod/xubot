using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xubot.Commands.Global;
using static xubot.JokeException;
using SLImage = SixLabors.ImageSharp.Image;
using Color = Discord.Color;

namespace xubot.src
{
#if (DEBUG)

    [Group("debug"), Summary("A group of debug commands for quick debug work. Cannot be used by anyone except owner, and don't have examples given."), RequireOwner]
    public class Debug : ModuleBase
    {
        [Command("return_attach")]
        public async Task ReturnAttachments()
        {
            var attach = Context.Message.Attachments;
            IAttachment attached = null;

            foreach (var tempAttachment in attach)
            {
                attached = tempAttachment;
            }

            await ReplyAsync($"{attach}\nURL:{(attached != null ? attached.Url : "No url")}");
        }

        [Command("return_source")]
        public async Task Test001()
        {
            var stuff = Context.Message.Source;

            await ReplyAsync(stuff.ToString());
        }

        [Command("return_type")]
        public async Task Test002()
        {
            var stuff = Context.Message.Type;

            await ReplyAsync(stuff.ToString());
        }

        [Command("get_mood")]
        public async Task Test003(ulong id)
        {
            MoodTools.AddOrRefreshMood(Program.XuClient.GetUser(id));
            double mood = MoodTools.ReadMood(Program.XuClient.GetUser(id));

            string moodAsStr = mood switch
            {
                >= -16 and <= 16 => "neutral",
                <= -16 => "negative",
                >= 16 => "positive",
                _ => "invalid"
            };

            await ReplyAsync($"{mood} / {moodAsStr}");
        }

        [Command("throw_new")]
        public async Task Test004(int id)
        {
            try
            {
                switch (id)
                {
                    case 0: throw new ItsFuckingBrokenException();
                    case 1: throw new HaveNoFuckingIdeaException();
                    case 2: throw new PleaseKillMeException();
                    case 3: throw new ShitCodeException();
                    case 4: throw new StopDoingThisMethodException();
                    case 5: throw new ExceptionException();
                    case 6: throw new InsertBetterExceptionNameException();
                    default:
                        {
                            await ReplyAsync("invaild id");
                            break;
                        }
                }
            }
            catch (Exception exp)
            {
                await Util.Error.BuildError(exp, Context);
            }
        }

        [Command("ct")]
        public async Task Test005()
        {
            await ReplyAsync(Color.LightOrange.ToString());
        }

        [Command("li")]
        public async Task Test006()
        {
            List<SocketGuild> guildList = Program.XuClient.Guilds.ToList();
            string all = "";

            foreach (var item in guildList)
            {
                all += $"{item.Name} ({item.Id})\n";
            }

            await ReplyAsync(all);
        }

        [Command("attachment data")]
        public async Task Test007()
        {
            string all = $"c: {Context.Message.Attachments.Count}\nl: <{Util.File.ReturnLastAttachmentUrl(Context)}>\nf:";

            await Util.File.DownloadLastAttachmentAsync(Context, Path.GetTempPath() + "/download_success.data");

            await Context.Channel.SendFileAsync(Path.GetTempPath() + "/download_success.data", all);
        }

        [Command("manipulation")]
        public async Task Test008()
        {
            try
            {
                await Util.File.DownloadLastAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                await ReplyAsync("past download");
                string type = Path.GetExtension(Util.File.ReturnLastAttachmentUrl(Context));
                await ReplyAsync("type retrieved");

                await ReplyAsync("going into the `using (var img = SLImage.Load(Path.GetTempPath() + \"manip\" + type))` block");
                using (var img = await SLImage.LoadAsync(Path.GetTempPath() + "manip" + type))
                {
                    img.Mutate(mut => mut.Invert());
                    await ReplyAsync("img manipulated");
                    await img.SaveAsync(Util.String.RandomTempFilename() + type);
                    await ReplyAsync("img save");
                }

                await ReplyAsync("begin send");
                await Context.Channel.SendFileAsync(Util.String.RandomTempFilename() + type);
                await ReplyAsync("end send");
            }
            catch (Exception e)
            {
                await Util.Error.BuildError(e, Context);
            }
        }

        [Command("channels")]
        public async Task Test009()
        {
            try
            {
                IDMChannel ifDm = await Context.Message.Author.CreateDMChannelAsync();
                // ITextChannel dMtoTxt = ifDm as ITextChannel;
                // ITextChannel sTtoTxt = Context.Channel as ITextChannel;

                await ReplyAsync(ifDm.Id.ToString());
                await ReplyAsync(Context.Channel.Id.ToString());
            }
            catch (Exception e)
            {
                await Util.Error.BuildError(e, Context);
            }
        }

        [Command("nsfw")]
        public async Task Test010()
        {
            try
            {
                await ReplyAsync((await Util.IsChannelNsfw(Context)).ToString());
            }
            catch (Exception e)
            {
                await Util.Error.BuildError(e, Context);
            }
        }

        [Command("new error handling")]
        public async Task Test011()
        {
            await Util.Error.BuildError("you triggered the debug command", Context);
        }

        //[Command("get_settings")]
        //public async Task Test012()
        //{
        //    PropertyInfo setting = Util.Settings.GetPropertyInfo("DMsAlwaysNSFW");
        //    await ReplyAsync($"{setting.ToString()}");
        //    await ReplyAsync($"{setting.Name}");
        //    await ReplyAsync($"{setting.GetMethod.Name}");
        //    await ReplyAsync($"{setting.GetMethod.ReturnType.ToString()}");
        //    await ReplyAsync($"{setting.GetMethod.Invoke(src.Settings.Global.Default, null)}");
        //    await ReplyAsync($"{Util.Settings.GetValueFromString("DMsAlwaysNSFW")}");
        //}
    }

#endif
}
