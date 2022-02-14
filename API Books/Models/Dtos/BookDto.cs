using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Books.Models.Dtos
{
    public class BookDto
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "The field Title needs to be filled")]
        public string Title { get; set; }
        [Required(ErrorMessage = "The field Description needs to be filled")]
        public string Description { get; set; }
        [Required(ErrorMessage = "The field Author needs to be filled")]
        public string Author { get; set; }
        [Required(ErrorMessage = "The field Price needs to be filled")]
        [Range(0, 9999999, ErrorMessage = "The Price range value is $0 - $99999999")]
        public double Price { get; set; }
        public DateTime ReleasedDate { get; set; }


    }
}
