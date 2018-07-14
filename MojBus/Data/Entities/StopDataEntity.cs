﻿using System;

namespace MojBus.Data.Entities
{
    public class StopDataEntity
    {
        public Int64 Id { get; set; }
        public string TripShortName { get; set; }
        public string TripHeadsign { get; set; }
        public int DirectionID { get; set; }
        public int RouteId { get; set; }
        public string HTMLColor { get; set; }
        public string DepartureTime { get; set; }
    }
}
