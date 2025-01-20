using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using YouTubeMusicAPI.Client;
using YouTubeMusicAPI.Models.Info;

namespace YouTubeMusicAPI.Tests;

/// <summary>
/// Get information
/// </summary>
internal class Get
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
    /// Get browse id from an album
    /// </summary>
    [Test]
    public void AlbumBrowseId()
    {
        string? browseId = null;
        Assert.DoesNotThrowAsync(async () =>
        {
            browseId = await client.GetAlbumBrowseIdAsync(TestData.AlbumId);
        });
        Assert.That(browseId, Is.Not.Null);

        // Output
        logger.LogInformation("\nAlbum Browse Id: {browseId} ", browseId);
    }
    
    /// <summary>
    /// Get browse id from a playlist
    /// </summary>
    [Test]
    public void CommunityPlaylistBrowseId()
    {
        string? browseId = null;
        Assert.DoesNotThrow(() =>
        {
            browseId = client.GetCommunityPlaylistBrowseId(TestData.PlaylistId);
        });
        Assert.That(browseId, Is.Not.Null);

        // Output
        logger.LogInformation("\nPlaylist Browse Id: {browseId} ", browseId);
    }


    /// <summary>
    /// Get song information
    /// </summary>
    [Test]
    public void SongVideo()
    {
        SongVideoInfo? song = null;

        Assert.DoesNotThrowAsync(async () =>
        {
            song = await client.GetSongVideoInfoAsync(TestData.SongVideoId);
        });
        Assert.That(song, Is.Not.Null);

        // Output
        string readableResults = JsonConvert.SerializeObject(song, Formatting.Indented);
        logger.LogInformation("\nSong Info:\n{readableResults}", readableResults);
    }

    /// <summary>
    /// Get album information
    /// </summary>
    [Test]
    public void Album()
    {
        AlbumInfo? album = null;

        Assert.DoesNotThrowAsync(async () =>
        {
            album = await client.GetAlbumInfoAsync(TestData.AlbumBrowseId);
        });
        Assert.That(album, Is.Not.Null);

        // Output
        string readableResults = JsonConvert.SerializeObject(album, Formatting.Indented);
        logger.LogInformation("\nAlbum Info:\n{readableResults}", readableResults);
    }

    /// <summary>
    /// Get community playlist information
    /// </summary>
    [Test]
    public void CommunityPlaylist()
    {
        CommunityPlaylistInfo? communityPlaylist = null;

        Assert.DoesNotThrowAsync(async () =>
        {
            communityPlaylist = await client.GetCommunityPlaylistInfoAsync(TestData.PlaylistBrowseId);
        });
        Assert.That(communityPlaylist, Is.Not.Null);

        // Output
        string readableResults = JsonConvert.SerializeObject(communityPlaylist, Formatting.Indented);
        logger.LogInformation("\nPlaylist Info:\n{readableResults}", readableResults);
    }

    /// <summary>
    /// Get artist information
    /// </summary>
    [Test]
    public void Artist()
    {
        ArtistInfo? artist = null;

        Assert.DoesNotThrowAsync(async () =>
        {
            artist = await client.GetArtistInfoAsync(TestData.ArtistBrowseId);
        });
        Assert.That(artist, Is.Not.Null);

        // Output
        string readableResults = JsonConvert.SerializeObject(artist, Formatting.Indented);
        logger.LogInformation("\nArtist Info:\n{readableResults}", readableResults);
    }
}