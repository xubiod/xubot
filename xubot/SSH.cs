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
                switch (hostnick)
                {
                    case "ironlake": await connectSSH("ironlake.online", 22, "xubot", "gaygaygay"); break;
                }
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
