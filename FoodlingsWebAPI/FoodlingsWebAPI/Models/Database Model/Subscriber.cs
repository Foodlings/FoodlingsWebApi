using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodlingsWebAPI.Models.Database_Model
{
    public class Subscriber
    {
        public int SubscriberID { get; set; }
        public string SubscriberName { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public int DisplayPictureID { get; set; }
        public string PhoneNumber { get; set; }
        public String Bio { get; set; }
        public string Gender { get; set; }
        public string DoB { get; set; }
    }
}