using MediaPlayer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.DAL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private MediaPlayerContext _db;

        /*
        *  Create  
        */
        public void Create(User entity)
        {
            _db = new();
            _db.Users.Add(entity);
            _db.SaveChanges();
        }

        /*
        *  Delete  
        */
        public void Delete(int id)
        {
            _db = new();
            _db.Remove(id);
            _db.SaveChanges();
        }

        /*
        *  Get All  
        */
        public List<User> GetAll()
        {
            _db = new();
            return _db.Users.ToList();
        }

        /*
        *  Get By Id  
        */
        public User? GetById(int id)
        {
            _db = new();
            return _db.Users.FirstOrDefault(u => u.UserId == id);
        }

        /*
        *  Get By Email  
        */
        public User? GetByEmail(string email)
        {
            _db = new();
            return _db.Users.FirstOrDefault(u => u.Email == email);
        }

        /*
        *  Get By Email/Password
        */
        public User? GetByEmailPassword(string email, string password)
        {
            _db = new();
            return _db.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
        }

        /*
        *  Update 
        */
        public void Update(User entity)
        {
            _db = new();
            _db.Users.Update(entity);
            _db.SaveChanges();
        }
    }
}
