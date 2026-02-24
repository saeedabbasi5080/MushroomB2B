namespace MushroomB2B.Domain.Exceptions;

public sealed class DomainException(string message) : Exception(message);
