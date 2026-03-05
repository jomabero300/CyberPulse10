using System.Net;

namespace CyberPulse10.Frontend.Respositories;

public class HttpResponseWrapper<T>
{
    public bool Error { get; set; }
    public T? Response { get; set; }
    public HttpResponseMessage HttpResponseMessage { get; set; }

    public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
    {
        HttpResponseMessage = httpResponseMessage;
        Response = response;
        Error = error;
    }

    public async Task<string?> GetErrorMessageAsync()
    {
        if (!Error)
        {
            return null;
        }

        var statusCode = HttpResponseMessage.StatusCode;

        if (statusCode == HttpStatusCode.NotFound)
        {
            return "HRW001";
        }
        else if (statusCode == HttpStatusCode.BadRequest)
        {
            return await HttpResponseMessage.Content.ReadAsStringAsync();
        }
        else if (statusCode == HttpStatusCode.Unauthorized)
        {
            return "HRW002";
        }
        else if (statusCode == HttpStatusCode.Forbidden)
        {
            return "HRW003";
        }

        return "HRW004";
    }
}