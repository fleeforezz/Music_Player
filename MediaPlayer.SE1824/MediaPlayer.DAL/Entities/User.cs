using System;
using System.Collections.Generic;

namespace MediaPlayer.DAL.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int Role { get; set; }

    public DateTime? CreatedAt { get; set; }
}
