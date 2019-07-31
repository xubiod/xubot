using Discord.Commands;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SixLabors.Fonts;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

using SLImage = SixLabors.ImageSharp.Image;
using xubot_core.Properties;
using SixLabors.ImageSharp;

namespace xubot_core.src
{
    public class Shitpost : ModuleBase
    {
        public static FontCollection fontCollect = new FontCollection();
        public static Font font;

        public static void Populate()
        {
            using (Stream stream = new MemoryStream(Properties.Resources.Roboto_Regular))
                fontCollect.Install(stream);
        }

        public static int Size, X, Y;
        public static string Text;

        [Command("shitpost", RunMode = RunMode.Async), Summary("FUNNY JOKE")]
        public async Task ShitpostCmd(string file, string param)
        {
            InterpParameters(param);

            await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "shitpost", true);
            string type = Path.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

            font = new Font(fontCollect.Find("Roboto"), Size);

            using (var img = SLImage.Load(Path.GetTempPath() + "shitpost" + type))
            {
                img.Mutate(mut => mut.DrawText(new TextGraphicsOptions() { WrapTextWidth = img.Width - X } ,Text, font, Rgba32.Black, new PointF(X, Y)));

                img.Save(Path.GetTempPath() + "shitpost_new" + type);
            }

            await Context.Channel.SendFileAsync(Path.GetTempPath() + "shitpost_new" + type);
        }

        public static void InterpParameters(string input)
        {
            string[] split = input.Split(",");

            X = int.Parse(split[0]);
            Y = int.Parse(split[1]);
            Text = split[2];
            Size = int.Parse(split[3]);
        }
    }
}
