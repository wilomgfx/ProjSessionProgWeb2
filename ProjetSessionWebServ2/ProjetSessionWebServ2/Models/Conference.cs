using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Conference : Evenement
    {
        public Nullable<int> TypeConferenceId { get; set; }
        public TypeConference TypeConference { get; set; }
    }
}