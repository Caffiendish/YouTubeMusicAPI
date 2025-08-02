using System.Net;

namespace YouTubeMusicAPI.Tests;

/// <summary>
/// Contains test data for tests
/// </summary>
internal abstract class TestData
{
    /// <summary>
    /// Test geographical location for requests
    /// </summary>
    public const string GeographicalLocation = "US";

    /// <summary>
    /// Test visitor data for requests
    /// </summary>
    public const string? VisitorData = "Cgtrb2Zqbjh1dEYwVSjuvbfEBjIKCgJERRIEEgAgVQ%3D%3D";

    /// <summary>
    /// Test po token for requests
    /// </summary>
    public const string? PoToken = "MnoncdjRufrHvSdPNAu1RIvPu4rxtEle9paKB7-Kt3JgSThJ_OJ8kU0Bsx-bQuV28hDadh0unwuz_VErF0x-32cez6yfwZ2tjuAfybIVoKVJS-dXVpPC6HE3vjUwTTuLRStGxpDJN6-Xc7238VB-h2ykBg-zf2ZbyZkWPg";

    /// <summary>
    /// Test cookies for authentication
    /// </summary>
    public static IEnumerable<Cookie>? Cookies
    {
        get
        {
            string? cookies = "__Secure-3PSID=g.a000zwgU-zG4vBbzHxyL3Kfdd2aH8Es2pe1t_DWMWtDZyG4P3oio7M08Egww7eF8qeumixyTiQACgYKAZwSARISFQHGX2MiHhjei211t3vObbupYHFDXRoVAUF8yKqSvpF7CfyVUJwgNzCzv4oq0076;__Secure-1PAPISID=MtQp2-n4M9vBZiRb/AclH1iQBRTDIXVery;__Secure-3PAPISID=MtQp2-n4M9vBZiRb/AclH1iQBRTDIXVery;SSID=AfAGXFAgGcfbHUZeS;HSID=ARstNkXfQaXxafCPV;SID=g.a000zwgU-zG4vBbzHxyL3Kfdd2aH8Es2pe1t_DWMWtDZyG4P3oioPZNbsColueU49tNlQZ9igQACgYKATESARISFQHGX2MiDx1EOhrJifrzGhPXgf6T5BoVAUF8yKqknYiXsfVnowVBowmReq2M0076;SAPISID=MtQp2-n4M9vBZiRb/AclH1iQBRTDIXVery;__Secure-3PSIDTS=sidts-CjUB5H03P9sT8EB2tRdw0yPzyB_-sR-hdaWURKAkvvYKWm9uI1OdwKrsebW2JB3vG8twFOsdXxAA;APISID=UwsOGllqz4ox2kr0/Ai04jH_YUfXoA8cde;__Secure-1PSID=g.a000zwgU-zG4vBbzHxyL3Kfdd2aH8Es2pe1t_DWMWtDZyG4P3oiok9duYfOarTb734nRqsAWMAACgYKARkSARISFQHGX2MitnTQ03Jx5WjQYeLKm_1Z2BoVAUF8yKrnKvc06amuN6tpdN-t-Ht80076;__Secure-1PSIDTS=sidts-CjUB5H03P9sT8EB2tRdw0yPzyB_-sR-hdaWURKAkvvYKWm9uI1OdwKrsebW2JB3vG8twFOsdXxAA";

            return cookies?
                .Split(';')
                .Select(cookieString =>
                {
                    string[] parts = cookieString.Split("=");
                    return new Cookie(parts[0], parts[1]) { Domain = ".youtube.com" };
                }) ?? null;
        }
    }


    /// <summary>
    /// Test query for search requests
    /// </summary>
    public const string SearchQuery = "Pashanim";


    /// <summary>
    /// Test offset from which items should be retruened for fetch requests
    /// </summary>
    public const int FetchOffset = 0;

    /// <summary>
    /// Test maximum items of items which should be retruened for fetch requests
    /// </summary>
    public const int FetchLimit = 20;


    /// <summary>
    /// Test song or video id
    /// </summary>
    public const string SongVideoId = "3b97cGKN_1A";

    /// <summary>
    /// Test album id
    /// </summary>
    public const string AlbumId = "OLAK5uy_muEnh0WPCqRdkgV3Qg24ttvmZTP1_RBTo";

    /// <summary>
    /// Test playlist id
    /// </summary>
    public const string PlaylistId = "PLuvXOFt0CoEbwWSQj5LmzPhIVKS0SvJ-1";


    /// <summary>
    /// Test album browse id
    /// </summary>
    public const string AlbumBrowseId = "MPREb_H2RWN4XY0Ny";

    /// <summary>
    /// Test playlist browse id
    /// </summary>
    public const string PlaylistBrowseId = "RDAMVMnP4Q-iszqb0";

    /// <summary>
    /// Test artist browse id
    /// </summary>
    public const string ArtistBrowseId = "UC5_H9CxsfukWbjYeYepADsw";


    /// <summary>
    /// Test watch time for update requests
    /// </summary>
    public static TimeSpan WatchTime = TimeSpan.FromMinutes(1.2);


    /// <summary>
    /// File path to download test media stream
    /// </summary>
    public static string FilePath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\test.mp4";
}
