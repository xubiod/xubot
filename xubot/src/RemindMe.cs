using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;

namespace xubot.src
{
    public class RemindMe : ModuleBase
    {
        [Command("remind", RunMode = RunMode.Async)]
        public async Task remindSet(string input, string msg)
        {
            int _sec = TimeParser(input);
            DateTime _rn = DateTime.Now;
            DateTime _remindAt = _rn.AddSeconds(_sec);

            if (_sec < 1209600)
            {
                await ReplyAsync("I'll try to remind you at **" + _remindAt.ToShortDateString() + " " + _remindAt.ToLongTimeString() + "** about *" + msg + "*.");
                Thread.Sleep(_sec * 1000);
                Discord.IDMChannel iDM = await Context.Message.Author.GetOrCreateDMChannelAsync();
                await iDM.SendMessageAsync(msg);

                //pipe shit
                //microsoft documentation
                /*Process pipeClient = new Process();

                pipeClient.StartInfo.FileName = "xubot-reminder-client_framework.exe";

                using (AnonymousPipeServerStream pipeServer =
                    new AnonymousPipeServerStream(PipeDirection.Out,
                    HandleInheritability.Inheritable))
                {
                    Console.WriteLine("[SERVER] Current TransmissionMode: {0}.",
                        pipeServer.TransmissionMode);

                    // Pass the client process a handle to the server.
                    pipeClient.StartInfo.Arguments =
                        pipeServer.GetClientHandleAsString();
                    Console.WriteLine("[SERVER] Arguments set");
                    pipeClient.StartInfo.UseShellExecute = false;
                    Console.WriteLine("[SERVER] Shell execute to false");
                    pipeClient.Start();
                    Console.WriteLine("[SERVER] Client should have started");

                    pipeServer.DisposeLocalCopyOfClientHandle();

                    try
                    {
                        // Read user input and send that to the client process.
                        using (StreamWriter sw = new StreamWriter(pipeServer))
                        {
                            sw.AutoFlush = true;
                            // Send a 'sync message' and wait for client to receive it.
                            sw.WriteLine("SYNC");
                            pipeServer.WaitForPipeDrain();

                            // Send the seconds of the reminder
                            sw.WriteLine("SEC:" + _sec);
                            pipeServer.WaitForPipeDrain();

                            // Send user ID
                            sw.WriteLine("ID:" + Context.Message.Author.Id);
                            pipeServer.WaitForPipeDrain();

                            // Send msg
                            sw.WriteLine("MSG:" + msg);
                            pipeServer.WaitForPipeDrain();
                        }
                        // confirmation

                        await ReplyAsync("I'll try to remind you at **" + _remindAt.ToShortDateString() + " " + _remindAt.ToLongTimeString() + "**.");

                        using (StreamReader sr = new StreamReader(pipeServer))
                        {
                            string temp = "";
                            do
                            {
                                temp = sr.ReadLine();
                            } while (temp != null);

                            Discord.IDMChannel iDM = await Context.Message.Author.GetOrCreateDMChannelAsync();
                            await iDM.SendMessageAsync(temp);
                        }
                    }
                    // Catch the IOException that is raised if the pipe is broken
                    // or disconnected.
                    
                    catch (IOException e)
                    {
                        await GeneralTools.CommHandler.BuildError(e, Context);
                    }
                }*/
            }
            else
            {
                await ReplyAsync("I can only remind you at a fortnight (2 weeks) or less.");
            }
        }
        
        //[Group("stickynote")]
        
        public int TimeParser(string parseme)
        {
            int _seconds = -1;
            string _parsed_n = "";
            char[] _all_c = parseme.ToArray();

            foreach (char _ in _all_c)
            {
                if (Char.IsDigit(_)) {
                    _parsed_n += _;
                }
            }

            if (parseme.Contains("hour") || parseme.Contains("hr"))
            {
                _seconds = Convert.ToInt32(_parsed_n) * 3600;
            }
            else if (parseme.Contains("min") || parseme.Contains("m"))
            {
                _seconds = Convert.ToInt32(_parsed_n) * 60;
            }
            else if (parseme.Contains("sec") || parseme.Contains("s"))
            {
                _seconds = Convert.ToInt32(_parsed_n);
            }

            return _seconds;
        }
    }
}
