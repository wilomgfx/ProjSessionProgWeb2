using GestionPhotoImmobilier.DAL;
using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class ConferenceRepository : GenericRepository<Conference>
    {
        public ConferenceRepository(ApplicationDbContext context) : base(context) { }
        public Conference ObtenirConferenceParID(int? id)
            {
                return GetByID(id);
            }
        public IEnumerable<Conference> ObtenirConference()
            {
                return Get();
            }
        public void InsertForfait(Conference Conference) { Insert(Conference); }
        public void DeleteForfait(Conference Conference) { Delete(Conference); }
        public void UpdateForfait(Conference Conference) { Update(Conference); }
    }
}