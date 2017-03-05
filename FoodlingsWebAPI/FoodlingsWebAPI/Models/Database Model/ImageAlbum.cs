using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodlingsWebAPI.Models.Database_Model
{
    public class ImageAlbum
    {
        public int ImageAlbumID { get; set; } 
        public string AlbumName { get; set; }
        public int SubscriberID { get; set; }
    }
}