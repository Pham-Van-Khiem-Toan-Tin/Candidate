using Azure;
using Candidate.Dtos;
using Candidate.Interface;
using Candidate.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Linq;

namespace Candidate.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<User> userManager,RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> Register([FromBody] RegisterForm registerDTO) 
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var user = new User {
                    Id = registerDTO.UserName,
                    UserName = registerDTO.UserName,
                    FullName = registerDTO.FullName,
                    Email = registerDTO.Email,
                    PhoneNumber = registerDTO.PhoneNumber,
                    Address = registerDTO.Address,
                    DateOfBirth = registerDTO.DateOfBirth,
                    Status = false
                };
                var createUser = await _userManager.CreateAsync(user, "0123456789Ab.");
                if (createUser.Succeeded) 
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "USER");
                    if (roleResult.Succeeded)
                    {
                        return Ok(new { message = "User created successfully"});
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createUser.Errors);
                }
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginForm loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await  _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDTO.email);
            if (user == null) return Unauthorized("Invalid Email");
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.password, false);
            if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");
            var role = await _userManager.GetRolesAsync(user);
            return Ok(new { user.Email, role , token = _tokenService.GenerateToken(user, role.ToList())});
        }
        [AllowAnonymous]
        [HttpPost("reset")]
        public async Task<IActionResult> Reset([FromBody] ResetForm resetDTO)
        {
            if (ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(resetDTO.Email);
            if (user == null) return BadRequest("Invalid email address");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = Url.Action(
                action: "ResetPassword",
                controller: "User",
                values: new { token = token, email = user.Email }
            );
            return Ok("Password reset email sent");
            
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<List<object>>> Search(
            [FromQuery] string name = null,
            [FromQuery] string status = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            const int maxPageSize = 100;
            pageSize = (pageSize > maxPageSize) ? maxPageSize : pageSize;

            IQueryable<User> query = _userManager.Users;

            if (!String.IsNullOrWhiteSpace(name))
            {
                query = query.Where(u => u.FullName.Contains(name));
            }
            if (!String.IsNullOrWhiteSpace(status))
            {
                bool isActive = status.Equals("Activated", StringComparison.OrdinalIgnoreCase);
                query = query.Where(u => u.Status == isActive);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var users = await query
                .OrderBy(u => u.FullName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var usersWithRoles = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add(new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    user.PhoneNumber,
                    user.Status,
                    user.Address,
                    user.DateOfBirth,
                    Roles = roles
                });
            }

            var result = new PagedResult<object>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = usersWithRoles
            };

            return Ok(result);

        }

        [Authorize(Roles ="Admin")]
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditUser(string id, [FromBody] EditUserForm editUserDTO)
        {
            // Kiểm tra tính hợp lệ của ModelState
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (editUserDTO == null)
                return BadRequest("Invalid user data.");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            // Cập nhật thông tin người dùng
            user.FullName = editUserDTO.FullName ?? user.FullName;
            user.Email = editUserDTO.Email ?? user.Email;
            user.PhoneNumber = editUserDTO.PhoneNumber ?? user.PhoneNumber;
            user.Address = editUserDTO.Address ?? user.Address;
            if (editUserDTO.DateOfBirth != null)
            {
                user.DateOfBirth = editUserDTO.DateOfBirth;
            }
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Thêm vai trò nếu chưa có
            if (!currentRoles.Contains(editUserDTO.Role))
            {
                var addResult = await _userManager.AddToRoleAsync(user, editUserDTO.Role);
                if (!addResult.Succeeded)
                    return StatusCode(500, addResult.Errors);
            }

            // Loại bỏ vai trò nếu không còn nằm trong yêu cầu
            var rolesToRemove = currentRoles.Where(role => role != editUserDTO.Role).ToList();
            if (rolesToRemove.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                    return StatusCode(500, removeResult.Errors);
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return Ok(new { message = "User updated successfully" });
            else
                return StatusCode(500, result.Errors);
        }
        [Authorize(Roles ="Admin, Leader")]
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetUserDetails(string id)
        {
            if (_userManager == null)
                return StatusCode(500, "UserManager service is not available.");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            // Kiểm tra giá trị của user
            IList<String> roles = await _userManager.GetRolesAsync(user);
            return Ok(new
            {
                user.FullName,
                user.Email,
                user.PhoneNumber,
                user.Address,
                user.DateOfBirth,
                user.Status,
                Roles= roles[0],
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (_userManager == null)
                return StatusCode(500, "UserManager service is not available.");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { message = "User deleted successfully" });
            }
            else
            {
                return StatusCode(500, result.Errors);
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpPatch("activate/{id}")]
        public async Task<IActionResult> ActivateUser(string id, [FromBody] ActivatedForm activatedDTO)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            user.Status = activatedDTO.IsActivated ? true : false;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(activatedDTO.IsActivated ? "User activated successfully." : "User deactivated successfully.");
            }
            else
            {
                return StatusCode(500, "Error update");
            }
        }
        [Authorize(Roles = "Admin, Leader")]
        [HttpGet("roles")]
        public async Task<ActionResult> GetRoleList()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            if (roles == null || roles.Count == 0)
                return NotFound("No roles found.");

            return Ok(roles);

        }
        [Authorize(Roles = "Admin, Leader")]
        [HttpGet("exports")]
        public async Task<ActionResult> ExportUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Users");
            worksheet.Cells[1, 1].Value = "FullName";
            worksheet.Cells[1, 2].Value = "Email";
            worksheet.Cells[1, 3].Value = "PhoneNumber";
            worksheet.Cells[1, 4].Value = "Address";
            worksheet.Cells[1, 5].Value = "DateOfBirth";
            worksheet.Cells[1, 6].Value = "Status";
            for (int i = 0; i < users.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = users[i].FullName;
                worksheet.Cells[i + 2, 2].Value = users[i].Email;
                worksheet.Cells[i + 2, 3].Value = users[i].PhoneNumber;
                worksheet.Cells[i + 2, 4].Value = users[i].Address;
                if (users[i].DateOfBirth != null)
                {
                    worksheet.Cells[i + 2, 5].Value = users[i].DateOfBirth; // Excel tự động nhận diện DateTime
                    worksheet.Cells[i + 2, 5].Style.Numberformat.Format = "yyyy-MM-dd"; // Định dạng ngày tháng trong Excel
                }
                else
                {
                    worksheet.Cells[i + 2, 5].Value = ""; // Hoặc giá trị mặc định nếu không có ngày sinh
                }
                worksheet.Cells[i + 2, 6].Value = users[i].Status ? "Activated" : "Deactivated";
            }
            worksheet.Cells.AutoFitColumns();

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            var fileName = "Users.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }


}
