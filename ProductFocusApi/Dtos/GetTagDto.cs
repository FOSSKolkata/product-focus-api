using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocusApi.Dtos
{
    public sealed class GetTagDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
