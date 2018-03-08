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
                [Command]
                public async Task read()
                {
                    var attach = Context.Message.Attachments;
                    IAttachment attached = null;

                    foreach (var tempAttachment in attach)
                    {
                        attached = tempAttachment;
                    }

                    new Thread(new ThreadStart(async () =>
                    {
                        var msg = await ReplyAsync("Please wait, this takes a while to read images.");

                        Random rnd = new Random();
                        int rnd_res = rnd.Next(99999);
                        string[] imgSplit = attached.ToString().Split('.');
                        string fileType = imgSplit[imgSplit.Length - 1];
                        fileType = "." + fileType;

                        System.Drawing.Image imgFromStream = null;

                        using (HttpClient client = new HttpClient())
                        using (HttpResponseMessage response = await client.GetAsync(attached.Url))
                        using (HttpContent content = response.Content)
                        {
                            imgFromStream = System.Drawing.Image.FromStream(await content.ReadAsStreamAsync());
                            Bitmap bitmap = (Bitmap)imgFromStream;

                            string fileDir = "C:\\tmp\\" + rnd_res.ToString() + fileType;
                            bitmap.Save(fileDir);

                            var Ocr = new AutoOcr();
                            var Result = Ocr.Read(fileDir);
                            Console.WriteLine(Result.Text);

                            await msg.DeleteAsync();
                            await ReplyAsync("**(Automatic) Iron OCR returned:\n** " + Result);
                        }
                    })).Start();
                }

                [Command]
                public async Task read(string img)
                {
                    new Thread(new ThreadStart(async () =>
                    {
                        var msg = await ReplyAsync("Please wait, this takes a while to read images.");

                        Random rnd = new Random();
                        int rnd_res = rnd.Next(99999);
                        string[] imgSplit = img.Split('.');
                        string fileType = imgSplit[imgSplit.Length - 1];
                        fileType = "." + fileType;

                        System.Drawing.Image imgFromStream = null;

                        using (HttpClient client = new HttpClient())
                        using (HttpResponseMessage response = await client.GetAsync(img))
                        using (HttpContent content = response.Content)
                        {
                            imgFromStream = System.Drawing.Image.FromStream(await content.ReadAsStreamAsync());
                            Bitmap bitmap = (Bitmap)imgFromStream;

                            string fileDir = "C:\\tmp\\" + rnd_res.ToString() + fileType;
                            bitmap.Save(fileDir);

                            var Ocr = new AutoOcr();
                            var Result = Ocr.Read(fileDir);
                            Console.WriteLine(Result.Text);

                            await msg.DeleteAsync();
                            await ReplyAsync("**(Automatic) Iron OCR returned:\n** " + Result);
                        }
                    })).Start();
                }
            }
        }

        [Group("file")]
        public class file_comm : ModuleBase
        {
            [Command("sendback")]
            public async Task sendbackPic(string img)
            {
                Random rnd = new Random();
                int rnd_res = rnd.Next(99999);
                string[] imgSplit = img.Split('.');
                string fileType = imgSplit[imgSplit.Length - 1];
                fileType = "." + fileType;

                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(img))
                using (HttpContent content = response.Content)
                {
                    FileStream fs = new FileStream("C:\\tmp\\" + rnd_res.ToString() + fileType, FileMode.Create, FileAccess.ReadWrite);
                    byte[] byteBlock = await content.ReadAsByteArrayAsync();

                    fs.Write(await content.ReadAsByteArrayAsync(), 0, byteBlock.Length);
                    fs.Flush();
                    fs.Close();
                }

                await Context.Channel.SendFileAsync("C:\\tmp\\" + rnd_res.ToString() + fileType);
            }

            [Command("corrupt")]
            public async Task corrupt(string img)
            {
                Random rnd = new Random();
                int rnd_res = rnd.Next(99999);
                string[] imgSplit = img.Split('.');
                string fileType = imgSplit[imgSplit.Length - 1];
                fileType = "." + fileType;

                Random _r = new Random();
                int breakAmount;
                byte[] byteTo = new byte[1];

                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(img))
                using (HttpContent content = response.Content)
                {
                    FileStream fs = new FileStream("C:\\tmp\\" + rnd_res.ToString() + fileType, FileMode.Create, FileAccess.ReadWrite);
                    byte[] byteBlock = await content.ReadAsByteArrayAsync();
                    breakAmount = _r.Next(byteBlock.Length);

                    for (int k = 0; k <= breakAmount; k++)
                    {
                        _r.NextBytes(byteTo);
                        int b = _r.Next(byteBlock.Length - 1);
                        byteBlock[b] = byteTo[0];
                    }

                    fs.Write(byteBlock, 0, byteBlock.Length);
                    fs.Flush();
                    fs.Close();
                }

                await Context.Channel.SendFileAsync("C:\\tmp\\" + rnd_res.ToString() + fileType, "Attempted break amount (bytes): " + breakAmount);
            }

            //[Command("compress")]
            public async Task compress()
            {
                var attach = Context.Message.Attachments;
                IAttachment attached = null;

                foreach (var tempAttachment in attach)
                {
                    attached = tempAttachment;
                }

                string img = attached.Url;

                new Thread(new ThreadStart(async () =>
                {
                    Random rnd = new Random();
                    int rnd_res = rnd.Next(99999);
                    string[] imgSplit = img.Split('.');
                    string fileType = imgSplit[imgSplit.Length - 1];
                    fileType = "." + fileType;

                    using (HttpClient client = new HttpClient())
                    using (HttpResponseMessage response = await client.GetAsync(img))
                    using (HttpContent content = response.Content)
                    {
                        FileStream fs = new FileStream("C:\\tmp\\" + rnd_res.ToString() + fileType, FileMode.Create, FileAccess.ReadWrite);
                        byte[] byteBlock = await content.ReadAsByteArrayAsync();

                        using (GZipStream zip = new GZipStream(fs, CompressionLevel.Optimal))
                        {

                        }

                        fs.Write(await content.ReadAsByteArrayAsync(), 0, byteBlock.Length);
                        fs.Flush();
                        fs.Close();
                    }

                    await Context.Channel.SendFileAsync("C:\\tmp\\" + rnd_res.ToString() + fileType);
                })).Start();
            }

        }
    }
}