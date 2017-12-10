using System;
using LinqToTwitter;
using System.Linq;
using System.Collections.Generic;

public partial class Trends : System.Web.UI.Page
{
    string screenName;
    ulong userID;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private async void buscarTrends()
    {
        var context = AutenticarContext();

        try
        {
            int woeid = Int32.Parse(woeidTextBox.Text);

            var trends = await (from trend in context.Trends where trend.Type == TrendType.Place && trend.WoeID == woeid select trend).ToListAsync();

            if (trends != null && trends.Any() && trends.First().Locations != null)
            {
                string layout = "";
                trends.ForEach
                (
                    t => layout += string.Format
                    (
                        "<div class=\"media\" style=\"border: 1px solid #0099ff; border-radius: 8px; padding: 5px; margin-bottom: 10px;\">" +
                            "<div class=\"media-body\">" +
                                "<a href=\"{0}\"><h4 class=\"media-heading\" style=\"color: #006bb3;\">{1}</h4></a>" +
                                "<p style=\"color:#a6a6a6;\">{2}</p>" +
                                "<p>{3}</p>" +
                                "<a href=\"{4}\" target=\"_blank\"><p style=\"color: #000;\">Veja no Twitter</p></a>" +
                                "<p style=\"text-align: right; color: #006bb3;\">{5:HH:mm}</p>" +
                            "</div>" +
                        "</div>",
                        t.SearchUrl, t.Name, t.PromotedContent, t.Events, t.SearchUrl, t.CreatedAt
                    )
                );

                trendsResulLabel.Text = layout;
            }
        }
        catch(Exception)
        {
            Response.Redirect("Login.aspx", false);
        }
    }

    /*private string formatarLocation (List<Location> locations)
    {
        string layout = " <h4> Locais:</h4> ";

        locations.ForEach
        (
            l => string.Format
            (
                "<div class=\"media\" style=\"border: 1px solid #005c99; border-radius: 4px; padding: 5px;\">" +
                    "<div class=\"media-body\">" +
                        "<h5>{0}</h5>" +
                        "<p>{1}</p>" +
                        "<a href=\"{2}\"><p>{3}</p>" +
                "</div>",
                l.Name, l.Country, l.Url, l.WoeID
            )
        );

        return layout;
    }*/

    private TwitterContext AutenticarContext()
    {
        var auth = new AspNetAuthorizer
        {
            CredentialStore = new SessionStateCredentialStore(),
            GoToTwitterAuthorization = twitterUrl => { }
        };

        var credentials = auth.CredentialStore;
        userID = credentials.UserID;
        screenName = credentials.ScreenName;

        return new TwitterContext(auth);
    }

    protected void getTrendsButton_Click(object sender, EventArgs e)
    {
        buscarTrends();
    }
}