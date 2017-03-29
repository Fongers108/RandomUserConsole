using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandomUserConsole.Models
{
    public class RandomUser
    {
        [Key]
        public int RandomUserId { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }
        [MaxLength(14)]
        public string Phone { get; set; }   // for this example assuming 14 digits
        [MaxLength(14)]
        public string Cell { get; set; }    // for this example assuming 14 digits
        [MaxLength(255)]
        public string FullName { get; set; }

        [NotMapped]
        public Name name { get; set; }
    }

    // Used to parse JSON results
    public class Name
    {
        public string title { get; set; }
        public string first { get; set; }
        public string last { get; set; }
    }

    // JSON Result set
    public class RootObject
    {
        public List<RandomUser> results { get; set; }
    }
}
