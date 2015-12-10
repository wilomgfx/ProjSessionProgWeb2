using ProjetSessionWebServ2.DAL;
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
        public IEnumerable<Conference> ObtenirConferences()
            {
                return Get(null, null, "TypeConference,Users,Congres");
            }
        public void InsertConference(Conference Conference) { Insert(Conference); }
        public void DeleteConference(Conference Conference) { Delete(Conference); }
        public void UpdateConference(Conference Conference) { Update(Conference); }
    }
}