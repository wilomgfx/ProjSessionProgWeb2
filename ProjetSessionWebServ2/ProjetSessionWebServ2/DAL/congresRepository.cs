using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class CongresRepository : GenericRepository<Congres>
    {
        public CongresRepository(ApplicationDbContext context) : base(context) { }
        public Congres ObtenirCongresParID(int? id)
        {
            return GetByID(id);
        }
        public IEnumerable<Congres> ObtenirCongres()
        {
            return Get();
        }

        public IEnumerable<Congres> ObtenirCongresParNom(string nom)
        {
            IEnumerable<Congres> lstFiltered = Get(t => t.Nom.Contains(nom)).ToList();
            return lstFiltered.Count() == 0 ? ObtenirCongres() : lstFiltered;
        }

        public void InsertCongres(Congres Congres) { Insert(Congres); }
        public void DeleteCongres(Congres Congres) { Delete(Congres); }
        public void UpdateCongres(Congres Congres) { Update(Congres); }
    }
}