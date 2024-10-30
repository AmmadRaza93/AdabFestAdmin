using BAL.Repositories;
using AdabFest_Admin._Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using QRCoder;
using System.IO;
using System.Net.Mail;

namespace AdabFest_Admin.BLL._Services
{
    public class userService : baseService
    {
        userDB _service;
        public userService()
        {
            _service = new userDB();
        }
        public List<UserBLL> GetAllUser()
        {
            try
            {
                return _service.GetAllUser();
            }
            catch (Exception ex)
            {
                return new List<UserBLL>();
            }
        }
        public UserBLL GetUser(int id)
        {
            try
            {
                return _service.GetUser(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int InsertUser(UserBLL data)
        {
            try
            {
                data.CreatedDate = DateTime.UtcNow.AddMinutes(300);
                var result = _service.InsertUser(data);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int UpdateUser(UserBLL data, IWebHostEnvironment _env)
        {
            try
            {
                data.UpdatedDate = DateTime.UtcNow.AddMinutes(300);
                var result = _service.UpdateUser(data);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int DeleteUser(UserBLL data)
        {
            try
            {

                var result = _service.DeleteUser(data);
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
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
                //smtp.UseDefaultCredentials = true;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
