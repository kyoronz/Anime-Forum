using CodeKicker.BBCode;
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

public partial class Topic : System.Web.UI.Page
{
    //Topic.aspx?page=12&title=1
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ACGDatabaseConnectionString"].ConnectionString);
    int rowToPrint = 10;
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["Username"] = "ChaiYiFan";
        if (!Page.IsPostBack)
        {
            displayTopic();
            checkPageNumber();
            generateTopic();
            generateAddReply();
        }        
    }
    private void displayTopic()
    {
        if (Request.QueryString["title"] == null)
        {
            Response.Redirect("PageNotFound.aspx");
        }
        string topicid = Request.QueryString["title"].ToString();
        Session["topicID"] = Request.QueryString["title"].ToString();

        string sql = "SELECT TopicTitle, TopicCreator, TopicDate, BoardName, UserDetails.ProfileID FROM Topic LEFT JOIN UserDetails ON Topic.TopicCreator = UserDetails.Username WHERE TopicID=@tid";
        SqlCommand cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@tid", topicid);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();        
        if (dr.HasRows)
        {
            dr.Read();            
            string topictitle = dr["TopicTitle"].ToString();
            string topiccreator = dr["TopicCreator"].ToString();
            string topicdate = getDataTableDate(dr["TopicDate"]);
            string boardname = dr["BoardName"].ToString();

            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["id"] = dr["ProfileID"].ToString();            
            string imageurl = "ProfileImageHandler.ashx?" + queryString;

            generateTopicTitle(topictitle, topiccreator, topicdate, boardname, imageurl);
        }
        else
        {
            con.Close();
            Response.Redirect("PageNotFound.aspx");
        }        
        con.Close();
    }
    private void generateTopicTitle(string topictitle, string topiccreator, string topicdate, string boardname, string imagelink)
    {
        //Image
        HtmlGenericControl imgcontainer = new HtmlGenericControl("div");
        imgcontainer.Attributes["class"] = "d-inline-block";
        HtmlGenericControl containerImage = new HtmlGenericControl("div");
        containerImage.Attributes["class"] = "d-inline-block";
        HtmlGenericControl imageTocenter = new HtmlGenericControl("div");
        imageTocenter.Attributes["class"] = "d-flex justify-content-center";
        HtmlGenericControl UserImage = new HtmlGenericControl("image");
        UserImage.Attributes["src"] = imagelink;
        UserImage.Attributes["style"] = "width:70px;height:70px;";

        imageTocenter.Controls.Add(UserImage);
        containerImage.Controls.Add(imageTocenter);
        imgcontainer.Controls.Add(containerImage);

        //Details
        HtmlGenericControl topicContainer = new HtmlGenericControl("div");
        topicContainer.Attributes["class"] = "ml-3 d-flex align-self-center";
        HtmlGenericControl containerTopic = new HtmlGenericControl("div");
        
 
        HtmlGenericControl topicTitleText = new HtmlGenericControl("div");
        topicTitleText.Attributes["class"] = "acgpagetitle";
        topicTitleText.InnerText = topictitle;

        HtmlGenericControl topicDetails = new HtmlGenericControl("div");

        NameValueCollection boardURLString = HttpUtility.ParseQueryString(string.Empty);
        boardURLString["category"] = boardname;
        string boardLink = "BoardView.aspx?"+boardURLString;
        NameValueCollection profileURLString = HttpUtility.ParseQueryString(string.Empty);
        profileURLString["id"] = "1";
        string profileLink = "";
        string topicDetailsString = "By <a style='color:inherit' href=''>" + topiccreator + "</a>, " + topicdate + " in <a style='color:inherit' href='"+boardLink+"'>" + boardname+"</a>";
        topicDetails.InnerHtml = topicDetailsString;
                
        containerTopic.Controls.Add(topicTitleText);
        containerTopic.Controls.Add(topicDetails);
        topicContainer.Controls.Add(containerTopic);

        acgtopictitle.Controls.Add(imgcontainer);
        acgtopictitle.Controls.Add(topicContainer);

    }
    private void checkPageNumber()
    {
        string pagestring=String.Empty;
        if (Request.QueryString["page"] == null)
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["page"] = "1";
            queryString["title"] = Request.QueryString["title"].ToString();
            Response.Redirect("Topic.aspx?" + queryString);
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
            if (page == 0) {                
                NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
                queryString["page"] = "1";
                queryString["title"] = Request.QueryString["title"].ToString();
                Response.Redirect("Topic.aspx?" + queryString);
            }
        }
        SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Reply WHERE TopicID=@tid", con);
        cmd.Parameters.AddWithValue("@tid", Request.QueryString["title"].ToString());
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
                noboard.InnerText = "No post available.";

                noboardContainer.Controls.Add(noboard);
                replylist.Controls.Add(noboardContainer);
                return;
            }
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["page"] = fullpageNumber.ToString();
            queryString["title"] = Request.QueryString["title"].ToString();
            Response.Redirect("Topic.aspx?"+queryString);
        }
        Session["topicPage"] = page;
    }    
    
    private void generateTopic()
    {
        string sql = "SELECT UserDetails.UName, UserDetails.ProfileID, UserDetails.USign, UserDetails.UType, ReplyDate, ReplyContent, ReplyID, ReplyStatus, UserDetails.Username FROM Reply LEFT JOIN UserDetails ON Reply.ReplyCreator = UserDetails.Username WHERE TopicID= @tid AND (ReplyStatus= @status OR ReplyStatus= @status2) ORDER BY ReplyDate";
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        da.SelectCommand.Parameters.AddWithValue("@tid", Request.QueryString["title"].ToString());
        da.SelectCommand.Parameters.AddWithValue("@status", "Active");
        da.SelectCommand.Parameters.AddWithValue("@status2", "Deleted");
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
            string title = dt.Rows[i]["Username"].ToString();
            //if (title.Equals(string.Empty))
            //{
            //    title = dt.Rows[i]["Username"].ToString();
            //}
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["id"] = dt.Rows[i]["ProfileID"].ToString();
            string imageurl = "ProfileImageHandler.ashx?" + queryString;
            string username = dt.Rows[i]["Username"].ToString();
            string date = getDataTableDate(dt.Rows[i]["ReplyDate"]);
            string content = dt.Rows[i]["ReplyContent"].ToString();
            string userType = dt.Rows[i]["UType"].ToString();
            if (userType.Equals("User")) { userType = "Member"; }
            else if (userType.Equals("Moderator")|| userType.Equals("Admin")) { userType = "Moderator"; }            
            string userStatus = dt.Rows[i]["USign"].ToString();
            string replyID = dt.Rows[i]["ReplyID"].ToString();
            int replyStatus = dt.Rows[i]["ReplyStatus"].ToString().Equals("Active")?1:2;
            content = BBCode.ToHtml(content);
            generateReply(title, imageurl, date, content,userType,userStatus,replyID,replyStatus,username);
            //generateReply(title, imageurl, date, content);
        }
        int fullpageNumber = dt.Rows.Count / rowToPrint;
        int lastpageNumber = dt.Rows.Count % rowToPrint;
        if (lastpageNumber != 0)
        {
            fullpageNumber += 1;
        }
        generatePagination(page, fullpageNumber);
    }
    private string getDataTableDate(Object dateObject)
    {
        string dateString = dateObject.ToString();
        DateTime realdate = DateTime.ParseExact(dateString, "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
        return realdate.ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture);
    }

    private void generateReply(string ReplyCreator, string ImageUrl, string ReplyDate, string ReplyContent, string UserType, string UserStatus, string ReplyID, int PostStatus, string username)
    {
        //status 1=active 2=deleted
        HtmlGenericControl replyContainer = new HtmlGenericControl("div");
        replyContainer.Attributes["class"] = "pb-4 row mt-4 acgsection";
        //post creator info
        HtmlGenericControl replyCreatorInfo = new HtmlGenericControl("div");
        replyCreatorInfo.Attributes["class"] = "d-inline-block col-3 p-0";
        HtmlGenericControl wrapperCreator = new HtmlGenericControl("div");
        wrapperCreator.Attributes["class"] = "d-flex flex-column";

        HtmlGenericControl wrapperName = new HtmlGenericControl("div");
        wrapperName.Attributes["class"] = "d-flex justify-content-center p-3 acgtopicheader";
        HtmlGenericControl replyName = new HtmlGenericControl("span");
        replyName.InnerText = ReplyCreator;

        HtmlGenericControl wrapperDetails = new HtmlGenericControl("div");
        wrapperDetails.Attributes["class"] = "d-flex flex-column align-items-center pt-3 px-3";

        HtmlGenericControl containerImage = new HtmlGenericControl("div");
        containerImage.Attributes["class"] = "d-inline-block";
        HtmlGenericControl imageTocenter = new HtmlGenericControl("div");
        imageTocenter.Attributes["class"] = "d-flex justify-content-center";
        HtmlGenericControl profileImage = new HtmlGenericControl("img");
        profileImage.Attributes["src"] = ImageUrl;
        profileImage.Attributes["style"]= "width:130px;height:130px;";
        imageTocenter.Controls.Add(profileImage);
        containerImage.Controls.Add(imageTocenter);

        HtmlGenericControl replyType = new HtmlGenericControl("span");
        replyType.InnerText = UserType;

        HtmlGenericControl replyStatus = new HtmlGenericControl("div");
        HtmlGenericControl replylblStatus = new HtmlGenericControl("span");
        HtmlGenericControl replytxtStatus = new HtmlGenericControl("span");
        replylblStatus.InnerText = "";
        replytxtStatus.InnerText = UserStatus;
        replyStatus.Controls.Add(replylblStatus);
        replyStatus.Controls.Add(replytxtStatus);

        wrapperName.Controls.Add(replyName);
        wrapperDetails.Controls.Add(containerImage);
        wrapperDetails.Controls.Add(replyType);
        wrapperDetails.Controls.Add(replyStatus);

        wrapperCreator.Controls.Add(wrapperName);
        wrapperCreator.Controls.Add(wrapperDetails);
        
        replyCreatorInfo.Controls.Add(wrapperCreator);

        //reply content details
        HtmlGenericControl replyContentDetails = new HtmlGenericControl("div");
        replyContentDetails.Attributes["class"] = "d-inline-block col-9 p-0";
        HtmlGenericControl wrapperContent = new HtmlGenericControl("div");
        wrapperContent.Attributes["class"] = "d-flex flex-column justify-content-center";
        HtmlGenericControl wrapperDate = new HtmlGenericControl("div");
        wrapperDate.Attributes["class"] = "d-flex justify-content-between p-3 acgtopicheader";
        HtmlGenericControl replyDate = new HtmlGenericControl("span");
        replyDate.InnerText = ReplyDate;
        HtmlGenericControl buttonContainer = new HtmlGenericControl("div");
        buttonContainer.Attributes["class"] = "acgtopicbutton";

        HtmlGenericControl Reportbtn = new HtmlGenericControl("a");
        Reportbtn.Attributes["class"] = "fas fa-exclamation-triangle";
        NameValueCollection reportquery = HttpUtility.ParseQueryString(string.Empty);
        reportquery["id"] = ReplyID;
        Reportbtn.Attributes["href"] = "AddReport.aspx?" + reportquery;

        HtmlGenericControl Editbtn = new HtmlGenericControl("a");
        Editbtn.Attributes["class"]= "ml-3 fas fa-edit";
        NameValueCollection editquery = HttpUtility.ParseQueryString(string.Empty);
        editquery["id"] = ReplyID;
        if (PostStatus == 1)
        {
            Editbtn.Attributes["href"] = "TopicEdit.aspx?" + editquery;
        }
        else
        {
            Editbtn.Attributes["href"] = "#" + editquery;
        }    
        
        HtmlGenericControl Deletebtn = new HtmlGenericControl("a");
        Deletebtn.Attributes["class"] = "ml-3 fas fa-trash-alt deletebutton";
        NameValueCollection deletequery = HttpUtility.ParseQueryString(string.Empty);
        deletequery["id"] = ReplyID;                
        Deletebtn.Attributes["href"] = "TopicDelete.ashx?"+deletequery;
        if (PostStatus == 1)
        {
            buttonContainer.Controls.Add(Reportbtn);
        }
       
        if (Session["Username"] != null)
        {
            if (Session["Username"].ToString().Equals(username))
            {                
                buttonContainer.Controls.Add(Editbtn);
                buttonContainer.Controls.Add(Deletebtn);
            }
        }
        
        

        HtmlGenericControl containerReplyContent = new HtmlGenericControl("div");
        containerReplyContent.Attributes["class"] = "p-3";
        HtmlGenericControl wrapperUserContent = new HtmlGenericControl("div");
        wrapperUserContent.Attributes["class"] = "pt-3 px-3";
        HtmlGenericControl replyContent = new HtmlGenericControl("div");

        if (PostStatus == 1)
        {
            //active
            replyContent.InnerHtml = ReplyContent;
        }
        else
        {
            //delete
            replyContent.InnerHtml = "<i>Deleted</i>";
        }


        wrapperDate.Controls.Add(replyDate);
        
        wrapperDate.Controls.Add(buttonContainer);
        wrapperUserContent.Controls.Add(replyContent);

        wrapperContent.Controls.Add(wrapperDate);
        wrapperContent.Controls.Add(wrapperUserContent);

        replyContentDetails.Controls.Add(wrapperContent);

        replyContainer.Controls.Add(replyCreatorInfo);
        replyContainer.Controls.Add(replyContentDetails);
        replylist.Controls.Add(replyContainer);
    }
    private void generatePagination(int currentpage, int lastpage)
    {
        int pageRange = 5;
        HtmlGenericControl paginationContainer = new HtmlGenericControl("ul");
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
            for (int i = (lastpage-9<=0?1:lastpage-9); i <= (lastpage < 10 ? 10 : lastpage); i++)
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
            if (lastpage != 1 && lastpage != 0)
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
        link.Attributes["href"] = "Topic.aspx?page=" + current.ToString() + "&title=" + Request.QueryString["title"].ToString();
        link.InnerText = current.ToString();
        pageHtml.Controls.Add(link);
        return pageHtml;
    }
    private HtmlGenericControl generatePageItem_PrevNext(Boolean isprevious,int selectedPage, int maxpage)
    {
        HtmlGenericControl pageHtml = new HtmlGenericControl("li");
        if (selectedPage == 1||selectedPage==maxpage)
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
        link.Attributes["href"] = "Topic.aspx?page=" + page.ToString() + "&title=" + Request.QueryString["title"].ToString();
        link.InnerText = innertext;
        pageHtml.Controls.Add(link);
        return pageHtml;
    }

    private void generateAddReply()
    {
        if (Session["Username"] == null)
        {
            btnpost.Visible = false;
        //    Button btnlogin = new Button();
        //    btnlogin.Text = "Login";
        //    btnlogin.Click += new System.EventHandler(btnlogin_Click);

        //    Button btnsignup = new Button();
        //    btnsignup.Text = "Sign Up";
        //    btnsignup.Attributes["class"] = "ml-3";
        //    btnsignup.Click += new System.EventHandler(btnsignup_Click);

        //    buttonContainer.Controls.Add(btnlogin);
        //    buttonContainer.Controls.Add(btnsignup);
        }
        else
        {
            btnlogin.Visible = false;
            btnsignup.Visible = false;
        //    Button btnpost = new Button();
        //    btnpost.Text = "Post Reply";
        //    btnpost.Click += new System.EventHandler(btnpost_Click);
        //    buttonContainer.Controls.Add(btnpost);
        }
    }
    protected void btnlogin_Click(object sender, EventArgs e)
    {
        Response.Redirect("Login2.aspx");
    }
    protected void btnsignup_Click(object sender, EventArgs e)
    {
        Response.Redirect("Register2.aspx");
    }
    protected void btnpost_Click(object sender, EventArgs e)
    {
        if (Session["Username"] == null)
        {
            //redirect to logged out notification page
            Response.Redirect("Disconnected.aspx");
            return;
        }
        string topicsql = "UPDATE Topic SET LastReply=@lastreply WHERE TopicID=@tid";
        string replysql = "INSERT INTO Reply (TopicID, ReplyContent, ReplyDate, ReplyCreator, ReplyStatus) VALUES (@tid,@content,@rdate,@creator,@status)";
        string topicID = Session["topicID"].ToString();
        string lastReply = Session["Username"].ToString();

        TextBox txtreplycontent = (TextBox)pnlreply.FindControl(txtContent.ID);
        string content = txtreplycontent.Text;
        DateTime date = DateTime.Now;
        string replystatus = "Active";


        SqlCommand cmd = new SqlCommand(topicsql, con);
        cmd.Parameters.AddWithValue("@lastReply", lastReply);
        cmd.Parameters.AddWithValue("@tid", topicID);

        con.Open();
        int i = cmd.ExecuteNonQuery();

        SqlCommand cmd2 = new SqlCommand(replysql, con);
        cmd2.Parameters.AddWithValue("@tid", Convert.ToInt32(topicID));
        cmd2.Parameters.AddWithValue("@content", content);
        cmd2.Parameters.AddWithValue("@rdate", date);
        cmd2.Parameters.AddWithValue("@creator", lastReply);
        cmd2.Parameters.AddWithValue("@status", replystatus);

        int j = cmd2.ExecuteNonQuery();

        con.Close();
        NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString["page"] = "1";
        queryString["title"] = topicID;
        Response.Redirect("Topic.aspx?" + queryString);
    }
}