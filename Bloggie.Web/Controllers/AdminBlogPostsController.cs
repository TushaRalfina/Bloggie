using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;


namespace Bloggie.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository,IBlogPostRepository blogPostRepository)

        {
                this.tagRepository = tagRepository;
                this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async  Task<IActionResult> Add()
        {
            //get tags from repository

            var tags = await tagRepository.GetAllAsync();
            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            // Map view model to domain model
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,
            };

            // Map Tags from selected tags
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }

            // Mapping tags back to domain model
            blogPost.Tags = selectedTags;
            await blogPostRepository.AddAsync(blogPost);

            return RedirectToAction("Add");
        }







        //-------------------------------------------show all blog posts--------------------------------------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();

            return View(blogPosts);
        }


        //-------------------------------------------EDIT BLOG POST--------------------------------------------------------------------------------------------


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //get blogpost from repository
            var blogPost = await blogPostRepository.GetAsync(id);
            var tags = await tagRepository.GetAllAsync();

            if (blogPost == null)
            {
                return NotFound();
            }
            else
            {
                //map the domain model to view model
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    Visible = blogPost.Visible,
                    Tags = tags.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {

            // Map view model to domain model
            var blogPost = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Author = editBlogPostRequest.Author,
                Visible = editBlogPostRequest.Visible
            };

            // Map Tags from selected tags into domain model
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in editBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }

            blogPost.Tags = selectedTags;

            // Update blog post
            var updatedBlog= await blogPostRepository.UpdateAsync(blogPost);

            if (updatedBlog == null)
            {
                return NotFound();
            }

            // Redirect to list
            return RedirectToAction("List");
        }

        //-------------------------------------------DELETE BLOG POST--------------------------------------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
             var deletedBlogPost = await blogPostRepository.DeleteAsync(id);
            if (deletedBlogPost == null)
                {
                return NotFound();
            }
            return RedirectToAction("List");
        }
    
    
    
    
    
    
    
    
    
    
    
    
    
    }
}
