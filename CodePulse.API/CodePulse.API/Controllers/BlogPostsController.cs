using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Implementation;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository,
            ICategoryRepository categoryRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRquestDto request)
        {
            var blogPost = new BlogPost
            {
                Author = request.Author,
                Title = request.Title,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                ShortDescription = request.ShortDescription,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetByIdAsync(categoryGuid);
                if(existingCategory is not null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            blogPost = await blogPostRepository.CreateAsync(blogPost);

            var reponse = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Title = blogPost.Title,
                Categories = blogPost.Categories.Select(x => new CategoryDto { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList(),
            };

            return Ok(reponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();

            var response = new List<BlogPostDto>();
            foreach(var blogPost in blogPosts)
            {
                response.Add(new BlogPostDto
                {
                    Id = blogPost.Id,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    IsVisible = blogPost.IsVisible,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Title = blogPost.Title,
                    Categories = blogPost.Categories.Select(x => new CategoryDto { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList(),
                });
            }

            return Ok(response);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var blogPost = await blogPostRepository.GetByIdAsync(id);

            if(blogPost == null)
            {
                return NotFound();
            }
            var reponse = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Title = blogPost.Title,
                Categories = blogPost.Categories.Select(x => new CategoryDto { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList(),
            };

            return Ok(reponse);

        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDto request)
        {
            var blogPost = new BlogPost
            {
                Id = id,
                Author = request.Author,
                Title = request.Title,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                ShortDescription = request.ShortDescription,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetByIdAsync(categoryGuid);
                if (existingCategory is not null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            var result = await blogPostRepository.UpdateAsync(blogPost);
            if(result == null)
            {
                return NotFound();
            }
            var reponse = new BlogPostDto
            {
                Id = result.Id,
                Author = result.Author,
                Content = result.Content,
                FeaturedImageUrl = result.FeaturedImageUrl,
                IsVisible = result.IsVisible,
                ShortDescription = result.ShortDescription,
                UrlHandle = result.UrlHandle,
                PublishedDate = result.PublishedDate,
                Title = result.Title,
                Categories = result.Categories.Select(x => new CategoryDto { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList(),
            };
            return Ok(reponse);

        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var blogPost = await blogPostRepository.DeleteAsync(id);

            if (blogPost == null)
            {
                return NotFound();
            }
            var reponse = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Title = blogPost.Title,
            };

            return Ok(reponse);

        }
    }
}
