using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using xubot.Attributes;

namespace xubot.Commands
{
    [Group("math"), Alias("m"), Summary("A calculator, but shittier.")]
    public class Math : ModuleBase
    {
        // SIMPLE MATH
        [Example("2 2 -1")]
        [Command("add"), Alias("plus"), Summary("Adds the floats given.")]
        public async Task Add(params float[] inputs)
        {
            float result = inputs.Sum();

            await ReplyAsync($"The result is: {result}");
        }

        [Example("6 2 -3")]
        [Command("sub"), Alias("subtract"), Summary("Subtracts the floats given.")]
        public async Task Sub(params float[] inputs)
        {
            float result = inputs.Aggregate<float, float>(0, (current, num) => current - num);

            await ReplyAsync($"The result is: {result}");
        }

        [Example("2 5 7")]
        [Command("multi"), Alias("multiply"), Summary("Multiplies the floats given.")]
        public async Task Multi(params float[] inputs)
        {
            float result = inputs.Aggregate<float, float>(0, (current, num) => current * num);

            await ReplyAsync($"The result is: {result}");
        }

        [Example("144 12")]
        [Command("divide"), Alias("division"), Summary("Divides the floats given.")]
        public async Task Divide(params float[] inputs)
        {
            float result = inputs.Aggregate<float, float>(0, (current, num) => current / num);

            await ReplyAsync($"The result is: {result}");
        }

        [Example("4220 5")]
        [Command("mod"), Alias("modulo"), Summary("Modulus the floats given.")]
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
                await ReplyAsync("The result was changed to Infinity or -Infinity. Please use smaller numbers.");
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
                await ReplyAsync("The result was changed to Infinity or -Infinity. Please use smaller numbers.");
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
                await ReplyAsync("The sine was changed to Infinity or -Infinity. Please use smaller numbers.");
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
                await ReplyAsync("The hyperbolic sine was changed to Infinity or -Infinity. Please use smaller numbers.");
            }
        }

        [Example("45")]
        [Command("asin"), Summary("Gets the asin of a number and returns an angle.")]
        public async Task Asin([Summary("double")] double num)
        {
            double result = System.Math.Asin(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync($"The asin of {num} is: {result}.");
            }
            else
            {
                await ReplyAsync("The asin was changed to Infinity or -Infinity. Please use smaller numbers.");
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
                await ReplyAsync("The cosine was changed to Infinity or -Infinity. Please use smaller numbers.");
            }
        }

        [Example("4")]
        [Command("cosh"), Alias("hyperbolic-cosine"), Summary("Returns the hyperbolic cosine of a number.")]
        public async Task HyperbolicCosine([Summary("double")] double num)
        {
            double result = System.Math.Cosh(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync($"The hyperbolic cosine of {num} is: {result}.");
            }
            else
            {
                await ReplyAsync("The hyperbolic cosine was changed to Infinity or -Infinity. Please use smaller numbers.");
            }
        }

        [Example("22.5")]
        [Command("acos"), Alias("acos"), Summary("Gets the acos of a number and returns an angle.")]
        public async Task Acos([Summary("double")] double num)
        {
            double result = System.Math.Acos(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync($"The acos of {num} is: {result}.");
            }
            else
            {
                await ReplyAsync("The acos was changed to Infinity or -Infinity. Please use smaller numbers.");
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
                await ReplyAsync("The tangent was changed to Infinity or -Infinity. Please use smaller numbers.");
            }
        }

        [Example("1")]
        [Command("tanh"), Alias("hyperbolic-tan"), Summary("Gets the hyperbolic tangent of a number.")]
        public async Task HyperbolicTangent([Summary("double")] double num)
        {
            double result = System.Math.Tanh(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync($"The hyperbolic tangent of {num} is: {result}.");
            }
            else
            {
                await ReplyAsync("The hyperbolic tangent was changed to Infinity or -Infinity. Please use smaller numbers.");
            }
        }

        [Example("15")]
        [Command("atan"), Summary("Gets the atan of a number and returns an angle.")]
        public async Task Atan([Summary("double")] double num)
        {
            double result = System.Math.Atan(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync($"The atan of {num} is: {result}.");
            }
            else
            {
                await ReplyAsync("The atan was changed to Infinity or -Infinity. Please use smaller numbers.");
            }
        }

        //OTHER
        [Example("\"4+2*5\"")]
        [Command("quick-eval"), Alias("eval", "quick-do", "do"), Summary("Does quick math operations with integers.")]
        public async Task Evaluate([Summary("eval input")] string input)
        {
            DataTable table = new DataTable();
            table.Columns.Add("expression", string.Empty.GetType(), input);
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            int result = int.Parse((string)row["expression"]);
            await ReplyAsync($"The equation was evaluated and returned **{result}**.");
        }

        [Group("convert"), Alias("c"), Summary("converts some stuff")]
        public class Convert : ModuleBase
        {
            [Example("32 f2c")]
            [Command("temperature"), Alias("temp"), Summary("Converts Celsius or Fahrenheit to the other using `c2f` and `f2c`.")]
            public async Task Temp([Summary("double 1")] double num1, string fromTo)
            {
                switch (fromTo)
                {
                    case "c2f":
                        await ReplyAsync($"*Celsius to Fahrenheit:* {num1 / 9 * (5 + 32)}");
                        break;
                    case "f2c":
                    default:
                        await ReplyAsync($"*Fahrenheit to Celsius:* {(num1 - 32) * ((double)9 / 5)}");
                        break;
                }
            }

            [Example("1 m2ft")]
            [Command("length"), Alias("height"), Summary("Converts feet to meters to the other using `ft2m` and `m2ft`.")]
            public async Task Length([Summary("double 1")] double num1, string fromTo)
            {
                switch (fromTo)
                {
                    case "ft2m":
                        await ReplyAsync($"*Feet to Meters:* {num1 * 0.3048}");
                        break;
                    case "m2ft":
                    default:
                        await ReplyAsync($"*Meters to Feet:* {num1 / 0.3048}");
                        break;
                }
            }
        }
    }
}
