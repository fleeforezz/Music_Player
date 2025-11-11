using ManagementMusicPlayer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementMusicPlayer.DAL.Reposistories
{
    public class MusicPlayerRepo
    {
        private ManagementMusicPlayerContext _ctx;

        public List<MusicPlayer> GetAll()
        {
            _ctx = new();
            return _ctx.MusicPlayers.Include("Company").ToList();
        }
    }
}
