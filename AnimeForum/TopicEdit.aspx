<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="TopicEdit.aspx.cs" Inherits="BoardAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">    
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <script src="jquery.wysibb.min.js"></script>
    <link rel="stylesheet" href="wbbtheme.css" />
    <script>
        $(function () {
            $("#<%=txtContent.ClientID%>").wysibb();
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="background: url(https://wallup.net/wp-content/uploads/2015/12/166724-landscape-digital_art-power_lines-signs-bokeh-clouds-sunlight.jpg) no-repeat; background-size: cover; background-position-x: center; position: fixed; z-index: -1; height: 100%; width: 100%; top: 0; left: 0;"></div>

    <div class="p-3">
        <div class="px-3 py-2 acgpage-topic">
            <div class="acgpagetitle">Edit Post</div>
        </div>

        <!--Form #0d1a26-->
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        
        <div class="mt-4">
            <div class="container-fluid p-3 acgsection">
                <!--alert-->
                <div id="acgalert" runat="server">
                </div>
                <div class="py-3 px-1 d-flex mt-2 row">
                    <span class="col-2">Content: </span>
                    <div class="col-9 p-0">
                        <asp:TextBox ID="txtContent" runat="server" Rows="10" TextMode="MultiLine" Width="600" ForeColor="Black"></asp:TextBox>
                    </div>
                </div>
                <div>
                </div>
                <div class="row">
                    <span class="col-2"></span>
                    <div class="d-flex col-9 p-0 mt-2">
                        <asp:Button ID="btnCreate" runat="server" Text="Edit Post" CssClass="ml-auto" OnClick="btnEdit_Click" OnClientClick="return checkEmpty();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

