using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class TypeTournoiRepository : GenericRepository<TypeTournoi>
    {
        public TypeTournoiRepository(ApplicationDbContext context) : base(context) { }

        public TypeTournoi ObtenirTypeTournoiParID(int? id)
        {
            return GetByID(id);
        }
        public IEnumerable<TypeTournoi> ObtenirTypeTournois()
        {
            return Get();
        }

        public IEnumerable<TypeTournoi> ObtenirTypeTournoiParNom(string type)
        {
            return Get().Where(e => e.Nom.Equals(type));
        }

        public void InsertTypeTournoi(TypeTournoi TypeTournoi) { Insert(TypeTournoi); }

        //public void DeleteTypeKiosque(TypeKiosque TypeKiosque) { Delete(TypeKiosque); }

        public void UpdateTypeTournoi(TypeTournoi TypeTournoi) { Update(TypeTournoi); }
    }
}