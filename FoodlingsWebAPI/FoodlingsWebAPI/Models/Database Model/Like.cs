using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodlingsWebAPI.Models.Database_Model
{
    public class Like
    {
        public string LikeID { get; set; }
        public string SubscriberID { get; set; }
        public string PostID { get; set; }
        public string TimeStamp { get; set; }
    }
}