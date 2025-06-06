﻿namespace YouTubeMusicAPI.Models.Info;

/// <summary>
/// Contains information about a YouTube Music community playlist
/// </summary>
/// <param name="name">The name of the community playlist</param>
/// <param name="id">The id of the community playlist</param>
/// <param name="description">The description of the community playlist</param>
/// <param name="creator">The creator of the community playlist</param>
/// <param name="viewsInfo">The views info of the community playlist</param>
/// <param name="duration">The total duration of all tracks in the community playlist</param>
/// <param name="songCount">The count of songs in the community playlist</param>
/// <param name="creationYear">The creation year of the community playlist</param>
/// <param name="isInfinite">Weither the community playlist has an infinite amount of songs</param>
/// <param name="thumbnails">The thumbnails of the community playlist</param>
public class CommunityPlaylistInfo(
    string name,
    string id,
    string? description,
    NamedEntity creator,
    string? viewsInfo,
    TimeSpan duration,
    int songCount,
    int creationYear,
    bool isInfinite,
    Thumbnail[] thumbnails) : EntityInfo(name, id, thumbnails)
{
    /// <summary>
    /// The description of the community playlist
    /// </summary>
    public string? Description { get; } = description;

    /// <summary>
    /// The artist of the community playlist
    /// </summary>
    public NamedEntity Creator { get; } = creator;

    /// <summary>
    /// The views info of the community playlist
    /// </summary>
    public string? ViewsInfo { get; } = viewsInfo;

    /// <summary>
    /// The total duration of all tracks in the community playlist
    /// </summary>
    public TimeSpan Duration { get; } = duration;

    /// <summary>
    /// The count of songs in the community playlist
    /// </summary>
    public int SongCount { get; } = songCount;

    /// <summary>
    /// The creation year of the community playlist
    /// </summary>
    public int CreationYear { get; } = creationYear;

    /// <summary>
    /// Weither the community playlist has an infinite amount of songs
    /// </summary>
    public bool IsInfinite { get; } = isInfinite;
}