using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

/// <summary>
/// Summary description for MotorPoolWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class MotorPoolWebService : System.Web.Services.WebService
{

    public MotorPoolWebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    #region "Motor Pool"

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public string GetVehicleImage(string plate_no)
    {
        string sql = "";
        String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(connectionString))
        {

            sql += "SELECT image FROM vehicle where plate_no = @plate_no";

            DataTable dataTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(sql, con);
            adapter.SelectCommand.Parameters.AddWithValue("@plate_no", plate_no);
            adapter.Fill(dataTable);
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



    public string DataTableToJSONWithJSONNet(DataTable table)
    {
        string JsonString = string.Empty;
        JsonString = JsonConvert.SerializeObject(table, Formatting.None, new IsoDateTimeConverter() { DateTimeFormat = "MM/dd/yyyy" });
        return JsonString;
    }

    #endregion

}
