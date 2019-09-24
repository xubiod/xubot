using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot.src
{
    public static class SmallLangInterps
    {
        //adapted from https://esolangs.org/wiki/Deadfish#C.23
        public class Deadfish
        {
            private static string output = "";

            private static int cell = 0;

            public static string Execute(string input)
            {
                input = input.Replace(((char)13).ToString(), "");

                output = "";
                cell = 0;

                foreach (char c in input)
                {
                    if (c == 'i')
                    {
                        cell++;
                    }
                    else if (c == 'd')
                    {
                        cell--;
                    }
                    else if (c == 's')
                    {
                        int i = cell * cell;
                        cell = i;
                    }
                    else if (c == 'o')
                    {
                        output += cell.ToString();
                    }

                    if (cell == -1 || cell == 256)
                    {
                        cell = 0;
                    }
                }

                return output;
            }
        }

        // adapted from https://github.com/james1345-1/Brainfuck/blob/master/C%23/Brainfuck.cs
        public class Brainfuck
        {
            private static char[] memory;
            private static int memory_pointer;

            private static char[] actions;
            private static int action_pointer;

            private static char[] inputs;
            private static int input_pointer;

            private static char _;
            private static string output;

            public static string Execute(string input, string ascii_input = "a")
            {
                // initialize/reset variables
                memory = new char[10000];
                memory_pointer = 0;

                actions = input.ToCharArray();
                action_pointer = 0;

                inputs = ascii_input.ToCharArray();
                input_pointer = 0;

                output = "";

                while (action_pointer < actions.Length)
                {
                    _ = actions[action_pointer];

                    switch (_)
                    {
                        case '>':
                            {
                                memory_pointer++;
                                break;
                            }
                        case '<':
                            {
                                memory_pointer--;
                                break;
                            }
                        case '+':
                            {
                                memory[memory_pointer]++;
                                break;
                            }
                        case '-':
                            {
                                memory[memory_pointer]--;
                                break;
                            }
                        case '.':
                            {
                                output += memory[memory_pointer];
                                break;
                            }
                        case ',':
                            try
                            {
                                memory[memory_pointer] = inputs[input_pointer];
                                input_pointer++;
                            }
                            catch (Exception e)
                            {
                                // do nothing
                            }
                            break;
                        case '[':
                            if (memory[memory_pointer] == 0)
                            {
                                while (actions[action_pointer] != ']') action_pointer++;
                            }
                            break;

                        case ']':
                            if (memory[memory_pointer] != 0)
                            {
                                while (actions[action_pointer] != '[') action_pointer--;
                            }
                            break;
                    }

                    // increment instruction mp
                    action_pointer++;
                }

                return output;
            }
        }
    }
}
