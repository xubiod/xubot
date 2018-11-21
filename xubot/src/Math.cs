using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot.src
{
    [Group("math"), Alias("m"), Summary("a calculator, but shittier")]
    public class Math : ModuleBase
    {
        // SIMPLE MATH
        [Command("add"), Alias("plus"), Summary("Adds two floats.")]
        public async Task add([Summary("float 1")] float num1, [Summary("float 2")] float num2)
        {
            float result = num1 + num2;
            await ReplyAsync("The result is: " + result.ToString());
        }

        [Command("sub"), Alias("subtract"), Summary("Subtract two floats.")]
        public async Task sub([Summary("float 1")] float num1, [Summary("float 2")] float num2)
        {
            float result = num1 - num2;
            await ReplyAsync("The result is: " + result.ToString());
        }

        [Command("multi"), Alias("multiply"), Summary("Multiplies two floats.")]
        public async Task multi([Summary("float 1")] float num1, [Summary("float 2")] float num2)
        {
            float result = num1 * num2;
            await ReplyAsync("The result is: " + result.ToString());
        }

        [Command("divide"), Alias("division"), Summary("Divides two floats.")]
        public async Task divide([Summary("float 1")] float num1, [Summary("float 2")] float num2)
        {
            float result = num1 / num2;
            await ReplyAsync("The result is: " + result.ToString());
        }

        [Command("mod"), Alias("modulo"), Summary("Modulos two floats.")]
        public async Task mod([Summary("float 1")] float num1, [Summary("float 2")] float num2)
        {
            float result = num1 % num2;
            await ReplyAsync("The result is: " + result.ToString());
        }

        [Command("pow"), Alias("power"), Summary("Takes a number to another number as the power.")]
        public async Task pow([Summary("double 1")] double num1, [Summary("double 2")] double num2)
        {
            double result = System.Math.Pow(num1, num2);
            
            if (!Double.IsInfinity(result))
            {
                await ReplyAsync("The result is: " + result.ToString());
            }
            else
            {
                await ReplyAsync("The result was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Command("sqrt"), Alias("squareroot"), Summary("Square roots a number.")]
        public async Task sqrt([Summary("double 1")] double num1)
        {
            double result = System.Math.Sqrt(num1);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync("The result is: " + result.ToString());
            }
            else
            {
                await ReplyAsync("The result was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        //TRIG
        [Command("sin"), Alias("sine"), Summary("Returns the sine of a number.")]
        public async Task sin([Summary("double")] double num)
        {
            double result = System.Math.Sin(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync("The sine of " + num + " is: " + result.ToString() + ".");
            }
            else
            {
                await ReplyAsync("The sine was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Command("sinh"), Alias("sineh"), Summary("Returns the hyperbolic sine of a number.")]
        public async Task sinh([Summary("double")] double num)
        {
            double result = System.Math.Sinh(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync("The hyperbolic sine of " + num + " is: " + result.ToString() + ".");
            }
            else
            {
                await ReplyAsync("The hyperbolic sine was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Command("asin"), Alias("asine"), Summary("Gets the asine of a number and returns an angle.")]
        public async Task asin([Summary("double")] double num)
        {
            double result = System.Math.Asin(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync("The asine of " + num + " is: " + result.ToString() + ".");
            }
            else
            {
                await ReplyAsync("The asine was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Command("cos"), Alias("cosine"), Summary("Returns the cosine of a number.")]
        public async Task cos([Summary("double")] double num)
        {
            double result = System.Math.Cos(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync("The cosine of " + num + " is: " + result.ToString() + ".");
            }
            else
            {
                await ReplyAsync("The cosine was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Command("cosh"), Alias("cosineh"), Summary("Returns the hyperbolic cosine of a number.")]
        public async Task cosh([Summary("double")] double num)
        {
            double result = System.Math.Cosh(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync("The hyperbolic cosine of " + num + " is: " + result.ToString() + ".");
            }
            else
            {
                await ReplyAsync("The hyperbolic cosine was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Command("acos"), Alias("acosine"), Summary("Gets the acosine of a number and returns an angle.")]
        public async Task acos([Summary("double")] double num)
        {
            double result = System.Math.Acos(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync("The acosine of " + num + " is: " + result.ToString() + ".");
            }
            else
            {
                await ReplyAsync("The acosine was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Command("tan"), Alias("tangent"), Summary("Returns the tangent of a number.")]
        public async Task tan([Summary("double")] double num)
        {
            double result = System.Math.Sin(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync("The tangent of " + num + " is: " + result.ToString() + ".");
            }
            else
            {
                await ReplyAsync("The tanget was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Command("tanh"), Alias("tangenth"), Summary("Gets the hyperbolic tangent of a number.")]
        public async Task tanh([Summary("double")] double num)
        {
            double result = System.Math.Tanh(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync("The hyperbolic tangent of " + num + " is: " + result.ToString() + ".");
            }
            else
            {
                await ReplyAsync("The hyperbolic tangent was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        [Command("atan"), Alias("atangent"), Summary("Gets the atangent of a number and returns an angle.")]
        public async Task atan([Summary("double")] double num)
        {
            double result = System.Math.Atan(num);

            if (!Double.IsInfinity(result))
            {
                await ReplyAsync("The atangent of " + num + " is: " + result.ToString() + ".");
            }
            else
            {
                await ReplyAsync("The atangent was changed to Infinty or -Infinity. Please use smaller numbers.");
            }
        }

        //OTHER
        [Command("quickeval"), Alias("eval", "quickdo", "do"), Summary("Does quick math operations with integers.")]
        public async Task evalu([Summary("eval input")] string input)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("expression", string.Empty.GetType(), input);
            System.Data.DataRow row = table.NewRow();
            table.Rows.Add(row);
            int result = int.Parse((string)row["expression"]);
            await ReplyAsync("The equation was evaluated and returned **" + result.ToString() + "**.");
        }

        [Group("convert"), Alias("c"), Summary("converts some stuff")]
        public class convert : ModuleBase
        {
            [Command("temperature"), Alias("temp"), Summary("Converts Celsius or Fahrenheit to the other using `c2f` and `f2c`.")]
            public async Task temp([Summary("double 1")] double num1, string fromto)
            {
                if (fromto == "c2f")
                {
                    await ReplyAsync("*Celsius to Fahrenheit:* " + ((num1 / 9) * (5 + 32)).ToString());
                }
                else if (fromto == "f2c")
                {
                    await ReplyAsync("*Fahrenheit to Celsius:* " + ((num1 - 32) * (9 / 5)).ToString());
                }
            }

            [Command("length"), Alias("height"), Summary("Converts feet to meters to the other using `ft2m` and `m2ft`.")]
            public async Task leng([Summary("double 1")] double num1, string fromto)
            {
                if (fromto == "ft2m")
                {
                    await ReplyAsync("*Feet to Meters:* " + (num1 * 0.3048).ToString());
                }
                else if (fromto == "m2ft")
                {
                    await ReplyAsync("*Meters to Feet:* " + (num1 / 0.3048).ToString());
                }
            }
        }

    }
}
