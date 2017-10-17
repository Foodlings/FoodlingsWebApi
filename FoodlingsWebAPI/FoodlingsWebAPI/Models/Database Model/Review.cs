using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodlingsWebAPI.Models.Database_Model
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int PostID { get; set; }
        public int SubscriberID { get; set; }
        public int RestaurantID { get; set; }
        public string ReviewText { get; set; }
        public string TimeStamp { get; set; }
        public string Taste { get; set; }
        public string Ambience { get; set; }
        public string Service { get; set; }
        public string OrderTime { get; set; }
        public string Price { get; set; }
        public string SubscriberName { get; set; }
        public string DisplayPicture { get; set; }
    }
}