using System.Collections.Generic;

namespace BackendApi.Dtos
{
    public class RoleResponseDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public List<RoleDto> Data { get; set; }
    }
} 