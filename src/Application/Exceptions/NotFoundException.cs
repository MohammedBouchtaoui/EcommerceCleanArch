namespace Application.Exceptions;

public class NotFoundException(string entity, object key)
    : Exception($"{entity} avec l'identifiant '{key}' est introuvable.");
