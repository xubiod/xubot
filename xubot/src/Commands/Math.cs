using Discord.Commands;
using SteamKit2.GC.TF2.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xubot.src.Attributes;

namespace xubot.src.Commands
{
    [Group("math"), Alias("m"), Summary("A calculator, but shittier.")]
    public class Math : ModuleBase
    {
        // SIMPLE MATH
        [Example("2 2 -1")]
        [Command("add"), Alias("plus"), Summary("Adds the floats given.")]
        public async Task Add(params float[] inputs)
        {
            float result = 0;
            foreach (float num in inputs)
                result += num;

            await ReplyAsync($"The result is: {result}");
        }

        [Example("6 2 -3")]
        [Command("sub"), Alias("subtract"), Summary("Subtracts the floats given.")]
        public async Task Sub(params float[] inputs)
        {
            float result = 0;
            foreach (float num in inputs)
                result -= num;

            await ReplyAsync($"The result is: {result}");
        }

        [Example("2 5 7")]
        [Command("multi"), Alias("multiply"), Summary("Multiplies the floats given.")]
        public async Task Multi(params float[] inputs)
        {
            float result = 0;
            foreach (float num in inputs)
                result *= num;

            await ReplyAsync($"The result is: {result}");
        }

        [Example("144 12")]
        [Command("divide"), Alias("division"), Summary("Divides the floats given.")]
        public async Task Divide(params float[] inputs)
        {
            float result = 0;
            foreach (float num in inputs)
                result /= num;

            await ReplyAsync($"The result is: {result}");
        }

        [Example("4220 5")]
        [Command("mod"), Alias("modulo"), Summary("Modulos the floats given.")]
        public async Task Mod(float input, float modBy)
        {
            float result = input % modBy;

            await ReplyAsync($"The result is: {result}");
        }

        [Example("2 8")]
        [Command("pow"), Alias("power"), Summary("Takes a number to another number as the power.")]
        public async Task Pow([Summary("double 1")] double num1, [Summary("double 2")] double num2)
        {
            double result = System.Math.Pow(num1, num2);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync($"The result is: {result}");
            }
            else
            {
                await ReplyAsync($"The result was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Example("4761")]
        [Command("sqrt"), Alias("squareroot"), Summary("Square roots a number.")]
        public async Task Sqrt([Summary("double 1")] double num1)
        {
            double result = System.Math.Sqrt(num1);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync($"The result is: {result}");
            }
            else
            {
                await ReplyAsync($"The result was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        //TRIG
        [Example("3.14")]
        [Command("sin"), Alias("sine"), Summary("Returns the sine of a number.")]
        public async Task Sine([Summary("double")] double num)
        {
            double result = System.Math.Sin(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync($"The sine of {num} is: {result}.");
            }
            else
            {
                await ReplyAsync($"The sine was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Example("5")]
        [Command("sinh"), Alias("sineh"), Summary("Returns the hyperbolic sine of a number.")]
        public async Task HyperbolicSine([Summary("double")] double num)
        {
            double result = System.Math.Sinh(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync($"The hyperbolic sine of {num} is: {result}.");
            }
            else
            {
                await ReplyAsync($"The hyperbolic sine was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Example("45")]
        [Command("asin"), Alias("asine"), Summary("Gets the asine of a number and returns an angle.")]
        public async Task Asine([Summary("double")] double num)
        {
            double result = System.Math.Asin(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync($"The asine of {num} is: {result}.");
            }
            else
            {
                await ReplyAsync($"The asine was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Example("2")]
        [Command("cos"), Alias("cosine"), Summary("Returns the cosine of a number.")]
        public async Task Cosine([Summary("double")] double num)
        {
            double result = System.Math.Cos(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync($"The cosine of {num} is: {result}.");
            }
            else
            {
                await ReplyAsync($"The cosine was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Example("4")]
        [Command("cosh"), Alias("cosineh"), Summary("Returns the hyperbolic cosine of a number.")]
        public async Task HyperbolicCosine([Summary("double")] double num)
        {
            double result = System.Math.Cosh(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync($"The hyperbolic cosine of {num} is: {result}.");
            }
            else
            {
                await ReplyAsync($"The hyperbolic cosine was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Example("22.5")]
        [Command("acos"), Alias("acosine"), Summary("Gets the acosine of a number and returns an angle.")]
        public async Task Acosine([Summary("double")] double num)
        {
            double result = System.Math.Acos(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync($"The acosine of {num} is: {result}.");
            }
            else
            {
                await ReplyAsync($"The acosine was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Example("1.7")]
        [Command("tan"), Alias("tangent"), Summary("Returns the tangent of a number.")]
        public async Task Tangent([Summary("double")] double num)
        {
            double result = System.Math.Sin(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync($"The tangent of {num} is: {result}.");
            }
            else
            {
                await ReplyAsync($"The tanget was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Example("1")]
        [Command("tanh"), Alias("tangenth"), Summary("Gets the hyperbolic tangent of a number.")]
        public async Task HyperbolicTangent([Summary("double")] double num)
        {
            double result = System.Math.Tanh(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync($"The hyperbolic tangent of {num} is: {result}.");
            }
            else
            {
                await ReplyAsync($"The hyperbolic tangent was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Example("15")]
        [Command("atan"), Alias("atangent"), Summary("Gets the atangent of a number and returns an angle.")]
        public async Task Atangent([Summary("double")] double num)
        {
            double result = System.Math.Atan(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync($"The atangent of {num} is: {result}.");
            }
            else
            {
                await ReplyAsync($"The atangent was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        //OTHER
        [Example("\"4+2*5\"")]
        [Command("quickeval"), Alias("eval", "quickdo", "do"), Summary("Does quick math operations with integers.")]
        public async Task Evalutare([Summary("eval input")] string input)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("expression", string.Empty.GetType(), input);
            System.Data.DataRow row = table.NewRow();
            table.Rows.Add(row);
            int result = int.Parse((string)row["expression"]);
            await ReplyAsync($"The equation was evaluated and returned **{result}**.");
        }

        [Group("convert"), Alias("c"), Summary("converts some stuff")]
        public class convert : ModuleBase
        {
            [Example("32 f2c")]
            [Command("temperature"), Alias("temp"), Summary("Converts Celsius or Fahrenheit to the other using `c2f` and `f2c`.")]
            public async Task Temp([Summary("double 1")] double num1, string fromto)
            {
                if (fromto == "c2f")
                {
                    await ReplyAsync($"*Celsius to Fahrenheit:* {(num1 / 9) * (5 + 32)}");
                }
                else if (fromto == "f2c")
                {
                    await ReplyAsync($"*Fahrenheit to Celsius:* {(num1 - 32) * (9 / 5)}");
                }
            }

            [Example("1 m2ft")]
            [Command("length"), Alias("height"), Summary("Converts feet to meters to the other using `ft2m` and `m2ft`.")]
            public async Task Length([Summary("double 1")] double num1, string fromto)
            {
                if (fromto == "ft2m")
                {
                    await ReplyAsync($"*Feet to Meters:* {num1 * 0.3048}");
                }
                else if (fromto == "m2ft")
                {
                    await ReplyAsync($"*Meters to Feet:* {num1 / 0.3048}");
                }
            }
        }
    }
}
