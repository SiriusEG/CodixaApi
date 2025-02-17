using System.ComponentModel.DataAnnotations;

namespace Codixa.Core.Models.CourseModels
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

    }
}
