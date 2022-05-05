using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Repositories
{
    public interface IDataRepository: IReadWriteRepository<Data,int>
    {
        List<Data> GetCreatedDate(int minid);
        //Data GetDataList(int id);
        List<Data> GetDataList(int uid);
        
        //List<Data> FindByDataList(int minid,int deptid, string dept, int serviceid, string service, string fromdate, string todate, int uploadedby, string uploadedbyname);

        //for sms inbound
        Data GetDataInfoByMobileNo(string mobile);
    }
}
