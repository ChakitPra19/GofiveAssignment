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
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RolesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<RoleResponseDto>> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();

            var roleDtos = roles.Select(r => new RoleDto
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName
            }).ToList();

            var response = new RoleResponseDto
            {
                Code = "200",
                Description = "Success",
                Data = roleDtos
            };

            return Ok(response);
        }
    }
} 