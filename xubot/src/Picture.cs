using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Net.Http;
using System.Drawing;
using System.IO;
using IronOcr;
using System.Threading;
using Discord;
using System.IO.Compression;
using SixLabors.ImageSharp;
using System.Web;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

using SLImage = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Processing.Filters;

namespace xubot
{
    public class PictureAndFile : ModuleBase
    {
        [Group("pic")]
        public class music_comm : ModuleBase
        {
            [Group("ocr")]
            public class ocr_comm : ModuleBase
            {
                [Command(RunMode = RunMode.Async)]
                public async Task read()
                {
                    var attach = Context.Message.Attachments;
                    IAttachment attached = null;

                    foreach (var _att in attach)
                    {
                        attached = _att;
                    }

                    var msg = await ReplyAsync("Please wait, this takes a while to read images.");

                    Random rnd = new Random();
                    int rnd_res = rnd.Next(99999);
                    string[] imgSplit = attached.ToString().Split('.');
                    string fileType = imgSplit[imgSplit.Length - 1];
                    fileType = "." + fileType;
                    string path = Path.Combine(Path.GetTempPath(), rnd_res.ToString()) + fileType;

                    System.Drawing.Image imgFromStream = null;

                    using (HttpClient client = new HttpClient())
                    using (HttpResponseMessage response = await client.GetAsync(attached.Url))
                    using (HttpContent content = response.Content)
                    {
                        imgFromStream = System.Drawing.Image.FromStream(await content.ReadAsStreamAsync());
                        Bitmap bitmap = (Bitmap)imgFromStream;

                        bitmap.Save(path);

                        var Ocr = new AutoOcr();
                        var Result = Ocr.Read(path);
                        Console.WriteLine(Result.Text);

                        await msg.DeleteAsync();
                        await ReplyAsync("**(Automatic) Iron OCR returned:\n** " + Result);
                    }
                }

                [Command(RunMode = RunMode.Async)]
                public async Task read(string img)
                {
                    var msg = await ReplyAsync("Please wait, this takes a while to read images.");

                    Random rnd = new Random();
                    int rnd_res = rnd.Next(99999);
                    string[] imgSplit = img.Split('.');
                    string fileType = imgSplit[imgSplit.Length - 1];
                    fileType = "." + fileType;
                    string path = Path.Combine(Path.GetTempPath(), rnd_res.ToString()) + fileType;

                    System.Drawing.Image imgFromStream = null;

                    using (HttpClient client = new HttpClient())
                    using (HttpResponseMessage response = await client.GetAsync(img))
                    using (HttpContent content = response.Content)
                    {
                        imgFromStream = System.Drawing.Image.FromStream(await content.ReadAsStreamAsync());
                        Bitmap bitmap = (Bitmap)imgFromStream;

                        bitmap.Save(path);

                        var Ocr = new AutoOcr();
                        var Result = Ocr.Read(path);
                        Console.WriteLine(Result.Text);

                        await msg.DeleteAsync();
                        await ReplyAsync("**(Automatic) Iron OCR returned:\n** " + Result);
                    }
                }
            }

            [Group("manip")]
            public class manipulation : ModuleBase
            {
                [Command("brightness", RunMode = RunMode.Async)]
                public async Task ghost(float amt)
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = VirtualPathUtility.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Brightness(amt));
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("bw", RunMode = RunMode.Async)]
                public async Task bw()
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = VirtualPathUtility.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.BlackWhite());
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("colorblind", RunMode = RunMode.Async)]
                public async Task emuColor(string _type)
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = VirtualPathUtility.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        switch (_type.ToLower())
                        {
                            case "achromatomaly": img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Achromatomaly)); break;
                            case "part-mono"    : img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Achromatomaly)); break;

                            case "achromatopsia": img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Achromatopsia)); break;
                            case "mono"         : img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Achromatopsia)); break;

                            case "deuteranomaly": img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Deuteranomaly)); break;
                            case "weak-green"   : img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Deuteranomaly)); break;

                            case "deuteranopia" : img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Deuteranopia)); break;
                            case "blind-green"  : img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Deuteranopia)); break;

                            case "protanomaly"  : img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Protanomaly)); break;
                            case "weak-red"     : img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Protanomaly)); break;

                            case "protanopia"   : img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Protanopia)); break;
                            case "blind-red"    : img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Protanopia)); break;

                            case "tritanomaly"  : img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Tritanomaly)); break;
                            case "weak-blue"    : img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Tritanomaly)); break;

                            case "tritanopia"   : img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Tritanopia)); break;
                            case "blind-blue"   : img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Tritanopia)); break;
                            default: break;
                        }

                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("contrast", RunMode = RunMode.Async)]
                public async Task contrast(float amt)
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = VirtualPathUtility.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Contrast(amt));
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("hue-rotate", RunMode = RunMode.Async)]
                public async Task huerotate(float deg)
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = VirtualPathUtility.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Hue(deg));
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("invert", RunMode = RunMode.Async)]
                public async Task invert()
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = VirtualPathUtility.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Invert());
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("kodachrome", RunMode = RunMode.Async)]
                public async Task kodachrome()
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = VirtualPathUtility.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Kodachrome());
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("lomograph", RunMode = RunMode.Async)]
                public async Task lomo()
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = VirtualPathUtility.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Lomograph());
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("polaroid", RunMode = RunMode.Async)]
                public async Task polaroid()
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = VirtualPathUtility.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Polaroid());
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("saturate", RunMode = RunMode.Async)]
                public async Task sat(float amount)
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = VirtualPathUtility.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Saturate(amount));
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("sepia", RunMode = RunMode.Async)]
                public async Task sepia()
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = VirtualPathUtility.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Sepia());
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("matrix", RunMode = RunMode.Async)]
                public async Task customfilter(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44)
                {
                    System.Numerics.Matrix4x4 custFilt = new System.Numerics.Matrix4x4();
                    custFilt.M11 = m11; custFilt.M12 = m12; custFilt.M13 = m13; custFilt.M14 = m14;
                    custFilt.M21 = m21; custFilt.M22 = m22; custFilt.M23 = m23; custFilt.M24 = m24;
                    custFilt.M31 = m31; custFilt.M32 = m32; custFilt.M33 = m33; custFilt.M34 = m34;
                    custFilt.M41 = m41; custFilt.M42 = m42; custFilt.M43 = m43; custFilt.M44 = m44;

                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = VirtualPathUtility.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Filter(custFilt));
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }
            }
        }
    }
}
