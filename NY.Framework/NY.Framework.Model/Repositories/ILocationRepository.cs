using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Repositories
{
    public interface ILocationRepository : IReadWriteRepository<Location,int>
    {
        Location FindByName(string name);
        Location FindByCodeAndLocType(string code,int loctype);
        Location FindByNameAndType(string name, int loctype);
        Location FindByNameAndParentIdAndType(string name, int loctype,int parentid);
        List<Location> GetAllStateDivision();
        List<Location> GetAllDistrictByStateDivisonId(int statedivid);
        List<Location> GetAllTownshipByDistrictId(int districtid);
        List<Location> GetAllTownshipByMinistryId(int ministry_id);
        //  int  GetSmsSentCount(string fdate,string todate,int minid,int uid)
    }
}
