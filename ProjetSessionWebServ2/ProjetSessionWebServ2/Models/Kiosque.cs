using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Kiosque : Evenement
    {
        public Nullable<int> TypeKiosqueId { get; set; }
        public TypeKiosque TypeKiosque { get; set; }
    }
}