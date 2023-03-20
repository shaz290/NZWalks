﻿namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequest
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public double Area { get; set; }

        public double Lat { get; set; }
        public double Long { get; set; }

        public double Populations { get; set; }
    }
}
