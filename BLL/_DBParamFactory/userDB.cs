
using DAL.Models;
using AdabFest_Admin._Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebAPICode.Helpers;

namespace BAL.Repositories
{

    public class userDB : baseDB
    {
        public static LoginBLL repo;
        public static UserBLL user;
        public static DataTable _dt;
        public static DataSet _ds;
        public userDB()
            : base()
        {
            user = new UserBLL();
            repo = new LoginBLL();
            _dt = new DataTable();
            _ds = new DataSet();
        }
        public List<UserBLL> GetAllUser()
        {
            try
            {
                var lst = new List<UserBLL>();
                SqlParameter[] p = new SqlParameter[0];

                _dt = (new DBHelper().GetTableFromSP)("sp_GetRegisterUser_Admin", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<UserBLL>>();
                    }
                }
                return lst;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public UserBLL GetUser(int id)
        {
            try
            {
                var _obj = new UserBLL();
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@id", id);
                _dt = (new DBHelper().GetTableFromSP)("sp_GetUserbyID_Admin", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        _obj = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<UserBLL>>().FirstOrDefault();
                    }
                }
                return _obj;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public int InsertUser(UserBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[4];

                p[0] = new SqlParameter("@UserName", data.UserName);
                p[1] = new SqlParameter("@ContactNo", data.ContactNo);
                p[2] = new SqlParameter("@StatusID", data.StatusID);
                p[3] = new SqlParameter("@CreatedDate", data.CreatedDate);

                rtn = (new DBHelper().ExecuteNonQueryReturn)("sp_InsertUser_Admin", p);
                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int UpdateUser(UserBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[5];

                p[0] = new SqlParameter("@UserName", data.UserName);
                p[1] = new SqlParameter("@ContactNo", data.ContactNo);
                p[2] = new SqlParameter("@StatusID", data.StatusID);
                p[3] = new SqlParameter("@UpdatedDate", data.UpdatedDate);
                p[4] = new SqlParameter("@UserID", data.UserID);

                rtn = (new DBHelper().ExecuteNonQueryReturn)("sp_UpdateUser_Admin", p);

                return rtn;
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
                int _obj = 0;
                data.UpdatedDate = DateTime.UtcNow.AddMinutes(180);
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@UserID", data.UserID);
                p[1] = new SqlParameter("@UpdatedDate", data.UpdatedDate);

                _obj = (new DBHelper().ExecuteNonQueryReturn)("sp_DeleteUser_Admin", p);

                return _obj;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
