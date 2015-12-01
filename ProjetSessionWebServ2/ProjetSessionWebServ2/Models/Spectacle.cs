using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Spectacle : Evenement
    {
        public Nullable<int> TypeSpectacleId { get; set; }
        public TypeSpectacle TypeSpectacle { get; set; }
    }
}