using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class PlageHoraireRepository : GenericRepository<PlageHoraire>
    {
        public PlageHoraireRepository(ApplicationDbContext context) : base(context) { }
        public PlageHoraire ObtenirPlageHoraireParID(int? id)
        {
            return GetByID(id);
        }
        public IEnumerable<PlageHoraire> ObtenirPlageHoraires()
        {
            return Get();
        }
        public void InsertPlageHoraire(PlageHoraire PlageHoraire) { Insert(PlageHoraire); }
        public void DeletePlageHoraire(PlageHoraire PlageHoraire) { Delete(PlageHoraire); }
        public void UpdatePlageHoraire(PlageHoraire PlageHoraire) { Update(PlageHoraire); }
    }
}