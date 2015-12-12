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
            return Get(null, null, "TypeSpectacle,Salle,Congres,PlageHoraires");
        }

        public IEnumerable<Spectacle> ObtenirSpectacleParTypeSpectacle(TypeSpectacle type)
        {
            return Get().Where(s => s.TypeSpectacle.Nom.Equals(type.Nom));
        }

        public IEnumerable<Spectacle> ObtenirSpectacleParNom(string nom)
        {
            IEnumerable<Spectacle> lstFiltered = Get(t => t.Nom.Contains(nom)).ToList();
            return lstFiltered.Count() == 0 ? ObtenirSpectacles() : lstFiltered;
        }

        public void InsertSpectacle(Spectacle Spectacle) { Insert(Spectacle); }
        public void DeleteSpectacle(Spectacle Spectacle) { Delete(Spectacle); }
        public void UpdateSpectacle(Spectacle Spectacle) { Update(Spectacle); }
    }
}