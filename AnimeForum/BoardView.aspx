<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="BoardView.aspx.cs" Inherits="BoardView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .acgpagination-pageitem {
            display: inline-block;
            padding: 5px;
            color:#dfdfdf;
        }

        .acgpagination li a{
            color:#dfdfdf;
            float: left;
            padding: 8px 16px;
            text-decoration: none;
        }
        .acgpagination a:hover{
            color:#bfbfbf;            
        }
        .acgpagination a, active {
            color:#ffffff;            
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="background: url(https://wallup.net/wp-content/uploads/2015/12/166724-landscape-digital_art-power_lines-signs-bokeh-clouds-sunlight.jpg) no-repeat; background-size: cover; background-position-x: center; position: fixed; z-index: -1; height: 100%; width: 100%; top: 0; left: 0;"></div>

    <div class="p-3">
        <!-- Title-->
        <div class="px-3 py-2 d-flex justify-content-between align-items-center acgpage-topic">
            <div id="categoryTitle" runat="server" class="acgpagetitle"></div>
            <div>
                <asp:Button ID="btnCreateTopic" runat="server" Text="Create Topic" OnClick="btnCreateTopic_Click" />
            </div>
        </div>


        <div class="acgsection">
            <!-- title list -->
            <div class="mt-3 container-fluid acgsectiontitle" style="background-color: rgba(10,10,10,0.8); border-bottom: 1px solid white">
                <div class="row py-3">
                    <div class="col-8 d-inline-block">
                        <span>Title</span>
                    </div>
                    <div class="col-2 d-inline-block">
                        <span>Replies</span>
                    </div>
                    <div class="col-2 d-inline-block">
                        <span>Last Reply</span>
                    </div>
                </div>
            </div>
            <!--Topic-->
            <div id="topic" runat="server">
            </div>
            <!--Pagination-->
            <div class="container-fluid acgpagination-section ">
                <div class="py-2 row">
                    <div id="acgpagination" runat="server"></div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

