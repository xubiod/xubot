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
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using SixLabors.ImageSharp.Processing.Processors.Dithering;

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
                private static Dictionary<string, IQuantizer> all_quantizers = new Dictionary<string, IQuantizer>() {
                    { "websafe", KnownQuantizers.WebSafe }, { "web", KnownQuantizers.WebSafe }, { "web-safe", KnownQuantizers.WebSafe },
                    { "werner", KnownQuantizers.Werner }, { "1821", KnownQuantizers.Werner },
                    { "wu", KnownQuantizers.Wu },{ "xiaolin-wu", KnownQuantizers.Wu },{ "high", KnownQuantizers.Wu },{ "highquality", KnownQuantizers.Wu },{ "high-quality", KnownQuantizers.Wu },
                    { "octree", KnownQuantizers.Octree },{ "fast", KnownQuantizers.Octree },{ "adaptive", KnownQuantizers.Octree },{ "f", KnownQuantizers.Octree }
                };

                private static Dictionary<string, IDither> all_dithering =
                    new Dictionary<string, IDither>() { { "atkinson", KnownDitherings.Atkinson }, { "bayer2", KnownDitherings.Bayer2x2 }, { "bayer4", KnownDitherings.Bayer4x4}, { "bayer8", KnownDitherings.Bayer8x8}, { "burks", KnownDitherings.Burks},
                        { "floydsteinberg", KnownDitherings.FloydSteinberg }, {"jarvisjudiceninke", KnownDitherings.JarvisJudiceNinke}, { "ordered3", KnownDitherings.Ordered3x3}, { "sierra2", KnownDitherings.Sierra2}, { "sierra3", KnownDitherings.Sierra3},
                        { "sierralite", KnownDitherings.SierraLite}, { "stevensonarce", KnownDitherings.StevensonArce}, { "stucki", KnownDitherings.Stucki } };

                [Command("bw", RunMode = RunMode.Async), Summary("Makes an image black and white.")]
                public async Task BW() { HandleFilter(Context, mut => mut.BlackWhite()); }

                [Command("invert", RunMode = RunMode.Async), Summary("iNVERTS THE COLORS OF AN IMAGE.")]
                public async Task Invert() { HandleFilter(Context, mut => mut.Invert()); }

                [Command("kodachrome", RunMode = RunMode.Async), Summary("Applies a kodachrome filter to an image.")]
                public async Task Kodachrome() { HandleFilter(Context, mut => mut.Kodachrome()); }

                [Command("lomograph", RunMode = RunMode.Async), Summary("Applies a lomograph filter to an image.")]
                public async Task Lomograph() { HandleFilter(Context, mut => mut.Lomograph()); }

                [Command("polaroid", RunMode = RunMode.Async), Summary("Applies a polaroid filter to an image.")]
                public async Task Polaroid() { HandleFilter(Context, mut => mut.Polaroid()); }

                [Command("sepia", RunMode = RunMode.Async), Summary("Applies a sepia filter to an image.")]
                public async Task Sepia() { HandleFilter(Context, mut => mut.Sepia()); }

                [Command("oilpaint", RunMode = RunMode.Async), Summary("Applies a basic oilpaint filter to an image.")]
                public async Task OilPaint() { HandleFilter(Context, mut => mut.OilPaint()); }

                [Command("vignette", RunMode = RunMode.Async), Summary("Applies a basic vignette to an image.")]
                public async Task Vignette() { HandleFilter(Context, mut => mut.Vignette()); }

                [Command("glow", RunMode = RunMode.Async), Summary("Applies a basic glow to an image.")]
                public async Task Glow() { HandleFilter(Context, mut => mut.Glow()); }

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

                [Group("params"), Alias("p"), Summary("Filters with parameters.")]
                public class Advanced : ModuleBase
                {
                    [Command("brightness", RunMode = RunMode.Async), Summary("Increases/decreases the brightness of the image.")]
                    public async Task Brightness(float amt) { HandleFilter(Context, mut => mut.Brightness(amt)); }

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
                                Text = Util.Globals.EmbedFooter,
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

                    [Command("saturate", RunMode = RunMode.Async), Summary("Increases/decreases the saturation of an image.")]
                    public async Task Saturate(float amount) { HandleFilter(Context, mut => mut.Saturate(amount)); }

                    [Command("threshold", RunMode = RunMode.Async), Summary("Applies a binary threshold to an image.")]
                    public async Task BinaryThreshold(float threshold) { HandleFilter(Context, mut => mut.BinaryThreshold(threshold)); }

                    [Command("oilpaint", RunMode = RunMode.Async), Summary("Applies an oilpaint filter to an image.")]
                    public async Task OilPaint(int levels, int brushSize) { HandleFilter(Context, mut => mut.OilPaint(levels, brushSize)); }

                    [Command("pixelate", RunMode = RunMode.Async), Summary("Applies a pixelation filter to an image.")]
                    public async Task Pixelate(int pixelSize) { HandleFilter(Context, mut => mut.Pixelate(pixelSize)); }

                    [Command("vignette", RunMode = RunMode.Async), Summary("Applies a vignette to an image.")]
                    public async Task Vignette(float radiusX, float radiusY) { HandleFilter(Context, mut => mut.Vignette(radiusX, radiusY)); }

                    [Command("quantize", RunMode = RunMode.Async), Summary("Applies a quantize filter to an image. 4 are available, accessible with 0 - 3 which is modulo'd with 4.")]
                    public async Task Quantize(string name) { HandleFilter(Context, mut => mut.Quantize(all_quantizers[name])); }

                    [Command("quantizers", RunMode = RunMode.Async), Summary("Returns all valid inputs for quantizers.")]
                    public async Task QuantizerListing()
                    {
                        string list = "";

                        foreach (string item in all_quantizers.Keys)
                            list += item + "\n";

                        await Context.Channel.SendMessageAsync("", false, GetListEmbed("Quantizer Methods", "Valid Methods", list).Build());
                    }

                    [Command("glow", RunMode = RunMode.Async), Summary("Applies a glow to an image.")]
                    public async Task Glow(float radius) { HandleFilter(Context, mut => mut.Glow(radius)); }

                    [Command("entropycrop", RunMode = RunMode.Async), Alias("entropy-crop"), Summary("Crops an image to the area of greatest entropy using a given threshold. Defaults to 0.5.")]
                    public async Task EntropyCrop(float threshold = 0.5F) { HandleFilter(Context, mut => mut.EntropyCrop(threshold)); }

                    [Command("basic-dither", RunMode = RunMode.Async), Summary("Applies a binary dithering effect to an image. 13 are available, accessible with its name. Use `pic manip ditherings` to get all valid names.")]
                    public async Task BinaryDither(string name)
                    {
                        if (!all_dithering.ContainsKey(name.ToLower())) { await ReplyAsync("That's not a dithering I know about..."); return; }
                        HandleFilter(Context, mut => mut.BinaryDither(all_dithering[name.ToLower()]));
                    }

                    [Command("dither", RunMode = RunMode.Async), Summary("Applies a dithering effect to an image. 13 are available, accessible with its name (use `pic manip ditherings` to get all valid names). The full palette is RGBA32 colors as hexadecimal strings.")]
                    public async Task Dither(string name, params string[] palette)
                    {
                        SixLabors.ImageSharp.Color[] colors = new SixLabors.ImageSharp.Color[palette.Length];
                        for (int i = 0; i < palette.Length; i++)
                            colors[i] = new SixLabors.ImageSharp.Color(Tools.ColorFromHexString(palette[i]));

                        ReadOnlyMemory<SixLabors.ImageSharp.Color> rom_palette = colors;

                        if (!all_dithering.ContainsKey(name.ToLower())) { await ReplyAsync("That's not a dithering I know about..."); return; }
                        HandleFilter(Context, mut => mut.Dither(all_dithering[name.ToLower()], rom_palette));
                    }

                    [Command("ditherings", RunMode = RunMode.Async), Summary("Returns all ditherings names.")]
                    public async Task DitherListing()
                    {
                        string list = "";

                        foreach (string item in all_dithering.Keys)
                            list += item + "\n";

                        await Context.Channel.SendMessageAsync("", false, GetListEmbed("Dithering Methods", "Valid Ditherings", list).Build());
                    }
                }

                private static string ApplyFilter(string load, string type, System.Action<IImageProcessingContext> mutation)
                {
                    string filename = Util.Str.RandomTempFilename() + type;

                    using (var img = SLImage.Load(load + type))
                    {
                        img.Mutate(mutation);
                        img.Save(filename);
                    }

                    return filename;
                }

                private static async void HandleFilter(ICommandContext Context, System.Action<IImageProcessingContext> mutation)
                {
                    using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                    {
                        string loadas = Path.GetTempPath() + "manip";

                        await Util.File.DownloadLastAttachmentAsync(Context, loadas, true);
                        string type = Path.GetExtension(Util.File.ReturnLastAttachmentURL(Context));

                        string filename = ApplyFilter(loadas, type, mutation);

                        await Context.Channel.SendFileAsync(filename);
                    }
                }

                private static EmbedBuilder GetListEmbed(string title, string name, string list)
                {
                    return new EmbedBuilder
                    {
                        Title = title,
                        Color = Discord.Color.Blue,
                        Description = "So you don't need to look at the source!",

                        Footer = new EmbedFooterBuilder
                        {
                            Text = Util.Globals.EmbedFooter
                        },
                        Timestamp = DateTime.UtcNow,
                        Fields = new List<EmbedFieldBuilder>()
                            {
                                new EmbedFieldBuilder
                                {
                                    Name = name,
                                    Value = "```" + list + "```",
                                    IsInline = false
                                }
                            }
                    };
                }
            }

            [Group("tools"), Summary("Tools for quickie things.")]
            public class Tools : ModuleBase
            {
                [Command("rgba"), Summary("Gets RGBA elements from a hex string representing an RGBA32 value.")]
                public async Task GetComponents(string hex)
                {
                    Rgba32 color = ColorFromHexString(hex);

                    EmbedBuilder embedd = new EmbedBuilder
                    {
                        Title = "Color",
                        Color = new Discord.Color(color.R, color.G, color.B),
                        Description = "Colors!",

                        Footer = new EmbedFooterBuilder
                        {
                            Text = Util.Globals.EmbedFooter
                        },
                        Timestamp = DateTime.UtcNow,
                        Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "RGB",
                                Value = $"{color.R}, {color.G}, {color.B} ({color.R.ToString("X")}{color.G.ToString("X")}{color.B.ToString("X")})",
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "RGBA",
                                Value = $"{color.R}, {color.G}, {color.B}, {color.A} ({color.R.ToString("X")}{color.G.ToString("X")}{color.B.ToString("X")}{color.A.ToString("X")})",
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Unpacked RGBA",
                                Value = $"{color.Rgba} ({color.Rgba.ToString("X")})",
                                IsInline = false
                            }
                        }
                    };

                    await Context.Channel.SendMessageAsync("", false, embedd.Build());
                }

                public static Rgba32 ColorFromHexString(string hexstring)
                {
                    return new Rgba32(uint.Parse(hexstring.ToLower().Replace("0x", "").Replace("&h", ""), System.Globalization.NumberStyles.AllowHexSpecifier));
                }
            }
        }
    }
}
