using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class DataRepository:ReadWriteRepositoryBase<Data,int>, IDataRepository
    {
        public DataRepository(IDbContext context):base(context,new string[] { }) { }

       

        public List<Data> GetCreatedDate(int minid)
        {
            return CustomQuery().Where(d => d.ministry_id.Equals(minid) && d.CreatedDate != null && d.IsDeleted.Equals(false)).ToList();
        }

        //public Data GetDataList(int id)
        //{
        //    return CustomQuery().Where(d => d.CreatedBy.Equals(id)).FirstOrDefault();
        //}

        public List<Data> GetDataList(int uid)
        {
            return CustomQuery().Where(d => d.CreatedBy.Equals(uid) || d.CreatedByUser.parent_id.Equals(uid) || d.CreatedByUser.ParentUser.parent_id.Equals(uid) && d.IsDeleted.Equals(false)).ToList();
        }

        

        //public List<Data> FindByDataList(int minid,int deptid, string dept, int serviceid, string service, string fromdate, string todate, int uploadedby, string uploadedbyname)
        //{
        //    DateTime fromdt = DateTime.ParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        //    DateTime todt = DateTime.ParseExact(todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        //    if (deptid > 0 && serviceid > 0 && uploadedby > 0)
        //    {
        //        return CustomQuery().Where(d => d.ministry_id.Equals(minid) && d.department_id.Equals(deptid) && d.service_id.Equals(serviceid) && d.CreatedDate >= fromdt && d.CreatedDate <= todt && d.CreatedBy.Equals(uploadedby) && d.IsDeleted.Equals(false)).ToList();
        //    }
        //    else if (deptid > 0 && serviceid > 0 && uploadedbyname == "Uploaded by All")
        //    {
        //        return CustomQuery().Where(d => d.ministry_id.Equals(minid) && d.department_id.Equals(deptid) && d.service_id.Equals(serviceid) && d.CreatedDate >= fromdt && d.CreatedDate <= todt && d.IsDeleted.Equals(false)).ToList();
        //    }
        //    else if (deptid > 0 && service == "All Services" && uploadedby > 0)
        //    {
        //        return CustomQuery().Where(d => d.ministry_id.Equals(minid) && d.department_id.Equals(deptid) && d.CreatedDate >= fromdt && d.CreatedDate <= todt && d.CreatedBy.Equals(uploadedby) && d.IsDeleted.Equals(false)).ToList();
        //    }
        //    else if (dept == "All Departments" && serviceid > 0 && uploadedby > 0)
        //    {
        //        return CustomQuery().Where(d => d.ministry_id.Equals(minid) && d.service_id.Equals(serviceid) && d.CreatedDate >= fromdt && d.CreatedDate <= todt && d.CreatedBy.Equals(uploadedby) && d.IsDeleted.Equals(false)).ToList();
        //    }
        //    else if (dept == "All Departments" && service == "All Services" && uploadedby > 0)
        //    {
        //        return CustomQuery().Where(d => d.ministry_id.Equals(minid) && d.CreatedDate >= fromdt && d.CreatedDate <= todt && d.CreatedBy.Equals(uploadedby) && d.IsDeleted.Equals(false)).ToList();
        //    }
        //    else if (dept == "All Departments" && serviceid > 0 && uploadedbyname == "Uploaded by All")
        //    {
        //        return CustomQuery().Where(d => d.ministry_id.Equals(minid) && d.service_id.Equals(serviceid) && d.CreatedDate >= fromdt && d.CreatedDate <= todt && d.IsDeleted.Equals(false)).ToList();
        //    }
        //    else if (deptid > 0 && service == "All Services" && uploadedbyname == "Uploaded by All")
        //    {
        //        return CustomQuery().Where(d => d.ministry_id.Equals(minid) && d.department_id.Equals(deptid) && d.CreatedDate >= fromdt && d.CreatedDate <= todt && d.IsDeleted.Equals(false)).ToList();
        //    }
        //    else
        //    {
        //        return CustomQuery().Where(d => d.ministry_id.Equals(minid) && d.CreatedDate >= fromdt && d.CreatedDate <= todt && d.IsDeleted.Equals(false)).ToList();
        //    }

        //}

        //for sms inbound
        public Data GetDataInfoByMobileNo(string mobile)
        {
            return CustomQuery().Where(d=>d.mobile.Equals(mobile) && d.IsDeleted==false).OrderByDescending(d=>d.ID).FirstOrDefault();
        }
    }
}
