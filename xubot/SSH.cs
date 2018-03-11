using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket;
using Renci.SshNet;

namespace xubot
{
    public class SSH : ModuleBase
    {
        public static SshClient xuSSH;

        [Group("ssh")]
        public class commands : ModuleBase
        {
            [Command("connect")]
            public async Task connectSSH(string host, int port, string user, string password)
            {
                xuSSH = new SshClient(host, port, user, password);
                xuSSH.Connect();
                await ReplyAsync("SSH client set and connected.");
            }

            [Command("send")]
            public async Task sendSSH(string command)
            {
                xuSSH.RunCommand(command);
            }
        }
    }
}
