using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendApi.Data;
using BackendApi.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.Include(u => u.Role).ToListAsync();
        }

        [HttpGet("DataTable")]
        public async Task<ActionResult<PaginatedUserResponse>> GetUsersDataTable(
            [FromQuery] string? orderBy = null,
            [FromQuery] string? orderDirection = "asc",
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            if (pageNumber < 1 || pageSize < 1) {
                return BadRequest("pageNumber and pageSize must be positive integers.");
            }

            var query = _context.Users.AsQueryable();

            query = query.OrderBy(u => u.UserId);

            if (!string.IsNullOrEmpty(search)) {
                query = query.Where(u => u.FirstName!.Contains(search) ||
                                         u.LastName!.Contains(search) ||
                                         u.Email!.Contains(search) ||
                                         u.Username!.Contains(search));
            }

            if (!string.IsNullOrEmpty(orderBy)) {
                switch (orderBy.ToLower()) {
                    case "userid":
                         query = (orderDirection?.ToLower() == "desc") ? query.OrderByDescending(u => u.UserId) : query.OrderBy(u => u.UserId);
                         break;
                    case "firstname":
                        query = (orderDirection?.ToLower() == "desc") ? query.OrderByDescending(u => u.FirstName) : query.OrderBy(u => u.FirstName);
                        break;
                    case "lastname":
                         query = (orderDirection?.ToLower() == "desc") ? query.OrderByDescending(u => u.LastName) : query.OrderBy(u => u.LastName);
                         break;
                    case "email":
                         query = (orderDirection?.ToLower() == "desc") ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email);
                         break;
                    case "username":
                         query = (orderDirection?.ToLower() == "desc") ? query.OrderByDescending(u => u.Username) : query.OrderBy(u => u.Username);
                         break;
                    case "createddate":
                         query = (orderDirection?.ToLower() == "desc") ? query.OrderByDescending(u => u.CreatedDate) : query.OrderBy(u => u.CreatedDate);
                         break;
                     case "roleid":
                         query = (orderDirection?.ToLower() == "desc") ? query.OrderByDescending(u => u.RoleId) : query.OrderBy(u => u.RoleId);
                         break;
                    default:
                         query = (orderDirection?.ToLower() == "desc") ? query.OrderByDescending(u => u.UserId) : query.OrderBy(u => u.UserId);
                         break;
                }
            }

            var totalCount = await query.CountAsync();

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(u => u.Role)
                .ToListAsync();

            var response = new PaginatedUserResponse
            {
                DataSource = users,
                TotalCount = totalCount,
                Page = pageNumber,
                PageSize = pageSize
            };

            return Ok(response);
        }
    }
}