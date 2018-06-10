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
            context.Response.Write(new JavaScriptSerializer().Serialize(
            new
            {
                isValid = false
            }));
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
        SqlCommand cmd = new SqlCommand("SELECT * FROM Category WHERE CategoryName=@cname",con);
        cmd.Parameters.AddWithValue("@cname", name);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        Boolean isRecordExist = false;
        if (dr.HasRows)
        {
            isRecordExist = false;
        }
        else
        {
            isRecordExist = true;
        }
        con.Close();
        context.Response.Write(new JavaScriptSerializer().Serialize(
        new
        {
            isValid = isRecordExist
        }));
    }

    private void generateBoardDetails(string name, HttpContext context)
    {
        SqlCommand cmd = new SqlCommand("SELECT * FROM Board WHERE BoardName=@bname",con);
        cmd.Parameters.AddWithValue("@bname", name);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        Boolean isRecordExist = false;
        if (dr.HasRows)
        {
            isRecordExist = false;
        }
        else
        {
            isRecordExist = true;
        }
        con.Close();
        context.Response.Write(new JavaScriptSerializer().Serialize(
        new
        {
            isValid = isRecordExist
        }));
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}