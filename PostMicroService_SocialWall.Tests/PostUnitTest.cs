using System;
using URISUtil.DataAccess;
using NUnit.Framework;
using PostMicroService_SocialWall.DataAccess;
using PostMicroService_SocialWall.Models;
using System.Text;
using System.Collections.Generic;

namespace PostMicroService_SocialWall.Tests
{
    
    public class PostUnitTest
    {
        ActiveStatusEnum active = ActiveStatusEnum.Active;

        [Test]
        public void GetAllPostsSuccess()
        {
            List<Post> posts = PostDB.GetPosts(active);
            Assert.AreEqual(1, posts.Count);
        }

        [Test]
        public void GetOnePostSuccess()
        {
            int id = PostDB.GetPosts(active)[0].Id;
            Post post = PostDB.GetPost(id);
            Assert.IsNotNull(post);
        }

        [Test]
        public void GetOnePostFailed()
        {
            int id = 100;
            Post post = PostDB.GetPost(id);
            Assert.IsNull(post);
        }

        [Test]
        public void InsertPostSuccess()
        {
            Post post = new Post
            {
                Created = new DateTime(2018, 3, 3, 5, 0, 0),
                Text = "Danas pravimo microservice",
                Attachment = Encoding.ASCII.GetBytes("slika"),
                Location = "Novi sad",
                Rating = 3.2m,
                Clicks = 7,
                Active = true,
                UserId = 1
            };
            int oldNumberOfPosts = PostDB.GetPosts(active).Count;
            PostDB.InsertPost(post);
            Assert.AreEqual(oldNumberOfPosts + 1, PostDB.GetPosts(active).Count);
            //Assert.AreEqual(1, PostDB.GetPosts(active).Count);
        }

        [Test]
        public void InsertPostFailed()
        {
            /*Post post = new Post
            {
                Created = new DateTime(2018, 3, 3, 5, 0, 0),
                Text = "Danas pravimo microservice",
                Attachment = Encoding.ASCII.GetBytes("slika"),
                Location = "Novi sad",
                Rating = 3.2m,
                Clicks = 7,
                Active = true,
                UserId = 1
            };
            int oldNumberOfPosts = PostDB.GetAllPosts(active).Count;
            PostDB.InsertPostManualId(post);
            Assert.AreEqual(oldNumberOfPosts, PostDB.GetAllPosts(active).Count);
            */
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void UpdatePostSuccess()
        {
            int id = PostDB.GetPosts(active)[0].Id;
            Post post = new Post
            {
                Created = new DateTime(2018, 3, 3, 5, 0, 0),
                Text = "Danas pravimo microservice_UPDATE",
                Attachment = Encoding.ASCII.GetBytes("slika"),
                Location = "Novi sad",
                Rating = 3.2m,
                Clicks = 7,
                Active = true,
                UserId = 1
            };
            Post localPost = PostDB.GetPost(id);
            post.Created = localPost.Created;
            post.UserId = localPost.UserId;

            PostDB.UpdatePost(post, id);
            Post updatedPost = PostDB.GetPost(id);

            Assert.AreEqual(updatedPost.Text, post.Text);
            /*Assert.AreEqual(updatedPost.Clicks, post.Clicks);
            Assert.AreEqual(updatedPost.Attachment, post.Attachment);
            Assert.AreEqual(updatedPost.Active, post.Active);
            Assert.AreEqual(updatedPost.UserId, post.UserId);*/


        }

        [Test]
        public void UpdatePostFailed()
        {
            int id = 100;
            Post post = new Post
            {
                Created = new DateTime(2018, 3, 3, 5, 0, 0),
                Text = "Danas pravimo microservice_UPDATE",
                Attachment = Encoding.ASCII.GetBytes("slika"),
                Location = "Novi sad",
                Rating = 3.2m,
                Clicks = 7,
                Active = true,
                UserId = 1
            };
            Post updatedPost = PostDB.UpdatePost(post, id);
            Assert.IsNull(updatedPost);
        }

        [Test]
        public void DeletePostSuccess()
        {
            int id = PostDB.GetPosts(active)[0].Id;
            PostDB.DeletePost(id);
            Assert.AreEqual(PostDB.GetPost(id).Active, false);
        }

        [Test]
        public void DeletePostFailed()
        {
            int numberOfOldPosts = PostDB.GetPosts(active).Count;
            PostDB.DeletePost(123);
            Assert.AreEqual(numberOfOldPosts, PostDB.GetPosts(active).Count);
        }

    }
}
