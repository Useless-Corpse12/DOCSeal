namespace DOCSeal.Infrastructure.DataContext.Exceptions;

public class DbBuildValueException : Exception
{
    public DbBuildValueException(string paramName) 
        : base($"> parameter: '{paramName}' with !null! or !empty! value making me shitting my pants.") { }
}

public class DbBuildAnyException : Exception
{
    public DbBuildAnyException(Exception innerException) 
        : base($"> I just shitting my pants with no any reason.", innerException) { }
}