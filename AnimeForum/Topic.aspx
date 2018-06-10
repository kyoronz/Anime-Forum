<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="Topic.aspx.cs" Inherits="Topic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.0.12/css/all.css" integrity="sha384-G0fIWCsCzJIMAVNQPfjH08cyYaUtMwjJwqiRKxxE/rx96Uroj1BtIQ6MLJuheaO9" crossorigin="anonymous" />
    <style type="text/css">
        .acgtopicheader {
            background-color: rgba(90,90,90,0.8);
            font-weight: bold;
            color: #efefef;
        }

        .acgtopicbutton a {
            color: inherit;
        }

            .acgtopicbutton a:hover {
                color: #cfcfcf;
            }
    </style>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <script src="jquery.wysibb.min.js"></script>
    <link rel="stylesheet" href="wbbtheme.css" />
    <script>
        $(function () {
            $("#<%=txtContent.ClientID%>").wysibb();
            $('.deletebutton').click(function (e) {
                e.preventDefault();
                var valueToPass = $(this).attr('href');
                $.ajax({
                    url: valueToPass,
                    data: {
                        format: 'json'
                    },
                    dataType: 'json',
                    success: function (data) {
                        if (!data.isValid) {
                            alert("Fail to delete the post. Please try again later.");
                            return;
                        }
                        location.reload();
                    },
                    error: function () {
                        alert("Fail to delete the post. Please try again later.");
                    }
                });

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="p-3">
        <div class="px-3 py-2 acgpage-topic">
            <div id="acgtopictitle" runat="server" class="d-flex align-content-center"></div>
        </div>
        <div id="replylist" class="container-fluid" runat="server">
        </div>
        <asp:Panel ID="pnlreply" runat="server"></asp:Panel>
        <div class="container-fluid acgpagination-section ">
            <div class="py-2 row">
                <div id="acgpagination" runat="server"></div>
            </div>
        </div>

        <div class="container-fluid p-3 acgsection mt-3">
            <!--alert-->
            <div id="acgalert" runat="server">
            </div>
            <div class="p-4 d-flex row">
                <div class="col-12">
                    <div id="editor"></div>
                    <asp:TextBox ID="txtContent" runat="server" Rows="10" TextMode="MultiLine" Width="600" CssClass="col-12" placeholder="editor"></asp:TextBox>
                </div>
            </div>
            <div>
            </div>
            <div class="d-flex justify-content-end">
                <div id="buttonContainer">
                    <asp:Button ID="btnpost" runat="server" Text="Post Reply" OnClick="btnpost_Click"/>
                    <asp:Button ID="btnlogin" runat="server" Text="Login" OnClick="btnlogin_Click"/>
                    <asp:Button ID="btnsignup" runat="server" Text="Sign Up" OnClick="btnsignup_Click"/>
                </div>
            </div>
        </div>
    </div>
    <div id="delete-alert" style="display: none">
        <div>Deletion have fail. Please try again later.</div>
    </div>
</asp:Content>

