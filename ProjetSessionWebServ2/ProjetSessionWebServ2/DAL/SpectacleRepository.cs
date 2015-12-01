using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class SpectacleRepository : GenericRepository<Spectacle>
    {
        public SpectacleRepository(ApplicationDbContext context) : base(context) { }
        public Spectacle ObtenirSpectacleParID(int? id)
            {
                return GetByID(id);
            }
        public IEnumerable<Spectacle> ObtenirSpectacles()
            {
                return Get(null, null, "TypeSpectacle");
            }
        public void InsertSpectacle(Spectacle Spectacle) { Insert(Spectacle); }
        public void DeleteSpectacle(Spectacle Spectacle) { Delete(Spectacle); }
        public void UpdateSpectacle(Spectacle Spectacle) { Update(Spectacle); }
    }
}