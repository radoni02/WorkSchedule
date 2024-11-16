using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkSchedule.Api.Models;
using WorkSchedule.Application.Interfaces;
using WorkSchedule.Domain.Preference;
using WorkSchedule.Domain.User;
using WorkSchedule.Infrastructure.Token;

namespace WorkSchedule.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferencesController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPreferenceRepository _preferenceRepository;
        private readonly LoginUserService _loginUserService;

        public PreferencesController(IPreferenceRepository preferenceRepository, IUserRepository userRepo, ITokenService tokenService, IPasswordHasher passwordHasher, LoginUserService loginUserService)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _loginUserService = loginUserService;
            _preferenceRepository = preferenceRepository;
        }
        /// <summary>
        /// Retrieves list of preferences
        /// </summary>
        /// <param name="date">Date of preference</param>
        /// <param name="end">Optional end date for ranges</param>
        /// <param name="userID">Targeted user ids (no ids will get every preference)</param>
        /// <returns>List of PreferenceModel objects</returns>
        [HttpGet("GetPreferences/{date:datetime}"), Authorize]
        public async Task<IActionResult> GetPreferences(DateTime date, [FromQuery] DateTime? end, [FromQuery] IList<string> userID)
        {
            if (end != null && date.CompareTo(end) > 0)
                return BadRequest("Wrong range given in the query!");

            IList<User> dbUsers = [];
            foreach(var id in userID)
            {
                User? tmpUser = await _userRepo.GetUserById(Guid.Parse(id));
                if (tmpUser == null)
                    return NotFound("User with the given id not found in the system!");
                dbUsers.Add(tmpUser);
            }

            IList<PreferenceModel> answer = [];
            IList<Preference> dbPreferences;
            if (end != null)
            {
                dbPreferences = await _preferenceRepository.GetPreferencesByRange(DateOnly.FromDateTime(date), DateOnly.FromDateTime((DateTime)end), dbUsers);
            }
            else
            {
                dbPreferences = await _preferenceRepository.GetPreferences(DateOnly.FromDateTime(date), dbUsers);
            }
            
            foreach(Preference preference in dbPreferences)
            {
                User? tmpUser = await _userRepo.GetUserById(preference.UserId);
                answer.Add(new(tmpUser?.Name ?? "Unknown name", tmpUser?.Lastname ?? "Unknown lastname", tmpUser?.Id ?? Guid.Empty, preference.Start, preference.End));
            }

            return new JsonResult(answer);
        }
        /// <summary>
        /// Sets new user Preference
        /// </summary>
        /// <param name="preference">New preference to be set</param>
        [HttpPut("SetUserPreference"), Authorize(Roles = "SuperAdmin,Admin,Worker")]
        public async Task<IActionResult> SetUserPreference([FromBody] PreferenceModel preference)
        {
            User? ownUser = await _userRepo.GetUserByEmail(User?.Identity?.Name ?? string.Empty);
            if (ownUser == null)
                return BadRequest("Internal authorization error!");
            if (User != null && User.IsInRole("Worker") && !ownUser.Id.Equals(preference.UserId))
                return BadRequest("Regular user cannot change other people preferences!");

            User? dbUser = await _userRepo.GetUserById(preference.UserId);
            if (dbUser == null)
                return BadRequest("User given in the preference not found in the system!");
            await _preferenceRepository.SetUserPreference(preference.Start, preference.End, dbUser);
            await _preferenceRepository.SaveChangesAsync();

            return Ok("Preference set!");
        }
        /// <summary>
        /// Clears preference from a given day
        /// </summary>
        /// <param name="userID">Target user ID</param>
        /// <param name="date">Target date</param>
        /// <returns></returns>
        [HttpDelete("ClearUserPreference/{userID}/{date:datetime}"), Authorize(Roles = "SuperAdmin,Admin,Worker")]
        public async Task<IActionResult> ClearUserPreference(string userID, DateTime date)
        {
            User? ownUser = await _userRepo.GetUserByEmail(User?.Identity?.Name ?? string.Empty);
            if (ownUser == null)
                return BadRequest("Internal authorization error!");
            if (User != null && User.IsInRole("Worker") && !ownUser.Id.Equals(userID))
                return BadRequest("Regular user cannot change other people preferences!");

            User? dbUser = await _userRepo.GetUserById(Guid.Parse(userID));
            if (dbUser == null)
                return BadRequest("User given in the preference not found in the system!");
            await _preferenceRepository.ClearUserPreference(DateOnly.FromDateTime(date), dbUser);
            await _preferenceRepository.SaveChangesAsync();

            return Ok("Preferences cleared!");
        }
    }
}
