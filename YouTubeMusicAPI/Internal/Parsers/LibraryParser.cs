﻿using Newtonsoft.Json.Linq;
using System.Globalization;
using YouTubeMusicAPI.Models;
using YouTubeMusicAPI.Models.Library;

namespace YouTubeMusicAPI.Internal.Parsers;

/// <summary>
/// Contains methods to parse library items from json tokens
/// </summary>
internal class LibraryParser
{
    /// <summary>
    /// Parses community playlist data from the json token
    /// </summary>
    /// <param name="jsonToken">The json token containing the item data</param>
    /// <returns>The community playlist</returns>
    /// <exception cref="ArgumentNullException">Occurs when some parsed info is null</exception>
    public static LibraryCommunityPlaylist GetCommunityPlaylist(
        JObject jsonToken)
    {
        JToken[] runs = jsonToken.SelectObject<JToken[]>("musicTwoRowItemRenderer.subtitle.runs");

        string? creatorId = jsonToken.SelectObjectOptional<string>("musicTwoRowItemRenderer.subtitle.runs[0].navigationEndpoint.browseEndpoint.browseId");

        return new(
            name: jsonToken.SelectObject<string>("musicTwoRowItemRenderer.title.runs[0].text"),
            id: jsonToken.SelectObject<string>("musicTwoRowItemRenderer.thumbnailOverlay.musicItemThumbnailOverlayRenderer.content.musicPlayButtonRenderer.playNavigationEndpoint.watchPlaylistEndpoint.playlistId"),
            creator: new(creatorId is null ? "YouTube Music" : jsonToken.SelectObject<string>("musicTwoRowItemRenderer.subtitle.runs[0].text"), creatorId),
            songCount: int.Parse(jsonToken.SelectObject<string>($"musicTwoRowItemRenderer.subtitle.runs[{runs.Length - 1}].text").Split(' ')[0], NumberStyles.AllowThousands, CultureInfo.InvariantCulture),
            radio: jsonToken.SelectRadioOptional("musicTwoRowItemRenderer.menu.menuRenderer.items[1].menuNavigationItemRenderer.navigationEndpoint.watchPlaylistEndpoint.playlistId", null),
            thumbnails: jsonToken.SelectThumbnails("musicTwoRowItemRenderer.thumbnailRenderer.musicThumbnailRenderer.thumbnail.thumbnails"));
    }

    /// <summary>
    /// Parses song data from the json token
    /// </summary>
    /// <param name="jsonToken">The json token containing the item data</param>
    /// <returns>The song</returns>
    /// <exception cref="ArgumentNullException">Occurs when some parsed info is null</exception>
    public static LibrarySong GetSong(
        JObject jsonToken)
    {
        return new(
            name: jsonToken.SelectObject<string>("musicResponsiveListItemRenderer.flexColumns[0].musicResponsiveListItemFlexColumnRenderer.text.runs[0].text"),
            id: jsonToken.SelectObject<string>("musicResponsiveListItemRenderer.flexColumns[0].musicResponsiveListItemFlexColumnRenderer.text.runs[0].navigationEndpoint.watchEndpoint.videoId"),
            artists: jsonToken.SelectArtists("musicResponsiveListItemRenderer.flexColumns[1].musicResponsiveListItemFlexColumnRenderer.text.runs"),
            album: jsonToken.SelectNamedEntity("musicResponsiveListItemRenderer.flexColumns[2].musicResponsiveListItemFlexColumnRenderer.text.runs[0].text", "musicResponsiveListItemRenderer.flexColumns[2].musicResponsiveListItemFlexColumnRenderer.text.runs[0].navigationEndpoint.browseEndpoint.browseId"),
            duration: jsonToken.SelectObject<string>($"musicResponsiveListItemRenderer.fixedColumns[0].musicResponsiveListItemFixedColumnRenderer.text.runs[0].text").ToTimeSpan(),
            isExplicit: jsonToken.SelectIsExplicit("musicResponsiveListItemRenderer.badges"),
            radio: jsonToken.SelectRadio("musicResponsiveListItemRenderer.menu.menuRenderer.items[0].menuNavigationItemRenderer.navigationEndpoint.watchEndpoint.playlistId", "musicResponsiveListItemRenderer.menu.menuRenderer.items[0].menuNavigationItemRenderer.navigationEndpoint.watchEndpoint.videoId"),
            thumbnails: jsonToken.SelectThumbnails("musicResponsiveListItemRenderer.thumbnail.musicThumbnailRenderer.thumbnail.thumbnails"));
    }

    /// <summary>
    /// Parses album data from the json token
    /// </summary>
    /// <param name="jsonToken">The json token containing the item data</param>
    /// <returns>The album</returns>
    /// <exception cref="ArgumentNullException">Occurs when some parsed info is null</exception>
    public static LibraryAlbum GetAlbum(
        JObject jsonToken)
    {
        JToken[] runs = jsonToken.SelectObject<JToken[]>("musicTwoRowItemRenderer.subtitle.runs");

        NamedEntity[] artists = jsonToken.SelectArtists("musicTwoRowItemRenderer.subtitle.runs", 2, jsonToken.SelectObjectOptional<JToken>($"musicTwoRowItemRenderer.subtitle.runs[{runs.Length - 1}].navigationEndpoint") is null ? 1 : 0);
        int yearIndex = artists[0].Id is null ? 4 : artists.Length * 2 + 2;

        return new(
            name: jsonToken.SelectObject<string>("musicTwoRowItemRenderer.title.runs[0].text"),
            id: jsonToken.SelectObject<string>("musicTwoRowItemRenderer.menu.menuRenderer.items[0].menuNavigationItemRenderer.navigationEndpoint.watchPlaylistEndpoint.playlistId"),
            artists: artists,
            releaseYear: yearIndex == runs.Length - 1 ? jsonToken.SelectObject<int>($"musicTwoRowItemRenderer.subtitle.runs[{yearIndex}].text") : 1970,
            isSingle: jsonToken.SelectObject<string>("musicTwoRowItemRenderer.subtitle.runs[0].text") == "Single",
            isEp: jsonToken.SelectObject<string>("musicTwoRowItemRenderer.subtitle.runs[0].text") == "EP",
            radio: jsonToken.SelectRadio("musicTwoRowItemRenderer.menu.menuRenderer.items[1].menuNavigationItemRenderer.navigationEndpoint.watchPlaylistEndpoint.playlistId", null),
            thumbnails: jsonToken.SelectThumbnails("musicTwoRowItemRenderer.thumbnailRenderer.musicThumbnailRenderer.thumbnail.thumbnails"));
    }

    /// <summary>
    /// Parses artist data from the json token
    /// </summary>
    /// <param name="jsonToken">The json token containing the item data</param>
    /// <returns>The artist</returns>
    /// <exception cref="ArgumentNullException">Occurs when some parsed info is null</exception>
    public static LibraryArtist GetArtist(
        JObject jsonToken)
    {
        return new(
            name: jsonToken.SelectObject<string>("musicResponsiveListItemRenderer.flexColumns[0].musicResponsiveListItemFlexColumnRenderer.text.runs[0].text"),
            id: jsonToken.SelectObject<string>("musicResponsiveListItemRenderer.navigationEndpoint.browseEndpoint.browseId").Substring(4),
            songCount: int.Parse(jsonToken.SelectObject<string>("musicResponsiveListItemRenderer.flexColumns[1].musicResponsiveListItemFlexColumnRenderer.text.runs[0].text").Split(' ')[0], NumberStyles.AllowThousands, CultureInfo.InvariantCulture),
            radio: jsonToken.SelectRadioOptional("musicResponsiveListItemRenderer.menu.menuRenderer.items[1].menuNavigationItemRenderer.navigationEndpoint.watchPlaylistEndpoint.playlistId", null),
            thumbnails: jsonToken.SelectThumbnails("musicResponsiveListItemRenderer.thumbnail.musicThumbnailRenderer.thumbnail.thumbnails"));
    }

    /// <summary>
    /// Parses subscription data from the json token
    /// </summary>
    /// <param name="jsonToken">The json token containing the item data</param>
    /// <returns>The subscription</returns>
    /// <exception cref="ArgumentNullException">Occurs when some parsed info is null</exception>
    public static LibrarySubscription GetSubscription(
        JObject jsonToken)
    {
        return new(
            name: jsonToken.SelectObject<string>("musicResponsiveListItemRenderer.flexColumns[0].musicResponsiveListItemFlexColumnRenderer.text.runs[0].text"),
            id: jsonToken.SelectObject<string>("musicResponsiveListItemRenderer.navigationEndpoint.browseEndpoint.browseId"),
            subscribersInfo: jsonToken.SelectObject<string>("musicResponsiveListItemRenderer.flexColumns[1].musicResponsiveListItemFlexColumnRenderer.text.runs[0].text"),
            radio: jsonToken.SelectRadioOptional("musicResponsiveListItemRenderer.menu.menuRenderer.items[1].menuNavigationItemRenderer.navigationEndpoint.watchPlaylistEndpoint.playlistId", null),
            thumbnails: jsonToken.SelectThumbnails("musicResponsiveListItemRenderer.thumbnail.musicThumbnailRenderer.thumbnail.thumbnails"));
    }

    /// <summary>
    /// Parses podcast data from the json token
    /// </summary>
    /// <param name="jsonToken">The json token containing the item data</param>
    /// <returns>The podcast</returns>
    /// <exception cref="ArgumentNullException">Occurs when some parsed info is null</exception>
    public static LibraryPodcast GetPodcast(
        JObject jsonToken)
    {
        return new(
            name: jsonToken.SelectObject<string>("musicTwoRowItemRenderer.title.runs[0].text"),
            id: jsonToken.SelectObject<string>("musicTwoRowItemRenderer.thumbnailOverlay.musicItemThumbnailOverlayRenderer.content.musicPlayButtonRenderer.playNavigationEndpoint.watchPlaylistEndpoint.playlistId"),
            host: jsonToken.SelectNamedEntity("musicTwoRowItemRenderer.subtitle.runs[0].text", "musicTwoRowItemRenderer.subtitle.runs[0].navigationEndpoint.browseEndpoint.browseId"),
            thumbnails: jsonToken.SelectThumbnails("musicTwoRowItemRenderer.thumbnailRenderer.musicThumbnailRenderer.thumbnail.thumbnails"));
    }
}