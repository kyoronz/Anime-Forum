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
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ACGDatabaseConnectionString"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["category"] == null)
            {
                Response.Redirect("PageNotFound.aspx");
            }
            Session["addcategory"] = Request.QueryString["category"].ToString();
        }        
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Session["Username"] = "ChaiYiFan";
        if (Session["Username"] == null)
        {
            //redirect to logged out notification page
            Response.Redirect("Disconnected.aspx");
            return;
        }
        string topicsql = "INSERT INTO Topic (TopicDate, TopicTitle, TopicCreator, TopicStatus, LastReply, BoardName) OUTPUT INSERTED.TopicID VALUES (@tdate, @title, @creator, @status, @lastReply,@bname)";
        string replysql = "INSERT INTO Reply (TopicID, ReplyContent, ReplyDate, ReplyCreator, ReplyStatus) VALUES (@tid,@content,@rdate,@creator,@status)";
        string title = txtTitle.Text;
        string content = txtContent.Text;
        if (title.Equals(String.Empty) || content.Equals(String.Empty))
        {
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
            query["category"] = Session["addcategory"].ToString();
            Response.Redirect("TopicAdd.aspx?");
        }
        string creator = Session["Username"].ToString();
        string status = "Active";
        string lastReply = creator;
        DateTime date = DateTime.Now;
        string replystatus = "Active";
        string boardName = Session["addcategory"].ToString();

        SqlCommand cmd = new SqlCommand(topicsql, con);
        cmd.Parameters.AddWithValue("@tdate", date);
        cmd.Parameters.AddWithValue("@title", title);
        cmd.Parameters.AddWithValue("@creator", creator);
        cmd.Parameters.AddWithValue("@status", status);
        cmd.Parameters.AddWithValue("@lastReply", lastReply);
        cmd.Parameters.AddWithValue("@bname", boardName);

        con.Open();
        int topicID = 0;
        try
        {
            topicID = (Int32)cmd.ExecuteScalar();
        }
        catch (Exception ex)
        {
            con.Close();
            Response.Redirect("Error.aspx");
        }

        con.Close();

        if (topicID == 0)
        {
            Response.Redirect("Error.aspx");
        }

        SqlCommand cmd2 = new SqlCommand(replysql, con);
        cmd2.Parameters.AddWithValue("@tid", topicID.ToString());
        cmd2.Parameters.AddWithValue("@content", content);
        cmd2.Parameters.AddWithValue("@rdate", date);
        cmd2.Parameters.AddWithValue("@creator", creator);
        cmd2.Parameters.AddWithValue("@status", replystatus);
        con.Open();
        int j = cmd2.ExecuteNonQuery();

        con.Close();
        NameValueCollection redirectquery = HttpUtility.ParseQueryString(string.Empty);
        redirectquery["category"] = Session["addcategory"].ToString();
        Response.Redirect("BoardView.aspx?"+redirectquery);
    }
}