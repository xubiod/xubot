using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket;
using Renci.SshNet;
using System.IO;
using System.Threading;
using System.Xml.Linq;

#pragma warning disable CS4014

namespace xubot
{
    public class SSH : ModuleBase
    {
        public static SshClient xuSSH;
        public static ShellStream xuSSHStream;

        [Group("ssh")]
        public class commands : ModuleBase
        {
            [Command("connect")]
            public async Task connectSSH(string host, int port, string user, string password)
            {
                xuSSH = new SshClient(host, port, user, password);
                xuSSH.Connect();

                xuSSHStream = xuSSH.CreateShellStream("xubot", 40, 60, 60, 40, 0);

                await ReplyAsync("SSH client set and connected.");
            }

            [Command("quick-con"), Alias("qc")]
            public async Task quick(string hostnick)
            {
                string reply = "That quick connection doesn't exist.";
                bool exists = false;

                var xdoc = XDocument.Load("SSHQuickConnect.xml");

                var items = from i in xdoc.Descendants("connection")
                            select new
                            {
                                nick = (string)i.Attribute("nick"),
                                host = (string)i.Attribute("host"),
                                port = (int)i.Attribute("port"),
                                user = (string)i.Attribute("user"),
                                password = (string)i.Attribute("password")
                            };

                foreach (var item in items)
                {
                    if (item.nick.ToLower() == hostnick.ToLower())
                    {
                        exists = true;
                        connectSSH(item.host, item.port, item.user, item.password);
                    }
                }

                if (!exists) { await ReplyAsync(reply); }
            }

            [Command("send")]
            public async Task sendSSH(string cmd)
            {
                Task.Run(async () =>
                {
                    //adapted from wamwoowam's bot
                    xuSSHStream.WriteLine(cmd);
                    StringBuilder ret = new StringBuilder();
                    while (true)
                    {
                        try
                        {
                            //xuSSHStream.
                            Thread.Sleep(1000);
                            string str = xuSSHStream.ReadLine(TimeSpan.FromSeconds(1));
                            if (str != null)
                            {
                                if (str.Trim() != cmd)
                                {
                                    ret.Append(str.TrimEnd() + "\r\n");
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch
                        {
                            ret.Append("...fuck.");
                            break;
                        }
                    }
                    string _eval = cmd;
                    string _result = ret.ToString();
                    await ReplyAsync("", false, CompileTools.BuildEmbed("Bash", "Using SSH", "bash", cmd, _result));
                });
            }
        }
    }
}
