using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class LocationRepository : ReadWriteRepositoryBase<Location, int>, ILocationRepository
    {
        public LocationRepository(IDbContext context)
            : base(context, new string[] { })
        {

        }
        public Location FindByCodeAndLocType(string code,int loctype)
        {
            return CustomQuery().Where(l => l.Pcode.Equals(code) && l.Location_Type==loctype && l.IsDeleted == false).FirstOrDefault();
        }

        public Location FindByName(string name)
        {
            return CustomQuery().Where(l => l.Name.Equals(name) && l.IsDeleted == false).FirstOrDefault();
        }
        public Location FindByNameAndParentIdAndType(string name, int loctype,int parentid)
        {
            return CustomQuery().Where(x => x.Name.Equals(name) &&x.Parent_Id==parentid
            && x.Location_Type == loctype&&x.IsDeleted==false).FirstOrDefault();
        }
        public Location FindByNameAndType(string name, int loctype)
        {
            return CustomQuery().Where(x => x.Name.Equals(name)&& x.Location_Type == loctype && x.IsDeleted == false).FirstOrDefault();
        }

        public List<Location> GetAllDistrictByStateDivisonId(int statedivid)
        {
            return CustomQuery().Where(l => l.Location_Type == (int)LocationType.District && l.Parent_Id == statedivid &&
            l.IsDeleted == false).ToList();
        }

        public List<Location> GetAllStateDivision()
        {
            return CustomQuery().Where(l => l.Location_Type == (int)LocationType.StateDivision && l.IsDeleted == false).ToList();
        }

        public List<Location> GetAllTownshipByDistrictId(int districtid)
        {
            return CustomQuery().Where(l => l.Location_Type == (int)LocationType.Township && l.Parent_Id == districtid &&
            l.IsDeleted == false).ToList();
        }

        public List<Location> GetAllTownshipByMinistryId(int ministry_id)
        {
            return CustomQuery().Where(l => l.Location_Type == (int)LocationType.Township && l.IsDeleted == false && (l.Parent_Id == ministry_id || l.ParentLocation.Parent_Id == ministry_id)).ToList();
        }
        //public int GetSmsSentCount(string fdate,string todate,int minid,int uid)
        //{}

    }
}
