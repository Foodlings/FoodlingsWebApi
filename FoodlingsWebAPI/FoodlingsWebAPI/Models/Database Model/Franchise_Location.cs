using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodlingsWebAPI.Models.Database_Model
{
    public class Franchise_Location
    {
        public int FranchiseID { get; set;}
        public int RestaurantID { get; set; }
        public int LocationID { get; set; }
    }
}