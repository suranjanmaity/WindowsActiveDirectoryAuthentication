using System.ComponentModel.DataAnnotations;

namespace ActiveDirectory.Models
{
    public class Group
    {
        public int Id { get; set; }
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }
    }
}
