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

namespace xubot_core.src
{
    public class Picture : ModuleBase
    {
        [Group("pic"), Summary("Does shit with images.")]
        public class music_comm : ModuleBase
        {
            [Group("manip"), Summary("Manipulates images. Can be used to deepfry.")]
            public class manipulation : ModuleBase
            {
                [Command("brightness", RunMode = RunMode.Async), Summary("Increases/decreases the brightness of the image.")]
                public async Task ghost(float amt)
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = Path.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Brightness(amt));
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("bw", RunMode = RunMode.Async), Summary("Makes an image black and white.")]
                public async Task bw()
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = Path.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.BlackWhite());
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("colorblind", RunMode = RunMode.Async), Alias("colourblind"), Summary("Applies a filter to simulate colourblindness.")]
                public async Task emuColor(string _type)
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = Path.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        switch (_type.ToLower())
                        {
                            // NO COLOUR
                            case "achromatomaly":
                                {
                                    img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Achromatomaly));
                                    break;
                                }
                            case "part-mono":
                                {
                                    img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Achromatomaly));
                                    break;
                                }

                            case "achromatopsia":
                                {
                                    img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Achromatopsia));
                                    break;
                                }
                            case "mono":
                                {
                                    img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Achromatopsia));
                                    break;
                                }

                            // GREEN
                            case "deuteranomaly":
                                {
                                    img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Deuteranomaly));
                                    break;
                                }
                            case "weak-green":
                                {
                                    img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Deuteranomaly));
                                    break;
                                }

                            case "deuteranopia":
                                {
                                    img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Deuteranopia));
                                    break;
                                }
                            case "blind-green":
                                {
                                    img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Deuteranopia));
                                    break;
                                }

                            // RED
                            case "protanomaly":
                                {
                                    img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Protanomaly));
                                    break;
                                }
                            case "weak-red":
                                {
                                    img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Protanomaly));
                                    break;
                                }

                            case "protanopia":
                                {
                                    img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Protanopia));
                                    break;
                                }
                            case "blind-red":
                                {
                                    img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Protanopia));
                                    break;
                                }

                            // BLUE
                            case "tritanomaly":
                                {
                                    img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Tritanomaly));
                                    break;
                                }
                            case "weak-blue":
                                {
                                    img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Tritanomaly));
                                    break;
                                }

                            case "tritanopia":
                                {
                                    img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Tritanopia));
                                    break;
                                }
                            case "blind-blue":
                                {
                                    img.Mutate(mut => mut.ColorBlindness(ColorBlindnessMode.Tritanopia));
                                    break;
                                }
                            default: break;
                        }

                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("colorblind?list", RunMode = RunMode.Async), Alias("colourblind?list"), Summary("Lists the colourblindness filters.")]
                public async Task emuColorList()
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
                                Value = "**Achromatomaly** (part-mono)\n**Achromatopsia** (mono)`" ,
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
                public async Task contrast(float amt)
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = Path.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Contrast(amt));
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("hue-rotate", RunMode = RunMode.Async), Summary("Shifts/rotates the hues of an image by a given amount of degrees.")]
                public async Task huerotate(float deg)
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = Path.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Hue(deg));
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("invert", RunMode = RunMode.Async), Summary("iNVERTS THE COLORS OF AN IMAGE.")]
                public async Task invert()
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = Path.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Invert());
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("kodachrome", RunMode = RunMode.Async), Summary("Applies a kodachrome filter to an image.")]
                public async Task kodachrome()
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = Path.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Kodachrome());
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("lomograph", RunMode = RunMode.Async), Summary("Applies a lomograph filter to an image.")]
                public async Task lomo()
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = Path.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Lomograph());
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("polaroid", RunMode = RunMode.Async), Summary("Applies a polaroid filter to an image.")]
                public async Task polaroid()
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = Path.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Polaroid());
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("saturate", RunMode = RunMode.Async), Summary("Increases/decreases the saturation of an image.")]
                public async Task sat(float amount)
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = Path.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Saturate(amount));
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }

                [Command("sepia", RunMode = RunMode.Async), Summary("Applies a sepia filter to an image.")]
                public async Task sepia()
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    string type = Path.GetExtension(GeneralTools.ReturnAttachmentURL(Context));

                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Sepia());
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                    }

                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                }
            }
        }
    }
}
