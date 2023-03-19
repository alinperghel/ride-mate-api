﻿using System;
namespace RideMateApi
{
	public class Ride
	{
        public int Id { get; set; }
        public int DriverId { get; set; }
        public int SourceId { get; set; }
        public int DestinationId { get; set; }
        public DateTime Date { get; set; }
        public int AvailableSeats { get; set; }
    }
}

