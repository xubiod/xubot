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
using xubot.src.Attributes;

namespace xubot.src.Commands
{
    [Group("pic"), Summary("Does shit with images.")]
    public class Picture : ModuleBase
    {
        [Group("manip"), Summary("Manipulates images. Can be used to deepfry.")]
        public class Manipulation : ModuleBase
        {
            private static readonly Dictionary<string, IQuantizer> AllQuantizers = new Dictionary<string, IQuantizer>() {
                { "websafe", KnownQuantizers.WebSafe }, { "web", KnownQuantizers.WebSafe }, { "web-safe", KnownQuantizers.WebSafe },
                { "werner", KnownQuantizers.Werner }, { "1821", KnownQuantizers.Werner },
                { "wu", KnownQuantizers.Wu }, { "xiaolin-wu", KnownQuantizers.Wu }, { "high", KnownQuantizers.Wu }, { "highquality", KnownQuantizers.Wu }, { "high-quality", KnownQuantizers.Wu },
                { "octree", KnownQuantizers.Octree }, { "fast", KnownQuantizers.Octree }, { "adaptive", KnownQuantizers.Octree }, { "f", KnownQuantizers.Octree }
            };

            private static readonly Dictionary<string, IDither> AllDithering =
                new Dictionary<string, IDither>() {
                    { "atkinson", KnownDitherings.Atkinson }, { "bayer2", KnownDitherings.Bayer2x2 }, { "bayer4", KnownDitherings.Bayer4x4}, { "bayer8", KnownDitherings.Bayer8x8}, { "burks", KnownDitherings.Burks},
                    { "floydsteinberg", KnownDitherings.FloydSteinberg }, {"jarvisjudiceninke", KnownDitherings.JarvisJudiceNinke}, { "ordered3", KnownDitherings.Ordered3x3}, { "sierra2", KnownDitherings.Sierra2}, { "sierra3", KnownDitherings.Sierra3},
                    { "sierralite", KnownDitherings.SierraLite}, { "stevensonarce", KnownDitherings.StevensonArce}, { "stucki", KnownDitherings.Stucki } };

            private static readonly Dictionary<string, ColorBlindnessMode> ColourBlindnessMap =
                new Dictionary<string, ColorBlindnessMode>() {
                    { "achromatomaly", ColorBlindnessMode.Achromatomaly }, { "part-mono",    ColorBlindnessMode.Achromatomaly },
                    { "weak-color",    ColorBlindnessMode.Achromatomaly }, { "color-weak",   ColorBlindnessMode.Achromatomaly },
                    { "weak-colour",   ColorBlindnessMode.Achromatomaly }, { "colour-weak",  ColorBlindnessMode.Achromatomaly },
                    { "achromatopsia", ColorBlindnessMode.Achromatopsia }, { "mono",         ColorBlindnessMode.Achromatopsia },
                    { "no-color",      ColorBlindnessMode.Achromatopsia }, { "blind-color",  ColorBlindnessMode.Achromatopsia }, { "color-blind",  ColorBlindnessMode.Achromatopsia },
                    { "no-colour",     ColorBlindnessMode.Achromatopsia }, { "blind-colour", ColorBlindnessMode.Achromatopsia }, { "colour-blind", ColorBlindnessMode.Achromatopsia },
                    { "deuteranomaly", ColorBlindnessMode.Deuteranomaly }, { "weak-green",   ColorBlindnessMode.Deuteranomaly }, { "green-weak",   ColorBlindnessMode.Deuteranomaly },
                    { "deuteranopia",  ColorBlindnessMode.Deuteranopia },  { "blind-green",  ColorBlindnessMode.Deuteranopia },  { "green-blind",  ColorBlindnessMode.Deuteranopia },
                    { "protanomaly",   ColorBlindnessMode.Protanomaly },   { "weak-red",     ColorBlindnessMode.Protanomaly },   { "red-weak",     ColorBlindnessMode.Protanomaly },
                    { "protanopia",    ColorBlindnessMode.Protanopia },    { "blind-red",    ColorBlindnessMode.Protanopia },    { "red-blind",    ColorBlindnessMode.Protanopia },
                    { "tritanomaly",   ColorBlindnessMode.Tritanomaly },   { "weak-blue",    ColorBlindnessMode.Tritanomaly },   { "blue-weak",    ColorBlindnessMode.Tritanomaly },
                    { "tritanopia",    ColorBlindnessMode.Tritanopia },    { "blind-blue",   ColorBlindnessMode.Tritanopia },    { "blue-blind",   ColorBlindnessMode.Tritanopia }
                };

            [ExampleAttribute(true)]
            [Command("bw", RunMode = RunMode.Async), Summary("Makes an image black and white.")]
            public async Task Bw() { HandleFilter(Context, mut => mut.BlackWhite()); }

            [ExampleAttribute(true)]
            [Command("invert", RunMode = RunMode.Async), Summary("iNVERTS THE COLORS OF AN IMAGE.")]
            public async Task Invert() { HandleFilter(Context, mut => mut.Invert()); }

            [ExampleAttribute(true)]
            [Command("kodachrome", RunMode = RunMode.Async), Summary("Applies a kodachrome filter to an image.")]
            public async Task Kodachrome() { HandleFilter(Context, mut => mut.Kodachrome()); }

            [ExampleAttribute(true)]
            [Command("lomograph", RunMode = RunMode.Async), Summary("Applies a lomograph filter to an image.")]
            public async Task Lomograph() { HandleFilter(Context, mut => mut.Lomograph()); }

            [ExampleAttribute(true)]
            [Command("polaroid", RunMode = RunMode.Async), Summary("Applies a polaroid filter to an image.")]
            public async Task Polaroid() { HandleFilter(Context, mut => mut.Polaroid()); }

            [ExampleAttribute(true)]
            [Command("sepia", RunMode = RunMode.Async), Summary("Applies a sepia filter to an image.")]
            public async Task Sepia() { HandleFilter(Context, mut => mut.Sepia()); }

            [ExampleAttribute(true)]
            [Command("oilpaint", RunMode = RunMode.Async), Summary("Applies a basic oilpaint filter to an image.")]
            public async Task OilPaint() { HandleFilter(Context, mut => mut.OilPaint()); }

            [ExampleAttribute(true)]
            [Command("vignette", RunMode = RunMode.Async), Summary("Applies a basic vignette to an image.")]
            public async Task Vignette() { HandleFilter(Context, mut => mut.Vignette()); }

            [ExampleAttribute(true)]
            [Command("glow", RunMode = RunMode.Async), Summary("Applies a basic glow to an image.")]
            public async Task Glow() { HandleFilter(Context, mut => mut.Glow()); }

            [Group("blur"), Summary("Differnet blur and sharpen effects.")]
            public class Blur : ModuleBase
            {
                [ExampleAttribute(true)]
                [Command("bokeh", RunMode = RunMode.Async), Summary("Applies a basic bokeh blur to an image.")]
                public async Task Bokeh() { HandleFilter(Context, mut => mut.BokehBlur()); }

                [ExampleAttribute("4 4 1.5", true)]
                [Command("bokeh", RunMode = RunMode.Async), Summary("Applies bokeh blur to an image.")]
                public async Task Bokeh(int radius, int kernelCount, float gamma) { HandleFilter(Context, mut => mut.BokehBlur(radius, kernelCount, gamma)); }

                [ExampleAttribute(true)]
                [Command("box", RunMode = RunMode.Async), Summary("Applies a basic box blur to an image.")]
                public async Task Box() { HandleFilter(Context, mut => mut.BoxBlur()); }

                [ExampleAttribute("4", true)]
                [Command("box", RunMode = RunMode.Async), Summary("Applies box blur to an image.")]
                public async Task Box(int radius) { HandleFilter(Context, mut => mut.BoxBlur(radius)); }

                [ExampleAttribute(true)]
                [Command("gaussian", RunMode = RunMode.Async), Summary("Applies a basic Gaussian blur to an image.")]
                public async Task Gaussian() { HandleFilter(Context, mut => mut.GaussianBlur()); }

                [ExampleAttribute("2.5", true)]
                [Command("gaussian", RunMode = RunMode.Async), Summary("Applies Gaussian blur to an image.")]
                public async Task Gaussian(float weight) { HandleFilter(Context, mut => mut.GaussianBlur(weight)); }

                [ExampleAttribute(true)]
                [Command("gaussian-sharp", RunMode = RunMode.Async), Summary("Applies a basic Gaussian sharpen to an image.")]
                public async Task GaussianSharp() { HandleFilter(Context, mut => mut.GaussianSharpen()); }

                [ExampleAttribute("2.5", true)]
                [Command("gaussian-sharp", RunMode = RunMode.Async), Summary("Applies a basic Gaussian sharpen to an image.")]
                public async Task GaussianSharp(float weight) { HandleFilter(Context, mut => mut.GaussianSharpen(weight)); }
            }

            [Group("params"), Alias("p"), Summary("Filters with parameters.")]
            public class Advanced : ModuleBase
            {
                [ExampleAttribute("1.50", true)]
                [Command("brightness", RunMode = RunMode.Async), Summary("Increases/decreases the brightness of the image.")]
                public async Task Brightness(float amt) { HandleFilter(Context, mut => mut.Brightness(amt)); }

                [ExampleAttribute("blue-blind", true)]
                [Command("colorblind", RunMode = RunMode.Async), Alias("colourblind"), Summary("Applies a filter to simulate colourblindness.")]
                public async Task EmulateColourblindness(string type)
                {
                    type = type.ToLower();

                    if (!ColourBlindnessMap.ContainsKey(type))
                    {
                        await ReplyAsync("I wasn't given a valid filter name...");
                        return;
                    }

                    HandleFilter(Context, mut => mut.ColorBlindness(ColourBlindnessMap[type]));
                }

                [Command("colorblind?list", RunMode = RunMode.Async), Alias("colourblind?list"), Summary("Lists the colourblindness filters.")]
                public async Task ColourblindnessList()
                {
                    EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Colourblind Filter List", "All the filters for the colourblindness emulation.", Discord.Color.Magenta);
                    embed.Fields = new List<EmbedFieldBuilder>()
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
                    };

                    await ReplyAsync("", false, embed.Build());
                }

                [ExampleAttribute("2.00", true)]
                [Command("contrast", RunMode = RunMode.Async), Summary("Increases/decreases the contrast of an image.")]
                public async Task Contrast(float amt) { HandleFilter(Context, mut => mut.Contrast(amt)); }

                [ExampleAttribute("180.0", true)]
                [Command("hue-rotate", RunMode = RunMode.Async), Summary("Shifts/rotates the hues of an image by a given amount of degrees.")]
                public async Task RotateHue(float deg) { HandleFilter(Context, mut => mut.Hue(deg)); }

                [ExampleAttribute("2.00", true)]
                [Command("saturate", RunMode = RunMode.Async), Summary("Increases/decreases the saturation of an image.")]
                public async Task Saturate(float amount) { HandleFilter(Context, mut => mut.Saturate(amount)); }

                [ExampleAttribute("0.50", true)]
                [Command("threshold", RunMode = RunMode.Async), Summary("Applies a binary threshold to an image.")]
                public async Task BinaryThreshold(float threshold) { HandleFilter(Context, mut => mut.BinaryThreshold(threshold)); }

                [ExampleAttribute("3 6", true)]
                [Command("oilpaint", RunMode = RunMode.Async), Summary("Applies an oilpaint filter to an image.")]
                public async Task OilPaint(int levels, int brushSize) { HandleFilter(Context, mut => mut.OilPaint(levels, brushSize)); }

                [ExampleAttribute("8", true)]
                [Command("pixelate", RunMode = RunMode.Async), Summary("Applies a pixelation filter to an image.")]
                public async Task Pixelate(int pixelSize) { HandleFilter(Context, mut => mut.Pixelate(pixelSize)); }

                [ExampleAttribute("720.0 480.0", true)]
                [Command("vignette", RunMode = RunMode.Async), Summary("Applies a vignette to an image.")]
                public async Task Vignette(float radiusX, float radiusY) { HandleFilter(Context, mut => mut.Vignette(radiusX, radiusY)); }

                [ExampleAttribute("fast", true)]
                [Command("quantize", RunMode = RunMode.Async), Summary("Applies a quantize filter to an image. 4 are available, accessible with 0 - 3 which is modulo'd with 4.")]
                public async Task Quantize(string name) { HandleFilter(Context, mut => mut.Quantize(AllQuantizers[name])); }

                [Command("quantizers", RunMode = RunMode.Async), Summary("Returns all valid inputs for quantizers.")]
                public async Task QuantizerListing()
                {
                    string list = "";

                    foreach (string item in AllQuantizers.Keys)
                        list += item + "\n";

                    await Context.Channel.SendMessageAsync("", false, GetListEmbed("Quantizer Methods", "Valid Methods", list).Build());
                }

                [ExampleAttribute("120", true)]
                [Command("glow", RunMode = RunMode.Async), Summary("Applies a glow to an image.")]
                public async Task Glow(float radius) { HandleFilter(Context, mut => mut.Glow(radius)); }

                [ExampleAttribute("0.50", true)]
                [Command("entropycrop", RunMode = RunMode.Async), Alias("entropy-crop"), Summary("Crops an image to the area of greatest entropy using a given threshold. Defaults to 0.5.")]
                public async Task EntropyCrop(float threshold = 0.5F) { HandleFilter(Context, mut => mut.EntropyCrop(threshold)); }

                [ExampleAttribute("bayer8", true)]
                [Command("basic-dither", RunMode = RunMode.Async), Summary("Applies a binary dithering effect to an image. 13 are available, accessible with its name. Use `pic manip ditherings` to get all valid names.")]
                public async Task BinaryDither(string name)
                {
                    if (!AllDithering.ContainsKey(name.ToLower())) { await ReplyAsync("That's not a dithering I know about..."); return; }
                    HandleFilter(Context, mut => mut.BinaryDither(AllDithering[name.ToLower()]));
                }

                [ExampleAttribute("bayer8 FF000000 FFFFFFFF FFFFF000 FF000FFF", true)]
                [Command("dither", RunMode = RunMode.Async), Summary("Applies a dithering effect to an image. 13 are available, accessible with its name (use `pic manip ditherings` to get all valid names). The full palette is RGBA32 colors as hexadecimal strings.")]
                public async Task Dither(string name, params string[] palette)
                {
                    SixLabors.ImageSharp.Color[] colours = new SixLabors.ImageSharp.Color[palette.Length];
                    for (int i = 0; i < palette.Length; i++)
                        colours[i] = new SixLabors.ImageSharp.Color(Tools.ColorFromHexString(palette[i]));

                    ReadOnlyMemory<SixLabors.ImageSharp.Color> romPalette = colours;

                    if (!AllDithering.ContainsKey(name.ToLower())) { await ReplyAsync("That's not a dithering I know about..."); return; }
                    HandleFilter(Context, mut => mut.Dither(AllDithering[name.ToLower()], romPalette));
                }

                [Command("ditherings", RunMode = RunMode.Async), Summary("Returns all ditherings names.")]
                public async Task DitherListing()
                {
                    string list = "";

                    foreach (string item in AllDithering.Keys)
                        list += item + "\n";

                    await Context.Channel.SendMessageAsync("", false, GetListEmbed("Dithering Methods", "Valid Ditherings", list).Build());
                }
            }

            private static string ApplyFilter(string load, string type, System.Action<IImageProcessingContext> mutation)
            {
                string filename = Util.String.RandomTempFilename() + type;

                using (var img = SLImage.Load(load + type))
                {
                    img.Mutate(mutation);
                    img.Save(filename);
                }

                return filename;
            }

            private static async void HandleFilter(ICommandContext context, System.Action<IImageProcessingContext> mutation)
            {
                using (Util.WorkingBlock wb = new Util.WorkingBlock(context))
                {
                    string loadas = Path.GetTempPath() + "manip";

                    await Util.File.DownloadLastAttachmentAsync(context, loadas, true);
                    string type = Path.GetExtension(Util.File.ReturnLastAttachmentUrl(context));

                    string filename = ApplyFilter(loadas, type, mutation);

                    await context.Channel.SendFileAsync(filename);
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
            [ExampleAttribute("FFFFF000")]
            [Command("rgba"), Summary("Gets RGBA elements from a hex string representing an RGBA32 value.")]
            public async Task GetComponents(string hex)
            {
                Rgba32 colour = ColorFromHexString(hex);

                EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Color", "Colors!", new Discord.Color(colour.R, colour.G, colour.B));
                embed.Fields = new List<EmbedFieldBuilder>()
                {
                    new EmbedFieldBuilder
                    {
                        Name = "RGB",
                        Value = $"{colour.R}, {colour.G}, {colour.B} ({colour.R.ToString("X")}{colour.G.ToString("X")}{colour.B.ToString("X")})",
                        IsInline = false
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "RGBA",
                        Value = $"{colour.R}, {colour.G}, {colour.B}, {colour.A} ({colour.R.ToString("X")}{colour.G.ToString("X")}{colour.B.ToString("X")}{colour.A.ToString("X")})",
                        IsInline = false
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Unpacked RGBA",
                        Value = $"{colour.Rgba} ({colour.Rgba.ToString("X")})",
                        IsInline = false
                    }
                };

                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }

            public static Rgba32 ColorFromHexString(string hexstring)
            {
                return new Rgba32(uint.Parse(hexstring.ToLower().Replace("0x", "").Replace("&h", ""), System.Globalization.NumberStyles.AllowHexSpecifier));
            }
        }
    }
}
