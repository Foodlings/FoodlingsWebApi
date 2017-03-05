using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodlingsWebAPI.Models.Database_Model
{
    public class Like
    {
        public int LikeID { get; set; }
        public int SubscriberID { get; set; }
        public int PostID { get; set; }
        public string TimeStamp { get; set; }
    }
}