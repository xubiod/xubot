using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace xubot_reminder_client
{
    class Program
    {
        static void Main(string[] args)
        {
            int _seconds = 0;
            int _id = 0;
            string _message = "";

            if (args.Length > 0)
            {
                using (PipeStream pipeClient =
                    new AnonymousPipeClientStream(PipeDirection.In, args[0]))
                {
                    Console.WriteLine("[CLIENT] Current TransmissionMode: {0}.",
                       pipeClient.TransmissionMode);

                    using (StreamReader sr = new StreamReader(pipeClient))
                    {
                        // Display the read text to the console
                        string temp;

                        // Wait for 'sync message' from the server.
                        do
                        {
                            Console.WriteLine("[CLIENT] Wait for sync...");
                            temp = sr.ReadLine();
                        }
                        while (!temp.StartsWith("SYNC"));

                        // Read the server data and echo to the console.
                        while ((temp = sr.ReadLine()) != null)
                        {
                            if (temp.StartsWith("SEC") || temp.StartsWith("ID"))
                            {
                                char[] _in = temp.ToCharArray();
                                string _assem = "";

                                foreach (char _ in _in)
                                {
                                    if (Char.IsNumber(_)) _assem += _;
                                }

                                if (temp.StartsWith("SEC"))
                                {
                                    _seconds = Convert.ToInt32(_assem);
                                }
                                else if (temp.StartsWith("ID"))
                                {
                                    _id = Convert.ToInt32(_assem);
                                }
                            }
                            else
                            {
                                _message = temp.Replace("MSG:", "");
                            }
                        }
                    }

                    uint ms = (uint)(_seconds * 1000);

                    Thread.Sleep((int)ms);

                    using (StreamWriter sw = new StreamWriter(pipeClient))
                    {
                        sw.AutoFlush = true;

                        sw.WriteLine(_message);
                    }
                }
            }
        }
    }
}
