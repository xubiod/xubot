using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentFTP;
using Newtonsoft.Json.Linq;
using System.IO.Compression;
using Ionic.Zip;
using System.Diagnostics;

namespace xubot_launcher
{
    class Program
    {
        public static dynamic keys;

        static void Main(string[] args)
        {
            string currentDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            keys = JObject.Parse(File.ReadAllText(Path.Combine(currentDir, "data.json")));

            Console.WriteLine("* xubot launcher + updater * * *");
            Console.WriteLine();
            Console.WriteLine("logging into local update server via FTP on port 22...");
            
            FtpClient client = new FtpClient(keys.uri.ToString(), keys.user.ToString(), keys.pass.ToString());
            client.Connect();

            Console.WriteLine("connection successful: logged in as " + client.Credentials.UserName);
            Console.WriteLine();

            if (client.FileExists(keys.location.ToString() + "/xubot_win7.zip"))
            {
                Console.WriteLine("cleaning");
                Directory.Delete("./xubot");
                File.Delete("./xubot_win7.zip");

                Console.WriteLine("downloading update");
                client.DownloadFile(currentDir + "/xubot_win7.zip", keys.location.ToString() + "/xubot_win7.zip",true);

                Console.WriteLine("download complete, renaming remote file");
                client.MoveFile(keys.location.ToString() + "/xubot_win7.zip", keys.location.ToString() + "/xubot_win7_OLD.zip");

                Console.WriteLine("disconnecting");
                client.Disconnect();


                // unpack file

                string zipPath = currentDir + "/xubot_win7.zip";
                string extractPath = currentDir + "/xubot";

                Console.WriteLine("unpacking file");

                using (ZipFile zip = ZipFile.Read(zipPath))
                {
                    zip.ExtractAll(extractPath);
                }

                Console.WriteLine("file extracted, launching xubot");

            } else
            {
                Console.WriteLine(keys.location.ToString() + "/xubot_win7.zip not found, probably latest build, disconnecting");
                client.Disconnect();
            }

            Process.Start(currentDir + "\\xubot\\Debug\\xubot.exe");
        }
    }
}
