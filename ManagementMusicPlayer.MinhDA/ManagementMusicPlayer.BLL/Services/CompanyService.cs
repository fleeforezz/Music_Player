using ManagementMusicPlayer.DAL.Entities;
using ManagementMusicPlayer.DAL.Reposistories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementMusicPlayer.BLL.Services
{
    public class CompanyService
    {
        private CompanyRepo _repo = new();

        public List<Company> GetAllSupplier()
        {
            return _repo.GetAll();
        }
    }
}
