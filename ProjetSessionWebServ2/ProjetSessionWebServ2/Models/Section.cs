using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public virtual Dimension Dimension { get; set; }
    }
}