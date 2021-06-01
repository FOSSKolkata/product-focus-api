﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Dtos
{
    public sealed class GetUserDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}