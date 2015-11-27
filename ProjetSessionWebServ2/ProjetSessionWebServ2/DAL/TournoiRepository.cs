using GestionPhotoImmobilier.DAL;
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
            return Get();
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
            IEnumerable<Tournoi> lstFiltered = Get(t => t.Nom.Equals(nom)).ToList();
            return lstFiltered.Count() == 0 ? ObtenirTournois() : lstFiltered;
        }

        public List<PlageHoraire> ObtenirPlageHoraireTournoi(int? id)
        {
            return GetByID(id).PlageHoraires;
        }

        public void InsertTournoi(Tournoi Tournoi) { Insert(Tournoi); }

        //public void DeleteTournoi(Tournoi Tournoi) { Delete(Tournoi); }

        public void UpdateTournoi(Tournoi Tournoi) { Update(Tournoi); }
    }
}