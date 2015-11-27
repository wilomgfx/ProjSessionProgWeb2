using ProjetSessionWebServ2.DAL;
using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class TypeConferenceRepository : GenericRepository<TypeConference>
    {
        public TypeConferenceRepository(ApplicationDbContext context) : base(context) { }
        public TypeConference ObtenirTypeConferenceParID(int? id)
            {
                return GetByID(id);
            }
        public IEnumerable<TypeConference> ObtenirTypeConferences()
            {
                return Get();
            }
        public void InsertTypeConference(TypeConference TypeConference) { Insert(TypeConference); }
        public void DeleteTypeConference(TypeConference TypeConference) { Delete(TypeConference); }
        public void UpdateTypeConference(TypeConference TypeConference) { Update(TypeConference); }
    }
}