using System;
using System.Collections.Generic;
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
    //    removeDisabled(View_Entry);
    }

    /*
    private void removeDisabled(Control parent) { 
     foreach(Control c in parent.Controls){
         if (c is TextBox){
                    ((TextBox)c).Enabled = false;}
     
     }
    }*/
}