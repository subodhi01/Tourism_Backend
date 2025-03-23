using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourismGalle.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }

        public string Comment { get; set; }

        public string Type { get; set; }

        public int ItemId { get; set; }

        public int Rating { get; set; }

        public DateTime Date { get; set; }
    }
}