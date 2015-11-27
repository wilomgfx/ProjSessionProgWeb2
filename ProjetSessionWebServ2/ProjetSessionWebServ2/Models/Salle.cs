﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Salle
    {
        public int Id { get; set; }

        public string NoSalle { get; set; }

        public virtual List<Section> Sections { get; set; }

        public virtual Dimension Dimension { get; set; }
    }
}