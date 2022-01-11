using System;

namespace ProductFocusApi.Dtos
{
    public class AddSprintDto
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
