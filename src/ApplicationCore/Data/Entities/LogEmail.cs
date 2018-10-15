using System;

namespace ApplicationCore.Data.Entities
{
    public class LogEmail
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime SendAt { get; set; }
    }
}
