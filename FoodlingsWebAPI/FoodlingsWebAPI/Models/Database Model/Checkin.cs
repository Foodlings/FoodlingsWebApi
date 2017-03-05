using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodlingsWebAPI.Models.Database_Model
{
    public class Checkin
    {
        public int CheckinID { get; set; }
        public int PostID { get; set; }
        public int LocationID { get; set; }
        public int FranchiseID { get; set; }
        public string TimeStamp { get; set; }
    }
}