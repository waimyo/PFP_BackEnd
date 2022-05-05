using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Infrastructure.Utilities;
using NY.Framework.Model.Entities;
using NY.Framework.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static NY.Framework.Web.Models.CampaignDetailViewModel;

namespace NY.Framework.Web.Mappers
{
    public class CampaignMapper
    {
        //for campaign list
        public JqueryDataTableQueryOptions<Campaigns> PrepareQueryOptionForRepository(JqueryDataTableQueryOptions<Campaigns> queryOption,CampaignListFilterViewModel filtervm)
        {
            queryOption.FilterBy = (c => c.IsDeleted == false);
            if (!string.IsNullOrEmpty(filtervm.Name))
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy,
                    c => c.Name.Contains(filtervm.Name));
            }
            if (filtervm.Status != null)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy,
                  c => c.Status==filtervm.Status);
            }
            if (!string.IsNullOrEmpty(filtervm.SmsMessage))
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy,
                    c => c.Sms_Message.Contains(filtervm.SmsMessage));
            }
            if (!string.IsNullOrEmpty(filtervm.GroupNo))
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy,
                    c => c.group.Name.Contains(filtervm.GroupNo));
            }
            //for ministry acc filter
            if (filtervm.MinistryId > 0 && filtervm.CreatedUserId==0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy,
                  c => c.CreatedByUser.ID == filtervm.MinistryId ||
                   (c.CreatedByUser.parent_id == filtervm.MinistryId)||
                  ( c.CreatedByUser.ParentUser.parent_id==filtervm.MinistryId)
                 );
            }
            //for cpu acc filter
            if (filtervm.MinistryId > 0&&filtervm.CreatedUserId > 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy,
                  c => c.CreatedByUser.ID == filtervm.CreatedUserId || c.CreatedByUser.parent_id == filtervm.CreatedUserId);
            }
            if (!string.IsNullOrEmpty(filtervm.CreatedDateFrom)&&!string.IsNullOrEmpty(filtervm.CreatedDateTo))
            {
                DateTime fromdate = DateTime.ParseExact(filtervm.CreatedDateFrom, "yyyy-MM-dd",
                               CultureInfo.InvariantCulture);
                DateTime todate = DateTime.ParseExact(filtervm.CreatedDateTo, "yyyy-MM-dd",
                               CultureInfo.InvariantCulture);
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy,
                  c => c.CreatedDate.Value.Date>=fromdate && c.CreatedDate.Value.Date<=todate);

            }
            if (queryOption.SortColumnsName.Count > 0)
            {
                foreach (string colName in queryOption.SortColumnsName)
                {
                    queryOption.SortBy = new List<Func<Campaigns, object>>();

                    if (colName == "campaignserialno")
                    {
                        queryOption.SortBy.Add(c => c.ID);
                    }
                    else if (colName == "createdbyname")
                    {
                        queryOption.SortBy.Add(c => c.CreatedByUser.name);
                    }
                    else if (colName == "campaignname")
                    {
                        queryOption.SortBy.Add(c => c.Name);
                    }
                    else if (colName == "status")
                    {
                        queryOption.SortBy.Add(c => c.Status);
                    }
                    else if (colName == "smsmessage")
                    {
                        queryOption.SortBy.Add(c => c.Sms_Message);
                    }
                    else if (colName == "closingmessage")
                    {
                        queryOption.SortBy.Add(c => c.Closing_Message);
                    }
                    else if (colName == "groupname")
                    {
                        queryOption.SortBy.Add(c => c.group.Name);
                    }
                    else if (colName == "starttimeendtime")
                    {
                        queryOption.SortBy.Add(c => c.Start_Time);
                    }
                    else
                    {
                        queryOption.SortBy.Add(c => c.ID);
                        queryOption.SortOrder = SortOrder.DESC;
                    }
                }
            }
            return queryOption;
        }

        //for campaign detail list
        public JqueryDataTableQueryOptions<CampaignDetailListStoreProcedure> PrepareQueryOptionForDetailListRepository(JqueryDataTableQueryOptions<CampaignDetailListStoreProcedure> queryOption)
        {

            if (!string.IsNullOrEmpty(queryOption.SearchValue))
            {
                string searchtext = queryOption.SearchValue;
                DateTime replydate;
                bool isDateFormat = DateTime.TryParse(searchtext, out replydate);
                //for reply date filter
                if (isDateFormat)
                {          
                    queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy,
                     c => c.ReplyDate!=null && c.ReplyDate.Value.ToString("yyyy-MM-dd").Contains(searchtext));
                }
                //for gender
                bool gender;
                bool isGender= Boolean.TryParse(searchtext, out gender);
                if (isGender)
                {
                    queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy,
                    c => c.Gender==gender);
                }
                //for other fields
                else
                {
                    queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy,
                   c => (c.Name!=null&&c.Name.Contains(searchtext)) ||(c.Mobile!=null&&c.Mobile.Contains(searchtext)) || 
                            (c.Department!=null &&c.Department.Contains(searchtext)) ||
                            (c.Category!=null && c.Category.Contains(searchtext)) || 
                            (c.ReplyMessage!=null &&c.ReplyMessage.Contains(searchtext)) || c.SentMessage.Contains(searchtext));
                }               
            }
            if (queryOption.SortColumnsName.Count > 0)
            {
                foreach (string colName in queryOption.SortColumnsName)
                {
                    queryOption.SortBy = new List<Func<CampaignDetailListStoreProcedure, object>>();

                    if (colName == "id")
                    {
                        queryOption.SortBy.Add(c => c.ID);
                    }
                    else if (colName == "name")
                    {
                        queryOption.SortBy.Add(c => c.Name);
                    }
                    else if (colName == "mobile")
                    {
                        queryOption.SortBy.Add(c => c.Mobile);
                    }
                    else if (colName == "department")
                    {
                        queryOption.SortBy.Add(c => c.Department);
                    }
                    else if (colName == "gender")
                    {
                        queryOption.SortBy.Add(c => c.Gender);
                    }
                    else if (colName == "responsetype")
                    {
                        queryOption.SortBy.Add(c => c.Category);
                    }
                    else if (colName == "responsemessage")
                    {
                        queryOption.SortBy.Add(c => c.ReplyMessage);
                    }
                    else if (colName == "sentmessage")
                    {
                        queryOption.SortBy.Add(c => c.SentMessage);
                    }
                    else if (colName == "responsetime")
                    {
                        queryOption.SortBy.Add(c => c.ReplyDate);
                    }                    
                    else
                    {
                        queryOption.SortBy.Add(c => c.ID);
                    }
                }
            }
            return queryOption;
        }


        //for save or update
        public void MapEntryViewModelToModel(Campaigns campaign, CampaignEntryViewModel vm)
        {
            campaign.Name = vm.Name;
            campaign.Status = true;//open
            campaign.Sms_Message = vm.SmsMessage;
            campaign.Closing_Message = vm.ClosingMessage;
            campaign.Sms_Code_Id = vm.SmsCodeId;
            campaign.Group_Id = vm.GroupId;
            campaign.Start_Time = DateTime.Now;
        }

        //for Campaign Detail 
        public CampaignDetailViewModel MapModelToDetailViewModel(Campaigns camp)
        {
            CampaignDetailViewModel vm = new CampaignDetailViewModel();
            vm.Id = camp.ID;
            vm.Name = camp.Name;
            if (camp.Status)
            {
                vm.Status = "Open";
            }
            else
            {
                vm.Status = "Closed";
            }
            vm.SmsMessage = camp.Sms_Message;
            vm.ClosingMessage = camp.Closing_Message;
            vm.StartTime = string.Format("{0:dd-MM-yyyy HH:mm:ss}", camp.Start_Time);
            vm.EndTime = string.Format("{0:dd-MM-yyyy HH:mm:ss}",camp.End_Time);
            vm.GroupNo = camp.group.Name;
            return vm;

        }

        public JQueryDataTablePagedResult<CampaignListViewModel> MapModelToListViewModel(JQueryDataTablePagedResult<Campaigns> campaignlist)
        {
            JQueryDataTablePagedResult<CampaignListViewModel> vmlist = new JQueryDataTablePagedResult<CampaignListViewModel>();
            foreach (var camp in campaignlist.data)
            {
                CampaignListViewModel vm = new CampaignListViewModel();
                vm.Id = camp.ID;
                if (camp.CreatedByUser != null)
                {
                    vm.CampaignCreatedBy = camp.CreatedByUser.name;
                }
            
                vm.CampaignName = camp.Name;
                if (camp.Status)
                {
                    vm.CampaignStatus = "Open";
                }
                else
                {
                    vm.CampaignStatus = "Closed";
                }
                vm.SmsMessage = camp.Sms_Message;
                vm.ClosingMessage = camp.Closing_Message;
                vm.GroupName = camp.group.Name;
                if (camp.Start_Time != null && camp.End_Time==null)
                {
                    vm.StartTimeEndTime = string.Format("{0:dd-MM-yyyy HH:mm:ss}", camp.Start_Time);
                }
                else if(camp.Start_Time == null && camp.End_Time != null)
                {
                    vm.StartTimeEndTime = string.Format("{0:dd-MM-yyyy HH:mm:ss}", camp.End_Time);
                }
               else if (camp.Start_Time!=null&&camp.End_Time != null)
                {
                    vm.StartTimeEndTime = string.Format("{0:dd-MM-yyyy HH:mm:ss}", camp.Start_Time)+
                                                              "/" + string.Format("{0:dd-MM-yyyy HH:mm:ss}", camp.End_Time);
                }
                

                vmlist.data.Add(vm);
                vmlist.recordsFiltered = campaignlist.recordsFiltered;
                vmlist.recordsTotal = campaignlist.recordsTotal;
            }
            return vmlist;
        }


        public JQueryDataTablePagedResult<CampaignDetailListViewModel> MapModelToDetailListViewModel(JQueryDataTablePagedResult<CampaignDetailListStoreProcedure> campaignlist)
        {
            JQueryDataTablePagedResult<CampaignDetailListViewModel> vmlist = new JQueryDataTablePagedResult<CampaignDetailListViewModel>();
            foreach (var camp in campaignlist.data)
            {
                CampaignDetailListViewModel vm = new CampaignDetailListViewModel();
                // vm.Id = camp.ID;
                vm.Name = camp.Name;                
                if (!string.IsNullOrEmpty(camp.Mobile))
                {
                    if (camp.Mobile.Length <= 4)
                    {
                        vm.Mobile = camp.Mobile;
                    }
                    else
                    {
                        var first = camp.Mobile.Substring(0, 4);
                        var last = camp.Mobile.Last().ToString();
                        var star = "";
                        for (int i = 0; i < camp.Mobile.Length - 5; i++)
                        {
                            star += "*";
                        }
                        vm.Mobile = first + star + last;
                    }
                    
                }
                vm.DepartmentName = camp.Department;
                if (camp.Gender)
                {
                    vm.Gender = "ကျား";
                }
                else
                {
                    vm.Gender = "မ";
                }

                vm.SmsMessage = camp.SentMessage;
                vm.ResponseMessage = camp.ReplyMessage;
                vm.ResponseMessageTime= string.Format("{0:dd-MM-yyyy HH:mm:ss}", camp.ReplyDate);
                vm.CategorizedResponse = camp.Category;

                vmlist.data.Add(vm);
                vmlist.recordsFiltered = campaignlist.recordsFiltered;
                vmlist.recordsTotal = campaignlist.recordsTotal;
            }
            return vmlist;
        }

    }
}
