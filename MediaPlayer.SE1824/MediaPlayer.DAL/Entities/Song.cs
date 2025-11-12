using System;
using System.Collections.Generic;

namespace MediaPlayer.DAL.Entities;

public partial class Song
{
    public int SongId { get; set; }

    public string Title { get; set; } = null!;

    public int? ArtistId { get; set; }

    public int? AlbumId { get; set; }

    public string FilePath { get; set; } = null!;

    public int? Duration { get; set; }

    public string? Genre { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Artist? Artist { get; set; }

    public virtual ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();
}
