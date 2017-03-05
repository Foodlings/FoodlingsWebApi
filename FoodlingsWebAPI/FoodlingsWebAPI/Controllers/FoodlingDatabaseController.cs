﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FoodlingsWebAPI.Models.Database_Model;
using MySql.Data.MySqlClient;

namespace FoodlingsWebAPI.Controllers
{
    public class FoodlingDatabaseController : ApiController
    {

        string ConnectionString = "Database=acsm_5ef0b0e6a67e8cb;Data Source=ap-cdbr-azure-east-a.cloudapp.net;User Id=b5b082fd287046;Password=f6f10d0c";



        // ======================================= Tables CRUD Operations ======================================


        // Subscriber Table - CRUD Operations
        [HttpPost]
        public IHttpActionResult createSubscriber(int ID, string SubscriberName, string Password, string Type, string Email, int DisplayPictureID, string PhoneNumber, string Bio, string Gender, string DoB)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "INSERT INTO subscriber VALUES(" + ID + ",'" + SubscriberName + "'" + ",'" + Password + "'" + ",'" + Type + "'" + ",'" + Email + "'" + "," + DisplayPictureID + ",'" + PhoneNumber + "'" + ",'" + Bio + "'" + ",'" + Gender + "'" + ",'" + DoB + "')";

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                Subscriber addedSubscriber = new Subscriber();
                addedSubscriber.SubscriberID = ID;
                addedSubscriber.SubscriberName = SubscriberName;
                addedSubscriber.Password = Password;
                addedSubscriber.Type = Type;
                addedSubscriber.Email = Email;
                addedSubscriber.DisplayPictureID = DisplayPictureID;
                addedSubscriber.PhoneNumber = PhoneNumber;
                addedSubscriber.Bio = Bio;
                addedSubscriber.Gender = Gender;
                addedSubscriber.DoB = DoB;

                List<Subscriber> list = new List<Subscriber>();
                list.Add(addedSubscriber);

                return Ok(new { Subscriber = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getSubscriber(string SubscriberName)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM subscriber WHERE SubscriberName = '" + SubscriberName + "'";

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
                            { retrievedSubscriber.DisplayPictureID = (int)Reader.GetValue(i); }
                            else if (counter == 7)
                            { retrievedSubscriber.PhoneNumber = (String)Reader.GetValue(i); }
                            else if (counter == 8)
                            { retrievedSubscriber.Bio = (string)Reader.GetValue(i); }
                            else if (counter == 9)
                            { retrievedSubscriber.Gender = (string)Reader.GetValue(i); }
                            else if (counter == 10)
                            { retrievedSubscriber.DoB = (string)Reader.GetValue(i); }

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



        // Post Table - CRUD Operations
        [HttpPost]
        public IHttpActionResult createPost(int ID, int SubscriberID, int ImagePresence, int ImageAlbumID, int ReviewPresence, int CheckinPresence, string Privacy, string Timestamp, string PostDescription)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "INSERT INTO post VALUES(" + ID + "," + SubscriberID + "," + ImagePresence + "," + ImageAlbumID +  "," + ReviewPresence + "," + CheckinPresence + ",'" + Privacy + "'" + ",'" + Timestamp + "'" + ",'" + PostDescription + "')";

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                Post addedPost = new Post();
                addedPost.PostID = ID;
                addedPost.SubscriberID = SubscriberID;
                addedPost.ImagePresence = ImagePresence;
                addedPost.ImageAlbumID = ImageAlbumID;
                addedPost.ReviewPresence = ReviewPresence;
                addedPost.CheckinPresence = CheckinPresence;
                addedPost.Privacy = Privacy;
                addedPost.TimeStamp = Timestamp;
                addedPost.PostDescription = PostDescription;

                List<Post> list = new List<Post>();
                list.Add(addedPost);

                return Ok(new { Post = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getPost(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM post WHERE PostID = " + ID;

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                Post retrievedPost = new Post();

                using (MySqlDataReader Reader = getCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        int counter = 1;

                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (counter == 1)
                            { retrievedPost.PostID = (int)Reader.GetValue(i); }
                            else if (counter == 2)
                            { retrievedPost.SubscriberID = (int)Reader.GetValue(i); }
                            else if (counter == 3)
                            { retrievedPost.ImagePresence = (int)Reader.GetValue(i); }
                            else if (counter == 4)
                            { retrievedPost.ImageAlbumID = (int)Reader.GetValue(i); }
                            else if (counter == 5)
                            { retrievedPost.ReviewPresence = (int)Reader.GetValue(i); }
                            else if (counter == 6)
                            { retrievedPost.CheckinPresence = (int)Reader.GetValue(i); }
                            else if (counter == 7)
                            { retrievedPost.Privacy = (string)Reader.GetValue(i); }
                            else if (counter == 8)
                            { retrievedPost.TimeStamp = (string)Reader.GetValue(i); }
                            else if (counter == 9)
                            { retrievedPost.PostDescription = (string)Reader.GetValue(i); }

                            counter++;
                        }
                    }
                }


                List<Post> list = new List<Post>();
                list.Add(retrievedPost);

                Connection.Close();

                return Ok(new { Post = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }


            return null;
        }


        [HttpGet]
        public IHttpActionResult getAllPosts(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM post WHERE SubscriberID = " + ID;

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
                        retrievedPost.ImagePresence = (int)SelectReader.GetValue(2);
                        retrievedPost.ImageAlbumID = (int)SelectReader.GetValue(3);
                        retrievedPost.ReviewPresence = (int)SelectReader.GetValue(4);
                        retrievedPost.CheckinPresence = (int)SelectReader.GetValue(5);
                        retrievedPost.Privacy = (string)SelectReader.GetValue(6);
                        retrievedPost.TimeStamp = (string)SelectReader.GetValue(7);
                        retrievedPost.PostDescription = (string)SelectReader.GetValue(8);

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


        [HttpPost]
        public IHttpActionResult updatePost(int ID, int SubscriberID, int ImagePresence, int ImageAlbumID, int ReviewPresence, int CheckinPresence, string Privacy, string Timestamp, string PostDescription)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "UPDATE post SET SubscriberID=" + SubscriberID + ", ImagePresence=" + ImagePresence + ", ReviewPresence=" + ReviewPresence + ", CheckinPresence=" + CheckinPresence + ", Privacy='" + Privacy + ", Timestamp='" + Timestamp + ", PostDescription='" + PostDescription + "'" + " WHERE PostID=" + ID;

                MySqlCommand updateCommand = new MySqlCommand(Query, Connection);

                updateCommand.ExecuteNonQuery();

                Connection.Close();

                Post updatedPost = new Post();
                updatedPost.SubscriberID = SubscriberID;
                updatedPost.ImagePresence = ImagePresence;
                updatedPost.ImageAlbumID = ImageAlbumID;
                updatedPost.ReviewPresence = ReviewPresence;
                updatedPost.CheckinPresence = CheckinPresence;
                updatedPost.Privacy = Privacy;
                updatedPost.TimeStamp = Timestamp;
                updatedPost.PostDescription = PostDescription;

                List<Post> list = new List<Post>();
                list.Add(updatedPost);

                return Ok(new { Post = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpPost]
        public IHttpActionResult deletePost(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "DELETE FROM post WHERE PostID=" + ID;

                MySqlCommand deleteCommand = new MySqlCommand(Query, Connection);

                deleteCommand.ExecuteNonQuery();

                Connection.Close();

                Post deletedPost = new Post();
                deletedPost.PostID = ID;

                List<Post> list = new List<Post>();
                list.Add(deletedPost);

                return Ok(new { Post = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }



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
        public IHttpActionResult createLike(int ID, int SubscriberID, int PostID, string Timestamp)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "INSERT INTO like_table VALUES(" + ID + "," + SubscriberID + "," + PostID + ",'" + Timestamp +  "')";

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                Like addedLike = new Like();
                addedLike.LikeID = ID;
                addedLike.SubscriberID = SubscriberID;
                addedLike.PostID = PostID;
                addedLike.TimeStamp = Timestamp;

                List<Like> list = new List<Like>();
                list.Add(addedLike);

                return Ok(new { Like = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getLike(int ID)
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
                            { retrievedLike.LikeID = (int)Reader.GetValue(i); }
                            else if (counter == 2)
                            { retrievedLike.SubscriberID = (int)Reader.GetValue(i); }
                            else if (counter == 3)
                            { retrievedLike.PostID = (int)Reader.GetValue(i); }
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
        public IHttpActionResult getAllLikes(int ID)
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
                        retrievedLike.LikeID = (int)SelectReader.GetValue(0);
                        retrievedLike.SubscriberID = (int)SelectReader.GetValue(1);
                        retrievedLike.PostID = (int)SelectReader.GetValue(2);
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
        public IHttpActionResult deleteLike(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "DELETE FROM like_table WHERE LikeID=" + ID;

                MySqlCommand deleteCommand = new MySqlCommand(Query, Connection);

                deleteCommand.ExecuteNonQuery();

                Connection.Close();

                Like deletedLike = new Like();
                deletedLike.LikeID = ID;

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
        public IHttpActionResult createComment(int ID, int SubscriberID, int PostID, string CommentText, string Timestamp)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "INSERT INTO comment_table VALUES(" + ID + "," + SubscriberID + "," + PostID + ",'" + CommentText + "','" + Timestamp + "')";

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                Comment addedComment = new Comment();
                addedComment.CommentID = ID;
                addedComment.SubscriberID = SubscriberID;
                addedComment.PostID = PostID;
                addedComment.CommentText = CommentText;
                addedComment.TimeStamp = Timestamp;

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
        public IHttpActionResult getAllComments(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM comment_table WHERE SubscriberID=" + ID;

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                List<Comment> list = new List<Comment>();

                Comment retrievedComment = new Comment();

                MySqlDataReader SelectReader = getCommand.ExecuteReader();

                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        retrievedComment.CommentID = (int)SelectReader.GetValue(0);
                        retrievedComment.SubscriberID = (int)SelectReader.GetValue(1);
                        retrievedComment.PostID = (int)SelectReader.GetValue(2);
                        retrievedComment.CommentText = (string)SelectReader.GetValue(3);
                        retrievedComment.TimeStamp = (string)SelectReader.GetValue(4);

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
        public IHttpActionResult createTag(int ID, int SubscriberID, int TaggedSubscriber , int PostID, string Timestamp)
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
        public IHttpActionResult createReview(int ID, int PostID, int RestaurantID, string ReviewText, string Timestamp)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "INSERT INTO review VALUES(" + ID + "," + PostID + "," + RestaurantID + ",'" + ReviewText + "','" + Timestamp + "')";

                MySqlCommand insertCommand = new MySqlCommand(Query, Connection);

                insertCommand.ExecuteNonQuery();

                Connection.Close();

                Review addedReview = new Review();
                addedReview.ReviewID = ID;
                addedReview.PostID = PostID;
                addedReview.RestaurantID = RestaurantID;
                addedReview.ReviewText = ReviewText;
                addedReview.TimeStamp = Timestamp;

                List<Review> list = new List<Review>();
                list.Add(addedReview);

                return Ok(new { Review = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getReview(int ID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "SELECT * FROM review WHERE ReviewID = " + ID;

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                Review retrievedReview = new Review();

                using (MySqlDataReader Reader = getCommand.ExecuteReader())
                {
                    if (Reader.HasRows && Reader.Read())
                    {
                        int counter = 1;

                        for (int i = 0; i < Reader.FieldCount; i++)
                        {
                            if (counter == 1)
                            { retrievedReview.ReviewID = (int)Reader.GetValue(i); }
                            else if (counter == 2)
                            { retrievedReview.PostID = (int)Reader.GetValue(i); }
                            else if (counter == 3)
                            { retrievedReview.RestaurantID = (int)Reader.GetValue(i); }
                            else if (counter == 4)
                            { retrievedReview.ReviewText = (string)Reader.GetValue(i); }
                            else if (counter == 5)
                            { retrievedReview.TimeStamp = (string)Reader.GetValue(i); }

                            counter++;
                        }
                    }
                }


                List<Review> list = new List<Review>();
                list.Add(retrievedReview);

                Connection.Close();

                return Ok(new { Review = list.AsEnumerable().ToList() });
            }
            catch (Exception ex)
            { }

            return null;
        }


        [HttpGet]
        public IHttpActionResult getAllReviews(int ID, int RestaurantID)
        {
            try
            {
                MySqlConnection Connection = new MySqlConnection(ConnectionString);

                Connection.Open();

                string Query = "";

                if (ID == -1)
                {
                    Query = "SELECT * FROM review WHERE PostID IN(SELECT PostID FROM post WHERE SubscriberID = " + ID + " AND ReviewPresence = 1)";
                }
                else if (RestaurantID == -1)
                {
                    Query = "SELECT * FROM review WHERE RestaurantID=" + RestaurantID;
                }

                MySqlCommand getCommand = new MySqlCommand(Query, Connection);

                List<Review> list = new List<Review>();

                Review retrievedReview = new Review();

                MySqlDataReader SelectReader = getCommand.ExecuteReader();

                while (SelectReader.Read())
                {
                    if (SelectReader.HasRows)
                    {
                        retrievedReview.ReviewID = (int)SelectReader.GetValue(0);
                        retrievedReview.PostID = (int)SelectReader.GetValue(1);
                        retrievedReview.RestaurantID = (int)SelectReader.GetValue(2);
                        retrievedReview.ReviewText = (string)SelectReader.GetValue(3);
                        retrievedReview.TimeStamp = (string)SelectReader.GetValue(4);

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
