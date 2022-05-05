using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Infrastructure.Utilities;
using NY.Framework.Model.Entities;
using NY.Framework.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Mappers
{
    public class SmsMapper
    {
        public JqueryDataTableQueryOptions<Sms> Preparetorepository(JqueryDataTableQueryOptions<Sms> queryoption)
        {
            if (!string.IsNullOrEmpty(queryoption.SearchValue))
            {
                queryoption.FilterBy = LinqExpressionHelper.AppendAnd(queryoption.FilterBy, (s => s.Sms_Text.Contains(queryoption.SearchValue)||s.ID.ToString().Contains(queryoption.SearchValue)||s.Campaign_Id.ToString().Contains(queryoption.SearchValue)||s.Sms_Time.Equals(queryoption.SearchValue)||s.DataInfo.mobile.Contains(queryoption.SearchValue) ||s.Category.name.Equals(queryoption.SearchValue) || s.Campaign.Name.Contains(queryoption.SearchValue)));

            }
            if (queryoption.SortColumnsName.Count > 0)
            {
                foreach (var colName in queryoption.SortColumnsName)
                {
                    queryoption.SortBy = new List<Func<Sms, object>>();
                    if (colName == "sms_code")
                    {
                        queryoption.SortBy.Add(c => c.ID);
                    }
                    if (colName == "campaign_id")
                    {
                        queryoption.SortBy.Add(c => c.Campaign.ID);
                    }
                    if (colName == "phono")
                    {
                        queryoption.SortBy.Add(c => c.DataInfo.mobile);
                    }  
                    if (colName == "sms_text")
                    {
                        queryoption.SortBy.Add(c => c.Sms_Text);
                    }
                    if (colName == "categoryname")
                    {
                        queryoption.SortBy.Add(c => c.Category.name);
                    }
                    if (colName == "name")
                    {
                        queryoption.SortBy.Add(c => c.CreatedByUser.name);
                    }
                    if (colName == "sms_time")
                    {
                        queryoption.SortBy.Add(c => c.Sms_Time);
                    }
                    else
                    {
                        queryoption.SortOrder = SortOrder.DESC;
                        queryoption.SortBy.Add((x => x.ID));
                    }
                }

            }
            return queryoption;
        }
       

        //for sms save or update
        //added by soelae//
        public Sms MapEntryViewModelToModel(Sms sms,SmsEntryViewModel vm)
        {
            sms.Direction = vm.Direction;
            sms.Sms_Code_Id = vm.SmsCodeId;
            if (vm.DataInfoId != 0)
            {
                sms.DataInfo_Id = vm.DataInfoId;
            }          
            sms.Sms_Time = vm.SmsTime;
            sms.Sms_Text = vm.SmsText;
            if (vm.CampaignId != 0)
            {
                sms.Campaign_Id = vm.CampaignId;
            }          
            sms.Message_Type = vm.MessageType;
            sms.Operator = vm.Operator;
            sms.Reason = vm.Reason;
            return sms;
        }
        public JQueryDataTablePagedResult<SmsViewModel> MapModelToListViewModel(JQueryDataTablePagedResult<Sms> sms,int category_id)
        {
            JQueryDataTablePagedResult<SmsViewModel> vmlst = new JQueryDataTablePagedResult<SmsViewModel>();
            foreach (var res in sms.data)
            {
               
                SmsViewModel vm = new SmsViewModel();
                vm.id = res.ID;
                vm.sms_code = res.ID;
                vm.datainfo_id =Convert.ToInt32(res.DataInfo_Id);
                vm.direction = res.Direction;
                vm.message_type = res.Message_Type;
                vm.sms_text = res.Sms_Text;
                if (res.DataInfo!=null)
                {
                    var first = res.DataInfo.mobile.Substring(0, 4);
                    var last = res.DataInfo.mobile.Last().ToString();
                    var star = "";
                    for (int i = 0; i < res.DataInfo.mobile.Length - 5; i++)
                    {
                        star += "*";
                    }
                    vm.phono = first + star + last;
                }                
                vm.sms_time = string.Format("{0:dd-MM-yyyy  HH:mm:ss}", res.Sms_Time);
                vm.campaign_id =Convert.ToInt32(res.Campaign_Id);
                if (res.Campaign != null)
                {
                    vm.campaign = res.Campaign.Name;
                }                
                vm.category_id = category_id;
                vmlst.data.Add(vm);                             
            }
            vmlst.recordsFiltered = sms.recordsFiltered;
            vmlst.recordsTotal = sms.recordsTotal;
            return vmlst;

        }
        public JQueryDataTablePagedResult<SmsViewModel> MapModelToListViewModelForCategorized(JQueryDataTablePagedResult<Sms> sms)
        {
            JQueryDataTablePagedResult<SmsViewModel> vmlst = new JQueryDataTablePagedResult<SmsViewModel>();
            foreach (var res in sms.data)
            {

                SmsViewModel vm = new SmsViewModel();
                vm.id = res.ID;
                vm.sms_code = res.ID;
                vm.datainfo_id = Convert.ToInt32(res.DataInfo_Id);
                vm.direction = res.Direction;
                vm.message_type = res.Message_Type;
                vm.sms_text = res.Sms_Text;
                if (res.DataInfo != null)
                {
                    var first = res.DataInfo.mobile.Substring(0, 4);
                    var last = res.DataInfo.mobile.Last().ToString();
                    var star = "";
                    for (int i = 0; i < res.DataInfo.mobile.Length - 5; i++)
                    {
                        star += "*";
                    }
                    vm.phono = first + star + last;
                }
                vm.sms_time = string.Format("{0:dd-MM-yyyy  HH:mm:ss}", res.Sms_Time);
                vm.campaign_id = Convert.ToInt32(res.Campaign_Id);
                if (res.Campaign != null)
                {
                    vm.campaign = res.Campaign.Name;
                }                
                //vm.category_id = res.Category_Id;
                vm.categoryname = res.Category.name;
                //vm.categorized_by = res.Categorized_by;
                vm.name = res.CreatedByUser.name;
                vmlst.data.Add(vm);

            }
            vmlst.recordsFiltered = sms.recordsFiltered;
            vmlst.recordsTotal = sms.recordsTotal;
            return vmlst;

        }
    }
}
