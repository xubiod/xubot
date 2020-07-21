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
            fontCollect.Install("./include/Roboto-Regular.ttf");
        }

        [Group("text-overlay"), Summary("A couple of commands relating to overlaying text on an attached image.")]
        public class TextOverlay : ModuleBase
        {
            public static int Size, X, Y;
            public static string Text;

            public static int Wraparound;
            public static int R, G, B;

            public static int HeaderHeight;
            public static int LRMargin;
            public static int TBMargin;

            [Command("direct", RunMode = RunMode.Async), Summary("Overlays text on an image. The parameter string has a very specific format that **must** be followed: ```\"x,y,text,size\"```The optional parameter has a specific format too: ```\"textwrap width,r,g,b\"```")]
            public async Task Direct(string parameter, string optional = "")
            {
                DirectUtils.InterpParameters(parameter);
                if (optional != "") DirectUtils.InterpOptionalParameters(optional);

                await Util.Attachment.DownloadLastAttachmentAsync(Context, Path.GetTempPath() + "textoverlay", true);
                string type = Path.GetExtension(Util.Attachment.ReturnLastAttachmentURL(Context));

                font = new Font(fontCollect.Find("Roboto"), Size);

                using (var img = SLImage.Load(Path.GetTempPath() + "textoverlay" + type))
                using (Image<Rgba32> container = new Image<Rgba32>(img.Width * 5, img.Height * 5))
                {
                    container.Mutate(mut => mut.DrawImage(img, new Point(img.Width * 2, img.Height * 2), PixelColorBlendingMode.Normal, 1.0F));
                    if (optional != "")
                    {
                        container.Mutate(mut => mut.DrawText(new TextGraphicsOptions() { WrapTextWidth = Wraparound, ColorBlendingMode = PixelColorBlendingMode.Normal }, Text, font, new Rgba32(R / 255, G / 255, B / 255), new PointF((img.Width * 2) + X, (img.Height * 2) + Y)));
                    }
                    else
                    {
                        container.Mutate(mut => mut.DrawText(Text, font, Rgba32.Black, new PointF((img.Width * 2) + X, (img.Height * 2) + Y)));
                    }

                    // proper cropping
                    container.Mutate(mut => mut.Crop(new Rectangle(img.Width * 2, img.Height * 2, img.Width, img.Height)));

                    // produces a "bts" result scaled down
                    // container.Mutate(mut => mut.Resize(new ResizeOptions() { Mode = ResizeMode.Crop, Size = new Size(img.Width, img.Height) }));

                    container.Save(Path.GetTempPath() + "textoverlay_new" + type);
                }
                await Context.Channel.SendFileAsync(Path.GetTempPath() + "textoverlay_new" + type);
            }

            [Command("header", RunMode = RunMode.Async), Summary("Makes a header on up of an image. The parameter string has a very specific format that **must** be followed: ```\"header height,left-right margin,top-bottom margin,text,size\"```The optional parameter has a specific format too: ```\"r, g, b\"```")]
            public async Task Header(string parameter, string optional = "")
            {
                HeaderUtils.InterpParameters(parameter);
                if (optional != "") HeaderUtils.InterpOptionalParameters(optional);

                await Util.Attachment.DownloadLastAttachmentAsync(Context, Path.GetTempPath() + "textoverlay", true);
                string type = Path.GetExtension(Util.Attachment.ReturnLastAttachmentURL(Context));

                font = new Font(fontCollect.Find("Roboto"), Size);

                using (var img = SLImage.Load(Path.GetTempPath() + "textoverlay" + type))
                using (Image<Rgba32> container = new Image<Rgba32>(img.Width, img.Height + HeaderHeight))
                {
                    // forced parameters
                    X = LRMargin;
                    Y = TBMargin;
                    Wraparound = img.Width - (2 * LRMargin);

                    container.Mutate(mut => mut.Fill(Rgba32.White));

                    container.Mutate(mut => mut.DrawImage(img, new Point(0, HeaderHeight), PixelColorBlendingMode.Normal, 1.0F));
                    if (optional != "")
                    {
                        container.Mutate(mut => mut.DrawText(new TextGraphicsOptions() { WrapTextWidth = Wraparound, ColorBlendingMode = PixelColorBlendingMode.Normal }, Text, font, new Rgba32(R / 255, G / 255, B / 255), new PointF(X, Y)));
                    }
                    else
                    {
                        container.Mutate(mut => mut.DrawText(new TextGraphicsOptions() { WrapTextWidth = Wraparound, ColorBlendingMode = PixelColorBlendingMode.Normal }, Text, font, Rgba32.Black, new PointF(X, Y)));
                    }

                    container.Save(Path.GetTempPath() + "textoverlay_new" + type);
                }
                await Context.Channel.SendFileAsync(Path.GetTempPath() + "textoverlay_new" + type);
            }

            public class DirectUtils
            {
                public static void InterpParameters(string input)
                {
                    string[] split = input.Split(",");

                    X = int.Parse(split[0]);
                    Y = int.Parse(split[1]);
                    Text = split[2];
                    Size = int.Parse(split[3]);
                }

                public static void InterpOptionalParameters(string input)
                {
                    string[] split = input.Split(",");

                    R = int.Parse(split[0]);
                    G = int.Parse(split[1]);
                    B = int.Parse(split[2]);
                }
            }

            public class HeaderUtils
            {
                public static void InterpParameters(string input)
                {
                    string[] split = input.Split(",");

                    HeaderHeight = int.Parse(split[0]);
                    LRMargin = int.Parse(split[1]);
                    TBMargin = int.Parse(split[2]);
                    Text = split[3];
                    Size = int.Parse(split[4]);
                }

                public static void InterpOptionalParameters(string input)
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
