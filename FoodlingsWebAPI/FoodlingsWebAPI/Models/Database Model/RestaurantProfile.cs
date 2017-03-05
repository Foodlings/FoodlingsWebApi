using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodlingsWebAPI.Models.Database_Model
{
    public class RestaurantProfile
    {
        public int RestaurantID { get; set; }
        public int SubscriberID { get; set; }
        public string Address { get; set; }
        public string Timing { get; set; }
        public string Category { get; set; }
    }
}