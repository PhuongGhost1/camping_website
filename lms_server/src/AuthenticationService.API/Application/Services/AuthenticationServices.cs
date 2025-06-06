﻿using AuthenticationService.API.Application.DTOs.Authentication;
using AuthenticationService.API.Application.DTOs.Mail;
using AuthenticationService.API.Application.DTOs.OTP;
using AuthenticationService.API.Application.Shared.Constant;
using AuthenticationService.API.Application.Shared.Constant.Type;
using AuthenticationService.API.Core.Jwt;
using AuthenticationService.API.Core.Mail;
using AuthenticationService.API.Domain;
using AuthenticationService.API.Infrastructure.Cache;
using AuthenticationService.API.Infrastructure.Messaging.Publisher;
using AuthenticationService.API.Infrastructure.Repository.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.API.Application.Services;

public interface IAuthenticationService
{
    Task<IActionResult> Login(LoginRequest req);
    Task<IActionResult> Register(RegisterRequest req);
    Task<IActionResult> RefreshToken(RefreshTokenRequest req);
    Task<IActionResult> LogOut(Guid userId);
    Task<IActionResult> VerifyEmail(RegisterVerifyRequest req);
    Task<IActionResult> VerifyOtp(VerifyOtpRequest req);
}
public class AuthenticationServices : IAuthenticationService
{
    private readonly IAuthenticationRepository _authRepo;
    private readonly IJwtService _jwtService;
    private readonly IEventPublisher _eventPublisher;
    private readonly IMailService _mailService;
    private readonly ICacheService _cacheService;
    public AuthenticationServices(IAuthenticationRepository authRepo, IJwtService jwtService,
    IEventPublisher eventPublisher, IMailService mailService, ICacheService cacheService)
    {
        _authRepo = authRepo;
        _jwtService = jwtService;
        _eventPublisher = eventPublisher;
        _mailService = mailService;
        _cacheService = cacheService;
    }
    public async Task<IActionResult> Login(LoginRequest req)
    {
        try
        {
            var user = await _authRepo.Login(req.email, _jwtService.HashObject<string>(req.pwd));

            if (user is null) return ErrorResp.Unauthorized("Login Failed!");

            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenObj = new Refreshtokens
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpirationDate = DateTime.UtcNow.AddDays(JwtConst.REFRESH_TOKEN_EXP),
                CreatedAt = DateTime.UtcNow
            };

            var isSaved = await _authRepo.SaveRefreshToken(refreshTokenObj);
            if (!isSaved) return ErrorResp.BadRequest("Failed to save refresh token!");

            return SuccessResp.Ok(new LoginResponse
            (
                user.Email,
                user.PasswordHash,
                _jwtService.GenerateToken(user.Id, Guid.NewGuid(), user.Email, JwtConst.ACCESS_TOKEN_EXP),
                refreshTokenObj.Token
            ));
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> LogOut(Guid userId)
    {
        try
        {
            if(userId == Guid.Empty || userId == null) 
                return ErrorResp.Unauthorized("User not authorized!");

            var isDeleted = await _authRepo.DeleteRefreshTokenByUserId(userId);
            if (!isDeleted) return ErrorResp.BadRequest("Failed to delete refresh token!");

            return SuccessResp.Ok("Logout Successfully!");
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> RefreshToken(RefreshTokenRequest req)
    {
        try
        {
            var rftToken = await _authRepo.GetRefreshToken(req.refreshToken);

            if (rftToken is null) 
                return ErrorResp.Unauthorized("Invalid refresh token!");

            if (rftToken.ExpirationDate < DateTime.UtcNow) 
                return ErrorResp.Unauthorized("Refresh token expired!");

            rftToken.Token = _jwtService.GenerateRefreshToken();
            rftToken.ExpirationDate = DateTime.UtcNow.AddDays(JwtConst.REFRESH_TOKEN_EXP);

            var isUpdated = await _authRepo.UpdateRefreshToken(rftToken);
            if (!isUpdated) return ErrorResp.BadRequest("Failed to update refresh token!");

            var user = await _authRepo.GetUserById(rftToken.UserId);
            if (user is null) return ErrorResp.Unauthorized("User not found!");

            var newAccessToken = _jwtService.GenerateToken(user.Id, Guid.NewGuid(), user.Email, JwtConst.ACCESS_TOKEN_EXP);

            return SuccessResp.Ok(new RefreshTokenResponse
            (
                newAccessToken,
                rftToken.Token
            ));
                
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> Register(RegisterRequest req)
    {
        try
        {
            var newUser = new Users
            {
                Name = req.name,
                Email = req.email,
                PasswordHash = _jwtService.HashObject<string>(req.pwd)
            };

            var user = await _authRepo.Register(newUser);

            _eventPublisher.PublishUserRegisteredIn(userId: newUser.Id);

            return user ? SuccessResp.Ok(new RegisterResponse(newUser, "Register Successfully!")) 
            : ErrorResp.BadRequest("Faild to create user!");
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> VerifyEmail(RegisterVerifyRequest req)
    {
        try
        {
            var isExist = await _authRepo.IsEmailExists(req.email);
            if (isExist) return ErrorResp.BadRequest("Email already exists!");

            var otp = new Random().Next(100000, 999999).ToString();

            await _cacheService.Set(
                $"otp:{req.email}",
                new RegisterVerifyResponse(req.name, req.email, req.pwd, otp),
                TimeSpan.FromMinutes(5));

            var mailRequest = new MailRequest
            {
                ToEmail = req.email,
                Subject = "Your OTP Code",
                Body = $"<h2>Your verification OTP code is: {otp}</h2>"
            };

            await _mailService.SendEmailAsync(mailRequest);

            return SuccessResp.Ok("OTP sent to email!");
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> VerifyOtp(VerifyOtpRequest req)
    {
        try
        {
            var cachedOtp = await _cacheService.Get<RegisterVerifyResponse>($"otp:{req.Email}");

            if (cachedOtp == null)
                return ErrorResp.BadRequest("OTP expired or not found");

            if (cachedOtp.otp != req.Otp)
                return ErrorResp.BadRequest("Invalid OTP");

            await _cacheService.Remove($"otp:{req.Email}");

            return await Register(new RegisterRequest
            (
                cachedOtp.name,
                cachedOtp.email,
                cachedOtp.pwd
            ));
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}
