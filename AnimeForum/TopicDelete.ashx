<%@ WebHandler Language="C#" Class="TopicDelete" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Web.SessionState;

public class TopicDelete : IHttpHandler,IReadOnlySessionState {
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ACGDatabaseConnectionString"].ConnectionString);
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        if (context.Request.QueryString["id"] == null || context.Session["Username"] == null)
        {
            context.Response.Write(new JavaScriptSerializer().Serialize(
            new
            {
                isValid = false
            }));
            return;
        }
        string replyid = context.Request.QueryString["id"];
        SqlCommand cmd = new SqlCommand("SELECT * FROM Reply WHERE ReplyID=@rid AND ReplyCreator=@creator", con);
        cmd.Parameters.AddWithValue("@rid", context.Request.QueryString["id"].ToString());
        cmd.Parameters.AddWithValue("@creator", context.Session["Username"].ToString());
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (!dr.HasRows)
        {
            con.Close();
            context.Response.Write(new JavaScriptSerializer().Serialize(
            new
            {
                isValid = false
            }));
            return;
        }
        con.Close();
        cmd = new SqlCommand("UPDATE Reply SET ReplyStatus='Deleted' WHERE ReplyID=@rid", con);
        cmd.Parameters.AddWithValue("@rid", replyid);
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
        context.Response.Write(new JavaScriptSerializer().Serialize(
        new
        {
            isValid = true
        }));
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}