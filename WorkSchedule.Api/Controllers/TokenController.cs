using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkSchedule.Api.Models;
using WorkSchedule.Application.Interfaces;
using WorkSchedule.Domain.User;

namespace WorkSchedule.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly ITokenService _tokenService;

    public TokenController(IUserRepository userRepo, ITokenService tokenService)
    {
        this._userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
        this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }


    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> RefreshAsync(TokenApiModel tokenApiModel)
    {
        if (tokenApiModel is null)
            return BadRequest("Invalid client request");
        string accessToken = tokenApiModel.AccessToken;
        string refreshToken = tokenApiModel.RefreshToken;
        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        var email = principal.Identity.Name; //this is mapped to the Name claim by default
        var user = await _userRepo.GetUserByEmail(email);
        if (user is null || user.Account.RefreshToken != refreshToken || user.Account.RefreshTokenExpTime <= DateTime.Now)
            return BadRequest("Invalid client request");
        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        user.Account.SetRefreshToken(newRefreshToken);
        await _userRepo.SaveChangesAsync();
        return Ok(new TokenApiModel()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }

    [HttpPost, Authorize]
    [Route("revoke")]
    public async Task<IActionResult> Revoke()
    {
        var email = User.Identity.Name;
        var user = await _userRepo.GetUserByEmail(email);
        if (user == null) return BadRequest();
        user.Account.SetRefreshToken(null);
        await _userRepo.SaveChangesAsync();
        return NoContent();
    }
}

