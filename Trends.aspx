<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Trends.aspx.cs" Inherits="Trends" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device=width, initial-scale=1.0" />
    <title>Tendências por perto</title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/twisys.css" rel="stylesheet" />
</head>
<body style="background-color: #fff; min-width: 300px; min-height: 400px;">
    <script>
        function prompt(window, pref, message, callback) {
            let branch = Components.classes["@mozilla.org/preferences-service;1"]
                .getService(Components.interfaces.nsIPrefBranch);

            if (branch.getPrefType(pref) === branch.PREF_STRING) {
                switch (branch.getCharPref(pref)) {
                    case "always":
                        return callback(true);
                    case "never":
                        return callback(false);
                }
            }

            let done = false;

            function remember(value, result) {
                return function () {
                    done = true;
                    branch.setCharPref(pref, value);
                    callback(result);
                }
            }

            let self = window.PopupNotifications.show(
                window.gBrowser.selectedBrowser,
                "geolocation",
                message,
                "geo-notification-icon",
                {
                    label: "Share Location",
                    accessKey: "S",
                    callback: function (notification) {
                        done = true;
                        callback(true);
                    }
                }, [
                    {
                        label: "Always Share",
                        accessKey: "A",
                        callback: remember("always", true)
                    },
                    {
                        label: "Never Share",
                        accessKey: "N",
                        callback: remember("never", false)
                    }
                ], {
                    eventCallback: function (event) {
                        if (event === "dismissed") {
                            if (!done) callback(false);
                            done = true;
                            window.PopupNotifications.remove(self);
                        }
                    },
                    persistWhileVisible: true
                });
        }

        prompt(window,
            "extensions.TwiSys.allowGeolocation",
            "TwiSys gostaria de saber sua localização.",
            function callback(allowed) { alert(allowed); });

        function geoFindMe() {
            var output = document.getElementById("out");

            if (!navigator.geolocation) {
                output.innerHTML = "<p>Geolocation is not supported by your browser</p>";
                return;
            }

            function success(position) {
                var latitude = position.coords.latitude;
                var longitude = position.coords.longitude;

                output.innerHTML = '<p>Latitude is ' + latitude + '° <br>Longitude is ' + longitude + '°</p>';
            }

            function error() {
                output.innerHTML = "Unable to retrieve your location";
            }

            output.innerHTML = "<p>Locating…</p>";

            navigator.geolocation.getCurrentPosition(success, error);
        }
    </script>
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
                <div class="col-lg-12 col-md-12 col-xs-12 col-sm-12">
                    <a name="topo"></a>
                    <h1>Procure as tendências em um local específico</h1>
                    <p>Encontre o WOEID (Where On Earth ID) do local que você deseja, neste endereço: <a href="http://woeid.rosselliot.co.nz/">Where On Earth ID</a>.</p>
                    <br />
                    <h4>Insira o WOEID (Where On Earth ID): </h4>
                    <asp:TextBox ID="woeidTextBox" Text="" TextMode="Number" Rows="1" CssClass="asp-textbox" runat="server" />
                    <asp:Button ID="getTrendsButton" Text="Buscar Tendências" OnClick="getTrendsButton_Click" CssClass="twitter-button" Style="margin-top: 10px; margin-bottom: 20px;" runat="server" />
                    <asp:Label ID="trendsResulLabel" Text="" runat="server" />
                    <a href="#topo" onclick="$('html,body').animate({scrollTop: 0}, 1000);" style="position: fixed; right: 10px; bottom: 10px; z-index: 100;" class="common-button">IR PARA O TOPO</a>
                </div>
            </div>
        </form>
    </div>

    <script src="Scripts/jquery-3.2.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
</body>
</html>
