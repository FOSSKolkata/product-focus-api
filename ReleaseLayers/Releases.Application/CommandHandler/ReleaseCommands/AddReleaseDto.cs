using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Releases.Application.CommandHandler.ReleaseCommands
{
    public class AddReleaseDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
    }
}
