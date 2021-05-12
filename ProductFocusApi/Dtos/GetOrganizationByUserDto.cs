using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Dtos
{
    public sealed class GetOrganizationByUserDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsOwner { get; set; }
    }
}
