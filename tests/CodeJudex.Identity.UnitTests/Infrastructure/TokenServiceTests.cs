using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CodeJudex.Identity.Domain.Models;
using CodeJudex.Identity.Infrastructure.Authentication;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace CodeJudex.Identity.UnitTests.Infrastructure;

public class TokenServiceTests
{
    private readonly TokenService _tokenService;
    private readonly JwtOptions _jwtOptions;

    public TokenServiceTests()
    {
        _jwtOptions = new JwtOptions
        {
            Secret = "SuperSecretKeyForTestingPurposesOnly123!",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiryMinutes = 60
        };

        var optionsMock = new Mock<IOptions<JwtOptions>>();
        optionsMock.Setup(x => x.Value).Returns(_jwtOptions);

        _tokenService = new TokenService(optionsMock.Object);
    }

    [Fact]
    public void GenerateTokens_WhenCalled_ShouldReturnValidTokens()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@user.com",
            FullName = "Test User"
        };
        var roles = new List<string> { "Admin", "Student" };

        // Act
        var (accessToken, refreshToken, expiresAt) = _tokenService.GenerateTokens(user, roles);

        // Assert
        accessToken.Should().NotBeNullOrEmpty();
        refreshToken.Should().NotBeNullOrEmpty();
        expiresAt.Should().BeAfter(DateTime.UtcNow);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(accessToken);

        token.Issuer.Should().Be(_jwtOptions.Issuer);
        token.Audiences.Should().Contain(_jwtOptions.Audience);
        token.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == user.Id.ToString());
        token.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
        token.Claims.Should().Contain(c => c.Type == "fullName" && c.Value == user.FullName);
    }

    [Fact]
    public void GetClaimsFromToken_WhenTokenIsValidButExpired_ShouldReturnClaims()
    {
        // Arrange
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: new[] { new Claim(ClaimTypes.Name, "test@user.com") },
            expires: DateTime.UtcNow.AddMinutes(-10),
            signingCredentials: creds
        );

        var expiredToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        // Act
        var principal = _tokenService.GetClaimsFromToken(expiredToken);

        // Assert
        principal.Should().NotBeNull();
        principal.Identity?.Name.Should().Be("test@user.com");
    }

    [Fact]
    public void GetClaimsFromToken_WhenTokenSignatureIsInvalid_ShouldThrowException()
    {
        // Arrange
        var invalidToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.e30.invalid_signature";

        // Act
        var act = () => _tokenService.GetClaimsFromToken(invalidToken);

        // Assert
        act.Should().Throw<Exception>();
    }
}