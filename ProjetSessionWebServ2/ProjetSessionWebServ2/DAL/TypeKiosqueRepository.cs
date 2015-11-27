using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class TypeKiosqueRepository : GenericRepository<TypeKiosque>
    {
         public TypeKiosqueRepository(ApplicationDbContext context) : base(context) { }

        public TypeKiosque ObtenirTypeKiosqueParID(int? id)
        {
            return GetByID(id);
        }
        public IEnumerable<TypeKiosque> ObtenirTypeKiosques()
        {
            return Get();
        }

        public IEnumerable<TypeKiosque> ObtenirTypeKiosqueParNom(string type)
        {
            return Get().Where(e => e.Nom.Equals(type));
        }

        public void InsertTypeKiosque(TypeKiosque TypeKiosque) { Insert(TypeKiosque); }

        //public void DeleteTypeKiosque(TypeKiosque TypeKiosque) { Delete(TypeKiosque); }

        public void UpdateTypeKiosque(TypeKiosque TypeKiosque) { Update(TypeKiosque); }
    }
}