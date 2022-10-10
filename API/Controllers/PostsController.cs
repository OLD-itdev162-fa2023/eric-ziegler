using Microsoft.AspNetCore.Mvc;
using Domain;
using Persistence;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly DataContext context;

        public PostsController(DataContext context)
        {
            this.context = context;
        }

        /// <summary>
        ///  GET api/posts
        /// </summary>
        /// <returns>A list of all posts</returns> 
        [HttpGet(Name = "GetPosts")]
        public ActionResult<List<Post>> Get()
        {
            return context.Posts.ToList();
        }

        /// <summary>
        /// GET api/posts/[id]
        /// </summary>
        /// <param name="id">Post id</param>
        /// <returns>A single post</returns>
        [HttpGet("{id}", Name = "GetById")]
        public ActionResult<Post> GetById(Guid id)
        {
            var post = this.context.Posts.Find(id);
            if (post is null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        /// <summary>
        /// POST api/posts
        /// </summary>
        /// <param name="request">JSON request containing post fields</param>
        /// <returns>A new post</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost(Name = "Create")]        
        public ActionResult<Post> Create([FromBody]Post request)
        {
            var post = new Post
            {
                ID = request.ID,
                Title = request.Title,
                Body = request.Body,
                Date = request.Date
            };

            context.Posts.Add(post);
            var success = context.SaveChanges() > 0;

            if(success)
            {
                return Ok(post);
            }

            throw new Exception("Error creating post");
        }

        /// <summary>
        /// PUT api/posts
        /// </summary>
        /// <param name="request">JSOn request containing one or more updated post fields</param>
        /// <returns></returns>
        [HttpPut(Name = "Update")]
        public ActionResult<Post> Update([FromBody]Post request)
        {
            var post = context.Posts.Find(request.ID);
            if (post == null)
                throw new Exception("Could not find post");

            // update post properties with requests
            post.Title = request.Title != null ? request.Title : post.Title;
            post.Body = request.Body != null ? request.Body : post.Body;
            post.Date = request.Date != null ? request.Date : post.Date;

            var success = context.SaveChanges() > 0;

            if(success)
                return Ok(post);

            throw new Exception("Error updating post");
        }
    }
}