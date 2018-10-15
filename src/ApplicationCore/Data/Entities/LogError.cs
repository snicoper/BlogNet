using System;

namespace ApplicationCore.Data.Entities
{
    public class LogError
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Username { get; set; }
        public string Path { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public bool Checked { get; set; }
    }
}
