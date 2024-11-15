﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkSchedule.Api.Models;
using WorkSchedule.Application.Interfaces;
using WorkSchedule.Domain.User;
using WorkSchedule.Infrastructure.Token;

namespace WorkSchedule.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRoles _roles;
        private readonly LoginUserService _loginUserService;

        public UserController(IRoles rolesRepo, IUserRepository userRepo, ITokenService tokenService, IPasswordHasher passwordHasher, LoginUserService loginUserService)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _loginUserService = loginUserService;
            _roles = rolesRepo;
        }
        /// <summary>
        /// Retrieves every role in the system
        /// </summary>
        /// <returns>List of possible roles</returns>
        [HttpGet("ListRoles")]
        public async Task<IActionResult> ListRoles() => new JsonResult(await _roles.GetAllRoles());
        /// <summary>
        /// Modifies user data
        /// </summary>
        /// <param name="targetedUserEmail">(optional) Email of other user</param>
        /// <param name="newUserData">Data of the new user</param>
        [HttpPut("ModifyUser"), Authorize]
        public async Task<IActionResult> ModifyUser([FromQuery] string? targetedUserEmail, [FromBody] UserModel newUserData)
        {
            string? email = User?.Identity?.Name;
            if (email == null)
                return BadRequest("User's e-mail data is missing from the token!");
            if (!Enum.TryParse(newUserData.Role, out AppRole parsedRole))
                return BadRequest("Wrong role specified in the user data!");
            User? dbUser = await _userRepo.GetUserByEmail(email);
            if (dbUser == null)
                return NotFound("User with the given token mail not found in the system!");

            if (!string.IsNullOrWhiteSpace(targetedUserEmail))
            {
                if (User != null && (!User.IsInRole(AppRole.SuperAdmin.ToString()) && !User.IsInRole(AppRole.Admin.ToString())))
                    return BadRequest("Regular user cannot modify other users!");

                User? targetedDbUser = await _userRepo.GetUserByEmail(targetedUserEmail);
                if (targetedDbUser == null)
                    return NotFound("User requested for the change not found in the system!");
                targetedDbUser = new(targetedDbUser.Id, newUserData.Name, newUserData.Lastname, parsedRole, targetedDbUser.AccountId, targetedDbUser.Account);
                await _userRepo.ModifyUser(targetedDbUser);
            }
            else
            {
                if ((parsedRole == AppRole.SuperAdmin || parsedRole == AppRole.Admin) && User != null && (User.IsInRole(AppRole.Worker.ToString()) || User.IsInRole(AppRole.Spectator.ToString())))
                    return BadRequest("Regular user cannot set SuperAdmin or Admin role!");

                dbUser = new(dbUser.Id, newUserData.Name, newUserData.Lastname, parsedRole, dbUser.AccountId, dbUser.Account);
                await _userRepo.ModifyUser(dbUser);
            }
            await _userRepo.SaveChangesAsync();

            return Ok("User changed!");
        }
        /// <summary>
        /// Deletes targeted user account
        /// </summary>
        /// <param name="targetEmail">E-mail of the targeted user</param>
        [HttpDelete("DeleteUser/{targetEmail}"), Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> DeleteUser(string targetEmail)
        {
            if (targetEmail.Equals(User?.Identity?.Name))
                return BadRequest("User cannot delete it's own account!");

            User? dbUser = await _userRepo.GetUserByEmail(targetEmail);
            if (dbUser == null) 
                return NotFound("User for deletion not found!");

            await _userRepo.DeleteUser(dbUser);
            await _userRepo.SaveChangesAsync();
            return Ok("User deleted");
        }
    }
}
