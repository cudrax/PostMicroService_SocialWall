﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using PostMicroService_SocialWall.Models;
using System.Net;
using URISUtil.DataAccess;
using URISUtil.Logging;
using URISUtil.Response;

namespace PostMicroService_SocialWall.DataAccess
{
    public class PostDB
    {
        private static Post ReadRow(SqlDataReader reader)
        {
            Post retVal = new Post();

            retVal.Id = (int)reader["Id"];
            retVal.Created = (DateTime)reader["Created"];
            retVal.Text = (string)reader["Text"];
            retVal.Attachment = (byte[])reader["Attachment"];
            retVal.Location = (string)reader["Location"];
            retVal.Rating = (decimal)reader["Rating"];
            retVal.Clicks = (int)reader["Clicks"];

            retVal.Active = (bool)reader["Active"];
            retVal.UserId = (int)reader["UserId"];


            return retVal;
        }

        private static string AllColumnSelect
        {
            get
            {
                return @"
                    [Post].[Id],
	                [Post].[Created],
	                [Post].[Text],
                    [Post].[Attachment],
                    [Post].[Location],
                    [Post].[Rating],
                    [Post].[Clicks],
                    [Post].[Active],
                    [Post].[UserId]
                ";
            }
        }

        private static void FillData(SqlCommand command, Post post)
        {
            //command.AddParameter("@Id", SqlDbType.Int, post.Id);
            command.AddParameter("@Created", SqlDbType.DateTime, post.Created);
            command.AddParameter("@Text", SqlDbType.NVarChar, post.Text);
            command.AddParameter("@Attachment", SqlDbType.VarBinary, post.Attachment);
            command.AddParameter("@Location", SqlDbType.NVarChar, post.Location);
            command.AddParameter("Rating", SqlDbType.Decimal, post.Rating);
            command.AddParameter("@Clicks", SqlDbType.Int, post.Clicks);
            command.AddParameter("@Active", SqlDbType.Bit, post.Active);
            command.AddParameter("@UserId", SqlDbType.Int, post.UserId);

        }


        public static List<Post> GetPosts(ActiveStatusEnum active)
        {
            try
            {
                List<Post> retVal = new List<Post>();
                using (SqlConnection connection = new SqlConnection(DBFunctions.ConnectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = String.Format(@"
                        SELECT {0} FROM [dbo].[Post] 
                        WHERE @Active IS NULL OR [dbo].[Post].active = @Active
                        ", AllColumnSelect);

                    command.Parameters.Add("@Active", SqlDbType.Bit);
                    switch (active)
                    {
                        case ActiveStatusEnum.Active:
                            command.Parameters["@Active"].Value = true;
                            break;
                        case ActiveStatusEnum.Inactive:
                            command.Parameters["@Active"].Value = false;
                            break;
                        case ActiveStatusEnum.All:
                            command.Parameters["@Active"].Value = DBNull.Value;
                            break;
                    }
                    System.Diagnostics.Debug.WriteLine(command.CommandText);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retVal.Add(ReadRow(reader));
                        }
                    }
                }
                return retVal;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                throw ErrorResponse.ErrorMessage(HttpStatusCode.BadRequest, ex);
            }
        }

        public static Post GetPost(int Id)
        {
            try
            {
                Post retVal = new Post();

                using (SqlConnection connection = new SqlConnection(DBFunctions.ConnectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = String.Format(@"
                        SELECT {0} FROM [dbo].[Post] 
                        WHERE [id] = @Id
                    ", AllColumnSelect);
                    command.AddParameter("@Id", SqlDbType.Int, Id);
                    
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            retVal = ReadRow(reader);
                        }
                        else
                        {
                            ErrorResponse.ErrorMessage(HttpStatusCode.NotFound);
                            return null;
                        }
                    }
                }
                return retVal;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                throw ErrorResponse.ErrorMessage(HttpStatusCode.BadRequest, ex);
            }
        }

        public static Post InsertPost(Post post)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBFunctions.ConnectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = @"
                        INSERT INTO [dbo].[Post]
                        (
                            [Created],
                            [Text],
                            [Attachment],
                            [Location],
                            [Rating],
                            [Clicks],
                            [Active],
                            [UserId]       
                        )
                        VALUES
                        (
                            GETDATE(),
                            @Text,
                            @Attachment,
                            @Location,
                            @Rating,
                            @Clicks,
                            @Active,
                            @UserId
                        )";

                    FillData(command, post);
                    if (post.Attachment == null)
                        if (post.Text == null || post.Text == "")
                            return null;
                    connection.Open();
                    command.ExecuteNonQuery();

                    return post;

                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                throw ErrorResponse.ErrorMessage(System.Net.HttpStatusCode.BadRequest, ex);
            }
        }



        public static Post UpdatePost(Post post, int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBFunctions.ConnectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = String.Format(@"
                        UPDATE
                            [dbo].[Post]
                        SET
                            [Text] = @Text,
                            [Attachment] = @Attachment,
                            [Active] = @Active,
                            [Clicks] = @Clicks                
                        WHERE
                            [Id] = @Id
                    ");
                    FillData(command, post);
                    command.AddParameter("@Id", SqlDbType.Int, id);
                    if (post.Attachment == null)
                        if (post.Text == null || post.Text == "")
                            return null;
                    connection.Open();
                    command.ExecuteNonQuery();

                    return GetPost(id);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                throw ErrorResponse.ErrorMessage(HttpStatusCode.BadRequest, ex);
            }
        }

        public static void DeletePost(int Id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBFunctions.ConnectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = String.Format(@"
                        UPDATE
                            [dbo].[Post]
                        SET
                            [active] = 0
                        WHERE
                            [Id] = @Id     
                    ");
                    command.AddParameter("@Id", SqlDbType.Int, Id);
                    connection.Open();
                    command.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                throw ErrorResponse.ErrorMessage(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}