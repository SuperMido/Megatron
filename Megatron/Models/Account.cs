using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Megatron.Models
{
    public class Account
    {

        public int Id { get; set; }
        [Required, DisplayName("First name")]
        public string Firstname { get; set; }
        [Required, DisplayName("Last name")]
        public string Lastname { get; set; }
        [Required, DisplayName("Falcuty")]
        public string Falcuty { get; set; }
        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

    }
}
