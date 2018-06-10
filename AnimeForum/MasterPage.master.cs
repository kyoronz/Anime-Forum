using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uType"] == null)
        {            
            Button4.Visible = false;
            file.Visible = false;
            //file.style.add("display", "none")
        }
        else if (Session["uType"].ToString().Equals("User"))
        {         
            Button4.Visible = false;
        }
        else if (Session["uType"].ToString().Equals("Moderator"))
        {         
            Button4.Visible = false;
        }
        else if (Session["uType"].ToString().Equals("Admin"))
        {         
            Button4.Visible = true;
        }
        


    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Response.Redirect("Login2.aspx");
    }
    protected void btnReg_Click(object sender, EventArgs e)
    {
        Response.Redirect("Register2.aspx");
    }
    protected void btnLog_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("Login2.aspx");
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        Response.Redirect("BoardView.aspx?category=FAQ");
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageForumStructure.aspx");
    }
}
