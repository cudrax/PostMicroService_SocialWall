using PostMicroService_SocialWall.DataAccess;
using PostMicroService_SocialWall.Models;
using PostMicroService_SocialWall.ServiceCalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using URISUtil.DataAccess;

namespace PostMicroService_SocialWall.Controllers
{
    [RoutePrefix("api/Post")]
    public class PostController : ApiController
    {
        [Route(""), HttpGet]
        public IEnumerable<Post> GetPosts([FromUri]ActiveStatusEnum active = ActiveStatusEnum.Active)
        {
            return PostDB.GetPosts(active);
        }

        [Route("{id}"), HttpGet]
        public Post GetPost(int id)
        {
            return PostDB.GetPost(id);
        }

        [Route(""), HttpPost]
        public Post InsertPost(Post post)
        {
            return PostDB.InsertPost(post);
        }

        [Route("{id}"), HttpPut]
        public Post UpdatePost([FromBody]Post post, int id)
        {
            return PostDB.UpdatePost(post, id);
        }

        [Route("{id}"), HttpDelete]
        public void DeletePost(int id)
        {
            PostDB.DeletePost(id);
        }

        [Route("api/User/{id}"), HttpGet]
        public User GetUser(int id)
        {
            return UserService.GetUser(id);
        }

    }
}