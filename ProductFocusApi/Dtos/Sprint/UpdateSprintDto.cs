﻿using System;

namespace ProductFocusApi.Dtos.Sprint
{
    public class UpdateSprintDto
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
