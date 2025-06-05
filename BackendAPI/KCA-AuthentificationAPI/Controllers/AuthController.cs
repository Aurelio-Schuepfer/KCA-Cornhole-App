using KCA_AuthentificationAPI.Data;
using KCA_AuthentificationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KCA_AuthentificationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;
        private readonly IEmailSender<AppUser> _emailSender;

        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration,
            TokenService tokenService,
            IEmailSender<AppUser> emailSender
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _emailSender = emailSender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AppRegisterRequest registerRequest)
        {
            var user = new AppUser { UserName = registerRequest.UserName, Email = registerRequest.Email, EmailConfirmed = false };
            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { userId = user.Id, token }, Request.Scheme);
            await _emailSender.SendConfirmationLinkAsync(user, user.Email, confirmationLink);

            return Ok(new { requiresEmailConfirmation = true });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AppLoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName)
                ?? await _userManager.FindByEmailAsync(request.UserName);

            if (user == null)
                return Unauthorized("Invalid username/email or password.");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return Unauthorized("Please confirm your email first.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                return Unauthorized("Invalid username/email or password.");

            var token = _tokenService.CreateToken(user, request.RememberMe); 
            return Ok(new { Token = token });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(Guid userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return BadRequest("Invalid Link.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return Ok("E-Mail succesfully confirmed");
            else
                return BadRequest("Confirmation Failed please try again or contact support");
        }

        [HttpGet("secure-data")]
        [Authorize]
        public async Task<IActionResult> GetSecureData()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return Unauthorized();

            return Ok(new
            {
                Username = user.UserName,
                email = user.Email,
            });
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(new { error = "EMAIL_REQUIRED" });


            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                return Ok();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action(
                "ResetPassword",
                "Auth",
                new { email = user.Email, token = Uri.EscapeDataString(token) },
                protocol: Request.Scheme
            );

            await _emailSender.SendPasswordResetLinkAsync(user, user.Email, resetLink);

            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return BadRequest("Invalid request.");

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (result.Succeeded)
                return Ok("Password reset successful");
            else
                return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst("sub")?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var canChangeUsername = !user.LastUsernameChange.HasValue ||
                (DateTime.UtcNow - user.LastUsernameChange.Value).TotalDays >= 7;

            return Ok(new
            {
                username = user.UserName,
                email = user.Email,
                canChangeUsername
            });
        }
    }
}