﻿using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkSchedule.Api.Models;
using WorkSchedule.Application.Interfaces;
using WorkSchedule.Infrastructure.Token;
using WorkSchedule.Application.User.Register;
using WorkSchedule.Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace WorkSchedule.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly LoginUserService _loginUserService;

    public AuthController(IUserRepository userRepo, ITokenService tokenService, IPasswordHasher passwordHasher, LoginUserService loginUserService)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _loginUserService = loginUserService;
    }

    [HttpPost, Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        if (loginModel is null)
        {
            return BadRequest("Invalid client request");
        }

        var user = await _loginUserService.Handle(new LoginUserService.Request(loginModel.Email, loginModel.Password));

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();
        user.Account.SetRefreshToken(refreshToken);
        user.Account.SetRefreshTokenExpTime(DateTime.UtcNow.AddDays(7));
        await _userRepo.SaveChangesAsync();
        return Ok(new TokenApiModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }

    [HttpPost, Route("rejstacja")]
    public async Task<IActionResult> Rejstracja([FromBody] UserRegister model)
    {
        if(!model.Password.Equals(model.PasswordConfirmation))
        {
            return BadRequest();
        }
        var userc = await _userRepo.GetUserByEmail(model.Email);
        if (userc is not null)
        {
            return BadRequest();
        }
        var accountId = Guid.NewGuid();
        await _userRepo.AddUser(new User(Guid.NewGuid(), model.Name, model.Lastname, model.Rola, accountId, new Account(accountId, model.Email, _passwordHasher.Hash(model.Password))));
        await _userRepo.SaveChangesAsync();
        var user = await _userRepo.GetUserByEmail(model.Email);
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.Role, model.Rola.ToString())
            };
        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();
        user.Account.SetRefreshToken(refreshToken);
        user.Account.SetRefreshTokenExpTime(DateTime.UtcNow.AddDays(7));
        await _userRepo.SaveChangesAsync();
        return Ok(new TokenApiModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }
    /// <summary>
    /// Retrieves details of the current user
    /// </summary>
    /// <returns>UserModel object with user details</returns>
    [HttpGet("GetUserDetails"), Authorize]
    public async Task<IActionResult> GetUserDetails()
    {
        string? email = User?.Identity?.Name;
        if (email == null)
            return BadRequest("User's e-mail data is missing from the token!");

        User? dbUser = await _userRepo.GetUserByEmail(email);
        if (dbUser == null)
            return NotFound("User with the given mail not found in the system!");

        UserModel answer = new(dbUser.Name, dbUser.Lastname, dbUser.Role.ToString(), dbUser.Id);
        return new JsonResult(answer);
    }
}
