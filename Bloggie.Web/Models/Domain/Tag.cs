namespace Bloggie.Web.Models.Domain
{
    public class Tag
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }


        //many to many relationship nje post mund te kete shume tagje dhe nje tag mund te jete ne shume postime

        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
