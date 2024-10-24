﻿

using AdabFest_Admin._Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebAPICode.Helpers;

namespace BAL.Repositories
{

    public class categoryDB : baseDB
    {
        public static CategoryBLL repo;
        public static DataTable _dt;
        public static DataSet _ds;
        public categoryDB()
           : base()
        {
            repo = new CategoryBLL();
            _dt = new DataTable();
            _ds = new DataSet();
        }

        public List<CategoryBLL> GetAll(int brandID)
        {
            try
            {
                var lst = new List<CategoryBLL>();
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@brandid", brandID);

                _dt = (new DBHelper().GetTableFromSP)("sp_GetCategory", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = _dt.DataTableToList<CategoryBLL>();
                       
                        //lst= JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToArray<CategoryBLL>()
                        //lst = _dt.ToList<CategoryBLL>().ToList();
                    }
                }
           
                return lst;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<CategoryBLL> GetAllActive(int brandID)
        {
            try
            {
                var lst = new List<CategoryBLL>();
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@brandid", brandID);

                _dt = (new DBHelper().GetTableFromSP)("sp_GetCategoryActive", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = _dt.DataTableToList<CategoryBLL>();

                        //lst= JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToArray<CategoryBLL>()
                        //lst = _dt.ToList<CategoryBLL>().ToList();
                    }
                }

                return lst;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public CategoryBLL Get(int id, int brandID)
        {
            try
            {
                var _obj = new CategoryBLL();
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@id", id);
                p[1] = new SqlParameter("@brandid", brandID);

                _dt = (new DBHelper().GetTableFromSP)("sp_GetCategorybyID_Admin", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        _obj = _dt.DataTableToList<CategoryBLL>().FirstOrDefault();
                        //_obj = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<CategoryBLL>();
                    }
                }

                return _obj;
            }
            catch (Exception)
            {
                return null;
            }
        }
       
        public int Insert(CategoryBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[10];

                p[0] = new SqlParameter("@Name", data.Name);
                p[1] = new SqlParameter("@ArabicName", data.ArabicName);
                p[2] = new SqlParameter("@Description", data.Description);
                p[3] = new SqlParameter("@Image", data.Image);
                p[4] = new SqlParameter("@DisplayOrder", data.DisplayOrder);
                p[5] = new SqlParameter("@LastUpdatedBy", data.LastUpdatedBy);
                p[6] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                p[7] = new SqlParameter("@StatusID", data.StatusID);
                p[8] = new SqlParameter("@LocationID", data.LocationID);
                p[9] = new SqlParameter("@BrandID", data.BrandID);
                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_insertCategory_Admin", p);
              
                return rtn;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int Update(CategoryBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[11];

                p[0] = new SqlParameter("@Name", data.Name);
                p[1] = new SqlParameter("@ArabicName", data.ArabicName);
                p[2] = new SqlParameter("@Description", data.Description);
                p[3] = new SqlParameter("@Image", data.Image);
                p[4] = new SqlParameter("@DisplayOrder", data.DisplayOrder);
                p[5] = new SqlParameter("@LastUpdatedBy", data.LastUpdatedBy);
                p[6] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                p[7] = new SqlParameter("@StatusID", data.StatusID);
                p[8] = new SqlParameter("@LocationID", data.LocationID);
                p[9] = new SqlParameter("@BrandID", data.BrandID);
                p[10] = new SqlParameter("@CategoryID", data.CategoryID);
                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_updateCategory_Admin", p);

                return rtn;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int Delete(CategoryBLL data)
        {
            try
            {
                int _obj = 0;
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@id", data.CategoryID);
                p[1] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);

                _obj = (new DBHelper().ExecuteNonQueryReturn)("sp_DeleteCategory", p);

                return _obj;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
