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
    public class WorkDayController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IWorkDayRepository _workDayRepository;

        public WorkDayController(IWorkDayRepository workDayRepository, IUserRepository userRepo, ITokenService tokenService, IPasswordHasher passwordHasher)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _workDayRepository = workDayRepository;
        }
        /// <summary>
        /// Retrieves list of workdays
        /// </summary>
        /// <param name="date">Date of workday</param>
        /// <param name="end">Optional end date for ranges</param>
        /// <param name="userID">Targeted user ids (no ids will get every workday)</param>
        /// <returns>List of WorkDayModel objects</returns>
        [HttpGet("GetWorkdays/{date:datetime}"), Authorize]
        public async Task<IActionResult> GetWorkdays(DateTime date, [FromQuery] DateTime? end, [FromQuery] IList<string> userID)
        {
            if (end != null && date.CompareTo(end) > 0)
                return BadRequest("Wrong range given in the query!");

            IList<User> dbUsers = [];
            foreach (var id in userID)
            {
                User? tmpUser = await _userRepo.GetUserById(Guid.Parse(id));
                if (tmpUser == null)
                    return NotFound("User with the given id not found in the system!");
                dbUsers.Add(tmpUser);
            }

            IList<WorkDayModel> answer = [];
            IList<WorkDay> dbWorkdays;
            if (end != null)
            {
                dbWorkdays = await _workDayRepository.GetWorkdaysByRange(DateOnly.FromDateTime(date), DateOnly.FromDateTime((DateTime)end), dbUsers);
            }
            else
            {
                dbWorkdays = await _workDayRepository.GetWorkday(DateOnly.FromDateTime(date), dbUsers);
            }

            foreach (WorkDay workday in dbWorkdays)
            {
                User? tmpUser = await _userRepo.GetUserById(workday.UserId);
                answer.Add(new(tmpUser?.Name ?? "Unknown name", tmpUser?.Lastname ?? "Unknown lastname", tmpUser?.Id ?? Guid.Empty, workday.Start, workday.End));
            }

            return new JsonResult(answer);
        }
        /// <summary>
        /// Sets new user Workday
        /// </summary>
        /// <param name="workday">New workday to be set</param>
        [HttpPut("SetUserWorkday"), Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> SetUserWorkday([FromBody] WorkDayModel workday)
        {
            User? dbUser = await _userRepo.GetUserById(workday.UserId);
            if (dbUser == null)
                return BadRequest("User given in the workday not found in the system!");
            await _workDayRepository.SetUserWorkday(workday.Start, workday.End, dbUser);
            await _workDayRepository.SaveChangesAsync();

            return Ok("Workday set!");
        }
        /// <summary>
        /// Transfers preferences to workday form
        /// </summary>
        /// <param name="preferences">List of preferences to be transfered to a regular workday</param>
        [HttpPost("TransferUserPreferences"), Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> TransferUserPreferences([FromBody] IList<PreferenceModel> preferences)
        {
            foreach(PreferenceModel preference in preferences)
            {
                User? tmpUser = await _userRepo.GetUserById(preference.UserId);
                if (tmpUser == null)
                    return BadRequest(string.Concat("User not found in the system! (userID=", preference.UserId, ")"));
                try
                {
                    await _workDayRepository.TransferUserPreference(preference.Start, preference.End, tmpUser);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            await _workDayRepository.SaveChangesAsync();
            return Ok("Preferences transfered to regular workdays");
        }
        /// <summary>
        /// Clears workday from a given day
        /// </summary>
        /// <param name="userID">Target user ID</param>
        /// <param name="date">Target date</param>
        [HttpDelete("ClearUserWorkday/{userID}/{date:datetime}"), Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> ClearUserWorkday(string userID, DateTime date)
        {
            User? dbUser = await _userRepo.GetUserById(Guid.Parse(userID));
            if (dbUser == null)
                return BadRequest("User given in the workday not found in the system!");
            await _workDayRepository.ClearUserWorkday(DateOnly.FromDateTime(date), dbUser);
            await _workDayRepository.SaveChangesAsync();

            return Ok("Workday cleared!");
        }
    }
}
