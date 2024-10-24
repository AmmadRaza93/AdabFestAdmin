﻿

using AdabFest_Admin._Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using WebAPICode.Helpers;

namespace BAL.Repositories
{

    public class eventAttendeesDB : baseDB
    {
        public static EventAttendeesBLL repo;
        public static DataTable _dt;
        public static DataTable _dt1;
        public static DataSet _ds;
        public eventAttendeesDB()
           : base()
        {
            repo = new EventAttendeesBLL();
            _dt = new DataTable();
            _ds = new DataSet();
        }

        public List<EventAttendeesBLL> GetAll()
        {
            try
            {
                var lst = new List<EventAttendeesBLL>();
                SqlParameter[] p = new SqlParameter[0];
                 
                _dt = (new DBHelper().GetTableFromSP)("sp_GetRegisterUser_Admin", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<EventAttendeesBLL>>();
                    }
                }

                return lst;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<EventBLL> GetAllDropdown()
        {
            try
            {
                var lst = new List<EventBLL>();
                SqlParameter[] p = new SqlParameter[0];

                _dt = (new DBHelper().GetTableFromSP)("sp_GetAllDropdownEvents", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<EventBLL>>();
                    }
                }

                return lst;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public int Status(EventAttendeesBLL data)
        {
            try
            {
                int _obj = 0;
                SqlParameter[] p = new SqlParameter[7];
                p[0] = new SqlParameter("@AttendeesID", data.AttendeesID);
                p[1] = new SqlParameter("@StatusID", data.StatusID);
                p[2] = new SqlParameter("@MessageForAttendee", data.MessageForAttendee);
                p[3] = new SqlParameter("@Subject", data.Subject);
                p[4] = new SqlParameter("@Updatedon", DateTime.UtcNow.AddMinutes(300));
                p[5] = new SqlParameter("@MeetingLink", data.MeetingLink);
                p[6] = new SqlParameter("@Password", data.Password);

                _obj = (new DBHelper().ExecuteNonQueryReturn)("sp_EventAttendeesStatus", p);
                return _obj;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        //aarij
        public EventAttendeesBLL Get(int id)
        {
            try
            {
                var _obj = new EventAttendeesBLL();
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@id", id);

                _dt = (new DBHelper().GetTableFromSP)("sp_GetEventAttendeesbyID_Admin", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        _obj = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<EventAttendeesBLL>>().FirstOrDefault();
                        //if (_obj.ImageSS != null && _obj.ImageSS != "")
                        //{
                        //    _obj.ImageSS = "akuapp-001-site1.mysitepanel.net/" + _obj.ImageSS;
                        //}
                        //else
                        //{
                        //    _obj.ImageSS = "";
                        //}
                    }
                }
                return _obj;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public EventAttendeesBLL Getcustomer(int? UserID)
        {
            try
            {
                var _obj = new EventAttendeesBLL();
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@UserID", UserID);
                _dt = (new DBHelper().GetTableFromSP)("sp_GetAttendeeByUserID_Admin", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        _obj = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<EventAttendeesBLL>>().FirstOrDefault();
                    }
                }
                return _obj;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public int UpdateApproved(EventAttendeesBLL obj, IWebHostEnvironment _env, string Body)
        {
            string msg = "";
            //msg = obj.StatusMsg;
            string contentRootPath = _env.ContentRootPath;

            string path = "/ClientApp/dist/assets/Upload/";
            string filePath = contentRootPath + path;

            try
            {
                var data = Get(obj.AttendeesID);
                var email = Getcustomer(obj.UserID);

                string ToEmail, SubJect;
                ToEmail = email.Email;

                Body = Body.Replace("#description#", data.MessageForAttendee == null || data.MessageForAttendee == "" ? "N/A" : data.MessageForAttendee)
                    .Replace("#date#", data.Createdon.ToString())

                    .Replace("#name#", email.FullName)
                    .Replace("#contact#", email.PhoneNo)
                    .Replace("#username#", obj.Email);
                SendEmail(obj.Subject, Body, email.Email);
            }
            catch { }
            return 1;
        }
        public void SendEmail(string _SubjectEmail, string _BodyEmail, string _To)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(_To);
                mail.From = new MailAddress("akuhevents@gmail.com");
                mail.Subject = _SubjectEmail;
                mail.Body = _BodyEmail;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                smtp.Port = Int32.Parse("587");
                smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential
                     ("akuhevents@gmail.com", "ueuzxvrsgtaxdbev");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
            }
        }
        public List<string> GetItemImages(int id)
        {

            try
            {
                var _obj = new EventBLL();
                List<string> ImagesSource = new List<string>();
                _dt = new DataTable();
                SqlParameter[] p1 = new SqlParameter[1];
                p1[0] = new SqlParameter("@id", id);
                _dt = (new DBHelper().GetTableFromSP)("sp_GetEventImages_Admin", p1);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        _obj.EventImages = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<EventImagesBLL>>();

                        for (int i = 0; i < _obj.EventImages.Count; i++)
                        {
                            ImagesSource.Add(_obj.EventImages[i].Image);
                        }
                    }
                }

                return ImagesSource;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ItemSettingsBLL GetItemSettings(int brandID)
        {
            try
            {
                var _obj = new ItemSettingsBLL();
                var _obj1 = new ItemSettingsBLL();
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@brandid", brandID);

                SqlParameter[] p1 = new SqlParameter[1];
                p1[0] = new SqlParameter("@AppInfoID",1);
                _dt = (new DBHelper().GetTableFromSP)("sp_GetItemSettings_Admin", p);
                _dt1 = (new DBHelper().GetTableFromSP)("sp_GetItemSettingsTitle_Admin", p1);
                
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        _obj= JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<ItemSettingsBLL>>().FirstOrDefault();
                        _obj.BrandID = brandID;
                        _obj1 = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt1)).ToObject<List<ItemSettingsBLL>>().FirstOrDefault();
                        _obj1.Items = _obj.Items;
                        _obj1.BrandID = _obj.BrandID;
                    }
                }
                return _obj1;
            }
            catch (Exception)
            {
                return new ItemSettingsBLL();
            }
        }
        public int Insert(EventAttendeesBLL data)
        {
            try
            {                 
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[5];

                p[0] = new SqlParameter("@FullName", data.FullName);
                p[1] = new SqlParameter("@Email", data.Email);                
                p[2] = new SqlParameter("@PhoneNo", data.PhoneNo);
                p[3] = new SqlParameter("@StatusID", 101);
                p[4] = new SqlParameter("@Createdon", DateTime.UtcNow.AddMinutes(300));

                rtn = int.Parse(new DBHelper().GetTableFromSP("dbo.sp_InsertEventAttendees", p).Rows[0]["AttendeesID"].ToString());
                 
                return rtn;
            }
            catch (Exception ex)
            {
                LogExceptionToFile(ex, @"E:\exceptionLog.txt");
                return 0;
            }

        }
        private void LogExceptionToFile(Exception ex, string logFilePath)
        {
            
            // Log exception details to a text file
            using (StreamWriter writer = new StreamWriter(logFilePath, append: true))
            {
                writer.WriteLine($"Timestamp: {DateTime.Now}");
                writer.WriteLine($"Exception Type: {ex.GetType().Name}");
                writer.WriteLine($"Message: {ex.Message}");
                writer.WriteLine($"StackTrace: {ex.StackTrace}");
                writer.WriteLine(new string('-', 50)); // Separator for better readability
            }
        }

        public int Update(EventAttendeesBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[5];

                p[0] = new SqlParameter("@FullName", data.FullName);                
                p[1] = new SqlParameter("@Email", data.Email);
                p[2] = new SqlParameter("@PhoneNo", data.PhoneNo);                                                       
                p[3] = new SqlParameter("@AttendeesID", data.AttendeesID);
                p[4] = new SqlParameter("@Updatedon", data.Updatedon);

                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_UpdateEventAttendees_Admin", p);
               
                return rtn;
            }
            catch (Exception ex)
            {
                LogExceptionToFile(ex, @"E:\exceptionLog.txt");
                return 0;
            }
        }
        public int UpdateItemSettings(ItemSettingsBLL data)
        {
            try
            {
                if (data.Items == "")
                {
                    SqlParameter[] p = new SqlParameter[2];
                    p[0] = new SqlParameter("@BrandID", data.BrandID);
                    p[1] = new SqlParameter("@Items", data.Items);
                    (new DBHelper().ExecuteNonQueryReturn)("sp_DeleteTodaySpecial_Admin", p);

                    SqlParameter[] pp = new SqlParameter[2];
                    pp[0] = new SqlParameter("@ItemSettingTitle", data.ItemSettingTitle);
                    pp[1] = new SqlParameter("@IsItemSetting", data.IsItemSetting);
                    (new DBHelper().ExecuteNonQueryReturn)("sp_UpdateTodaySpecial_Admin", pp);
                }
                else if (data.Items != "" && data.Items != null)
                {
                    SqlParameter[] p1 = new SqlParameter[2];
                    p1[0] = new SqlParameter("@BrandID", data.BrandID);
                    p1[1] = new SqlParameter("@Items", data.Items);
                    (new DBHelper().ExecuteNonQueryReturn)("sp_insertItemSettings_Admin", p1);

                    SqlParameter[] pp = new SqlParameter[2];
                    pp[0] = new SqlParameter("@ItemSettingTitle", data.ItemSettingTitle);
                    pp[1] = new SqlParameter("@IsItemSetting", data.IsItemSetting);
                    (new DBHelper().ExecuteNonQueryReturn)("sp_UpdateTodaySpecial_Admin", pp);
                }

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int Delete(EventAttendeesBLL data)
        {
            try
            {
                int _obj = 0;
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@AttendeesID", data.AttendeesID);
                p[1] = new SqlParameter("@Updatedon", data.Updatedon);

                _obj = (new DBHelper().ExecuteNonQueryReturn)("sp_DeleteEventAttendees_Admin", p);

                return _obj;
            }
            catch (Exception ex)
            {
                LogExceptionToFile(ex, @"E:\exceptionLog.txt");
                return 0;
            }
        }
        public List<EventDetailsBLL> GetEventsDetailRpt(string EventID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                string a = EventID.Replace("$", "");
                var lst = new List<EventDetailsBLL>();

                SqlParameter[] p = new SqlParameter[3];
                
                p[0] = new SqlParameter("@EventID", a);
                p[1] = new SqlParameter("@fromdate", FromDate);
                p[2] = new SqlParameter("@todate", ToDate);
                


                _dt = (new DBHelper().GetTableFromSP)("sp_rptEventDetailsReport", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<EventDetailsBLL>>();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return new List<EventDetailsBLL>();
            }
        }
        public List<EventDetailsBLL> ConfirmListReport(string EventID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                string a = EventID.Replace("$", "");
                var lst = new List<EventDetailsBLL>();

                SqlParameter[] p = new SqlParameter[3];

                p[0] = new SqlParameter("@EventID", a);
                p[1] = new SqlParameter("@fromdate", FromDate);
                p[2] = new SqlParameter("@todate", ToDate);



                _dt = (new DBHelper().GetTableFromSP)("sp_rptConfirmListReport", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<EventDetailsBLL>>();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return new List<EventDetailsBLL>();
            }
        }
        
    }
}
