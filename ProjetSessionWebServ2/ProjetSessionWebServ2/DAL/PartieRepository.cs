using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class PartieRepository : GenericRepository<Partie>
    {
        public PartieRepository(ApplicationDbContext context) : base(context) { }
        //public ForfaitRepository(GestionPhotoImmobilierEntities1 context) : base(context) { }

        public Partie ObtenirPartieParID(int? id)
        {
            return GetByID(id);
        }
        public IEnumerable<Partie> ObtenirParties()
        {
            return Get(null, null, "Equipes,Tournoi");
        }

        public void InsertPartie(Partie Partie) { Insert(Partie); }

        //public void DeletePartie(Partie Partie) { Delete(Partie); }

        public void UpdatePartie(Partie Partie) { Update(Partie); }

        public Partie ObtenirPartieCompletParId(int? id)
        {
            return Get(null, null, "Equipes,Tournoi").Where(e => e.Id == id).SingleOrDefault();
        }

        public IEnumerable<Partie> ObtenirPartiesParTournoi(int? id)
        {
            return Get(null, null, "Equipes,Tournoi").Where(p => p.Tournoi.Id == id);
        }
    }
}