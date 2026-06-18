namespace DOCSeal.Infrastructure.DataContext.Exceptions;

public class EntityParamNotFound(string entityName, string paramName) : Exception(
    $">entity: '{entityName}' with parameter: '{paramName}' with !null! or !empty! value making me shitting my pants.");

public class EntityInstanceIdNotFound(string entityName, Guid instanceId) : Exception(
    $">entity: '{entityName}' with id: '{instanceId}' not found check up ur code");