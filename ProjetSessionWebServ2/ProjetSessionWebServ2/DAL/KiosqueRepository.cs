using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class KiosqueRepository : GenericRepository<Kiosque>
    {
        public KiosqueRepository(ApplicationDbContext context) : base(context) { }

        public Kiosque ObtenirKiosqueParID(int? id)
        {
            return GetByID(id);
        }
        public IEnumerable<Kiosque> ObtenirKiosques()
        {
            return Get(null, null, "TypeKiosque,Users,Congres,PlageHoraires,Salle");
        }

        public IEnumerable<Kiosque> ObtenirKiosqueParType(Evenement.TypeEvent type)
        {
            return Get().Where(e => e.TypeEvenement.Equals(type));
        }

        public IEnumerable<Kiosque> ObtenirKiosqueParTypeKiosque(TypeKiosque type)
        {
            return Get().Where(k => k.TypeKiosque.Nom.Equals(type.Nom));
        }

        public string ObtenirDescriptionKiosque(int? id)
        {
            return GetByID(id).Description;
        }

        public List<PlageHoraire> ObtenirPlageHoraireKiosque(int? id)
        {
            return GetByID(id).PlageHoraires;
        }

        public void InsertKiosque(Kiosque Kiosque) { Insert(Kiosque); }


        public void UpdateKiosque(Kiosque Kiosque) { Update(Kiosque); }
    }
}