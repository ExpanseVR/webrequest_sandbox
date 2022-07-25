using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class WebService : MonoBehaviour
{
    private const string StagingBaseUrl = "https://mandq-dev-server.viewport.com.au";
    
    private static string BaseURL = StagingBaseUrl;
    private readonly IPoolService poolService;

    private readonly HttpClient client;

    public WebService()
    {
        poolService = new DefaultPoolService();

        client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(60);
    }
    
    public async Task<HttpResult> MakeRequest<TReq, TResp>(TReq request) where TReq : WebRequestBase where TResp : WebResponseBase, new()
    {
        var contentString = JsonConvert.SerializeObject(request);
        StringContent content = new StringContent(
            contentString,
            Encoding.UTF8,
            "application/json"
        );
        
        var response = poolService.Get<TResp>();
        
        try
        {
            var url = $"{BaseURL}/{request.UrlPath()}";

            HttpRequestMessage webRequest = new HttpRequestMessage(request.Method(), url);
            webRequest.Content = content;
            var webResponse = await client.SendAsync(webRequest);
            if (!webResponse.IsSuccessStatusCode) return await HandleErrorResult(webResponse, response);
            var responseBody = await webResponse.Content.ReadAsStringAsync();
            JsonConvert.PopulateObject(responseBody, response);
            return new HttpResult() { Success = true, Response = response};
        }
        catch (WebException webException)
        {
            response.Error = webException.ToString();
            return new HttpResult()
            {
                Success = false,
                Response = response
            };
        }
        catch (TaskCanceledException)
        {
            response.Error = "Timed out contacting server";
            return new HttpResult()
            {
                Success = false,
                Response = response,
            };
        }
    }
    
    private async Task<HttpResult> HandleErrorResult(HttpResponseMessage webResponse, WebResponseBase response)
    {
        switch (webResponse.StatusCode)
        {
            case HttpStatusCode.InternalServerError:
            case HttpStatusCode.BadGateway:
            case HttpStatusCode.NotImplemented:
            case HttpStatusCode.ServiceUnavailable:
            case HttpStatusCode.GatewayTimeout:
            case HttpStatusCode.HttpVersionNotSupported:
                response.Error = $"Server Error Occurred: {webResponse.StatusCode}";
                break;
            case HttpStatusCode.Unauthorized:
                response.Error = $"Invalid Username or Password";
                break;
            case HttpStatusCode.NotFound:
                response.Error = $"Not found";
                break;
            default:
                var responseBody = await webResponse.Content.ReadAsStringAsync();
                JsonConvert.PopulateObject(responseBody, response);
                break;
        }

        return new HttpResult()
        {
            Response = response,
            Success = false,
        };
    }
}

public struct HttpResult
{
    public bool Success;
    public WebResponseBase Response;
}

public abstract class WebRequestBase : PoolBase
{
    public abstract string UrlPath();
    public abstract HttpMethod Method();

    public abstract Type ResponseType();
}

public abstract class WebResponseBase : PoolBase
{
    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }

    public override void Reset()
    {
        Status = null;
        Error = null;
    }
}    