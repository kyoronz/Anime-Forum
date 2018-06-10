using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BoardAdd : System.Web.UI.Page
{
    //TopicEdit.aspx?id=1
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ACGDatabaseConnectionString"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["Username"] = "ChaiYiFan";
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["id"] == null)
            {
                Response.Redirect("PageNotFound.aspx");
            }
            if (Session["Username"] == null)
            {
                Response.Redirect("Disconnected.aspx");
            }
            Session["replyID"] = Request.QueryString["id"].ToString();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Reply WHERE ReplyID=@rid AND ReplyCreator=@creator AND NOT ReplyStatus='Deleted' ", con);
            cmd.Parameters.AddWithValue("@rid", Session["replyID"]);
            cmd.Parameters.AddWithValue("@creator", Session["Username"].ToString());
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            string content = "";
            if (!dr.HasRows)
            {
                dr.Read();
                NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
                query["title"] = dr["TopicID"].ToString();
                dr.Close();
                con.Close();
                Response.Redirect("Topic.aspx?" + query);
            }
            else
            {
                dr.Read();
                content = dr["ReplyContent"].ToString();
                dr.Close();
            }
            con.Close();
            txtContent.Text = content;
        }        
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (Session["Username"] == null)
        {
            //redirect to logged out notification page
            Response.Redirect("Disconnected.aspx");
            return;
        }
        SqlCommand cmd = new SqlCommand("UPDATE Reply SET ReplyContent=@content WHERE ReplyCreator=@creator AND ReplyID=@id",con);
        cmd.Parameters.AddWithValue("@content", txtContent.Text);
        cmd.Parameters.AddWithValue("@creator", Session["Username"].ToString());
        cmd.Parameters.AddWithValue("@id", Session["replyID"].ToString());
        con.Open();
        cmd.ExecuteNonQuery();
        cmd = new SqlCommand("SELECT TopicID FROM Reply WHERE ReplyID=@id",con);
        cmd.Parameters.AddWithValue("@id", Session["replyID"].ToString());        
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
            query["title"] = dr["TopicID"].ToString();
            con.Close();
            Response.Redirect("Topic.aspx?" + query);
        }
        else
        {
            //error
        }        
        con.Close();
        Response.Redirect("MainPage.aspx");
    }
}