using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class TypeSpectacleRepository : GenericRepository<TypeSpectacle>
    {
        public TypeSpectacleRepository(ApplicationDbContext context) : base(context) { }
        public TypeSpectacle ObtenirTypeSpectacleParID(int? id) { return GetByID(id); }
        public IEnumerable<TypeSpectacle> ObtenirTypeSpectacles() { return Get(); }
        public IEnumerable<TypeSpectacle> ObtenirTypeSpectacleParNom(string type) { return Get().Where(e => e.Nom.Equals(type)); }
        public void InsertTypeSpectacle(TypeSpectacle TypeSpectacle) { Insert(TypeSpectacle); }
        public void DeleteTypeSpectacle(TypeSpectacle TypeSpectacle) { Delete(TypeSpectacle); }
        public void UpdateTypeSpectacle(TypeSpectacle TypeSpectacle) { Update(TypeSpectacle); }
    }
}