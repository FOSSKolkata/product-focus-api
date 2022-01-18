﻿using System;

namespace ProductFocusApi.Dtos
{
    public sealed class GetBusinessRequirementAttachmentDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public string ContentType { get; set; }
        public byte[] Contents { get; set; }
        public DateTimeOffset? LastModified { get; set; }
    }
}
