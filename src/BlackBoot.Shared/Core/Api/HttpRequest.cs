using Newtonsoft.Json;

namespace BlackBoot.Shared.Core.Api;

public static class HttpRequest
{
    public static async Task<T> GetAsync<T>(string url, Dictionary<string, string> parameter)
    {
        var param = string.Empty;
        if (parameter is not null)
            foreach (var item in parameter)
                param += $"{item.Key}={item.Value}&";
        var completeUrl = string.IsNullOrWhiteSpace(param) ? url : $"{url}?{param[0..^1]}";

        using HttpClient client = new();
        var response = await client.GetAsync(completeUrl);
        var result = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(result);
    }
}
