namespace CodeJudex.Identity.Domain.Exceptions;

public class UnauthorizedException(string message) : BaseException(message, 401);