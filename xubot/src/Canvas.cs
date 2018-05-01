using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using SixLabors.ImageSharp;
using SixLabors.Primitives;
using SixLabors.Shapes;
using SixLabors.ImageSharp.Processing;

using SLImage = SixLabors.ImageSharp.Image;

namespace xubot.src
{
    [Group("canvas")]
    public class Canvas : ModuleBase
    {
        [Command("create")]
        public async Task create(int width, int height)
        {
            
        }
    }
}
