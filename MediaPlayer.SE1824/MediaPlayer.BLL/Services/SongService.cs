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
        private SongRepository _repo = new();

        /*
        *  Get All Songs  
        */
        public List<Song> GetAllSongs()
        {
            return _repo.GetAll();
        }

        /*
        *  Create new Song
        */
        public void CreateSong(Song song)
        {
            _repo.Create(song);
        }

        /*
        *  Update song
        */
        public void UpdateSong(Song song)
        {
            _repo.Update(song);
        }

        /*
        *  Delete song
        */
        public void DeleteSong(int id)
        {
            _repo.Delete(id);
        }
    }
}
