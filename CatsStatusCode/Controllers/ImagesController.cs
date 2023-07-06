using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CatsStatusCode.Controllers;

[ApiController]
[Route("imgStatus")]
public class ImagesController: ControllerBase
{
    private readonly IMemoryCache _cache;
    
    public ImagesController(IMemoryCache cache)
    {
        this._cache = cache;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetImage(string url)
    {
        try
        {
            using (var client = new HttpClient())
            {
                //get request to web resource
                HttpResponseMessage response = await client.GetAsync(url);
                int statusCode = (int)response.StatusCode;
        
                //check cache
                if (_cache.TryGetValue(statusCode, out byte[] imgCache))
                {
                    return File(imgCache, "image/jpeg");
                }
            
                //get img from resource
                string apiCatUrl = $"https://http.cat/{statusCode}.jpg";
                HttpResponseMessage responseCat = await client.GetAsync(apiCatUrl);

                if (responseCat.StatusCode == HttpStatusCode.OK)
                {
                    //get img
                    byte[] imgByte = await responseCat.Content.ReadAsByteArrayAsync();

                    //send img for caching
                    Task.Run(() => CacheImg(statusCode, imgByte));
                
                    return File(imgByte, "image/jpeg");
                }
                else
                {
                    return BadRequest("Error. Cats asleep...");
                }
            }
        }
        catch (Exception e)
        {
            return BadRequest("Error. Check your url");
        }
    }

    private void CacheImg(int key, byte[] value)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(1));
        try
        {
            _cache.Set(key, value, cacheEntryOptions);
        }
        catch (Exception e)
        {
            Console.WriteLine("Problem with cache");
        }
    }

}