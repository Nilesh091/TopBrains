namespace UserService.Application.DTOs
{
  public class VerifyOtpDTO
  {
    public Guid UserId { get; set; }
    public string OtpCode { get; set; } = null!;
    public string ClientId { get; set; } = null!;
  }
}
