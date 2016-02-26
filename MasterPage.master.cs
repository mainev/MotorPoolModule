using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
 

partial class MasterPage : System.Web.UI.MasterPage
{

    protected void  Page_Load(object sender, EventArgs e)
    {
        //if (Page.IsPostBack)
        //{ //do something 
        //     Page_Load(sender, e);
        //}
        //lblVersion.Text = AppSettings("version").ToString().Trim()
        lblUser.Text = HttpContext.Current.Session["username"].ToString().Trim();
        if (HttpContext.Current.Session["password"] != "change")
        {
            if (Session.IsNewSession==true | Page.User.Identity.Name == "")
            {
                Response.Redirect("logout.aspx");
            }
        }        
    }
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    //lblVersion.Text = AppSettings("version").ToString.Trim
    //    lblUser.Text = HttpContext.Current.Session("username");
    //    if (HttpContext.Current.Session("password") != "change")
    //    {
    //        if (Session.IsNewSession | string.IsNullOrEmpty(Page.User.Identity.Name))
    //        {
    //            Response.Redirect("logout.aspx");
    //        }
    //    }
    //}



    protected void DD_UserCompany_Init(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["username"] != null)
        {
            if (HttpContext.Current.Session["password"] == "change")
                return;
            Masterpage_Option oManager = new Masterpage_Option();
            oManager.Open();

            try
            {
                DropDownList ddl =  sender as DropDownList;
                
                ddl.DataSource = oManager.UserAssignCompany(HttpContext.Current.Session["username"].ToString().Trim());
                ddl.DataTextField = "descs";
                ddl.DataValueField = "code";

                ddl.DataBind();

                if (HttpContext.Current.Session["company_code"] != null)
                {
                    ddl.SelectedValue = HttpContext.Current.Session["company_code"].ToString().Trim();
                }
                else
                {
                    HttpContext.Current.Session["company_code"] = ddl.SelectedValue;
                }


            }
            catch (Exception ex)
            {
            }

            oManager.Close();
        }

    }


    protected void DD_UserCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        HttpContext.Current.Session["company_code"] = DD_UserCompany.SelectedValue;
        Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
        //If HttpContext.Current.Session["url") Is Nothing Then
        //    Response.Redirect("home.aspx")
        //Else
        //    Response.Redirect(HttpContext.Current.Session["url"))
        //End If        
    }

    protected void Repeater_1_Init(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["group_access"] != null)
        {
            if (HttpContext.Current.Session["password"] == "change")
                return;
            Repeater rp1 =  sender as Repeater;
            
            Masterpage_Option oManager = new Masterpage_Option();
            oManager.Open();

            rp1.DataSource = oManager.UserProgramMenu(HttpContext.Current.Session["group_access"].ToString().Trim());
            rp1.DataBind();

            oManager.Close();
        }
    }


    protected void Repeater_1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        if (e.Item.ItemType == ListItemType.Item | e.Item.ItemType == ListItemType.AlternatingItem)
        {

            Repeater rp2 = (Repeater)e.Item.FindControl("Repeater_2");
            int index = int.Parse(DataBinder.Eval(e.Item.DataItem, "menu_index").ToString());
            String caret = (String)(DataBinder.Eval(e.Item.DataItem, "glyph_icon_caret").ToString());
            //int index = (int)e.Item.DataItem("menu_index");
            //String caret = (String)e.Item.DataItem("glyph_icon_caret").ToString().Trim();



            if (caret == "none")
            {
                rp2.DataSource = null;
                rp2.DataBind();


            }
            else
            {

                Masterpage_Option oManager = new Masterpage_Option();
                oManager.Open();

                rp2.ItemDataBound += this.Repeater_2_ItemDataBound;

                rp2.DataSource = oManager.UserProgramMenu_sub1(HttpContext.Current.Session["group_access"].ToString().Trim(), index);
                rp2.DataBind();

                oManager.Close();
            }
        }

    }

    protected void Repeater_2_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        if (e.Item.ItemType == ListItemType.Item | e.Item.ItemType == ListItemType.AlternatingItem)
        {

            Repeater rp3 = (Repeater)e.Item.FindControl("Repeater_3");
            int index = int.Parse(DataBinder.Eval(e.Item.DataItem, "menu_index").ToString());
            int index_1 = int.Parse(DataBinder.Eval(e.Item.DataItem, "menu_index_1").ToString());
            String caret = (String)(DataBinder.Eval(e.Item.DataItem, "glyph_icon_caret").ToString());

            if (caret == "none")
            {
                rp3.DataSource = null;
                rp3.DataBind();


            }
            else
            {
                Masterpage_Option oManager = new Masterpage_Option();
                oManager.Open();

                rp3.DataSource = oManager.UserProgramMenu_sub2(HttpContext.Current.Session["group_access"].ToString().Trim(), index, index_1);
                rp3.DataBind();

                oManager.Close();

            }
        }
    }
   
}
