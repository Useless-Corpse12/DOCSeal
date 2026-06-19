using Microsoft.Extensions.Caching.Memory;
namespace DOCSeal.Infrastructure.Services.VerificationCode;

public class VerificationCodeWorker(IMemoryCache cache):IVerificationCodeService
{
    private readonly IMemoryCache _cache = cache;   
    
    public string GenerateAndSaveCode(Guid userId)
    {
        var code = new Random().Next(100000, 999999).ToString();
        
        var cacheKey = $"vrf_code:{userId}";
//                                                                                                  \/
        var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));//   <==
//                                                                                                  /\
        _cache.Set(cacheKey, code, cacheOptions);
        
        return code;
    }
    
    public bool ValidateCode(Guid userId, string code)
    {
        var cacheKey = $"vrf_code:{userId}";
        
        if (_cache.TryGetValue(cacheKey, out string? cachedCode))
        {
            return cachedCode == code;
        }
        
        return false;
    }
    
    public void RemoveCode(Guid userId)
    {
        var cacheKey = $"vrf_code:{userId}";
        
        _cache.Remove(cacheKey);
    }
}