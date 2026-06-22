namespace CodeJudex.Identity.Application.DTOs.Responses;

public record AuthResponseDto(
    string AccessToken,
    string RefreshToken,
    string Email,
    string FullName,
    DateTime ExpiresAt
);