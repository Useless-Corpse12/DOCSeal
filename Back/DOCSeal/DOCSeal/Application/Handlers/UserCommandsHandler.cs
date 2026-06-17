using DOCSeal.Infrastructure.DataContext;


namespace DOCSeal.Application.Handlers;


public class UserCommandsHandler
{
    private AppDbContext DbContext { get; } = AppDbContext;
    public async Task<string> RegisterSelfAsync(RegisterSelfCommand cmd)
    {
        
        
        
    }
    
    public async Task<string> RegisterOtherAsync(RegisterOtherCommand cmd)
    {
        
        
        
    }

    public async Task<string> AuthAsync(AuthCommand cmd)
    {
        
    }

    public async Task<string> VerifyAsync(VerifyCommand cmd)
    {
    
        
        
    }

    public async Task<string> ChangePassAsync(ChangePassCommand cmd)
    {
        
    }
}