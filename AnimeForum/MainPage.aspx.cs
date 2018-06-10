using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;

public partial class _Default : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ACGDatabaseConnectionString"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["Username"] = "ChaiYiFan";
        if (!Page.IsPostBack)
        {
            generateCategoryList();
        }
        
    }
    private int getCategoryNumber()
    {
        int categoryNumber = 0;
        string sql = "SELECT COUNT(*) FROM Category";
        SqlCommand cmd = new SqlCommand(sql, con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        dr.Read();
        categoryNumber = Convert.ToInt32(dr[0].ToString());
        con.Close();
        return categoryNumber;
    }
    private void generateCategoryList()
    {
        Panel categoryContainer = new Panel();

        //get category number and store category name
        int categoryNumber = getCategoryNumber();
        if (categoryNumber == 0)
        {
            return;
        }

        string[] categoryName = new string[categoryNumber];
        SqlCommand cmd = new SqlCommand("SELECT * FROM Category", con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        for (int i = 0; i < categoryNumber; i++)
        {
            dr.Read();
            categoryName[i] = dr["CategoryName"].ToString();
            ListItem licategory = new ListItem();
            licategory.Value = dr[1].ToString(); ;
            //ddlCategory.Items.Add(dr["CategoryName"].ToString());
            //ddlNewCategory.Items.Add(dr["CategoryName"].ToString());
        }
        con.Close();

        //loop through all category name
        for (int i = 0; i < categoryNumber; i++)
        {
            //define container 
            //then define category container to store category details
            HtmlGenericControl container = new HtmlGenericControl("div");

            HtmlGenericControl categoryNameContainer = new HtmlGenericControl("div");
            categoryNameContainer.Attributes["class"] = "px-3 pt-3 pb-2 d-flex justify-content-between acgcategory-structure";

            HtmlGenericControl categoryNameText = new HtmlGenericControl("span");
            categoryNameText.InnerText = categoryName[i];

            categoryNameContainer.Controls.Add(categoryNameText);
            //categoryNameContainer.Controls.Add(generateControlBoard(1, categoryName[i]));

            container.Controls.Add(categoryNameContainer);
            container.Controls.Add(generateCategory(categoryName[i]));
            categoryContainer.Controls.Add(container);
        }
        categoryList.Controls.Add(categoryContainer);
    }

    private HtmlGenericControl generateCategory(string categoryName)
    {
        //define one container and put board details
        HtmlGenericControl boardlistContainer = new HtmlGenericControl("div");
        boardlistContainer.Attributes["class"] = "p-2 pl-4 pr-3 d-flex flex-column";
        SqlCommand cmd = new SqlCommand("SELECT BoardName, Board.Description as BoardDescription FROM Board LEFT JOIN Category ON Board.CategoryName = Category.CategoryName WHERE Board.CategoryName=@cid", con);
        cmd.Parameters.AddWithValue("@cid", categoryName);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            boardlistContainer.Controls.Add(generateBoard(dr["BoardName"].ToString(), dr["BoardDescription"].ToString()));
        }
        con.Close();
        return boardlistContainer;
    }

    private HtmlGenericControl generateBoard(string boardName, string boardDesc)
    {
        HtmlGenericControl boardContainer = new HtmlGenericControl("div");
        boardContainer.Attributes["class"] = "mt-2 pl-2 d-flex justify-content-between";

        HtmlGenericControl boardNameText = new HtmlGenericControl("div");
        HtmlGenericControl boardlink = new HtmlGenericControl("a");

        boardlink.InnerText = boardName;
        NameValueCollection boardURLString = HttpUtility.ParseQueryString(string.Empty);
        boardURLString["category"] = boardName;
        
        boardlink.Attributes["href"] = "BoardView.aspx?" + boardURLString; 

        boardNameText.Controls.Add(boardlink);
        boardContainer.Controls.Add(boardNameText);
        //boardContainer.Controls.Add(generateControlBoard(2, boardName));

        return boardContainer;
    }

}
