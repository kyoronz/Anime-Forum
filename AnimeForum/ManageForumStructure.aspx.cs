using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class ManageForumStructure : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ACGDatabaseConnectionString"].ConnectionString);  
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["Username"] = "ChaiYiFan";
        if (!Page.IsPostBack)
        {           
            checkAuthority();
            generateCategoryList();
        }
    }
    private void checkAuthority()
    {
        if (Session["Username"] == null)
        {
            Response.Redirect("PageNotFound.aspx");
        }
        SqlCommand cmd = new SqlCommand("SELECT * FROM UserDetails WHERE Username=@username AND UType=@type",con);
        cmd.Parameters.AddWithValue("@username", Session["Username"]);
        cmd.Parameters.AddWithValue("@type", "Admin");
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            //gain access
        }
        else
        {
            con.Close();
            Response.Redirect("PageNotFound.aspx");
        }
        con.Close();
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
            ddlCategory.Items.Add(dr["CategoryName"].ToString());
            ddlNewCategory.Items.Add(dr["CategoryName"].ToString());
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
            categoryNameContainer.Controls.Add(generateControlBoard(1, categoryName[i]));

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
        boardNameText.InnerText = boardName;

        boardContainer.Controls.Add(boardNameText);
        boardContainer.Controls.Add(generateControlBoard(2, boardName));

        return boardContainer;
    }
    private HtmlGenericControl generateControlBoard(int type, string controlID)
    {
        //type 1 = category, 2 = board
        HtmlGenericControl controlContainer = new HtmlGenericControl("div");
        Button btnedit = new Button();
        btnedit.Text = "Edit";
        Button btndelete = new Button();
        btndelete.Text = "Delete";
        if (type == 1)
        {            
            btnedit.Attributes["class"] = "edit";
            btnedit.OnClientClick= "return editClick(" + type + ",'" + controlID + "');";
            btndelete.Attributes["class"] = "delete";
            btndelete.OnClientClick = "return deleteClick(" + type + ",'" + controlID + "');";
        }
        else if (type == 2)
        {
            btnedit.Attributes["class"] = "edit";
            btnedit.OnClientClick = "return editClick(" + type + ",'" + controlID + "');";
            btndelete.Attributes["class"] = "delete";
            btndelete.OnClientClick = "return deleteClick(" + type + ",'" + controlID + "');";
        }

        controlContainer.Controls.Add(btnedit);
        controlContainer.Controls.Add(btndelete);
        return controlContainer;
    }
    private void checkCookieAvailable(string cookiesName)
    {
        if (Request.Cookies[cookiesName] == null)
        {
            Response.Redirect("ManageForumStructure.aspx");
        }
    }
    protected void btnDialogAddCategory_Click(object sender, EventArgs e)
    {
        if (txtCategoryName.Text.Equals(string.Empty))
        {
            Response.Redirect("ManageForumStructure.aspx");
            return;
        }
        SqlCommand cmd = new SqlCommand("INSERT INTO Category (CategoryName, Description) VALUES (@name,@desc)", con);
        cmd.Parameters.AddWithValue("@name", txtCategoryName.Text);
        cmd.Parameters.AddWithValue("@desc", txtCategoryDesc.Text);
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
        Response.Redirect("ManageForumStructure.aspx");
    }

    protected void btnDialogAddBoard_Click(object sender, EventArgs e)
    {
        if (txtBoardName.Text.Equals(string.Empty))
        {
            Response.Redirect("ManageForumStructure.aspx");
            return;
        }
        SqlCommand cmd = new SqlCommand("INSERT INTO Board (BoardName, Description, CategoryName) VALUES (@name,@desc,@cid)", con);
        cmd.Parameters.AddWithValue("@name", txtBoardName.Text);
        cmd.Parameters.AddWithValue("@desc", txtBoardDesc.Text);
        cmd.Parameters.AddWithValue("@cid", ddlCategory.SelectedValue);
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
        Response.Redirect("ManageForumStructure.aspx");
    }

    protected void btnDialogDeleteCategory_Click(object sender, EventArgs e)
    {
        checkCookieAvailable("categoryname_delete");
        string categoryToDelete = Request.Cookies["categoryname_delete"].Value;
        string cond1 = "(SELECT BoardName FROM Board WHERE CategoryName=@cid)";
        string cond2 = "(SELECT TopicID FROM Topic WHERE BoardName IN ("+cond1+"))";
        SqlCommand cmd = new SqlCommand("DELETE FROM Reply WHERE TopicID IN "+cond2+" ", con);
        cmd.Parameters.AddWithValue("@cid", categoryToDelete);
        con.Open();
        cmd.ExecuteNonQuery();

        cmd = new SqlCommand("DELETE FROM Topic WHERE BoardName IN "+cond1+"", con);
        cmd.Parameters.AddWithValue("@cid", categoryToDelete);
        cmd.ExecuteNonQuery();

        cmd = new SqlCommand("DELETE FROM Board WHERE CategoryName=@cid", con);
        cmd.Parameters.AddWithValue("@cid", categoryToDelete);
        cmd.ExecuteNonQuery();

        cmd = new SqlCommand("DELETE FROM Category WHERE CategoryName=@cid", con);
        cmd.Parameters.AddWithValue("@cid", categoryToDelete);
        cmd.ExecuteNonQuery();
        con.Close();
        Response.Redirect("ManageForumStructure.aspx");
    }

    protected void btnDialogDeleteBoard_Click(object sender, EventArgs e)
    {
        checkCookieAvailable("boardname_delete");
        string boardToDelete = Request.Cookies["boardname_delete"].Value;
        string cond1 = "(SELECT TopicID FROM Topic WHERE BoardName=@bname)";
        SqlCommand cmd = new SqlCommand("DELETE FROM Reply WHERE TopicID IN "+cond1+" ", con);
        cmd.Parameters.AddWithValue("@bname", boardToDelete);
        con.Open();
        cmd.ExecuteNonQuery();

        cmd = new SqlCommand("DELETE FROM Topic WHERE BoardName=@bname", con);
        cmd.Parameters.AddWithValue("@bname", boardToDelete);
        cmd.ExecuteNonQuery();

        cmd = new SqlCommand("DELETE FROM Board WHERE BoardName=@bname", con);
        cmd.Parameters.AddWithValue("@bname", boardToDelete);
        cmd.ExecuteNonQuery();
        con.Close();
        Response.Redirect("ManageForumStructure.aspx");
    }

    protected void btnDialogEditCategory_Click(object sender, EventArgs e)
    {
        checkCookieAvailable("txtOriginalCategoryName");
        string originalCategory = Request.Cookies["txtOriginalCategoryName"].Value;
        SqlCommand cmd = new SqlCommand("UPDATE Board SET CategoryName=@cname WHERE CategoryName=@cid", con);
        cmd.Parameters.AddWithValue("@cname", txtNewCategoryName.Text);        
        cmd.Parameters.AddWithValue("@cid", originalCategory);
        con.Open();
        cmd.ExecuteNonQuery();

        cmd = new SqlCommand("UPDATE Category SET CategoryName=@name, Description=@desc WHERE CategoryName=@cid", con);
        cmd.Parameters.AddWithValue("@name", txtNewCategoryName.Text);
        cmd.Parameters.AddWithValue("@desc", txtNewCategoryDesc.Text);
        cmd.Parameters.AddWithValue("@cid", originalCategory);
        cmd.ExecuteNonQuery();

        con.Close();
        Response.Redirect("ManageForumStructure.aspx");
    }

    protected void btnDialogEditBoard_Click(object sender, EventArgs e)
    {
        checkCookieAvailable("txtOriginalBoardName");
        string originalBoard = Request.Cookies["txtOriginalBoardName"].Value;
        SqlCommand cmd = new SqlCommand("UPDATE Topic SET BoardName=@name WHERE BoardName=@bid", con);
        cmd.Parameters.AddWithValue("@name", txtNewBoardName.Text);        
        cmd.Parameters.AddWithValue("@bid", originalBoard);
        con.Open();
        cmd.ExecuteNonQuery();

        cmd = new SqlCommand("UPDATE Board SET BoardName=@name, Description=@desc, CategoryName=@cname WHERE BoardName=@bid", con);
        cmd.Parameters.AddWithValue("@name", txtNewBoardName.Text);
        cmd.Parameters.AddWithValue("@desc", txtNewBoardDesc.Text);
        cmd.Parameters.AddWithValue("@cname", ddlNewCategory.SelectedValue);
        cmd.Parameters.AddWithValue("@bid", originalBoard);        
        cmd.ExecuteNonQuery();

        con.Close();
        Response.Redirect("ManageForumStructure.aspx");
    }
}