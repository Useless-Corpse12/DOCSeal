using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DOCSeal.Infrastructure.DataContext.Exceptions;

public class EntityParamNotFound(string entityType, string paramName) : Exception(
    $">entity: '{entityType}' with parameter: '{paramName}' with !null! or !empty! value making me shitting my pants.");

public class EntityInstanceIdNotFound(string entityName, Guid instanceId) : Exception(
    $">entity: '{entityName}' with id: '{instanceId}' not found check up ur code");