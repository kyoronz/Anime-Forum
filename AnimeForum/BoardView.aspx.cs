using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class BoardView : System.Web.UI.Page
{
    //BoardView.aspx?category=FAQ&page=1
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ACGDatabaseConnectionString"].ConnectionString);
    private int rowToPrint = 15;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session["Username"] = "ChaiYiFan";
            displaySubCategory();
            checkPageNumber();            
            topicGeneration();
            if (Session["Username"] == null)
            {
                btnCreateTopic.Visible = false;
            }
            //string ttitle = "New Generated Topic";
            //string twriter = "Kyoron";
            //string tdate = "04/05/2018";
            //int treplyNumber = 200;
            //string tlastreplyName = "Kyoron";
            //string tlastreplyDate = "04/05/2018";

                //GenerateTopic(ttitle, twriter, tdate, treplyNumber, tlastreplyName, tlastreplyDate, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT7FkQNurL0DFuWxltZvzFWIM3DGmyyl4FP2OSQBe5wNd8dC5fW0Q", "");
        }
        
    }
    private void displaySubCategory()
    {
        if (Request.QueryString["category"] == null)
        {
            Response.Redirect("MainPage.aspx");
        }
        Session["category"] = Request.QueryString["category"];
        string boardCategory = Request.QueryString["category"];
        
        SqlCommand cmd = new SqlCommand("SELECT BoardName FROM Board WHERE BoardName=@bname", con);
        cmd.Parameters.AddWithValue("@bname", Request.QueryString["category"].ToString());
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            dr.Read();
            categoryTitle.InnerText = dr["BoardName"].ToString();
        }
        else
        {
            con.Close();
            Response.Redirect("PageNotFound.aspx");
        }
        con.Close();
    }
    private void checkPageNumber()
    {
        string pagestring=String.Empty;
        if (Request.QueryString["page"] == null)
        {
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
            query["category"] = Request.QueryString["category"].ToString();
            query["page"] = "1";
            Response.Redirect("BoardView.aspx?" + query);
        }
        else
        {
            pagestring = Request.QueryString["page"].ToString();
        }

        int page = 0;
        if (pagestring.Equals(String.Empty))
        {
            page = 1;
        }
        else
        {
            page = Convert.ToInt32(pagestring);
            if (page == 0)
            {
                NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
                query["category"] = Request.QueryString["category"].ToString();
                query["page"] = "1";
                Response.Redirect("BoardView.aspx?" + query);
            }
        }
        SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Topic WHERE BoardName=@bid", con);
        cmd.Parameters.AddWithValue("@bid", Request.QueryString["category"].ToString());
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        dr.Read();
        int rowCount = Convert.ToInt32(dr[0].ToString());
        con.Close();
        int fullpageNumber = rowCount / rowToPrint;
        int lastpageNumber = rowCount % rowToPrint;
        if (lastpageNumber != 0)
        {
            fullpageNumber += 1;
        }
        if (page > fullpageNumber)
        {
            if (rowCount == 0)
            {
                HtmlGenericControl noboardContainer = new HtmlGenericControl("div");
                noboardContainer.Attributes["class"] = "d-flex justify-content-center align-items-center";
                noboardContainer.Attributes["style"] = "height:10rem";

                HtmlGenericControl noboard = new HtmlGenericControl("div");
                noboard.Attributes["class"] = "";
                noboard.InnerText = "No topic available.";

                noboardContainer.Controls.Add(noboard);                                
                topic.Controls.Add(noboardContainer);
                return;
            }
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
            query["category"] = Request.QueryString["category"].ToString();
            query["page"] = fullpageNumber.ToString(); 
            Response.Redirect("BoardView.aspx?" + query);
        }        
        Session["boardPage"] = page;        
    }
    private string getDataTableDate(Object dateObject) { 
        return dateObject.ToString();
       // string dateString = dateObject.ToString();
        //DateTime realdate = DateTime.ParseExact(dateString, "d'/'M'/'yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
        //return realdate.ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture);
    }
    private void topicGeneration()
    {
        //UserDetails in sql is basically information of last reply user
        string lastreplydatesql = "(SELECT TOP 1 ReplyDate FROM Reply WHERE Reply.TopicID = Topic.TopicID AND Reply.ReplyCreator = Topic.LastReply ORDER BY ReplyDate DESC) AS ReplyDate";
        string replyCountsql = "(SELECT COUNT(*) FROM Reply WHERE Reply.TopicID = Topic.TopicID AND ReplyStatus=@replystatus) as ReplyCount ";
        string sql = "SELECT TopicID, TopicDate, TopicTitle, TopicCreator, " + replyCountsql + ", UserDetails.Username, " + lastreplydatesql + ", UserDetails.ProfileID FROM (Topic LEFT JOIN UserDetails On Topic.LastReply = UserDetails.Username) WHERE Topic.BoardName=@bname AND TopicStatus=@status";
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        da.SelectCommand.Parameters.AddWithValue("@replystatus", "Active");
        da.SelectCommand.Parameters.AddWithValue("@bname", Request.QueryString["category"].ToString());
        da.SelectCommand.Parameters.AddWithValue("@status", "Active");
        SqlConnectionStringBuilder cmdbuild = new SqlConnectionStringBuilder();
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        int page = Convert.ToInt32(Request.QueryString["page"].ToString());
        int startingrow = (page - 1) * rowToPrint;
        int endingrow = (page * rowToPrint);
        if (endingrow > dt.Rows.Count)
        {
            endingrow = dt.Rows.Count;
        }

        for (int i = startingrow; i < endingrow; i++)
        {
            string title = dt.Rows[i]["TopicTitle"].ToString();
            string date = getDataTableDate(dt.Rows[i]["TopicDate"]);
            string creator = dt.Rows[i]["TopicCreator"].ToString();

            string replyDate = getDataTableDate(dt.Rows[i]["ReplyDate"]); 
            int replyCount = Convert.ToInt32(dt.Rows[i]["ReplyCount"].ToString());
            string replyName = dt.Rows[i]["Username"].ToString();

            NameValueCollection imagequeryString = HttpUtility.ParseQueryString(string.Empty);
            imagequeryString["id"] = dt.Rows[i]["ProfileID"].ToString();
            string imageurl = "ProfileImageHandler.ashx?" + imagequeryString;

            string topicID = dt.Rows[i]["TopicID"].ToString();
            NameValueCollection topicqueryString = HttpUtility.ParseQueryString(string.Empty);
            topicqueryString["title"] = dt.Rows[i]["TopicID"].ToString();
            topicqueryString["page"] = "1";
            string topiclink = "Topic.aspx?" + topicqueryString;

            GenerateTopic(title,creator,date,replyCount,replyName,replyDate,imageurl,topiclink);
        }
        int fullpageNumber = dt.Rows.Count / rowToPrint;
        int lastpageNumber = dt.Rows.Count % rowToPrint;
        if (lastpageNumber != 0)
        {
            fullpageNumber += 1;
        }
        generatePagination(page, fullpageNumber);
    }
    private void generatePagination(int currentpage, int lastpage)
    {
        int pageRange = 5;
        HtmlGenericControl paginationContainer = new HtmlGenericControl("ul");
        paginationContainer.Attributes["class"]="mb-0 pl-2";
        //previous page item
        if (currentpage != 1)
        {
            paginationContainer.Controls.Add(generatePageItem_PrevNext(true, currentpage, lastpage));
        }
        //middle page
        if (lastpage < 10) //page less than 10 page
        {
            for (int i = 1; i <= lastpage; i++)
            {
                paginationContainer.Controls.Add(generatePageItem(i, currentpage));
            }
        }
        else if (currentpage <= pageRange) //current page that less than range
        {
            for (int i = 1; i <= 10; i++)
            {
                paginationContainer.Controls.Add(generatePageItem(i, currentpage));
            }
        }
        else if (lastpage - currentpage < pageRange) //page with the last page with difference less than the range
        {
            for (int i = (lastpage - 9 <= 0 ? 1 : lastpage - 9); i <= (lastpage < 10 ? 10 : lastpage); i++)
            {
                paginationContainer.Controls.Add(generatePageItem(i, currentpage));
            }
        }
        else //middle current page
        {
            int left = currentpage - pageRange;
            int right = currentpage + pageRange;
            for (int i = 0; i < lastpage; i++)
            {
                //if page is one, or page is last page
                //or page in the range of left page of right page
                if (i >= left && i < right)
                {
                    paginationContainer.Controls.Add(generatePageItem(i, currentpage));
                }
                else if (i >= right) { break; }
            }
        }
        //last button
        if (currentpage != lastpage)
        {
            if (lastpage != 1&&lastpage!=0)
            {
                paginationContainer.Controls.Add(generatePageItem_PrevNext(false, currentpage, lastpage));
            }
        }

        acgpagination.Controls.Add(paginationContainer);
    }
    private HtmlGenericControl generatePageItem(int current, int selectedPage)
    {
        HtmlGenericControl pageHtml = new HtmlGenericControl("li");
        if (selectedPage == current)
        {
            pageHtml.Attributes["class"] = "active acgpagination-pageitem";
        }
        else
        {
            pageHtml.Attributes["class"] = "acgpagination-pageitem";
        }

        HtmlGenericControl link = new HtmlGenericControl("a");
        link.Attributes["href"] = "BoardView.aspx?page=" + current.ToString() + "&category=" + Request.QueryString["category"].ToString();
        link.InnerText = current.ToString();
        pageHtml.Controls.Add(link);
        return pageHtml;
    }
    private HtmlGenericControl generatePageItem_PrevNext(Boolean isprevious, int selectedPage, int maxpage)
    {
        HtmlGenericControl pageHtml = new HtmlGenericControl("li");
        if (selectedPage == 1 || selectedPage == maxpage)
        {
            pageHtml.Attributes["class"] = "disabled acgpagination-pageitem";
        }
        else
        {
            pageHtml.Attributes["class"] = "acgpagination-pageitem";
        }
        int page = 0;
        string innertext = "";
        if (isprevious)
        {
            page = 1;
            innertext = "Prev";
        }
        else
        {
            page = maxpage;
            innertext = "Last";
        }
        HtmlGenericControl link = new HtmlGenericControl("a");
        link.Attributes["href"] = "BoardView.aspx?page=" + page.ToString() + "&category=" + Request.QueryString["category"].ToString();
        link.InnerText = innertext;
        pageHtml.Controls.Add(link);
        return pageHtml;
    }
    void GenerateTopic(string title, string writer, string date, int replyNumber, string lastreplyName, string lastreplyDate, string lastreplyImage, string topiclink)
    {
        HtmlGenericControl topicContainer = new HtmlGenericControl("div");
        topicContainer.Attributes["class"] = "container-fluid pt-2 pb-2 acgtopic";
        HtmlGenericControl topicRow = new HtmlGenericControl("div");
        topicRow.Attributes["class"] = "row";

        //HtmlGenericControl Ltitle;
        //left
        HtmlGenericControl leftContainer = new HtmlGenericControl("div");
        leftContainer.Attributes["class"] = "col-8 d-inline-block";
        HtmlGenericControl Ltitle = new HtmlGenericControl("div");
        HtmlGenericControl Ltitletext = new HtmlGenericControl("a");
        Ltitletext.Attributes["class"] = "acgtopic-title";
        Ltitletext.Attributes["href"] = topiclink;
        Ltitletext.InnerText = title;
        HtmlGenericControl Ltitlecretor = new HtmlGenericControl("div");
        HtmlGenericControl Ltitlecretortext = new HtmlGenericControl("span");
        Ltitlecretortext.Attributes["class"] = "acgtopic-subtext";
        Ltitlecretortext.InnerText = writer + ", " + date;
        Ltitle.Controls.Add(Ltitletext);
        Ltitlecretor.Controls.Add(Ltitlecretortext);

        leftContainer.Controls.Add(Ltitle);
        leftContainer.Controls.Add(Ltitlecretor);

        HtmlGenericControl centerContainer = new HtmlGenericControl("div");
        centerContainer.Attributes["class"] = "col-2 d-inline-block";
        HtmlGenericControl CinnerContainer = new HtmlGenericControl("div");
        CinnerContainer.Attributes["class"] = "row";
        HtmlGenericControl CinnerContainer2 = new HtmlGenericControl("div");
        CinnerContainer2.Attributes["class"] = "col-9";

        HtmlGenericControl CrepliesContainer = new HtmlGenericControl("div");
        CrepliesContainer.Attributes["class"] = "d-flex justify-content-between";
        HtmlGenericControl Creplies = new HtmlGenericControl("span");
        Creplies.Attributes["class"] = "acgtopic-subtext";
        if (replyNumber != 1)
        {
            Creplies.InnerText = replyNumber + " replies";
        }
        else
        {
            Creplies.InnerText = replyNumber + " reply";
        }

        centerContainer.Controls.Add(CinnerContainer);
        CinnerContainer.Controls.Add(CinnerContainer2);
        CinnerContainer2.Controls.Add(CrepliesContainer);

        CrepliesContainer.Controls.Add(Creplies);

        HtmlGenericControl rightContainer = new HtmlGenericControl("div");
        rightContainer.Attributes["class"] = "col-2 d-flex align-content-center";
        HtmlGenericControl RimageContainer = new HtmlGenericControl("div");
        RimageContainer.Attributes["class"] = "d-inline-block";
        HtmlGenericControl RimageTocenter = new HtmlGenericControl("div");
        RimageTocenter.Attributes["class"] = "d-flex justify-content-center";
        HtmlGenericControl Rimage = new HtmlGenericControl("image");
        Rimage.Attributes["src"] = lastreplyImage;
        Rimage.Attributes["class"] = "acgtopic-image40px";
        //Rimage.Attributes["src"] = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT7FkQNurL0DFuWxltZvzFWIM3DGmyyl4FP2OSQBe5wNd8dC5fW0Q";

        HtmlGenericControl RlastreplyContainer = new HtmlGenericControl("div");
        RlastreplyContainer.Attributes["class"] = "pl-1 d-inline-block";
        HtmlGenericControl Rlastreplyname = new HtmlGenericControl("span");
        Rlastreplyname.Attributes["class"] = "d-block acgtopic-subtext";
        Rlastreplyname.InnerText = lastreplyName;
        HtmlGenericControl Rlastreplydate = new HtmlGenericControl("span");
        Rlastreplydate.Attributes["class"] = "acgtopic-subtext";
        Rlastreplydate.InnerText = lastreplyDate;

        RimageContainer.Controls.Add(RimageTocenter);
        RimageTocenter.Controls.Add(Rimage);

        RlastreplyContainer.Controls.Add(Rlastreplyname);
        RlastreplyContainer.Controls.Add(Rlastreplydate);

        rightContainer.Controls.Add(RimageContainer);
        rightContainer.Controls.Add(RlastreplyContainer);

        topic.Controls.Add(topicContainer);
        topicContainer.Controls.Add(topicRow);
        topicRow.Controls.Add(leftContainer);
        topicRow.Controls.Add(centerContainer);
        topicRow.Controls.Add(rightContainer);
    }

    protected void btnCreateTopic_Click(object sender, EventArgs e)
    {
        NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
        query["category"]= Request.QueryString["category"].ToString();
        Response.Redirect("TopicAdd.aspx?" + query);
    }

    private void topicGeneration12()
    {
        SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Topic", con);
        SqlDataReader dr;

        con.Open();
        dr = cmd.ExecuteReader();
        int totalTopic;
        if (dr.HasRows)
        {
            dr.Read();
            totalTopic = Convert.ToInt32(dr[0].ToString());
            dr.Close();
        }
        else
        {
            totalTopic = 0;
        }
        con.Close();

        int startingNumber = 20 * Convert.ToInt32(Request.QueryString["page"]);
        int endingNumber = startingNumber + 20;
        if (endingNumber > totalTopic)
        {
            endingNumber = totalTopic;
        }

        for (int i = startingNumber; i < endingNumber; i++)
        {
            string lastreplydatesql = "(SELECT TOP 1 ReplyDate FROM Reply WHERE Reply.TopicID = Topic.TopicID ORDER BY ReplyDate DESC) AS ReplyDate";
            string replyCountsql = "(SELECT COUNT(*) FROM Reply WHERE Reply.TopicID = Topic.TopicID) as ReplyCount";
            string sql = "SELECT TopicID, TopicDate, TopicTitle, TopicCreator, " + replyCountsql + ", UserDetails.UName, " + lastreplydatesql + ", UserDetails.ProfileID FROM (Topic LEFT JOIN UserDetails On Topic.LastReply = UserDetails.Username)";

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            con.Open();
            da.Fill(dt);
            con.Close();

            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["ID"] = dt.Rows[i]["ProfileID"].ToString();

            string title = (string)dt.Rows[i]["TopicTitle"];
            string writer = (string)dt.Rows[i]["TopicCreator"];
            string date = dt.Rows[i][1].ToString();
            int replyNumber = Convert.ToInt32(dt.Rows[i]["ReplyCount"].ToString());
            string lastreplyName = (string)dt.Rows[i]["UName"];
            string lastreplyDate = dt.Rows[i]["ReplyDate"].ToString();
            string lastreplyImage = "ProfileImageHandler.ashx?" + queryString;

            GenerateTopic(title, writer, date, replyNumber, lastreplyName, lastreplyDate, lastreplyImage,"");
        }
    }

    

    
}