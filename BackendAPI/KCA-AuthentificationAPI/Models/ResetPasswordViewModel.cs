public class ResetPasswordViewModel
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}