using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BloggieDbContext bloggieDbContext;

        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult SubmitTag(AddTagRequest addTagRequest)
        {
             
            //mapping ADDTagRequest to Tag DOMAIN model
            bloggieDbContext.Tags.Add(new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            });

            bloggieDbContext.SaveChanges();

            //redirect to the same page
            return RedirectToAction("Add");
             
        }
    }

}
