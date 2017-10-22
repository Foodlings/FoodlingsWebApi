using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodlingsWebAPI.Models.Database_Model
{
    public class Friend
    {
        public int ListID { get; set; }
        public int SubscriberID { get; set; }
        public int FriendID { get; set; }
        public string Since { get; set; }
        public string SubscriberName { get; set; }
        public string DisplayPicture { get; set; }
    }
}