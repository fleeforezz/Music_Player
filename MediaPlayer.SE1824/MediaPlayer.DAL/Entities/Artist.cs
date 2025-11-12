using System;
using System.Collections.Generic;

namespace MediaPlayer.DAL.Entities;

public partial class Artist
{
    public int ArtistId { get; set; }

    public string ArtistName { get; set; } = null!;

    public string? Bio { get; set; }

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
