using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodlingsWebAPI.Models.Database_Model
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int SubscriberID { get; set; }
        public string SubscriberName { get; set; }
        public int PostID { get; set; }
        public string CommentText { get; set; }
        public string TimeStamp { get; set; }
    }
}