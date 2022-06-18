using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using xubot.Attributes;
using SLImage = SixLabors.ImageSharp.Image;

namespace xubot.Commands
{
    public class ShitPost : ModuleBase
    {
        public static FontCollection fontCollect = new();
        public static Font font;

        public static void Populate()
        {
            fontCollect.Add("./include/Roboto-Regular.ttf");
        }

        [Group("text-overlay"), Summary("A couple of commands relating to overlaying text on an attached image."), Deprecated]
        public class TextOverlay : ModuleBase
        {
            public static int Size, X, Y;
            public static string Text;

            public static int Wraparound;
            public static int R, G, B;

            public static int HeaderHeight;
            public static int lrMargin;
            public static int tbMargin;

            [Example("\"0,0,example,24\"",true)]
            [Command("direct", RunMode = RunMode.Async), Summary("Overlays text on an image. The parameter string has a very specific format that **must** be followed: ```\"x,y,text,size\"```The optional parameter has a specific format too: ```\"text-wrap width,r,g,b\"```")]
            public async Task Direct(string parameter, string optional = "")
            {
                DirectUtils.InterpretParameters(parameter);
                if (optional != "") DirectUtils.InterpretOptionalParameters(optional);

                await Util.File.DownloadLastAttachmentAsync(Context, Path.GetTempPath() + "text-overlay", true);
                string type = Path.GetExtension(Util.File.ReturnLastAttachmentUrl(Context));

                font = new Font(fontCollect.Get("Roboto"), Size);

                using (var img = await SLImage.LoadAsync(Path.GetTempPath() + "text-overlay" + type))
                using (Image<Rgba32> container = new Image<Rgba32>(img.Width * 5, img.Height * 5))
                {
                    container.Mutate(mut => mut.DrawImage(img, new Point(img.Width * 2, img.Height * 2), PixelColorBlendingMode.Normal, 1.0F));

                    if (optional != "")
                    {
                        container.Mutate(mut => mut.DrawText(Text, font, new Rgba32(R / 255, G / 255, B / 255), new PointF(img.Width * 2 + X, img.Height * 2 + Y)));
                    }
                    else
                    {
                        container.Mutate(mut => mut.DrawText(Text, font, Rgba32.ParseHex("000000"), new PointF(img.Width * 2 + X, img.Height * 2 + Y)));
                    }

                    // proper cropping
                    container.Mutate(mut => mut.Crop(new Rectangle(img.Width * 2, img.Height * 2, img.Width, img.Height)));

                    // produces a "bts" result scaled down
                    // container.Mutate(mut => mut.Resize(new ResizeOptions() { Mode = ResizeMode.Crop, Size = new Size(img.Width, img.Height) }));

                    await container.SaveAsync(Path.GetTempPath() + "text-overlay_new" + type);
                }
                await Context.Channel.SendFileAsync(Path.GetTempPath() + "text-overlay_new" + type);
            }

            [Example("\"36,4,24,example,24\"", true)]
            [Command("header", RunMode = RunMode.Async), Summary("Makes a header on up of an image. The parameter string has a very specific format that **must** be followed: ```\"header height,left-right margin,top-bottom margin,text,size\"```The optional parameter has a specific format too: ```\"r, g, b\"```")]
            public async Task Header(string parameter, string optional = "")
            {
                HeaderUtils.InterpretParameters(parameter);
                if (optional != "") HeaderUtils.InterpretOptionalParameters(optional);

                await Util.File.DownloadLastAttachmentAsync(Context, Path.GetTempPath() + "text-overlay", true);
                string type = Path.GetExtension(Util.File.ReturnLastAttachmentUrl(Context));

                font = new Font(fontCollect.Get("Roboto"), Size);

                using (var img = await SLImage.LoadAsync(Path.GetTempPath() + "text-overlay" + type))
                using (Image<Rgba32> container = new Image<Rgba32>(img.Width, img.Height + HeaderHeight))
                {
                    // forced parameters
                    X = lrMargin;
                    Y = tbMargin;
                    Wraparound = img.Width - 2 * lrMargin;

                    container.Mutate(mut => mut.Fill(Rgba32.ParseHex("FFFFFF")));

                    container.Mutate(mut => mut.DrawImage(img, new Point(0, HeaderHeight), PixelColorBlendingMode.Normal, 1.0F));
                    if (optional != "")
                    {
                        container.Mutate(mut => mut.DrawText(Text, font, new Rgba32(R / 255, G / 255, B / 255), new PointF(X, Y)));
                    }
                    else
                    {
                        container.Mutate(mut => mut.DrawText(Text, font, Rgba32.ParseHex("000000"), new PointF(X, Y)));
                    }

                    await container.SaveAsync(Path.GetTempPath() + "text-overlay_new" + type);
                }
                await Context.Channel.SendFileAsync(Path.GetTempPath() + "text-overlay_new" + type);
            }

            public class DirectUtils
            {
                public static void InterpretParameters(string input)
                {
                    string[] split = input.Split(",");

                    X = int.Parse(split[0]);
                    Y = int.Parse(split[1]);
                    Text = split[2];
                    Size = int.Parse(split[3]);
                }

                public static void InterpretOptionalParameters(string input)
                {
                    string[] split = input.Split(",");

                    R = int.Parse(split[0]);
                    G = int.Parse(split[1]);
                    B = int.Parse(split[2]);
                }
            }

            public class HeaderUtils
            {
                public static void InterpretParameters(string input)
                {
                    string[] split = input.Split(",");

                    HeaderHeight = int.Parse(split[0]);
                    lrMargin = int.Parse(split[1]);
                    tbMargin = int.Parse(split[2]);
                    Text = split[3];
                    Size = int.Parse(split[4]);
                }

                public static void InterpretOptionalParameters(string input)
                {
                    string[] split = input.Split(",");

                    Wraparound = int.Parse(split[0]);
                    R = int.Parse(split[1]);
                    G = int.Parse(split[2]);
                    B = int.Parse(split[3]);
                }
            }
        }
    }
}
