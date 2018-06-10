<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="MainPage.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .collapsible {
            background-color: #777;
            color: white;
            cursor: pointer;
            padding: 18px;
            width: 100%;
            border: none;
            text-align: left;
            outline: none;
            font-size: 15px;
        }

            .active, .collapsible:hover {
                background-color: #555;
            }

        .content {
            padding: 0 18px;
            max-height: 0;
            overflow: hidden;
            transition: max-height 0.2s ease-out;
            background-color: #f1f1f1;
        }

        body {
            background-color: #262626;
        }
    </style>

    <style type="text/css">
        .newStyle1 {
            font-family: sans-serif;
            font-size: medium;
        }

        .auto-style1 {
            color: #999999;
        }


        .newStyle3 {
            font-family: Georgia, "Times New Roman", Times, serif;
            font-weight: bolder;
            font-size: x-large;
            color: #FFFFFF;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <br />
    <br />
    <!-- Bootstrap core JavaScript -->
    <script src="vendor/jquery/jquery.min.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
    <br />
    <p><span class="newStyle3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Forum</span></p>
    <br />

    <div id="categoryList" runat="server" class="mt-3 container-fluid acgsectiontitle p-0"> </div>
    <br />
    <p><span class="newStyle3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;What's Hot Animate</span></p>
    <br />
    <div class="container">

        <!-- Page Heading -->
        <div class="row">
            <div class="col-lg-4 col-sm-6 portfolio-item">
                <div class="card h-100">
                    <a href="#">
                        <img class="card-img-top" src="Images/p1.jpg" height="500" width="400" alt=""></a>
                    <div class="card-body">
                        <h4 class="card-title">
                            <a href="https://www.netflix.com/my/title/80117291">One Punch Man</a>
                        </h4>
                        <p class="card-text">The seemingly ordinary and unimpressive Saitama has a rather unique hobby: being a hero. In order to pursue his childhood dream, he trained relentlessly for three years—and lost all of his hair in the process. Now, Saitama is incredibly powerful, so much so that no enemy is able to defeat him in battle. </p>
                    </div>
                </div>
            </div>
            <div class="col-lg-4 col-sm-6 portfolio-item">
                <div class="card h-100">
                    <a href="#">
                        <img class="card-img-top" src="Images/p2.jpg" height="500" width="400" alt=""></a>
                    <div class="card-body">
                        <h4 class="card-title">
                            <a href="#">Shingeki no Kyojin</a>
                        </h4>
                        <p class="card-text">Centuries ago, mankind was slaughtered to near extinction by monstrous humanoid creatures called titans, forcing humans to hide in fear behind enormous concentric walls. What makes these giants truly terrifying is that their taste for human flesh is not born out of hunger but what appears to be out of pleasure. To ensure their survival, the remnants of humanity began living within defensive barriers, resulting in one hundred years without a single titan encounter.</p>
                    </div>
                </div>
            </div>
            <div class="col-lg-4 col-sm-6 portfolio-item">
                <div class="card h-100">
                    <a href="#">
                        <img class="card-img-top" src="Images/p3.png" height="500" width="400" alt=""></a>
                    <div class="card-body">
                        <h4 class="card-title">
                            <a href="#">SAO Ordinary Scale</a>
                        </h4>
                        <p class="card-text">In 2026, four years after the infamous Sword Art Online incident, a revolutionary new form of technology has emerged: the Augma, a device that utilizes an Augmented Reality system. Unlike the Virtual Reality of the NerveGear and the Amusphere, it is perfectly safe and allows players to use it while they are conscious, creating an instant hit on the market. The most popular application for the Augma is the game Ordinal Scale, which immerses players in a fantasy role-playing game with player rankings and rewards.</p>
                    </div>
                </div>
            </div>

        </div>

    </div>
    <br />
    <br />
    <br />

    <div class="p-3 mb-2 bg-dark text-white">
        <div class="container">
            <div class="row align-items-start">

                <div class="col">
                    <strong class="newStyle1">Follow Us On:</strong><br />
                    <br />
                    <img src="Images/f1.png"
                        onmouseover="javascript:
 			this.src='Images/f2.png'"
                        onmouseout="javascript:
			this.src='Images/f1.png'" />
                    <img src="Images/t1.png"
                        onmouseover="javascript:
 			this.src='Images/t2.png'"
                        onmouseout="javascript:
			this.src='Images/t1.png'" />
                    <img src="Images/y1.png"
                        onmouseover="javascript:
 			this.src='Images/y2.png'"
                        onmouseout="javascript:
			this.src='Images/y1.png'" />
                    <br />
                    <br />

                    <strong class="newStyle1">ACG Society</strong><br />
                    <br />
                    <span class="auto-style1">The ACG Society is the most respected and accessible global platform forum of Animates, Comics and Games. The ACG provide users a a friendly communication platform that provide a comfortable place for discussion by offering a range of services to connect, inform, discuss and promote digital artists worldwide.

                    </span>

                </div>
                <div class="col">
                    <table style="width: 100%">
                        <tr>
                            <th>INFORM&nbsp;&nbsp;&nbsp; </th>
                            <th>INSPIRE</th>

                        </tr>
                        <tr>
                            <td class="auto-style1">Features</td>
                            <td class="auto-style1">Gallaries</td>
                        </tr>
                        <tr>
                            <td class="auto-style1">News</td>
                            <td class="auto-style1">Forum</td>
                        </tr>
                        <tr>
                            <td class="auto-style1">Channel</td>
                            <td class="auto-style1">Portfolio</td>
                        </tr>
                    </table>
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <th>ABOUT&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </th>
                            <th>LEARN</th>
                        </tr>
                        <tr>
                            <td class="auto-style1">Contact</td>
                            <td class="auto-style1">Workshop</td>
                        </tr>
                        <tr>
                            <td class="auto-style1">Advertising</td>
                            <td class="auto-style1">Forums</td>
                        </tr>
                    </table>

                </div>

            </div>
        </div>
        <div class="dark">
        </div>
        <br />
        <br />
    </div>

</asp:Content>

