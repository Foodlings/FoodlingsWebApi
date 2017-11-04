using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodlingsWebAPI.Models.Database_Model
{
    public class SearchResult
    {
        public int SubscriberID { set; get; }
        public int RestaurantID { set; get; }
        public string Name { set; get; }
        public string Type { set; get; }
        public string Email { set; get; }
        public string DisplayPicture { set; get; }
        public string FriendCheck { set; get; }
        public int ReviewsCount { set; get; }
        public string Scope { set; get; }
    }
}