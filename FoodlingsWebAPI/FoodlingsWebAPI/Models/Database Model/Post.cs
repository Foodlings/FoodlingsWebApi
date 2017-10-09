using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodlingsWebAPI.Models.Database_Model
{
    public class Post
    {
        public int PostID { get; set; }
        public int SubscriberID { get; set; }
        public string SubscriberName { get; set; }
        public string DisplayPicture { get; set; }
        public int ImagePresence { get; set; }
        public int ImageAlbumID { get; set; }
        public int ReviewPresence { get; set; }
        public int CheckinPresence { get; set; }
        public string Privacy { get; set; }
        public string TimeStamp { get; set; }
        public string PostDescription { get; set; }
        public string ImageString { get; set; }
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
        public string CurrentUsersLike { get; set; }
    }
}