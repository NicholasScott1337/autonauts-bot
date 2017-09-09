using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Discord;

namespace Autonauts_BOT_CS
{
    public class UpArrow : IEmote
    {
        public string Name { get; set; }
        public UpArrow()
        {
            Name = "⬆";
        }
    }
    public class DownArrow : IEmote
    {
        public string Name { get; set; }
        public DownArrow()
        {
            Name = "⬇";
        }
    }
}
