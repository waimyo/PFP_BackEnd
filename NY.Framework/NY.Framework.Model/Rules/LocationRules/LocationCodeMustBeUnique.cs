using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Rules.LocationRules
{
    public class LocationCodeMustBeUnique : IBusinessRule
    {
        ILocationRepository locationRepo;
        Location location;

        public LocationCodeMustBeUnique
            (ILocationRepository _locationRepository,
            Location _location)
        {
            locationRepo = _locationRepository;
            location = _location;
        }

        public string[] GetRules()
        {
            string messageText = Constants.LocationCodeMustBeUnique;
            return new string[] { messageText};
        }

        public bool IsSatisfied()
        {
            Location loc=locationRepo.FindByCodeAndLocType(location.Pcode,location.Location_Type);
            if (loc != null)
            {
                if (loc.ID != location.ID)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
