namespace CodeJudex.Identity.Domain.Exceptions;

public class BadRequestException(string message) : BaseException(message, 400);