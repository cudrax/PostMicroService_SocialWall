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
        public void GetPostsSuccess()
        {
            List<Post> posts = PostDB.GetPosts(active);
            Assert.AreEqual(2, posts.Count);
            

        }

        [Test]
        public void GetPostSuccess()
        {
            int id = PostDB.GetPosts(active)[0].Id;
            Post post = PostDB.GetPost(id);
            Assert.IsNotNull(post);
        }

        [Test]
        public void GetPostFailed()
        {
            Post post = PostDB.GetPost(100);
            Assert.IsNull(post);
        }

        [Test]
        public void InsertPostSuccess()
        {
            Post post = new Post
            {
                Created = new DateTime(2018, 5, 1, 8, 30, 52),
                Text = "Test unosa UNIT",
                Attachment = null,
                Location = "Novi Sad, Srbija",
                Rating = 4.1m,
                Clicks = 32,
                Active = true,
                UserId = 4
            };
            int oldNumberOfPosts = PostDB.GetPosts(active).Count;
            PostDB.InsertPost(post);
            Assert.AreEqual(oldNumberOfPosts + 1, PostDB.GetPosts(active).Count);
        }

        [Test]
        public void InsertPostFailed()
        {
            Post post = new Post
            {
                Created = new DateTime(2018, 5, 1, 8, 30, 52),
                Text = "Test unosa UNIT_faild",
                Attachment = null,
                Location = "Novi Sad, Srbija",
                Rating = 4.1m,
                Clicks = 32,
                Active = true,
                UserId = 4
            };
            int oldNumberOfPosts = PostDB.GetPosts(active).Count;
            PostDB.InsertPost(post);
            Assert.AreEqual(oldNumberOfPosts, PostDB.GetPosts(active).Count);
        }

        [Test]
        public void UpdatePostSuccess()
        {
            int id = PostDB.GetPosts(active)[0].Id;
            Post post = new Post
            {
                Created = new DateTime(2018, 5, 1, 8, 30, 52),
                Text = "Test unosa UNIT_UPDATE",
                Attachment = null,
                Location = "Novi Sad, Srbija",
                Rating = 4.1m,
                Clicks = 32,
                Active = true,
                UserId = 4
            };
            Post updatedPost = PostDB.UpdatePost(post, id);
            Assert.AreEqual(updatedPost.Created, post.Created);
            Assert.AreEqual(updatedPost.Text, post.Text);
            Assert.AreEqual(updatedPost.Attachment, post.Attachment);
            Assert.AreEqual(updatedPost.Location, post.Location);
            Assert.AreEqual(updatedPost.Rating, post.Rating);
            Assert.AreEqual(updatedPost.Clicks, post.Clicks);
            Assert.AreEqual(updatedPost.Active, post.Active);
            Assert.AreEqual(updatedPost.UserId, post.UserId);
        }

        [Test]
        public void UpdatePostFailed()
        {
            int id = PostDB.GetPosts(active)[0].Id;
            Post post = new Post
            {
                Created = new DateTime(2018, 5, 1, 8, 30, 52),
                Text = "Test unosa UNIT_UPDATE_FAILED",
                Attachment = null,
                Location = "Novi Sad, Srbija",
                Rating = 4.1m,
                Clicks = 32,
                Active = true,
                UserId = 4
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
            PostDB.DeletePost(100);
            Assert.AreEqual(numberOfOldPosts, PostDB.GetPosts(active).Count);
        }

    }
}
