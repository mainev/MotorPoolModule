using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class vehicle : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.IsNewSession == true | User.Identity.Name == "")
        {
            Response.Redirect("Logout.aspx");
        }
        if (HttpContext.Current.Session["username"] == null)
        {
            Response.Redirect("Logout.aspx");
        }

        MultiView1.SetActiveView(ViewButtons);
        MultiView2.SetActiveView(View_Entry);
        MultiView3.SetActiveView(View_Table);
        MultiView4.SetActiveView(View_Vehicle_User);
        BindEmployeeDropDownList();
   
    }

    private void BindEmployeeDropDownList() {

        ListItem listItem = new ListItem();
        listItem.Text = "---";
        listItem.Value = "";

        using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
        {

            String q = @"SELECT LastName + ', '+FirstName+' '+MiddleInitial+'.' AS Name,
                                EmployeeID, 
                                strEmployeeID FROM Employees order by LastName asc;";

            DataTable dtEmployees = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(q, con);
            adapter.Fill(dtEmployees);

            foreach (DataRow dr in dtEmployees.Rows)
            {
                dr["Name"] = dr["Name"].ToString().Trim();
                dr["EmployeeID"] = Convert.ToInt32(dr["EmployeeID"]);
                dr["strEmployeeID"] = dr["strEmployeeID"].ToString().Trim();
            }

            DD_Employee.DataSource = dtEmployees;
            DD_Employee.DataTextField = "Name";
            DD_Employee.DataValueField = "EmployeeID";
            DD_Employee.DataBind();
            DD_Employee.Items.Insert(0, listItem);
        }

    }

    /*
    private void removeDisabled(Control parent) { 
     foreach(Control c in parent.Controls){
         if (c is TextBox){
                    ((TextBox)c).Enabled = false;}
     
     }
    }*/
}