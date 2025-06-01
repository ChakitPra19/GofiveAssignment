using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendApi.Data;
using BackendApi.Models;
using BackendApi.Dtos;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PermissionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PermissionResponseDto>> GetPermissions()
        {
            var permissions = await _context.Permissions.ToListAsync();

            var permissionDtos = permissions.Select(p => new SimplePermissionDto
            {
                PermissionId = p.PermissionId,
                PermissionName = p.PermissionName
            }).ToList();

            var response = new PermissionResponseDto
            {
                Code = "200",
                Description = "Success",
                Data = permissionDtos
            };

            return Ok(response);
        }
    }
} 