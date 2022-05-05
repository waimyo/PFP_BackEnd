using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NY.Framework.Application;
using NY.Framework.Application.Interfaces;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Web.Mappers;
using NY.Framework.Web.Models;

namespace NY.Framework.Web.Controllers.pfp
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DataUploadExcelController : BaseController
    {
        IDataService dataService;
        IServiceService serviceServ;
        IServiceRepository serviceRepo;
        ILocationService locationServ;
        ILocationRepository locationRepo;

        public DataUploadExcelController(
            IDataService _dataService,
            IServiceService _serviceServ,
            IServiceRepository _serviceRepo,
            ILocationService _locationServ,
            ILocationRepository _locationRepo)
            : base(typeof(DataUploadExcelController), ProgramCodeEnum.DATA_UPLOAD)
        {
            this.dataService = _dataService;
            this.serviceServ = _serviceServ;
            this.serviceRepo = _serviceRepo;
            this.locationServ = _locationServ;
            this.locationRepo = _locationRepo;
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult Upload([FromForm]DataViewModel entrymodel)
        {
            try
            {
                int loginid = GetLoggedInId();
                CommandResult<Data> result = new CommandResult<Data>();
                //string newPath = @"E:\ymo\ACCM PFP\PFP\";
                string newPath = NY.Framework.Infrastructure.Constants.UploadFile;
                //StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(newPath))
                    Directory.CreateDirectory(newPath);
                if (entrymodel.uploadfile.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(entrymodel.uploadfile.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, entrymodel.uploadfile.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        entrymodel.uploadfile.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension == ".xls")//This will read the Excel 97-2000 formats    
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                            sheet = hssfwb.GetSheetAt(0);
                        }
                        else //This will read 2007 Excel format    
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                            sheet = hssfwb.GetSheetAt(0);
                        }

                        //if (sheet == null)
                        //{
                        //    return false;
                        //}
                        // col(0)   name 
                        // col(1)   mobile  
                        // col(2)   gender
                        // col(3)   service
                        // col(4)   region / state    
                        // col(5)   district
                        // col(6)   township
                        // col(7)   date of application 
                        // col(8)   date of completion
                        //int colCount = sheet.GetRow(0).LastCellNum;

                        List<DataViewModel> list = new List<DataViewModel>();
                        int rowCount = sheet.LastRowNum;
                        for (int i = 1; i <= rowCount; i++)
                        {
                            Data entity = new Data();
                            IRow curRow = sheet.GetRow(i);
                            if(curRow != null)
                            {
                                //to show error message with excel row number
                                int excelrownumber = sheet.GetRow(i).RowNum + 1;
                                entity.name = curRow.GetCell(0).StringCellValue.Trim();
                                //entity.mobile = curRow.GetCell(1).StringCellValue.Trim();
                                bool m = false;
                                entity.mobile = String.Concat(curRow.GetCell(1).StringCellValue.Where(c => !Char.IsWhiteSpace(c)));
                                string mPattern = @"(^(09)[0-9]+$)";
                                Regex mobilePattern = new Regex(mPattern);
                                if (entity.mobile != null)
                                {
                                    //Regex mobilePattern = new Regex(@"^[0-9]\d{12}$");
                                    //bool mo = mobilePattern.IsMatch(entity.mobile);
                                    if (mobilePattern.IsMatch(entity.mobile)) //&& entity.mobile.Length <= 11
                                    {
                                        m = true;
                                    }
                                }

                                int mf = 0;
                                if (curRow.GetCell(2) != null)
                                {
                                    if (curRow.GetCell(2).StringCellValue.Trim() == "ကျား")
                                    {
                                        entity.gender = true;
                                    }
                                    else if (curRow.GetCell(2).StringCellValue.Trim() == "မ")
                                    {
                                        entity.gender = false;
                                    }
                                    else
                                    {
                                        mf = 1;
                                    }
                                }
                                if (curRow.GetCell(3) != null)
                                {
                                    string servicestr = curRow.GetCell(3).StringCellValue.Trim();
                                    if (!string.IsNullOrEmpty(servicestr))
                                    {
                                        CommandResult<Service> cmdresult = new CommandResult<Service>();
                                        Service serv = serviceRepo.FindByNameAndDept(servicestr, entrymodel.department_id);
                                        if (serv != null)
                                        {
                                            entity.service_id = serv.ID;
                                        }
                                        /*else
                                        {
                                            serv = new Service();
                                            serv.name = servicestr;
                                            serv.Dept_id = entrymodel.department_id;
                                            serv.CreatedBy = loginid;
                                            serv.CreatedDate = DateTime.Now;
                                            cmdresult = serviceServ.CreateOrUpdate(serv);
                                            if (cmdresult.Success)
                                            {
                                                entity.service_id = cmdresult.Result[0].ID;
                                            }
                                        }*/
                                    }
                                }
                                if (curRow.GetCell(4) != null)
                                {
                                    string state = curRow.GetCell(4).StringCellValue.Trim();
                                    if (!string.IsNullOrEmpty(state))
                                    {
                                        CommandResult<Location> cmdresult = new CommandResult<Location>();
                                        Location loc = locationRepo.FindByNameAndParentIdAndType(state, (int)LocationType.StateDivision, 0);
                                        if (loc != null)
                                        {
                                            entity.location_state_id = loc.ID;
                                        }
                                        /* else
                                         {
                                             loc = new Location();
                                             loc.Name = state;
                                             loc.Pcode = state;
                                             loc.Location_Type = (int)LocationType.StateDivision;
                                             loc.CreatedBy = loginid;
                                             loc.CreatedDate = DateTime.Now;
                                             cmdresult = locationServ.CreateOrUpdate(loc);
                                             if (cmdresult.Success)
                                             {
                                                 entity.location_state_id = cmdresult.Result[0].ID;
                                             }
                                         }*/
                                    }
                                }
                                if (curRow.GetCell(5) != null)
                                {
                                    string district = curRow.GetCell(5).StringCellValue.Trim();
                                    if (!string.IsNullOrEmpty(district))
                                    {
                                        CommandResult<Location> cmdresult = new CommandResult<Location>();
                                        Location loc = locationRepo.FindByNameAndParentIdAndType(district, (int)LocationType.District, entity.location_state_id);
                                        if (loc != null)
                                        {
                                            entity.location_district_id = loc.ID;
                                        }
                                        /*  else
                                          {
                                              loc = new Location();
                                              loc.Name = district;
                                              loc.Pcode = district;
                                              loc.Parent_Id = entity.location_state_id;
                                              loc.Location_Type = (int)LocationType.District;
                                              loc.CreatedBy = loginid;
                                              loc.CreatedDate = DateTime.Now;
                                              cmdresult = locationServ.CreateOrUpdate(loc);
                                              if (cmdresult.Success)
                                              {
                                                  entity.location_district_id = cmdresult.Result[0].ID;
                                              }
                                          }*/
                                    }
                                }
                                if (curRow.GetCell(6) != null)
                                {
                                    string township = curRow.GetCell(6).StringCellValue.Trim();
                                    if (!string.IsNullOrEmpty(township))
                                    {
                                        CommandResult<Location> cmdresult = new CommandResult<Location>();
                                        Location loc = locationRepo.FindByNameAndParentIdAndType(township, (int)LocationType.Township, entity.location_district_id);
                                        if (loc != null)
                                        {
                                            entity.location_township_id = loc.ID;
                                        }
                                        /*else
                                        {
                                            loc = new Location();
                                            loc.Name = township;
                                            loc.Pcode = township;
                                            loc.Parent_Id = entity.location_district_id;
                                            loc.Location_Type = (int)LocationType.Township;
                                            loc.CreatedBy = loginid;
                                            loc.CreatedDate = DateTime.Now;
                                            cmdresult = locationServ.CreateOrUpdate(loc);
                                            if (cmdresult.Success)
                                            {
                                                entity.location_township_id = cmdresult.Result[0].ID;
                                            }
                                        }
                                        */
                                    }
                                }
                                if (curRow.GetCell(7) != null)
                                {
                                    var ct = curRow.GetCell(7).CellType;
                                    if (ct == NPOI.SS.UserModel.CellType.Numeric)
                                    {
                                        entity.date_of_application = curRow.GetCell(7).DateCellValue;
                                    }
                                    else
                                    {
                                        string dopstr = string.Format("{0:yyyy-MM-dd}", curRow.GetCell(7).StringCellValue);
                                        var fff = curRow.GetCell(7);
                                        string[] dopdateString = dopstr.Split('.');
                                        DateTime date_of_application = Convert.ToDateTime(dopdateString[1] + "/" + dopdateString[0] + "/" + dopdateString[2]);
                                        entity.date_of_application = date_of_application;
                                    }
                                }
                                if (curRow.GetCell(8) != null)
                                {
                                    var ct = curRow.GetCell(8).CellType;
                                    if (ct == NPOI.SS.UserModel.CellType.Numeric)
                                    {
                                        entity.date_of_completion = curRow.GetCell(8).DateCellValue;
                                    }
                                    else
                                    {
                                        string docstr = string.Format("{0:yyyy-MM-dd}", curRow.GetCell(8).StringCellValue);
                                        string[] docdateString = docstr.Split('.');
                                        DateTime date_of_completion = Convert.ToDateTime(docdateString[1] + "/" + docdateString[0] + "/" + docdateString[2]);
                                        entity.date_of_completion = date_of_completion;
                                    }
                                }
                                entity.CreatedDate = DateTime.Now;
                                entity.CreatedBy = loginid;
                                entity.ministry_id = entrymodel.ministry_id;
                                entity.department_id = entrymodel.department_id;
                                if (m == true && mf == 0 && (entity.location_state_id != 0 && entity.location_district_id != 0) && entity.location_township_id != 0 && entity.service_id != 0
                                    && entity.date_of_application <= DateTime.Now && entity.date_of_completion >= entity.date_of_application)
                                {
                                    result = dataService.CreateOrUpdate(entity);
                                }
                                else
                                {
                                    result.Success = false;
                                    int succesrow = excelrownumber - 1;
                                    if (succesrow == 1)
                                    {
                                        if (result.Messages.Count > 0)
                                        {
                                            result.Messages[0] = "Excel Row Number " + excelrownumber +
                                                " တွင် ထည့်သွင်းထားသော အချက်အလက်များ လွဲနေပါသည်။";
                                        }
                                        else
                                        {
                                            result.Messages.Add("Excel Row Number " + excelrownumber +
                                                " တွင် ထည့်သွင်းထားသော အချက်အလက်များ လွဲနေပါသည်။");
                                        }
                                    }
                                    else
                                    {
                                        if (result.Messages.Count > 0)
                                        {
                                            result.Messages[0] = "Excel Row Number " + excelrownumber +
                                                " တွင် ထည့်သွင်းထားသော အချက်အလက်များ လွဲနေပါသည်။Row Number " + succesrow + " ထိ ထည့်သွင်းပြီးဖြစ်ပါသည်။";
                                        }
                                        else
                                        {
                                            result.Messages.Add("Excel Row Number " + excelrownumber +
                                                " တွင် ထည့်သွင်းထားသော အချက်အလက်များ လွဲနေပါသည်။Row Number " + succesrow + "  ထိ ထည့်သွင်းပြီးဖြစ်ပါသည်။");
                                        }
                                    }

                                    break;
                                }
                            }
                            else
                            {
                                return Json(result);
                            }
                        }
                    }
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(GetServerJsonResult<Data>());
            };
        }

        //public ActionResult OnPostImportFromExcel()
        //{
        //    IFormFile file = Request.Form.Files[0];
        //    string folderName = "Upload";
        //    string webRootPath = "";
        //    string newPath = Path.Combine(webRootPath, folderName);
        //    StringBuilder sb = new StringBuilder();
        //    if (!Directory.Exists(newPath))
        //        Directory.CreateDirectory(newPath);
        //    if (file.Length > 0)
        //    {
        //        string sFileExtension = Path.GetExtension(file.FileName).ToLower();
        //        ISheet sheet;
        //        string fullPath = Path.Combine(newPath, file.FileName);
        //        using (var stream = new FileStream(fullPath, FileMode.Create))
        //        {
        //            file.CopyTo(stream);
        //            stream.Position = 0;
        //            if (sFileExtension == ".xls")//This will read the Excel 97-2000 formats    
        //            {
        //                HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
        //                sheet = hssfwb.GetSheetAt(0);
        //            }
        //            else //This will read 2007 Excel format    
        //            {
        //                XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
        //                sheet = hssfwb.GetSheetAt(0);
        //            }
        //            IRow headerRow = sheet.GetRow(0);
        //            int cellCount = headerRow.LastCellNum;
        //            // Start creating the html which would be displayed in tabular format on the screen  
        //            sb.Append("<table class='table'><tr>");
        //            for (int j = 0; j < cellCount; j++)
        //            {
        //                NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
        //                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
        //                sb.Append("<th>" + cell.ToString() + "</th>");
        //            }
        //            sb.Append("</tr>");
        //            sb.AppendLine("<tr>");
        //            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
        //            {
        //                IRow row = sheet.GetRow(i);
        //                if (row == null) continue;
        //                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
        //                for (int j = row.FirstCellNum; j < cellCount; j++)
        //                {
        //                    if (row.GetCell(j) != null)
        //                        sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
        //                }
        //                sb.AppendLine("</tr>");
        //            }
        //            sb.Append("</table>");
        //        }
        //    }
        //    return this.Content(sb.ToString());
        //}
    }
}
