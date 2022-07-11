using Newtonsoft.Json;
using RestSharp;
using Whatsapp_bot.ServiceContracts;

namespace Whatsapp_bot.Services;
public class HttpService<TInput, TResult> : IHttpService<TInput, TResult>
{
    public async Task<TResult> PostAsync(string url, TInput data, string token = "")
    {
        if (data is null)
        {
            throw new ArgumentNullException("Null data");
        }

        string jsonString = JsonConvert.SerializeObject(data);

        if (string.IsNullOrEmpty(jsonString))
        {
            throw new ArgumentNullException("Null data");
        }

        var client = new RestClient(url);

        var request = new RestRequest();
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + token);

        request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
        try
        {
            var response = await client.PostAsync(request);

            if (typeof(TResult).Equals(typeof(string)))
            {
                return (TResult)Convert.ChangeType(response.Content, typeof(TResult));
            }
            return JsonConvert.DeserializeObject<TResult>(response.Content);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}