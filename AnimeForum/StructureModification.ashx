<%@ WebHandler Language="C#" Class="StructureModification" %>

using System;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

public class StructureModification : IHttpHandler {
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ACGDatabaseConnectionString"].ConnectionString);
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        if (context.Request.QueryString["type"] == null|| context.Request.QueryString["id"]==null)
        {
            return;
        }
        string type = context.Request.QueryString["type"].ToString();
        string name = context.Request.QueryString["id"].ToString();
        if (type.Equals("1"))
        {
            generateCategoryDetails(name,context);
        }
        else
        {
            generateBoardDetails(name,context);
        }
    }
    private void generateCategoryDetails(string name, HttpContext context)
    {
        SqlCommand cmd = new SqlCommand("SELECT * FROM Category WHERE categoryName=@cname",con);
        cmd.Parameters.AddWithValue("@cname", name);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            context.Response.Write(new JavaScriptSerializer().Serialize(
            new
            {
                categoryName = dr["CategoryName"].ToString(),
                categoryDesc = dr["Description"].ToString()                
            }));
        }
        con.Close();
    }

    private void generateBoardDetails(string name, HttpContext context)
    {
        SqlCommand cmd = new SqlCommand("SELECT * FROM Board WHERE BoardName=@bname",con);
        cmd.Parameters.AddWithValue("@bname", name);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            context.Response.Write(new JavaScriptSerializer().Serialize(
            new
            {
                boardName = dr["BoardName"].ToString(),
                boardDesc = dr["Description"].ToString(),
                categoryName = dr["CategoryName"].ToString()
            }));
        }
        con.Close();
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}