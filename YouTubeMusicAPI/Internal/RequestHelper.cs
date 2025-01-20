using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace YouTubeMusicAPI.Internal;

/// <summary>
/// Helper which handles all HTTP requests
/// </summary>
internal class RequestHelper
{
    readonly ILogger? logger;

    readonly HttpClient httpClient;

    /// <summary>
    /// Creates a new request helper
    /// </summary>
    /// <param name="cookies">Initial cookies used for authentication</param>
    public RequestHelper(
        IEnumerable<Cookie>? cookies = null)
    {
        httpClient = new(new CookiesHttpHandler(cookies));
    }

    /// <summary>
    /// Creates a new request helper
    /// </summary>
    /// <param name="logger">The optional logger used for logging</param>
    /// <param name="cookies">Initial cookies used for authentication</param>
    public RequestHelper(
        ILogger logger,
        IEnumerable<Cookie>? cookies = null)
    {
        this.logger = logger;

        httpClient = new(new CookiesHttpHandler(cookies));

        logger.LogInformation($"[RequestHelper-.ctor] RequestHelper with extendended logging functions has been initialized.");
    }


    /// <summary>
    /// Sends a new GET request to the given uri with the parameters
    /// </summary>
    /// <param name="url">The uri the request should be made to</param>
    /// <param name="parameters">The query parameters which should be added</param>
    /// <param name="cancellationToken">The cancellation token to cancel the action</param>
    /// <exception cref="System.InvalidOperationException">May occurs when sending the web request fails</exception>
    /// <exception cref="System.Net.Http.HttpRequestException">May occurs when sending the web request fails</exception>
    /// <exception cref="System.Threading.Tasks.TaskCanceledException">Occurs when The task was cancelled</exception>
    /// <returns>The HTTP response message</returns>
    public Task<HttpResponseMessage> GetAsync(
        string url,
        string? parameters = null,
        CancellationToken cancellationToken = default,
        BrowserType browser = BrowserType.Chrome)
    {
        // Create HTTP request
        HttpRequestMessage request = new()
        {
            Method = HttpMethod.Get,
            RequestUri = new UriBuilder(url) { Query = parameters }.Uri
        };

        AddUserAgent(request, browser);

        // Send HTTP request
        logger?.LogInformation($"[RequestHelper-GetAsync] Sending HTTP reuqest. GET: {url}.");
        return httpClient.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// Sends a new GET request to the given uri with the parameters and validates it
    /// </summary>
    /// <param name="uri">The uri the request should be made to</param>
    /// <param name="parameters">The query parameters which should be added</param>
    /// <param name="cancellationToken">The cancellation token to cancel the action</param>
    /// <exception cref="System.InvalidOperationException">May occurs when sending the web request fails</exception>
    /// <exception cref="System.Net.Http.HttpRequestException">May occurs when sending the web request fails</exception>
    /// <exception cref="System.Threading.Tasks.TaskCanceledException">Occurs when The task was cancelled</exception>
    /// <returns>The validated HTTP response data</returns>
    public async Task<string> GetAndValidateAsync(
        string uri,
        string? parameters = null,
        CancellationToken cancellationToken = default,
        BrowserType browser = BrowserType.Chrome)
    {
        // Send HTTP request
        HttpResponseMessage httpResponse = await GetAsync(uri, parameters, cancellationToken).ConfigureAwait(false);

        // Parse HTTP response data
        logger?.LogInformation($"[RequestHelper-GetAndValidateAsync] Parsing HTTP response.");
        string httpResponseData = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

        // Check for exception
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger?.LogError($"[RequestHelper-GetAndValidateAsync] HTTP request failed. Statuscode: {httpResponse.StatusCode}.");
            throw new HttpRequestException($"HTTP request failed. StatusCode: {httpResponse.StatusCode}.", new(httpResponseData));
        }

        // Return response data
        return httpResponseData;
    }


    /// <summary>
    /// Sends a new POST request to the given uri with the serializes body
    /// </summary>
    /// <param name="url">The uri the request should be made to</param>
    /// <param name="body">The body which should be serialized</param>
    /// <param name="parameters">The query parameters which should be added</param>
    /// <param name="cancellationToken">The cancellation token to cancel the action</param>
    /// <exception cref="System.NotSupportedException">May occurs when the json serialization fails</exception>
    /// <exception cref="System.InvalidOperationException">May occurs when sending the web request fails</exception>
    /// <exception cref="System.Net.Http.HttpRequestException">May occurs when sending the web request fails</exception>
    /// <exception cref="System.Threading.Tasks.TaskCanceledException">Occurs when The task was cancelled</exception>
    /// <returns>The HTTP response message</returns>
    public Task<HttpResponseMessage> PostAsync(
        string url,
        object? body = null,
        string? parameters = null,
        CancellationToken cancellationToken = default,
        BrowserType browser = BrowserType.Chrome)
    {
        // Create HTTP request
        HttpRequestMessage request = new()
        {
            Method = HttpMethod.Post,
            RequestUri = new UriBuilder(url) { Query = parameters }.Uri,
        };

        if (body is not null)
            request.Content = new StringContent(JsonConvert.SerializeObject(body, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }), Encoding.UTF8, "application/json");

        AddUserAgent(request, browser);

        // Send HTTP request
        logger?.LogInformation($"[RequestHelper-PostBodyAsync] Sending HTTP reuqest. POST: {url}.");
        return httpClient.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// Sends a new POST request to the given uri with the serializes body and validates it
    /// </summary>
    /// <param name="uri">The uri the request should be made to</param>
    /// <param name="body">The body which should be serialized</param>
    /// <param name="parameters">The query parameters which should be added</param>
    /// <param name="cancellationToken">The cancellation token to cancel the action</param>
    /// <exception cref="System.NotSupportedException">May occurs when the json serialization fails</exception>
    /// <exception cref="System.InvalidOperationException">May occurs when sending the web request fails</exception>
    /// <exception cref="System.Net.Http.HttpRequestException">May occurs when sending the web request fails</exception>
    /// <exception cref="System.Threading.Tasks.TaskCanceledException">Occurs when The task was cancelled</exception>
    /// <returns>The validated HTTP response data</returns>
    public async Task<string> PostAndValidateAsync(
        string uri,
        object? body = null,
        string? parameters = null,
        CancellationToken cancellationToken = default,
        BrowserType browser = BrowserType.Chrome)
    {
        // Send HTTP request
        HttpResponseMessage httpResponse = await PostAsync(uri, body, parameters, cancellationToken).ConfigureAwait(false);

        // Parse HTTP response data
        logger?.LogInformation($"[RequestHelper-PostBodyAndValidateAsync] Parsing HTTP response.");
        string httpResponseData = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

        // Check for exception
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger?.LogError($"[RequestHelper-PostBodyAndValidateAsync] HTTP request failed. Statuscode: {httpResponse.StatusCode}.");
            throw new HttpRequestException($"HTTP request failed. StatusCode: {httpResponse.StatusCode}.", new(httpResponseData));
        }

        // Return response data
        return httpResponseData;
    }


    private static void AddUserAgent(HttpRequestMessage message, BrowserType browser)
    {
        switch(browser)
        {
            case BrowserType.IOS:
                {
                    message.Headers.Add("User-Agent", "com.google.ios.youtube/19.29.1 (iPhone16,2; U; CPU iOS 17_5_1 like Mac OS X;)");
                }break;
            case BrowserType.Chrome:
                {
                    message.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/132.0.0.0 Safari/537.36");
                }break;
        }
    }

    public enum BrowserType
    {
        IOS,
        Chrome,
    }
}