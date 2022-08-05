using System;

namespace PrismApp.Models
{
    public class User
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Title { get; set; }
        public int? Id { get; set; }
        public int? SupervisorId { get; set; }
    }
}
