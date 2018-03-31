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
        }
    }
}
