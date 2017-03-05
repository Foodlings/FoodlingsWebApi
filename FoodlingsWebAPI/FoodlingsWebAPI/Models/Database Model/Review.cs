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
        public int RestaurantID { get; set; }
        public string ReviewText { get; set; }
        public string TimeStamp { get; set; }
    }
}