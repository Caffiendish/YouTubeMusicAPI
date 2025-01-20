using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using YouTubeMusicAPI.Client;
using YouTubeMusicAPI.Models.Streaming;

namespace YouTubeMusicAPI.Tests;

/// <summary>
/// Streaming
/// </summary>
internal class Streaming
{
    ILogger logger;
    YouTubeMusicClient client;
    bool usingCookies = true;

    [SetUp]
    public void Setup()
    {
        ILoggerFactory factory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        logger = factory.CreateLogger<Search>();

        if (usingCookies)
        {
            IEnumerable<Cookie> parsedCookies = TestData.CookiesString
                .Split(";")
                .Select(cookieString =>
                {
                    string[] parts = cookieString.Split("\t");
                    return new Cookie(parts[0], parts[1]) { Domain = ".youtube.com" };
                });
            client = new(logger, TestData.GeographicalLocation, parsedCookies);
        }
        else
        {
            client = new(logger, TestData.GeographicalLocation);
        }
    }


    /// <summary>
    /// Get streaming data
    /// </summary>
    [Test]
    public void StreamingData()
    {
        StreamingData? data = null;
        Assert.DoesNotThrowAsync(async () =>
        {
            data = await client.GetStreamingDataAsync(TestData.SongVideoId);
        });
        Assert.That(data, Is.Not.Null);
        Assert.That(data.StreamInfo, Is.Not.Empty);

        // Output
        string readableResults = JsonConvert.SerializeObject(data, Formatting.Indented);
        logger.LogInformation("\nStreaming Data:\n{data}", readableResults);
    }

    /// <summary>
    /// Get stream
    /// </summary>
    [Test]
    public void Stream()
    {
        Stream? stream = null;
        Assert.DoesNotThrowAsync(async () =>
        {
            StreamingData data = await client.GetStreamingDataAsync(TestData.SongVideoId);
            Assert.That(data, Is.Not.Null);
            Assert.That(data.StreamInfo, Is.Not.Empty);

            stream = await data.StreamInfo[0].GetStreamAsync();
        });
        Assert.That(stream, Is.Not.Null);

        // Output
        logger.LogInformation("\nStream Lenght:\n{streamLenght}", stream.Length);
    }


    /// <summary>
    /// Download to file
    /// </summary>
    [Test]
    public void Download()
    {
        Assert.DoesNotThrowAsync(async () =>
        {
            StreamingData data = await client.GetStreamingDataAsync(TestData.SongVideoId);
            Assert.That(data, Is.Not.Null);
            Assert.That(data.StreamInfo, Is.Not.Empty);

            Stream stream = await data.StreamInfo[0].GetStreamAsync();
            Assert.That(stream, Is.Not.Null);
            Assert.That(stream.Length, Is.GreaterThan(0));

            using FileStream fileStream = new(TestData.FilePath, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(fileStream);
        });

        // Output
        logger.LogInformation("Download finished");
    }
}