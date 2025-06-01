using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendApi.Data;
using BackendApi.Models;
using BackendApi.Dtos;
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
                var searchTerms = search.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                
                if (searchTerms.Length == 2) {
                    var firstName = searchTerms[0];
                    var lastName = searchTerms[1];
                    query = query.Where(u => 
                        (u.FirstName != null && u.FirstName.ToLower().Contains(firstName)) &&
                        (u.LastName != null && u.LastName.ToLower().Contains(lastName))
                    );
                } else if (searchTerms.Length == 1) {
                    var term = searchTerms[0];
                    query = query.Where(u =>
                        (u.FirstName != null && u.FirstName.ToLower().Contains(term)) ||
                        (u.LastName != null && u.LastName.ToLower().Contains(term)) ||
                        (u.Email != null && u.Email.ToLower().Contains(term)) ||
                        (u.Username != null && u.Username.ToLower().Contains(term))
                    );
                } else {
                }
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteUserResponseDto>> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return Ok(new DeleteUserResponseDto
                {
                    Status = new Status
                    {
                        Code = "404",
                        Description = "Not Found"
                    },
                    Data = new DeleteUserData
                    {
                        Result = false,
                        Message = "User not found"
                    }
                });
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(new DeleteUserResponseDto
                {
                    Status = new Status
                    {
                        Code = "200",
                        Description = "Success"
                    },
                    Data = new DeleteUserData
                    {
                        Result = true,
                        Message = "User deleted successfully"
                    }
                });
            }
            catch (Exception ex)
            {
                return Ok(new DeleteUserResponseDto
                {
                    Status = new Status
                    {
                        Code = "500",
                        Description = "Internal Server Error"
                    },
                    Data = new DeleteUserData
                    {
                        Result = false,
                        Message = "Error deleting user: " + ex.Message
                    }
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserByIdResponseDto>> GetUserById(string id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return Ok(new GetUserByIdResponseDto
                {
                    Status = new Status
                    {
                        Code = "404",
                        Description = "Not Found"
                    },
                    Data = null
                });
            }

            var response = new GetUserByIdResponseDto
            {
                Status = new Status
                {
                    Code = "200",
                    Description = "Success"
                },
                Data = new UserData
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName ?? string.Empty,
                    LastName = user.LastName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Phone = string.Empty,
                    Username = user.Username ?? string.Empty,
                    Role = new RoleData
                    {
                        RoleId = user.Role?.RoleId ?? string.Empty,
                        RoleName = user.Role?.RoleName ?? string.Empty
                    },
                    Permissions = new List<PermissionData>()
                }
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(AddUserDto addUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == addUserDto.Username))
            {
                return BadRequest("Username already exists");
            }

            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == addUserDto.Email))
            {
                return BadRequest("Email already exists");
            }

            // Check if role exists
            var role = await _context.Roles.FindAsync(addUserDto.RoleId);
            if (role == null)
            {
                return BadRequest("Invalid role");
            }

            var user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                FirstName = addUserDto.FirstName,
                LastName = addUserDto.LastName,
                Email = addUserDto.Email,
                Username = addUserDto.Username,
                RoleId = addUserDto.RoleId,
                CreatedDate = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }

        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GetUserByIdResponseDto>> UpdateUser(string id, [FromBody] UpdateUsersRequestDTO request)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == request.RoleId);

            if (role == null)
            {
                return BadRequest(new GetUserByIdResponseDto
                {
                    Status = new Status
                    {
                        Code = "400",
                        Description = "Invalid role name"
                    },
                    Data = null
                });
            }

            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound(new GetUserByIdResponseDto
                {
                    Status = new Status
                    {
                        Code = "404",
                        Description = "User not found"
                    },
                    Data = null
                });
            }

            // Update fields
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Phone = request.Phone;
            user.Username = request.Username;
            user.Password = request.Password;
            user.RoleId = role.RoleId;
            user.CreatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Return in the same format as GetUserById
            return Ok(new GetUserByIdResponseDto
            {
                Status = new Status
                {
                    Code = "200",
                    Description = "User updated successfully"
                },
                Data = new UserData
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName ?? string.Empty,
                    LastName = user.LastName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Phone = user.Phone ?? string.Empty,
                    Username = user.Username ?? string.Empty,
                    Role = new RoleData
                    {
                        RoleId = user.Role?.RoleId ?? string.Empty,
                        RoleName = user.Role?.RoleName ?? string.Empty
                    }
                }
            });
        }
    }
}