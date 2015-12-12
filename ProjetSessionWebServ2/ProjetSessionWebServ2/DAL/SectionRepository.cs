using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class SectionRepository:GenericRepository<Section>
    {
        public SectionRepository(ApplicationDbContext context) : base(context) { }
        public Section ObtenirSectionParID(int? id)
            {
                return GetByID(id);
            }
        public IEnumerable<Section> ObtenirSections()
            {
                return Get(null, null, "Salle, Dimension");
            }

        public void InsertSection(Section Section) { Insert(Section); }
        public void DeleteSection(Section Section) { Delete(Section); }
        public void UpdateSection(Section Section) { Update(Section); }
    }
}