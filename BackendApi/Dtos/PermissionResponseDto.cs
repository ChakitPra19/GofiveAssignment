using System.Collections.Generic;

namespace BackendApi.Dtos
{
    public class PermissionResponseDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public List<SimplePermissionDto> Data { get; set; }
    }
} 