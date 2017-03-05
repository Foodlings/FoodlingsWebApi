using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodlingsWebAPI.Models.Database_Model
{
    public class Images
    {
        public int ImageID { get; set; }
        public int ImageAlbumID { get; set; }
        public int PostID { get; set; }
    }
}