using System.ComponentModel.DataAnnotations;

namespace ActiveDirectory.Models
{
    public class User
    {
        public int Id { get; set; }
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        public string Samaccountname { get; set; }
    }
}
