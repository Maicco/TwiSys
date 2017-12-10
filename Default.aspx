<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device=width, initial-scale=1.0" />
    <title>TwiSys</title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/twisys.css" rel="stylesheet" />
</head>
<body style="background-color: #fff; min-width: 300px; min-height: 400px;">
    <!-- Fixed navbar -->
    <nav class="navbar navbar-default navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="Default.aspx">TwiSys</a>
            </div>
            <div id="navbar" class="navbar-collapse collapse">
                <ul class="nav navbar-nav navbar-right">
                    <li><a href="https://twitter.com/">Twitter</a></li>
                </ul>
            </div>
            <!--/.nav-collapse -->
        </div>
    </nav>
    <div style="margin-top: 50px;">
        <form id="form1" runat="server">
            <div class="container">
                <a name="topo"></a>
                <div class="col-lg-9 col-md-9 col-sm-7 col-xs-7" style="background-color: #f4f4f4">
                    <h2>Bem-vindo!</h2>
                    <asp:Label ID="wallLabel" Text="" runat="server" />
                    <asp:TextBox ID="tweetFieldTextBox" Width="100%" BorderWidth="2px" BorderColor="#005580" Height="100px" MaxLength="140" TextMode="MultiLine" Style="resize: vertical; padding: 4px 8px 4px 8px; margin-bottom: 20px;" Font-Size="Medium" BorderStyle="Solid" Visible="false" runat="server" />
                    <asp:Button ID="tweetButton" Text="ENVIAR TWEET" Style="margin-bottom: 10px;" OnClick="tweetButton_Click" Font-Bold="true" CssClass="twitter-button" Visible="false" runat="server" />
                    <br />
                    <br />
                    <asp:Label ID="twitterLabel" Text="" runat="server" />
                    <asp:Button ID="refreshTwitterButton" Text="ATUALIZAR" CssClass="twitter-button" Font-Bold="true" OnClick="refreshTwitterButton_Click" Style="margin-bottom: 20px;" runat="server" />
                    <a href="#topo" onclick="$('html,body').animate({scrollTop: 0}, 1000);" style="position: fixed; right: 10px; bottom: 10px; z-index: 100;" class="common-button">IR PARA O TOPO</a>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-5 col-xs-5">
                    <div style="position: fixed;">
                        <h2 style="text-align: center;">Funções</h2>
                        <asp:Button ID="showTweetsButton" Text="Mostrar Feed" OnClick="showTweetsButton_Click" CssClass="twitter-button" Style="margin-bottom: 10px;" runat="server" />
                        <asp:Button ID="showFollowersButton" Text="Mostrar Seguidores" OnClick="showFollowersButton_Click" CssClass="twitter-button" Style="margin-bottom: 10px;" runat="server" />
                        <asp:Button ID="showTweetFieldButton" Text="Tweetar" OnClick="showTweetFieldButton_Click" CssClass="twitter-button" Style="margin-bottom: 10px;" runat="server" />
                        <asp:Button ID="callTrendsPageButton" Text="Trends" OnClick="callTrendsPageButton_Click" CssClass="twitter-button" Style="margin-bottom: 10px;" runat="server" />
                    </div>
                </div>
            </div>
        </form>
    </div>
    <script src="Scripts/jquery-3.2.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
</body>
</html>
