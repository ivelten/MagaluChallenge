using System;

namespace Magalu.Challenge.Infrastructure.Logging
{
    public class RequestResponseLog
    {
        public long Id { get; set; }

        public string RemoteAddress { get; set; }

        public string RequestUrl { get; set; }

        public string RequestBody { get; set; }

        public string ResponseBody { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
