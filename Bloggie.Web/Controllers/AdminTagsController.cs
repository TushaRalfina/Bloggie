using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
 

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;
 
        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
         }

        //--------------------------------------------------ADD TAG CONTROLLER---------------------------------------------------------------------

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> SubmitTag(AddTagRequest addTagRequest)
        {

            //mapping ADDTagRequest to Tag DOMAIN model
           var tag=new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await tagRepository.AddAsync(tag);

            //redirect to the tag list
            return RedirectToAction("List");

        }

        //---------------------------------------------------------------LIST--------------------------------------------------------------------------------------------
         

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var tags = await  tagRepository.GetAllAsync();
            return View(tags);
        }
     
         
        //-------------------------------EDIT TAG CONTROLLER-------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await tagRepository.GetAsync(id);
            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);

            }
            return View(null);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {

            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

           var updatedTag= await tagRepository.UpdateAsync(tag);

            if (updatedTag != null)
            {
                //show success message
               // return RedirectToAction("List");
            }
            else
            {
                //show error message
            }

            return RedirectToAction("Edit", new { id = editTagRequest.Id });

        }
         //--------------------------------------------------delete tag controller---------------------------------------------------------------------

        [HttpGet]
        public async  Task<IActionResult> Delete(Guid id) {
           var deletedTag= await tagRepository.DeleteAsync(id);
           
            return RedirectToAction("List");
         
        }
 










    }
}
