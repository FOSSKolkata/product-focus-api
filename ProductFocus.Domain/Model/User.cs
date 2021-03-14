﻿using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductFocus.Domain.Model
{
    public class User : Entity<long>
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}