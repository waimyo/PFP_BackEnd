using NY.Framework.Infrastructure.Commands;
using  NY.Framework.Infrastructure;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Rules.LocationRules
{
    public class LocationNameMustBeUnique : IBusinessRule
    {
        ILocationRepository locationRepo;
        Location location;

        public LocationNameMustBeUnique(ILocationRepository _locationRepo,
            Location _location)
        {
            locationRepo = _locationRepo;
            location = _location;
        }

        public string[] GetRules()
        {
            string messageText = Constants.LocationNameMustBeUnique;
            return new string[] { messageText};
        }

        public bool IsSatisfied()
        {
            Location loc=locationRepo.FindByNameAndType(location.Name,location.Location_Type);
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
