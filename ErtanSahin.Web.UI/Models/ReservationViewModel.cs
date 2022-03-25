using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErtanSahin.Web.UI.Models
{
    public class ReservationViewModel
    {
        public List<RoomModel> Rooms { get; set; } = new List<RoomModel>();
        public CreateForm CreateForm { get; set; } = new CreateForm();
    }

    public class CreateForm
    {
        public string RoomId { get; set; }
        public string OwnerName { get; set; } = "Ertan";//Get from Identity
        public string OwnerEmail { get; set; } = "Ertan@hotmail.com";//Get from cache my in redis 
        public DateTime ReservationStart { get; set; }
        public DateTime ReservationEnd { get; set; }
        public string Mails { get; set; }
        public List<Member> Members { get; set; } = new List<Member>();

        public class Member
        {
            public string Email { get; set; }
        }
    }
}
