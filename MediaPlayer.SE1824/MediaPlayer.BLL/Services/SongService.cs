using MediaPlayer.DAL.Entities;
using MediaPlayer.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.BLL.Services
{
    public class SongService
    {
        private SongRepository _repo;

        public List<Song> GetAllSongs()
        {
            return _repo.GetAll();
        }
    }
}
