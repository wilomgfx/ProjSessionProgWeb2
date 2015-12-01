using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class SalleRepository : GenericRepository<Salle>
    {
        public SalleRepository(ApplicationDbContext context) : base(context) { }
        public Salle ObtenirSalleParID(int? id)
            {
                return GetByID(id);
            }
        public IEnumerable<Salle> ObtenirSalles()
            {
                return Get(null,null,"Sections");
            }
        
        public void InsertSalle(Salle Salle) { Insert(Salle); }
        public void DeleteSalle(Salle Salle) { Delete(Salle); }
        public void UpdateSalle(Salle Salle) { Update(Salle); }
    }
}