namespace DOCSeal.Infrastructure.DataContext.Exceptions;

public class DbBuildValueException(string paramName)
    : Exception($"> parameter: '{paramName}' with !null! or !empty! value making me shitting my pants.");

public class DbBuildAnyException(Exception innerException)
    : Exception("> I just shitting my pants with no any reason.", innerException);