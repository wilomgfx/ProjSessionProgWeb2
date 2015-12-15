﻿using System;
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
        [DataType(DataType.Date)]
        public DateTime? DateEtHeureDebut { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateEtHeureFin { get; set; }

        public virtual Evenement Evenement { get; set; }

        public virtual Congres Congres { get; set; }

    }
}