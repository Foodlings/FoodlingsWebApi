using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FoodlingsWebAPI.Models.Database_Model;
using MySql.Data.MySqlClient;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.IO;

namespace FoodlingsWebAPI.Controllers
{
    public class FoodlingDatabaseController : ApiController
    {

        string ConnectionString = "Database=heroku_f5604cdba7afdb8;Data Source=us-cdbr-iron-east-03.cleardb.net;User Id=b6a09ce98741f6;Password=b94e7509";

        #region Cloudinary Credentials
        static Account account = new Account("foodlings", "519417396994997", "F5KrqIroyIz1OxJgtzvc7Ee8Tcg");
        Cloudinary cloudinary = new Cloudinary(account);
        #endregion

        // ======================================= Tables CRUD Operations ======================================

        // Subscriber Table - CRUD Operations
        [HttpPost]
        public IHttpActionResult createSubscriber([FromBody] Subscriber subscriber)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "INSERT INTO subscriber(SubscriberName,Password,Type,EmailAddress,PhoneNumber,Bio,Gender,DoB) VALUES('" + subscriber.SubscriberName + "'" + ",'" + subscriber.Password + "'" + ",'" + subscriber.Type + "'" + ",'" + subscriber.Email + "','" + subscriber.PhoneNumber + "','" + subscriber.Bio + "'" + ",'" + subscriber.Gender + "'" + ",'" + subscriber.DoB + "')";

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                Subscriber addedSubscriber = new Subscriber();
                addedSubscriber.SubscriberName = subscriber.SubscriberName;
                addedSubscriber.Password = subscriber.Password;
                addedSubscriber.Type = subscriber.Type;
                addedSubscriber.Email = subscriber.Email;
                addedSubscriber.DisplayPictureID = 0;
                addedSubscriber.PhoneNumber = subscriber.PhoneNumber;
                addedSubscriber.Bio = subscriber.Bio;
                addedSubscriber.Gender = subscriber.Gender;
                addedSubscriber.DoB = subscriber.DoB;
                addedSubscriber.DisplayPicture = "";
                addedSubscriber.Timing = "";
                addedSubscriber.Category = "";
                addedSubscriber.Address = "";

                List<Subscriber> list = new List<Subscriber>();
                list.Add(addedSubscriber);

                return Ok(new { Subscriber = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }

        [HttpPost]
        public IHttpActionResult createRestaurant([FromBody] Subscriber subscriber)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);
                Connection.Open();
                string Query = "INSERT INTO subscriber(SubscriberName,Password,Type,EmailAddress,PhoneNumber,Bio) VALUES('" + subscriber.SubscriberName + "'" + ",'" + subscriber.Password + "'" + ",'" + subscriber.Type + "'" + ",'" + subscriber.Email + "','" + subscriber.PhoneNumber + "','" + subscriber.Bio + "')";
                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);
                insertCommand.ExecuteNonQuery();

                Subscriber addedSubscriber = new Subscriber();
                addedSubscriber.SubscriberName = subscriber.SubscriberName;
                addedSubscriber.Password = subscriber.Password;
                addedSubscriber.Type = subscriber.Type;
                addedSubscriber.Email = subscriber.Email;
                addedSubscriber.DisplayPictureID = 0;
                addedSubscriber.PhoneNumber = subscriber.PhoneNumber;
                addedSubscriber.Bio = subscriber.Bio;
                addedSubscriber.Gender = "";
                addedSubscriber.DoB = "";
                addedSubscriber.DisplayPicture = "";
                addedSubscriber.CoverPhoto = "";

                Query = "SELECT * FROM subscriber WHERE EmailAddress = '" + subscriber.Email + "'";
                MySqlCommand getCommand = new MySqlCommand(Query, Connection);
                Subscriber retrievedSubscriber = new Subscriber();
                using (MySqlDataReader Reader = getCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        int counter = 1;
                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (counter == 1)
                            { retrievedSubscriber.SubscriberID = (int)Reader.GetValue(i); }

                            counter++;
                        }
                    }
                }

                Query = "INSERT INTO restaurantprofile(SubscriberID, Address, Timing, Category) VALUES(" + retrievedSubscriber.SubscriberID + ",'" + subscriber.Address + "','" + subscriber.Timing + "','" + subscriber.Category + "')";
                insertCommand = new MySqlCommand(Query, Connection);
                insertCommand.ExecuteNonQuery();

                Connection.Close();

                List<Subscriber> list = new List<Subscriber>();
                list.Add(addedSubscriber);
                return Ok(new { Subscriber = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }

        [HttpGet]
        public IHttpActionResult validateEmail(string SubscriberEmail)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);
                Connection.Open();
                string Query = Query = "SELECT * FROM subscriber WHERE EmailAddress = '" + SubscriberEmail + "'";
                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                Subscriber retrievedSubscriber = new Subscriber();
                retrievedSubscriber.SubscriberID = 0;
                retrievedSubscriber.SubscriberName = "";
                retrievedSubscriber.Password = "";
                retrievedSubscriber.PhoneNumber = "";
                retrievedSubscriber.Type = "";
                retrievedSubscriber.Gender = "";
                retrievedSubscriber.DoB = "";
                retrievedSubscriber.DisplayPictureID = 0;
                retrievedSubscriber.DisplayPicture = "";
                retrievedSubscriber.CoverPhoto = "";
                retrievedSubscriber.Bio = "";
                retrievedSubscriber.Timing = "";
                retrievedSubscriber.Category = "";
                retrievedSubscriber.Address = "";
                retrievedSubscriber.Email = "Available";

                using (MySqlDataReader Reader = getCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        int counter = 1;

                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (counter == 5)
                            { retrievedSubscriber.Email = (string)Reader.GetValue(i); }

                            counter++;
                        }
                    }
                }

                List<Subscriber> list = new List<Subscriber>();
                list.Add(retrievedSubscriber);

                Connection.Close();

                return Ok(new { Subscriber = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }


            return null;
        }


        [HttpGet]
        public IHttpActionResult getSubscriber(string SubscriberName, string SubscriberEmail)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = null;

                if (!SubscriberName.Equals("EmailNone"))
                {
                    Query = "SELECT * FROM subscriber WHERE SubscriberName = '" + SubscriberName + "'";
                }
                else if (!SubscriberEmail.Equals("Name"))
                {
                    Query = "SELECT * FROM subscriber WHERE EmailAddress = '" + SubscriberEmail + "'";
                }

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);


                Subscriber retrievedSubscriber = new Subscriber();

                using (MySqlDataReader Reader = getCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        int counter = 1;

                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (counter == 1)
                            { retrievedSubscriber.SubscriberID = (int)Reader.GetValue(i); }
                            else if (counter == 2)
                            { retrievedSubscriber.SubscriberName = (string)Reader.GetValue(i); }
                            else if (counter == 3)
                            { retrievedSubscriber.Password = (string)Reader.GetValue(i); }
                            else if (counter == 4)
                            { retrievedSubscriber.Type = (string)Reader.GetValue(i); }
                            else if (counter == 5)
                            { retrievedSubscriber.Email = (string)Reader.GetValue(i); }
                            else if (counter == 6)
                            {
                                try
                                {
                                    retrievedSubscriber.DisplayPictureID = (int)Reader.GetValue(i);
                                }
                                catch (Exception e)
                                {
                                    retrievedSubscriber.DisplayPictureID = 0;
                                }
                            }
                            else if (counter == 7)
                            { retrievedSubscriber.PhoneNumber = (String)Reader.GetValue(i); }
                            else if (counter == 8)
                            { retrievedSubscriber.Bio = (string)Reader.GetValue(i); }
                            else if (counter == 9)
                            {
                                try
                                {
                                    retrievedSubscriber.Gender = (string)Reader.GetValue(i);
                                }
                                catch (Exception e)
                                {
                                    retrievedSubscriber.Gender = "";
                                }
                            }
                            else if (counter == 10)
                            {
                                try
                                {
                                    retrievedSubscriber.DoB = (string)Reader.GetValue(i);
                                }
                                catch (Exception e)
                                {
                                    retrievedSubscriber.DoB = "";
                                }
                            }
                            else if (counter == 11)
                            {
                                try
                                {
                                    retrievedSubscriber.DisplayPicture = (string)Reader.GetValue(i);
                                }
                                catch (Exception e)
                                {
                                    retrievedSubscriber.DisplayPicture = "";
                                }
                            }
                            else if (counter == 12)
                            {
                                try
                                {
                                    retrievedSubscriber.CoverPhoto = (string)Reader.GetValue(i);
                                }
                                catch (Exception e)
                                {
                                    retrievedSubscriber.CoverPhoto = "";
                                }
                            }

                            counter++;
                        }
                    }
                }

                retrievedSubscriber.Timing = "";
                retrievedSubscriber.Category = "";
                retrievedSubscriber.Address = "";

                if (retrievedSubscriber.Type.Equals("Restaurant"))
                {
                    MySqlConnection restaurantConnection = new MySqlConnection(ConnectionString);
                    restaurantConnection.Open();
                    string resQuery = "SELECT * FROM restaurantprofile WHERE SubscriberID = " + retrievedSubscriber.SubscriberID;
                    MySqlCommand resCommand = new MySqlCommand(resQuery, restaurantConnection);

                    using (MySqlDataReader resReader = resCommand.ExecuteReader())
                    {
                        if (resReader.HasRows && resReader.Read())
                        {
                            int counter = 1;

                            for (int i = 0; i < resReader.FieldCount; i++)
                            {
                                if (counter == 1)
                                { retrievedSubscriber.RestaurantID = (int)resReader.GetValue(i); }
                                else if (counter == 3)
                                { retrievedSubscriber.Address = (string)resReader.GetValue(i); }
                                else if (counter == 4)
                                { retrievedSubscriber.Timing = (string)resReader.GetValue(i); }
                                else if (counter == 5)
                                { retrievedSubscriber.Category = (string)resReader.GetValue(i); }

                                counter++;
                            }
                        }
                    }

                    restaurantConnection.Close();
                }

                List<Subscriber> list = new List<Subscriber>();
                list.Add(retrievedSubscriber);

                Connection.Close();

                return Ok(new { Subscriber = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }

        [HttpGet]
        public IHttpActionResult getSubscriber(string SubscriberID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = Query = "SELECT * FROM subscriber WHERE SubscriberID = " + SubscriberID;

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                Subscriber retrievedSubscriber = new Subscriber();

                using (MySqlDataReader Reader = getCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        int counter = 1;

                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (counter == 1)
                            { retrievedSubscriber.SubscriberID = (int)Reader.GetValue(i); }
                            else if (counter == 2)
                            { retrievedSubscriber.SubscriberName = (string)Reader.GetValue(i); }
                            else if (counter == 3)
                            { retrievedSubscriber.Password = (string)Reader.GetValue(i); }
                            else if (counter == 4)
                            { retrievedSubscriber.Type = (string)Reader.GetValue(i); }
                            else if (counter == 5)
                            { retrievedSubscriber.Email = (string)Reader.GetValue(i); }
                            else if (counter == 6)
                            {
                                try
                                {
                                    retrievedSubscriber.DisplayPictureID = (int)Reader.GetValue(i);
                                }
                                catch (Exception e)
                                {
                                    retrievedSubscriber.DisplayPictureID = 0;
                                }
                            }
                            else if (counter == 7)
                            { retrievedSubscriber.PhoneNumber = (String)Reader.GetValue(i); }
                            else if (counter == 8)
                            { retrievedSubscriber.Bio = (string)Reader.GetValue(i); }
                            else if (counter == 9)
                            {
                                try
                                {
                                    retrievedSubscriber.Gender = (string)Reader.GetValue(i);
                                }
                                catch (Exception e)
                                {
                                    retrievedSubscriber.Gender = "";
                                }
                            }
                            else if (counter == 10)
                            {
                                try
                                {
                                    retrievedSubscriber.DoB = (string)Reader.GetValue(i);
                                }
                                catch (Exception e)
                                {
                                    retrievedSubscriber.DoB = "";
                                }
                            }
                            else if (counter == 11)
                            {
                                try
                                {
                                    retrievedSubscriber.DisplayPicture = (string)Reader.GetValue(i);
                                }
                                catch (Exception e)
                                {
                                    retrievedSubscriber.DisplayPicture = "none";
                                }
                            }
                            else if (counter == 12)
                            {
                                try
                                {
                                    retrievedSubscriber.CoverPhoto = (string)Reader.GetValue(i);
                                }
                                catch (Exception e)
                                {
                                    retrievedSubscriber.CoverPhoto = "none";
                                }
                            }

                            counter++;
                        }
                    }
                }

                retrievedSubscriber.Timing = "";
                retrievedSubscriber.Category = "";
                retrievedSubscriber.Address = "";

                if (retrievedSubscriber.Type.Equals("Restaurant"))
                {
                    MySqlConnection restaurantConnection = new MySqlConnection(ConnectionString);
                    restaurantConnection.Open();
                    string resQuery = "SELECT * FROM restaurantprofile WHERE SubscriberID = " + retrievedSubscriber.SubscriberID;
                    MySqlCommand resCommand = new MySqlCommand(resQuery, restaurantConnection);

                    using (MySqlDataReader resReader = resCommand.ExecuteReader())
                    {
                        if (resReader.HasRows && resReader.Read())
                        {
                            int counter = 1;

                            for (int i = 0; i < resReader.FieldCount; i++)
                            {
                                if (counter == 1)
                                { retrievedSubscriber.RestaurantID = (int)resReader.GetValue(i); }
                                if (counter == 3)
                                { retrievedSubscriber.Address = (string)resReader.GetValue(i); }
                                else if (counter == 4)
                                { retrievedSubscriber.Timing = (string)resReader.GetValue(i); }
                                else if (counter == 5)
                                { retrievedSubscriber.Category = (string)resReader.GetValue(i); }

                                counter++;
                            }
                        }
                    }

                    restaurantConnection.Close();
                }


                List<Subscriber> list = new List<Subscriber>();
                list.Add(retrievedSubscriber);

                Connection.Close();

                return Ok(new { Subscriber = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }


            return null;
        }

        [HttpPost]
        public IHttpActionResult createDisplayPicture([FromBody] Subscriber subscriber)
        {
            try
            {
                byte[] bytes;

                if (!subscriber.DisplayPicture.Equals("CoverPhoto"))
                {
                    bytes = Convert.FromBase64String(subscriber.DisplayPicture);
                }
                else
                {
                    bytes = Convert.FromBase64String(subscriber.CoverPhoto);
                }

                Stream stream = new MemoryStream(bytes);
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription("image", stream)
                };

                var uploadResult = cloudinary.Upload(uploadParams);

                string url = "empty";
                try
                {
                    url = (string)uploadResult.JsonObj.SelectToken("url");
                }
                catch (Exception ex)
                { url = "error"; }

                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query;

                if (!subscriber.DisplayPicture.Equals("CoverPhoto"))
                {
                    Query = "update subscriber set DisplayPicture='" + url + "' where SubscriberID=" + subscriber.SubscriberID;
                }
                else
                {
                    Query = "update subscriber set CoverPhoto='" + url + "' where SubscriberID=" + subscriber.SubscriberID;
                }

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                Subscriber addedSubscriber = subscriber;

                List<Subscriber> list = new List<Subscriber>();
                list.Add(addedSubscriber);

                return Ok(new { Subscriber = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }

        [HttpGet]
        public IHttpActionResult getAllSubscribers()
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM subscriber";

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                List<Subscriber> list = new List<Subscriber>();

                Subscriber retrievedSubscriber = new Subscriber();

                MySqlDataReader SelectReader = getCommand.ExecuteReader();

                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        retrievedSubscriber.SubscriberID = (int)SelectReader.GetValue(0);
                        retrievedSubscriber.SubscriberName = (string)SelectReader.GetValue(1);
                        retrievedSubscriber.Password = (string)SelectReader.GetValue(2);
                        retrievedSubscriber.Type = (string)SelectReader.GetValue(3);
                        retrievedSubscriber.Email = (string)SelectReader.GetValue(4);
                        retrievedSubscriber.DisplayPictureID = (int)SelectReader.GetValue(5);
                        retrievedSubscriber.PhoneNumber = (string)SelectReader.GetValue(6);
                        retrievedSubscriber.Bio = (string)SelectReader.GetValue(7);
                        retrievedSubscriber.Gender = (string)SelectReader.GetValue(8);
                        retrievedSubscriber.DoB = (string)SelectReader.GetValue(9);

                        list.Add(retrievedSubscriber);

                        retrievedSubscriber = new Subscriber();
                    }
                }

                Connection.Close();

                return Ok(new { Subscriber = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }

        [HttpPost]
        public IHttpActionResult searchSubscribers([FromBody] SearchResult srchResult)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);
                Connection.Open();
                string Query = "SELECT * FROM subscriber where LOWER(SubscriberName) = '" + srchResult.Name.ToLower() + "'";
                MySqlCommand getCommand = new MySqlCommand(Query, Connection);
                MySqlDataReader SelectReader = getCommand.ExecuteReader();

                SearchResult searchResult = new SearchResult();
                List<SearchResult> list = new List<SearchResult>();

                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        searchResult.SubscriberID = (int)SelectReader.GetValue(0);
                        searchResult.Name = (string)SelectReader.GetValue(1);
                        searchResult.Type = (string)SelectReader.GetValue(3);
                        searchResult.Email = (string)SelectReader.GetValue(4);
                        try
                        {
                            searchResult.DisplayPicture = (string)SelectReader.GetValue(10);
                        }
                        catch (Exception e)
                        {
                            searchResult.DisplayPicture = "none";
                        }

                        searchResult.RestaurantID = 0;

                        if (searchResult.Type.Equals("Restaurant"))
                        {
                            MySqlConnection restaurantConnection = new MySqlConnection(ConnectionString);
                            restaurantConnection.Open();
                            string resQuery = "SELECT * FROM restaurantprofile WHERE SubscriberID = " + searchResult.SubscriberID;
                            MySqlCommand resCommand = new MySqlCommand(resQuery, restaurantConnection);

                            using (MySqlDataReader resReader = resCommand.ExecuteReader())
                            {
                                if (resReader.HasRows && resReader.Read())
                                {
                                    int counter = 1;

                                    for (int i = 0; i < resReader.FieldCount; i++)
                                    {
                                        if (counter == 1)
                                        { searchResult.RestaurantID = (int)resReader.GetValue(i); break; }

                                        counter++;
                                    }
                                }
                            }
                        }

                        list.Add(searchResult);
                        searchResult = new SearchResult();
                    }
                }

                    Connection.Close();

                    return Ok(new { SearchResult = list.AsEnumerable().ToList() });
                }
            catch (Exception ex)
            { }


            return null;
        }


        [HttpPost]
        public IHttpActionResult updateSubscriber(int ID, string SubscriberName, string Password, string Type, string Email, int DisplayPictureID, string PhoneNumber, string Bio, string Gender, string DoB)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "UPDATE subscriber SET SubscriberName='" + SubscriberName + "', Password='" + Password + "', Type='" + Type + "', EmailAddress='" + Email + "', DisplayPictureID=" + DisplayPictureID + ", PhoneNumber='" + PhoneNumber + "', Bio='" + Bio + "', Gender='" + Gender + "', DoB='" + DoB + "' WHERE SubscriberID=" + ID;

                MySqlCommand updateCommand = new MySqlCommand(Query, Connection);

                updateCommand.ExecuteNonQuery();

                Connection.Close();

                Subscriber updatedSubscriber = new Subscriber();
                updatedSubscriber.SubscriberID = ID;
                updatedSubscriber.SubscriberName = SubscriberName;
                updatedSubscriber.Password = Password;
                updatedSubscriber.Type = Type;
                updatedSubscriber.Email = Email;
                updatedSubscriber.DisplayPictureID = DisplayPictureID;
                updatedSubscriber.PhoneNumber = PhoneNumber;
                updatedSubscriber.Bio = Bio;
                updatedSubscriber.Gender = Gender;
                updatedSubscriber.DoB = DoB;

                List<Subscriber> list = new List<Subscriber>();
                list.Add(updatedSubscriber);

                return Ok(new { Subscriber = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult deleteSubscriber(string SubscriberName)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "DELETE FROM subscriber WHERE SubscriberName='" + SubscriberName + "'";

                MySqlCommand deleteCommand = new MySqlCommand(Query, Connection);


                deleteCommand.ExecuteNonQuery();

                Connection.Close();

                Subscriber deletedSubscriber = new Subscriber();
                deletedSubscriber.SubscriberName = SubscriberName;

                List<Subscriber> list = new List<Subscriber>();
                list.Add(deletedSubscriber);

                return Ok(new { Subscriber = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public bool deleteAllSubscriber()
        {
            bool status = false;

            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "DELETE FROM subscriber";

                MySqlCommand deleteCommand = new MySqlCommand(Query, Connection);


                deleteCommand.ExecuteNonQuery();

                Connection.Close();

                status = true;

                return status;
            }
            catch (Exception ex)
            { }

            return status;
        }



        // Friend Table - CRUD Operations
        [HttpPost]
        public IHttpActionResult createFriend(int ID, int SubscriberID, int FriendID, string Since)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "INSERT INTO Friend VALUES(" + ID + "," + SubscriberID + "," + FriendID + ",'" + Since + "')";

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                Friend newFriend = new Friend();
                newFriend.ListID = ID;
                newFriend.SubscriberID = SubscriberID;
                newFriend.FriendID = FriendID;
                newFriend.Since = Since;

                List<Friend> list = new List<Friend>();
                list.Add(newFriend);

                return Ok(new { Friend = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getFriend(int ID, string FriendName)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM friend WHERE SubscriberID = " + ID + " AND FriendID = (SELECT subscriber.SubscriberID FROM subscriber WHERE SubscriberName = '" + FriendName + "')";


                MySqlCommand getCommand = new MySqlCommand(Query, Connection);


                Friend retrievedFriend = new Friend();

                using (MySqlDataReader Reader = getCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        int counter = 1;

                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (counter == 1)
                            { retrievedFriend.ListID = (int)Reader.GetValue(i); }
                            else if (counter == 2)
                            { retrievedFriend.SubscriberID = (int)Reader.GetValue(i); }
                            else if (counter == 3)
                            { retrievedFriend.FriendID = (int)Reader.GetValue(i); }
                            else if (counter == 4)
                            { retrievedFriend.Since = (string)Reader.GetValue(i); }

                            counter++;
                        }
                    }
                }


                List<Friend> list = new List<Friend>();
                list.Add(retrievedFriend);

                Connection.Close();

                return Ok(new { Friend = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getAllFriends()
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM friend";

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                List<Friend> list = new List<Friend>();

                Friend retrievedFriend = new Friend();

                MySqlDataReader SelectReader = getCommand.ExecuteReader();

                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        retrievedFriend.ListID = (int)SelectReader.GetValue(0);
                        retrievedFriend.SubscriberID = (int)SelectReader.GetValue(1);
                        retrievedFriend.FriendID = (int)SelectReader.GetValue(2);
                        retrievedFriend.Since = (string)SelectReader.GetValue(3);

                        list.Add(retrievedFriend);

                        retrievedFriend = new Friend();
                    }
                }

                Connection.Close();

                return Ok(new { Friend = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult deleteFriend(int ID, string FriendName)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT SubscriberID FROM subscriber WHERE SubscriberName = '" + FriendName + "'";

                MySqlCommand deleteCommand = new MySqlCommand(Query, Connection);

                int FriendID = -1;

                using (MySqlDataReader Reader = deleteCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        int counter = 1;

                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (counter == 1)
                            { FriendID = (int)Reader.GetValue(i); }

                            counter++;
                        }
                    }
                }

                Connection.Close();


                MySqlConnection Connection2 = new MySqlConnection(ConnectionString);
                Connection2.Open();
                string Query2 = "DELETE FROM friend WHERE SubscriberID = " + ID + " AND FriendID = " + FriendID;
                MySqlCommand deleteCommand2 = new MySqlCommand(Query2, Connection2);
                deleteCommand2.ExecuteNonQuery();
                Connection2.Close();

                Subscriber deletedFriend = new Subscriber();
                deletedFriend.SubscriberName = FriendName;

                List<Subscriber> list = new List<Subscriber>();
                list.Add(deletedFriend);

                return Ok(new { Subscriber = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }



        // ImageAlbum Table - CRUD Operations
        [HttpPost]
        public IHttpActionResult createImageAlbum(int ID, string AlbumName, int SubscriberID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "INSERT INTO imagealbum VALUES(" + ID + ",'" + AlbumName + "'," + SubscriberID + ")";

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                ImageAlbum addedImageAlbum = new ImageAlbum();
                addedImageAlbum.ImageAlbumID = ID;
                addedImageAlbum.AlbumName = AlbumName;
                addedImageAlbum.SubscriberID = SubscriberID;

                List<ImageAlbum> list = new List<ImageAlbum>();
                list.Add(addedImageAlbum);

                return Ok(new { ImageAlbum = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getImageAlbum(string AlbumName)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM imagealbum WHERE AlbumName = '" + AlbumName + "'";

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                ImageAlbum retrievedImageAlbum = new ImageAlbum();

                using (MySqlDataReader Reader = getCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        int counter = 1;

                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (counter == 1)
                            { retrievedImageAlbum.ImageAlbumID = (int)Reader.GetValue(i); }
                            else if (counter == 2)
                            { retrievedImageAlbum.AlbumName = (string)Reader.GetValue(i); }
                            else if (counter == 3)
                            { retrievedImageAlbum.SubscriberID = (int)Reader.GetValue(i); }

                            counter++;
                        }
                    }
                }


                List<ImageAlbum> list = new List<ImageAlbum>();
                list.Add(retrievedImageAlbum);

                Connection.Close();

                return Ok(new { ImageAlbum = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getAllImageAlbums()
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM imagealbum";

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                List<ImageAlbum> list = new List<ImageAlbum>();

                ImageAlbum retrievedImageAlbum = new ImageAlbum();

                MySqlDataReader SelectReader = getCommand.ExecuteReader();

                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        retrievedImageAlbum.ImageAlbumID = (int)SelectReader.GetValue(0);
                        retrievedImageAlbum.AlbumName = (string)SelectReader.GetValue(1);
                        retrievedImageAlbum.SubscriberID = (int)SelectReader.GetValue(2);

                        list.Add(retrievedImageAlbum);

                        retrievedImageAlbum = new ImageAlbum();
                    }
                }

                Connection.Close();

                return Ok(new { ImageAlbum = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult updateImageAlbum(int ID, string AlbumName)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "UPDATE imagealbum SET AlbumName='" + AlbumName + "' WHERE ImageAlbumID=" + ID;

                MySqlCommand updateCommand = new MySqlCommand(Query, Connection);

                updateCommand.ExecuteNonQuery();

                Connection.Close();

                ImageAlbum updatedImageAlbum = new ImageAlbum();
                updatedImageAlbum.ImageAlbumID = ID;
                updatedImageAlbum.AlbumName = AlbumName;

                List<ImageAlbum> list = new List<ImageAlbum>();
                list.Add(updatedImageAlbum);

                return Ok(new { ImageAlbum = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult deleteImageAlbum(string AlbumName)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "DELETE FROM imagealbum WHERE AlbumName='" + AlbumName + "'";

                MySqlCommand deleteCommand = new MySqlCommand(Query, Connection);

                deleteCommand.ExecuteNonQuery();

                Connection.Close();

                ImageAlbum deletedImageAlbum = new ImageAlbum();
                deletedImageAlbum.AlbumName = AlbumName;

                List<ImageAlbum> list = new List<ImageAlbum>();
                list.Add(deletedImageAlbum);

                return Ok(new { ImageAlbum = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }

        [HttpGet]
        public IHttpActionResult getMenu(int SubscriberID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);
                Connection.Open();
                string Query = "select ImageString from post where MenuPresence = 1 and SubscriberID = " + SubscriberID;
                MySqlCommand getCommand = new MySqlCommand(Query, Connection);
                List<Post> list = new List<Post>();

                Post retrievedPost = new Post();

                MySqlDataReader SelectReader = getCommand.ExecuteReader();

                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        retrievedPost.ImageString = (string)SelectReader.GetValue(0);

                        if (!retrievedPost.ImageString.Equals("none"))
                        {
                            list.Add(retrievedPost);
                        }

                        retrievedPost = new Post();
                    }
                }

                Connection.Close();

                return Ok(new { Post = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        //Post Table - CRUD Operations
        [HttpPost]
        public IHttpActionResult createPost([FromBody] Post post)
        {
            try
            {

                string url = "none";

                if (!post.ImageString.Equals("none"))
                {
                    byte[] bytes = Convert.FromBase64String(post.ImageString);
                    Stream stream = new MemoryStream(bytes);
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription("image", stream)
                    };

                    var uploadResult = cloudinary.Upload(uploadParams);

                    try
                    {
                        url = (string)uploadResult.JsonObj.SelectToken("url");
                    }
                    catch (Exception ex)
                    { url = "error"; }
                }

                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "INSERT INTO post(SubscriberID, ImagePresence, ReviewPresence, CheckinPresence, Privacy, Timestamp, PostDescription, ImageString, MenuPresence) VALUES(" + Convert.ToInt32(post.SubscriberID) + "," + Convert.ToInt32(post.ImagePresence) + "," + Convert.ToInt32(post.ReviewPresence) + "," + Convert.ToInt32(post.CheckinPresence) + ",'" + post.Privacy + "','" + post.TimeStamp + "'" + ",'" + post.PostDescription + "','" + url + "'," + post.MenuPresence + ")";

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                Post addedPost = post;

                List<Post> list = new List<Post>();
                list.Add(addedPost);

                return Ok(new { Post = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        //[HttpGet]
        //public IHttpActionResult getPost(int ID)
        //{
        //    try
        //    {
        //        MySqlConnection Connection = new MySqlConnection(ConnectionString);

        //        Connection.Open();

        //        string Query = "SELECT * FROM post WHERE PostID = " + ID;

        //        MySqlCommand getCommand = new MySqlCommand(Query, Connection);

        //        Post retrievedPost = new Post();

        //        using (MySqlDataReader Reader = getCommand.ExecuteReader())
        //        {
        //            if (Reader.HasRows && Reader.Read())
        //            {
        //                int counter = 1;

        //                for (int i = 0; i < Reader.FieldCount; i++)
        //                {
        //                    if (counter == 1)
        //                    { retrievedPost.PostID = (int)Reader.GetValue(i); }
        //                    else if (counter == 2)
        //                    { retrievedPost.SubscriberID = (int)Reader.GetValue(i); }
        //                    else if (counter == 3)
        //                    { retrievedPost.ImagePresence = (int)Reader.GetValue(i); }
        //                    else if (counter == 4)
        //                    {
        //                        try
        //                        {
        //                            retrievedPost.ImageAlbumID = (int)Reader.GetValue(i);
        //                        }
        //                        catch (Exception e)
        //                        {
        //                            retrievedPost.ImageAlbumID = 1000;
        //                        }
        //                    }
        //                    else if (counter == 5)
        //                    { retrievedPost.ReviewPresence = (int)Reader.GetValue(i); }
        //                    else if (counter == 6)
        //                    { retrievedPost.CheckinPresence = (int)Reader.GetValue(i); }
        //                    else if (counter == 7)
        //                    { retrievedPost.Privacy = (string)Reader.GetValue(i); }
        //                    else if (counter == 8)
        //                    { retrievedPost.TimeStamp = (string)Reader.GetValue(i); }
        //                    else if (counter == 9)
        //                    { retrievedPost.PostDescription = (string)Reader.GetValue(i); }
        //                    else if (counter == 10)
        //                    {
        //                        try
        //                        {
        //                            retrievedPost.ImageString = (String)Reader.GetValue(i);
        //                        }
        //                        catch (Exception e)
        //                        {
        //                            retrievedPost.ImageString = "";
        //                        }
        //                    }

        //                    counter++;
        //                }
        //            }
        //        }


        //        List<Post> list = new List<Post>();
        //        list.Add(retrievedPost);

        //        Connection.Close();

        //        return Ok(new { Post = list.AsEnumerable().ToList() });
        //    }
        //    catch (Exception ex)
        //    { }


        //    return null;
        //}

        [HttpGet]
        public IHttpActionResult getAllPosts()
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "select PostID, post.SubscriberID, SubscriberName, ImagePresence, ImageAlbumID, ReviewPresence, CheckinPresence, Privacy, Timestamp, PostDescription, ImageString, DisplayPicture from post INNER JOIN subscriber ON post.SubscriberID=subscriber.SubscriberID";

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                List<Post> list = new List<Post>();

                Post retrievedPost = new Post();

                MySqlDataReader SelectReader = getCommand.ExecuteReader();

                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        retrievedPost.PostID = (int)SelectReader.GetValue(0);
                        retrievedPost.SubscriberID = (int)SelectReader.GetValue(1);
                        retrievedPost.SubscriberName = (string)SelectReader.GetValue(2);
                        retrievedPost.ImagePresence = (int)SelectReader.GetValue(3);

                        try
                        {
                            retrievedPost.ImageAlbumID = (int)SelectReader.GetValue(4);
                        }
                        catch (Exception e)
                        {
                            retrievedPost.ImageAlbumID = 1000;
                        }

                        retrievedPost.ReviewPresence = (int)SelectReader.GetValue(5);
                        retrievedPost.CheckinPresence = (int)SelectReader.GetValue(6);
                        retrievedPost.Privacy = (string)SelectReader.GetValue(7);
                        retrievedPost.TimeStamp = (string)SelectReader.GetValue(8);
                        retrievedPost.PostDescription = (string)SelectReader.GetValue(9);

                        try
                        {
                            retrievedPost.ImageString = (string)SelectReader.GetValue(10);
                        }
                        catch (Exception e)
                        {
                            retrievedPost.ImageString = "none";
                        }

                        try
                        {
                            retrievedPost.DisplayPicture = (string)SelectReader.GetValue(11);
                        }
                        catch (Exception e)
                        {
                            retrievedPost.DisplayPicture = "none";
                        }

                        retrievedPost.CommentsCount = getCommentsCount(retrievedPost.PostID);
                        retrievedPost.LikesCount = getLikesCount(retrievedPost.PostID);

                        list.Add(retrievedPost);

                        retrievedPost = new Post();
                    }
                }

                Connection.Close();

                return Ok(new { Post = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }

        [HttpGet]
        public IHttpActionResult getAllPosts(int SubscriberID, string Scope)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);
                Connection.Open();
                string Query;

                if (Scope.Equals("NewsFeed"))
                {
                    Query = "select PostID, post.SubscriberID, SubscriberName, ImagePresence, ImageAlbumID, ReviewPresence, CheckinPresence, Privacy, Timestamp, PostDescription, ImageString, DisplayPicture from post INNER JOIN subscriber ON post.SubscriberID=subscriber.SubscriberID WHERE MenuPresence <> 1";
                }
                else
                {
                    Query = "select PostID, post.SubscriberID, SubscriberName, ImagePresence, ImageAlbumID, ReviewPresence, CheckinPresence, Privacy, Timestamp, PostDescription, ImageString from post INNER JOIN subscriber ON post.SubscriberID=subscriber.SubscriberID WHERE MenuPresence <> 1 and post.SubscriberID = " + SubscriberID;
                }

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);
                List<Post> list = new List<Post>();
                Post retrievedPost = new Post();
                MySqlDataReader SelectReader = getCommand.ExecuteReader();
                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        retrievedPost.PostID = (int)SelectReader.GetValue(0);
                        retrievedPost.SubscriberID = (int)SelectReader.GetValue(1);
                        retrievedPost.SubscriberName = (string)SelectReader.GetValue(2);
                        retrievedPost.ImagePresence = (int)SelectReader.GetValue(3);

                        try
                        {
                            retrievedPost.ImageAlbumID = (int)SelectReader.GetValue(4);
                        }
                        catch (Exception e)
                        {
                            retrievedPost.ImageAlbumID = 1000;
                        }

                        retrievedPost.ReviewPresence = (int)SelectReader.GetValue(5);
                        retrievedPost.CheckinPresence = (int)SelectReader.GetValue(6);
                        retrievedPost.Privacy = (string)SelectReader.GetValue(7);
                        retrievedPost.TimeStamp = (string)SelectReader.GetValue(8);
                        retrievedPost.PostDescription = (string)SelectReader.GetValue(9);

                        try
                        {
                            retrievedPost.ImageString = (string)SelectReader.GetValue(10);
                        }
                        catch (Exception e)
                        {
                            retrievedPost.ImageString = "none";
                        }

                        try
                        {
                            retrievedPost.DisplayPicture = (string)SelectReader.GetValue(11);
                        }
                        catch (Exception e)
                        {
                            retrievedPost.DisplayPicture = "none";
                        }

                        try
                        {
                            retrievedPost.MenuPresence = (int)SelectReader.GetValue(12);
                        }
                        catch (Exception e)
                        {
                            retrievedPost.MenuPresence = 0;
                        }

                        retrievedPost.CommentsCount = getCommentsCount(retrievedPost.PostID);
                        retrievedPost.LikesCount = getLikesCount(retrievedPost.PostID);

                        if (retrievedPost.LikesCount != 0)
                        {
                            MySqlConnection likeConnection = new MySqlConnection(ConnectionString);
                            likeConnection.Open();
                            string likeQuery = "SELECT COUNT(*) FROM like_table WHERE SubscriberID = " + SubscriberID + " and PostID = " + retrievedPost.PostID;
                            MySqlCommand likeCommand = new MySqlCommand(likeQuery, likeConnection);
                            MySqlDataReader SelectLikeReader = likeCommand.ExecuteReader();
                            while (SelectLikeReader.Read())
                            {
                                if (SelectLikeReader.HasRows)
                                {
                                    retrievedPost.CurrentUsersLike = Convert.ToInt32(SelectLikeReader.GetValue(0)) == 1 ? "Yes" : "No";
                                }
                            }
                            likeConnection.Close();
                        }

                        retrievedPost.CurrentUsersLike = retrievedPost.CurrentUsersLike == null ? "No" : retrievedPost.CurrentUsersLike;

                        list.Add(retrievedPost);

                        retrievedPost = new Post();
                    }
                }

                Connection.Close();

                return Ok(new { Post = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }

        #region Get Comments Count
        private int getCommentsCount(int PostID)
        {
            int CommentsCount = 0;
            MySqlConnection CommentsConnection = new MySqlConnection(ConnectionString);
            try
            {
                CommentsConnection.Open();
                string commentQuery = "select COUNT(*) from post JOIN comment_table ON post.PostID=comment_table.PostID where comment_table.PostID=" + PostID;
                MySqlCommand getCommentCommand = new MySqlCommand(commentQuery, CommentsConnection);
                MySqlDataReader SelectCommentReader = getCommentCommand.ExecuteReader();
                while (SelectCommentReader.Read())
                {
                    if (SelectCommentReader.HasRows)
                    {
                        CommentsCount = Convert.ToInt32(SelectCommentReader.GetValue(0));
                    }
                }
            }
            catch (Exception ex)
            {
                CommentsCount = 0;
            }

            CommentsConnection.Close();
            return CommentsCount;
        }
        #endregion

        #region Get Likes Count
        private int getLikesCount(int PostID)
        {
            int LikesCount = 0;
            MySqlConnection LikesConnection = new MySqlConnection(ConnectionString);
            try
            {
                LikesConnection.Open();
                string likeQuery = "select COUNT(*) from post JOIN like_table ON post.PostID=like_table.PostID where like_table.PostID=" + PostID;
                MySqlCommand getLikeCommand = new MySqlCommand(likeQuery, LikesConnection);
                MySqlDataReader SelectLikeReader = getLikeCommand.ExecuteReader();
                while (SelectLikeReader.Read())
                {
                    if (SelectLikeReader.HasRows)
                    {
                        LikesCount = Convert.ToInt32(SelectLikeReader.GetValue(0));
                    }
                }
            }
            catch (Exception ex)
            {
                LikesCount = 0;
            }

            LikesConnection.Close();
            return LikesCount;
        }
        #endregion

        //[HttpPost]
        //public IHttpActionResult updatePost(int ID, int SubscriberID, int ImagePresence, int ImageAlbumID, int ReviewPresence, int CheckinPresence, string Privacy, string Timestamp, string PostDescription)
        //{
        //    try
        //    {
        //        MySqlConnection Connection = new MySqlConnection(ConnectionString);

        //        Connection.Open();

        //        string Query = "UPDATE post SET SubscriberID=" + SubscriberID + ", ImagePresence=" + ImagePresence + ", ReviewPresence=" + ReviewPresence + ", CheckinPresence=" + CheckinPresence + ", Privacy='" + Privacy + ", Timestamp='" + Timestamp + ", PostDescription='" + PostDescription + "'" + " WHERE PostID=" + ID;

        //        MySqlCommand updateCommand = new MySqlCommand(Query, Connection);

        //        updateCommand.ExecuteNonQuery();

        //        Connection.Close();

        //        Post updatedPost = new Post();
        //        updatedPost.SubscriberID = SubscriberID;
        //        updatedPost.ImagePresence = ImagePresence;
        //        updatedPost.ImageAlbumID = ImageAlbumID;
        //        updatedPost.ReviewPresence = ReviewPresence;
        //        updatedPost.CheckinPresence = CheckinPresence;
        //        updatedPost.Privacy = Privacy;
        //        updatedPost.TimeStamp = Timestamp;
        //        updatedPost.PostDescription = PostDescription;

        //        List<Post> list = new List<Post>();
        //        list.Add(updatedPost);

        //        return Ok(new { Post = list.AsEnumerable().ToList() });
        //    }
        //    catch (Exception ex)
        //    { }

        //    return null;
        //}


        //[HttpPost]
        //public IHttpActionResult deletePost(int ID)
        //{
        //    try
        //    {
        //        MySqlConnection Connection = new MySqlConnection(ConnectionString);

        //        Connection.Open();

        //        string Query = "DELETE FROM post WHERE PostID=" + ID;

        //        MySqlCommand deleteCommand = new MySqlCommand(Query, Connection);

        //        deleteCommand.ExecuteNonQuery();

        //        Connection.Close();

        //        Post deletedPost = new Post();
        //        deletedPost.PostID = ID;

        //        List<Post> list = new List<Post>();
        //        list.Add(deletedPost);

        //        return Ok(new { Post = list.AsEnumerable().ToList() });
        //    }
        //    catch (Exception ex)
        //    { }

        //    return null;
        //}



        // Images Table - CRUD Operations

        [HttpPost]
        public IHttpActionResult createImage(int ID, int ImageAlbumID, int PostID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "INSERT INTO images VALUES(" + ID + "," + ImageAlbumID + "," + PostID + ")";

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                Images addedImage = new Images();
                addedImage.ImageID = ID;
                addedImage.ImageAlbumID = ImageAlbumID;
                addedImage.PostID = PostID;

                List<Images> list = new List<Images>();
                list.Add(addedImage);

                return Ok(new { Images = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getImage(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM images WHERE ImageID = " + ID;

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                Images retrievedImage = new Images();

                using (MySqlDataReader Reader = getCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        int counter = 1;

                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (counter == 1)
                            { retrievedImage.ImageID = (int)Reader.GetValue(i); }
                            else if (counter == 2)
                            { retrievedImage.ImageAlbumID = (int)Reader.GetValue(i); }
                            else if (counter == 3)
                            { retrievedImage.PostID = (int)Reader.GetValue(i); }

                            counter++;
                        }
                    }
                }


                List<Images> list = new List<Images>();
                list.Add(retrievedImage);

                Connection.Close();

                return Ok(new { Images = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getAllImages(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM images WHERE SubscriberID=" + ID;

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                List<Images> list = new List<Images>();

                Images retrievedImage = new Images();

                MySqlDataReader SelectReader = getCommand.ExecuteReader();

                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        retrievedImage.ImageID = (int)SelectReader.GetValue(0);
                        retrievedImage.ImageAlbumID = (int)SelectReader.GetValue(1);
                        retrievedImage.PostID = (int)SelectReader.GetValue(2);

                        list.Add(retrievedImage);

                        retrievedImage = new Images();
                    }
                }

                Connection.Close();

                return Ok(new { Images = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult updateImage(int ID, int PostID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "UPDATE images SET PostID=" + PostID + " WHERE ImageID=" + ID;

                MySqlCommand updateCommand = new MySqlCommand(Query, Connection);

                updateCommand.ExecuteNonQuery();

                Connection.Close();

                Images updatedImage = new Images();
                updatedImage.ImageID = ID;
                updatedImage.PostID = PostID;

                List<Images> list = new List<Images>();
                list.Add(updatedImage);

                return Ok(new { Images = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult deleteImage(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "DELETE FROM images WHERE ImageID=" + ID;

                MySqlCommand deleteCommand = new MySqlCommand(Query, Connection);

                deleteCommand.ExecuteNonQuery();

                Connection.Close();

                Images deletedImage = new Images();
                deletedImage.ImageID = ID;

                List<Images> list = new List<Images>();
                list.Add(deletedImage);

                return Ok(new { Image = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }




        // Like Table - CRUD Operations
        [HttpPost]
        public IHttpActionResult createLike([FromBody] Like like)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "INSERT INTO like_table(subscriberID, PostID, Timestamp) VALUES(" + like.SubscriberID + "," + like.PostID + ",'" + like.TimeStamp + "')";

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                Like addedLike = new Like();
                addedLike.SubscriberID = like.SubscriberID;
                addedLike.PostID = like.PostID;
                addedLike.TimeStamp = like.TimeStamp;

                List<Like> list = new List<Like>();
                list.Add(addedLike);

                return Ok(new { Like = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getLike(string ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM like_table WHERE LikeID = " + ID;

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                Like retrievedLike = new Like();

                using (MySqlDataReader Reader = getCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        int counter = 1;

                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (counter == 1)
                            { retrievedLike.LikeID = (string)Reader.GetValue(i); }
                            else if (counter == 2)
                            { retrievedLike.SubscriberID = (string)Reader.GetValue(i); }
                            else if (counter == 3)
                            { retrievedLike.PostID = (string)Reader.GetValue(i); }
                            else if (counter == 4)
                            { retrievedLike.TimeStamp = (string)Reader.GetValue(i); }

                            counter++;
                        }
                    }
                }


                List<Like> list = new List<Like>();
                list.Add(retrievedLike);

                Connection.Close();

                return Ok(new { Like = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getAllLikes(string ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM like_table WHERE SubscriberID=" + ID;

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                List<Like> list = new List<Like>();

                Like retrievedLike = new Like();

                MySqlDataReader SelectReader = getCommand.ExecuteReader();

                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        retrievedLike.LikeID = (string)SelectReader.GetValue(0);
                        retrievedLike.SubscriberID = (string)SelectReader.GetValue(1);
                        retrievedLike.PostID = (string)SelectReader.GetValue(2);
                        retrievedLike.TimeStamp = (string)SelectReader.GetValue(3);

                        list.Add(retrievedLike);

                        retrievedLike = new Like();
                    }
                }

                Connection.Close();

                return Ok(new { Like = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult deleteLike(string SubscriberID, string PostID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "DELETE FROM like_table WHERE SubscriberID=" + SubscriberID + " and PostID=" + PostID;

                MySqlCommand deleteCommand = new MySqlCommand(Query, Connection);

                deleteCommand.ExecuteNonQuery();

                Connection.Close();

                Like deletedLike = new Like();
                deletedLike.SubscriberID = SubscriberID;
                deletedLike.PostID = PostID;

                List<Like> list = new List<Like>();
                list.Add(deletedLike);

                return Ok(new { Like = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }



        // Comment Table - CRUD Operations
        [HttpPost]
        public IHttpActionResult createComment([FromBody] Comment comment)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "INSERT INTO comment_table(SubscriberID, PostID, CommentText, Timestamp) VALUES(" + comment.SubscriberID + "," + comment.PostID + ",'" + comment.CommentText + "','" + comment.TimeStamp + "')";

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                Comment addedComment = new Comment();
                addedComment.SubscriberID = comment.SubscriberID;
                addedComment.PostID = comment.PostID;
                addedComment.CommentText = comment.CommentText;
                addedComment.TimeStamp = comment.TimeStamp;

                List<Comment> list = new List<Comment>();
                list.Add(addedComment);

                return Ok(new { Comment = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getComment(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM comment_table WHERE CommentID = " + ID;

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                Comment retrievedComment = new Comment();

                using (MySqlDataReader Reader = getCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        int counter = 1;

                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (counter == 1)
                            { retrievedComment.CommentID = (int)Reader.GetValue(i); }
                            else if (counter == 2)
                            { retrievedComment.SubscriberID = (int)Reader.GetValue(i); }
                            else if (counter == 3)
                            { retrievedComment.PostID = (int)Reader.GetValue(i); }
                            else if (counter == 4)
                            { retrievedComment.CommentText = (string)Reader.GetValue(i); }
                            else if (counter == 5)
                            { retrievedComment.TimeStamp = (string)Reader.GetValue(i); }

                            counter++;
                        }
                    }
                }


                List<Comment> list = new List<Comment>();
                list.Add(retrievedComment);

                Connection.Close();

                return Ok(new { Comment = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getAllComments(int PostID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT subscriber.SubscriberName, comment_table.CommentText, comment_table.Timestamp, DisplayPicture FROM comment_table INNER JOIN subscriber ON comment_table.SubscriberID=subscriber.SubscriberID where PostID=" + PostID;

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                List<Comment> list = new List<Comment>();

                Comment retrievedComment = new Comment();

                MySqlDataReader SelectReader = getCommand.ExecuteReader();

                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        retrievedComment.SubscriberName = (string)SelectReader.GetValue(0);
                        retrievedComment.CommentText = (string)SelectReader.GetValue(1);
                        retrievedComment.TimeStamp = (string)SelectReader.GetValue(2);
                        retrievedComment.DisplayPicture = (string)SelectReader.GetValue(3);

                        list.Add(retrievedComment);

                        retrievedComment = new Comment();
                    }
                }

                Connection.Close();

                return Ok(new { Comment = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult updateComment(int ID, string CommentText)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "UPDATE comment_table SET CommentText='" + CommentText + "' WHERE CommentID=" + ID;

                MySqlCommand updateCommand = new MySqlCommand(Query, Connection);

                updateCommand.ExecuteNonQuery();

                Connection.Close();

                Comment updatedComment = new Comment();
                updatedComment.CommentID = ID;
                updatedComment.CommentText = CommentText;

                List<Comment> list = new List<Comment>();
                list.Add(updatedComment);

                return Ok(new { Comment = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult deleteComment(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "DELETE FROM comment_table WHERE CommentID=" + ID;

                MySqlCommand deleteCommand = new MySqlCommand(Query, Connection);

                deleteCommand.ExecuteNonQuery();

                Connection.Close();

                Comment deletedComment = new Comment();
                deletedComment.CommentID = ID;

                List<Comment> list = new List<Comment>();
                list.Add(deletedComment);

                return Ok(new { Comment = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }



        // Tag Table - CRUD Operations
        [HttpPost]
        public IHttpActionResult createTag(int ID, int SubscriberID, int TaggedSubscriber, int PostID, string Timestamp)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "INSERT INTO tag VALUES(" + ID + "," + SubscriberID + "," + TaggedSubscriber + "," + PostID + ",'" + Timestamp + "')";

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                Tag addedTag = new Tag();
                addedTag.TagID = ID;
                addedTag.SubscriberID = SubscriberID;
                addedTag.TaggedSubscriber = TaggedSubscriber;
                addedTag.PostID = PostID;
                addedTag.TimeStamp = Timestamp;

                List<Tag> list = new List<Tag>();
                list.Add(addedTag);

                return Ok(new { Tag = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getTag(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM tag WHERE TagID = " + ID;

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                Tag retrievedTag = new Tag();

                using (MySqlDataReader Reader = getCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        int counter = 1;

                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (counter == 1)
                            { retrievedTag.TagID = (int)Reader.GetValue(i); }
                            else if (counter == 2)
                            { retrievedTag.SubscriberID = (int)Reader.GetValue(i); }
                            else if (counter == 3)
                            { retrievedTag.TaggedSubscriber = (int)Reader.GetValue(i); }
                            else if (counter == 4)
                            { retrievedTag.PostID = (int)Reader.GetValue(i); }
                            else if (counter == 5)
                            { retrievedTag.TimeStamp = (string)Reader.GetValue(i); }

                            counter++;
                        }
                    }
                }


                List<Tag> list = new List<Tag>();
                list.Add(retrievedTag);

                Connection.Close();

                return Ok(new { Tag = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getAllTags(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM tag WHERE SubscriberID=" + ID;

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                List<Tag> list = new List<Tag>();

                Tag retrievedTag = new Tag();

                MySqlDataReader SelectReader = getCommand.ExecuteReader();

                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        retrievedTag.TagID = (int)SelectReader.GetValue(0);
                        retrievedTag.SubscriberID = (int)SelectReader.GetValue(1);
                        retrievedTag.TaggedSubscriber = (int)SelectReader.GetValue(2);
                        retrievedTag.PostID = (int)SelectReader.GetValue(3);
                        retrievedTag.TimeStamp = (string)SelectReader.GetValue(4);

                        list.Add(retrievedTag);

                        retrievedTag = new Tag();
                    }
                }

                Connection.Close();

                return Ok(new { Tag = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult updateTag(int ID, int TaggedSubscriber, int PostID, string Timestamp)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "UPDATE tag SET TaggedSubscriber=" + TaggedSubscriber + ", PostID=" + PostID + ", Timestamp='" + Timestamp + "' WHERE CommentID=" + ID;

                MySqlCommand updateCommand = new MySqlCommand(Query, Connection);

                updateCommand.ExecuteNonQuery();

                Connection.Close();

                Tag updatedTag = new Tag();
                updatedTag.TagID = ID;
                updatedTag.TaggedSubscriber = TaggedSubscriber;
                updatedTag.PostID = PostID;
                updatedTag.TimeStamp = Timestamp;

                List<Tag> list = new List<Tag>();
                list.Add(updatedTag);

                return Ok(new { Tag = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult deleteTag(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "DELETE FROM tag WHERE TagID=" + ID;

                MySqlCommand deleteCommand = new MySqlCommand(Query, Connection);

                deleteCommand.ExecuteNonQuery();

                Connection.Close();

                Tag deletedTag = new Tag();
                deletedTag.TagID = ID;

                List<Tag> list = new List<Tag>();
                list.Add(deletedTag);

                return Ok(new { Tag = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }



        // Restaurant Profile Table - CRUD Operations
        [HttpPost]
        public IHttpActionResult createRestaurantProfile(int ID, int SubscriberID, string Address, string Timing, string Category)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "INSERT INTO restaurantprofile VALUES(" + ID + "," + SubscriberID + ",'" + Address + "','" + Timing + "','" + Category + "')";

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                RestaurantProfile addedRestaurantProfile = new RestaurantProfile();
                addedRestaurantProfile.RestaurantID = ID;
                addedRestaurantProfile.SubscriberID = SubscriberID;
                addedRestaurantProfile.Address = Address;
                addedRestaurantProfile.Timing = Timing;
                addedRestaurantProfile.Category = Category;

                List<RestaurantProfile> list = new List<RestaurantProfile>();
                list.Add(addedRestaurantProfile);

                return Ok(new { RestaurantProfile = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getRestaurantProfile(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM restaurantprofile WHERE RestaurantID = " + ID;

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                RestaurantProfile retrievedRestaurantProfile = new RestaurantProfile();

                using (MySqlDataReader Reader = getCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        int counter = 1;

                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (counter == 1)
                            { retrievedRestaurantProfile.RestaurantID = (int)Reader.GetValue(i); }
                            else if (counter == 2)
                            { retrievedRestaurantProfile.SubscriberID = (int)Reader.GetValue(i); }
                            else if (counter == 3)
                            { retrievedRestaurantProfile.Address = (string)Reader.GetValue(i); }
                            else if (counter == 4)
                            { retrievedRestaurantProfile.Timing = (string)Reader.GetValue(i); }
                            else if (counter == 5)
                            { retrievedRestaurantProfile.Category = (string)Reader.GetValue(i); }

                            counter++;
                        }
                    }
                }


                List<RestaurantProfile> list = new List<RestaurantProfile>();
                list.Add(retrievedRestaurantProfile);

                Connection.Close();

                return Ok(new { RestaurantProfile = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getAllRestaurantProfiles(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM restaurantprofile";

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                List<RestaurantProfile> list = new List<RestaurantProfile>();

                RestaurantProfile retrievedRestaurantProfile = new RestaurantProfile();

                MySqlDataReader SelectReader = getCommand.ExecuteReader();

                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        retrievedRestaurantProfile.RestaurantID = (int)SelectReader.GetValue(0);
                        retrievedRestaurantProfile.SubscriberID = (int)SelectReader.GetValue(1);
                        retrievedRestaurantProfile.Address = (string)SelectReader.GetValue(2);
                        retrievedRestaurantProfile.Timing = (string)SelectReader.GetValue(3);
                        retrievedRestaurantProfile.Category = (string)SelectReader.GetValue(4);

                        list.Add(retrievedRestaurantProfile);

                        retrievedRestaurantProfile = new RestaurantProfile();
                    }
                }

                Connection.Close();

                return Ok(new { RestaurantProfile = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult updateRestaurantProfile(int ID, string Address, string Timing, string Category)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "UPDATE restaurantprofile SET Address='" + Address + "', Timing='" + Timing + "', Category='" + Category + "' WHERE RestaurantID=" + ID;

                MySqlCommand updateCommand = new MySqlCommand(Query, Connection);

                updateCommand.ExecuteNonQuery();

                Connection.Close();

                RestaurantProfile updatedRestaurantProfile = new RestaurantProfile();
                updatedRestaurantProfile.RestaurantID = ID;
                updatedRestaurantProfile.Address = Address;
                updatedRestaurantProfile.Timing = Timing;
                updatedRestaurantProfile.Category = Category;

                List<RestaurantProfile> list = new List<RestaurantProfile>();
                list.Add(updatedRestaurantProfile);

                return Ok(new { RestaurantProfile = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult deleteRestaurantProfile(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "DELETE FROM restaurantprofile WHERE RestaurantID=" + ID;

                MySqlCommand deleteCommand = new MySqlCommand(Query, Connection);

                deleteCommand.ExecuteNonQuery();

                Connection.Close();

                RestaurantProfile deletedRestaurantProfile = new RestaurantProfile();
                deletedRestaurantProfile.RestaurantID = ID;

                List<RestaurantProfile> list = new List<RestaurantProfile>();
                list.Add(deletedRestaurantProfile);

                return Ok(new { RestaurantProfile = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }




        // Review Table - CRUD Operations
        [HttpPost]
        public IHttpActionResult createReview([FromBody] Review review)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);
                Connection.Open();
                string Query = "INSERT INTO post(SubscriberID, ImagePresence, ReviewPresence, CheckinPresence, Privacy, Timestamp, PostDescription, ImageString, MenuPresence) VALUES(" + Convert.ToInt32(review.SubscriberID) + ",0,1,0,'Public','" + review.TimeStamp + "'" + ",'none','none',0)";
                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);
                insertCommand.ExecuteNonQuery();

                Query = "SELECT * FROM post ORDER BY PostID DESC LIMIT 1";
                MySqlCommand getCommand = new MySqlCommand(Query, Connection);
                Post retrievedPost = new Post();
                using (MySqlDataReader Reader = getCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            retrievedPost.PostID = (int)Reader.GetValue(i);
                            break;
                        }
                    }
                }

                Query = "INSERT INTO review(PostID, SubscriberID, RestaurantID, ReviewText, Timestamp, Taste, Ambience, Service, OrderTime, Price) VALUES(" + retrievedPost.PostID + ", " + review.SubscriberID + ", " + review.RestaurantID + ",'" + review.ReviewText + "','" + review.TimeStamp + "','" + review.Taste + "','" + review.Ambience + "','" + review.Service + "','" + review.OrderTime + "','" + review.Price + "')";
                insertCommand = new MySqlCommand(Query, Connection);
                insertCommand.ExecuteNonQuery();

                Connection.Close();

                List<Review> list = new List<Review>();
                Review addedReview = new Review();
                addedReview.ReviewID = 0;
                addedReview.PostID = retrievedPost.PostID;
                addedReview.SubscriberID = review.SubscriberID;
                addedReview.RestaurantID = review.RestaurantID;
                addedReview.ReviewText = review.ReviewText;
                addedReview.TimeStamp = review.TimeStamp;
                addedReview.Taste = review.Taste;
                addedReview.Ambience = review.Ambience;
                addedReview.Service = review.Service;
                addedReview.OrderTime = review.OrderTime;
                addedReview.Price = review.Price;
                list.Add(addedReview);
                return Ok(new { Review = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getReview(int ReviewID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);
                Connection.Open();
                string Query = "SELECT * FROM review WHERE ReviewID = " + ReviewID;
                MySqlCommand getCommand = new MySqlCommand(Query, Connection);
                Review retrievedReview = new Review();
                List<Review> list = new List<Review>();
                MySqlDataReader SelectReader = getCommand.ExecuteReader();
                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        try
                        { retrievedReview.ReviewID = (int)SelectReader.GetValue(0); }
                        catch (Exception ex)
                        { retrievedReview.ReviewID = 0; }

                        try
                        { retrievedReview.PostID = (int)SelectReader.GetValue(1); }
                        catch (Exception ex)
                        { retrievedReview.PostID = 0; }

                        try
                        { retrievedReview.SubscriberID = (int)SelectReader.GetValue(2); }
                        catch (Exception ex)
                        { retrievedReview.SubscriberID = 0; }

                        try
                        { retrievedReview.RestaurantID = (int)SelectReader.GetValue(3); }
                        catch (Exception ex)
                        { retrievedReview.RestaurantID = 0; }

                        try
                        { retrievedReview.ReviewText = (string)SelectReader.GetValue(4); }
                        catch (Exception ex)
                        { retrievedReview.ReviewText = ""; }

                        try
                        { retrievedReview.TimeStamp = (string)SelectReader.GetValue(5); }
                        catch (Exception ex)
                        { retrievedReview.TimeStamp = ""; }

                        try
                        { retrievedReview.Taste = (string)SelectReader.GetValue(6); }
                        catch (Exception ex)
                        { retrievedReview.Taste = ""; }

                        try
                        { retrievedReview.Ambience = (string)SelectReader.GetValue(7); }
                        catch (Exception ex)
                        { retrievedReview.Ambience = ""; }

                        try
                        { retrievedReview.Service = (string)SelectReader.GetValue(8); }
                        catch (Exception ex)
                        { retrievedReview.Service = ""; }

                        try
                        { retrievedReview.OrderTime = (string)SelectReader.GetValue(9); }
                        catch (Exception ex)
                        { retrievedReview.OrderTime = ""; }

                        try
                        { retrievedReview.Price = (string)SelectReader.GetValue(10); }
                        catch (Exception ex)
                        { retrievedReview.Price = ""; }


                        list.Add(retrievedReview);
                    }
                }           

                Connection.Close();

                return Ok(new { Review = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getAllReviews(int RestaurantID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);
                Connection.Open();
                string Query = "select ReviewID, PostID, review.SubscriberID, RestaurantID, ReviewText, Timestamp, Taste, Ambience, Service, OrderTime, Price, SubscriberName, DisplayPicture from review INNER JOIN subscriber ON review.SubscriberID=subscriber.SubscriberID where RestaurantID = " + RestaurantID;
                MySqlCommand getCommand = new MySqlCommand(Query, Connection);
                List<Review> list = new List<Review>();
                Review retrievedReview = new Review();
                MySqlDataReader SelectReader = getCommand.ExecuteReader();
                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        try
                        { retrievedReview.ReviewID = (int)SelectReader.GetValue(0); }
                        catch (Exception ex)
                        { retrievedReview.ReviewID = 0; }

                        try
                        { retrievedReview.PostID = (int)SelectReader.GetValue(1); }
                        catch (Exception ex)
                        { retrievedReview.PostID = 0; }

                        try
                        { retrievedReview.SubscriberID = (int)SelectReader.GetValue(2); }
                        catch (Exception ex)
                        { retrievedReview.SubscriberID = 0; }

                        try
                        { retrievedReview.RestaurantID = (int)SelectReader.GetValue(3); }
                        catch (Exception ex)
                        { retrievedReview.RestaurantID = 0; }

                        try
                        { retrievedReview.ReviewText = (string)SelectReader.GetValue(4); }
                        catch (Exception ex)
                        { retrievedReview.ReviewText = ""; }

                        try
                        { retrievedReview.TimeStamp = (string)SelectReader.GetValue(5); }
                        catch (Exception ex)
                        { retrievedReview.TimeStamp = ""; }

                        try
                        { retrievedReview.Taste = (string)SelectReader.GetValue(6); }
                        catch (Exception ex)
                        { retrievedReview.Taste = ""; }

                        try
                        { retrievedReview.Ambience = (string)SelectReader.GetValue(7); }
                        catch (Exception ex)
                        { retrievedReview.Ambience = ""; }

                        try
                        { retrievedReview.Service = (string)SelectReader.GetValue(8); }
                        catch (Exception ex)
                        { retrievedReview.Service = ""; }

                        try
                        { retrievedReview.OrderTime = (string)SelectReader.GetValue(9); }
                        catch (Exception ex)
                        { retrievedReview.OrderTime = ""; }

                        try
                        { retrievedReview.Price = (string)SelectReader.GetValue(10); }
                        catch (Exception ex)
                        { retrievedReview.Price = ""; }

                        try
                        { retrievedReview.SubscriberName = (string)SelectReader.GetValue(11); }
                        catch (Exception ex)
                        { retrievedReview.SubscriberName = ""; }

                        try
                        { retrievedReview.DisplayPicture = (string)SelectReader.GetValue(12); }
                        catch (Exception ex)
                        { retrievedReview.DisplayPicture = ""; }

                        list.Add(retrievedReview);

                        retrievedReview = new Review();
                    }
                }

                Connection.Close();

                return Ok(new { Review = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult updateReview(int ID, string ReviewText, string Timestamp)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "UPDATE review SET ReviewText='" + ReviewText + "', Timestamp='" + Timestamp + "' WHERE ReviewID=" + ID;

                MySqlCommand updateCommand = new MySqlCommand(Query, Connection);

                updateCommand.ExecuteNonQuery();

                Connection.Close();

                Review UpdatedReview = new Review();
                UpdatedReview.ReviewID = ID;
                UpdatedReview.ReviewText = ReviewText;
                UpdatedReview.TimeStamp = Timestamp;

                List<Review> list = new List<Review>();
                list.Add(UpdatedReview);

                return Ok(new { ReviewText = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult deleteReview(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "DELETE FROM review WHERE ReviewID=" + ID;

                MySqlCommand deleteCommand = new MySqlCommand(Query, Connection);

                deleteCommand.ExecuteNonQuery();

                Connection.Close();

                Review deletedReview = new Review();
                deletedReview.ReviewID = ID;

                List<Review> list = new List<Review>();
                list.Add(deletedReview);

                return Ok(new { Review = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        // Franchise_Location Table - CRUD Operations
        [HttpPost]
        public IHttpActionResult createFranchise_Location(int ID, int RestaurantID, int LocationID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "INSERT INTO franchise_location VALUES(" + ID + "," + RestaurantID + "," + LocationID + ")";

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                Franchise_Location addedFranchise = new Franchise_Location();
                addedFranchise.FranchiseID = ID;
                addedFranchise.RestaurantID = RestaurantID;
                addedFranchise.LocationID = LocationID;

                List<Franchise_Location> list = new List<Franchise_Location>();
                list.Add(addedFranchise);

                return Ok(new { Franchise_Location = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getFranchise(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM franchise_location WHERE FranchiseID = " + ID;

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                Franchise_Location retrievedFranchise = new Franchise_Location();

                using (MySqlDataReader Reader = getCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        int counter = 1;

                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (counter == 1)
                            { retrievedFranchise.FranchiseID = (int)Reader.GetValue(i); }
                            else if (counter == 2)
                            { retrievedFranchise.RestaurantID = (int)Reader.GetValue(i); }
                            else if (counter == 3)
                            { retrievedFranchise.LocationID = (int)Reader.GetValue(i); }

                            counter++;
                        }
                    }
                }


                List<Franchise_Location> list = new List<Franchise_Location>();
                list.Add(retrievedFranchise);

                Connection.Close();

                return Ok(new { Franchise_Location = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getAllFranchises(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM franchise_location WHERE RestaurantID=" + ID;

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                List<Franchise_Location> list = new List<Franchise_Location>();

                Franchise_Location retrievedFranchise = new Franchise_Location();

                MySqlDataReader SelectReader = getCommand.ExecuteReader();

                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        retrievedFranchise.FranchiseID = (int)SelectReader.GetValue(0);
                        retrievedFranchise.RestaurantID = (int)SelectReader.GetValue(1);
                        retrievedFranchise.LocationID = (int)SelectReader.GetValue(2);

                        list.Add(retrievedFranchise);

                        retrievedFranchise = new Franchise_Location();
                    }
                }

                Connection.Close();

                return Ok(new { Franchise_Location = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult updateFranchise(int ID, int RestaurantID, int LocationID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "UPDATE franchise_location SET RestaurantID=" + RestaurantID + ", LocationID=" + LocationID + " WHERE FranchiseID=" + ID;

                MySqlCommand updateCommand = new MySqlCommand(Query, Connection);

                updateCommand.ExecuteNonQuery();

                Connection.Close();

                Franchise_Location UpdatedFranchise = new Franchise_Location();
                UpdatedFranchise.FranchiseID = ID;
                UpdatedFranchise.RestaurantID = RestaurantID;
                UpdatedFranchise.LocationID = LocationID;

                List<Franchise_Location> list = new List<Franchise_Location>();
                list.Add(UpdatedFranchise);

                return Ok(new { Franchise_Location = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult deleteFranchise(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "DELETE FROM franchise_location WHERE FranchiseID=" + ID;

                MySqlCommand deleteCommand = new MySqlCommand(Query, Connection);

                deleteCommand.ExecuteNonQuery();

                Connection.Close();

                Franchise_Location deletedFranchise = new Franchise_Location();
                deletedFranchise.FranchiseID = ID;

                List<Franchise_Location> list = new List<Franchise_Location>();
                list.Add(deletedFranchise);

                return Ok(new { Franchise_Location = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }



        // Location Table - CRUD Operations
        [HttpPost]
        public IHttpActionResult createLocation(int ID, string City, string Area)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "INSERT INTO location VALUES(" + ID + ",'" + City + "','" + Area + "')";

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                Location addedLocation = new Location();
                addedLocation.LocationID = ID;
                addedLocation.City = City;
                addedLocation.Area = Area;

                List<Location> list = new List<Location>();
                list.Add(addedLocation);

                return Ok(new { Location = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getLocation(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM location WHERE LocationID = " + ID;

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                Location retrievedLocation = new Location();

                using (MySqlDataReader Reader = getCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        int counter = 1;

                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (counter == 1)
                            { retrievedLocation.LocationID = (int)Reader.GetValue(i); }
                            else if (counter == 2)
                            { retrievedLocation.City = (string)Reader.GetValue(i); }
                            else if (counter == 3)
                            { retrievedLocation.Area = (string)Reader.GetValue(i); }

                            counter++;
                        }
                    }
                }


                List<Location> list = new List<Location>();
                list.Add(retrievedLocation);

                Connection.Close();

                return Ok(new { Location = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getAllLocations()
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM location";

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                List<Location> list = new List<Location>();

                Location retrievedLocation = new Location();

                MySqlDataReader SelectReader = getCommand.ExecuteReader();

                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        retrievedLocation.LocationID = (int)SelectReader.GetValue(0);
                        retrievedLocation.City = (string)SelectReader.GetValue(1);
                        retrievedLocation.Area = (string)SelectReader.GetValue(2);

                        list.Add(retrievedLocation);

                        retrievedLocation = new Location();
                    }
                }

                Connection.Close();

                return Ok(new { Location = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult updateLocation(int ID, string City, string Area)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "UPDATE location SET City='" + City + "', Area='" + Area + "' WHERE LocationID=" + ID;

                MySqlCommand updateCommand = new MySqlCommand(Query, Connection);

                updateCommand.ExecuteNonQuery();

                Connection.Close();

                Location UpdatedLocation = new Location();
                UpdatedLocation.LocationID = ID;
                UpdatedLocation.City = City;
                UpdatedLocation.Area = Area;

                List<Location> list = new List<Location>();
                list.Add(UpdatedLocation);

                return Ok(new { Location = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult deleteLocation(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "DELETE FROM location WHERE LocationID=" + ID;

                MySqlCommand deleteCommand = new MySqlCommand(Query, Connection);

                deleteCommand.ExecuteNonQuery();

                Connection.Close();

                Location deletedLocation = new Location();
                deletedLocation.LocationID = ID;

                List<Location> list = new List<Location>();
                list.Add(deletedLocation);

                return Ok(new { Location = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }




        // Checkin Table - CRUD Operations
        [HttpPost]
        public IHttpActionResult createCheckin(int ID, int PostID, int LocationID, int FranchiseID, string Timestamp)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "INSERT INTO checkin VALUES(" + ID + "," + PostID + "," + LocationID + "," + FranchiseID + ",'" + Timestamp + ")";

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                Checkin addedCheckin = new Checkin();
                addedCheckin.CheckinID = ID;
                addedCheckin.PostID = PostID;
                addedCheckin.LocationID = LocationID;
                addedCheckin.FranchiseID = FranchiseID;
                addedCheckin.TimeStamp = Timestamp;

                List<Checkin> list = new List<Checkin>();
                list.Add(addedCheckin);

                return Ok(new { Checkin = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getCheckin(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM checkin WHERE CheckinID = " + ID;

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                Checkin retrievedCheckin = new Checkin();

                using (MySqlDataReader Reader = getCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        int counter = 1;

                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (counter == 1)
                            { retrievedCheckin.CheckinID = (int)Reader.GetValue(i); }
                            else if (counter == 2)
                            { retrievedCheckin.PostID = (int)Reader.GetValue(i); }
                            else if (counter == 3)
                            { retrievedCheckin.LocationID = (int)Reader.GetValue(i); }
                            else if (counter == 4)
                            { retrievedCheckin.FranchiseID = (int)Reader.GetValue(i); }
                            else if (counter == 5)
                            { retrievedCheckin.TimeStamp = (string)Reader.GetValue(i); }

                            counter++;
                        }
                    }
                }


                List<Checkin> list = new List<Checkin>();
                list.Add(retrievedCheckin);

                Connection.Close();

                return Ok(new { Checkin = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getAllCheckin(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM checkin WHERE PostID IN (SELECT PostID FROM post WHERE SubscriberID = " + ID + " AND CheckinPresence=1)";

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                List<Checkin> list = new List<Checkin>();

                Checkin retrievedCheckin = new Checkin();

                MySqlDataReader SelectReader = getCommand.ExecuteReader();

                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        retrievedCheckin.CheckinID = (int)SelectReader.GetValue(0);
                        retrievedCheckin.PostID = (int)SelectReader.GetValue(1);
                        retrievedCheckin.LocationID = (int)SelectReader.GetValue(2);
                        retrievedCheckin.FranchiseID = (int)SelectReader.GetValue(3);
                        retrievedCheckin.TimeStamp = (string)SelectReader.GetValue(4);

                        list.Add(retrievedCheckin);

                        retrievedCheckin = new Checkin();
                    }
                }

                Connection.Close();

                return Ok(new { Checkin = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult updateCheckin(int ID, int LocationID, int FranchiseID, string Timestamp)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "UPDATE checkin SET LocationID=" + LocationID + ", FranchiseID=" + FranchiseID + ", Timestamp='" + Timestamp + "' WHERE CheckinID=" + ID;

                MySqlCommand updateCommand = new MySqlCommand(Query, Connection);

                updateCommand.ExecuteNonQuery();

                Connection.Close();

                Checkin UpdatedCheckin = new Checkin();
                UpdatedCheckin.CheckinID = ID;
                UpdatedCheckin.LocationID = LocationID;
                UpdatedCheckin.FranchiseID = FranchiseID;
                UpdatedCheckin.TimeStamp = Timestamp;

                List<Checkin> list = new List<Checkin>();
                list.Add(UpdatedCheckin);

                return Ok(new { Checkin = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult deleteCheckin(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "DELETE FROM checkin WHERE CheckinID=" + ID;

                MySqlCommand deleteCommand = new MySqlCommand(Query, Connection);

                deleteCommand.ExecuteNonQuery();

                Connection.Close();

                Checkin deletedCheckin = new Checkin();
                deletedCheckin.CheckinID = ID;

                List<Checkin> list = new List<Checkin>();
                list.Add(deletedCheckin);

                return Ok(new { Checkin = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }
    }
}
