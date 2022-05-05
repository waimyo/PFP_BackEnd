using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class LocationViewModel
    {
        public int Id { get; set; }
        public int LocationType { get; set; }
        public int StateDivisionId { get; set; }
        public string StateDivisionName { get; set; }
        public string StateDivisionCode { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string DistrictCode{ get; set; }
        public int TownshipId { get; set; }
        public string TownshipName { get; set; }
        public string TownshipCode { get; set; }
    }
}
