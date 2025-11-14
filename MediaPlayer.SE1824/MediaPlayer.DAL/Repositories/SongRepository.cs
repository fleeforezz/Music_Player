using MediaPlayer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.DAL.Repositories
{
    public class SongRepository : IRepository<Song>
    {
        private MediaPlayerContext _db = new();

        /*
        *  Create 
        */
        public void Create(Song song)
        {
            _db.Songs.Add(song);
            _db.SaveChanges();
        }

        /*
        *  Delete  
        */
        public void Delete(int id)
        {
            var song = _db.Songs.Find(id);
            if (song != null)
            {
                _db.Songs.Remove(song);
                _db.SaveChanges();
            }
        }

        /*
        *  Get All 
        */
        public List<Song> GetAll()
        {
            return _db.Songs
                .OrderByDescending(s => s.CreatedAt)
                .ToList();
        }

        /*
        *  Get By Id 
        */
        public Song? GetById(int id)
        {
            return _db.Songs.Find(id);
        }

        /*
        *  Update 
        */
        public void Update(Song song)
        {
            _db.Songs.Update(song);
            _db.SaveChanges();
        }

        /*
        *  Search by title, artist, or album
        */
        public List<Song> Search(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public int GetOrCreateArtist(string name)
        {
            var artist = _db.Artists.FirstOrDefault(a => a.ArtistName == name);
            if (artist != null) return artist.ArtistId;

            var newArtist = new Artist { ArtistName = name };
            _db.Artists.Add(newArtist);
            _db.SaveChanges();
            return newArtist.ArtistId;
        }
    }
}
