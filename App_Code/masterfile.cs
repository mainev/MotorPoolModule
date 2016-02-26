using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using System.Data.SqlClient;

using System.Web.Script.Services;
using System.Text;
using Newtonsoft.Json;
using System.Globalization;
using System.Web.Script.Serialization;

using ClosedXML.Excel;
using System.IO;
using System.Net.Mail;
using Newtonsoft.Json.Converters;


/// <summary>
/// Summary description for masterfile
/// </summary>
[WebService(Namespace = "http://microsoft.com/webservices/")] 
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService()]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class masterfile : System.Web.Services.WebService
{

    #region General Masterfiles

    [WebMethod(EnableSession = true)]
    public string Save_Masterfile(string xcode, string xdescs, int xstatus, string xhidden_entry)
    {
      
        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
              
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
        }


        if (xdescs.Replace("undefined", string.Empty) == string.Empty)
        {
            return "<span style='color:red'>Please input the required values...</span>";
        }



        string x = null;
        string audit_date = System.DateTime.Now.ToString();
        

        if (HttpContext.Current.Session["hidden_entry"] == "no")
        {

            if (string.IsNullOrEmpty(xcode))
            {
                string sql = "Select code FROM " + HttpContext.Current.Session["table_name"] + " WHERE (code = '" + xcode + "') ";
                //& " WHERE (descs = '" & xdescs.Replace("'", "''").Trim & "')"
                SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataReader dtr = default(SqlDataReader);

                cn.Open();
                dtr = cmd.ExecuteReader();
                bool sfind = false;
                if (dtr.Read())
                {
                    sfind = true;
                }
                cn.Close();


                if (sfind == false)
                {

                    if (HttpContext.Current.Session["table_name"] == "cf_company")
                    {
                        xcode = Security.Get_ID(HttpContext.Current.Session["table_name"].ToString()).ToString("00");
                    }
                    else
                    {
                        xcode = Security.Get_ID(HttpContext.Current.Session["table_name"].ToString()).ToString("00000");
                    }

                    sql = "INSERT INTO " + HttpContext.Current.Session["table_name"] + " " + "(code, descs, audit_user, audit_date, status) " + "VALUES('" + xcode + "', " + "'" + xdescs.Replace(",", string.Empty).Replace("'", "''") + "', " + "'" + HttpContext.Current.Session["username"] + "', " + "'" + audit_date + "', " + "1" + ") ";

                    cmd = new SqlCommand(sql, cn);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    cn.Dispose();

                    Security.Write_ID(HttpContext.Current.Session["table_name"].ToString(), Convert.ToDouble(xcode));

                    x = "New Record saved... <br>Description : " + xdescs;

                }
                else
                {
                    x = "<span style='color:red'>Duplicated Record found.... <br>Description : " + xdescs + "</span>";

                }


            }
            else
            {
                string sql = "UPDATE " + HttpContext.Current.Session["table_name"] + " ";
                sql = sql + "SET ";
                sql = sql + "descs = '" + xdescs.Replace(",", string.Empty).Replace("'", "''") + "',  ";
                sql = sql + "audit_user = '" + HttpContext.Current.Session["username"] + "',  ";
                sql = sql + "audit_date = '" + audit_date + "',  ";
                sql = sql + "status = " + xstatus + "  ";
                sql = sql + "WHERE code = '" + xcode + "' ";

                SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                cn.Dispose();

                x = "Record updated.... <br>Description : " + xdescs;
            }


        }
        else if (HttpContext.Current.Session["hidden_entry"] == "yes")
        {


            if (xhidden_entry == "new")
            {
                string sql = "Select code FROM " + HttpContext.Current.Session["table_name"] + " WHERE (code = '" + xcode + "') ";
                //OR (descs = '" & xdescs.Replace("'", "''").Trim & "')"
                SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataReader dtr = default(SqlDataReader);

                cn.Open();
                dtr = cmd.ExecuteReader();
                bool sfind = false;
                if (dtr.Read())
                {
                    sfind = true;
                }
                cn.Close();


                if (sfind == false)
                {
                    sql = "INSERT INTO " + HttpContext.Current.Session["table_name"] + " " + "(code, descs, audit_user, audit_date, status) " + "VALUES('" + xcode + "', " + "'" + xdescs.Replace(",", string.Empty).Replace("'", "''") + "', " + "'" + HttpContext.Current.Session["username"] + "', " + "'" + audit_date + "', " + "1" + ") ";

                    cmd = new SqlCommand(sql, cn);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    cn.Dispose();

                    x = "New Record saved... <br>Description : " + xdescs;

                }
                else
                {
                    x = "<span style='color:red'>Duplicated Record found.... <br>Code : " + xcode + "<br>Description : " + xdescs + "</span>";
                }

            }
            else
            {
                string sql = "UPDATE " + HttpContext.Current.Session["table_name"] + " ";
                sql = sql + "SET ";
                sql = sql + "descs = '" + xdescs.Replace(",", string.Empty).Replace("'", "''") + "',  ";
                sql = sql + "audit_user = '" + HttpContext.Current.Session["username"] + "',  ";
                sql = sql + "audit_date = '" + audit_date + "',  ";
                sql = sql + "status = " + xstatus + "  ";
                sql = sql + "WHERE code = '" + xcode + "' ";

                SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                cn.Dispose();

                x = "Record updated.... <br>Description : " + xdescs;

            }


        }

        return x;

    }

    [WebMethod(EnableSession = true)]
    public string Save_Group_Masterfile(string xcode, string xdescs, int xstatus, string xhidden_entry, string xGroup_field, string xGroup_entry)
    { 
        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
               
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
         
        }


        if (xdescs.Replace("undefined", string.Empty) == string.Empty)
        {
            return "<span style='color:red'>Please input the required values...</span>";
          
        }



        string x = null;
        string audit_date = System.DateTime.Now.ToString();


        if (HttpContext.Current.Session["hidden_entry"] == "no")
        {

            if (string.IsNullOrEmpty(xcode))
            {
                string sql = "Select code FROM " + HttpContext.Current.Session["table_name"] + " WHERE (descs = '" + xdescs.Replace("'", "''").Trim() + "') AND " + xGroup_field + " = '" + xGroup_entry + "' ";
                SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataReader dtr = default(SqlDataReader);

                cn.Open();
                dtr = cmd.ExecuteReader();
                bool sfind = false;
                if (dtr.Read())
                {
                    sfind = true;
                }
                cn.Close();


                if (sfind == false)
                {

                    if (HttpContext.Current.Session["table_name"] == "cf_company")
                    {
                        xcode = Security.Get_ID(HttpContext.Current.Session["table_name"].ToString()).ToString("00");
                    }
                    else
                    {
                        xcode = Security.Get_ID(HttpContext.Current.Session["table_name"].ToString()).ToString("00000");
                    }

                    sql = "INSERT INTO " + HttpContext.Current.Session["table_name"] + " " + "(code, descs, audit_user, audit_date, status, " + xGroup_field + ") " + "VALUES('" + xcode + "', " + "'" + xdescs.Replace(",", string.Empty).Replace("'", "''") + "', " + "'" + HttpContext.Current.Session["username"] + "', " + "'" + audit_date + "', " + "1, " + "'" + xGroup_entry + "' " + ") ";

                    cmd = new SqlCommand(sql, cn);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    cn.Dispose();

                    Security.Write_ID(HttpContext.Current.Session["table_name"].ToString(), Convert.ToDouble(xcode));

                    x = "New Record saved... <br>Description : " + xdescs;

                }
                else
                {
                    x = "<span style='color:red'>Duplicated Record found.... <br>Description : " + xdescs + "</span>";

                }


            }
            else
            {
                string sql = "UPDATE " + HttpContext.Current.Session["table_name"] + " ";
                sql = sql + "SET ";
                sql = sql + "descs = '" + xdescs.Replace(",", string.Empty).Replace("'", "''") + "',  ";
                sql = sql + "audit_user = '" + HttpContext.Current.Session["username"] + "',  ";
                sql = sql + "audit_date = '" + audit_date + "',  ";
                sql = sql + "status = " + xstatus + "  ";
                sql = sql + "WHERE code = '" + xcode + "' ";
                sql = sql + "AND " + xGroup_field + " = '" + xGroup_entry + "' ";

                SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                cn.Dispose();

                x = "Record updated.... <br>Description : " + xdescs;
            }


        }
        else if (HttpContext.Current.Session["hidden_entry"] == "yes")
        {


            if (xhidden_entry == "new")
            {
                string sql = "Select code FROM " + HttpContext.Current.Session["table_name"] + " WHERE (code = '" + xcode + "') AND " + xGroup_field + " = '" + xGroup_entry + "' OR (descs = '" + xdescs.Replace("'", "''").Trim() + "') AND " + xGroup_field + " = '" + xGroup_entry + "'";
                SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataReader dtr = default(SqlDataReader);

                cn.Open();
                dtr = cmd.ExecuteReader();
                bool sfind = false;
                if (dtr.Read())
                {
                    sfind = true;
                }
                cn.Close();


                if (sfind == false)
                {
                    sql = "INSERT INTO " + HttpContext.Current.Session["table_name"] + " " + "(code, descs, audit_user, audit_date, status, " + xGroup_field + ") " + "VALUES('" + xcode + "', " + "'" + xdescs.Replace(",", string.Empty).Replace("'", "''") + "', " + "'" + HttpContext.Current.Session["username"] + "', " + "'" + audit_date + "', " + "1, " + "'" + xGroup_entry + "' " + ") ";

                    cmd = new SqlCommand(sql, cn);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    cn.Dispose();

                    x = "New Record saved... <br>Description : " + xdescs;

                }
                else
                {
                    x = "<span style='color:red'>Duplicated Record found.... <br>Code : " + xcode + "<br>Description : " + xdescs + "</span>";
                }

            }
            else
            {
                string sql = "UPDATE " + HttpContext.Current.Session["table_name"] + " ";
                sql = sql + "SET ";
                sql = sql + "descs = '" + xdescs.Replace(",", string.Empty).Replace("'", "''") + "',  ";
                sql = sql + "audit_user = '" + HttpContext.Current.Session["username"] + "',  ";
                sql = sql + "audit_date = '" + audit_date + "',  ";
                sql = sql + "status = " + xstatus + "  ";
                sql = sql + "WHERE code = '" + xcode + "' ";
                sql = sql + "AND " + xGroup_field + " = '" + xGroup_entry + "' ";

                SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                cn.Dispose();

                x = "Record updated.... <br>Description : " + xdescs;

            }


        }

        return x;
      
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public string GetTableData()
    {
    
        var echo = int.Parse(HttpContext.Current.Request.Params["sEcho"]);
        var displayLength = int.Parse(HttpContext.Current.Request.Params["iDisplayLength"]);
        var displayStart = int.Parse(HttpContext.Current.Request.Params["iDisplayStart"]);
        var sortOrder = HttpContext.Current.Request.Params["sSortDir_0"].ToString(CultureInfo.CurrentCulture);
        var roleId = HttpContext.Current.Request.Params["roleId"].ToString(CultureInfo.CurrentCulture);
        var sortCol = HttpContext.Current.Request.Params["iSortCol_0"].ToString(CultureInfo.CurrentCulture);
        var xParameter = HttpContext.Current.Request.Params["parmsVals"].ToString();

        string _search = "";
        string _function = "";
        string _Group_field = "";
        string _Group_entry = "";

        string _warehouse_cd = "";
        string _status = "";
        string _doc_no = "";
        string _item_cd = "";

        if (xParameter.IndexOf(",") > -1)
        {
            _function = xParameter.Substring(0, xParameter.IndexOf(","));

            if (_function == "f1")
            {
                xParameter = xParameter.Replace(_function + ",", "");
                _search = xParameter;
            }
            else if (_function == "f2")
            {
                xParameter = xParameter.Replace(_function + ",", "");
                _Group_field = xParameter.Substring(0, xParameter.IndexOf(","));

                xParameter = xParameter.Replace(_Group_field + ",", "");
                _Group_entry = xParameter.Substring(0, xParameter.IndexOf(","));

                xParameter = xParameter.Replace(_Group_entry + ",", "");
                _search = xParameter;
            }
            else if (_function == "f3")
            {
                xParameter = xParameter.Replace(_function + ",", "");
                _search = xParameter;
            }
            else if (_function == "f4")
            {
                xParameter = xParameter.Replace(_function + ",", "");
                _search = xParameter;
            }
            else if (_function == "f5")
            {
                xParameter = xParameter.Replace(_function + ",", "");
                _search = xParameter;
            }
            else if (_function == "f6")
            {
                xParameter = xParameter.Replace(_function + ",", "");
                _warehouse_cd = xParameter.Substring(0, xParameter.IndexOf(","));

                xParameter = xParameter.Replace(_warehouse_cd + ",", "");
                _warehouse_cd = _warehouse_cd.Replace("w", "");
                _status = xParameter.Substring(0, xParameter.IndexOf(","));

                xParameter = xParameter.Replace(_status + ",", "");
                _status = _status.Replace("s", "");
                _doc_no = xParameter.Substring(0, xParameter.IndexOf(","));

                xParameter = xParameter.Replace(_doc_no + ",", "");
                _doc_no = _doc_no.Replace("d", "");
                _item_cd = xParameter;

                _item_cd = _item_cd.Replace("i", "");

            }
        }
        var records = GetMasterfile(_function, _Group_field, _Group_entry, _search, _warehouse_cd, _status, _doc_no, _item_cd).ToList();
        
        if (records == null)
        {
            return string.Empty;
        }     

        var orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Code) : records.OrderByDescending(o => o._Code);
       
        if (_function == "f3")
        {
            switch (sortCol)
            {
                case "0":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Username) : records.OrderByDescending(o => o._Username);
                    break;
                case "1":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Name) : records.OrderByDescending(o => o._Name);
                    break;
                case "2":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Department) : records.OrderByDescending(o => o._Department);
                    break;
                case "3":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Group_Access) : records.OrderByDescending(o => o._Group_Access);
                    break;
                case "4":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Status) : records.OrderByDescending(o => o._Status);
                    break;
            }
        }
        else if (_function == "f4")
        {
            switch (sortCol)
            {
                case "0":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Code) : records.OrderByDescending(o => o._Code);
                    break;
                case "1":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Description) : records.OrderByDescending(o => o._Description);
                    break;
                case "2":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._UOM) : records.OrderByDescending(o => o._UOM);
                    break;
                case "3":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Standard_Cost) : records.OrderByDescending(o => o._Standard_Cost);
                    break;
                case "4":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Latest_Cost) : records.OrderByDescending(o => o._Latest_Cost);
                    break;
                case "5":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Weight) : records.OrderByDescending(o => o._Weight);
                    break;
                case "6":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Type) : records.OrderByDescending(o => o._Type);
                    break;
                case "7":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Class) : records.OrderByDescending(o => o._Class);
                    break;
                case "8":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Category) : records.OrderByDescending(o => o._Category);
                    break;
                case "9":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Status) : records.OrderByDescending(o => o._Status);
                    break;
            }
        }
        else if (_function == "f5")
        {
            switch (sortCol)
            {
                case "0":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._UOM) : records.OrderByDescending(o => o._UOM);
                    break;
                case "1":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._UOM_To) : records.OrderByDescending(o => o._UOM_To);
                    break;
                case "2":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Weight) : records.OrderByDescending(o => o._Weight);
                    break;
            }
        }
        else if (_function == "f6")
        {
            switch (sortCol)
            {
                case "0":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._trx_type) : records.OrderByDescending(o => o._trx_type);
                    break;
                case "1":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._doc_no) : records.OrderByDescending(o => o._doc_no);
                    break;
                case "2":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._doc_date) : records.OrderByDescending(o => o._doc_date);
                    break;
                case "3":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._doc_descs) : records.OrderByDescending(o => o._doc_descs);
                    break;
                case "4":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Status) : records.OrderByDescending(o => o._Status);
                    break;
                case "5":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._item_cd) : records.OrderByDescending(o => o._item_cd);
                    break;
                case "6":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._descs) : records.OrderByDescending(o => o._descs);
                    break;
                case "7":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._item_weight) : records.OrderByDescending(o => o._item_weight);
                    break;
                case "8":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._UOM) : records.OrderByDescending(o => o._UOM);
                    break;
                case "9":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._qty) : records.OrderByDescending(o => o._qty);
                    break;
                case "10":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._unit_cost) : records.OrderByDescending(o => o._unit_cost);
                    break;
                case "11":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._total_cost) : records.OrderByDescending(o => o._total_cost);
                    break;
                case "12":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._lot_no) : records.OrderByDescending(o => o._lot_no);
                    break;
                case "13":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._mfg_date) : records.OrderByDescending(o => o._mfg_date);
                    break;
                case "14":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._expiry_date) : records.OrderByDescending(o => o._expiry_date);
                    break;
            }
        }
        else
        {
            switch (sortCol)
            {
                case "0":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Code) : records.OrderByDescending(o => o._Code);
                    break;
                case "1":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Description) : records.OrderByDescending(o => o._Description);
                    break;
                case "2":
                    orderedResults = sortOrder == "asc" ? records.OrderBy(o => o._Status) : records.OrderByDescending(o => o._Status);
                    break;
            }
        }

        var itemsToSkip = displayStart == 0 ? 0 : displayStart + 1;
        var pagedResults = orderedResults.Skip(itemsToSkip).Take(displayLength).ToList();
        var hasMoreRecords = false;

        var sb = new StringBuilder();
        sb.Append("{");
        sb.Append("\"sEcho\": " + echo + ",");
        sb.Append("\"recordsTotal\": " + records.Count + ",");
        sb.Append("\"recordsFiltered\": " + records.Count + ",");
        sb.Append("\"iTotalRecords\": " + records.Count + ",");
        sb.Append("\"iTotalDisplayRecords\": " + records.Count + ",");
        sb.Append("\"aaData\": [");
        foreach (var result in pagedResults)
        {
         
            if (hasMoreRecords)
            {
                sb.Append(",");
            }
            if (_function == "f3")
            {
                sb.Append("[");
                sb.Append("\"" + result._Username + "\",");
                sb.Append("\"" + result._Name + "\",");
                sb.Append("\"" + result._Department + "\",");
                sb.Append("\"" + result._Group_Access + "\",");
                sb.Append("\"" + result._Status + "\"");
                sb.Append("]");
            }
            else if (_function == "f4")
            {
                sb.Append("[");
                sb.Append("\"" + result._Code + "\",");
                sb.Append("\"" + result._Description + "\",");
                sb.Append("\"" + result._UOM + "\",");
                sb.Append("\"" + result._Standard_Cost + "\",");
                sb.Append("\"" + result._Latest_Cost + "\",");
                sb.Append("\"" + result._Weight + "\",");
                sb.Append("\"" + result._Type + "\",");
                sb.Append("\"" + result._Class + "\",");
                sb.Append("\"" + result._Category + "\",");
                sb.Append("\"" + result._Status + "\"");
                sb.Append("]");
            }
            else if (_function == "f5")
            {
                sb.Append("[");
                sb.Append("\"" + result._UOM + "\",");
                sb.Append("\"" + result._UOM_To + "\",");
                sb.Append("\"" + result._Weight + "\"");
                sb.Append("]");
            }
            else if (_function == "f6")
            {
                sb.Append("[");
                sb.Append("\"" + result._trx_type + "\",");
                sb.Append("\"" + result._doc_no + "\",");
                sb.Append("\"" + result._doc_date + "\",");
                sb.Append("\"" + result._doc_descs + "\",");
                sb.Append("\"" + result._Status + "\",");
                sb.Append("\"" + result._item_cd + "\",");
                sb.Append("\"" + result._descs + "\",");
                sb.Append("\"" + result._item_weight + "\",");
                sb.Append("\"" + result._UOM + "\",");
                sb.Append("\"" + result._qty + "\",");
                sb.Append("\"" + result._unit_cost + "\",");
                sb.Append("\"" + result._total_cost + "\",");
                sb.Append("\"" + result._lot_no + "\",");
                sb.Append("\"" + result._mfg_date + "\",");
                sb.Append("\"" + result._expiry_date + "\"");
                sb.Append("]");
            }
            else
            {
                sb.Append("[");
                sb.Append("\"" + result._Code + "\",");
                sb.Append("\"" + result._Description + "\",");
                sb.Append("\"" + result._Status + "\"");
                sb.Append("]");
            }
            hasMoreRecords = true;
        }
        sb.Append("]}");
        return sb.ToString();
    }

    private static IEnumerable<MasterfileField> GetMasterfile(string _function, string _Group_field, string _Group_entry, string _search, string _warehouse_cd, string _status, string _doc_no, string _item_cd)
    {
        
        // At this point you can call to your database to get the data
        // but I will just populate a sample collection in code
        string sql = "";

        if (_function == "f3")
        {
            sql = "SELECT        cf_users.username, cf_users.employee_id, cf_users.name, cf_users.department_code, cf_users.group_access, cf_users.status, cf_users.first_name, cf_users.last_name, cf_users.middle_name, cf_users.suffix,  ";
            sql = sql + "cf_department.descs AS dept_descs, cf_group_access.descs AS group_access_descs ";
            sql = sql + "FROM            cf_users INNER JOIN ";
            sql = sql + "cf_group_access ON cf_users.group_access = cf_group_access.code INNER JOIN ";
            sql = sql + "cf_department ON cf_users.department_code = cf_department.code ";
            if (!string.IsNullOrEmpty(_search))
            {
                sql = sql + "AND cf_users.name + ' ' + cf_users.username +' '+ cf_users.employee_id + ' ' + cf_department.descs + ' ' + cf_group_access.descs like '%" + _search + "%' ";
            }
            //sql = sql & "ORDER BY cf_users.last_name, cf_users.first_name, cf_users.middle_name, cf_users.suffix "

            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader dtr = default(SqlDataReader);
            cn.Open();
            dtr = cmd.ExecuteReader();

            List<MasterfileField> _list = new List<MasterfileField>();

            while (dtr.Read())
            {
                MasterfileField _fields = new MasterfileField();

                _fields._Username = dtr["username"].ToString().Trim();
                _fields._Name = dtr["name"].ToString().Trim();
                _fields._Department = dtr["dept_descs"].ToString().Trim();
                _fields._Group_Access = dtr["group_access_descs"].ToString().Trim();
                _fields._Status = (Convert.ToBoolean(dtr["status"]) == false ? "In-Active" : "Active");
                _list.Add(_fields);

            }
            dtr.Close();
            cn.Close();
            cn.Dispose();

            return _list;
        }
        else if (_function == "f4")
        {
            sql = "SELECT        item.item_cd, item.descs, item.uom, item.item_type_cd, item.item_category_cd, item.remarks, item.standard_cost, item.latest_cost, item.item_weight, item.audit_user, item.audit_date, item.item_class_cd, ";
            sql = sql + "item.status, item_type.descs AS type_descs, item_class.descs AS class_descs, item_category.descs AS category_descs, item_uom.descs AS uom_descs ";
            sql = sql + "FROM            item INNER JOIN ";
            sql = sql + "item_type ON item.item_type_cd = item_type.code INNER JOIN ";
            sql = sql + "item_category ON item.item_category_cd = item_category.code AND item.item_class_cd = item_category.item_class_cd INNER JOIN ";
            sql = sql + "item_uom ON item.uom = item_uom.code INNER JOIN ";
            sql = sql + "item_class ON item.item_class_cd = item_class.code ";
            if (!string.IsNullOrEmpty(_search))
            {
                sql = sql + "WHERE  (item.item_cd + ' ' + item.descs + ' ' + item_type.descs + ' ' + item_class.descs + ' ' + item_category.descs + ' ' + item_uom.descs LIKE '%" + _search + "%') ";
            }
            //sql = sql & "ORDER BY item.descs "

            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader dtr = default(SqlDataReader);
            cn.Open();
            dtr = cmd.ExecuteReader();

            List<MasterfileField> _list = new List<MasterfileField>();

            while (dtr.Read())
            {
                MasterfileField _fields = new MasterfileField();

                _fields._Code = dtr["item_cd"].ToString().Trim();
                _fields._Description = dtr["descs"].ToString().Trim();
                _fields._UOM = dtr["uom_descs"].ToString().Trim();
                _fields._Standard_Cost = Convert.ToDouble(dtr["standard_cost"]).ToString("#,##0.0000");
                _fields._Latest_Cost = Convert.ToDouble(dtr["latest_cost"]).ToString("#,##0.0000");
                _fields._Weight = dtr["item_weight"].ToString().Trim();
                _fields._Type = dtr["type_descs"].ToString().Trim();
                _fields._Class = dtr["class_descs"].ToString().Trim();
                _fields._Category = dtr["category_descs"].ToString().Trim();
                _fields._Status = (Convert.ToBoolean(dtr["status"]) == false ? "In-Active" : "Active");
                _list.Add(_fields);

            }

            dtr.Close();
            cn.Close();
            cn.Dispose();

            return _list;
       
        }
        else if (_function == "f5")
        {

            sql = "SELECT        item_uom_convesion_table.uom_cd_from, item_uom_convesion_table.uom_cd_to, item_uom_convesion_table.conversion_factor ";
            sql = sql + "FROM            item_uom_convesion_table INNER JOIN ";
            sql = sql + "item_uom ON item_uom_convesion_table.uom_cd_from = item_uom.code ";
            sql = sql + "WHERE        (item_uom_convesion_table.status = 1) ";

            if (!string.IsNullOrEmpty(_search))
            {
                sql = sql + "AND (RTRIM(item_uom_convesion_table.uom_cd_from) + ' - ' + RTRIM(item_uom_convesion_table.uom_cd_to) + ' - ' + item_uom.descs LIKE '%" + _search + "%') ";
            }

            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader dtr = default(SqlDataReader);
            cn.Open();
            dtr = cmd.ExecuteReader();

            List<MasterfileField> _list = new List<MasterfileField>();

            while (dtr.Read())
            {
                MasterfileField _fields = new MasterfileField();

                _fields._UOM = dtr["uom_cd_from"].ToString().Trim();
                _fields._UOM_To = dtr["uom_cd_to"].ToString().Trim();
                _fields._Weight = dtr["conversion_factor"].ToString().Trim();
                _list.Add(_fields);

            }

            dtr.Close();
            cn.Close();
            cn.Dispose();

            return _list;
        

        }
        else if (_function == "f6")
        {
            sql = "SELECT        stock_card.company_cd, stock_card.warehouse_cd, stock_card.trx_type, stock_card.doc_no, stock_card.item_sequence, stock_card.doc_date, stock_card.doc_descs, stock_card.status, stock_card.item_cd, stock_card.item_weight, ";
            sql = sql + "stock_card.uom, stock_card.qty, stock_card.unit_cost, stock_card.total_cost, stock_card.lot_no, stock_card.mfg_date, stock_card.expiry_date, stock_card.uom_conversion, stock_card.item_type_cd, ";
            sql = sql + "stock_card.item_class_cd, stock_card.item_category_cd, item.descs ";
            sql = sql + "FROM            (SELECT        company_cd, warehouse_cd, trx_type, doc_no, item_sequence, doc_date, descs AS doc_descs, status, item_cd, item_weight, uom, qty, unit_cost, total_cost, lot_no, mfg_date, expiry_date, uom_conversion,  ";
            sql = sql + "item_type_cd, item_class_cd, item_category_cd ";
            sql = sql + "FROM            stock_card AS stock_card_1 ";
            sql = sql + "UNION ";
            sql = sql + "SELECT        company_cd, warehouse_cd, trx_type, doc_no, item_sequence, doc_date, descs AS doc_descs, status, item_cd, item_weight, uom, qty, unit_cost, total_cost, lot_no, mfg_date, expiry_date, uom_conversion,  ";
            sql = sql + "item_type_cd, item_class_cd, item_category_cd ";
            sql = sql + "FROM            stock_card_posted) AS stock_card INNER JOIN ";
            sql = sql + "item ON stock_card.item_cd = item.item_cd AND stock_card.item_type_cd = item.item_type_cd AND stock_card.item_category_cd = item.item_category_cd AND stock_card.item_class_cd = item.item_class_cd ";

            sql = sql + "WHERE (stock_card.doc_no is not null) ";
            
            if (_warehouse_cd != "All")
            {
                sql = sql + " AND (stock_card.company_cd = '" + HttpContext.Current.Session["company_code"] + "') ";
                sql = sql + " AND (stock_card.warehouse_cd = '" + _warehouse_cd + "') ";
            }

            if (_status != "All")
            {
                if (_status == "Expired")
                {
                    sql = sql + "AND (stock_card.status IN ('Approved', 'Draft', 'Posted')) ";
                    sql = sql + " AND (stock_card.expiry_date < '" + System.DateTime.Now.ToShortDateString() + "') ";
                }
                else
                {
                    sql = sql + " AND (stock_card.status = '" + _status + "') ";
                }
            }
            else if (_status == "All")
            {
                sql = sql + "AND (stock_card.status IN ('Approved', 'Draft', 'Posted')) ";
            }
            //If _date_from <> "" And _date_to <> "" Then
            //    sql = sql & " AND (stock_card.doc_date between '" & _date_from & " 00:00:00' AND '" & _date_to & " 23:59:59') "
            //End If
            if (_doc_no != "All")
            {
                sql = sql + " AND (stock_card.doc_no = '" + _doc_no + "') ";
            }
            if (_item_cd != "All")
            {
                sql = sql + " AND (stock_card.item_cd = '" + _item_cd + "') ";
            }

            sql = sql + "ORDER BY stock_card.doc_date, stock_card.trx_type, stock_card.doc_no, stock_card.item_sequence ";

            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader dtr = default(SqlDataReader);
            cn.Open();
            dtr = cmd.ExecuteReader();

            List<MasterfileField> _list = new List<MasterfileField>();

            while (dtr.Read())
            {
                MasterfileField _fields = new MasterfileField();

                _fields._trx_type = dtr["trx_type"].ToString().Trim();
                _fields._doc_no = dtr["doc_no"].ToString().Trim();
                _fields._doc_date = Convert.ToDateTime(dtr["doc_date"].ToString().Trim()).ToShortDateString();
                _fields._doc_descs = dtr["doc_descs"].ToString().Trim();
                _fields._Status = dtr["status"].ToString().Trim();
                _fields._item_cd = dtr["item_cd"].ToString().Trim();
                _fields._descs = dtr["descs"].ToString().Trim();
                _fields._item_weight = dtr["item_weight"].ToString().Trim();
                _fields._UOM = dtr["uom"].ToString().Trim() + (string.IsNullOrEmpty(dtr["uom_conversion"].ToString().Trim()) ? "" : " / " + dtr["uom_conversion"].ToString().Trim());
                _fields._qty = Convert.ToDouble(dtr["qty"]).ToString("#,##0.0000");
                _fields._unit_cost = Convert.ToDouble(dtr["unit_cost"]).ToString("#,##0.0000");
                _fields._total_cost = Convert.ToDouble(dtr["total_cost"]).ToString("#,##0.0000");
                _fields._lot_no = dtr["lot_no"].ToString().Trim();
                _fields._mfg_date = (Convert.ToDateTime(dtr["mfg_date"].ToString().Trim()) == Convert.ToDateTime("1/1/1900") ? "" : Convert.ToDateTime(dtr["mfg_date"].ToString().Trim()).ToShortDateString());
                _fields._expiry_date = Convert.ToDateTime(dtr["expiry_date"].ToString().Trim()).ToShortDateString();
                _list.Add(_fields);
            }

            dtr.Close();
            cn.Close();
            cn.Dispose();

            return _list;
        }
        else
        {
            sql = "SELECT code, descs, status FROM " + HttpContext.Current.Session["table_name"] + " ";
            if (_function == "f1")
            {
                if (!string.IsNullOrEmpty(_search))
                {
                    sql = sql + "WHERE descs like '%" + _search + "%' ";
                }
            }
            else if (_function == "f2")
            {
                sql = sql + "WHERE " + _Group_field + " = '" + _Group_entry + "' ";
                if (!string.IsNullOrEmpty(_search))
                {
                    sql = sql + "AND descs like '%" + _search + "%' ";
                }
            }
            //sql = sql & "ORDER BY descs"

            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader dtr = default(SqlDataReader);
            cn.Open();
            dtr = cmd.ExecuteReader();

            List<MasterfileField> _list = new List<MasterfileField>();

            while (dtr.Read())
            {
                MasterfileField _fields = new MasterfileField();

                _fields._Code = dtr["code"].ToString().Trim();
                _fields._Description = dtr["descs"].ToString().Trim();
                _fields._Status = (Convert.ToBoolean(dtr["status"]) == false ? "In-Active" : "Active");
                _list.Add(_fields);
            }

            dtr.Close();
            cn.Close();
            cn.Dispose();

            return _list;
        }
        return null;      
    }

    [WebMethod(EnableSession = true)]
    public string _searchuser(string _username)
    { 
        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            return null;
        }


        string sql = "";

        sql = "SELECT        cf_users.username, cf_users.employee_id, cf_users.name, cf_users.department_code, ";
        sql = sql + "cf_users.group_access, cf_users.status, cf_users.first_name, cf_users.last_name, cf_users.middle_name, ";
        sql = sql + "cf_users.suffix, ";
        sql = sql + "cf_department.descs AS dept_descs, cf_group_access.descs AS group_access_descs, cf_users.access_level ";
        sql = sql + "FROM            cf_users INNER JOIN ";
        sql = sql + "cf_group_access ON cf_users.group_access = cf_group_access.code INNER JOIN ";
        sql = sql + "cf_department ON cf_users.department_code = cf_department.code ";

        sql = sql + "WHERE cf_users.username = '" + _username + "' ";

        // sql = sql & "ORDER BY cf_users.last_name, cf_users.first_name, cf_users.middle_name, cf_users.suffix "

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, cn);
        SqlDataReader dtr = default(SqlDataReader);

        cn.Open();
        dtr = cmd.ExecuteReader();
        string _return = "";
        if (dtr.Read())
        {
            _return = dtr["username"].ToString().Trim();
            _return = _return + "," + dtr["first_name"].ToString().Trim();
            _return = _return + "," + dtr["last_name"].ToString().Trim();
            _return = _return + "," + dtr["middle_name"].ToString().Trim();
            _return = _return + "," + dtr["suffix"].ToString().Trim();
            _return = _return + "," + dtr["employee_id"].ToString().Trim();
            _return = _return + "," + dtr["department_code"].ToString().Trim();
            _return = _return + "," + dtr["group_access"].ToString().Trim();
            _return = _return + "," + dtr["access_level"].ToString().Trim();

        }
        cn.Close();

        return _return;
        

    }

    [WebMethod(EnableSession = true)]
    public string _searchitem(string _item_code)
    {
      


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return null;
                
            }
        }
        catch (Exception ex)
        {
            return null;
            
        }


        string sql = "SELECT item_cd, descs, uom, item_type_cd, item_category_cd, remarks, standard_cost, latest_cost, ";
        sql = sql + "item_weight, audit_user, audit_date, item_class_cd, status ";
        sql = sql + "FROM item ";
        sql = sql + "WHERE (item_cd = '" + _item_code + "') ";
        // sql = sql & "ORDER BY descs "

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, cn);
        SqlDataReader dtr = default(SqlDataReader);

        cn.Open();
        dtr = cmd.ExecuteReader();
        string _return = "";
        if (dtr.Read())
        {
            _return = dtr["item_cd"].ToString().Trim();
            _return = _return + "," + dtr["descs"].ToString().Trim();
            _return = _return + "," + dtr["uom"].ToString().Trim();
            _return = _return + "," + dtr["item_type_cd"].ToString().Trim();
            _return = _return + "," + dtr["item_category_cd"].ToString().Trim();
            _return = _return + "," + dtr["remarks"].ToString().Trim();
            _return = _return + "," + dtr["standard_cost"].ToString().Trim();
            _return = _return + "," + dtr["latest_cost"].ToString().Trim();
            _return = _return + "," + dtr["item_weight"].ToString().Trim();
            _return = _return + "," + dtr["item_class_cd"].ToString().Trim();
            _return = _return + "," + dtr["status"].ToString().Trim();
        }
        cn.Close();

        return _return;
        
    }

    [WebMethod(EnableSession = true)]
    public string Save_Masterfile_User(string _username, string _firstname, string _lastname, string _middlename, string _suffix, string _employeeid, string _dept, string _group, int _password, int _status,
    string xhidden_entry, string _level)
    {


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
           
        }


        if (_username.Replace("undefined", string.Empty) == string.Empty |
            _firstname.Replace("undefined", string.Empty) == string.Empty |
            _lastname.Replace("undefined", string.Empty) == string.Empty |
            _middlename.Replace("undefined", string.Empty) == string.Empty | 
            _employeeid.Replace("undefined", string.Empty) == string.Empty | 
            _dept.Replace("undefined", string.Empty) == string.Empty | 
            _group.Replace("undefined", string.Empty) == string.Empty)
        {
            return "<span style='color:red'>Please input the required values...</span>";
        }


        string x = null;
        string audit_date = System.DateTime.Now.ToString();


        if (xhidden_entry == "new")
        {

            string sql = "SELECT username FROM cf_users WHERE (username = '" + _username + "') AND (employee_id = '" + _employeeid + "')";
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader dtr;

            cn.Open();
            dtr = cmd.ExecuteReader();
            bool sfind = false;
            if (dtr.Read())
            {
                sfind = true;
            }
            cn.Close();


            if (sfind == false)
            {
                sql = "INSERT INTO cf_users ";
                sql = sql + "(username, password, employee_id, name, department_code, group_access, status, first_name, last_name, middle_name, suffix, access_level) ";
                sql = sql + "VALUES('" + _username.Replace(",", string.Empty).Replace("'", "''") + "', ";
                //If _password = 1 Then
                sql = sql + "'" + Security.Ascii("123456") + "', ";
                //End If
                sql = sql + "'" + _employeeid + "', ";
                sql = sql + "'" + _firstname.Replace(",", string.Empty).Replace("'", "''").Trim() + " " + _middlename.Replace(",", string.Empty).Replace("'", "''").Trim() + " " + _lastname.Replace(",", string.Empty).Replace("'", "''").Trim() + "', ";
                sql = sql + "'" + _dept + "', ";
                sql = sql + "'" + _group + "', ";
                sql = sql + _status + ", ";
                sql = sql + "'" + _firstname.Replace(",", string.Empty).Replace("'", "''") + "', ";
                sql = sql + "'" + _lastname.Replace(",", string.Empty).Replace("'", "''") + "', ";
                sql = sql + "'" + _middlename.Replace(",", string.Empty).Replace("'", "''") + "', ";
                sql = sql + "'" + _suffix + "',";
                sql = sql + "'" + _level + "'";
                sql = sql + ") ";

                cmd = new SqlCommand(sql, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                cn.Dispose();

                x = "New Record saved... <br>NAME : " + _firstname.Trim() + " " + _middlename.Trim() + " " + _lastname.Trim();

            }
            else
            {
                x = "<span style='color:red'>Duplicated Record found.... <br>NAME : " + _firstname.Trim() + " " + _middlename.Trim() + " " + _lastname.Trim() + "</span>";

            }


        }
        else
        {
            string sql = "UPDATE cf_users ";
            sql = sql + "SET ";
            if (_password == 1)
            {
                sql = sql + "password = '" + Security.Ascii("123456") + "', ";
            }
            sql = sql + "name = '" + _firstname.Replace(",", string.Empty).Replace("'", "''").Trim() + " " + _middlename.Replace(",", string.Empty).Replace("'", "''").Trim() + " " + _lastname.Replace(",", string.Empty).Replace("'", "''").Trim() + "', ";
            sql = sql + "department_code = '" + _dept + "', ";
            sql = sql + "group_access = '" + _group + "', ";
            sql = sql + "status = " + _status + ", ";
            sql = sql + "first_name = '" + _firstname.Replace(",", string.Empty).Replace("'", "''") + "', ";
            sql = sql + "last_name = '" + _lastname.Replace(",", string.Empty).Replace("'", "''") + "', ";
            sql = sql + "middle_name = '" + _middlename.Replace(",", string.Empty).Replace("'", "''") + "', ";
            sql = sql + "suffix = '" + _suffix + "',";
            sql = sql + "access_level = '" + _level + "'";
            sql = sql + "WHERE username = '" + _username + "' ";
            sql = sql + "AND employee_id = '" + _employeeid + "' ";

            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
            cn.Dispose();

            x = "Record updated.... <br>NAME : " + _firstname.Trim() + " " + _middlename.Trim() + " " + _lastname.Trim();

        }

        return x;
    }

    [WebMethod(EnableSession = true)]
    public string Save_Masterfile_Item(string _item_cd, string _descs, string _uom, string _item_type_cd, string _item_category_cd, string _remarks, string _standard_cost, string _latest_cost, string _item_weight, string _item_class_cd,
    int _status, string xhidden_entry)
    {


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
        }

        if (_item_cd.Replace("undefined", string.Empty) == string.Empty | _descs.Replace("undefined", string.Empty) == string.Empty | _item_type_cd.Replace("undefined", string.Empty) == string.Empty | _item_category_cd.Replace("undefined", string.Empty) == string.Empty | _item_class_cd.Replace("undefined", string.Empty) == string.Empty)
        {
            //_standard_cost.Replace("undefined", 0) = 0 Or
            //_item_weight.Replace("undefined", 0) = 0 Then

            return "<span style='color:red'>Please input the required values...</span>";
            
        }

        string x = null;
        string audit_date = System.DateTime.Now.ToString();


        if (xhidden_entry == "new")
        {
            string sql = "SELECT item_cd FROM item WHERE (item_cd = '" + _item_cd + "') ";
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader dtr;

            cn.Open();
            dtr = cmd.ExecuteReader();
            bool sfind = false;
            if (dtr.Read())
            {
                sfind = true;
            }
            cn.Close();


            if (sfind == false)
            {
                sql = "INSERT INTO item ";
                sql = sql + "(item_cd, descs, uom, item_type_cd, item_category_cd, remarks, standard_cost, latest_cost, item_weight, ";
                sql = sql + "audit_user, audit_date, item_class_cd, status) ";
                sql = sql + " VALUES( ";
                sql = sql + "'" + _item_cd + "', ";
                sql = sql + "'" + _descs.Replace(",", string.Empty).Replace("'", "''") + "', ";
                sql = sql + "'" + _uom + "', ";
                sql = sql + "'" + _item_type_cd + "', ";
                sql = sql + "'" + _item_category_cd + "', ";
                sql = sql + "'" + _remarks.Replace(",", string.Empty).Replace("'", "''") + "', ";
                sql = sql + Convert.ToDouble(_standard_cost.Replace(",", string.Empty)).ToString() + ", ";
                sql = sql + Convert.ToDouble(_latest_cost.Replace(",", string.Empty)).ToString() + ", ";
                sql = sql + Convert.ToDouble(_item_weight.Replace(",", string.Empty)).ToString() + ", ";
                sql = sql + "'" + HttpContext.Current.Session["username"] + "', ";
                sql = sql + "'" + audit_date + "', ";
                sql = sql + "'" + _item_class_cd + "', ";
                sql = sql + _status + " ";
                sql = sql + ") ";

                cmd = new SqlCommand(sql, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                cn.Dispose();

                x = "New Record saved... <br>ITEM : " + _descs;

            }
            else
            {
                x = "<span style='color:red'>Duplicated Record found.... <br>ITEM : " + _descs + "</span>";

            }


        }
        else
        {
            string sql = "UPDATE item ";
            sql = sql + "SET ";
            sql = sql + "descs = '" + _descs.Replace("'", "''") + "', ";
            sql = sql + "uom = '" + _uom + "', ";
            sql = sql + "item_type_cd = '" + _item_type_cd + "', ";
            sql = sql + "item_category_cd = '" + _item_category_cd + "', ";
            sql = sql + "remarks = '" + _remarks + "', ";
            sql = sql + "standard_cost = " + _standard_cost + ", ";
            sql = sql + "latest_cost = " + _latest_cost + ", ";
            sql = sql + "item_weight = " + _item_weight + ", ";
            sql = sql + "audit_user = '" + HttpContext.Current.Session["username"] + "', ";
            sql = sql + "audit_date = '" + audit_date + "', ";
            sql = sql + "item_class_cd = '" + _item_class_cd + "', ";
            sql = sql + "status = " + _status + " ";
            sql = sql + "WHERE item_cd = '" + _item_cd + "' ";


            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
            cn.Dispose();

            x = "Record updated.... <br>ITEM : " + _descs;

        }

        return x;
    }

    [WebMethod(EnableSession = true)]
    public string BindDropdownlist(string _class_cd)
    {        
        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return null;
               
            }
        }
        catch (Exception ex)
        {
            return null;
       
        }

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter("SELECT code, descs FROM item_category WHERE (status = 1) AND (item_class_cd = '" + _class_cd + "') ORDER BY descs", cn);
        cn.Open();
        da.Fill(dt);
        cn.Close();

        List<List_Item> listitem = new List<List_Item>();

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                List_Item objst = new List_Item();
                objst.Code = dt.Rows[i]["code"].ToString().Trim();
                objst.Descs = dt.Rows[i]["descs"].ToString().Trim();
                listitem.Insert(i, objst);

            }
        }
        JavaScriptSerializer jscript = new JavaScriptSerializer();
        return jscript.Serialize(listitem);
    }

    [WebMethod(EnableSession = true)]
    public string Password_Changed(string _username, string _password)
    {


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
               
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
          
        }


        if (_password.Replace("undefined", string.Empty) == string.Empty)
        {
            return "<span style='color:red'>Please input the required values...</span>";
          
        }

        string sql = "UPDATE cf_users ";
        sql = sql + "SET ";
        sql = sql + "password = '" + Security.Ascii(_password) + "' ";
        sql = sql + "WHERE username = '" + _username + "' ";

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, cn);
        cn.Open();
        cmd.ExecuteNonQuery();
        cn.Close();
        cn.Dispose();

        return "Password Changed, Please Log-in again to continue...";
    }



    [WebMethod(EnableSession = true)]
    public string Save_Masterfile_UOM(string _uom_from, string _uom_to, string _factor, string xhidden_entry)
    {


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
               
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
          
        }

        if (_uom_from.Replace("undefined", string.Empty) == string.Empty | _uom_to.Replace("undefined", string.Empty) == string.Empty | _factor.Replace("undefined", string.Empty) == string.Empty)
        {
            return "<span style='color:red'>Please input the required values...</span>";
           

        }


        string x = null;
        string audit_date = System.DateTime.Now.ToString();


        if (xhidden_entry == "new")
        {
            string sql = "SELECT uom_cd_from FROM item_uom_convesion_table WHERE        (status = 1) AND (uom_cd_from = '" + _uom_from + "') AND (uom_cd_to = '" + _uom_to + "')";
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader dtr;

            cn.Open();
            dtr = cmd.ExecuteReader();
            bool sfind = false;
            if (dtr.Read())
            {
                sfind = true;
            }
            cn.Close();


            if (sfind == false)
            {
                sql = "INSERT INTO item_uom_convesion_table ";
                sql = sql + "(uom_cd_from, uom_cd_to, conversion_factor, audit_user, audit_date, status) ";
                sql = sql + "VALUES('" + _uom_from + "', ";

                sql = sql + "'" + _uom_to + "', ";
                sql = sql + "" + Convert.ToDouble(_factor.Replace(",", string.Empty)) + ", ";
                sql = sql + "'" + HttpContext.Current.Session["username"] + "', ";
                sql = sql + "'" + System.DateTime.Now.ToString() + "', ";
                sql = sql + "1 ";
                sql = sql + ") ";

                cmd = new SqlCommand(sql, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                cn.Dispose();

                x = "New Record saved... <br>CONVERSION FROM : " + _uom_from + " TO : " + _uom_to;

            }
            else
            {
                x = "<span style='color:red'>Duplicated Record found....  <br>CONVERSION FROM : " + _uom_from + " TO : " + _uom_to;

            }


        }
        else
        {
            string sql = "UPDATE item_uom_convesion_table ";
            sql = sql + "SET ";

            sql = sql + "conversion_factor = " + Convert.ToDouble(_factor.Replace(",", string.Empty)) + ", ";
            sql = sql + "audit_user = '" + HttpContext.Current.Session["username"] + "', ";
            sql = sql + "audit_date = '" + System.DateTime.Now.ToString() + "' ";
            sql = sql + "WHERE uom_cd_from = '" + _uom_from + "' ";
            sql = sql + "AND uom_cd_to = '" + _uom_to + "' ";

            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
            cn.Dispose();

            x = "Record updated.... <br>CONVERSION FROM : " + _uom_from + " TO : " + _uom_to;

        }

        return x;
    }

    #endregion

    #region "Inventory"
    [WebMethod(EnableSession = true)]
    public string Set_SessionVariable(string _warehouse_cd)
    {

        HttpContext.Current.Session["warehouse_cd"] = _warehouse_cd;
        return null;

    }

    [WebMethod(EnableSession = true)]
    public string BindInventoryReportDocument(string _obj, string _warehouse_cd, string _date_from, string _date_to, string _status)
    {

        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        DataTable dt = new DataTable();
        string sql = "";

        List<List_Item> listitem = new List<List_Item>();
        List_Item objst = new List_Item();

        int ii = 0;

        switch (_obj)
        {

            case "#ContentPlaceHolder1_DDDocuments":

                sql = "SELECT DISTINCT  TOP (20) doc_no AS code, Rtrim(doc_no) + ' - ' + Rtrim(descs)  + ' - ' + Rtrim(status) as descs ";
                sql = sql + "FROM            stock_card ";
                sql = sql + "WHERE        (company_cd = '" + HttpContext.Current.Session["company_code"] + "') ";
                bool allow_wh = true;
                if (_warehouse_cd == "undefined")
                {
                    allow_wh = false;
                }
                if (_warehouse_cd.Trim() == "")
                {
                    allow_wh = false;
                }
                if (allow_wh == true)
                {
                    sql = sql + "AND (warehouse_cd = '" + _warehouse_cd + "') ";
                }

                sql = sql + "AND (trx_type = '" + HttpContext.Current.Session["trx_type"] + "') ";
                if (_date_from == "")
                {
                    _date_from = System.DateTime.Now.ToShortDateString();
                }
                if (_date_to == "")
                {
                    _date_to = System.DateTime.Now.ToShortDateString();
                }
                sql = sql + "AND (doc_date BETWEEN '" + _date_from + " 00:00:00' AND '" + _date_to + " 23:59:59') ";
                allow_wh = true;
                if (_status == "All")
                {
                    allow_wh = false;
                }
                if (_status == "")
                {
                    allow_wh = false;
                }
                if (allow_wh == true)
                {
                    sql = sql + "AND (status = '" + _status + "') ";
                }
                sql = sql + "ORDER BY code asc, descs asc ";

                objst = new List_Item();
                objst.Code = "All";
                objst.Descs = "All";
                listitem.Insert(ii, objst);
                ii += 1;

                break;

        }


        SqlDataAdapter da = new SqlDataAdapter(sql, cn);
        cn.Open();
        da.Fill(dt);
        cn.Close();



        if (dt.Rows.Count > 0)
        {

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                objst = new List_Item();
                objst.Code = dt.Rows[i]["code"].ToString().Trim();
                objst.Descs = dt.Rows[i]["descs"].ToString().Trim();
                listitem.Insert(ii, objst);

            }
        }
        JavaScriptSerializer jscript = new JavaScriptSerializer();
        return jscript.Serialize(listitem);
    }

    [WebMethod(EnableSession = true)]
    public string BindInventoryDropdownlist_Report(string _obj, string _warehouse_cd, string _status, string _doc_no)
    {

        //ByVal _date_from As String,
        //                                               ByVal _date_to As String,
        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        string sql = "";

        List<List_Item> listitem = new List<List_Item>();
        List_Item objst = new List_Item();

        int ii = 0;

        objst = new List_Item();

        objst.Code = "All";
        objst.Descs = "All";

        listitem.Insert(ii, objst);
        ii += 1;

     
        switch (_obj)
        {
            case "#ContentPlaceHolder1_DDReport_DocNo":

                sql = "SELECT DISTINCT doc_no as code, rtrim(doc_no) + ' - '+ rtrim(descs) + ' / '+ rtrim(status) as _descs, status FROM ";
                sql = sql + "(SELECT doc_no, descs, status, company_cd, warehouse_cd, expiry_date ";
                sql = sql + "FROM            stock_card ";
                sql = sql + "UNION ";
                sql = sql + "SELECT doc_no, descs, status, company_cd, warehouse_cd, expiry_date ";
                sql = sql + "FROM            stock_card_posted) as stock_card ";

                sql = sql + "WHERE        (doc_no IS NOT NULL) ";

                if (_warehouse_cd != "All")
                {
                    sql = sql + " AND (company_cd = '" + HttpContext.Current.Session["company_code"] + "') ";
                    sql = sql + " AND (warehouse_cd = '" + _warehouse_cd + "') ";
                }

                if (_status != "All")
                {
                    if (_status == "Expired")
                    {
                        sql = sql + "AND (status IN ('Approved', 'Draft', 'Posted')) ";
                        sql = sql + " AND (expiry_date < '" + System.DateTime.Now.ToShortDateString() + "') ";
                    }
                    else
                    {
                        sql = sql + " AND (status = '" + _status + "') ";
                    }

                }
                else if (_status == "All")
                {
                    sql = sql + "AND (status IN ('Approved', 'Draft', 'Posted')) ";
                }

                sql = sql + "ORDER BY status, _descs ";

                break;

            case "#ContentPlaceHolder1_DDReport_Item_Code":
                sql = "SELECT DISTINCT stock_card.item_cd AS code, item.descs AS _descs ";
                sql = sql + "FROM            (SELECT        item_cd, company_cd, warehouse_cd, doc_no, status, item_type_cd, item_class_cd, item_category_cd, expiry_date ";
                sql = sql + "FROM            stock_card AS stock_card_1 ";
                sql = sql + "UNION ";
                sql = sql + "SELECT        item_cd, company_cd, warehouse_cd, doc_no, status, item_type_cd, item_class_cd, item_category_cd, expiry_date ";
                sql = sql + "FROM            stock_card_posted) AS stock_card INNER JOIN ";
                sql = sql + "item ON stock_card.item_cd = item.item_cd AND stock_card.item_type_cd = item.item_type_cd AND stock_card.item_category_cd = item.item_category_cd AND ";
                sql = sql + "stock_card.item_class_cd = item.item_class_cd ";
                //sql = sql & "WHERE        (stock_card.warehouse_cd = '00011') AND (stock_card.doc_no IS NOT NULL) AND (stock_card.company_cd = '07') AND (stock_card.status IN ('Approved', 'Draft', 'Posted')) AND (stock_card.doc_no = 'GR00000043') "
                //sql = sql & "ORDER BY _descs "

                //sql = "SELECT    DISTINCT    stock_card.item_cd as code, item.descs as _descs  "
                //sql = sql & "FROM            stock_card INNER JOIN "
                //sql = sql & "item ON stock_card.item_cd = item.item_cd AND stock_card.item_type_cd = item.item_type_cd AND stock_card.item_category_cd = item.item_category_cd AND "
                //sql = sql & "stock_card.item_class_cd = item.item_class_cd "
                sql = sql + "WHERE        (stock_card.doc_no IS NOT NULL) ";

                sql = sql + " AND (stock_card.company_cd = '" + HttpContext.Current.Session["company_code"] + "') ";

                if (_warehouse_cd != "All")
                {
                    sql = sql + " AND (stock_card.warehouse_cd = '" + _warehouse_cd + "') ";
                }

                if (_status != "All")
                {
                    if (_status == "Expired")
                    {
                        sql = sql + "AND (stock_card.status IN ('Approved', 'Draft', 'Posted')) ";
                        sql = sql + " AND (stock_card.expiry_date < '" + System.DateTime.Now.ToShortDateString() + "') ";
                    }
                    else
                    {
                        sql = sql + " AND (stock_card.status = '" + _status + "') ";
                    }

                }
                else if (_status == "All")
                {
                    sql = sql + "AND (stock_card.status IN ('Approved', 'Draft', 'Posted')) ";
                }
                //If _date_from <> "" And _date_to <> "" Then
                //    sql = sql & " AND (stock_card.doc_date between '" & _date_from & " 00:00:00' AND '" & _date_to & " 23:59:59') "
                //End If

                if (_doc_no != "All")
                {
                    sql = sql + " AND (stock_card.doc_no = '" + _doc_no + "') ";
                }

                sql = sql + "ORDER BY _descs ";

                break;

        }


        da = new SqlDataAdapter(sql, cn);
        cn.Open();
        da.Fill(dt);
        cn.Close();

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                bool sfind = false;
                for (int index = 0; index <= listitem.Count - 1; index++)
                {
                    if (dt.Rows[i]["code"].ToString().Trim() == listitem[index].Code)
                    {
                        sfind = true;
                    }
                }

                if (sfind == false)
                {
                    objst = new List_Item();

                    objst.Code = dt.Rows[i]["code"].ToString().Trim();
                    objst.Descs = dt.Rows[i]["_descs"].ToString().Trim();

                    listitem.Insert(ii, objst);
                    ii += 1;
                }
            }
        }

        JavaScriptSerializer jscript = new JavaScriptSerializer();
        return jscript.Serialize(listitem);
    }

    [WebMethod(EnableSession = true)]
    public string BindInventoryDropdownlist(string _obj)
    {

        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        DataTable dt = new DataTable();
        string sql = "";

        List<List_Item> listitem = new List<List_Item>();
        List_Item objst = new List_Item();

        int ii = 0;

        switch (_obj)
        {
            case "#ContentPlaceHolder1_DDepartment":
                sql = "SELECT code, descs FROM cf_department WHERE (status = 1)  AND (code = '" + HttpContext.Current.Session["department_code"] + "') ORDER BY descs";

                break;
            case "#ContentPlaceHolder1_DDivision":
                sql = "SELECT code, descs FROM cf_division WHERE (status = 1) ORDER BY descs";
                objst = new List_Item();
                objst.Code = "";
                objst.Descs = "";
                listitem.Insert(ii, objst);
                ii += 1;

                break;
            case "#ContentPlaceHolder1_DDWarehouse":
                sql = "SELECT        cf_warehouse.code, cf_warehouse.descs ";
                sql = sql + "FROM            cfs_user_warehouse INNER JOIN ";
                sql = sql + "cf_warehouse ON cfs_user_warehouse.warehouse_code = cf_warehouse.code ";
                sql = sql + "WHERE        (cfs_user_warehouse.username = '" + HttpContext.Current.Session["username"] + "') ";
                sql = sql + "ORDER BY cf_warehouse.descs ";

                break;
            case "#ContentPlaceHolder1_DDReport_Company":
                sql = "SELECT        cf_company.code, cf_company.descs ";
                sql = sql + "FROM            cf_company INNER JOIN ";
                sql = sql + "cfs_user_company ON cf_company.code = cfs_user_company.company_code ";
                sql = sql + "WHERE        (cf_company.status = 1) AND (cfs_user_company.username = '" + HttpContext.Current.Session["username"] + "') ";
                sql = sql + "ORDER BY cf_company.descs asc ";

                objst = new List_Item();
                objst.Code = "All";
                objst.Descs = "All";
                listitem.Insert(ii, objst);
                ii += 1;

                break;
            case "#ContentPlaceHolder1_DDReport_Warehouse":
                sql = "SELECT        cf_warehouse.code, cf_warehouse.descs ";
                sql = sql + "FROM            cfs_user_warehouse INNER JOIN ";
                sql = sql + "cf_warehouse ON cfs_user_warehouse.warehouse_code = cf_warehouse.code ";
                sql = sql + "WHERE        (cfs_user_warehouse.username = '" + HttpContext.Current.Session["username"] + "') ";
                sql = sql + "ORDER BY cf_warehouse.descs asc ";

                break;
            //objst = New List_Item()
            //objst.Code = "All"
            //objst.Descs = "All"
            //listitem.Insert(ii, objst)
            //ii += 1
            case "#ContentPlaceHolder1_DDTransfer_Company":
                sql = "SELECT        cf_company.code, cf_company.descs ";
                sql = sql + "FROM            cf_company INNER JOIN ";
                sql = sql + "cfs_user_company ON cf_company.code = cfs_user_company.company_code ";
                sql = sql + "WHERE        (cf_company.status = 1) AND (cfs_user_company.username = '" + HttpContext.Current.Session["username"] + "') ";
                sql = sql + "ORDER BY cf_company.descs asc ";

                break;
            case "#ContentPlaceHolder1_DDTransfer_Warehouse":
                sql = "SELECT        cf_warehouse.code, cf_warehouse.descs ";
                sql = sql + "FROM            cfs_user_warehouse INNER JOIN ";
                sql = sql + "cf_warehouse ON cfs_user_warehouse.warehouse_code = cf_warehouse.code ";
                sql = sql + "WHERE        (cfs_user_warehouse.username = '" + HttpContext.Current.Session["username"] + "') ";
                sql = sql + "ORDER BY cf_warehouse.descs asc ";

                break;
            case "#ContentPlaceHolder1_DDUOMFrom":
            case "#ContentPlaceHolder1_DDUOMTo":

                sql = "SELECT        code, RTRIM(code) + ' - ' + RTRIM(descs) AS _descs ";
                sql = sql + "FROM            item_uom ";
                sql = sql + "WHERE        (status = 1) ";
                sql = sql + "ORDER BY descs DESC ";

                break;
        }


        SqlDataAdapter da = new SqlDataAdapter(sql, cn);
        cn.Open();
        da.Fill(dt);
        cn.Close();



        if (dt.Rows.Count > 0)
        {

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                objst = new List_Item();
                switch (_obj)
                {
                    case "#ContentPlaceHolder1_DDUOMFrom":
                    case "#ContentPlaceHolder1_DDUOMTo":

                        objst.Code = dt.Rows[i]["code"].ToString().Trim();
                        objst.Descs = dt.Rows[i]["_descs"].ToString().Trim();

                        break;
                    default:

                        objst.Code = dt.Rows[i]["code"].ToString().Trim();
                        objst.Descs = dt.Rows[i]["descs"].ToString().Trim();

                        break;
                }
                listitem.Insert(ii, objst);
                ii += 1;
            }
        }
        JavaScriptSerializer jscript = new JavaScriptSerializer();
        return jscript.Serialize(listitem);
    }

    [WebMethod(EnableSession = true)]
    public string BindInventoryConversion(string _obj, string _uom)
    {

        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }

        if (_uom == "")
        {
            return "<span style='color:red'>No Item Selected for UOM Conversion...</span>";
            
        }

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        DataTable dt = new DataTable();
        string sql = "";

        List<List_Item> listitem = new List<List_Item>();
        List_Item objst = new List_Item();

        int ii = 0;

        switch (_obj)
        {

            case "#ContentPlaceHolder1_DDConversion_UOM":

                sql = "SELECT        item_uom_convesion_table.uom_cd_from, item_uom_convesion_table.uom_cd_to, item_uom_convesion_table.conversion_factor, item_uom.descs AS uom_from, item_uom_to.descs AS uom_to ";
                sql = sql + "FROM            item_uom_convesion_table INNER JOIN ";
                sql = sql + "item_uom ON item_uom_convesion_table.uom_cd_from = item_uom.code INNER JOIN ";
                sql = sql + "item_uom AS item_uom_to ON item_uom_convesion_table.uom_cd_to = item_uom_to.code ";
                sql = sql + "WHERE        (item_uom_convesion_table.status = 1) ";
                sql = sql + "AND        (item_uom_convesion_table.uom_cd_from = '" + _uom + "') ";
                sql = sql + "ORDER BY uom_from desc, uom_to desc ";

                break;
        }


        SqlDataAdapter da = new SqlDataAdapter(sql, cn);
        cn.Open();
        da.Fill(dt);
        cn.Close();



        if (dt.Rows.Count > 0)
        {

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                objst = new List_Item();
                objst.Code = dt.Rows[i]["uom_cd_to"].ToString().Trim() + "-" + dt.Rows[i]["conversion_factor"].ToString().Trim();
                objst.Descs = dt.Rows[i]["uom_from"].ToString().Trim() + " (" + dt.Rows[i]["uom_cd_from"].ToString().Trim() + ")  >>  " + dt.Rows[i]["uom_to"].ToString().Trim() + " (" + dt.Rows[i]["uom_cd_to"].ToString().Trim() + ")";
                listitem.Insert(ii, objst);
                ii += 1;
            }
        }
        JavaScriptSerializer jscript = new JavaScriptSerializer();
        return jscript.Serialize(listitem);
    }

    [WebMethod(EnableSession = true)]
    public string _get_trx_type(int edit_mode)
    {
        //ByVal _module As String, ByVal _trx_type As String


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }


        string sql = "SELECT prefix, descs, trx_class, trx_mode ";
        sql = sql + "FROM cf_trx_type ";
        sql = sql + "WHERE (module = '" + HttpContext.Current.Session["module"] + "') AND (trx_type = '" + HttpContext.Current.Session["trx_type"] + "') ";

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, cn);
        SqlDataReader dtr;

        cn.Open();
        dtr = cmd.ExecuteReader();
        string _return = "";
        if (dtr.Read())
        {
            if (edit_mode == 0)
            {
                _return = dtr["prefix"].ToString().Trim() + "########";
                _return = _return + "," + dtr["descs"].ToString().Trim();
            }
            

            HttpContext.Current.Session["prefix"] = dtr["prefix"].ToString().Trim();
            HttpContext.Current.Session["trx_class"] = dtr["trx_class"].ToString().Trim();
            HttpContext.Current.Session["trx_mode"] = dtr["trx_mode"].ToString().Trim();

        }
        cn.Close();

        return _return;

    }

    [WebMethod(EnableSession = true)]
    public string SearchItem(string xsearch, string _search_param, string _warehouse)
    {
        String[] arr = new String[4]; 
        

        xsearch = xsearch.Trim();
        if (xsearch == "") { return null; };

        try
        {
            if (HttpContext.Current.Session["username"] == null) { return "<span style='color:red' class='list-group-item'>Session Time-out. Please Log-in Again</span>"; };
        }
        catch (Exception ex)
        {
            return "<span style='color:red' class='list-group-item'>Session Time-out. Please Log-in Again</span>";
        }

        int ii = -1;

        String xtmp = xsearch + "~";
        xtmp = xtmp.Replace(" ", "~");
        String xstr = "";

        while (xtmp.IndexOf("~") > -1)
        {
            xstr = xtmp.Substring(0, xtmp.IndexOf("~") + 1);
            xtmp = xtmp.Replace(xstr, "");

            ii += 1;
        }
        // ERROR: Not supported in C#: ReDimStatement

        ii = 0;
       
        xtmp = xsearch + "~";
        xtmp = xtmp.Replace(" ", "~");
        while (xtmp.IndexOf("~") > -1)
        {
            xstr = xtmp.Substring(0, xtmp.IndexOf("~") + 1);
            arr[ii] = xstr.Replace("~", "").Replace(",", "").Replace("-", "").Replace("_", "").Replace(".", "");
            xtmp = xtmp.Replace(xstr, "");
            ii += 1;
        }

      
        string item_cd = "";
        //string lot_no = "";
        //string balance = "";

        if (_search_param != "")
        {
            item_cd = _search_param;
            //.Substring(5, _search_param.IndexOf("lot_no:") - 5)
            //lot_no = _search_param.Substring(_search_param.IndexOf("lot_no:") + 7, _search_param.IndexOf("balance:") + 8)
            //balance = _search_param.Substring(_search_param.IndexOf("balance:") + 9, _search_param.Length - (_search_param.IndexOf("balance:") + 9))
            //item_cd = item_cd.Replace("[", "'").Replace("]", "'").Replace(",", "','")
            //lot_no = lot_no.Replace("[", "'").Replace("]", "'").Replace(",", "','")

        }
        
        StringBuilder sb = new StringBuilder();
        bool sfind = false;

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dtr;
        SqlDataAdapter adapter = new SqlDataAdapter();

        adapter.InsertCommand = cmd;
        adapter.InsertCommand.Connection = cn;

        adapter.InsertCommand.CommandText = "sp_inventory_search_item";
        adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
        adapter.InsertCommand.CommandTimeout = 300;

        SqlParameter _param = new SqlParameter("@trx_type", SqlDbType.NChar, 2);
        _param.Value = HttpContext.Current.Session["trx_type"];
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        _param = new SqlParameter("@company_cd", SqlDbType.NChar, 2);
        _param.Value = HttpContext.Current.Session["company_code"];
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        _param = new SqlParameter("@warehouse_cd", SqlDbType.NChar, 5);
        _param.Value = _warehouse;
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);
        
       
        for (int i = 0; i <= 4; i++)
        {     
            try
            {
                _param = new SqlParameter("@search" + (i + 1).ToString(), SqlDbType.NVarChar, 50);
                _param.Value = "%" + arr[i] + "%";
                _param.Direction = ParameterDirection.Input;
                adapter.InsertCommand.Parameters.Add(_param);
            }
            catch (Exception ex)
            {
                _param = new SqlParameter("@search" + (i + 1).ToString(), SqlDbType.NVarChar, 50);
                _param.Value = "%%";
                _param.Direction = ParameterDirection.Input;
                adapter.InsertCommand.Parameters.Add(_param);
            }
        }

        adapter.InsertCommand.Connection.Open();
        dtr = adapter.InsertCommand.ExecuteReader();
        string sfind_item = "";

        DateTime null_date = new DateTime(1900, 1, 1);
        string mfg_date = null_date.ToShortDateString();

        while (dtr.Read())
        {

            double cost = 0;
            

            switch (HttpContext.Current.Session["trx_type"].ToString())
            {
                case "RS":
                case "RQ":


                    sb.AppendFormat("<a href='#' onclick=" + '\u0022' + "GetItemDetails('" + dtr["item_cd"].ToString().Trim() + "','" + dtr["descs"].ToString().Trim() + "','" + dtr["uom"].ToString().Trim() + "','" + dtr["uom_descs"].ToString().Trim() + "','" + dtr["item_weight"].ToString().Trim() + "'," + cost + ");" +  '\u0022' + " class='list-group-item'>");
                    sb.AppendFormat("<h5 class='list-group-item-heading' style='color:#114188;'>" + dtr["descs"].ToString().Trim() + "</h5>");
                    sb.AppendFormat("<p class='list-group-item-text'><b>ITEM TYPE : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["type_descs"].ToString().Trim() + "</span></p>");
                    sb.AppendFormat("<p class='list-group-item-text'><b>ITEM CLASS : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["class_descs"].ToString().Trim() + "</span></p>");
                    sb.AppendFormat("<p class='list-group-item-text'><b>ITEM CATEGORY : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["category_descs"].ToString().Trim() + "</span></p>");
                    sb.AppendFormat("<p class='list-group-item-text'><b>ITEM UOM : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["uom_descs"].ToString().Trim() + "</span></p>");
                    break;
                
                case "IS":
                case "IT":

                    cost = Convert.ToDouble(dtr["unit_cost"].ToString().Trim());
                    bool _add = true;
                    if (item_cd.IndexOf("[" + dtr["item_cd"].ToString().Trim() + "~" + dtr["lot_no"].ToString().Trim() + "~") > -1)
                    {
                        string _str = "[" + dtr["item_cd"].ToString().Trim() + "~" + dtr["lot_no"].ToString().Trim() + "~";
                        int _length = item_cd.IndexOf("[" + dtr["item_cd"].ToString().Trim() + "~" + dtr["lot_no"].ToString().Trim() + "~") + _str.Length;
                        string _new_str = item_cd.Substring(_length);
                        double _find_balance = Convert.ToDouble(_new_str.Substring(0, _new_str.IndexOf("]")).Replace(",", ""));

                        if (_find_balance == 0)
                            _add = false;

                    }

                    if (_add == true)
                    {
                        

                        mfg_date = (Convert.ToDateTime(dtr["mfg_date"]) == null_date) ? "" : Convert.ToDateTime(dtr["mfg_date"].ToString().Trim()).ToShortDateString();

                        if (sfind_item == dtr["item_cd"].ToString().Trim())
                        {
                            sb.AppendFormat("<a href='#' class='list-group-item disabled'>");
                        }
                        else
                        {
                             
                            sb.AppendFormat("<a href='#' onclick=" +  
                                '\u0022' +
                                "Get_IS_ItemDetails('" + 
                                dtr["item_cd"].ToString().Trim() + 
                                "','" + dtr["descs"].ToString().Trim() + 
                                "','" + dtr["uom"].ToString().Trim() + 
                                "','" + dtr["uom_descs"].ToString().Trim() + 
                                "','" + dtr["item_weight"].ToString().Trim() + 
                                "'," + cost + 
                                "," + dtr["available_qty"].ToString().Trim() + 
                                ",'" + Convert.ToDateTime(dtr["expiry_date"].ToString().Trim()).ToShortDateString() +
                                "','" + mfg_date +
                                "','" + dtr["receiving_receipt"].ToString().Trim() + 
                                "','" + dtr["lot_no"].ToString().Trim() + 
                                "','" + dtr["control_no"].ToString().Trim() + 
                                "'" + 
                                ");" +  
                                '\u0022' +
                                " class='list-group-item'>");
                        }

                        sfind_item = dtr["item_cd"].ToString().Trim();
                        sb.AppendFormat("<h5 class='list-group-item-heading' style='color:#114188;'>" + dtr["descs"].ToString().Trim() + "</h5>");
                        sb.AppendFormat("<p class='list-group-item-text'><b>ITEM TYPE : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["type_descs"].ToString().Trim() + "</span></p>");
                        sb.AppendFormat("<p class='list-group-item-text'><b>ITEM CLASS : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["class_descs"].ToString().Trim() + "</span></p>");
                        sb.AppendFormat("<p class='list-group-item-text'><b>ITEM CATEGORY : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["category_descs"].ToString().Trim() + "</span></p>");
                        sb.AppendFormat("<p class='list-group-item-text'><b>ITEM UOM : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["uom_descs"].ToString().Trim() + "</span></p>");
                        sb.AppendFormat("<p class='list-group-item-text'><b>AVAILABLE QUANTITY : </b><span style='font-weight:normal;color:#5cb85c;'>" + Convert.ToDouble(dtr["available_qty"].ToString().Trim()).ToString("#,##0.0000") + "</span></p>");
                        sb.AppendFormat("<p class='list-group-item-text'><b>EXPIRY DATE : </b><span style='font-weight:normal;color:#5cb85c;'>" + Convert.ToDateTime(dtr["expiry_date"].ToString().Trim()).ToShortDateString() + "</span></p>");
                        sb.AppendFormat("<p class='list-group-item-text'><b>MANUFACTURING DATE : </b><span style='font-weight:normal;color:#5cb85c;'>" + mfg_date + "</span></p>");

                    }

                    break;
                case "RA":
                case "IA":
                    cost = Convert.ToDouble(dtr["unit_cost"].ToString().Trim());
            

                    mfg_date = (Convert.ToDateTime(dtr["mfg_date"]) == null_date) ? "" : Convert.ToDateTime(dtr["mfg_date"].ToString().Trim()).ToShortDateString();

                    sb.AppendFormat("<a href='#' onclick=" +  '\u0022' + "Get_IS_ItemDetails('" + dtr["item_cd"].ToString().Trim() + "','" + dtr["descs"].ToString().Trim() + "','" + dtr["uom"].ToString().Trim() + "','" + dtr["uom_descs"].ToString().Trim() + "','" + dtr["item_weight"].ToString().Trim() + "'," + cost + "," + dtr["available_qty"].ToString().Trim() + ",'" + Convert.ToDateTime(dtr["expiry_date"].ToString().Trim()).ToShortDateString() + "','" + mfg_date + "','" + dtr["receiving_receipt"].ToString().Trim() + "','" + dtr["lot_no"].ToString().Trim() + "','" + dtr["control_no"].ToString().Trim() + "'" + ");" +  '\u0022' + " class='list-group-item'>");

                    sfind_item = dtr["item_cd"].ToString().Trim();
                    sb.AppendFormat("<h5 class='list-group-item-heading' style='color:#114188;'>" + dtr["descs"].ToString().Trim() + "</h5>");
                    sb.AppendFormat("<p class='list-group-item-text'><b>ITEM TYPE : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["type_descs"].ToString().Trim() + "</span></p>");
                    sb.AppendFormat("<p class='list-group-item-text'><b>ITEM CLASS : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["class_descs"].ToString().Trim() + "</span></p>");
                    sb.AppendFormat("<p class='list-group-item-text'><b>ITEM CATEGORY : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["category_descs"].ToString().Trim() + "</span></p>");
                    sb.AppendFormat("<p class='list-group-item-text'><b>ITEM UOM : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["uom_descs"].ToString().Trim() + "</span></p>");
                    sb.AppendFormat("<p class='list-group-item-text'><b>AVAILABLE QUANTITY : </b><span style='font-weight:normal;color:#5cb85c;'>" + Convert.ToDouble(dtr["available_qty"].ToString().Trim()).ToString("#,##0.0000") + "</span></p>");
                    sb.AppendFormat("<p class='list-group-item-text'><b>RECEIVING RECEIPT : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["receiving_receipt"].ToString().Trim() + "</span></p>");
                    sb.AppendFormat("<p class='list-group-item-text'><b>LOT NO. : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["lot_no"].ToString().Trim() + "</span></p>");
                    sb.AppendFormat("<p class='list-group-item-text'><b>EXPIRY DATE : </b><span style='font-weight:normal;color:#5cb85c;'>" + Convert.ToDateTime(dtr["expiry_date"].ToString().Trim()).ToShortDateString() + "</span></p>");
                    sb.AppendFormat("<p class='list-group-item-text'><b>MANUFACTURING DATE : </b><span style='font-weight:normal;color:#5cb85c;'>" + mfg_date + "</span></p>");

                    break;

            }

            //If HttpContext.Current.Session["trx_type") = "RS" Then

            //ElseIf HttpContext.Current.Session["trx_type") = "IS" Then

            //End If
            sb.AppendFormat("</a>");

            sfind = true;

        }
        dtr.Close();
        adapter.InsertCommand.Connection.Close();
        adapter.InsertCommand.Connection.Dispose();

        cn.Close();
        cn.Dispose();


        if (sfind == false)
        {
            sb.AppendFormat("<a href='#' class='list-group-item'>");
            sb.AppendFormat("<h5 class='list-group-item-heading' style='color:#114188;'>No search found...</h5>");
            sb.AppendFormat("</a>");

        }


        return sb.ToString();

    }

    [WebMethod(EnableSession = true)]
    public string _get_doc_no(string _Doc_No)
    {


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red' class='list-group-item'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red' class='list-group-item'>Session Time-out. Please Log-in Again</span>";
            
        }

        _Doc_No = _Doc_No.Replace("#", "");

        string Doc_no = _Doc_No + Security.Get_ID(_Doc_No).ToString("00000000");
        Security.Write_ID(_Doc_No, Convert.ToDouble(Doc_no.Replace(_Doc_No, "")));
        HttpContext.Current.Session["datenow"] = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");

        return Doc_no;

    }

    [WebMethod(EnableSession = true)]
    public string _delete_doc_no(string _Doc_No)
    {


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red' class='list-group-item'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red' class='list-group-item'>Session Time-out. Please Log-in Again</span>";
            
        }


        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();

        string sql = "DELETE FROM stock_card ";
        sql = sql + "WHERE        (company_cd = '" + HttpContext.Current.Session["company_code"] + "')  ";
        sql = sql + "AND (trx_type = '" + HttpContext.Current.Session["trx_type"] + "') ";
        sql = sql + "AND (doc_no = '" + _Doc_No + "') ";

        cmd = new SqlCommand(sql, cn);
        cn.Open();
        cmd.ExecuteNonQuery();
        cn.Close();

        HttpContext.Current.Session["datenow"] = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");


        return _Doc_No;

    }

    [WebMethod(EnableSession = true)]
    public string Save_Inventory(string item_sequence, string warehouse_cd, string doc_no, string doc_date, string descs, string ref_no, string po_no, string item_cd, string qty, string unit_cost,
    string total_cost, string receiving_receipt, string lot_no, string qc_no, string mfg_date, string expiry_date, string status, string div_cd, string dept_cd, string uom,
    string item_weight, string created_user, string transfer_company_cd, string transfer_warehouse_cd, string transfer_status_cd, string tracking_no, string item_conversion_uom, string item_conversion_factor)
    {

        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }

        string x = null;


        SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        using ((sqlCon))
        {

            SqlCommand sqlComm = new SqlCommand();

            sqlComm.Connection = sqlCon;

            sqlComm.CommandText = "sp_inventory_entry";
            sqlComm.CommandType = CommandType.StoredProcedure;

            sqlComm.Parameters.AddWithValue("@item_sequence", Convert.ToDouble(item_sequence));
            sqlComm.Parameters.AddWithValue("@company_cd", HttpContext.Current.Session["company_code"]);
            sqlComm.Parameters.AddWithValue("@warehouse_cd", warehouse_cd);
            sqlComm.Parameters.AddWithValue("@doc_no", doc_no.ToUpper());
            sqlComm.Parameters.AddWithValue("@doc_date", doc_date);
            sqlComm.Parameters.AddWithValue("@prefix", HttpContext.Current.Session["prefix"]);
            sqlComm.Parameters.AddWithValue("@trx_type", HttpContext.Current.Session["trx_type"]);
            sqlComm.Parameters.AddWithValue("@trx_class", HttpContext.Current.Session["trx_class"]);
            sqlComm.Parameters.AddWithValue("@trx_mode", HttpContext.Current.Session["trx_mode"]);
            sqlComm.Parameters.AddWithValue("@descs", descs.Replace("'", "''"));
            sqlComm.Parameters.AddWithValue("@ref_no", ref_no);
            sqlComm.Parameters.AddWithValue("@receiving_receipt", receiving_receipt);
            sqlComm.Parameters.AddWithValue("@po_no", po_no);
            sqlComm.Parameters.AddWithValue("@item_cd", item_cd);
            sqlComm.Parameters.AddWithValue("@qty", Convert.ToDouble(qty.Replace(",", "")));
            sqlComm.Parameters.AddWithValue("@unit_cost", 0);
            sqlComm.Parameters.AddWithValue("@total_cost", 0);
            sqlComm.Parameters.AddWithValue("@lot_no", lot_no.ToUpper());
            sqlComm.Parameters.AddWithValue("@qc_no", qc_no.ToUpper());
            sqlComm.Parameters.AddWithValue("@mfg_date", mfg_date);
            sqlComm.Parameters.AddWithValue("@expiry_date", expiry_date);
            sqlComm.Parameters.AddWithValue("@status", status);
            sqlComm.Parameters.AddWithValue("@audit_user", HttpContext.Current.Session["username"]);
            sqlComm.Parameters.AddWithValue("@audit_date", Convert.ToDateTime(HttpContext.Current.Session["datenow"]));
            sqlComm.Parameters.AddWithValue("@div_cd", div_cd);
            sqlComm.Parameters.AddWithValue("@dept_cd", dept_cd);
            sqlComm.Parameters.AddWithValue("@uom", uom.Trim());
            sqlComm.Parameters.AddWithValue("@item_weight", Convert.ToDouble(item_weight.Replace(",", "")));
            sqlComm.Parameters.AddWithValue("@created_by", created_user);
            sqlComm.Parameters.AddWithValue("@transfer_company_cd", transfer_company_cd);
            sqlComm.Parameters.AddWithValue("@transfer_warehouse_cd", transfer_warehouse_cd);
            sqlComm.Parameters.AddWithValue("@transfer_status_cd", transfer_status_cd);
            if (ref_no.IndexOf("ST") > -1)
            {
                sqlComm.Parameters.AddWithValue("@tracking_no", tracking_no);
            }
            else
            {
                sqlComm.Parameters.AddWithValue("@tracking_no", "");
            }
            sqlComm.Parameters.AddWithValue("@item_conversion_uom", item_conversion_uom);
            sqlComm.Parameters.AddWithValue("@item_conversion_factor", Convert.ToDouble(item_conversion_factor.Replace(",", "")));

            sqlCon.Open();

            sqlComm.ExecuteNonQuery();

            x = "New Inventory Record saved...";

            sqlCon.Close();
            sqlCon.Dispose();

        }



        return x;
    }

    [WebMethod(EnableSession = true)]
    public string SearchInventoryDocument(string xsearch)
    {

        xsearch = xsearch.Trim();
        if (xsearch == "")
        {
            return null;
            
        }

        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red' class='list-group-item'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red' class='list-group-item'>Session Time-out. Please Log-in Again</span>";
            
        }

        StringBuilder sb = new StringBuilder();
        bool sfind = false;

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dtr;
        SqlDataAdapter adapter = new SqlDataAdapter();

        adapter.InsertCommand = cmd;
        adapter.InsertCommand.Connection = cn;

        adapter.InsertCommand.CommandText = "sp_inventory_search_list";
        adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
        adapter.InsertCommand.CommandTimeout = 300;

        SqlParameter _param = new SqlParameter("@company_cd", SqlDbType.Char, 5);
        _param.Value = HttpContext.Current.Session["company_code"];
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        _param = new SqlParameter("@trx_type", SqlDbType.Char, 5);
        _param.Value = HttpContext.Current.Session["trx_type"];
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        _param = new SqlParameter("@username", SqlDbType.VarChar, 20);
        _param.Value = HttpContext.Current.Session["username"];
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        _param = new SqlParameter("@status", SqlDbType.VarChar, 20);
        _param.Value = HttpContext.Current.Session["search_status"];
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        _param = new SqlParameter("@search", SqlDbType.NVarChar, 250);
        _param.Value = "%" + xsearch + "%";
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        adapter.InsertCommand.Connection.Open();
        dtr = adapter.InsertCommand.ExecuteReader();


        while (dtr.Read())
        {

            sb.AppendFormat("<a href='#' onclick=" +  '\u0022' + "GetDocumentDetails('" + dtr["doc_no"].ToString().Trim() + "');" +  '\u0022' + " class='list-group-item'>");
            sb.AppendFormat("<h5 class='list-group-item-heading' style='color:#114188;'>DOCUMENT NO. : " + dtr["doc_no"].ToString().Trim() + "</h5>");
            sb.AppendFormat("<p class='list-group-item-text'><b>DESCRIPTION : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["descs"].ToString().Trim() + "</span></p>");
            sb.AppendFormat("<p class='list-group-item-text'><b>DOCUMENT DATE : </b><span style='font-weight:normal;color:#5cb85c;'>" + Convert.ToDateTime(dtr["doc_date"].ToString().Trim()).ToShortDateString() + "</span></p>");
            sb.AppendFormat("<p class='list-group-item-text'><b>WAREHOUSE : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["warehouse_descs"].ToString().Trim() + "</span></p>");
            sb.AppendFormat("</a>");

            sfind = true;

        }
        dtr.Close();
        adapter.InsertCommand.Connection.Close();



        if (sfind == false)
        {
            sb.AppendFormat("<a href='#' class='list-group-item'>");
            sb.AppendFormat("<h5 class='list-group-item-heading' style='color:#114188;'>No search found...</h5>");
            sb.AppendFormat("</a>");

        }


        return sb.ToString();

    }

    [WebMethod(EnableSession = true)]
    public string _get_doc_header(string _doc_no)
    {


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }

        string sql = "SELECT DISTINCT doc_no, doc_date, descs, ref_no, receiving_receipt, po_no,  dept_cd, div_cd, warehouse_cd, created_by, transfer_company_cd, transfer_warehouse_cd, transfer_status, transfer_tracking_no ";
        sql = sql + "FROM            stock_card ";
        sql = sql + "WHERE        (company_cd = '" + HttpContext.Current.Session["company_code"] + "') ";
        sql = sql + "AND (trx_type = '" + HttpContext.Current.Session["trx_type"] + "') ";
        sql = sql + "AND (doc_no = '" + _doc_no + "') ";

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, cn);
        SqlDataReader dtr;

        cn.Open();
        dtr = cmd.ExecuteReader();
        string _return = "";
        if (dtr.Read())
        {
            _return = dtr["doc_no"].ToString().Trim();
            _return = _return + "~" + dtr["descs"].ToString().Trim();
            _return = _return + "~" + Convert.ToDateTime(dtr["doc_date"].ToString().Trim()).ToShortDateString();
            _return = _return + "~" + dtr["ref_no"].ToString().Trim();
            _return = _return + "~" + dtr["po_no"].ToString().Trim();
            _return = _return + "~" + dtr["dept_cd"].ToString().Trim();
            _return = _return + "~" + dtr["div_cd"].ToString().Trim();
            _return = _return + "~" + dtr["warehouse_cd"].ToString().Trim();
            _return = _return + "~" + dtr["created_by"].ToString().Trim();
            _return = _return + "~" + dtr["transfer_tracking_no"].ToString().Trim();
            _return = _return + "~" + dtr["transfer_company_cd"].ToString().Trim();
            _return = _return + "~" + dtr["transfer_warehouse_cd"].ToString().Trim();
            _return = _return + "~" + dtr["transfer_status"].ToString().Trim();
        }
        cn.Close();
        //_return = _return.Replace(",,,", "")

        return _return;

    }

    [WebMethod(EnableSession = true)]
    public string _get_doc_details(string _doc_no, string _warehouse_cd)
    {


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }


        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dtr;
        SqlDataAdapter adapter = new SqlDataAdapter();

        adapter.InsertCommand = cmd;
        adapter.InsertCommand.Connection = cn;

        adapter.InsertCommand.CommandText = "sp_inventory_stock_details";
        adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
        adapter.InsertCommand.CommandTimeout = 300;

        SqlParameter _param = new SqlParameter("@trx_type", SqlDbType.NChar, 2);
        _param.Value = HttpContext.Current.Session["trx_type"];
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        _param = new SqlParameter("@company_cd", SqlDbType.NChar, 2);
        _param.Value = HttpContext.Current.Session["company_code"];
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        _param = new SqlParameter("@warehouse_cd", SqlDbType.NChar, 5);
        _param.Value = _warehouse_cd;
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        _param = new SqlParameter("@doc_no", SqlDbType.NVarChar, 20);
        _param.Value = _doc_no;
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);


        adapter.InsertCommand.Connection.Open();
        dtr = adapter.InsertCommand.ExecuteReader();

        Boolean hasMoreRecords = false;
        StringBuilder sb = new StringBuilder();

        sb.Append("[");


        while (dtr.Read())
        {
            if (hasMoreRecords)
            {
                sb.Append(",");
            }
            DateTime null_date = new DateTime(1900, 1, 1);

            string mfg_date = (Convert.ToDateTime(dtr["mfg_date"]) == null_date) ? "" : Convert.ToDateTime(dtr["mfg_date"].ToString().Trim()).ToShortDateString();


            sb.Append("{");
            sb.Append("\"item_cd\":\"" + dtr["item_cd"].ToString().Trim() + "\",");
            sb.Append("\"descs\":\"" + dtr["descs"].ToString().Trim() + "\",");
            sb.Append("\"item_weight\":\"" + dtr["item_weight"].ToString().Trim() + "\",");
            sb.Append("\"uom\":\"" + dtr["uom"] + "\",");
            sb.Append("\"qty\":\"" + Convert.ToDouble(dtr["qty"]).ToString("#,##0.0000") + "\",");
            sb.Append("\"unit_cost\":\"" + Convert.ToDouble(dtr["unit_cost"]).ToString("#,##0.0000") + "\",");
            sb.Append("\"total_cost\":\"" + Convert.ToDouble(dtr["total_cost"]).ToString("#,##0.0000") + "\",");
            sb.Append("\"receiving_receipt\":\"" + dtr["receiving_receipt"].ToString().Trim() + "\",");
            sb.Append("\"lot_no\":\"" + dtr["lot_no"].ToString().Trim() + "\",");
            sb.Append("\"qc_no\":\"" + dtr["control_no"].ToString().Trim() + "\",");
            sb.Append("\"mfg_date\":\"" + mfg_date + "\",");
            sb.Append("\"expiry_date\":\"" + Convert.ToDateTime(dtr["expiry_date"].ToString().Trim()).ToShortDateString() + "\",");
            sb.Append("\"available_qty\":\"" + dtr["available_qty"].ToString().Trim() + "\",");
            sb.Append("\"conversion_uom\":\"" + dtr["uom_conversion"].ToString().Trim() + "\",");
            sb.Append("\"conversion_factor\":\"" + dtr["uom_conversion_factor"].ToString().Trim() + "\"");
            sb.Append("}");

            hasMoreRecords = true;


        }
        dtr.Close();
        adapter.InsertCommand.Connection.Close();
        adapter.InsertCommand.Connection.Dispose();

        cn.Close();
        cn.Dispose();

        sb.Append("]");
        return sb.ToString();


    }

    [WebMethod(EnableSession = true)]
    public string Update_Inventory_in_report(string warehouse_cd, string doc_no, string status, string remarks, string trx_type)
    {


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }

        string x = null;


        SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        using ((sqlCon))
        {

            SqlCommand sqlComm = new SqlCommand();

            sqlComm.Connection = sqlCon;

            sqlComm.CommandText = "sp_inventory_update";
            sqlComm.CommandType = CommandType.StoredProcedure;


            sqlComm.Parameters.AddWithValue("@company_cd", HttpContext.Current.Session["company_code"]);
            sqlComm.Parameters.AddWithValue("@warehouse_cd", warehouse_cd);
            sqlComm.Parameters.AddWithValue("@doc_no", doc_no);
            sqlComm.Parameters.AddWithValue("@trx_type", trx_type);
            sqlComm.Parameters.AddWithValue("@status", status);
            sqlComm.Parameters.AddWithValue("@remarks", remarks.Replace("'", "''"));
            sqlComm.Parameters.AddWithValue("@audit_user", HttpContext.Current.Session["username"]);
            sqlComm.Parameters.AddWithValue("@audit_date", System.DateTime.Now);

            if (trx_type == "IT")
            {
                //Dim str As String = Date.Now.ToString("ddyyyyMMHHmmss")
                //Dim tracking As String = Security.Get_ID(str).ToString("000")

                //tracking = str & "-" & tracking

                //sqlComm.Parameters.AddWithValue("@transfer_status", "In-Transit")
                //sqlComm.Parameters.AddWithValue("@transfer_tracking", tracking)
                x = "Inventory Record cannot be process in this module <br /><b>Document No. : " + doc_no + "</b>";


            }
            else if (trx_type == "RS")
            {
                sqlComm.Parameters.AddWithValue("@transfer_status", null);
                sqlComm.Parameters.AddWithValue("@transfer_tracking", null);
                x = "Inventory Record has been " + status + "...<br /><b>Document No. : " + doc_no + "</b>";

            }
            else
            {
                sqlComm.Parameters.AddWithValue("@transfer_status", null);
                sqlComm.Parameters.AddWithValue("@transfer_tracking", null);
                x = "Inventory Record has been " + status + "...<br /><b>Document No. : " + doc_no + "</b>";
            }
            sqlCon.Open();

            sqlComm.ExecuteNonQuery();

            sqlCon.Close();
            sqlCon.Dispose();

        }



        return x;
    }

    [WebMethod(EnableSession = true)]
    public string Post_Listing()
    {
        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }

        string x = null;


        SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        using ((sqlCon))
        {

            SqlCommand sqlComm = new SqlCommand();

            sqlComm.Connection = sqlCon;

            sqlComm.CommandText = "sp_inventory_posting";
            sqlComm.CommandType = CommandType.StoredProcedure;


            sqlComm.Parameters.AddWithValue("@company_cd", HttpContext.Current.Session["company_code"]);
            sqlComm.Parameters.AddWithValue("@audit_user", HttpContext.Current.Session["username"]);

            sqlCon.Open();

            sqlComm.ExecuteNonQuery();
            x = "Approved inventory listing are now posted...";
            sqlCon.Close();
            sqlCon.Dispose();

        }
        return x;
    }

    [WebMethod(EnableSession = true)]
    public string Update_Inventory(string warehouse_cd, string doc_no, string status, string remarks, string ref_no, string tracking_no)
    {


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }

        string x = null;


        SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        using ((sqlCon))
        {

            SqlCommand sqlComm = new SqlCommand();

            sqlComm.Connection = sqlCon;

            sqlComm.CommandText = "sp_inventory_update";
            sqlComm.CommandType = CommandType.StoredProcedure;


            sqlComm.Parameters.AddWithValue("@company_cd", HttpContext.Current.Session["company_code"]);
            sqlComm.Parameters.AddWithValue("@warehouse_cd", warehouse_cd);
            sqlComm.Parameters.AddWithValue("@doc_no", doc_no);
            sqlComm.Parameters.AddWithValue("@trx_type", HttpContext.Current.Session["trx_type"]);
            sqlComm.Parameters.AddWithValue("@status", status);
            sqlComm.Parameters.AddWithValue("@remarks", remarks.Replace("'", "''"));
            sqlComm.Parameters.AddWithValue("@audit_user", HttpContext.Current.Session["username"]);
            sqlComm.Parameters.AddWithValue("@audit_date", System.DateTime.Now);

            if (HttpContext.Current.Session["trx_type"] == "IT")
            {
                //Dim str As String = CDbl(Date.Now.ToOADate()).ToString '.ToString("yyyyMMddHHmmss")
                string str = Convert.ToDouble(System.DateTime.Now.ToOADate()).ToString("000000");

                string tracking = Security.Get_ID(str).ToString("000");

                // tracking = str & "-" & tracking

                sqlComm.Parameters.AddWithValue("@transfer_status", "In-Transit");
                sqlComm.Parameters.AddWithValue("@transfer_tracking", str + "-" + tracking);
                x = "Inventory Record has been " + status + "...<br /><b>Document No. : " + doc_no + "</b><br /><b>Tracking No. : " + str + "-" + tracking + "</b>";

                Security.Write_ID(str, Convert.ToDouble(tracking));

            }
            else if (HttpContext.Current.Session["trx_type"] == "RS")
            {
                if (ref_no.IndexOf("ST") > -1)
                {
                    sqlComm.Parameters.AddWithValue("@transfer_status", "Received");
                    sqlComm.Parameters.AddWithValue("@transfer_tracking", tracking_no);
                    x = "Inventory Record has been " + status + " and received...<br /><b>Document No. : " + doc_no + "</b><br /><b>Tracking No. : " + tracking_no + "</b>";
                }
                else
                {
                    sqlComm.Parameters.AddWithValue("@transfer_status", null);
                    sqlComm.Parameters.AddWithValue("@transfer_tracking", null);
                    x = "Inventory Record has been " + status + "...<br /><b>Document No. : " + doc_no + "</b>";
                }
            }
            else
            {
                sqlComm.Parameters.AddWithValue("@transfer_status", null);
                sqlComm.Parameters.AddWithValue("@transfer_tracking", null);
                x = "Inventory Record has been " + status + "...<br /><b>Document No. : " + doc_no + "</b>";
            }
            sqlCon.Open();

            sqlComm.ExecuteNonQuery();

            sqlCon.Close();
            sqlCon.Dispose();

        }



        return x;
    }

    [WebMethod(EnableSession = true)]
    public string SearchInventory_other_sources(string _item_cd)
    {

        _item_cd = _item_cd.Trim();
        if (_item_cd == "")
        {
            return null;
            
        }

        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red' class='list-group-item'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red' class='list-group-item'>Session Time-out. Please Log-in Again</span>";
            
        }

        StringBuilder sb = new StringBuilder();
        bool sfind = false;

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dtr;
        SqlDataAdapter adapter = new SqlDataAdapter();

        adapter.InsertCommand = cmd;
        adapter.InsertCommand.Connection = cn;

        adapter.InsertCommand.CommandText = "sp_inventory_other_sources";
        adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
        adapter.InsertCommand.CommandTimeout = 300;

        SqlParameter _param = new SqlParameter("@item_cd", SqlDbType.Char, 10);
        _param.Value = _item_cd;
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        //_param = New SqlParameter("@warehouse_cd", SqlDbType.Char, 5)
        //_param.Value = warehouse_cd
        //_param.Direction = ParameterDirection.Input
        //adapter.InsertCommand.Parameters.Add(_param)

        //_param = New SqlParameter("@search", SqlDbType.NVarChar, 250)
        //_param.Value = "%" & xsearch & "%"
        //_param.Direction = ParameterDirection.Input
        //adapter.InsertCommand.Parameters.Add(_param)

        adapter.InsertCommand.Connection.Open();
        dtr = adapter.InsertCommand.ExecuteReader();


        while (dtr.Read())
        {

            if (HttpContext.Current.Session["company_code"].ToString().Trim() == dtr["company_cd"].ToString().Trim() & HttpContext.Current.Session["warehouse_cd"].ToString().Trim() == dtr["warehouse_cd"].ToString().Trim())
            {
            }
            else
            {
                sb.AppendFormat("<div class='list-group-item'>");
                sb.AppendFormat("<h5 class='list-group-item-heading' style='color:#114188;'><b>Company : </b>" + dtr["company_descs"].ToString().Trim() + "</h5>");
                sb.AppendFormat("<p class='list-group-item-text'><small><b>Warehouse : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["warehouse_descs"].ToString().Trim() + "</span></small></p>");
                sb.AppendFormat("<p class='list-group-item-text'><small><b>Quantity Available : </b><span style='font-weight:normal;color:#5cb85c;'>" + Convert.ToDouble(dtr["available_qty"].ToString().Trim()).ToString("#,##0.00") + "</span></small></p>");
                sb.AppendFormat("<p class='list-group-item-text'><small><b>Total Amount : </b><span style='font-weight:normal;color:#5cb85c;'>" + Convert.ToDouble(Convert.ToDouble(dtr["available_qty"].ToString().Trim()) * Convert.ToDouble(dtr["unit_cost"].ToString().Trim())).ToString("#,##0.00") + "</span></small></p>");
                sb.AppendFormat("</div>");

                sfind = true;
            }
        }
        dtr.Close();
        adapter.InsertCommand.Connection.Close();



        if (sfind == false)
        {
            sb.AppendFormat("<div class='list-group-item'>");
            sb.AppendFormat("<h5 class='list-group-item-heading' style='color:#114188;'>No item sources found...</h5>");
            sb.AppendFormat("</div>");

        }


        return sb.ToString();

    }

    [WebMethod(EnableSession = true)]
    public string SearchInventoryReference(string xsearch, string warehouse_cd)
    {

        xsearch = xsearch.Trim();
        if (xsearch == "")
        {
            return null;
            
        }

        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red' class='list-group-item'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red' class='list-group-item'>Session Time-out. Please Log-in Again</span>";
            
        }

        StringBuilder sb = new StringBuilder();
        bool sfind = false;

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dtr;
        SqlDataAdapter adapter = new SqlDataAdapter();

        adapter.InsertCommand = cmd;
        adapter.InsertCommand.Connection = cn;

        adapter.InsertCommand.CommandText = "sp_inventory_ref_no";
        adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
        adapter.InsertCommand.CommandTimeout = 300;

        SqlParameter _param = new SqlParameter("@company_cd", SqlDbType.Char, 5);
        _param.Value = HttpContext.Current.Session["company_code"];
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        _param = new SqlParameter("@warehouse_cd", SqlDbType.Char, 5);
        _param.Value = warehouse_cd;
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        _param = new SqlParameter("@trx_type", SqlDbType.Char, 5);
        _param.Value = HttpContext.Current.Session["trx_type"];
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        _param = new SqlParameter("@search", SqlDbType.NVarChar, 250);
        _param.Value = "%" + xsearch + "%";
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        adapter.InsertCommand.Connection.Open();
        dtr = adapter.InsertCommand.ExecuteReader();


        while (dtr.Read())
        {

            sb.AppendFormat("<a href='#' onclick=" +  '\u0022' + "GetReferenceDetails('" + dtr["doc_no"].ToString().Trim() + "','" + dtr["trx_type"].ToString().Trim() + "','" + dtr["warehouse_cd"].ToString().Trim() + "');" +  '\u0022' + " class='list-group-item'>");
            sb.AppendFormat("<h5 class='list-group-item-heading' style='color:#114188;'>DOCUMENT NO. : " + dtr["doc_no"].ToString().Trim() + "</h5>");
            sb.AppendFormat("<p class='list-group-item-text'><b>DESCRIPTION : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["descs"].ToString().Trim() + "</span></p>");
            sb.AppendFormat("<p class='list-group-item-text'><b>DOCUMENT DATE : </b><span style='font-weight:normal;color:#5cb85c;'>" + Convert.ToDateTime(dtr["doc_date"].ToString().Trim()).ToShortDateString() + "</span></p>");
            sb.AppendFormat("<p class='list-group-item-text'><b>COMPANY : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["company_descs"].ToString().Trim() + "</span></p>");
            sb.AppendFormat("<p class='list-group-item-text'><b>WAREHOUSE : </b><span style='font-weight:normal;color:#5cb85c;'>" + dtr["warehouse_descs"].ToString().Trim() + "</span></p>");
            sb.AppendFormat("</a>");

            sfind = true;

        }
        dtr.Close();
        adapter.InsertCommand.Connection.Close();



        if (sfind == false)
        {
            sb.AppendFormat("<a href='#' class='list-group-item'>");
            sb.AppendFormat("<h5 class='list-group-item-heading' style='color:#114188;'>No search found...</h5>");
            sb.AppendFormat("</a>");

        }


        return sb.ToString();

    }

    [WebMethod(EnableSession = true)]
    public string _get_reference_details(string _doc_no, string _trx_type, string _warehouse_cd, string _tracking_no)
    {

        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }


        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dtr;
        SqlDataAdapter adapter = new SqlDataAdapter();

        adapter.InsertCommand = cmd;
        adapter.InsertCommand.Connection = cn;

        adapter.InsertCommand.CommandText = "sp_inventory_stock_reference";
        adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
        adapter.InsertCommand.CommandTimeout = 300;

        SqlParameter _param = new SqlParameter("@trx_type", SqlDbType.NChar, 2);
        _param.Value = _trx_type;
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        _param = new SqlParameter("@company_cd", SqlDbType.NChar, 2);
        _param.Value = HttpContext.Current.Session["company_code"];
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        _param = new SqlParameter("@warehouse_cd", SqlDbType.NChar, 5);
        _param.Value = _warehouse_cd;
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        _param = new SqlParameter("@doc_no", SqlDbType.NVarChar, 20);
        _param.Value = _doc_no;
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        _param = new SqlParameter("@tracking_no", SqlDbType.NVarChar, 30);
        _param.Value = _tracking_no;
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);


        adapter.InsertCommand.Connection.Open();
        dtr = adapter.InsertCommand.ExecuteReader();

        bool hasMoreRecords = false;
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        bool sfind = false;

        while (dtr.Read())
        {
            if (hasMoreRecords)
            {
                sb.Append(",");
            }

            DateTime null_date = new DateTime(1900, 1, 1);
            string mfg_date = null_date.ToShortDateString();
            mfg_date = (Convert.ToDateTime(dtr["mfg_date"]) == null_date) ? "" : Convert.ToDateTime(dtr["mfg_date"].ToString().Trim()).ToShortDateString();

            sb.Append("{");
            sb.Append("\"item_cd\":\"" + dtr["item_cd"].ToString().Trim() + "\",");
            sb.Append("\"descs\":\"" + dtr["descs"].ToString().Trim() + "\",");
            sb.Append("\"item_weight\":\"" + dtr["item_weight"].ToString().Trim() + "\",");
            sb.Append("\"uom\":\"" + dtr["uom"] + "\",");
            sb.Append("\"qty\":\"" + Convert.ToDouble(dtr["qty"]).ToString("#,##0.0000") + "\",");
            sb.Append("\"receiving_receipt\":\"" + dtr["receiving_receipt"].ToString().Trim() + "\",");
            sb.Append("\"lot_no\":\"" + dtr["lot_no"].ToString().Trim() + "\",");
            sb.Append("\"qc_no\":\"" + dtr["control_no"].ToString().Trim() + "\",");
            sb.Append("\"mfg_date\":\"" + mfg_date + "\",");
            sb.Append("\"expiry_date\":\"" + Convert.ToDateTime(dtr["expiry_date"].ToString().Trim()).ToShortDateString() + "\",");
            sb.Append("\"available_qty\":\"" + dtr["available_qty"].ToString().Trim() + "\",");
            sb.Append("\"conversion_uom\":\"" + dtr["uom_conversion"].ToString().Trim() + "\",");
            sb.Append("\"conversion_factor\":\"" + dtr["uom_conversion_factor"].ToString().Trim() + "\"");
            sb.Append("}");

            hasMoreRecords = true;

            sfind = true;
        }
        dtr.Close();
        adapter.InsertCommand.Connection.Close();
        adapter.InsertCommand.Connection.Dispose();

        cn.Close();
        cn.Dispose();

        sb.Append("]");

        if (sfind == true)
        {
            return sb.ToString();
        }
        else
        {
            return "<span style='color:red'>Invalid Tracking No.</span>";
        }



    }

    #endregion


    #region "Human Resources"

    [WebMethod(EnableSession = true)]
    public string make_efile(string department_code, string datefrom, string dateto)
    {

        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }


        string filename = "Nutratech_DTR.xlsx";
        //_" & Date.Now.ToString("MMddyyyy") & "
        string x = "";

        if (File.Exists(Server.MapPath("~\\pdf\\" + HttpContext.Current.Session["username"] + "\\") + filename))
        {
            try
            {
                File.Delete(Server.MapPath("~\\pdf\\" + HttpContext.Current.Session["username"] + "\\") + filename);

            }
            catch (Exception ex)
            {
            }
        }

        // Create the workbook
        XLWorkbook workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Nutratech DTR " + System.DateTime.Now.ToString("MMM yyyy"));

        worksheet.Cell(1, 1).SetValue("NUTRATECH BIOPHARMA INC.");
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 12;
        worksheet.Cell(1, 1).Style.Font.FontName = "Calibri";

        worksheet.Cell(2, 1).SetValue("DAILY TIME RECORD MONITORING FORM");
        worksheet.Cell(2, 1).Style.Font.FontSize = 11;
        worksheet.Cell(2, 1).Style.Font.FontName = "Calibri";

        worksheet.Cell(3, 1).SetValue("DATE RANGE : " + datefrom + " - " + dateto);
        worksheet.Cell(3, 1).Style.Font.FontSize = 11;
        worksheet.Cell(3, 1).Style.Font.FontName = "Calibri";

        worksheet.Cell(4, 1).SetValue("DEPARTMENT : " + ExcelReport.hr_department_Descs(department_code));
        worksheet.Cell(4, 1).Style.Font.FontSize = 11;
        worksheet.Cell(4, 1).Style.Font.FontName = "Calibri";

        worksheet.Cell(1, 7).SetValue("LOG TYPE LEGEND : ");
        worksheet.Cell(1, 7).Style.Font.FontSize = 10;
        worksheet.Cell(1, 7).Style.Font.Bold = true;
        worksheet.Cell(1, 7).Style.Font.FontName = "Calibri";

        worksheet.Cell(1, 8).SetValue("OB - Official Business");
        worksheet.Cell(1, 8).Style.Font.FontSize = 10;
        worksheet.Cell(1, 8).Style.Font.FontName = "Calibri";

        worksheet.Cell(2, 8).SetValue("CT - Change of Timekeeping");
        worksheet.Cell(2, 8).Style.Font.FontSize = 10;
        worksheet.Cell(2, 8).Style.Font.FontName = "Calibri";

        worksheet.Column(1).Width = 5;
        worksheet.Column(2).Width = 15;
        worksheet.Column(3).Width = 30;
        worksheet.Column(4).Width = 15;
        worksheet.Column(5).Width = 15;
        worksheet.Column(6).Width = 15;
        worksheet.Column(7).Width = 15;
        worksheet.Column(8).Width = 10;
        worksheet.Column(9).Width = 30;
        worksheet.Column(10).Width = 20;

        int xrow = 6;
        int xcol = 1;

        //============================================================================================
        xcol = 1;
        worksheet.Cell(xrow, xcol).SetValue("No.");
        worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
        worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
        worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
        worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell(xrow, xcol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Range("A" + xrow + ":B" + (xrow + 1).ToString()).Column(1).Merge();
        worksheet.Range("A" + xrow + ":B" + (xrow + 1).ToString()).Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Range("A" + xrow + ":B" + (xrow + 1).ToString()).Style.Border.BottomBorder = XLBorderStyleValues.Double;
        worksheet.Range("A" + xrow + ":B" + (xrow + 1).ToString()).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        worksheet.Range("A" + xrow + ":B" + (xrow + 1).ToString()).Style.Border.RightBorder = XLBorderStyleValues.Thin;
      

        xcol = 2;
        worksheet.Cell(xrow, xcol).SetValue("Employee ID");
        worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
        worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
        worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
        worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell(xrow, xcol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Range("B" + xrow + ":C" + (xrow + 1).ToString()).Column(1).Merge();
        worksheet.Range("B" + xrow + ":C" + (xrow + 1).ToString()).Column(1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Range("B" + xrow + ":C" + (xrow + 1).ToString()).Column(1).Style.Border.BottomBorder = XLBorderStyleValues.Double;
        worksheet.Range("B" + xrow + ":C" + (xrow + 1).ToString()).Column(1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        worksheet.Range("B" + xrow + ":C" + (xrow + 1).ToString()).Column(1).Style.Border.RightBorder = XLBorderStyleValues.Thin;

        xcol = 3;
        worksheet.Cell(xrow, xcol).SetValue("Name");
        worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
        worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
        worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
        worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell(xrow, xcol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Range("C" + xrow + ":D" + (xrow + 1).ToString()).Column(1).Merge();
        worksheet.Range("C" + xrow + ":D" + (xrow + 1).ToString()).Column(1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Range("C" + xrow + ":D" + (xrow + 1).ToString()).Column(1).Style.Border.BottomBorder = XLBorderStyleValues.Double;
        worksheet.Range("C" + xrow + ":D" + (xrow + 1).ToString()).Column(1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        worksheet.Range("C" + xrow + ":D" + (xrow + 1).ToString()).Column(1).Style.Border.RightBorder = XLBorderStyleValues.Thin;

        xcol = 4;
        worksheet.Cell(xrow, xcol).Value = "AM";
        worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
        worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
        worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
        worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Range("D" + xrow + ":E" + xrow).Row(1).Merge();
        worksheet.Range("D" + xrow + ":E" + xrow).Row(1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Range("D" + xrow + ":E" + xrow).Row(1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        worksheet.Range("D" + xrow + ":E" + xrow).Row(1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        worksheet.Range("D" + xrow + ":E" + xrow).Row(1).Style.Border.RightBorder = XLBorderStyleValues.Thin;

        xcol = 6;
        worksheet.Cell(xrow, xcol).Value = "PM";
        worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
        worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
        worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
        worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Range("F" + xrow + ":G" + xrow).Row(1).Merge();
        worksheet.Range("F" + xrow + ":G" + xrow).Row(1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Range("F" + xrow + ":G" + xrow).Row(1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        worksheet.Range("F" + xrow + ":G" + xrow).Row(1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        worksheet.Range("F" + xrow + ":G" + xrow).Row(1).Style.Border.RightBorder = XLBorderStyleValues.Thin;

        xcol = 8;
        worksheet.Cell(xrow, xcol).SetValue("Log Type");
        worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
        worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
        worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
        worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell(xrow, xcol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Range("H" + xrow + ":I" + (xrow + 1).ToString()).Column(1).Merge();
        worksheet.Range("H" + xrow + ":I" + (xrow + 1).ToString()).Column(1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Range("H" + xrow + ":I" + (xrow + 1).ToString()).Column(1).Style.Border.BottomBorder = XLBorderStyleValues.Double;
        worksheet.Range("H" + xrow + ":I" + (xrow + 1).ToString()).Column(1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        worksheet.Range("H" + xrow + ":I" + (xrow + 1).ToString()).Column(1).Style.Border.RightBorder = XLBorderStyleValues.Thin;

        xcol = 9;
        worksheet.Cell(xrow, xcol).SetValue("Remarks");
        worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
        worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
        worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
        worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell(xrow, xcol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Range("I" + xrow + ":J" + (xrow + 1).ToString()).Column(1).Merge();
        worksheet.Range("I" + xrow + ":J" + (xrow + 1).ToString()).Column(1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Range("I" + xrow + ":J" + (xrow + 1).ToString()).Column(1).Style.Border.BottomBorder = XLBorderStyleValues.Double;
        worksheet.Range("I" + xrow + ":J" + (xrow + 1).ToString()).Column(1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        worksheet.Range("I" + xrow + ":J" + (xrow + 1).ToString()).Column(1).Style.Border.RightBorder = XLBorderStyleValues.Thin;

        xcol = 10;
        worksheet.Cell(xrow, xcol).SetValue("Signature");
        worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
        worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
        worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
        worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell(xrow, xcol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Range("J" + xrow + ":K" + (xrow + 1).ToString()).Column(1).Merge();
        worksheet.Range("J" + xrow + ":K" + (xrow + 1).ToString()).Column(1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Range("J" + xrow + ":K" + (xrow + 1).ToString()).Column(1).Style.Border.BottomBorder = XLBorderStyleValues.Double;
        worksheet.Range("J" + xrow + ":K" + (xrow + 1).ToString()).Column(1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        worksheet.Range("J" + xrow + ":K" + (xrow + 1).ToString()).Column(1).Style.Border.RightBorder = XLBorderStyleValues.Thin;

        //============================================================================================
        xrow += 1;

        xcol = 4;
        worksheet.Cell(xrow, xcol).SetValue("Time In");
        worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
        worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
        worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
        worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell(xrow, xcol).Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell(xrow, xcol).Style.Border.BottomBorder = XLBorderStyleValues.Double;
        worksheet.Cell(xrow, xcol).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        worksheet.Cell(xrow, xcol).Style.Border.RightBorder = XLBorderStyleValues.Thin;

        xcol = 5;
        worksheet.Cell(xrow, xcol).SetValue("Time Out");
        worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
        worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
        worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
        worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell(xrow, xcol).Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell(xrow, xcol).Style.Border.BottomBorder = XLBorderStyleValues.Double;
        worksheet.Cell(xrow, xcol).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        worksheet.Cell(xrow, xcol).Style.Border.RightBorder = XLBorderStyleValues.Thin;

        xcol = 6;
        worksheet.Cell(xrow, xcol).SetValue("Time In");
        worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
        worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
        worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
        worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell(xrow, xcol).Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell(xrow, xcol).Style.Border.BottomBorder = XLBorderStyleValues.Double;
        worksheet.Cell(xrow, xcol).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        worksheet.Cell(xrow, xcol).Style.Border.RightBorder = XLBorderStyleValues.Thin;

        xcol = 7;
        worksheet.Cell(xrow, xcol).SetValue("Time Out");
        worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
        worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
        worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
        worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell(xrow, xcol).Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell(xrow, xcol).Style.Border.BottomBorder = XLBorderStyleValues.Double;
        worksheet.Cell(xrow, xcol).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        worksheet.Cell(xrow, xcol).Style.Border.RightBorder = XLBorderStyleValues.Thin;

        //============================================================================================
        xrow += 1;

        int f_date_year = Convert.ToDateTime(datefrom).Year;
        int f_date_month = Convert.ToDateTime(datefrom).Month;
        int f_date_day = Convert.ToDateTime(datefrom).Day;

        int t_date_year = Convert.ToDateTime(dateto).Year;
        int t_date_month = Convert.ToDateTime(dateto).Month;
        int t_date_day = Convert.ToDateTime(dateto).Day;

        bool _loop = false;
        string xdate = "";

        for (int i = f_date_year; i <= t_date_year; i++)
        {

            if (i < t_date_year)
            {
                for (int ii = f_date_month; ii <= 12; ii++)
                {
                    if (ii <= 12)
                    {
                        if (_loop == true)
                        {
                            int end_month_day = ExcelReport.get_end_day(ii, i);
                            for (int iii = 1; iii <= end_month_day; iii++)
                            {
                                xdate = ii.ToString("00") + "/" + iii.ToString("00") + "/" + i.ToString("0000");
                                _loop = true;


                                //============================================================================================

                                xcol = 1;
                                worksheet.Cell(xrow, xcol).SetValue(Convert.ToDateTime(xdate).ToLongDateString());
                                worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
                                worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
                                worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
                                worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Merge();
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                xrow += 1;
                                xrow = ExcelReport.xstr_excelfile(worksheet, department_code, xdate, xrow);

                                xrow += 2;
                            }
                        }
                        else
                        {
                            int end_month_day = ExcelReport.get_end_day(ii, i);
                            for (int iii = f_date_day; iii <= end_month_day; iii++)
                            {
                                xdate = ii.ToString("00") + "/" + iii.ToString("00") + "/" + i.ToString("0000");
                                _loop = true;


                                //============================================================================================

                                xcol = 1;
                                worksheet.Cell(xrow, xcol).SetValue(Convert.ToDateTime(xdate).ToLongDateString());
                                worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
                                worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
                                worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
                                worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Merge();
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                xrow += 1;
                                xrow = ExcelReport.xstr_excelfile(worksheet, department_code, xdate, xrow);

                                xrow += 2;
                            }
                        }
                    }
                }

            }
            else if (i == t_date_year)
            {
                if (_loop == true)
                {
                    for (int ii = 1; ii <= t_date_month; ii++)
                    {
                        if (ii < t_date_month)
                        {
                            int end_month_day = ExcelReport.get_end_day(ii, i);
                            for (int iii = 1; iii <= end_month_day; iii++)
                            {
                                xdate = ii.ToString("00") + "/" + iii.ToString("00") + "/" + i.ToString("0000");
                                _loop = true;


                                //============================================================================================

                                xcol = 1;
                                worksheet.Cell(xrow, xcol).SetValue(Convert.ToDateTime(xdate).ToLongDateString());
                                worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
                                worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
                                worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
                                worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Merge();
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                xrow += 1;
                                xrow = ExcelReport.xstr_excelfile(worksheet, department_code, xdate, xrow);

                                xrow += 2;
                            }
                        }
                        else if (ii == t_date_month)
                        {
                            for (int iii = 1; iii <= t_date_day; iii++)
                            {
                                xdate = ii.ToString("00") + "/" + iii.ToString("00") + "/" + i.ToString("0000");
                                _loop = true;


                                //============================================================================================

                                xcol = 1;
                                worksheet.Cell(xrow, xcol).SetValue(Convert.ToDateTime(xdate).ToLongDateString());
                                worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
                                worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
                                worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
                                worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Merge();
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                xrow += 1;
                                xrow = ExcelReport.xstr_excelfile(worksheet, department_code, xdate, xrow);

                                xrow += 2;
                            }
                        }
                    }
                }
                else
                {
                    for (int ii = f_date_month; ii <= t_date_month; ii++)
                    {
                        if (ii < t_date_month)
                        {
                            if (_loop == true)
                            {
                                int end_month_day = ExcelReport.get_end_day(ii, i);
                                for (int iii = 1; iii <= end_month_day; iii++)
                                {
                                    xdate = ii.ToString("00") + "/" + iii.ToString("00") + "/" + i.ToString("0000");
                                    _loop = true;


                                    //============================================================================================

                                    xcol = 1;
                                    worksheet.Cell(xrow, xcol).SetValue(Convert.ToDateTime(xdate).ToLongDateString());
                                    worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
                                    worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
                                    worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
                                    worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Merge();
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                    xrow += 1;
                                    xrow = ExcelReport.xstr_excelfile(worksheet, department_code, xdate, xrow);

                                    xrow += 2;
                                }
                            }
                            else
                            {
                                int end_month_day = ExcelReport.get_end_day(ii, i);
                                for (int iii = f_date_day; iii <= end_month_day; iii++)
                                {
                                    xdate = ii.ToString("00") + "/" + iii.ToString("00") + "/" + i.ToString("0000");
                                    _loop = true;


                                    //============================================================================================

                                    xcol = 1;
                                    worksheet.Cell(xrow, xcol).SetValue(Convert.ToDateTime(xdate).ToLongDateString());
                                    worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
                                    worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
                                    worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
                                    worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Merge();
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                    xrow += 1;
                                    xrow = ExcelReport.xstr_excelfile(worksheet, department_code, xdate, xrow);

                                    xrow += 2;
                                }
                            }

                        }
                        else if (ii == t_date_month)
                        {
                            if (_loop == true)
                            {
                                for (int iii = 1; iii <= t_date_day; iii++)
                                {
                                    xdate = ii.ToString("00") + "/" + iii.ToString("00") + "/" + i.ToString("0000");
                                    _loop = true;


                                    //============================================================================================

                                    xcol = 1;
                                    worksheet.Cell(xrow, xcol).SetValue(Convert.ToDateTime(xdate).ToLongDateString());
                                    worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
                                    worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
                                    worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
                                    worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Merge();
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                    xrow += 1;
                                    xrow = ExcelReport.xstr_excelfile(worksheet, department_code, xdate, xrow);

                                    xrow += 2;
                                }
                            }
                            else
                            {
                                for (int iii = f_date_day; iii <= t_date_day; iii++)
                                {
                                    xdate = ii.ToString("00") + "/" + iii.ToString("00") + "/" + i.ToString("0000");
                                    _loop = true;


                                    //============================================================================================

                                    xcol = 1;
                                    worksheet.Cell(xrow, xcol).SetValue(Convert.ToDateTime(xdate).ToLongDateString());
                                    worksheet.Cell(xrow, xcol).Style.Font.FontSize = 10;
                                    worksheet.Cell(xrow, xcol).Style.Font.Bold = true;
                                    worksheet.Cell(xrow, xcol).Style.Font.FontName = "Calibri";
                                    worksheet.Cell(xrow, xcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Merge();
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                    worksheet.Range("A" + xrow + ":J" + xrow).Row(1).Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                    xrow += 1;
                                    xrow = ExcelReport.xstr_excelfile(worksheet, department_code, xdate, xrow);

                                    xrow += 2;
                                }
                            }
                        }
                    }
                }
            }

        }

        worksheet.Cell(xrow, 1).SetValue("****** End of File ******");
        worksheet.Cell(xrow, 1).Style.Font.FontSize = 10;
        worksheet.Cell(xrow, 1).Style.Font.FontName = "Calibri";

        workbook.SaveAs(Server.MapPath("~\\pdf\\" + HttpContext.Current.Session["username"] + "\\") + filename);

        worksheet.Dispose();
        workbook.Dispose();
        x = "Daily Time Record Generated...";


        return x;
    }

    [WebMethod(EnableSession = true)]
    public string email_alert(string email_to, string email_cc, string department, string date_range)
    {

        //Try
        //    If HttpContext.Current.Session["username") Is Nothing Then
        //        Return "<span style='color:red'>Session Time-out. Please Log-in Again</span>"
        //        Exit Function
        //    End If
        //Catch ex As Exception
        //    Return "<span style='color:red'>Session Time-out. Please Log-in Again</span>"
        //    Exit Function
        //End Try

        string x = "";


        string str = "<span style='font-size:10pt; font-weight:bold; font-family:Arial;'> ";
        str = str + "NUTRATECH BIOPHARMA INC. ";
        str = str + "<br />  ";
        str = str + "DAILY TIME RECORD MONITORING ";
        str = str + "<br />  ";
        str = str + "<br />  ";
        str = str + "DEPARTMENT : " + department;
        str = str + "<br />  ";
        str = str + "DATE RANGE : " + date_range;
        str = str + "<br /> ";
        str = str + "<br /> ";
        str = str + "<br /> ";
        str = str + "See Attachment... ";
        str = str + "<br /> ";
        str = str + "This is sent by a system generated e-mail and please do not reply. ";
        str = str + "</span> ";

        str = str + "<br /> ";
        str = str + "<br /> ";
        str = str + "<hr /> ";
        str = str + "<span style='font-family:Times New Roman; font-size:8pt'> ";
        str = str + "<b><p>Email Confidentiality Disclaimer:</b> The information in this electronic message is confidential and/or privileged, and intended for the exclusive use of the ";
        str = str + "addressee. If you are not the intended recipient, you are notified that disclosure, retention, dissemination, copying, alteration and distribution of this ";
        str = str + "communication and/or any attachment, or any part thereof or information therein, are strictly prohibited. If you receive this communication and any ";
        str = str + "attachments in error, kindly notify the sender immediately by email and delete this communication and all attachments. Any views or opinions presented in ";
        str = str + "this email are solely those of the author and do not necessarily represent those of Nutratech Biopharma Inc. </p>";
        str = str + "</span> ";


        try
        {
            SmtpClient smtpServer = new SmtpClient();
            
            smtpServer.Credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["email"].ToString().Trim(), System.Configuration.ConfigurationManager.AppSettings["email_password"].ToString().Trim());
            smtpServer.Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["email_port"].ToString().Trim());
            smtpServer.Host = System.Configuration.ConfigurationManager.AppSettings["email_server"].ToString().Trim();
            smtpServer.EnableSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["email_ssl"].ToString().Trim());
            smtpServer.Timeout = 500000000;

            string eFile = System.Web.HttpContext.Current.Server.MapPath("~\\pdf\\" + HttpContext.Current.Session["username"] + "\\Nutratech_DTR.xlsx");

            Attachment oAttch = new Attachment(eFile);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["email"].ToString().Trim(), " Nutratech Biopharma Inc.");
            mail.To.Add(email_to);
            if (email_cc.Trim() != "")
            {
                mail.CC.Add(email_cc);
            }

            mail.Subject = "DAILY TIME RECORD MONITORING FORM - Department : " + department + " / Date Range : " + date_range;

            mail.IsBodyHtml = true;
            mail.Body = str;
            mail.Attachments.Add(oAttch);

            smtpServer.Send(mail);
            mail.Dispose();
            smtpServer.Dispose();
            x = "DAILY TIME RECORD MONITORING SENT... <br />Department : " + department + "<br />Date Range : " + date_range;

        }
        catch (Exception ex)
        {
            x = "<span style='color:red'>" + ex.Message + "</span>";

        }
        return x;
    }

    [WebMethod(EnableSession = true)]
    public string read_efile()
    {
        string x = "";

        try
        {

            string filename = Server.MapPath("~\\pdf\\" + HttpContext.Current.Session["username"] + "\\Nutratech_DTR.xlsx");


            if (File.Exists(filename))
            {
                ExcelReport.delete_efile();

                XLWorkbook workbook = new XLWorkbook(filename);
                IXLWorksheet ws = workbook.Worksheet(1);

                IXLCell cell = ws.Cell(8, 1);
                string cell_validator = cell.GetString();

                int xrow = 8;
                string xdate = "";
                while (cell_validator != "****** End of File ******")
                {
                    cell = ws.Cell(xrow, 1);

                    IXLCell cell1 = ws.Cell(xrow, 1);
                    IXLCell cell2 = ws.Cell(xrow, 2);
                    IXLCell cell3 = ws.Cell(xrow, 3);
                    IXLCell cell4 = ws.Cell(xrow, 4);
                    IXLCell cell5 = ws.Cell(xrow, 5);
                    IXLCell cell6 = ws.Cell(xrow, 6);
                    IXLCell cell7 = ws.Cell(xrow, 7);
                    IXLCell cell8 = ws.Cell(xrow, 8);
                    IXLCell cell9 = ws.Cell(xrow, 9);

                    string _date_validator = cell1.GetString();
                    try
                    {
                        _date_validator = Convert.ToDateTime(_date_validator).ToShortDateString();
                        xdate = Convert.ToDateTime(_date_validator).ToShortDateString();

                    }
                    catch (Exception ex)
                    {
                    }

                    int _no = -1;
                    string _stremployeeid = "";
                    string _name = "";
                    string _time1 = "";
                    string _time2 = "";
                    string _time3 = "";
                    string _time4 = "";
                    string _log_type = "";
                    string _remarks = "";

                    try
                    {
                        _no = Int32.Parse(cell1.GetString().Replace(".", ""));
                          _stremployeeid = cell2.GetString();
                          _name = cell3.GetString();
                          _time1 = cell4.GetString();
                          _time2 = cell5.GetString();
                          _time3 = cell6.GetString();
                          _time4 = cell7.GetString();
                          _log_type = cell8.GetString().ToUpper().Trim();
                          _remarks = cell9.GetString().Trim();
                    }
                    catch (Exception ex)
                    {
                    }

                    try
                    {

                        if (_time1.Trim() != "")
                        {
                            decimal myDec = Convert.ToDecimal(_time1);

                            DateTime myDate = System.DateTime.FromOADate(Convert.ToDouble(myDec));
                            _time1 = myDate.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        // x = "Error Message : " & ex.Message
                    }
                    try
                    {

                        if (_time2.Trim() != "")
                        {
                            decimal myDec = Convert.ToDecimal(_time2);

                            DateTime myDate = System.DateTime.FromOADate(Convert.ToDouble(myDec));
                            _time2 = myDate.ToString();

                        }
                    }
                    catch (Exception ex)
                    {
                        //  x = "Error Message : " & ex.Message
                    }
                    try
                    {

                        if (_time3.Trim() != "")
                        {
                            decimal myDec = Convert.ToDecimal(_time3);

                            DateTime myDate = System.DateTime.FromOADate(Convert.ToDouble(myDec));
                            _time3 = myDate.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        //    x = "Error Message : " & ex.Message
                    }
                    try
                    {
                        if (_time4.Trim() != "")
                        {
                            decimal myDec = Convert.ToDecimal(_time4);

                            DateTime myDate = System.DateTime.FromOADate(Convert.ToDouble(myDec));
                            _time4 = myDate.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        //   x = "Error Message : " & ex.Message
                    }

                    int _pin = 0;
                    int _employeeid = 0;

                    string _Timedata = "";
                    string _InOut = "";

                    int _Count = 0;
                    bool _Append = true;
                    string _WorkStation = "HRD07";
                    int _Approval = 4;
                    bool _Encode = false;
                    string _Type = "MO";
                    string _DateStamp = System.DateTime.Now.ToString();

                    string _constring = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    if (_no > 0)
                    {
                        string sql = "SELECT EmployeeID, PIN, strEmployeeID FROM Employees WHERE        (strEmployeeID = '" + _stremployeeid + "')";
                        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringEDS"].ConnectionString);
                        SqlCommand cmd = new SqlCommand(sql, cn);
                        SqlDataReader dtr;

                        cn.Open();
                        dtr = cmd.ExecuteReader();

                        if (dtr.Read())
                        {
                            _employeeid = Convert.ToInt32(dtr["EmployeeID"].ToString().Trim());
                            _pin = Convert.ToInt32(dtr["PIN"].ToString().Trim());

                        }
                        dtr.Close();
                        cn.Close();
                        cn.Dispose();
                        if (_log_type != "" & _remarks != "")
                        {
                            _remarks = _log_type + " - " + _remarks;
                        }
                        else if (_log_type != "" & _remarks == "")
                        {
                            _remarks = _log_type + " - Official Business";
                        }


                        for (int i = 1; i <= 4; i++)
                        {
                            if (i == 1)
                            {
                                if (_time1 != "")
                                {
                                    _Count = i;
                                    _Timedata = Convert.ToDateTime(xdate + " " + Convert.ToDateTime(_time1).ToShortTimeString()).ToString();
                                    _InOut = "IN";
                                    ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _Count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                                    _Encode, _Type, _DateStamp, _name, _constring);
                                }
                            }
                            else if (i == 2)
                            {
                                if (_time2 != "")
                                {
                                    _Count = i;
                                    _Timedata = Convert.ToDateTime(xdate + " " + Convert.ToDateTime(_time2).ToShortTimeString()).ToString();
                                    _InOut = "OUT";
                                    ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _Count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                                    _Encode, _Type, _DateStamp, _name, _constring);
                                }
                            }
                            else if (i == 3)
                            {
                                if (_time3 != "")
                                {
                                    _Count = i;
                                    _Timedata = Convert.ToDateTime(xdate + " " + Convert.ToDateTime(_time3).ToShortTimeString()).ToString();
                                    _InOut = "IN";
                                    ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _Count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                                    _Encode, _Type, _DateStamp, _name, _constring);
                                }
                            }
                            else if (i == 4)
                            {
                                if (_time4 != "")
                                {
                                    _Count = i;
                                    _Timedata = Convert.ToDateTime(xdate + " " + Convert.ToDateTime(_time4).ToShortTimeString()).ToString();
                                    _InOut = "OUT";
                                    ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _Count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                                    _Encode, _Type, _DateStamp, _name, _constring);
                                }
                            }
                        }

                        if (_remarks.IndexOf("OB") > -1)
                        {
                            for (int i = (_Count + 1); i <= 4; i++)
                            {
                                if (i == 1)
                                {
                                    if (_time1 == "")
                                    {
                                        _Count = i;
                                        _Timedata = Convert.ToDateTime(xdate + " 8:00:01 AM").ToString();
                                        _InOut = "IN";
                                        ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _Count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                                        _Encode, _Type, _DateStamp, _name, _constring);
                                    }
                                    else
                                    {
                                        if (_time1 != "")
                                        {
                                            _Count = i;
                                            _Timedata = Convert.ToDateTime(xdate + " " + Convert.ToDateTime(_time1).ToShortTimeString()).ToString();
                                            _InOut = "IN";
                                            ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _Count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                                            _Encode, _Type, _DateStamp, _name, _constring);
                                        }
                                    }
                                }
                                else if (i == 2)
                                {
                                    if (_time2 == "")
                                    {
                                        _Count = i;
                                        _Timedata = Convert.ToDateTime(xdate + " 5:00:01 PM").ToString();
                                        _InOut = "OUT";
                                        ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _Count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                                        _Encode, _Type, _DateStamp, _name, _constring);
                                        break; // TODO: might not be correct. Was : Exit For
                                    }
                                    else
                                    {
                                        if (_time2 != "")
                                        {
                                            _Count = i;
                                            _Timedata = Convert.ToDateTime(xdate + " " + Convert.ToDateTime(_time2).ToShortTimeString()).ToString();
                                            _InOut = "OUT";
                                            ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _Count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                                            _Encode, _Type, _DateStamp, _name, _constring);
                                        }
                                    }
                                }
                                else if (i == 3)
                                {
                                    if (_time3 == "")
                                    {
                                        _Count = i;
                                        _Timedata = Convert.ToDateTime(xdate + " 1:00:01 PM").ToString();
                                        _InOut = "IN";
                                        ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _Count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                                        _Encode, _Type, _DateStamp, _name, _constring);
                                    }
                                    else
                                    {
                                        if (_time3 != "")
                                        {
                                            _Count = i;
                                            _Timedata = Convert.ToDateTime(xdate + " " + Convert.ToDateTime(_time3).ToShortTimeString()).ToString();
                                            _InOut = "IN";
                                            ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _Count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                                            _Encode, _Type, _DateStamp, _name, _constring);
                                        }
                                    }
                                }
                                else if (i == 4)
                                {
                                    if (_time4 == "")
                                    {
                                        _Count = i;
                                        _Timedata = Convert.ToDateTime(xdate + " 5:00:01 PM").ToString();
                                        _InOut = "OUT";
                                        ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _Count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                                        _Encode, _Type, _DateStamp, _name, _constring);
                                    }
                                    else
                                    {
                                        if (_time4 != "")
                                        {
                                            _Count = i;
                                            _Timedata = Convert.ToDateTime(xdate + " " + Convert.ToDateTime(_time4).ToShortTimeString()).ToString();
                                            _InOut = "OUT";
                                            ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _Count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                                            _Encode, _Type, _DateStamp, _name, _constring);
                                        }
                                    }
                                }
                            }
                        }


                    }

                    cell_validator = cell.GetString();
                    xrow += 1;

                }
                ws.Dispose();
                workbook.Dispose();


            }
        }
        catch (Exception ex)
        {
            x = "Error Message : " + ex.Message;
        }

        return x;
    }

    [WebMethod(EnableSession = true)]
    public string read_excel_header()
    {


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }

        string x = "";
        string filename = Server.MapPath("~\\pdf\\" + HttpContext.Current.Session["username"] + "\\Nutratech_DTR.xlsx");

        StringBuilder sb = new StringBuilder();





        if (File.Exists(filename))
        {
            XLWorkbook workbook = new XLWorkbook(filename);
            IXLWorksheet ws = workbook.Worksheet(1);

            IXLCell cell = ws.Cell(4, 1);
            string cell_department = cell.GetString();

            cell = ws.Cell(3, 1);
            string cell_daterange = cell.GetString();
            cell_daterange = cell_daterange.Replace("DATE RANGE : ", "");

            sb.Append("[");
            sb.Append("{");

            try
            {
                sb.Append("\"department\":\"" + cell_department.Replace("DEPARTMENT : ", "") + "\",");
            }
            catch (Exception ex)
            {
                sb.Append("\"department\":\"" + "" + "\",");
            }
            try
            {
                sb.Append("\"date_from\":\"" + cell_daterange.Substring(0, cell_daterange.IndexOf(" - ")).Trim() + "\",");
            }
            catch (Exception ex)
            {
                sb.Append("\"date_from\":\"" + "" + "\",");
            }
            try
            {
                sb.Append("\"date_to\":\"" + cell_daterange.Substring(cell_daterange.IndexOf(" - ") + 3).Trim() + "\"");
            }
            catch (Exception ex)
            {
                sb.Append("\"date_to\":\"" + "" + "\"");
            }

            sb.Append("}");

            sb.Append("]");
            ws.Dispose();
            workbook.Dispose();

        }

        return sb.ToString();


    }

    [WebMethod(EnableSession = true)]
    public string Load_Temp_Log_Employee()
    {


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }

        string sql = "SELECT DISTINCT name, strEmployeeID FROM temp_TimeLog ORDER BY name";

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, cn);
        SqlDataReader dtr;
        cn.Open();
        dtr = cmd.ExecuteReader();
        StringBuilder sb = new StringBuilder();
        Boolean hasMoreRecords = false;

        sb.Append("[");

        while (dtr.Read())
        {
            if (hasMoreRecords)
            {
                sb.Append(",");
            }

            sb.Append("{");

            try
            {
                sb.Append("\"employeeid\":\"" + dtr["strEmployeeID"].ToString().Trim() + "\",");
            }
            catch (Exception ex)
            {
                sb.Append("\"employeeid\":\"" + "" + "\",");
            }

            try
            {
                sb.Append("\"name\":\"" + dtr["name"].ToString().Trim() + "\"");
            }
            catch (Exception ex)
            {
                sb.Append("\"name\":\"" + "" + "\"");
            }

            sb.Append("}");

            hasMoreRecords = true;


        }
        dtr.Close();
        cn.Close();
        cn.Dispose();

        sb.Append("]");


        return sb.ToString();


    }

    [WebMethod(EnableSession = true)]
    public string Load_Temp_Log(string employeeid)
    {


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }

        StringBuilder sb = new StringBuilder();
        StringBuilder sb_temp = new StringBuilder();

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dtr;
        SqlDataAdapter adapter = new SqlDataAdapter();

        adapter.InsertCommand = cmd;
        adapter.InsertCommand.Connection = cn;

        adapter.InsertCommand.CommandText = "sp_hr_temp_log_read";
        adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
        adapter.InsertCommand.CommandTimeout = 300;

        SqlParameter _param = new SqlParameter("@employeeid", SqlDbType.NVarChar, 50);
        _param.Value = employeeid;
        _param.Direction = ParameterDirection.Input;
        adapter.InsertCommand.Parameters.Add(_param);

        adapter.InsertCommand.Connection.Open();
        dtr = adapter.InsertCommand.ExecuteReader();

        Boolean hasMoreRecords = false;

        sb.Append("[");

        while (dtr.Read())
        {
            sb_temp.Clear();

            sb_temp.Append("{");
            int _changes = 0;
            string _verify_time = "";
            try
            {
                sb_temp.Append("\"Timedata\":\"" + Convert.ToDateTime(dtr["Timedata"].ToString().Trim()).ToLongDateString() + "\",");
            }
            catch (Exception ex)
            {
                sb_temp.Append("\"Timedata\":\"" + "" + "\",");
            }

            try
            {
                _verify_time = ExcelReport.verifytime_day_efile(employeeid, Convert.ToDateTime(dtr["Timedata"].ToString().Trim()).ToShortDateString(), 1);
                if (_verify_time != "")
                {
                    if (Convert.ToDateTime(dtr["Timein_am"].ToString().Trim()).ToShortTimeString() == Convert.ToDateTime(_verify_time).ToShortTimeString())
                    {
                        sb_temp.Append("\"Timein_am\":\"" + Convert.ToDateTime(dtr["Timein_am"].ToString().Trim()).ToShortTimeString() + "\",");
                    }
                    else
                    {
                        sb_temp.Append("\"Timein_am\":\"" + Convert.ToDateTime(dtr["Timein_am"].ToString().Trim()).ToShortTimeString() + "<br />(" + Convert.ToDateTime(_verify_time).ToShortTimeString() + ")" + "\",");
                        _changes = 1;
                    }
                }
                else
                {
                    if (dtr["Timein_am"].ToString().Trim() != "")
                    {
                        try
                        {
                            try
                            {
                                if (Convert.ToDateTime(dtr["Timein_am"].ToString().Trim()).ToShortTimeString() == Convert.ToDateTime(_verify_time).ToShortTimeString())
                                {
                                    sb_temp.Append("\"Timein_am\":\"" + Convert.ToDateTime(dtr["Timein_am"].ToString().Trim()).ToShortTimeString() + "\",");
                                }
                                else
                                {
                                    sb_temp.Append("\"Timein_am\":\"" + Convert.ToDateTime(dtr["Timein_am"].ToString().Trim()).ToShortTimeString() + "\",");
                                    _changes = 2;
                                }
                            }
                            catch (Exception ex)
                            {
                                sb_temp.Append("\"Timein_am\":\"" + Convert.ToDateTime(dtr["Timein_am"].ToString().Trim()).ToShortTimeString() + "\",");
                                _changes = 2;
                            }
                        }
                        catch (Exception ex)
                        {
                            sb_temp.Append("\"Timein_am\":\"" + "" + "\",");
                        }
                    }
                    else
                    {
                        sb_temp.Append("\"Timein_am\":\"" + "" + "\",");
                    }
                }
            }
            catch (Exception ex)
            {
                sb_temp.Append("\"Timein_am\":\"" + "" + "\",");
            }

            try
            {
                _verify_time = ExcelReport.verifytime_day_efile(employeeid, Convert.ToDateTime(dtr["Timedata"].ToString().Trim()).ToShortDateString(), 2);
                if (_verify_time != "")
                {
                    if (Convert.ToDateTime(_verify_time) < Convert.ToDateTime(Convert.ToDateTime(dtr["Timedata"].ToString().Trim()).ToLongDateString() + " 1:00:01 PM"))
                    {
                        if (Convert.ToDateTime(dtr["Timeout_am"].ToString().Trim()).ToShortTimeString() == Convert.ToDateTime(_verify_time).ToShortTimeString())
                        {
                            sb_temp.Append("\"Timeout_am\":\"" + Convert.ToDateTime(dtr["Timeout_am"].ToString().Trim()).ToShortTimeString() + "\",");
                        }
                        else
                        {
                            sb_temp.Append("\"Timeout_am\":\"" + Convert.ToDateTime(dtr["Timeout_am"].ToString().Trim()).ToShortTimeString() + "<br />(" + Convert.ToDateTime(_verify_time).ToShortTimeString() + ")" + "\",");
                            _changes = 1;
                        }
                    }
                    else
                    {
                        if (dtr["Timeout_am"].ToString().Trim() != "")
                        {
                            try
                            {
                                try
                                {
                                    if (Convert.ToDateTime(dtr["Timeout_am"].ToString().Trim()).ToShortTimeString() == Convert.ToDateTime(_verify_time).ToShortTimeString())
                                    {
                                        sb_temp.Append("\"Timeout_am\":\"" + Convert.ToDateTime(dtr["Timeout_am"].ToString().Trim()).ToShortTimeString() + "\",");
                                    }
                                    else
                                    {
                                        sb_temp.Append("\"Timeout_am\":\"" + Convert.ToDateTime(dtr["Timeout_am"].ToString().Trim()).ToShortTimeString() + "\",");
                                        _changes = 2;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    sb_temp.Append("\"Timeout_am\":\"" + Convert.ToDateTime(dtr["Timeout_am"].ToString().Trim()).ToShortTimeString() + "\",");
                                    _changes = 2;
                                }
                            }
                            catch (Exception ex)
                            {
                                sb_temp.Append("\"Timeout_am\":\"" + "" + "\",");
                            }
                        }
                        else
                        {
                            sb_temp.Append("\"Timeout_am\":\"" + "" + "\",");
                        }
                    }
                }
                else
                {
                    if (dtr["Timeout_am"].ToString().Trim() != "")
                    {
                        try
                        {
                            try
                            {
                                if (Convert.ToDateTime(dtr["Timeout_am"].ToString().Trim()).ToShortTimeString() == Convert.ToDateTime(_verify_time).ToShortTimeString())
                                {
                                    sb_temp.Append("\"Timeout_am\":\"" + Convert.ToDateTime(dtr["Timeout_am"].ToString().Trim()).ToShortTimeString() + "\",");
                                }
                                else
                                {
                                    sb_temp.Append("\"Timeout_am\":\"" + Convert.ToDateTime(dtr["Timeout_am"].ToString().Trim()).ToShortTimeString() + "\",");
                                    _changes = 2;
                                }
                            }
                            catch (Exception ex)
                            {
                                sb_temp.Append("\"Timeout_am\":\"" + Convert.ToDateTime(dtr["Timeout_am"].ToString().Trim()).ToShortTimeString() + "\",");
                                _changes = 2;
                            }
                        }
                        catch (Exception ex)
                        {
                            sb_temp.Append("\"Timeout_am\":\"" + "" + "\",");
                        }
                    }
                    else
                    {
                        sb_temp.Append("\"Timeout_am\":\"" + "" + "\",");
                    }
                }
            }
            catch (Exception ex)
            {
                sb_temp.Append("\"Timeout_am\":\"" + "" + "\",");
            }

            try
            {
                _verify_time = ExcelReport.verifytime_day_efile(employeeid, Convert.ToDateTime(dtr["Timedata"].ToString().Trim()).ToShortDateString(), 3);
                if (_verify_time != "")
                {
                    if (Convert.ToDateTime(dtr["Timein_pm"].ToString().Trim()).ToShortTimeString() == Convert.ToDateTime(_verify_time).ToShortTimeString())
                    {
                        sb_temp.Append("\"Timein_pm\":\"" + Convert.ToDateTime(dtr["Timein_pm"].ToString().Trim()).ToShortTimeString() + "\",");
                    }
                    else
                    {
                        sb_temp.Append("\"Timein_pm\":\"" + Convert.ToDateTime(dtr["Timein_pm"].ToString().Trim()).ToShortTimeString() + "<br />(" + Convert.ToDateTime(_verify_time).ToShortTimeString() + ")" + "\",");
                        _changes = 1;
                    }
                }
                else
                {
                    if (dtr["Timein_pm"].ToString().Trim() != "")
                    {
                        try
                        {
                            try
                            {
                                if (Convert.ToDateTime(dtr["Timein_pm"].ToString().Trim()).ToShortTimeString() == Convert.ToDateTime(_verify_time).ToShortTimeString())
                                {
                                    sb_temp.Append("\"Timein_pm\":\"" + Convert.ToDateTime(dtr["Timein_pm"].ToString().Trim()).ToShortTimeString() + "\",");
                                }
                                else
                                {
                                    sb_temp.Append("\"Timein_pm\":\"" + Convert.ToDateTime(dtr["Timein_pm"].ToString().Trim()).ToShortTimeString() + "\",");
                                    _changes = 2;
                                }
                            }
                            catch (Exception ex)
                            {
                                sb_temp.Append("\"Timein_pm\":\"" + Convert.ToDateTime(dtr["Timein_pm"].ToString().Trim()).ToShortTimeString() + "\",");
                                _changes = 2;
                            }
                        }
                        catch (Exception ex)
                        {
                            sb_temp.Append("\"Timein_pm\":\"" + "" + "\",");
                        }
                    }
                    else
                    {
                        sb_temp.Append("\"Timein_pm\":\"" + "" + "\",");
                    }
                }
            }
            catch (Exception ex)
            {
                sb_temp.Append("\"Timein_pm\":\"" + "" + "\",");
            }


            try
            {
                _verify_time = ExcelReport.verifytime_day_efile(employeeid, Convert.ToDateTime(dtr["Timedata"].ToString().Trim()).ToShortDateString(), 2);
                if (_verify_time != "")
                {
                    if (Convert.ToDateTime(_verify_time) > Convert.ToDateTime(Convert.ToDateTime(dtr["Timedata"].ToString().Trim()).ToLongDateString() + " 1:00:01 PM"))
                    {
                        if (Convert.ToDateTime(dtr["Timeout_pm"].ToString().Trim()).ToShortTimeString() == Convert.ToDateTime(_verify_time).ToShortTimeString())
                        {
                            sb_temp.Append("\"Timeout_pm\":\"" + Convert.ToDateTime(dtr["Timeout_pm"].ToString().Trim()).ToShortTimeString() + "\",");
                        }
                        else
                        {
                            sb_temp.Append("\"Timeout_pm\":\"" + Convert.ToDateTime(dtr["Timeout_pm"].ToString().Trim()).ToShortTimeString() + "<br />(" + Convert.ToDateTime(_verify_time).ToShortTimeString() + ")" + "\",");
                            _changes = 1;
                        }
                    }
                    else
                    {
                        _verify_time = ExcelReport.verifytime_day_efile(employeeid, Convert.ToDateTime(dtr["Timedata"].ToString().Trim()).ToShortDateString(), 4);
                        if (_verify_time != "")
                        {
                            if (Convert.ToDateTime(dtr["Timeout_pm"].ToString().Trim()).ToShortTimeString() == Convert.ToDateTime(_verify_time).ToShortTimeString())
                            {
                                sb_temp.Append("\"Timeout_pm\":\"" + Convert.ToDateTime(dtr["Timeout_pm"].ToString().Trim()).ToShortTimeString() + "\",");
                            }
                            else
                            {
                                sb_temp.Append("\"Timeout_pm\":\"" + Convert.ToDateTime(dtr["Timeout_pm"].ToString().Trim()).ToShortTimeString() + "<br />(" + Convert.ToDateTime(_verify_time).ToShortTimeString() + ")" + "\",");
                                _changes = 1;
                            }
                        }
                        else
                        {
                            if (dtr["Timeout_pm"].ToString().Trim() != "")
                            {
                                try
                                {
                                    try
                                    {
                                        if (Convert.ToDateTime(dtr["Timeout_pm"].ToString().Trim()).ToShortTimeString() == Convert.ToDateTime(_verify_time).ToShortTimeString())
                                        {
                                            sb_temp.Append("\"Timeout_pm\":\"" + Convert.ToDateTime(dtr["Timeout_pm"].ToString().Trim()).ToShortTimeString() + "\",");
                                        }
                                        else
                                        {
                                            sb_temp.Append("\"Timeout_pm\":\"" + Convert.ToDateTime(dtr["Timeout_pm"].ToString().Trim()).ToShortTimeString() + "\",");
                                            _changes = 2;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sb_temp.Append("\"Timeout_pm\":\"" + Convert.ToDateTime(dtr["Timeout_pm"].ToString().Trim()).ToShortTimeString() + "\",");
                                        _changes = 2;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    sb_temp.Append("\"Timeout_pm\":\"" + "" + "\",");
                                }
                            }
                            else
                            {
                                sb_temp.Append("\"Timeout_pm\":\"" + "" + "\",");
                            }
                        }
                    }
                }
                else
                {
                    _verify_time = ExcelReport.verifytime_day_efile(employeeid, Convert.ToDateTime(dtr["Timedata"].ToString().Trim()).ToShortDateString(), 4);
                    if (_verify_time != "")
                    {
                        if (Convert.ToDateTime(dtr["Timeout_pm"].ToString().Trim()).ToShortTimeString() == Convert.ToDateTime(_verify_time).ToShortTimeString())
                        {
                            sb_temp.Append("\"Timeout_pm\":\"" + Convert.ToDateTime(dtr["Timeout_pm"].ToString().Trim()).ToShortTimeString() + "\",");
                        }
                        else
                        {
                            sb_temp.Append("\"Timeout_pm\":\"" + Convert.ToDateTime(dtr["Timeout_pm"].ToString().Trim()).ToShortTimeString() + "<br />(" + Convert.ToDateTime(_verify_time).ToShortTimeString() + ")" + "\",");
                            _changes = 1;
                        }
                    }
                    else
                    {
                        if (dtr["Timeout_pm"].ToString().Trim() != "")
                        {
                            try
                            {
                                try
                                {
                                    if (Convert.ToDateTime(dtr["Timeout_pm"].ToString().Trim()).ToShortTimeString() == Convert.ToDateTime(_verify_time).ToShortTimeString())
                                    {
                                        sb_temp.Append("\"Timeout_pm\":\"" + Convert.ToDateTime(dtr["Timeout_pm"].ToString().Trim()).ToShortTimeString() + "\",");
                                    }
                                    else
                                    {
                                        sb_temp.Append("\"Timeout_pm\":\"" + Convert.ToDateTime(dtr["Timeout_pm"].ToString().Trim()).ToShortTimeString() + "\",");
                                        _changes = 2;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    sb_temp.Append("\"Timeout_pm\":\"" + Convert.ToDateTime(dtr["Timeout_pm"].ToString().Trim()).ToShortTimeString() + "\",");
                                    _changes = 2;
                                }
                            }
                            catch (Exception ex)
                            {
                                sb_temp.Append("\"Timeout_pm\":\"" + "" + "\",");
                            }
                        }
                        else
                        {
                            sb_temp.Append("\"Timeout_pm\":\"" + "" + "\",");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                sb_temp.Append("\"Timeout_pm\":\"" + "" + "\",");
            }

            try
            {
                sb_temp.Append("\"Remarks\":\"" + dtr["Remarks"].ToString().Trim() + "\",");
            }
            catch (Exception ex)
            {
                sb_temp.Append("\"Remarks\":\"" + "" + "\",");
            }
            try
            {
                sb_temp.Append("\"changes\":\"" + _changes + "\"");
            }
            catch (Exception ex)
            {
                sb_temp.Append("\"changes\":\"" + "" + "\"");
            }
            sb_temp.Append("}");

            switch (_changes)
            {
                case 1:
                case 2:
                    if (hasMoreRecords)
                    {
                        sb.Append(",");
                    }
                    sb.Append(sb_temp);
                    hasMoreRecords = true;
                    break;
            }


        }
        dtr.Close();
        adapter.InsertCommand.Connection.Close();
        adapter.InsertCommand.Connection.Dispose();

        sb.Append("]");


        return sb.ToString();


    }

    [WebMethod(EnableSession = true)]
    public string Save_TimeLog(string _stremployeeid, string _date, string timein_am, string timeout_am, string timein_pm, string timeout_pm, string _remarks, string _ob, string _ct, string _reg)
    {


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }
        String x = "";
        Boolean _apply_changes = false;
        if (_ob == "on")
        {
            if (_remarks.IndexOf("OB") > -1)
            {
                _apply_changes = true;
            }
        }
        if (_ct == "on")
        {
            if (_remarks.IndexOf("CT") > -1)
            {
                _apply_changes = true;
            }
        }
        if (_reg == "on")
        {
            _apply_changes = false;
            if (_remarks.IndexOf("OB") > -1 & _ob == "on")
            {
                _apply_changes = true;
            }
            else if (_remarks.IndexOf("CT") > -1 & _ct == "on")
            {
                _apply_changes = true;
            }
            else if (_remarks.IndexOf("OB") > -1 & _ct == "")
            {
                _apply_changes = false;
            }
            else if (_remarks.IndexOf("CT") > -1 & _ct == "")
            {
                _apply_changes = false;
            }
            else
            {
                if (_remarks == "")
                {
                    _remarks = "No AMPS Available on location";
                    //Else
                    //   _remarks = "No AMPS Available on location / " & _remarks
                }
                _apply_changes = true;
            }
        }


        if (_apply_changes == true)
        {
            try
            {
                if (timein_am.IndexOf("(") > -1)
                {
                    timein_am = timein_am.Substring(0, timein_am.IndexOf("("));
                }

            }
            catch (Exception ex)
            {
            }
            try
            {
                if (timeout_am.IndexOf("(") > -1)
                {
                    timeout_am = timeout_am.Substring(0, timeout_am.IndexOf("("));
                }

            }
            catch (Exception ex)
            {
            }
            try
            {
                if (timein_pm.IndexOf("(") > -1)
                {
                    timein_pm = timein_pm.Substring(0, timein_pm.IndexOf("("));
                }

            }
            catch (Exception ex)
            {
            }
            try
            {
                if (timeout_pm.IndexOf("(") > -1)
                {
                    timeout_pm = timeout_pm.Substring(0, timeout_pm.IndexOf("("));
                }

            }
            catch (Exception ex)
            {
            }


            string sql = "SELECT        MAX(Count) AS Count FROM Timedata ";
            sql = sql + "WHERE        (strEmployeeID = '" + _stremployeeid + "') ";
            sql = sql + "AND (Timedata BETWEEN '" + Convert.ToDateTime(_date).ToShortDateString() + " 00:00:00' AND '" + Convert.ToDateTime(_date).ToShortDateString() + " 23:59:59') ";

            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringEDS"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader dtr;

           
            int _count = 0;
         
            int _pin = 0;
            int _employeeid = 0;

            string _Timedata = "";
            string _InOut = "";


            Boolean _Append = true;
            string _WorkStation = "HRD07";
            int _Approval = 4;
            Boolean _Encode = false;
            string _Type = "MO";
            string _DateStamp = System.DateTime.Now.ToString();

            string _constring = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringEDS"].ConnectionString;

            sql = "SELECT EmployeeID, PIN, strEmployeeID FROM Employees WHERE        (strEmployeeID = '" + _stremployeeid + "')";
            cn = new SqlConnection(_constring);
            cmd = new SqlCommand(sql, cn);

            cn.Open();
            dtr = cmd.ExecuteReader();

            if (dtr.Read())
            {
                _employeeid = Convert.ToInt32(dtr["EmployeeID"].ToString().Trim());
                _pin = Convert.ToInt32(dtr["PIN"].ToString().Trim());

            }
            dtr.Close();
            cn.Close();
            cn.Dispose();

            _constring = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringEDS"].ConnectionString;

            //For i As Integer = 1 To 4

            //If i = 1 Then
            if (timein_am != "")
            {
                _count = 1;
                _Timedata = Convert.ToDateTime(Convert.ToDateTime(_date).ToShortDateString() + " " + Convert.ToDateTime(timein_am).ToShortTimeString()).ToString();
                _InOut = "IN";

                if (ExcelReport.verifytime_day_efile(_stremployeeid, Convert.ToDateTime(_date).ToShortDateString(), _count) == "")
                {
                    if (_remarks.IndexOf("OB") > -1)
                    {
                        ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                    else if (_remarks.IndexOf("CT") > -1)
                    {
                        ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                    else
                    {
                        ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                }
                else
                {
                    if (_remarks.IndexOf("OB") > -1)
                    {
                        ExcelReport.update_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                    else if (_remarks.IndexOf("CT") > -1)
                    {
                        ExcelReport.update_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                    else
                    {
                        ExcelReport.update_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                }

            }
            //ElseIf i = 2 Then
            if (timeout_am != "")
            {
                _count += 1;
                _Timedata = Convert.ToDateTime(Convert.ToDateTime(_date).ToShortDateString() + " " + Convert.ToDateTime(timeout_am).ToShortTimeString()).ToString();
                _InOut = "OUT";

                if (ExcelReport.verifytime_day_efile(_stremployeeid, Convert.ToDateTime(_date).ToShortDateString(), _count) == "")
                {
                    if (_remarks.IndexOf("OB") > -1)
                    {
                        ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                    else if (_remarks.IndexOf("CT") > -1)
                    {
                        ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                    else
                    {
                        ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                }
                else
                {
                    if (_remarks.IndexOf("OB") > -1)
                    {
                        ExcelReport.update_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                    else if (_remarks.IndexOf("CT") > -1)
                    {
                        ExcelReport.update_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                    else
                    {
                        ExcelReport.update_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                }

            }
            //ElseIf i = 3 Then
            if (timein_pm != "")
            {
                _count += 1;
                _Timedata = Convert.ToDateTime(Convert.ToDateTime(_date).ToShortDateString() + " " + Convert.ToDateTime(timein_pm).ToShortTimeString()).ToString();
                _InOut = "IN";

                if (ExcelReport.verifytime_day_efile(_stremployeeid, Convert.ToDateTime(_date).ToShortDateString(), _count) == "")
                {
                    if (_remarks.IndexOf("OB") > -1)
                    {
                        ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                    else if (_remarks.IndexOf("CT") > -1)
                    {
                        ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                    else
                    {
                        ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                }
                else
                {
                    if (_remarks.IndexOf("OB") > -1)
                    {
                        ExcelReport.update_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                    else if (_remarks.IndexOf("CT") > -1)
                    {
                        ExcelReport.update_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                    else
                    {
                        ExcelReport.update_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                }

            }
            //ElseIf i = 4 Then
            if (timeout_pm != "")
            {
                _count += 1;
                _Timedata = Convert.ToDateTime(Convert.ToDateTime(_date).ToShortDateString() + " " + Convert.ToDateTime(timeout_pm).ToShortTimeString()).ToString();
                _InOut = "OUT";

                if (ExcelReport.verifytime_day_efile(_stremployeeid, Convert.ToDateTime(_date).ToShortDateString(), _count) == "")
                {
                    if (_remarks.IndexOf("OB") > -1)
                    {
                        ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                    else if (_remarks.IndexOf("CT") > -1)
                    {
                        ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                    else
                    {
                        ExcelReport.insert_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                }
                else
                {
                    if (_remarks.IndexOf("OB") > -1)
                    {
                        ExcelReport.update_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                    else if (_remarks.IndexOf("CT") > -1)
                    {
                        ExcelReport.update_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                    else
                    {
                        ExcelReport.update_efile(_Timedata, _employeeid, _pin, _stremployeeid, _count, _InOut, _remarks, _Append, _WorkStation, _Approval,
                        _Encode, _Type, _DateStamp, "", _constring);
                    }
                }

            }
            //End If




            //   Next

        }

        x = "Attendance Log Synced...";

        return x;

    }

    [WebMethod(EnableSession = true)]
    public string make_employee_textfile(string _department_code, string _department_name, string email_to, string email_cc)
    {


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }
        string x = "";

        string filename = "Employee_Info_" + _department_name + ".txt";
        string path = Server.MapPath("~\\pdf\\" + HttpContext.Current.Session["username"] + "\\") + filename;
        if (File.Exists(Server.MapPath("~\\pdf\\" + HttpContext.Current.Session["username"] + "\\") + filename))
        {
            try
            {
                File.Delete(Server.MapPath("~\\pdf\\" + HttpContext.Current.Session["username"] + "\\") + filename);

            }
            catch (Exception ex)
            {
            }
        }

        string sql = "SELECT        PIN, CASE WHEN LastName IS NOT NULL THEN rtrim(LastName) + ', ' ELSE '' END + ";
        sql = sql + "CASE WHEN FirstName IS NOT NULL THEN rtrim(FirstName) + ' ' ELSE '' END + ";
        sql = sql + "CASE WHEN MiddleInitial IS NOT NULL THEN rtrim(MiddleInitial) + '. ' ELSE '' END + ";
        sql = sql + "CASE WHEN ExtentionName IS NOT NULL THEN rtrim(ExtentionName) ELSE '' END ";
        sql = sql + "AS name ";
        sql = sql + "FROM            Employees ";
        sql = sql + "WHERE        (Department = " + _department_code + ") AND (Deactivated = 0) ";
        sql = sql + "ORDER BY LastName, FirstName, MiddleInitial ";

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringEDS"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, cn);
        SqlDataReader dtr;

        cn.Open();
        dtr = cmd.ExecuteReader();

        if (!File.Exists(path))
        {
            // Create a file to write to. 
            using (StreamWriter sw = File.CreateText(path))
            {


                while (dtr.Read())
                {
                    sw.WriteLine(dtr["PIN"].ToString().Trim() + "\t" + dtr["name"].ToString().Trim());

                }

            }


        }
        dtr.Close();
        cn.Close();
        cn.Dispose();

        string str = "<span style='font-size:10pt; font-weight:bold; font-family:Arial;'> ";
        str = str + "NUTRATECH BIOPHARMA INC. ";
        str = str + "<br />  ";
        str = str + "DAILY TIME RECORD MONITORING ";
        str = str + "<br />  ";
        str = str + "<br />  ";
        str = str + "DEPARTMENT : " + _department_name;
        str = str + "<br />  ";
        str = str + "DATE : " + System.DateTime.Now;
        str = str + "<br /> ";
        str = str + "<br /> ";
        str = str + "<br /> ";
        str = str + "See Attachment... ";
        str = str + "<br /> ";
        str = str + "This is sent by a system generated e-mail and please do not reply. ";
        str = str + "</span> ";

        str = str + "<br /> ";
        str = str + "<br /> ";
        str = str + "<hr /> ";
        str = str + "<span style='font-family:Times New Roman; font-size:8pt'> ";
        str = str + "<b><p>Email Confidentiality Disclaimer:</b> The information in this electronic message is confidential and/or privileged, and intended for the exclusive use of the ";
        str = str + "addressee. If you are not the intended recipient, you are notified that disclosure, retention, dissemination, copying, alteration and distribution of this ";
        str = str + "communication and/or any attachment, or any part thereof or information therein, are strictly prohibited. If you receive this communication and any ";
        str = str + "attachments in error, kindly notify the sender immediately by email and delete this communication and all attachments. Any views or opinions presented in ";
        str = str + "this email are solely those of the author and do not necessarily represent those of Nutratech Biopharma Inc. </p>";
        str = str + "</span> ";


        try
        {
            SmtpClient smtpServer = new SmtpClient();
            smtpServer.Credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["email"].ToString().Trim(), System.Configuration.ConfigurationManager.AppSettings["email_password"].ToString().Trim());
            smtpServer.Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["email_port"].ToString().Trim());
            smtpServer.Host = System.Configuration.ConfigurationManager.AppSettings["email_server"].ToString().Trim();
            smtpServer.EnableSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["email_ssl"].ToString().Trim());
            smtpServer.Timeout = 500000000;

            Attachment oAttch = new Attachment(path);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["email"].ToString().Trim(), " Nutratech Biopharma Inc.");
            mail.To.Add(email_to);
            if (email_cc.Trim() != "")
            {
                mail.CC.Add(email_cc);
            }

            mail.Subject = "DAILY TIME RECORD MONITORING - Department : " + _department_name + " / Date : " + System.DateTime.Now;

            mail.IsBodyHtml = true;
            mail.Body = str;
            mail.Attachments.Add(oAttch);

            smtpServer.Send(mail);
            mail.Dispose();
            smtpServer.Dispose();
            x = "DAILY TIME RECORD MONITORING - Department : " + _department_name + " / Date : " + System.DateTime.Now;

        }
        catch (Exception ex)
        {
            x = "<span style='color:red'>" + ex.Message + "</span>";

        }

        x = "Employee File Generated and Sent... </br>" + x;

        return x;

    }
    

    [WebMethod(EnableSession = true)]
    public string read_textfile()
    {


        try
        {
            if (HttpContext.Current.Session["username"] == null)
            {
                return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
                
            }
        }
        catch (Exception ex)
        {
            return "<span style='color:red'>Session Time-out. Please Log-in Again</span>";
            
        }

        string x = "";
        string filename = Server.MapPath("~\\pdf\\" + HttpContext.Current.Session["username"] + "\\Nutratech_DTR.txt");
        StringBuilder sb = new StringBuilder();


        if (File.Exists(filename))
        {
            string sql = "DELETE FROM temp_Offsite_Log";
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, cn);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();


            sql = "DELETE FROM temp_TimeLog";
            cmd = new SqlCommand(sql, cn);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();

            //Get a StreamReader class that can be used to read the file
            StreamReader objStreamReader;
            objStreamReader = File.OpenText(filename);


            while (objStreamReader.Peek() != -1)
            {
                string line = objStreamReader.ReadLine();


                if (line.Trim() != "")
                {

                    int _index = Convert.ToInt32(line.Substring(0, line.IndexOf("\t")));
                    line = line.Substring(line.IndexOf("\t") + 1).Trim();

                    int _employee_id = Convert.ToInt32(line.Substring(0, line.IndexOf("\t")));
                    line = line.Substring(line.IndexOf("\t") + 1).Trim();

                    int _inout = Convert.ToInt32(line.Substring(0, line.IndexOf("\t")));
                    line = line.Substring(line.IndexOf("\t") + 1).Trim();

                    string _date = line.Trim();

                    sql = "INSERT INTO temp_Offsite_Log";
                    sql = sql + "(index_, employee_id, InOut, TimeData) ";
                    sql = sql + "VALUES        (";
                    sql = sql + _index + ",";
                    sql = sql + _employee_id + ",";
                    sql = sql + _inout + ",";
                    sql = sql + "'" + _date + "'";
                    sql = sql + ")";

                    cmd = new SqlCommand(sql, cn);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();

                }
            }

            objStreamReader.Close();
            objStreamReader.Dispose();

            ExcelReport.read_textfile_db();



            sql = "SELECT         MIN(TimeData) AS time_in, MAX(TimeData) AS time_out FROM temp_Offsite_Log ";

            SqlConnection cn_time = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SqlCommand cmd_time = new SqlCommand(sql, cn_time);
            SqlDataReader dtr_time;

            string _time_in = "";
            string _time_out = "";
            cn_time.Open();
            dtr_time = cmd_time.ExecuteReader();

            if (dtr_time.Read())
            {
                _time_in = dtr_time["time_in"].ToString().Trim();
                _time_out = dtr_time["time_out"].ToString().Trim();

            }
            dtr_time.Close();
            cn_time.Close();


            sql = "SELECT         employee_id FROM temp_Offsite_Log ";

            cmd_time = new SqlCommand(sql, cn_time);


            string _department = "";

            cn_time.Open();
            dtr_time = cmd_time.ExecuteReader();

            if (dtr_time.Read())
            {

                sql = "SELECT        Departments.DepartmentName ";
                sql = sql + "FROM            Employees INNER JOIN ";
                sql = sql + "Departments ON Employees.Department = Departments.DepartmentNumber ";
                sql = sql + "WHERE        (Employees.PIN = " + dtr_time["employee_id"].ToString().Trim() + ")";
                SqlConnection cn_eds = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringEDS"].ConnectionString);
                SqlCommand cmd_eds = new SqlCommand(sql, cn_eds);
                SqlDataReader dtr_eds;

                cn_eds.Open();
                dtr_eds = cmd_eds.ExecuteReader();

                if (dtr_eds.Read())
                {
                    _department = dtr_eds["DepartmentName"].ToString().Trim();

                }
                dtr_eds.Close();
                cn_eds.Close();
                cn_eds.Dispose();

            }
            dtr_time.Close();
            cn_time.Close();
            cn_time.Dispose();

            sb.Append("[");
            sb.Append("{");

            try
            {
                sb.Append("\"department\":\"" + _department + "\",");
            }
            catch (Exception ex)
            {
                sb.Append("\"department\":\"" + "" + "\",");
            }
            try
            {
                sb.Append("\"date_from\":\"" + Convert.ToDateTime(_time_in).ToShortDateString() + "\",");
            }
            catch (Exception ex)
            {
                sb.Append("\"date_from\":\"" + "" + "\",");
            }
            try
            {
                sb.Append("\"date_to\":\"" + Convert.ToDateTime(_time_out).ToShortDateString() + "\"");
            }
            catch (Exception ex)
            {
                sb.Append("\"date_to\":\"" + "" + "\"");
            }
            sb.Append("}");
            sb.Append("]");
        }
        return sb.ToString();
    }
    
    #endregion


    #region filter
   
    [WebMethod(EnableSession = true)]
    public List<ItemDTO> getItems(String Obj, String ItemType, String ItemClass, 
                                  String ItemCategory)
    {
        List<ItemDTO> items = new List<ItemDTO>();
        
        String sql = "SELECT [item_cd] ,[descs], [item_type_cd],[item_category_cd],[item_class_cd] FROM [Nutratech_DB].[dbo].[item]" +
                     "WHERE item_cd IS NOT Null"; 
       
        //if (ItemType != "All")
            sql = sql + " AND item_type_cd ='" + ItemType + "'";
        //if (ItemClass != "All")
            sql = sql + " AND item_class_cd='" + ItemClass + "'";
        //if (ItemCategory != "All")
            sql = sql + " AND item_category_cd='" + ItemCategory + "'";

        if (ItemType != "All")
            System.Diagnostics.Debug.WriteLine("The value of ItemType is : " + ItemType );
        if (ItemClass != "All")
            System.Diagnostics.Debug.WriteLine("The value of ItemClass is : " + ItemClass);
        if (ItemCategory != "All")
            System.Diagnostics.Debug.WriteLine("The value of ItemCategory is : " + ItemCategory);
        
        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, cn);
        SqlDataReader dtr;
        cn.Open();
        dtr = cmd.ExecuteReader();   
        while (dtr.Read())
        {
            items.Add(new ItemDTO
            {
                item_cd = dtr["item_cd"].ToString().Trim(),
                descs = dtr["descs"].ToString().Trim(),
                item_type_cd = dtr["item_type_cd"].ToString().Trim(),
                item_category_cd = dtr["item_category_cd"].ToString().Trim(),
                item_class_cd = dtr["item_class_cd"].ToString().Trim()              
            });           
        }
        cn.Close();
        cn.Dispose();
        JavaScriptSerializer ser = new JavaScriptSerializer();
        System.Diagnostics.Debug.WriteLine("the items from [Nutratech_DB].[dbo].[item] are : " + ser.Serialize(items));
        return items;
    }

    [WebMethod(EnableSession = true)]
    public List<ListItemDTO>  getDropDownListItems(String Obj) {

        List<ListItemDTO> items = new List<ListItemDTO>();
        String sql = "";
        switch(Obj)
        {
            case "#ContentPlaceHolder1_DropDownListItemClass":
                        sql = @"SELECT [code],[descs] FROM [Nutratech_DB].[dbo].[item_class];";
                        break;
            case "#ContentPlaceHolder1_DropDownListItemType":
                        sql = @"SELECT [code],[descs] FROM [Nutratech_DB].[dbo].[item_type] Order by code;";
                        break;
            case "#ContentPlaceHolder1_DropDownListItemCategory":
                        sql = @"SELECT [code],[descs] FROM [Nutratech_DB].[dbo].[item_category];";
                        break;
            case "#ContentPlaceHolder1_DropDownListCompany":
                        sql = @"SELECT [CompanyCode],[DatabaseName],[CompanyName]FROM [NB-COMMON].[dbo].[COMPANY]
                                WHERE DatabaseName != 'DB_100' AND DatabaseName != 'DB_QA';;";
                        break;
            case "#ContentPlaceHolder1_DD_vehicle_plate_number":
                        sql = @"SELECT [name],[descs],[plate_no]    
                                FROM [Nutratech_DB].[dbo].[vehicle]";
                        break;
            case "#ContentPlaceHolder1_DD_driver_username":
                        sql = @"SELECT [username],[name]      
                                FROM [Nutratech_DB].[dbo].[cf_users]";
                        break;
            case "#ContentPlaceHolder1_va_DD_accessory_group":
                        sql = @"SELECT [code],[descs],[audit_user],[audit_date]
                                FROM [Nutratech_DB].[dbo].[accessory_group]";
                        break;
            case "#ContentPlaceHolder1_DD_accessory_group":
                        sql = @"SELECT [code],[descs],[audit_user],[audit_date]
                                FROM [Nutratech_DB].[dbo].[accessory_group]";
                        break;
            case "#ContentPlaceHolder1_DD_accessory":
                        sql = @"SELECT [code],[descs],[audit_user],[audit_date]
                                FROM [Nutratech_DB].[dbo].[accessory]";
                        break;          
            
        }

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, cn);
        SqlDataReader dtr;
        cn.Open();
        dtr = cmd.ExecuteReader();
        while (dtr.Read())
        {
            if (Obj == "#ContentPlaceHolder1_DropDownListCompany")
                items.Add(new ListItemDTO
                {
                    code = dtr["DatabaseName"].ToString().Trim(),
                    descs = dtr["CompanyName"].ToString().Trim()
                });
            else if (Obj == "#ContentPlaceHolder1_DD_vehicle_plate_number")
                items.Add(new ListItemDTO
                {
                    code = dtr["plate_no"].ToString().Trim(),
                    descs = dtr["name"].ToString().Trim()
                });
            else if (Obj == "#ContentPlaceHolder1_DD_driver_username")
                items.Add(new ListItemDTO
                {
                    code = dtr["username"].ToString().Trim(),
                    descs = dtr["name"].ToString().Trim()
                });
            else
                items.Add(new ListItemDTO
                {
                    code = dtr["code"].ToString().Trim(),
                    descs = dtr["descs"].ToString().Trim() 
                });
        }
        cn.Close();
        cn.Dispose();
        return items;
    }

    [WebMethod(EnableSession = true)]
    public List<ListItemDTO> getItemsOf(String Company, String SearchString, String SelectedItemCode, String SelectedItemDescs,
                                        String SelectedItemClass, String SelectedItemType, String SelectedItemCategory)
    {
        List<ListItemDTO> items = new List<ListItemDTO>();

        List<ItemNBMatrixDTO> assignedItems = getItemMatrix(SelectedItemType, SelectedItemClass, SelectedItemCategory, SelectedItemCode);

        String sql = "SELECT [ItemCode],[Description] FROM [" + Company + "].[dbo].[ITEMMASTER]";
        //if (SearchString != "" || SearchString != null)
        sql = sql + "WHERE (ItemCode like '%" + SearchString + "%' OR  Description like '%" + SearchString + "%')";

        foreach (ItemNBMatrixDTO item in assignedItems)
        {
            sql = sql + "AND ItemCode != '" + item.code + "' ";
        }
        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, cn);
        SqlDataReader dtr;
        cn.Open();
        dtr = cmd.ExecuteReader();
        while (dtr.Read())
        {
            items.Add(new ListItemDTO
            {
                code = dtr["ItemCode"].ToString().Trim(),
                descs = dtr["Description"].ToString().Trim()
            });
        }
        cn.Close();
        cn.Dispose();
        return items;
    }

    [WebMethod(EnableSession = true)]
    public List<ItemNBMatrixDTO> getItemMatrix(String item_type_cd, String item_class_cd, String item_category_cd, String item_cd) 
    {
        if (item_type_cd == null || item_category_cd == null || item_class_cd == null || item_cd == null)
            return null;
        List<ItemNBMatrixDTO> items = new List<ItemNBMatrixDTO>();

        String sql = "SELECT item_type_cd,item_class_cd,item_category_cd,item_cd,audit_user,audit_date,notebook_item_cd,notebook_item_descs, company_cd FROM [Nutratech_DB].[dbo].[item_notebook_matrix]" +
                     @"WHERE item_type_cd = @item_type_cd AND item_class_cd = @item_class_cd AND item_category_cd=@item_category_cd AND item_cd=@item_cd";
                
        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, cn);

        cmd.Parameters.AddWithValue("item_type_cd", item_type_cd);
        cmd.Parameters.AddWithValue("item_class_cd", item_class_cd);
        cmd.Parameters.AddWithValue("item_category_cd", item_category_cd);
        cmd.Parameters.AddWithValue("item_cd", item_cd);

        SqlDataReader dtr;
        cn.Open();
        dtr = cmd.ExecuteReader();
        
        while (dtr.Read())
        {
            items.Add(new ItemNBMatrixDTO
            {
                code = dtr["notebook_item_cd"].ToString().Trim(),
                descs = dtr["notebook_item_descs"].ToString().Trim(),
                company_cd = dtr["company_cd"].ToString().Trim()
            });
        }
        dtr.Close();
        cn.Close();        
        return items;
    }
    
//    [WebMethod(EnableSession=true)]
//    public ItemDTO insertToMatrixDb(String item_cd, String audit_user, DateTime audit_date, 
//                                    String notebook_item_cd, String notebook_item_descs, 
//                                    String company_cd)
//    {

//        String sql = @"INSERT INTO [Nutratech_DB].[dbo].[item_notebook_matrix]
//                       ([item_type_cd]
//                       ,[item_class_cd]
//                       ,[item_category_cd]
//                       ,[item_cd]
//                       ,[audit_user]
//                       ,[audit_date]
//                       ,[notebook_item_cd]
//                       ,[notebook_item_descs]
//                       ,[company_cd])
//                 VALUES
//                       (@item_type_cd
//                       ,@item_class_cd
//                       ,@item_category_cd
//                       ,@item_cd
//                       ,@audit_user
//                       ,@audit_date
//                       ,@notebook_item_cd
//                       ,@notebook_item_descs
//                       ,@company_cd)";

//        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
//        SqlCommand cmd = new SqlCommand(sql, cn);

//        ItemDTO item = getItemFromNutratechDb(item_cd);
//        item.company_cd = company_cd;
//        item.notebook_item_cd = notebook_item_cd;
//        item.notebook_item_descs = notebook_item_descs;
//        cmd.Parameters.AddWithValue("item_type_cd", item.item_type_cd);
//        cmd.Parameters.AddWithValue("item_class_cd", item.item_class_cd);
//        cmd.Parameters.AddWithValue("item_category_cd", item.item_category_cd);
//        cmd.Parameters.AddWithValue("item_cd", item_cd);
//        cmd.Parameters.AddWithValue("audit_user", audit_user);
//        cmd.Parameters.AddWithValue("audit_date", audit_date);
//        cmd.Parameters.AddWithValue("notebook_item_cd", notebook_item_cd);
//        cmd.Parameters.AddWithValue("notebook_item_descs", notebook_item_descs);
//        cmd.Parameters.AddWithValue("company_cd", company_cd);

//        SqlDataReader dtr;
//        cn.Open();
//        dtr = cmd.ExecuteReader();

//        dtr.Close();
//        cn.Close();

//        return item;

//    }

    [WebMethod(EnableSession = true)]
    public ItemDTO getItemFromNutratechDb(String item_cd,String SelectedItemDescs, String item_type_cd,
                                          String item_class_cd, String item_category_cd)
    {
        List<ItemDTO> items = new List<ItemDTO>();

        System.Diagnostics.Debug.WriteLine("the item_cd : " + item_cd);
        System.Diagnostics.Debug.WriteLine("the item_type_cd : " + item_type_cd);
        System.Diagnostics.Debug.WriteLine("the item_class_cd : " + item_class_cd);
        System.Diagnostics.Debug.WriteLine("the item_category_cd : " + item_category_cd);
        String sql = @"SELECT [item_cd] ,[descs], [item_type_cd],[item_category_cd],[item_class_cd] 
                        FROM [Nutratech_DB].[dbo].[item]   
                        WHERE item_cd =@item_cd AND item_type_cd = @item_type_cd AND 
                              item_category_cd = @item_category_cd AND 
                              item_class_cd =@item_class_cd;";

        ItemDTO item = null;
        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, cn);

        cmd.Parameters.AddWithValue("item_cd", item_cd);
        cmd.Parameters.AddWithValue("item_type_cd", item_type_cd);
        cmd.Parameters.AddWithValue("item_category_cd", item_category_cd);
        cmd.Parameters.AddWithValue("item_class_cd", item_class_cd);


        SqlDataReader dtr;
        cn.Open();
        dtr = cmd.ExecuteReader();
        if (dtr.Read())
        {
            item = new ItemDTO
            {
                item_cd = dtr["item_cd"].ToString().Trim(),
                descs = dtr["descs"].ToString().Trim(),
                item_type_cd = dtr["item_type_cd"].ToString().Trim(),
                item_category_cd = dtr["item_category_cd"].ToString().Trim(),
                item_class_cd = dtr["item_class_cd"].ToString().Trim()
            };
        }
        
        //while (dtr.Read())
        //{
            
            
        //}
       
        cn.Close();
        cn.Dispose();
        JavaScriptSerializer ser = new JavaScriptSerializer();
        System.Diagnostics.Debug.WriteLine("the item is : " + ser.Serialize(item));
        return item ;
    }
    
//    [WebMethod(EnableSession=true)]
//    public void removeFromMatrixDb(String code, String description, String company)
//    {
//        String sql = @"DELETE FROM [dbo].[item_notebook_matrix]
//                       WHERE notebook_item_cd = @notebook_item_cd AND 
//                             notebook_item_descs = @notebook_item_descs AND 
//                             company_cd = @company_cd;";

//        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
//        SqlCommand cmd = new SqlCommand(sql, cn);

//        cmd.Parameters.AddWithValue("notebook_item_cd", code);
//        cmd.Parameters.AddWithValue("notebook_item_descs", description);
//        cmd.Parameters.AddWithValue("company_cd", company);

//        SqlDataReader dtr;
//        cn.Open();
//        dtr = cmd.ExecuteReader();

//        dtr.Close();
//        cn.Close();
//    }


    [WebMethod(EnableSession = true)]
    public String batchRemoveFromMatrixDb(List<ItemNBMatrixDTO> Items, String SelectedItemCode, String SelectedItemDescs,
                                          String SelectedItemType, String SelectedItemClass, String SelectedItemCategory)
    {
        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        
        cn.Open();
       
        foreach (ItemNBMatrixDTO item in Items)
        {
            String sql = @"DELETE FROM [Nutratech_DB].[dbo].[item_notebook_matrix]
                          WHERE item_type_cd=@item_type_cd AND item_class_cd=@item_class_cd AND 
			                    item_category_cd=@item_category_cd AND item_cd = @item_cd AND 
			                    notebook_item_cd=@notebook_item_cd;";
            
            SqlCommand cmd = new SqlCommand(sql, cn);
          
            if(item.code == null){
                cmd.Parameters.AddWithValue("notebook_item_cd", DBNull.Value);
                System.Diagnostics.Debug.WriteLine("item.code is null");
            }
            else
                cmd.Parameters.AddWithValue("notebook_item_cd", item.code);
            if (item.descs == null)
            {
                cmd.Parameters.AddWithValue("notebook_item_descs", DBNull.Value);
                System.Diagnostics.Debug.WriteLine("item.descs is null");
            }
            else
                cmd.Parameters.AddWithValue("notebook_item_descs", item.descs);
            if (item.company_cd == null)
            {
                cmd.Parameters.AddWithValue("company_cd", DBNull.Value);
                System.Diagnostics.Debug.WriteLine("item.company_cd is null");
            }
            else
                cmd.Parameters.AddWithValue("company_cd", item.company_cd);

                cmd.Parameters.AddWithValue("item_cd", SelectedItemCode);
                cmd.Parameters.AddWithValue("item_type_cd", SelectedItemType);
                cmd.Parameters.AddWithValue("item_class_cd", SelectedItemClass);
                cmd.Parameters.AddWithValue("item_category_cd", SelectedItemCategory);               
                                   
            //SqlDataReader dtr;
            //dtr = cmd.ExecuteReader();
            //dtr.Close();

            // Change the SQL Command and execute
            cmd.CommandText = sql;
            //cmd.ExecuteNonQuery();
            cmd.ExecuteNonQuery();
        }
       
        cn.Close();
        return SelectedItemCode;
        
    }

    [WebMethod(EnableSession = true)]
    public String batchInsertToMatrixDb(List<ItemDTO> Items, String SelectedItemCode, String SelectedItemDescs,
                                        String SelectedItemType, String SelectedItemClass, String SelectedItemCategory)
    {
        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        cn.Open();

        System.Diagnostics.Debug.WriteLine("the SelectedItemCode : " + SelectedItemCode);
        System.Diagnostics.Debug.WriteLine("the SelectedItemType : " + SelectedItemType);
        System.Diagnostics.Debug.WriteLine("the SelectedItemClass : " + SelectedItemClass);
        System.Diagnostics.Debug.WriteLine("the SelectedItemCategory : " + SelectedItemCategory);

        ItemDTO item = getItemFromNutratechDb(SelectedItemCode, SelectedItemDescs, SelectedItemType,
                                              SelectedItemClass, SelectedItemCategory);

        System.Diagnostics.Debug.WriteLine("the item.item_type_cd is : " + item.item_type_cd);
        foreach (ItemDTO itemDTO in Items)
        {

            String sql = @"INSERT INTO [Nutratech_DB].[dbo].[item_notebook_matrix]
                       ([item_type_cd]
                       ,[item_class_cd]
                       ,[item_category_cd]
                       ,[item_cd]
                       ,[audit_user]
                       ,[audit_date]
                       ,[notebook_item_cd]
                       ,[notebook_item_descs]
                       ,[company_cd])
                 VALUES
                       (@item_type_cd
                       ,@item_class_cd
                       ,@item_category_cd
                       ,@item_cd
                       ,@audit_user
                       ,@audit_date
                       ,@notebook_item_cd
                       ,@notebook_item_descs
                       ,@company_cd)";

            SqlCommand cmd = new SqlCommand(sql, cn);
             
            cmd.Parameters.AddWithValue("item_type_cd", item.item_type_cd);
            cmd.Parameters.AddWithValue("item_class_cd", item.item_class_cd);
            cmd.Parameters.AddWithValue("item_category_cd", item.item_category_cd);
            cmd.Parameters.AddWithValue("item_cd", itemDTO.item_cd);
            cmd.Parameters.AddWithValue("audit_user", itemDTO.audit_user);
            cmd.Parameters.AddWithValue("audit_date", itemDTO.audit_date);
            cmd.Parameters.AddWithValue("notebook_item_cd", itemDTO.notebook_item_cd);
            cmd.Parameters.AddWithValue("notebook_item_descs", itemDTO.notebook_item_descs);
            cmd.Parameters.AddWithValue("company_cd", itemDTO.company_cd);
                        
            cmd.ExecuteNonQuery();
        }
        cn.Close();
        return SelectedItemCode;
    }
    
    #endregion 


    #region Motor Pool
    
    [WebMethod(EnableSession = true)]
    public String insertVehicleLog(VehicleLogDTO vehicleLogDTO)
    {

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        cn.Open();

        var audit_date = DateTime.Now;
        double result = 0;
        if (vehicleLogDTO.full_tank == "True")
        {
            var current_gas_price = vehicleLogDTO.total_cost / vehicleLogDTO.total_gas;
            var current_total_gas_liter = vehicleLogDTO.total_cost / current_gas_price;

            result = vehicleLogDTO.odometer / current_total_gas_liter;

            vehicleLogDTO.result = result;
        }
        vehicleLogDTO.result = result;
        String sql = @"INSERT INTO [dbo].[vehicle_log]
                           ([vehicle_plate_no]
                           ,[odometer]
                           ,[last_fuel_price]
                           ,[total_gas]
                           ,[total_cost]
                           ,[date]
                           ,[full_tank]
                           ,[result]
                           ,[driver_username]
                           ,[audit_user]
                           ,[audit_date])
                     VALUES
                           (@vehicle_plate_no
                           ,@odometer
                           ,@last_fuel_price
                           ,@total_gas
                           ,@total_cost
                           ,@date
                           ,@full_tank
                           ,@result
                           ,@driver_username
                           ,@audit_user
                           ,@audit_date)";

        SqlCommand cmd = new SqlCommand(sql, cn);
        try
        {
            JavaScriptSerializer jscript = new JavaScriptSerializer();
            System.Diagnostics.Debug.WriteLine("vehicleLogDTO : " + jscript.Serialize(vehicleLogDTO));
            cmd.Parameters.AddWithValue("vehicle_plate_no", vehicleLogDTO.vehicle_plate_no);
            cmd.Parameters.AddWithValue("odometer", vehicleLogDTO.odometer);
            cmd.Parameters.AddWithValue("last_fuel_price", vehicleLogDTO.last_fuel_price);
            cmd.Parameters.AddWithValue("total_gas", vehicleLogDTO.total_gas);
            cmd.Parameters.AddWithValue("total_cost", vehicleLogDTO.total_cost);
            cmd.Parameters.AddWithValue("date", vehicleLogDTO.date);
            cmd.Parameters.AddWithValue("full_tank", vehicleLogDTO.full_tank);
            cmd.Parameters.AddWithValue("result", vehicleLogDTO.result);
            cmd.Parameters.AddWithValue("driver_username", vehicleLogDTO.driver_username);
            cmd.Parameters.AddWithValue("audit_user", vehicleLogDTO.audit_user);
            cmd.Parameters.AddWithValue("audit_date", vehicleLogDTO.audit_date);

            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("the exception is : "  + ex.Message);
            return "<span style='color:red'>Please input the required values...</span>";
        }
       
        //var full = (vehicleLogDTO.full_tank == 1);
        //return new VehicleLogDTO
        //{
        //    vehicle_plate_no = vehicle_plate_no,
        //    odometer = odometer,
        //    last_fuel_price = last_fuel_price,
        //    total_gas = total_gas,
        //    total_cost = total_cost,
        //    date = date.ToShortDateString(),
        //    full_tank = full.ToString(),
        //    result = result,
        //    driver_username = driver_username,
        //    audit_user = audit_user,
        //    audit_date = audit_date.ToShortDateString()
        //};
        return "";
    }
       
    public string DataTableToJSONWithJSONNet(DataTable table)
    {
        string JsonString = string.Empty;
        JsonString = JsonConvert.SerializeObject(table, Formatting.None, new IsoDateTimeConverter() { DateTimeFormat = "MM/dd/yyyy" });
        return JsonString;
    }

    [WebMethod(EnableSession = true)]
    public List<VehicleLogDTO> getVehicleLogs(String vehicle_plate_no)
    {
        List<VehicleLogDTO> vehicleLogs = new List<VehicleLogDTO>();

        String sql = @"SELECT [vehicle_plate_no]
                              ,[odometer]
                              ,[last_fuel_price]
                              ,[total_gas]
                              ,[total_cost]
                              ,[date]
                              ,[full_tank]
                              ,[result]
                              ,[driver_username]
                              ,[audit_user]
                              ,[audit_date]
                      FROM [Nutratech_DB].[dbo].[vehicle_log]
                      WHERE vehicle_plate_no =@vehicle_plate_no order by 'date' desc;";

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, cn);

        cmd.Parameters.AddWithValue("vehicle_plate_no", vehicle_plate_no);
       
        SqlDataReader dtr;
        cn.Open();
        dtr = cmd.ExecuteReader();

        while (dtr.Read())
        {
            System.Diagnostics.Debug.WriteLine("and full_tank gkan sa db kai : " + dtr["full_tank"].ToString().Trim());
            vehicleLogs.Add(new VehicleLogDTO
            {
                vehicle_plate_no = dtr["vehicle_plate_no"].ToString().Trim(),
                odometer =  Convert.ToDouble(dtr["odometer"].ToString().Trim()),
                last_fuel_price = Convert.ToDouble(dtr["last_fuel_price"].ToString().Trim()),
                total_gas = Convert.ToDouble(dtr["total_gas"].ToString().Trim()),
                total_cost = Convert.ToDouble(dtr["total_cost"].ToString().Trim()),
                date = dtr["date"].ToString().Trim(),
                full_tank = dtr["full_tank"].ToString().Trim(),
                result = Convert.ToDouble(dtr["result"].ToString().Trim()),
                driver_username = dtr["driver_username"].ToString().Trim(),
                audit_user = dtr["audit_user"].ToString().Trim(),
                audit_date = dtr["audit_date"].ToString().Trim()
                
            });
        }
        dtr.Close();
        cn.Close();        
        return vehicleLogs;
    }
    
    [WebMethod(EnableSession = true)]
    public double getLastFuelPrice(String vehicle_plate_no)
    {
        double last_fuel_price = 0;

        String sql = @"SELECT TOP 1 [vehicle_plate_no]
                              ,[odometer]
                              ,[last_fuel_price]
                              ,[total_gas]
                              ,[total_cost]
                              ,[date]
                              ,[full_tank]
                              ,[result]
                              ,[driver_username]
                              ,[audit_user]
                              ,[audit_date]
                      FROM [Nutratech_DB].[dbo].[vehicle_log]
                      WHERE vehicle_plate_no =@vehicle_plate_no order by 'date' desc;";

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, cn);

        cmd.Parameters.AddWithValue("vehicle_plate_no", vehicle_plate_no);

        SqlDataReader dtr;
        cn.Open();
        dtr = cmd.ExecuteReader();

        while (dtr.Read())
        {
            var total_cost = Convert.ToDouble(dtr["total_cost"].ToString().Trim());
            var total_gas = Convert.ToDouble(dtr["total_gas"].ToString().Trim());

            last_fuel_price = total_cost / total_gas;
        }
        dtr.Close();
        cn.Close();
        return last_fuel_price;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public string GetVehicleImage(string plate_no)
    {
        string sql = "";
        String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(connectionString))
        {

            sql = @"SELECT vehicle.image,
                           vehicle_user.assigned_to,
                           vehicle.plate_no,
                           vehicle_user.date_assigned 
                           FROM vehicle LEFT JOIN vehicle_user on vehicle.plate_no = vehicle_user.vehicle_plate_no 
                           where vehicle.plate_no = @plate_no;";

            DataTable dataTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(sql, con);
            adapter.SelectCommand.Parameters.AddWithValue("@plate_no", plate_no);
            adapter.Fill(dataTable);
            foreach (DataRow dr in dataTable.Rows)
            {
                dr["image"] = dr["image"].ToString().Trim();
                dr["assigned_to"] = dr["assigned_to"].ToString().Trim();
                dr["plate_no"] = dr["plate_no"].ToString().Trim();
                // dr["date_assigned"] = dr["date_assigned"] == (object)DBNull.Value ? "" : Convert.ToDateTime(dr["date_assigned"]).ToString("MM/dd/yyyy");
            }
            return DataTableToJSONWithJSONNet(dataTable);
        }


    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public string GetVehicleList()
    {
        string sql = "";
        string selected_attr = "";
        String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            selected_attr += "name, descs, model_year, color, plate_no, insurance_policy_no, seater_capacity, ";
            selected_attr += "fuel_type, fuel_unit, distance_unit, location";

            sql += "SELECT " + selected_attr + " FROM vehicle";

            DataTable dataTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(sql, con);

            adapter.Fill(dataTable);

            foreach (DataRow dr in dataTable.Rows)
            {
                dr["name"] = dr["name"].ToString().Trim();
                dr["descs"] = dr["descs"].ToString().Trim();
                dr["model_year"] = dr["model_year"].ToString().Trim();
                dr["color"] = dr["color"].ToString().Trim();
                dr["plate_no"] = dr["plate_no"].ToString().Trim();
                dr["insurance_policy_no"] = dr["insurance_policy_no"].ToString().Trim();
                dr["seater_capacity"] = Convert.ToInt32(dr["seater_capacity"]);
                dr["fuel_type"] = dr["fuel_type"].ToString().Trim();
                dr["fuel_unit"] = dr["fuel_unit"].ToString().Trim();
                dr["distance_unit"] = dr["distance_unit"].ToString().Trim();
                dr["location"] = dr["location"].ToString().Trim();

            }
            String jsonData = DataTableToJSONWithJSONNet(dataTable);
            return "{\"aaData\":" + jsonData + "}";
            // return jsonData;
        }

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public void InsertVehicleUser(string plate_no, string assigned_to, string date_assigned)
    {
        string sql = "";

        String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {

            sql += "INSERT INTO vehicle_user (vehicle_plate_no, assigned_to, date_assigned, audit_user, audit_date) ";
            sql += "VALUES (@plate_no, @assigned_to, @date_assigned, @audit_user, @audit_date);";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@plate_no", plate_no);
            command.Parameters.AddWithValue("@assigned_to", assigned_to);
            command.Parameters.AddWithValue("@date_assigned", date_assigned == "" ? (object)DBNull.Value : System.DateTime.Parse(date_assigned));
            command.Parameters.AddWithValue("@audit_user", HttpContext.Current.Session["username"].ToString());
            command.Parameters.AddWithValue("@audit_date", Convert.ToDateTime(DateTime.Now));

            try
            {
                connection.Open();
                Int32 rowsAffected = command.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine("RowsAffected: {0}", rowsAffected);
                connection.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("error: " + ex.Message);
            }
        }



    }
   
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
    public string InsertVehicle(string name, string descs, string model_year, string color, string plate_no,
        string insurance_policy_no, int seater_capacity, string fuel_type, string location, string distance_unit,
        string fuel_unit, string image)
    {


        string sql = "";
        String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            sql += "INSERT INTO VEHICLE ";
            sql += "(name, descs, model_year, color, plate_no, insurance_policy_no, seater_capacity, ";
            sql += "fuel_type, location, audit_user, audit_date, distance_unit, fuel_unit, image) ";
            sql += "VALUES ";
            sql += "(@name, @descs, @modelYear, @color, @plateNo, @insurancePoNo, @seaterCap, ";
            sql += "@fuelType, @location, @auditUser, @auditDate, @distanceUnit, @fuelUnit, @image);";
            SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@descs", descs == "" ? (object)DBNull.Value : descs);
            command.Parameters.AddWithValue("@modelYear", model_year == "" ? (object)DBNull.Value : model_year);
            command.Parameters.AddWithValue("@color", color == "" ? (object)DBNull.Value : color);
            command.Parameters.AddWithValue("@plateNo", plate_no == "" ? (object)DBNull.Value : plate_no);
            command.Parameters.AddWithValue("@insurancePoNo", insurance_policy_no == "" ? (object)DBNull.Value : insurance_policy_no);
            command.Parameters.AddWithValue("@seaterCap", seater_capacity);
            command.Parameters.AddWithValue("@fuelType", fuel_type == "" ? (object)DBNull.Value : fuel_type);
            command.Parameters.AddWithValue("@location", location == "" ? (object)DBNull.Value : location);
            command.Parameters.AddWithValue("@distanceUnit", distance_unit == "" ? (object)DBNull.Value : distance_unit);
            command.Parameters.AddWithValue("@fuelUnit", fuel_unit == "" ? (object)DBNull.Value : fuel_unit);
            command.Parameters.AddWithValue("@auditUser", HttpContext.Current.Session["username"].ToString());
            command.Parameters.AddWithValue("@auditDate", Convert.ToDateTime(DateTime.Now));
            command.Parameters.AddWithValue("@image", image == "" ? (object)DBNull.Value : image);


            try
            {
                connection.Open();
                Int32 rowsAffected = command.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine("RowsAffected: {0}", rowsAffected);
                connection.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("error: " + ex.Message);
            }
            return "";
        }

    }

    [WebMethod(EnableSession = true)]
    public String saveAccessory(String code, String descs, String group_cd, String audit_user, bool is_new_entry)
    {
        if (code.Replace("undefined", string.Empty) == string.Empty)
        {
            return "<span style='color:red'>Please input the required values...</span>";

        }
        if (descs.Replace("undefined", string.Empty) == string.Empty)
        {
            return "<span style='color:red'>Please input the required values...</span>";

        }

        var audit_date = DateTime.Now;

        if (is_new_entry)
        {
            string checkSql = "SELECT [code],[descs],[group_cd],[audit_user],[audit_date]FROM [Nutratech_DB].[dbo].[accessory]" + " WHERE (code = '" + code + "') ";
            //& " WHERE (descs = '" & xdescs.Replace("'", "''").Trim & "')"
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SqlCommand command = new SqlCommand(checkSql, cn);
            SqlDataReader dtr = default(SqlDataReader);

            cn.Open();
            dtr = command.ExecuteReader();
            bool sfind = false;
            if (dtr.Read())
            {
                sfind = true;
            }
            cn.Close();


            if (sfind == false)
            {
                cn.Open();
                
                String sql = @"INSERT INTO [dbo].[accessory]
                           ([code]
                           ,[descs]
                           ,[group_cd]
                           ,[audit_user]
                           ,[audit_date])
                     VALUES
                           (@code
                           ,@descs
                           ,@group_cd                          
                           ,@audit_user
                           ,@audit_date)";

                SqlCommand cmd = new SqlCommand(sql, cn);
                try
                {
                    JavaScriptSerializer jscript = new JavaScriptSerializer();

                    cmd.Parameters.AddWithValue("code", code);
                    cmd.Parameters.AddWithValue("descs", descs);
                    cmd.Parameters.AddWithValue("group_cd", group_cd);
                    cmd.Parameters.AddWithValue("audit_user", audit_user);
                    cmd.Parameters.AddWithValue("audit_date", audit_date);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("the exception is : " + ex.Message);
                    return "<span style='color:red'>Please input the required values...</span>";
                }
                cn.Close();
                cn.Dispose();
            }
            else
            {
                return "<span style='color:red'>Duplicated Record found.... <br>Code : " + code + "</span>";
            }
        }
        else
        {

            string sql = @"UPDATE [dbo].[accessory]
                           SET [descs] = @descs
                              ,[group_cd] = @group_cd
                              ,[audit_user] = @audit_user
                              ,[audit_date] = @audit_date
                         WHERE code=@code;";

            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("descs", descs);
            cmd.Parameters.AddWithValue("group_cd", group_cd);
            cmd.Parameters.AddWithValue("audit_user", audit_user);
            cmd.Parameters.AddWithValue("audit_date", audit_date);
            cmd.Parameters.AddWithValue("code", code);
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
            cn.Dispose();
            return "<span style='color:green'>Accessory: "+code+" Successfully Updated...</span>";

        }
        return "<span style='color:green'>Accessory Successfully Saved...</span>";
    }

    [WebMethod(EnableSession = true)]
    public String saveAccessoryGroup(String code, String descs, String audit_user, bool is_new_entry)
    {
        if (code.Replace("undefined", string.Empty) == string.Empty)
        {
            return "<span style='color:red'>Please input the required values...</span>";

        }
        if (descs.Replace("undefined", string.Empty) == string.Empty)
        {
            return "<span style='color:red'>Please input the required values...</span>";

        }

        var audit_date = DateTime.Now;

        if (is_new_entry)
        {
            string checkSql = "SELECT [code],[descs],[audit_user],[audit_date]FROM [Nutratech_DB].[dbo].[accessory_group]" + " WHERE (code = '" + code + "') ";
            //& " WHERE (descs = '" & xdescs.Replace("'", "''").Trim & "')"
            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SqlCommand command = new SqlCommand(checkSql, cn);
            SqlDataReader dtr = default(SqlDataReader);

            cn.Open();
            dtr = command.ExecuteReader();
            bool sfind = false;
            if (dtr.Read())
            {
                sfind = true;
            }
            cn.Close();


            if (sfind == false)
            {
                cn.Open();

                String sql = @"INSERT INTO [dbo].[accessory_group]
                           ([code]
                           ,[descs]
                           ,[audit_user]
                           ,[audit_date])
                     VALUES
                           (@code
                           ,@descs                          
                           ,@audit_user
                           ,@audit_date)";

                SqlCommand cmd = new SqlCommand(sql, cn);
                try
                {
                    JavaScriptSerializer jscript = new JavaScriptSerializer();

                    cmd.Parameters.AddWithValue("code", code);
                    cmd.Parameters.AddWithValue("descs", descs);
                    cmd.Parameters.AddWithValue("audit_user", audit_user);
                    cmd.Parameters.AddWithValue("audit_date", audit_date);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("the exception is : " + ex.Message);
                    return "<span style='color:red'>Please input the required values...</span>";
                }
                cn.Close();
                cn.Dispose();
            }
            else
            {
                return "<span style='color:red'>Duplicated Record found.... <br>Code : " + code + "</span>";
            }
        }
        else
        {

            string sql = @"UPDATE [dbo].[accessory_group]
                           SET [descs] = @descs
                              ,[audit_user] = @audit_user
                              ,[audit_date] = @audit_date
                         WHERE code=@code;";

            SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("descs", descs);
            cmd.Parameters.AddWithValue("audit_user", audit_user);
            cmd.Parameters.AddWithValue("audit_date", audit_date);
            cmd.Parameters.AddWithValue("code", code);
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
            cn.Dispose();
            return "<span style='color:green'>false ang is new entry...</span>";

        }
        return "<span style='color:green'>Accessory Group Successfully Saved...</span>";
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public String getAccessoriesDT()
    {
        string sql = "";
        string selected_attr = "";

        List<AccessoryDTO> accessories = new List<AccessoryDTO>();

        var echo = int.Parse(HttpContext.Current.Request.Params["sEcho"]);
        var displayLength = int.Parse(HttpContext.Current.Request.Params["iDisplayLength"]);
        var displayStart = int.Parse(HttpContext.Current.Request.Params["iDisplayStart"]);
        var sortOrder = HttpContext.Current.Request.Params["sSortDir_0"].ToString(CultureInfo.CurrentCulture);
        var sortCol = HttpContext.Current.Request.Params["iSortCol_0"].ToString(CultureInfo.CurrentCulture);

        string search = HttpContext.Current.Request.Params["searchValue"].ToString();
        var roleId = HttpContext.Current.Request.Params["roleId"].ToString(CultureInfo.CurrentCulture);
        System.Diagnostics.Debug.WriteLine("the value of search is : " + search);
        String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            selected_attr += "code, descs, group_cd ";


            sql += "SELECT " + selected_attr + " FROM [Nutratech_DB].[dbo].[accessory]";

            if (!string.IsNullOrEmpty(search))
            {
                sql += "WHERE code like '%" + search + "%' OR descs like '%" + search + "%' OR group_cd like '%" + search + "%' ";
            }
            DataTable dataTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(sql, con);

            adapter.Fill(dataTable);

            foreach (DataRow dr in dataTable.Rows)
            {
                accessories.Add(new AccessoryDTO { 
                    code = dr["code"].ToString().Trim(),
                    descs = dr["descs"].ToString().Trim(),
                    group_cd = dr["group_cd"].ToString().Trim()                
                });
                //dr["code"] = dr["code"].ToString().Trim();
                //dr["descs"] = dr["descs"].ToString().Trim();
                //dr["group_cd"] = dr["group_cd"].ToString().Trim();
            }


            var orderedResults = sortOrder == "asc"
                              ? accessories.OrderBy(o => o.code)
                              : accessories.OrderByDescending(o => o.code);

            switch (sortCol)
            {
                case "0":
                    orderedResults = sortOrder == "asc" ? accessories.OrderBy(o => o.code) : accessories.OrderByDescending(o => o.code);
                    break;
                case "1":
                    orderedResults = sortOrder == "asc" ? accessories.OrderBy(o => o.descs) : accessories.OrderByDescending(o => o.descs);
                    break;
                case "2":
                    orderedResults = sortOrder == "asc" ? accessories.OrderBy(o => o.group_cd) : accessories.OrderByDescending(o => o.group_cd);
                    break;             
            }
            var itemsToSkip = displayStart == 0
                              ? 0
                              : displayStart + 1;
            var pagedResults = orderedResults.Skip(itemsToSkip).Take(displayLength).ToList();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            var sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"sEcho\": " + echo + ",");
            sb.Append("\"recordsTotal\": " + accessories.Count() + ",");
            sb.Append("\"recordsFiltered\": " + accessories.Count() + ",");
            sb.Append("\"iTotalRecords\": " + accessories.Count() + ",");
            sb.Append("\"iTotalDisplayRecords\": " + accessories.Count() + ",");
            sb.Append("\"aaData\": " + ser.Serialize(pagedResults));

            sb.Append("}");
            //String jsonData = DataTableToJSONWithJSONNet(dataTable);
            //return "{\"aaData\":" + jsonData + "}";

            return sb.ToString();
        }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public String getAccessoryGroups()
    {
        string sql = "";
        string selected_attr = "";
        String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            selected_attr += "code, descs ";


            sql += "SELECT " + selected_attr + " FROM [Nutratech_DB].[dbo].[accessory_group]";

            DataTable dataTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(sql, con);

            adapter.Fill(dataTable);

            foreach (DataRow dr in dataTable.Rows)
            {
                dr["code"] = dr["code"].ToString().Trim();
                dr["descs"] = dr["descs"].ToString().Trim();         
            }
            String jsonData = DataTableToJSONWithJSONNet(dataTable);
            return "{\"aaData\":" + jsonData + "}";
        }
    }

    [WebMethod(EnableSession = true)]
    public List<VehicleAccessoryDTO> getVehicleAccessories(String vehicle_plate_no)
    {
        List<VehicleAccessoryDTO> items = new List<VehicleAccessoryDTO>();

        String sql = @"SELECT  [vehicle_id]
                              ,[accessory_group]
                              ,[accessory]
                              ,[audit_user]
                              ,[audit_date]
                          FROM [Nutratech_DB].[dbo].[vehicle_accessory]
                          WHERE vehicle_id=@vehicle_id;";
        
        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, cn);

        cmd.Parameters.AddWithValue("vehicle_id", vehicle_plate_no);

        SqlDataReader dtr;
        cn.Open();
        dtr = cmd.ExecuteReader();

        while (dtr.Read())
        {            
            items.Add(new VehicleAccessoryDTO
            {
                vehicle_id = dtr["vehicle_id"].ToString().Trim(),
                accessory_group = dtr["accessory_group"].ToString().Trim(),
                accessory = dtr["accessory"].ToString().Trim()                
            });
        }
        dtr.Close();
        cn.Close();        

        return items;
    }

    [WebMethod(EnableSession = true)]
    public List<ListItemDTO> getAccessories(String group_cd, String vehicle_plate_no, String searchString) {
        
        List<ListItemDTO> items = new List<ListItemDTO>();
        List<VehicleAccessoryDTO> vehicleAccessories = getVehicleAccessories(vehicle_plate_no);

        String sql = @"SELECT [code],[descs] 
                            FROM [Nutratech_DB].[dbo].[accessory]
                            WHERE group_cd=@group_cd AND (code like '%" + searchString +"%' OR descs like '%"+searchString+"%')";
        foreach (VehicleAccessoryDTO vehicleAccessory in vehicleAccessories)
        {
            sql = sql + "AND descs != '" + vehicleAccessory.accessory + "' ";
        }

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, cn);

        cmd.Parameters.AddWithValue("group_cd", group_cd);
       // cmd.Parameters.AddWithValue("searchString", searchString);

        SqlDataReader dtr;
        cn.Open();
        dtr = cmd.ExecuteReader();

        while (dtr.Read())
        {
            items.Add(new ListItemDTO
            {
                code = dtr["code"].ToString().Trim(),
                descs = dtr["descs"].ToString().Trim()
            });
        }
        dtr.Close();
        cn.Close();

        return items;
    }

    [WebMethod(EnableSession = true)]
    public String batchInsertToVehicleAccessory(List<ListItemDTO> items, String vehicle_plate_no , String group_cd, String audit_user) {

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        cn.Open();

        var audit_date = DateTime.Now;        

        String sql = @"INSERT INTO [dbo].[vehicle_accessory]
                               ([vehicle_id]
                               ,[accessory_group]
                               ,[accessory]
                               ,[audit_user]
                               ,[audit_date])
                         VALUES ";

        string code = items[items.Count - 1].code;
        foreach (ListItemDTO itemDTO in items)
        {
            if (code == itemDTO.code)
                sql += "('" + vehicle_plate_no +
                       "','" + group_cd +
                       "','" + itemDTO.descs +
                       "','" + audit_user +
                       "','" + audit_date + "')";
            else
            sql += "('" + vehicle_plate_no +
                   "','" + group_cd +
                   "','" + itemDTO.descs + 
                   "','" + audit_user +
                   "','" + audit_date + "'),";          
        }
        System.Diagnostics.Debug.WriteLine("the slq is : " + sql );       
        SqlCommand cmd = new SqlCommand(sql, cn);
        cmd.ExecuteNonQuery();
        cn.Close();
        return "";
    }

    [WebMethod(EnableSession = true)]
    public String batchRemoveFromVehicleAccessory(List<ListItemDTO> items, String vehicle_plate_no) {

        String sql = "DELETE FROM [dbo].[vehicle_accessory] WHERE vehicle_id='"+vehicle_plate_no+"' ";

        SqlConnection cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        cn.Open();

        string accessoryFirst = items[0].descs;
        string accessoryLast = items[items.Count - 1].descs;
        int count = items.Count;
        foreach (ListItemDTO item in items)
        {
            if(count ==1)
                sql += " AND (accessory='" + item.descs + "') "; 
            else if(accessoryFirst == item.descs)
                sql += " AND (accessory='" + item.descs + "' OR "; 
            else if (accessoryLast == item.descs)
                sql += " accessory='"+item.descs+"')";
            else
                sql += " accessory='" + item.descs + "' OR";           
        }

        System.Diagnostics.Debug.WriteLine("the slq from batchRemoveFromVehicleAccessory is : " + sql);
        SqlCommand cmd = new SqlCommand(sql, cn);
        cmd.CommandText = sql;

        cmd.ExecuteNonQuery();
        cn.Close();
        return "";
    }
    #endregion
}
