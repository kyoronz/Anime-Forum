<%@ WebHandler Language="C#" Class="ProfileImageHandler" %>

using System;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
public class ProfileImageHandler : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        string constr = ConfigurationManager.ConnectionStrings["ACGDatabaseConnectionString"].ConnectionString;
        string imgID = context.Request.QueryString["ID"].ToString();
        //string imgID = HttpContext.Current.Request.QueryString("profID");
        //int imgID = HttpContext.Current.Request.QueryString["pID"].ToString();
        SqlConnection conn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("Select ProfileImage from ImageUpload where ImageID='" +imgID+"'" , conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            context.Response.BinaryWrite((byte[])dr["ProfileImage"]);
            dr.Close();
        }
        
        conn.Close();
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}