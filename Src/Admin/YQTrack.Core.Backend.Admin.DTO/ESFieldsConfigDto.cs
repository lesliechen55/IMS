using YQTrack.Core.Backend.Enums.Admin;

namespace YQTrack.Core.Backend.Admin.DTO
{
    public class ESFieldsConfigDto
    {
        public string FieldName { get; set; }
        public ESFieldType Type { get; set; }
        public string Category { get; set; }
        public bool IsValue { get; set; }
        public bool Required { get; set; }
        public string DefaultValue { get; set; }
    }
}