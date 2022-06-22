using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Releases.Application.QueryHandler
{
    public class GetReleaseDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ReleaseDate { get; set; }
    }
}
