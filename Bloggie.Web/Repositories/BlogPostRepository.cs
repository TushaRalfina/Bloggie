﻿using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {

        private readonly BloggieDbContext bloggieDbContext;
        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;   
        }



        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await bloggieDbContext.AddAsync(blogPost);
            await bloggieDbContext.SaveChangesAsync();
            return blogPost;

         }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await bloggieDbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if(existingBlogPost!=null)
            {
                bloggieDbContext.BlogPosts.Remove(existingBlogPost);
                await bloggieDbContext.SaveChangesAsync();
                return existingBlogPost;
            }
            return null;

             
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
           return await bloggieDbContext.BlogPosts.Include(t=>t.Tags).ToListAsync();
             

        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
         return await bloggieDbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x => x.Id == id);

         }


        
        public async Task<BlogPost?> GetByUrlHnadleAsync(string urlHandle)
        {
           return await bloggieDbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
         }





        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
             var existingBlogPost = await bloggieDbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if(existingBlogPost!=null)
                {
                existingBlogPost.Heading = blogPost.Heading;
                existingBlogPost.PageTitle = blogPost.PageTitle;
                existingBlogPost.Content = blogPost.Content;
                existingBlogPost.ShortDescription = blogPost.ShortDescription;
                existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlogPost.UrlHandle = blogPost.UrlHandle;
                existingBlogPost.PublishedDate = blogPost.PublishedDate;
                existingBlogPost.Author = blogPost.Author;
                existingBlogPost.Visible = blogPost.Visible;
                existingBlogPost.Tags = blogPost.Tags;
                await bloggieDbContext.SaveChangesAsync();
                return existingBlogPost;
            }
            return null;


         }
    }
}
