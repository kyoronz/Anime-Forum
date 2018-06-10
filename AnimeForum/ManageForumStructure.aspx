<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="ManageForumStructure.aspx.cs" Inherits="ManageForumStructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #acgcategory-add, #acgboard-add, #acgcategory-edit, #acgboard-edit, #acgcategory-delete, #acgboard-delete,#acgcategory-edit-notification,#acgboard-edit-notification {
            display: none;
        }

        #acgboard-add {
            display: none;
        }
        .acgcategory-structure{
            background-color: rgba(60,60,60,0.8);
        }
        .acgboard-structure{

        }
    </style>
    <link rel="stylesheet" href="jquery-ui-1.12.1/jquery-ui.min.css" />
    <script src="jquery-ui-1.12.1/external/jquery/jquery.js"></script>
    <script src="jquery-ui-1.12.1/jquery-ui.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="background: url(https://wallup.net/wp-content/uploads/2015/12/166724-landscape-digital_art-power_lines-signs-bokeh-clouds-sunlight.jpg) no-repeat; background-size: cover; background-position-x: center; position: fixed; z-index: -1; height: 100%; width: 100%; top: 0; left: 0;"></div>
    <div class="p-3">
        <!-- Title-->
        <div class="px-3 py-2 d-flex justify-content-between align-items-center acgpage-topic">
            <div class="acgpagetitle">Manage Forum Structure</div>
        </div>
        <div class="mt-3 p-3 container-fluid acgsection">
            <asp:button id="btnAddCategory" runat="server" text="Create Category" onclientclick="return false;" />
            <asp:button id="btnAddBoard" runat="server" text="Create Board" onclientclick="return false;"/>
        </div>
        <div id="categoryList" runat="server" class="mt-3 container-fluid acgsectiontitle p-0"> </div>
    </div>
    



    <div id="error"></div>
    <!--Dialog Box-->
    <div id="acgcategory-add">
        <div>
            <span>Category Name: </span>
            <asp:textbox id="txtCategoryName" runat="server"></asp:textbox>
        </div>
        <div>
            <span>Category Description: </span>
            <asp:textbox id="txtCategoryDesc" runat="server"></asp:textbox>
        </div>
        <asp:button id="btnDialogAddCategory" runat="server" text="Create Category" onclick="btnDialogAddCategory_Click" onclientclick="" />
    </div>

    <div id="acgboard-add">
        <div>
            <span>Board Name: </span>
            <asp:textbox id="txtBoardName" runat="server"></asp:textbox>
        </div>
        <div>
            <span>Board Description: </span>
            <asp:textbox id="txtBoardDesc" runat="server"></asp:textbox>
        </div>
        <div>
            <span>Category </span>
            <asp:dropdownlist id="ddlCategory" runat="server">
            </asp:dropdownlist>
            <asp:button id="btnDialogAddBoard" runat="server" text="Create Board" onclick="btnDialogAddBoard_Click" />
        </div>
    </div>

    <div id="acgcategory-delete">
        <div>You are about to delete</div>
        <div>
            <asp:textbox id="categoryname_delete" style="font-weight: bold;" enabled="False" borderwidth="0" backcolor="White" runat="server" forecolor="#333333"></asp:textbox>
        </div>
        <div id="category-delete-details"></div>
        <div>Type the word "Delete" in the space below to verify.</div>
        <asp:textbox id="txtCategoryDelete" runat="server" cssclass="d-block"></asp:textbox>
        <asp:checkbox id="cbCategoryDelete" runat="server" cssclass="d-inline-block"/>
        <span>I acknowledge that once deleted, these items cannot be recovered in any way.</span>
        <asp:button id="btnDialogDeleteCategory" runat="server" text="Delete Category" onclientclick="return realDelete_category();" onclick="btnDialogDeleteCategory_Click" CssClass="d-block" />
    </div>

    <div id="acgboard-delete">
        <div>You are about to delete</div>
        <div>
            <asp:textbox id="boardname_delete" style="font-weight: bold;" enabled="False" borderwidth="0" backcolor="White" forecolor="#333333" runat="server"></asp:textbox>
        </div>
        <div id="board-delete-details"></div>
        <div>Type the word "Delete" in the space below to verify.</div>
        <asp:textbox id="txtBoardDelete" runat="server" cssclass="d-block"></asp:textbox>
        <asp:checkbox id="cbBoardDelete" runat="server" cssclass="d-inline-block" />
        <span>I acknowledge that once deleted, these items cannot be recovered in any way.</span>
        <asp:button id="btnDialogDeleteBoard" runat="server" text="Delete Board" cssclass="d-block" onclientclick="return realDelete_board();" onclick="btnDialogDeleteBoard_Click" />
    </div>

    <div id="acgcategory-edit">
        <div>
            <span>Original Category Name: </span>
            <asp:textbox id="txtOriginalCategoryName" runat="server" enabled="False" borderwidth="0" forecolor="#333333" backcolor="White"></asp:textbox>
        </div>
        <div>
            <span>New Category Name: </span>
            <asp:textbox id="txtNewCategoryName" runat="server"></asp:textbox>
        </div>
        <div>
            <span>Category Description: </span>
            <asp:textbox id="txtNewCategoryDesc" runat="server"></asp:textbox>
        </div>
        <asp:button id="btnDialogEditCategory" runat="server" text="Edit Category" onclientclick="return realEdit_category();" onclick="btnDialogEditCategory_Click" />
        <asp:button id="hdnCategoryEditButton" runat="server" text="" style="display: none;" clientidmode="Static" onclick="btnDialogEditCategory_Click" />
    </div>

    <div id="acgboard-edit">
        <div>
            <span>Original Board Name: </span>
            <asp:textbox id="txtOriginalBoardName" runat="server" enabled="False" borderwidth="0" forecolor="#333333" backcolor="White"></asp:textbox>
        </div>
        <div>
            <span>New Board Name: </span>
            <asp:textbox id="txtNewBoardName" runat="server"></asp:textbox>
        </div>
        <div>
            <span>New Board Description: </span>
            <asp:textbox id="txtNewBoardDesc" runat="server"></asp:textbox>
        </div>
        <div>
            <span>Category: </span>
            <asp:dropdownlist id="ddlNewCategory" runat="server">
            </asp:dropdownlist>
        </div>
        <asp:button id="btnDialogEditBoard" runat="server" text="Edit Board" onclientclick="return realEdit_board();" onclick="btnDialogEditBoard_Click" />
        <asp:button id="hdnBoardEditButton" runat="server" text="" style="display: none;" clientidmode="Static" onclick="btnDialogEditBoard_Click" />
    </div>
    <div id="acgcategory-edit-notification">
        <div><span>The <strong>Category Name </strong>has been used by others</span></div>
    </div>
    <div id="acgboard-edit-notification">
        <div><span>The <strong>Board Name </strong>has been used by others</span></div>
    </div>
    <script>
        $(function () {
            $("#acgcategory-edit-notification").dialog({
                title: "Category name have been used",
                autoOpen: false,
                modal: true
            });
            $("#acgboard-edit-notification").dialog({
                title: "Board name have been used",
                autoOpen: false,
                modal: true
            });
            $("#acgcategory-add").dialog({
                title: "Create New Category",
                autoOpen: false,
                show: {
                    effect: "blind",
                    duration: 1000
                },
                hide: {
                    effect: "explode",
                    duration: 1000
                }
            });

            $("#acgboard-add").dialog({
                title: "Create New Board",
                autoOpen: false,
                modal: true,
                show: {
                    effect: "blind",
                    duration: 1000,
                },
                hide: {
                    effect: "explode",
                    duration: 1000,
                }
            });
            $("#acgcategory-edit").dialog({
                title: "Edit Category Details",
                autoOpen: false,
                modal: true,
                show: {
                    effect: "blind",
                    duration: 1000
                },
                hide: {
                    effect: "explode",
                    duration: 1000
                }
            });

            $("#acgboard-edit").dialog({
                title: "Edit Board Details",
                autoOpen: false,
                modal: false,
                show: {
                    effect: "blind",
                    duration: 1000,
                },
                hide: {
                    effect: "explode",
                    duration: 1000,
                }
            });
            $("#acgcategory-delete").dialog({
                title: "Delete Category",
                autoOpen: false,
                modal: true,
                show: {
                    effect: "blind",
                    duration: 1000
                },
                hide: {
                    effect: "explode",
                    duration: 1000
                }
            });

            $("#acgboard-delete").dialog({
                title: "Delete Board",
                autoOpen: false,
                modal: true,
                show: {
                    effect: "blind",
                    duration: 1000,
                },
                hide: {
                    effect: "explode",
                    duration: 1000,
                }
            });

            $("#<%=btnAddCategory.ClientID%>").on("click", function () {
                $("#acgcategory-add").dialog("open");
            });
            $("#<%=btnAddBoard.ClientID%>").on("click", function () {
                $("#acgboard-add").dialog("open");
            });
            $("#acgcategory-add").parent().appendTo(jQuery("form:first"));
            $("#acgboard-add").parent().appendTo(jQuery("form:first"));
            $("#acgcategory-edit").parent().appendTo(jQuery("form:first"));
            $("#acgboard-edit").parent().appendTo(jQuery("form:first"));
            $("#acgcategory-delete").parent().appendTo(jQuery("form:first"));
            $("#acgboard-delete").parent().appendTo(jQuery("form:first"));

            for (var i = 0; i < 2; i++) {
                deleteCookie("txtOriginalCategoryName");
                deleteCookie("txtOriginalBoardName");
                deleteCookie("categoryname_delete");
                deleteCookie("boardname_delete");
            }


        });
        function editClick(dialogType, ID) {
            if (dialogType == "1") {
                $.ajax({
                    url: 'StructureModification.ashx?id=' + encodeURIComponent(ID) + '&type=' + dialogType,
                    data: {
                        format: 'json'
                    },/*
                    error: function () {
                        $('#info').html('<p>An error has occurred</p>');
                    },*/
                    dataType: 'json',
                    success: function (data) {
                        document.getElementById("<%=txtOriginalCategoryName.ClientID%>").value = data.categoryName;
                        document.getElementById("<%=txtNewCategoryName.ClientID%>").value = data.categoryName;
                        document.getElementById("<%=txtNewCategoryDesc.ClientID%>").value = data.categoryDesc == undefined ? "" : data.categoryDesc;
                        $("#acgcategory-edit").dialog("open");
                    },
                    type: 'GET'
                });
            }
            if (dialogType == "2") {
                $.ajax({
                    url: 'StructureModification.ashx?id=' + encodeURIComponent(ID) + '&type=' + dialogType,
                    data: {
                        format: 'json'
                    },/*
                    error: function () {
                        $('#info').html('<p>An error has occurred</p>');
                    },*/
                    dataType: 'json',
                    success: function (data) {
                        document.getElementById("<%=txtOriginalBoardName.ClientID%>").value = data.boardName;
                        document.getElementById("<%=txtNewBoardName.ClientID%>").value = data.boardName;
                        document.getElementById("<%=txtNewBoardDesc.ClientID%>").value = data.boardDesc == undefined ? "" : data.boardDesc;
                        document.getElementById("<%=ddlNewCategory.ClientID%>").value = data.categoryName;
                        $("#acgboard-edit").dialog("open");                        
                    },
                    type: 'GET'
                });
            }
            return false;
        }
        //function getCookie(cname) {
        //    var name = cname + "=";
        //    var ca = document.cookie.split(';');
        //    for (var i = 0; i < ca.length; i++) {
        //        var c = ca[i];
        //        while (c.charAt(0) == ' ') c = c.substring(1);
        //        if (c.indexOf(name) == 0) {
        //            return c.substring(name.length, c.length);
        //        }
        //    }
        //    return "";
        //}
        function getCookie(name) {
            function escape(s) { return s.replace(/([.*+?\^${}()|\[\]\/\\])/g, '\\$1'); };
            var match = document.cookie.match(RegExp('(?:^|;\\s*)' + escape(name) + '=([^;]*)'));
            return match ? match[1] : null;
        }
        function setCookie(name, value) {
            var d = new Date();
            d.setTime(d.getTime() + (60 * 60 * 1000)); //one hour expire time
            var expires = "expires=" + d.toUTCString();
            document.cookie = name + "=" + value + "; " + expires;
        }
        function deleteCookie(name) {
            setCookie(name, "", -1);
        }

        function deleteClick(dialogType, ID) {
            if (dialogType == "1") {
                var xmlhttp = new XMLHttpRequest();
                xmlhttp.onreadystatechange = function () {
                    if (this.readyState === this.DONE) {
                        // do something; the request has completed                        
                        document.getElementById("<%=categoryname_delete.ClientID%>").value = ID;
                        document.getElementById("category-delete-details").innerHTML = xmlhttp.responseText;
                        $("#acgcategory-delete").dialog("open");
                    }
                }
                xmlhttp.open("GET", ("StructureDeletion.ashx?id=" + encodeURIComponent(ID) + "&type=" + dialogType));
                xmlhttp.send();
            }
            if (dialogType == "2") {
                var xmlhttp = new XMLHttpRequest();
                xmlhttp.onreadystatechange = function () {
                    if (this.readyState === this.DONE) {
                        // do something; the request has completed
                        document.getElementById("<%=boardname_delete.ClientID%>").value = ID;
                        document.getElementById("board-delete-details").innerHTML = xmlhttp.responseText;
                        $("#acgboard-delete").dialog("open");
                    }
                }
                xmlhttp.open("GET", ("StructureDeletion.ashx?id=" + encodeURIComponent(ID) + "&type=" + dialogType));
                xmlhttp.send();
            }
            return false;
        }
        function realDelete_category() {
            if (document.getElementById("<%=txtCategoryDelete.ClientID%>").value && document.getElementById("<%=cbCategoryDelete.ClientID%>").checked) {
                if (getCookie("categoryname_delete") != null) {
                    deleteCookie("categoryname_delete");
                }
                setCookie("categoryname_delete", document.getElementById("<%=categoryname_delete.ClientID%>").value);
                return true;
            }
            return false;
        }
        function realDelete_board() {
            if (document.getElementById("<%=txtBoardDelete.ClientID%>").value && document.getElementById("<%=cbBoardDelete.ClientID%>").checked) {
                if (getCookie("boardname_delete") != null) {
                    deleteCookie("boardname_delete");
                }
                setCookie("boardname_delete", document.getElementById("<%=boardname_delete.ClientID%>").value);
                return true;
            }
            return false;
        }
        function realEdit_category() {
            if (document.getElementById("<%=txtOriginalCategoryName.ClientID%>").value === document.getElementById("<%=txtNewCategoryName.ClientID%>").value) {
                if (getCookie("txtOriginalCategoryName") != null) {
                    deleteCookie("txtOriginalCategoryName");
                }
                setCookie("txtOriginalCategoryName", document.getElementById("<%=txtOriginalCategoryName.ClientID%>").value);
                $("#acgcategory-delete").dialog("close");
                $('#<%=hdnCategoryEditButton.ClientID%>').click();
            }
            else {
                $.ajax({
                    url: 'StructureModificationValid.ashx?id=' + encodeURIComponent(<%=txtNewCategoryName.ClientID%>.value) + '&type=1',
                    data: {
                        format: 'json'
                    },
                    dataType: 'json',
                    success: function (data) {
                        if (!data.isValid) {
                            $("#acgcategory-edit-notification").dialog("open");
                            return;
                        }
                        else {
                            if (getCookie("txtOriginalCategoryName") != null) {
                                deleteCookie("txtOriginalCategoryName");
                            }
                            setCookie("txtOriginalCategoryName", document.getElementById("<%=txtOriginalCategoryName.ClientID%>").value);
                            $("#acgcategory-delete").dialog("close");
                            $('#<%=hdnCategoryEditButton.ClientID%>').click();
                        }
                    },
                    type: 'GET'
                });

            }
            return false;
        }
        function realEdit_board() {
            if (document.getElementById("<%=txtOriginalBoardName.ClientID%>").value === document.getElementById("<%=txtNewBoardName.ClientID%>").value) {
                if (getCookie("txtOriginalBoardName") != null) {
                    deleteCookie("txtOriginalBoardName");
                }
                setCookie("txtOriginalBoardName", document.getElementById("<%=txtOriginalBoardName.ClientID%>").value);
                $("#acgboard-delete").dialog("close");
                $('#<%=hdnBoardEditButton.ClientID%>').click();
            }
            else {
                $.ajax({
                    url: 'StructureModificationValid.ashx?id=' + encodeURIComponent(<%=txtNewBoardName.ClientID%>.value) + '&type=2',
                    data: {
                        format: 'json'
                    },
                    dataType: 'json',
                    success: function (data) {
                        if (!data.isValid) {
                            $("#acgboard-edit-notification").dialog("open");
                            return;
                        }
                        else {
                            if (getCookie("txtOriginalBoardName") != null) {
                                deleteCookie("txtOriginalBoardName");
                            }
                            setCookie("txtOriginalBoardName", document.getElementById("<%=txtOriginalBoardName.ClientID%>").value);
                            $("#acgboard-delete").dialog("close");
                            $('#<%=hdnBoardEditButton.ClientID%>').click();
                        }
                    },
                    type: 'GET'
                });
            }
            return false;
        }
    </script>
</asp:Content>

