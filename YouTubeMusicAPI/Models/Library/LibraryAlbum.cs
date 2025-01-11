﻿namespace YouTubeMusicAPI.Models.Library;

/// <summary>
/// Represents a YouTube Music library album
/// </summary>
/// <param name="name">The name of this album</param>
/// <param name="id">The id of this album</param>
/// <param name="artists">The artists of this album</param>
/// <param name="releaseYear">The release year of this album</param>
/// <param name="isSingle">Weither this album is a single or not</param>
/// <param name="isEp">Weither this album is an EP or not</param>
/// <param name="radio">The radio channel of this album</param>
/// <param name="thumbnails">The thumbnails of this album</param>
public class LibraryAlbum(
    string name,
    string id,
    YouTubeMusicItem[] artists,
    int releaseYear,
    bool isSingle,
    bool isEp,
    Radio radio,
    Thumbnail[] thumbnails)
{
    /// <summary>
    /// The name of this album
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// The id of this album
    /// </summary>
    public string Id { get; } = id;

    /// <summary>
    /// The artists of this album
    /// </summary>
    public YouTubeMusicItem[] Artists { get; } = artists;

    /// <summary>
    /// The release year of this album
    /// </summary>
    public int ReleaseYear { get; } = releaseYear;

    /// <summary>
    /// Weither this album is a single or not
    /// </summary>
    public bool IsSingle { get; } = isSingle;

    /// <summary>
    /// Weither this album is an EP or not
    /// </summary>
    public bool IsEp { get; } = isEp;

    /// <summary>
    /// The radio channel of this album
    /// </summary>
    public Radio Radio { get; } = radio;

    /// <summary>
    /// The thumbnails of this album
    /// </summary>
    public Thumbnail[] Thumbnails { get; } = thumbnails;
}