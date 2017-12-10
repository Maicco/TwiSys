using System;
using System.Web.UI;
using LinqToTwitter;

public partial class Login : System.Web.UI.Page
{
    AspNetAuthorizer auth; // Contém as informações de autenticação devolvidas pelo Twitter
    protected async void Page_Load(object sender, EventArgs e)
    {
        string consumerKey = "GJqLExnoDcV8WTatLKbvrSTNF";
        string consumerSecret = "igKdMRcIYe478XXU2ourxz9Axr9BmqkqHHtsStai66wr1N6kL7";

        auth = new AspNetAuthorizer
        {
            CredentialStore = new SessionStateCredentialStore
            {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret
            },

            GoToTwitterAuthorization = twitterUrl => Response.Redirect(twitterUrl, false)
        };

        if (!Page.IsPostBack && Request.QueryString["oauth_token"] != null)
        {
            await auth.CompleteAuthorizeAsync(Request.Url);

            var credentials = auth.CredentialStore;
            string oauthToken = credentials.OAuthToken;
            string oauthTokenSecret = credentials.OAuthTokenSecret;
            string screenName = credentials.ScreenName;
            ulong userID = credentials.UserID;

            Response.Redirect("Default.aspx", false);
        }
    }

    // Botão para autenticar o Twitter
    protected async void autenticarButton_Click(object sender, EventArgs e)
    {
        await auth.BeginAuthorizeAsync(Request.Url);
    }
}