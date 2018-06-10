<%@ WebHandler Language="C#" Class="StructureDeletion" %>
using System.Configuration;
using System.Data.SqlClient;
using System;
using System.Web;

public class StructureDeletion : IHttpHandler {
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ACGDatabaseConnectionString"].ConnectionString);
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
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
    private void generateCategoryDetails(string categoryname,HttpContext context)
    {
        string boardNumber = "";
        string threadNumber = "";
        string postNumber = "";

        string cond1 = "(SELECT BoardName FROM Board WHERE CategoryName=@cname)";
        string cond2 = "(SELECT TopicID FROM Topic WHERE BoardName IN (" + cond1 + "))";
        SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Reply WHERE TopicID IN ("+cond2+")", con);
        cmd.Parameters.AddWithValue("@cname", categoryname);
        con.Open();
        SqlDataReader dr;
        dr = cmd.ExecuteReader();
        dr.Read();
        postNumber = dr[0].ToString();
        dr.Close();

        string tcond1 = "(SELECT BoardName FROM Board WHERE CategoryName=@cname)";
        cmd = new SqlCommand("SELECT COUNT(*) FROM Topic WHERE BoardName IN ("+tcond1+")", con);
        cmd.Parameters.AddWithValue("@cname", categoryname);
        dr = cmd.ExecuteReader();
        dr.Read();
        threadNumber = dr[0].ToString();
        dr.Close();

        cmd = new SqlCommand("SELECT COUNT(*) FROM Board WHERE CategoryName=@cname", con);
        cmd.Parameters.AddWithValue("@cname", categoryname);
        dr = cmd.ExecuteReader();
        dr.Read();
        boardNumber = dr[0].ToString();
        dr.Close();

        con.Close();

        context.Response.Write("<div><strong>Board: "); context.Response.Write(boardNumber); context.Response.Write("</strong></div>");
        context.Response.Write("<div><strong>Thread: "); context.Response.Write(threadNumber); context.Response.Write("</strong></div>");
        context.Response.Write("<div><strong>Post: "); context.Response.Write(postNumber); context.Response.Write("</strong></div>");
    }

    private void generateBoardDetails(string categoryname,HttpContext context)
    {
        string boardNumber = "";
        string threadNumber = "";
        string postNumber = "";

        string cond2 = "(SELECT TopicID FROM Topic WHERE BoardName=@bname)";
        SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Reply WHERE TopicID IN ("+cond2+")", con);
        cmd.Parameters.AddWithValue("@bname", categoryname);
        con.Open();
        SqlDataReader dr;
        dr = cmd.ExecuteReader();
        dr.Read();
        postNumber = dr[0].ToString();
        dr.Close();

        cmd = new SqlCommand("SELECT COUNT(*) FROM Topic WHERE BoardName =@bname", con);
        cmd.Parameters.AddWithValue("@bname", categoryname);
        dr = cmd.ExecuteReader();
        dr.Read();
        threadNumber = dr[0].ToString();
        dr.Close();

        boardNumber = "1";

        con.Close();

        context.Response.Write("<div><strong>Board: "); context.Response.Write(boardNumber); context.Response.Write("</strong></div>");
        context.Response.Write("<div><strong>Thread: "); context.Response.Write(threadNumber); context.Response.Write("</strong></div>");
        context.Response.Write("<div><strong>Post: "); context.Response.Write(postNumber); context.Response.Write("</strong></div>");
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}