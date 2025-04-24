using System.Net.Http;

namespace CIMEX_Project;

public class ApiClient
{
    private static readonly Lazy<HttpClient> _apiClientInstance = new Lazy<HttpClient>(() =>
    {
        return new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7031/api/Study/"),
            Timeout = TimeSpan.FromSeconds(120)
        };
    });

    private ApiClient()
    {
       
    }

    public static HttpClient Instance => _apiClientInstance.Value;

}