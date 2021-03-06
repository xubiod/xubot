using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Net.Http;
using System.Drawing;
using System.IO;
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
using System.Collections.Generic;

namespace xubot.src.Commands
{
    public class Picture : ModuleBase
    {
        [Group("pic"), Summary("Does shit with images.")]
        public class PictureCMD : ModuleBase
        {
            [Group("manip"), Summary("Manipulates images. Can be used to deepfry.")]
            public class Manipulation : ModuleBase
            {
                [Command("brightness", RunMode = RunMode.Async), Summary("Increases/decreases the brightness of the image.")]
                public async Task Brightness(float amt) { HandleFilter(Context, mut => mut.Brightness(amt)); }

                [Command("bw", RunMode = RunMode.Async), Summary("Makes an image black and white.")]
                public async Task BW() { HandleFilter(Context, mut => mut.BlackWhite()); }

                [Command("colorblind", RunMode = RunMode.Async), Alias("colourblind"), Summary("Applies a filter to simulate colourblindness.")]
                public async Task EmulateColourblindness(string _type)
                {
                    switch (_type.ToLower())
                        {
                        // NO COLOUR
                        case "achromatomaly":
                        case "part-mono":
                        {
                            HandleFilter(Context, mut => mut.ColorBlindness(ColorBlindnessMode.Achromatomaly));
                            break;
                        }

                        case "achromatopsia":
                        case "mono":
                        {
                            HandleFilter(Context, mut => mut.ColorBlindness(ColorBlindnessMode.Achromatopsia));
                            break;
                        }

                        // GREEN
                        case "deuteranomaly":
                        case "weak-green":
                        {
                            HandleFilter(Context, mut => mut.ColorBlindness(ColorBlindnessMode.Deuteranomaly));
                            break;
                        }

                        case "deuteranopia":
                        case "blind-green":
                        {
                            HandleFilter(Context, mut => mut.ColorBlindness(ColorBlindnessMode.Deuteranopia));
                            break;
                        }

                        // RED
                        case "protanomaly":
                        case "weak-red":
                        {
                            HandleFilter(Context, mut => mut.ColorBlindness(ColorBlindnessMode.Protanomaly));
                            break;
                        }

                        case "protanopia":
                        case "blind-red":
                        {
                            HandleFilter(Context, mut => mut.ColorBlindness(ColorBlindnessMode.Protanopia));
                            break;
                        }

                        // BLUE
                        case "tritanomaly":
                        case "weak-blue":
                        {
                            HandleFilter(Context, mut => mut.ColorBlindness(ColorBlindnessMode.Tritanomaly));
                            break;
                        }

                        case "tritanopia":
                        case "blind-blue":
                        {
                            HandleFilter(Context, mut => mut.ColorBlindness(ColorBlindnessMode.Tritanopia));
                            break;
                        }
                        default:
                        {
                            await ReplyAsync("I wasn't given a valid filter name...");
                            break;
                        }
                    }
                }

                [Command("colorblind?list", RunMode = RunMode.Async), Alias("colourblind?list"), Summary("Lists the colourblindness filters.")]
                public async Task ColourblindnessList()
                {
                    EmbedBuilder embedd = new EmbedBuilder
                    {
                        Title = "Colourblind Filter List",
                        Color = Discord.Color.Magenta,
                        Description = "All the filters for the colourblindness emulation.",
                        ThumbnailUrl = Program.xuClient.CurrentUser.GetAvatarUrl(),

                        Footer = new EmbedFooterBuilder
                        {
                            Text = "xubot :p",
                            IconUrl = Program.xuClient.CurrentUser.GetAvatarUrl()
                        },
                        Timestamp = DateTime.UtcNow,
                        Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Total Colour Blindness",
                                Value = "**Achromatomaly** (part-mono)\n**Achromatopsia** (mono)" ,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Red-Green Colour Deficiency (Low/No Green Cones)",
                                Value = "**Deuteranomaly** (weak-green)\n**Deuteranopia** (blind-green)" ,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Red-Green Colour Deficiency (Low/No Red Cones)",
                                Value = "**Protanomaly** (weak-red)\n**Protanopia** (blind-red)" ,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Blue-Yellow Colour Deficiency (Low/No Blue Cones)",
                                Value = "**Tritanomaly** (weak-blue)\n**Tritanopia** (blind-blue)" ,
                                IsInline = true
                            }
                        }
                    };
                    await ReplyAsync("", false, embedd.Build());
                }

                [Command("contrast", RunMode = RunMode.Async), Summary("Increases/decreases the contrast of an image.")]
                public async Task Contrast(float amt) { HandleFilter(Context, mut => mut.Contrast(amt)); }

                [Command("hue-rotate", RunMode = RunMode.Async), Summary("Shifts/rotates the hues of an image by a given amount of degrees.")]
                public async Task RotateHue(float deg) { HandleFilter(Context, mut => mut.Hue(deg)); }

                [Command("invert", RunMode = RunMode.Async), Summary("iNVERTS THE COLORS OF AN IMAGE.")]
                public async Task Invert() { HandleFilter(Context, mut => mut.Invert()); }

                [Command("kodachrome", RunMode = RunMode.Async), Summary("Applies a kodachrome filter to an image.")]
                public async Task Kodachrome() { HandleFilter(Context, mut => mut.Kodachrome()); }

                [Command("lomograph", RunMode = RunMode.Async), Summary("Applies a lomograph filter to an image.")]
                public async Task Lomograph() {  HandleFilter(Context, mut => mut.Lomograph()); }

                [Command("polaroid", RunMode = RunMode.Async), Summary("Applies a polaroid filter to an image.")]
                public async Task Polaroid() { HandleFilter(Context, mut => mut.Polaroid()); }

                [Command("saturate", RunMode = RunMode.Async), Summary("Increases/decreases the saturation of an image.")]
                public async Task Saturate(float amount) { HandleFilter(Context, mut => mut.Saturate(amount)); }

                [Command("sepia", RunMode = RunMode.Async), Summary("Applies a sepia filter to an image.")]
                public async Task Sepia() { HandleFilter(Context, mut => mut.Sepia()); }

                [Command("threshold", RunMode = RunMode.Async), Summary("Applies a binary threshold to an image.")]
                public async Task BinaryThreshold(float threshold) { HandleFilter(Context, mut => mut.BinaryThreshold(threshold)); }

                [Group("blur"), Summary("Differnet blur and sharpen effects.")]
                public class Blur : ModuleBase
                {
                    [Command("bokeh", RunMode = RunMode.Async), Summary("Applies a basic bokeh blur to an image.")]
                    public async Task Bokeh() { HandleFilter(Context, mut => mut.BokehBlur()); }

                    [Command("bokeh", RunMode = RunMode.Async), Summary("Applies bokeh blur to an image.")]
                    public async Task Bokeh(int radius, int kernel_count, float gamma) { HandleFilter(Context, mut => mut.BokehBlur(radius, kernel_count, gamma)); }

                    [Command("box", RunMode = RunMode.Async), Summary("Applies a basic box blur to an image.")]
                    public async Task Box() { HandleFilter(Context, mut => mut.BoxBlur()); }

                    [Command("box", RunMode = RunMode.Async), Summary("Applies box blur to an image.")]
                    public async Task Box(int radius) { HandleFilter(Context, mut => mut.BoxBlur(radius)); }

                    [Command("gaussian", RunMode = RunMode.Async), Summary("Applies a basic Gaussian blur to an image.")]
                    public async Task Gaussian() { HandleFilter(Context, mut => mut.GaussianBlur()); }

                    [Command("gaussian", RunMode = RunMode.Async), Summary("Applies Gaussian blur to an image.")]
                    public async Task Gaussian(float weight) { HandleFilter(Context, mut => mut.GaussianBlur(weight)); }

                    [Command("gaussian-sharp", RunMode = RunMode.Async), Summary("Applies a basic Gaussian sharpen to an image.")]
                    public async Task GaussianSharp() { HandleFilter(Context, mut => mut.GaussianSharpen()); }

                    [Command("gaussian-sharp", RunMode = RunMode.Async), Summary("Applies a basic Gaussian sharpen to an image.")]
                    public async Task GaussianSharp(float weight) { HandleFilter(Context, mut => mut.GaussianSharpen(weight)); }
                }

                private static string ApplyFilter(string load, string type, System.Action<IImageProcessingContext> mutation)
                {
                    string filename = Util.Str.RandomFilename() + type;

                    using (var img = SLImage.Load(load + type))
                    {
                        img.Mutate(mutation);
                        img.Save(filename);
                    }

                    return filename;
                }

                private static async void HandleFilter(ICommandContext Context, System.Action<IImageProcessingContext> mutation)
                {
                    Util.StatusReactions.Begin(Context);

                    string loadas = Path.GetTempPath() + "manip";

                    await Util.File.DownloadLastAttachmentAsync(Context, loadas, true);
                    string type = Path.GetExtension(Util.File.ReturnLastAttachmentURL(Context));

                    string filename = ApplyFilter(loadas, type, mutation);

                    await Context.Channel.SendFileAsync(filename);

                    Util.StatusReactions.End(Context);
                }
            }
        }
    }
}
