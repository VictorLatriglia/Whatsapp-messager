public interface IHttpService<TInput, TResult>
{
    Task<TResult> PostAsync(string url, TInput data, string token = "");
}