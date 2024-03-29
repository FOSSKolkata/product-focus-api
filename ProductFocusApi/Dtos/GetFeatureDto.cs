﻿using ProductFocus.Domain.Model;

namespace ProductFocusApi.Dtos
{
    public class GetFeatureDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public WorkItemType WorkItemType { get; set; }
    }
}
