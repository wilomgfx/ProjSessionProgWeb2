using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class EquipeAvancementRepository : GenericRepository<EquipeAvancement>
    {
        public EquipeAvancementRepository(ApplicationDbContext context) : base(context) { }
        //public ForfaitRepository(GestionPhotoImmobilierEntities1 context) : base(context) { }

        public EquipeAvancement ObtenirEquipeAvancementParID(int? id)
        {
            return GetByID(id);
        }
        public IEnumerable<EquipeAvancement> ObtenirEquipeAvancements()
        {
            return Get(null, null, "Equipe, Tournoi");
        }

        public IEnumerable<EquipeAvancement> ObtenirEquipeAvancementsParTournoi(int? id)
        {
            return Get(null, null, "Tournoi, Equipe").Where(e => e.Tournoi.Id == id);
        }

        public IEnumerable<EquipeAvancement> ObtenirEquipeAvancementsParTournoiTriee(int? id)
        {
            return Get(null, null, "Tournoi, Equipe").Where(e => e.Tournoi.Id == id).OrderByDescending(e => e.NbrDePoints);
        }

        public void InsertEquipeAvancement(EquipeAvancement EquipeAvancement) { Insert(EquipeAvancement); }

        //public void DeleteEquipeAvancement(EquipeAvancement EquipeAvancement) { Delete(EquipeAvancement); }

        public void UpdateEquipeAvancement(EquipeAvancement EquipeAvancement) { Update(EquipeAvancement); }

        public EquipeAvancement ObtenirEquipeAvancementCompletParId(int? id)
        {
            return Get(null, null, "Equipe, Tournoi").Where(e => e.EquipeAvancementId == id).SingleOrDefault();
        }

        public EquipeAvancement ObtenirEquipeAvancementParIdEquipeParTournoi(int? id, int? tournid)
        {
            return Get(null, null, "Equipe, Tournoi").Where(e => e.Equipe.Id == id && e.Tournoi.Id == tournid).SingleOrDefault();
        }
    }
}