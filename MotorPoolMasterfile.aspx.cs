using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MotorPoolMasterfile : System.Web.UI.Page
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

        if (string.IsNullOrEmpty(Request.QueryString["ID"]) == false)
        {

            if (Request.QueryString["ID"].ToString() == "ViewAccessory")
            {
                MultiView1.SetActiveView(ViewButtons);
                MultiView2.SetActiveView(ViewAccessory);
                lblMenu.Text = "/ <i class='fa fa-steam fa-fw'></i>Accessory : ";
            }
            else if (Request.QueryString["ID"].ToString() == "ViewAccessoryGroup")
            {
                MultiView1.SetActiveView(ViewButtons);
                MultiView2.SetActiveView(ViewAccessoryGroup);
                lblMenu.Text = "/ <i class='fa fa-cubes fa-fw'></i>Accessory Group : ";
            }
            else if (Request.QueryString["ID"].ToString() == "ViewVehicleAccessory")
            {
                MultiView1.SetActiveView(ViewNothing);
                MultiView2.SetActiveView(ViewVehicleAccessory);
                lblMenu.Text = "/ <i class='fa fa-car fa-fw'></i>Vehicle Accessory : "; 
            }
        }
        //MultiView1.SetActiveView(ViewButtons);
        //MultiView2.SetActiveView(ViewAccessory);

        hidden_username.Value = HttpContext.Current.Session["username"].ToString();

    }
}