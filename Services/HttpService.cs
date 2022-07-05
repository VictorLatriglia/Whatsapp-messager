using Newtonsoft.Json;
using RestSharp;
public class HttpService<TInput, TResult> : IHttpService<TInput, TResult>
{
    public async Task<TResult> PostAsync(string url, TInput data, string token = "")
    {
        string jsonString = JsonConvert.SerializeObject(data);

        var client = new RestClient(url);

        var request = new RestRequest();
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
        var response = await client.PostAsync(request);

        if (response?.Content == null)
            throw new Exception($"Request failed  {response?.ErrorMessage}  {response?.StatusCode}");

        if (typeof(TResult).Equals(typeof(string)))
        {
            return (TResult)Convert.ChangeType(response.Content, typeof(TResult));
        }
        return JsonConvert.DeserializeObject<TResult>(response.Content);
    }
}