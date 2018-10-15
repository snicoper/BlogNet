using System;

namespace ApplicationCore.Data.Entities
{
    public class Contact
    {
        public int Id { get; set; }
        public string EmailFrom { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool HasRead { get; set; }
        public DateTime SendAt { get; set; } = DateTime.Now;
    }
}
