using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodlingsWebAPI.Models.Database_Model
{
    public class Location
    {
        public int LocationID { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
    }
}