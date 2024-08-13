﻿using Azure;
using Candidate.Dtos;
using Candidate.Interface;
using Candidate.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        [Authorize("Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO) 
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var user = new User {
                    Id = registerDTO.Email,
                    UserName = "test123" + registerDTO.PhoneNumber,
                    FullName = registerDTO.FullName,
                    Email = registerDTO.Email,
                    PhoneNumber = registerDTO.PhoneNumber,
                    Address = registerDTO.Address,
                    DateOfBirth = registerDTO.DateOfBirth,
                    Status = false
                };
                var createUser = await _userManager.CreateAsync(user, registerDTO.Password);
                if (createUser.Succeeded) 
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "USER");
                    if (roleResult.Succeeded)
                    {
                        return Ok("user created");
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
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
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
        public async Task<IActionResult> Reset([FromBody] ResetDTO resetDTO)
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
        public async Task<ActionResult<List<object>>> AllUser(
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
        public async Task<IActionResult> EditUser(string id, [FromBody] EditUserDTO editUserDTO)
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

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return Ok("User updated successfully.");
            else
                return StatusCode(500, result.Errors);
        }
        [Authorize(Roles ="Admin, Leader")]
        [HttpGet("details/{id}")]
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
                user.Id,
                user.FullName,
                user.Email,
                user.PhoneNumber,
                user.Address,
                user.DateOfBirth,
                user.Status,
                Roles= roles[0],
            });
        }
        [Authorize(Roles = "Admin, Leader")]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDTO assignRoleDTO)
        {
            var user = await _userManager.FindByIdAsync(assignRoleDTO.UserId);
            if (user == null)
                return NotFound("User not found.");

            var currentRoles = await _userManager.GetRolesAsync(user);

            // Thêm vai trò nếu chưa có
            if (!currentRoles.Contains(assignRoleDTO.Roles))
            {
                var addResult = await _userManager.AddToRoleAsync(user, assignRoleDTO.Roles);
                if (!addResult.Succeeded)
                    return StatusCode(500, addResult.Errors);
            }

            // Loại bỏ vai trò nếu không còn nằm trong yêu cầu
            var rolesToRemove = currentRoles.Where(role => role != assignRoleDTO.Roles).ToList();
            if (rolesToRemove.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                    return StatusCode(500, removeResult.Errors);
            }

            return Ok("Role updated successfully.");
        }
        [Authorize(Roles ="Admin")]
        [HttpPatch("activate/{id}")]
        public async Task<IActionResult> ActivateUser(string id, [FromBody] ActivatedDTO activatedDTO)
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
    }
    
}