using DietManagementSystem.Application.DTOs;
using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DietManagementSystem.Application.Features.User.Commands.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    public LoginUserCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }
    public async Task<LoginUserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser user = null;
        if (!string.IsNullOrEmpty(request.userName))
        {
            user = await _userManager.FindByNameAsync(request.userName);
        }
        else if (!string.IsNullOrEmpty(request.email))
        {
            user = await _userManager.FindByEmailAsync(request.email);
        }

        var exception = new BadRequestException("Kullanıcı adı veya şifre hatalı.");
        if (user == null) throw exception;

        var passwordValid = await _userManager.CheckPasswordAsync(user, request.password);
        if (!passwordValid) throw exception;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
        };

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expireDays = double.Parse(_configuration["Jwt:ExpireDays"]);
        var expires = DateTime.UtcNow.AddDays(expireDays);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new LoginUserDto(tokenString, expires);
    }
}
