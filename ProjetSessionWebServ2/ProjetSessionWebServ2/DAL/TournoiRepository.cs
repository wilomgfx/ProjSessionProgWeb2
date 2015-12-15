using ProjetSessionWebServ2.DAL;
using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class TournoiRepository : GenericRepository<Tournoi>
    {
        public TournoiRepository(ApplicationDbContext context) : base(context) { }
        //public ForfaitRepository(GestionPhotoImmobilierEntities1 context) : base(context) { }

        public Tournoi ObtenirTournoiParID(int? id)
        {
            return GetByID(id);
        }
        public IEnumerable<Tournoi> ObtenirTournois()
        {
            return Get(null, null, "TypeTournoi,Congres,PlageHoraires");
        }

        public IEnumerable<Tournoi> ObtenirTournoiParType(Evenement.TypeEvent type)
        {
            return Get().Where(e => e.TypeEvenement.Equals(type));
        }

        public string ObtenirDescriptionTournoi(int? id)
        {
            return GetByID(id).Description;
        }

        public IEnumerable<Tournoi> ObtenirTournoiParNom(string nom)
        {
            IEnumerable<Tournoi> lstFiltered = Get(t => t.Nom.ToLower().Contains(nom.ToLower()), null, "TypeTournoi").ToList();
            // TODO: Proper Handling of when there are no results. Show an error message, maybe?
            //return lstFiltered.Count() == 0 ? ObtenirTournois() : lstFiltered;
            return lstFiltered.Count() == 0 ? lstFiltered : lstFiltered;
        }

        public List<PlageHoraire> ObtenirPlageHoraireTournoi(int? id)
        {
            return GetByID(id).PlageHoraires;
        }

        public void InsertTournoi(Tournoi Tournoi) { Insert(Tournoi); }

        //public void DeleteTournoi(Tournoi Tournoi) { Delete(Tournoi); }

        public void UpdateTournoi(Tournoi Tournoi) { Update(Tournoi); }

        public Tournoi ObtenirTournoiCompletParId(int? id)
        {
            return Get(null, null, "Equipes,Parties,Avancements,TypeTournoi").Where(e => e.Id == id).SingleOrDefault();
        }
    }
}