using System;

namespace xubot.Commands
{
    public static class SmallLangInterps
    {
        //adapted from https://esolangs.org/wiki/Deadfish#C.23
        public class Deadfish
        {
            private static string _output = "";

            private static int _cell;

            public static string Execute(string input)
            {
                input = input.Replace(((char)13).ToString(), "");

                _output = "";
                _cell = 0;

                foreach (char c in input)
                {
                    if (c == 'i')
                    {
                        _cell++;
                    }
                    else if (c == 'd')
                    {
                        _cell--;
                    }
                    else if (c == 's')
                    {
                        int i = _cell * _cell;
                        _cell = i;
                    }
                    else if (c == 'o')
                    {
                        _output += _cell.ToString();
                    }

                    if (_cell == -1 || _cell == 256)
                    {
                        _cell = 0;
                    }
                }

                return _output;
            }
        }

        // adapted from https://github.com/james1345-1/Brainfuck/blob/master/C%23/Brainfuck.cs
        public class Brainfuck
        {
            private static char[] _memory;
            private static int _memoryPointer;

            private static char[] _actions;
            private static int _actionPointer;

            private static char[] _inputs;
            private static int _inputPointer;

            private static char _;
            private static string _output;

            public static string Execute(string input, string asciiInput = "a")
            {
                // initialize/reset variables
                _memory = new char[10000];
                _memoryPointer = 0;

                _actions = input.ToCharArray();
                _actionPointer = 0;

                _inputs = asciiInput.ToCharArray();
                _inputPointer = 0;

                _output = "";

                while (_actionPointer < _actions.Length)
                {
                    _ = _actions[_actionPointer];

                    DoInstruction(_);

                    // increment instruction mp
                    _actionPointer++;
                }

                return _output;
            }

            private static void DoInstruction(char instruction)
            {
                switch (instruction)
                {
                    case '>':
                        {
                            _memoryPointer++;
                            break;
                        }
                    case '<':
                        {
                            _memoryPointer--;
                            break;
                        }
                    case '+':
                        {
                            _memory[_memoryPointer]++;
                            break;
                        }
                    case '-':
                        {
                            _memory[_memoryPointer]--;
                            break;
                        }
                    case '.':
                        {
                            _output += _memory[_memoryPointer];
                            break;
                        }
                    case ',':
                        try
                        {
                            _memory[_memoryPointer] = _inputs[_inputPointer];
                            _inputPointer++;
                        }
                        catch (Exception e)
                        {
                            // ignored
                        }

                        break;
                    case '[':
                        if (_memory[_memoryPointer] == 0)
                        {
                            while (_actions[_actionPointer] != ']') _actionPointer++;
                        }
                        break;

                    case ']':
                        if (_memory[_memoryPointer] != 0)
                        {
                            while (_actions[_actionPointer] != '[') _actionPointer--;
                        }
                        break;
                }
            }
        }
    }
}
