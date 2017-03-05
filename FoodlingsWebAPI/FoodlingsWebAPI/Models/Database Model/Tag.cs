using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodlingsWebAPI.Models.Database_Model
{
    public class Tag
    {
        public int TagID { get; set; }
        public int SubscriberID { get; set; }
        public int TaggedSubscriber { get; set; }
        public int PostID { get; set; }
        public string TimeStamp { get; set; }
    }
}