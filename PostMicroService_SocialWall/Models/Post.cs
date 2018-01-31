using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostMicroService_SocialWall.Models
{
    public class Post
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Text { get; set; }
        public byte[] Attachment { get; set; }
        public string Location { get; set; }
        public decimal Rating { get; set; }
        public int Clicks { get; set; }

        public bool Active { get; set; }

        public int UserId { get; set; }
    }
}