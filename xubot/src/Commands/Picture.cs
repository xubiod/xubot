using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Dithering;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using xubot.Attributes;
using Color = Discord.Color;
using SLImage = SixLabors.ImageSharp.Image;

namespace xubot.Commands
{
    [Group("pic"), Summary("Does shit with images.")]
    public class Picture : ModuleBase
    {
        // ReSharper disable once StringLiteralTypo
        [Group("manip"), Summary("Manipulates images. Can be used to deep-fry.")]
        public class Manipulation : ModuleBase
        {
            // ReSharper disable once IdentifierTypo
            private static readonly Dictionary<string, IQuantizer> AllQuantizer = new()
            {
                { "websafe", KnownQuantizers.WebSafe }, { "web", KnownQuantizers.WebSafe },
                { "web-safe", KnownQuantizers.WebSafe },
                { "werner", KnownQuantizers.Werner }, { "1821", KnownQuantizers.Werner },
                { "wu", KnownQuantizers.Wu }, { "xiaolin-wu", KnownQuantizers.Wu }, { "high", KnownQuantizers.Wu },
                { "highquality", KnownQuantizers.Wu }, { "high-quality", KnownQuantizers.Wu },
                { "octree", KnownQuantizers.Octree }, { "fast", KnownQuantizers.Octree },
                { "adaptive", KnownQuantizers.Octree }, { "f", KnownQuantizers.Octree }
            };

            private static readonly Dictionary<string, IDither> AllDithering =
                new()
                {
                    { "atkinson", KnownDitherings.Atkinson }, { "bayer2", KnownDitherings.Bayer2x2 },
                    { "bayer4", KnownDitherings.Bayer4x4 }, { "bayer8", KnownDitherings.Bayer8x8 },
                    { "burks", KnownDitherings.Burks },
                    { "floydsteinberg", KnownDitherings.FloydSteinberg },
                    { "jarvisjudiceninke", KnownDitherings.JarvisJudiceNinke },
                    { "ordered3", KnownDitherings.Ordered3x3 }, { "sierra2", KnownDitherings.Sierra2 },
                    { "sierra3", KnownDitherings.Sierra3 },
                    { "sierralite", KnownDitherings.SierraLite }, { "stevensonarce", KnownDitherings.StevensonArce },
                    { "stucki", KnownDitherings.Stucki }
                };

            private static readonly Dictionary<string, ColorBlindnessMode> ColourBlindnessMap =
                new()
                {
                    { "achromatomaly", ColorBlindnessMode.Achromatomaly },
                    { "part-mono", ColorBlindnessMode.Achromatomaly },
                    { "weak-color", ColorBlindnessMode.Achromatomaly },
                    { "color-weak", ColorBlindnessMode.Achromatomaly },
                    { "weak-colour", ColorBlindnessMode.Achromatomaly },
                    { "colour-weak", ColorBlindnessMode.Achromatomaly },
                    { "achromatopsia", ColorBlindnessMode.Achromatopsia }, { "mono", ColorBlindnessMode.Achromatopsia },
                    { "no-color", ColorBlindnessMode.Achromatopsia },
                    { "blind-color", ColorBlindnessMode.Achromatopsia },
                    { "color-blind", ColorBlindnessMode.Achromatopsia },
                    { "no-colour", ColorBlindnessMode.Achromatopsia },
                    { "blind-colour", ColorBlindnessMode.Achromatopsia },
                    { "colour-blind", ColorBlindnessMode.Achromatopsia },
                    { "deuteranomaly", ColorBlindnessMode.Deuteranomaly },
                    { "weak-green", ColorBlindnessMode.Deuteranomaly },
                    { "green-weak", ColorBlindnessMode.Deuteranomaly },
                    { "deuteranopia", ColorBlindnessMode.Deuteranopia },
                    { "blind-green", ColorBlindnessMode.Deuteranopia },
                    { "green-blind", ColorBlindnessMode.Deuteranopia },
                    { "protanomaly", ColorBlindnessMode.Protanomaly }, { "weak-red", ColorBlindnessMode.Protanomaly },
                    { "red-weak", ColorBlindnessMode.Protanomaly },
                    { "protanopia", ColorBlindnessMode.Protanopia }, { "blind-red", ColorBlindnessMode.Protanopia },
                    { "red-blind", ColorBlindnessMode.Protanopia },
                    { "tritanomaly", ColorBlindnessMode.Tritanomaly }, { "weak-blue", ColorBlindnessMode.Tritanomaly },
                    { "blue-weak", ColorBlindnessMode.Tritanomaly },
                    { "tritanopia", ColorBlindnessMode.Tritanopia }, { "blind-blue", ColorBlindnessMode.Tritanopia },
                    { "blue-blind", ColorBlindnessMode.Tritanopia }
                };

            [Example(true)]
            [Command("bw", RunMode = RunMode.Async), Summary("Makes an image black and white.")]
            public async Task Bw()
            {
                await HandleFilter(Context, mut => mut.BlackWhite());
            }

            [Example(true)]
            // ReSharper disable once StringLiteralTypo
            [Command("invert", RunMode = RunMode.Async), Summary("iNVERTS THE COLORS OF AN IMAGE.")]
            public async Task Invert()
            {
                await HandleFilter(Context, mut => mut.Invert());
            }

            [Example(true)]
            [Command("kodachrome", RunMode = RunMode.Async), Summary("Applies a kodachrome filter to an image.")]
            public async Task Kodachrome()
            {
                await HandleFilter(Context, mut => mut.Kodachrome());
            }

            [Example(true)]
            [Command("lomograph", RunMode = RunMode.Async), Summary("Applies a lomograph filter to an image.")]
            public async Task Lomograph()
            {
                await HandleFilter(Context, mut => mut.Lomograph());
            }

            [Example(true)]
            [Command("polaroid", RunMode = RunMode.Async), Summary("Applies a polaroid filter to an image.")]
            public async Task Polaroid()
            {
                await HandleFilter(Context, mut => mut.Polaroid());
            }

            [Example(true)]
            [Command("sepia", RunMode = RunMode.Async), Summary("Applies a sepia filter to an image.")]
            public async Task Sepia()
            {
                await HandleFilter(Context, mut => mut.Sepia());
            }

            [Example(true)]
            [Command("oil-paint", RunMode = RunMode.Async), Summary("Applies a basic oil-paint filter to an image.")]
            public async Task OilPaint()
            {
                await HandleFilter(Context, mut => mut.OilPaint());
            }

            [Example(true)]
            [Command("vignette", RunMode = RunMode.Async), Summary("Applies a basic vignette to an image.")]
            public async Task Vignette()
            {
                await HandleFilter(Context, mut => mut.Vignette());
            }

            [Example(true)]
            [Command("glow", RunMode = RunMode.Async), Summary("Applies a basic glow to an image.")]
            public async Task Glow()
            {
                await HandleFilter(Context, mut => mut.Glow());
            }

            [Group("blur"), Summary("Different blur and sharpen effects.")]
            public class Blur : ModuleBase
            {
                [Example(true)]
                [Command("bokeh", RunMode = RunMode.Async), Summary("Applies a basic bokeh blur to an image.")]
                public async Task Bokeh()
                {
                    await HandleFilter(Context, mut => mut.BokehBlur());
                }

                [Example("4 4 1.5", true)]
                [Command("bokeh", RunMode = RunMode.Async), Summary("Applies bokeh blur to an image.")]
                public async Task Bokeh(int radius, int kernelCount, float gamma)
                {
                    await HandleFilter(Context, mut => mut.BokehBlur(radius, kernelCount, gamma));
                }

                [Example(true)]
                [Command("box", RunMode = RunMode.Async), Summary("Applies a basic box blur to an image.")]
                public async Task Box()
                {
                    await HandleFilter(Context, mut => mut.BoxBlur());
                }

                [Example("4", true)]
                [Command("box", RunMode = RunMode.Async), Summary("Applies box blur to an image.")]
                public async Task Box(int radius)
                {
                    await HandleFilter(Context, mut => mut.BoxBlur(radius));
                }

                [Example(true)]
                [Command("gaussian", RunMode = RunMode.Async), Summary("Applies a basic Gaussian blur to an image.")]
                public async Task Gaussian()
                {
                    await HandleFilter(Context, mut => mut.GaussianBlur());
                }

                [Example("2.5", true)]
                [Command("gaussian", RunMode = RunMode.Async), Summary("Applies Gaussian blur to an image.")]
                public async Task Gaussian(float weight)
                {
                    await HandleFilter(Context, mut => mut.GaussianBlur(weight));
                }

                [Example(true)]
                [Command("gaussian-sharp", RunMode = RunMode.Async),
                 Summary("Applies a basic Gaussian sharpen to an image.")]
                public async Task GaussianSharp()
                {
                    await HandleFilter(Context, mut => mut.GaussianSharpen());
                }

                [Example("2.5", true)]
                [Command("gaussian-sharp", RunMode = RunMode.Async),
                 Summary("Applies a basic Gaussian sharpen to an image.")]
                public async Task GaussianSharp(float weight)
                {
                    await HandleFilter(Context, mut => mut.GaussianSharpen(weight));
                }
            }

            [Group("params"), Alias("p"), Summary("Filters with parameters.")]
            public class Advanced : ModuleBase
            {
                [Example("1.50", true)]
                [Command("brightness", RunMode = RunMode.Async),
                 Summary("Increases/decreases the brightness of the image.")]
                public async Task Brightness(float amt)
                {
                    await HandleFilter(Context, mut => mut.Brightness(amt));
                }

                [Example("blue-blind", true)]
                [Command("colorblind", RunMode = RunMode.Async), Alias("colourblind"),
                 Summary("Applies a filter to simulate colourblindness.")]
                public async Task EmulateColourblindness(string type)
                {
                    type = type.ToLower();

                    if (!ColourBlindnessMap.ContainsKey(type))
                    {
                        await ReplyAsync("I wasn't given a valid filter name...");
                        return;
                    }

                    await HandleFilter(Context, mut => mut.ColorBlindness(ColourBlindnessMap[type]));
                }

                [Command("colorblind?list", RunMode = RunMode.Async), Alias("colourblind?list"),
                 Summary("Lists the colourblindness filters.")]
                public async Task ColourblindnessList()
                {
                    EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Colourblind Filter List",
                        "All the filters for the colourblindness emulation.", Color.Magenta);
                    embed.Fields = new List<EmbedFieldBuilder>
                    {
                        new()
                        {
                            Name = "Total Colour Blindness",
                            Value = "**Achromatomaly** (part-mono)\n**Achromatopsia** (mono)",
                            IsInline = true
                        },
                        new()
                        {
                            Name = "Red-Green Colour Deficiency (Low/No Green Cones)",
                            Value = "**Deuteranomaly** (weak-green)\n**Deuteranopia** (blind-green)",
                            IsInline = true
                        },
                        new()
                        {
                            Name = "Red-Green Colour Deficiency (Low/No Red Cones)",
                            Value = "**Protanomaly** (weak-red)\n**Protanopia** (blind-red)",
                            IsInline = true
                        },
                        new()
                        {
                            Name = "Blue-Yellow Colour Deficiency (Low/No Blue Cones)",
                            Value = "**Tritanomaly** (weak-blue)\n**Tritanopia** (blind-blue)",
                            IsInline = true
                        }
                    };

                    await ReplyAsync("", false, embed.Build());
                }

                [Example("2.00", true)]
                [Command("contrast", RunMode = RunMode.Async), Summary("Increases/decreases the contrast of an image.")]
                public async Task Contrast(float amt)
                {
                    await HandleFilter(Context, mut => mut.Contrast(amt));
                }

                [Example("180.0", true)]
                [Command("hue-rotate", RunMode = RunMode.Async),
                 Summary("Shifts/rotates the hues of an image by a given amount of degrees.")]
                public async Task RotateHue(float deg)
                {
                    await HandleFilter(Context, mut => mut.Hue(deg));
                }

                [Example("2.00", true)]
                [Command("saturate", RunMode = RunMode.Async),
                 Summary("Increases/decreases the saturation of an image.")]
                public async Task Saturate(float amount)
                {
                    await HandleFilter(Context, mut => mut.Saturate(amount));
                }

                [Example("0.50", true)]
                [Command("threshold", RunMode = RunMode.Async), Summary("Applies a binary threshold to an image.")]
                public async Task BinaryThreshold(float threshold)
                {
                    await HandleFilter(Context, mut => mut.BinaryThreshold(threshold));
                }

                [Example("3 6", true)]
                [Command("oil-paint", RunMode = RunMode.Async), Summary("Applies an oil paint filter to an image.")]
                public async Task OilPaint(int levels, int brushSize)
                {
                    await HandleFilter(Context, mut => mut.OilPaint(levels, brushSize));
                }

                [Example("8", true)]
                [Command("pixelate", RunMode = RunMode.Async), Summary("Applies a pixelate filter to an image.")]
                public async Task Pixelate(int pixelSize)
                {
                    await HandleFilter(Context, mut => mut.Pixelate(pixelSize));
                }

                [Example("720.0 480.0", true)]
                [Command("vignette", RunMode = RunMode.Async), Summary("Applies a vignette to an image.")]
                public async Task Vignette(float radiusX, float radiusY)
                {
                    await HandleFilter(Context, mut => mut.Vignette(radiusX, radiusY));
                }

                [Example("fast", true)]
                [Command("quantize", RunMode = RunMode.Async),
                 Summary(
                     "Applies a quantize filter to an image. 4 are available, accessible with 0 - 3 which is modulo with 4.")]
                public async Task Quantize(string name)
                {
                    await HandleFilter(Context, mut => mut.Quantize(AllQuantizer[name]));
                }

                [Command("quantize", RunMode = RunMode.Async), Summary("Returns all valid inputs to quantize.")]
                public async Task QuantizeListing()
                {
                    string list = "";

                    foreach (string item in AllQuantizer.Keys)
                        list += item + "\n";

                    await Context.Channel.SendMessageAsync("", false,
                        GetListEmbed("Quantize Methods", "Valid Methods", list).Build());
                }

                [Example("120", true)]
                [Command("glow", RunMode = RunMode.Async), Summary("Applies a glow to an image.")]
                public async Task Glow(float radius)
                {
                    await HandleFilter(Context, mut => mut.Glow(radius));
                }

                [Example("0.50", true)]
                [Command("entropy-crop", RunMode = RunMode.Async),
                 Summary("Crops an image to the area of greatest entropy using a given threshold. Defaults to 0.5.")]
                public async Task EntropyCrop(float threshold = 0.5F)
                {
                    await HandleFilter(Context, mut => mut.EntropyCrop(threshold));
                }

                [Example("bayer8", true)]
                [Command("basic-dither", RunMode = RunMode.Async),
                 Summary(
                     "Applies a binary dithering effect to an image. 13 are available, accessible with its name. Use `pic manip dithering-pattern` to get all valid names.")]
                public async Task BinaryDither(string name)
                {
                    if (!AllDithering.ContainsKey(name.ToLower()))
                    {
                        await ReplyAsync("That's not a dithering I know about...");
                        return;
                    }

                    await HandleFilter(Context, mut => mut.BinaryDither(AllDithering[name.ToLower()]));
                }

                [Example("bayer8 FF000000 FFFFFFFF FFFFF000 FF000FFF", true)]
                [Command("dither", RunMode = RunMode.Async),
                 Summary(
                     "Applies a dithering effect to an image. 13 are available, accessible with its name (use `pic manip dithering-pattern` to get all valid names). The full palette is RGBA32 colors as hexadecimal strings.")]
                public async Task Dither(string name, params string[] palette)
                {
                    SixLabors.ImageSharp.Color[] colours = new SixLabors.ImageSharp.Color[palette.Length];
                    for (int i = 0; i < palette.Length; i++)
                        colours[i] = new SixLabors.ImageSharp.Color(Tools.ColorFromHexString(palette[i]));

                    ReadOnlyMemory<SixLabors.ImageSharp.Color> romPalette = colours;

                    if (!AllDithering.ContainsKey(name.ToLower()))
                    {
                        await ReplyAsync("That's not a dithering I know about...");
                        return;
                    }

                    await HandleFilter(Context, mut => mut.Dither(AllDithering[name.ToLower()], romPalette));
                }

                [Command("dithering-pattern", RunMode = RunMode.Async), Summary("Returns all dithering pattern names.")]
                public async Task DitherListing()
                {
                    string list = "";

                    foreach (string item in AllDithering.Keys)
                        list += item + "\n";

                    await Context.Channel.SendMessageAsync("", false,
                        GetListEmbed("Dithering Methods", "Valid Dithering Patterns", list).Build());
                }
            }

            private static string ApplyFilter(string load, string type, Action<IImageProcessingContext> mutation)
            {
                string filename = Util.String.RandomTempFilename() + type;

                using var img = SLImage.Load(load + type);
                img.Mutate(mutation);
                img.Save(filename);

                return filename;
            }

            private static async Task<IUserMessage> HandleFilter(ICommandContext context,
                Action<IImageProcessingContext> mutation)
            {
                using Util.WorkingBlock wb = new Util.WorkingBlock(context);
                string loadAs = Path.GetTempPath() + "manip";

                await Util.File.DownloadLastAttachmentAsync(context, loadAs, true);
                string type = Path.GetExtension(Util.File.ReturnLastAttachmentUrl(context));

                string filename = ApplyFilter(loadAs, type, mutation);

                return await context.Channel.SendFileAsync(filename);
            }

            private static EmbedBuilder GetListEmbed(string title, string name, string list)
            {
                return new EmbedBuilder
                {
                    Title = title,
                    Color = Color.Blue,
                    Description = "So you don't need to look at the source!",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = Util.Globals.EmbedFooter
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>
                    {
                        new()
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
            [Example("FFFFF000")]
            [Command("rgba"), Summary("Gets RGBA elements from a hex string representing an RGBA32 value.")]
            public async Task GetComponents(string hex)
            {
                Rgba32 colour = ColorFromHexString(hex);

                EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Color", "Colors!",
                    new Color(colour.R, colour.G, colour.B));
                embed.Fields = new List<EmbedFieldBuilder>
                {
                    new()
                    {
                        Name = "RGB",
                        Value = $"{colour.R}, {colour.G}, {colour.B} ({colour.R:X}{colour.G:X}{colour.B:X})",
                        IsInline = false
                    },
                    new()
                    {
                        Name = "RGBA",
                        Value =
                            $"{colour.R}, {colour.G}, {colour.B}, {colour.A} ({colour.R:X}{colour.G:X}{colour.B:X}{colour.A:X})",
                        IsInline = false
                    },
                    new()
                    {
                        Name = "Unpacked RGBA",
                        Value = $"{colour.Rgba} ({colour.Rgba:X})",
                        IsInline = false
                    }
                };

                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }

            public static Rgba32 ColorFromHexString(string hexString)
            {
                return new Rgba32(uint.Parse(hexString.ToLower().Replace("0x", "").Replace("&h", ""),
                    NumberStyles.AllowHexSpecifier));
            }
        }
    }
}