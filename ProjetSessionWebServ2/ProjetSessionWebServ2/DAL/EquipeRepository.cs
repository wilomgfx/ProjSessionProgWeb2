using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class EquipeRepository : GenericRepository<Equipe>
    {
        public EquipeRepository(ApplicationDbContext context) : base(context) { }
        //public ForfaitRepository(GestionPhotoImmobilierEntities1 context) : base(context) { }

        public Equipe ObtenirEquipeParID(int? id)
        {
            return GetByID(id);
        }
        public IEnumerable<Equipe> ObtenirEquipes()
        {
            return Get(null, null, "TypeEquipe");
        }

        public IEnumerable<Equipe> ObtenirEquipeParNom(string nom)
        {
            IEnumerable<Equipe> lstFiltered = Get(t => t.Nom.Contains(nom), null, "Joueurs").ToList();
            // TODO: Proper Handling of when there are no results. Show an error message, maybe?
            //return lstFiltered.Count() == 0 ? ObtenirEquipes() : lstFiltered;
            return lstFiltered.Count() == 0 ? lstFiltered : lstFiltered;
        }

        public void InsertEquipe(Equipe Equipe) { Insert(Equipe); }

        //public void DeleteEquipe(Equipe Equipe) { Delete(Equipe); }

        public void UpdateEquipe(Equipe Equipe) { Update(Equipe); }

        public Equipe ObtenirEquipeCompletParId(int? id)
        {
            return Get(null, null, "Joueurs").Where(e => e.Id == id).SingleOrDefault();
        }
    }
}