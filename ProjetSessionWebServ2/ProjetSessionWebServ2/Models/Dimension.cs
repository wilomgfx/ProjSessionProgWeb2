using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Dimension
    {
        public int Id { get; set; }
        [Range(1, int.MaxValue)]
        public int Longueur { get; set; }
        public int Largeur { get; set; }
    }
}