using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class DimensionRepository : GenericRepository<Dimension>
    {
        public DimensionRepository(ApplicationDbContext context) : base(context) { }
        public Dimension ObtenirDimensionParID(int? id)
            {
                return GetByID(id);
            }
        public IEnumerable<Dimension> ObtenirDimensions()
            {
                return Get();
            }

        public void InsertDimension(Dimension Dimension) { Insert(Dimension); }
        public void DeleteDimension(Dimension Dimension) { Delete(Dimension); }
        public void UpdateDimension(Dimension Dimension) { Update(Dimension); }
    }
}