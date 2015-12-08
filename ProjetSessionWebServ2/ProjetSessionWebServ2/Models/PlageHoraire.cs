using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProjetSessionWebServ2.Models
{
    public class PlageHoraire
    {
        public int Id { get; set; }

        //fix pour le fail de conversion de datetime2 vers datetime de entity...
        [DataType(DataType.DateTime)]
        public DateTime? DateEtHeureDebut { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DateEtHeureFin { get; set; }

        public virtual Evenement Evenement { get; set; }

    }
}