using GestionPhotoImmobilier.DAL;
using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class EvenementRepository : GenericRepository<Evenement>
    {
        public EvenementRepository(ApplicationDbContext context) : base(context) { }
        public Evenement ObtenirEvenementParID(int? id)
            {
                return GetByID(id);
            }
        public IEnumerable<Evenement> ObtenirEvenements()
        {
            return Get(null,null,"Salle");
        }
        public IEnumerable<Evenement> ObtenirEvenementParType(ProjetSessionWebServ2.Models.Evenement.TypeEvent type)
            {
                return Get().Where(e => e.TypeEvenement.Equals(type));
            }
        public string ObtenirDescriptionEvenement(int? id)
        {
            return GetByID(id).Description;
        }

        public List<PlageHoraire> ObtenirPlageHoraireEvenement(int? id)
        {
            return GetByID(id).PlageHoraires;
        }

        public void InsertEvenement(Evenement Evenement) { Insert(Evenement); }
        public void DeleteEvenement(Evenement Evenement) { Delete(Evenement); }
        public void UpdateEvenement(Evenement Evenement) { Update(Evenement); }
    }
}