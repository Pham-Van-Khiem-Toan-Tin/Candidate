﻿namespace Candidate.Model
{
    public class Channel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public ICollection<EventChannels> EventChannels { get; set; }
        public ICollection<Application> Applications { get; set; }
    }
}
