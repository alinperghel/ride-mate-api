using System;
namespace RideMateApi
{
    public class RideSearchInput
    {
        public int SourceId { get; set; }
        public int DestinationId { get; set; }
        public DateTime Date { get; set; }
    }
}

