using LinqToTwitter;
using System;
using System.Linq;
using System.Web.UI;


public partial class _Default : System.Web.UI.Page
{
    string screenName;
    ulong userID;
    string profileURL;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            GetTweets();
            setProfileBgImg();
            refreshTwitterButton.Visible = false;
        }

        //GetRateLimits();
    }

    // Atualiza os tweets mostrados
    protected void refreshTwitterButton_Click(object sender, EventArgs e)
    {
        GetTweets();
    }

    // Botão para criar e postar um tweet
    protected async void tweetButton_Click(object sender, EventArgs e)
    {
        var context = AutenticarContext();

        try
        {
            if ((tweetFieldTextBox.Text.Length > 0 && tweetFieldTextBox.Text.Length < 140) && tweetFieldTextBox.Text != null && tweetFieldTextBox.Text != "")
            {
                var tweet = await context.TweetAsync(tweetFieldTextBox.Text);

                tweetFieldTextBox.Text = "";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displaymsg", "alert('Texto inválido!');", true);
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("Login.aspx", false);
        }
    }

    // Pega as informações de um tweet
    public async void GetTweets()
    {
        twitterLabel.Text = "";
        var context = AutenticarContext();

        try
        {
            var tweets = await (from tweet in context.Status where tweet.Type == StatusType.Home && tweet.Count == 100 select tweet).ToListAsync();
            string resultTweets = "<h2>Feed</h2>";

            foreach (var tweet in tweets)
            {
                string nome = tweet.User.Name;
                string text = FormatString(tweet.Text);
                string created = string.Format("{0:dd/MM/yyyy}", tweet.CreatedAt);
                string profileImg = tweet.User.ProfileImageUrl;
                string profileUrl = tweet.User.Url;
                ulong likes = tweet.StatusID;

                string layout = string.Format
                (
                    "<div class=\"media\" style=\"border: 1px solid #0099ff; border-radius:8px; padding: 5px;\">" +
                        "<div class=\"media-left\">" +
                            "<a href=\"{0}\"> <img class=\"media-object\" src=\"{1}\" alt=\"{2}\" style=\"border-radius: 4px;\"> </a>" +
                        "</div>" +
                        "<div class=\"media-body\">" +
                            "<a href=\"{3}\" alt=\"{4}\"><h4 class=\"media-heading\">{5}</h4></a>" +
                            "<p style=\"border: 1px solid #b3e0ff; border-radius: 4px; padding: 4px;\">{6}</p>" +
                            "<h6 style=\"color:#1ab2ff;\">{7}</h6>" +
                        "</div>" +
                    "</div>",
                    profileUrl, profileImg, nome, profileUrl, nome, nome, text, created
                );

                resultTweets += string.Format("{0}<br />", layout);
            }
            twitterLabel.Text = resultTweets;
        }
        catch (Exception)
        {
            Response.Redirect("Login.aspx", false);
        }

        twitterLabel.Text += "<br /><br />";
    }

    // Verifica o uso da API
    protected async void GetRateLimits()
    {
        var context = AutenticarContext();
        var rateLimits = await (from help in context.Help where help.Type == HelpType.RateLimits select help).SingleOrDefaultAsync();

        twitterLabel.Text = "";

        if (rateLimits != null && rateLimits.RateLimits != null)
        {
            foreach (var categoria in rateLimits.RateLimits)
            {
                twitterLabel.Text += string.Format("<br/>Categoria: {0}<br/>", categoria.Key);

                foreach (var limite in categoria.Value)
                {
                    twitterLabel.Text += string.Format("Recurso: {0} <br/>Restante: {1} <br/>Reset: {2} <br/>Limite: {3}</br>",
                        limite.Resource, limite.Remaining, limite.Reset, limite.Limit);
                }

                twitterLabel.Text += "<br/><br/>";
            }
        }
    }

    // Coloca a imagem de fundo do usuário
    private async void setProfileBgImg()
    {
        var context = AutenticarContext();
        try
        {
            var userInfo = await (from user in context.User where user.Type == UserType.Show && user.UserID == userID select user).SingleOrDefaultAsync();

            if (userInfo != null)
            {
                string profileBg = "";

                if (userInfo.ProfileBannerUrl != null)
                {
                    profileBg = string.Format
                    (
                        "<div class=\"jumbotron\" style=\"background-image: url({0}); background-size: cover\">" +
                            "<img src=\"{1}\" alt=\"profileImg\" style=\"border-radius: 4px; max-height: 60px; max-width: 60px;\" />" +
                            "<h4 style=\"color: #fff; text-shadow: 1px 1px #0099ff;\">{2}</h4>" +
                        "</div>",
                        userInfo.ProfileBannerUrl, userInfo.ProfileImageUrl, screenName
                    );
                }
                else
                {
                    profileBg = string.Format
                    (
                        "<div class=\"jumbotron\" style=\"background-color: #{0}\">" +
                            "<img src=\"{1}\" alt=\"profileImg\" style=\"border-radius: 4px; max-height: 60px; max-width: 60px;\" />" +
                            "<h4 style=\"color: #fff; text-shadow: 1px 1px #0099ff;\">{2}</h4>" +
                        "</div>",
                        userInfo.ProfileBackgroundColor, userInfo.ProfileImageUrl, screenName
                    );
                }

                wallLabel.Text = profileBg;
            }
        }
        catch (Exception)
        {
            Response.Redirect("Login.aspx", false);
        }
    }

    // Retweet um tweet
    /*protected async void RetweetStatus(ulong statusID)
    {
        var retweet = await context.RetweetAsync(statusID);

        if (retweet != null && retweet.RetweetedStatus != null && retweet.RetweetedStatus.User != null)
        {

        }
    }*/

    // Lista os seguidores do usuário
    protected async void getFollowers()
    {
        var context = AutenticarContext();

        try
        {
            var followers = await (from follower in context.Friendship
                                   where follower.Type == FriendshipType.FollowersList && follower.ScreenName == screenName
                                   select follower).SingleOrDefaultAsync();

            if (followers != null && followers.Users != null)
            {
                string layout = "<h2>Seguidores</h2>";

                followers.Users.ForEach
                (
                    follower =>

                    layout += string.Format
                    (
                        "<div style=\"border: 1px solid #0099ff; border-radius: 8px; padding: 2px; margin-bottom: 20px;\">" +
                            "<div class=\"media\" style=\"background-image: url({0}); border-radius: 6px; max-height: 350px; width: 100%; opacity: 0.85; background-size: cover; min-height: 300px;\">" +
                                "<div class=\"media-left\">" +
                                    "<img class=\"media-object\" src=\"{1}\" alt=\"profileImg\" style=\"border-radius: 4px; max-height: 50px; max-width: 50px; margin: 4px 4px; box-shadow: 0 4px 12px 0 rgba(0,77,77,0.2), 0 4px 12px 0 rgba(0,77,77,0.19);\" />" +
                                "</div>" +
                                "<div class=\"media-body\">" +
                                    "<a href=\"{2}\"><h3 class=\"media-heading\" style=\"color: #33ffff; text-shadow: 1px 1px #212121;\">{3}</h3></a>" +
                                    "<h5 class=\"media-heading\" style=\"color: #ccffff; text-shadow: 1px 1px #212121\">{4}</h5>" +
                                "</div>" +
                            "</div>" +
                            "<div style=\"padding: 4px;\">" +
                                "<table style=\"width: 100%; margin-bottom: 10px; margin-top: 6px;\">" +
                                    "<tr>" +
                                        "<td style=\"text-align: left; color: #006bb3; font: 16px Lucida Console\">Seguidores: {5}</td>" +
                                        "<td style=\"text-align: right; color: #006bb3; font: 16px Lucida Console\">Seguindo: {6} </td>" +
                                    "</tr>" +
                                "</table>" +
                                "<div style=\"text-align: center;\">" +
                                    "<div style=\"border: 1px solid #99d6ff; border-radius: 6px; padding: 4px;\">" +
                                        "<h4 style=\"color: #004d80;\">Descrição</h4>" +
                                        "<p>{7}</p>" +
                                    "</div>" +
                                    "<div style=\"border: 1px solid #99d6ff; border-radius: 6px; padding: 4px;\">" +
                                        "<h4 style=\"color: #004d80;\">Status recente</h4>" +
                                        "<p>{8}</p>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>",

                        follower.ProfileBannerUrl, follower.ProfileImageUrl, follower.Url, follower.Name, follower.Location,
                        follower.FollowersCount, follower.FriendsCount, follower.Description, FormatString(follower.Status.Text)
                    )
                );

                twitterLabel.Text = layout;
            }
        }
        catch (Exception)
        {
            Response.Redirect("Login.aspx", false);
        }
    }

    // Autentica o usuário
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

    // Formata o texto do Tweet
    private string FormatString(string s)
    {
        string[] palavras = s.Split(' ');
        string formatado = "";

        foreach (string palavra in palavras)
        {
            if (palavra.StartsWith("@") || palavra.StartsWith(">@"))
            {
                formatado += string.Format("<a href=\"#\">{0}</a> ", palavra);
            }
            else if (palavra.StartsWith("#"))
            {
                formatado += string.Format("<a href=\"#\">{0}</a> ", palavra);
            }
            else if (palavra.StartsWith("http") || palavra.StartsWith(" http") || palavra.StartsWith("https") || palavra.StartsWith(" https"))
            {
                formatado += string.Format("<a href=\"{0}\">{1}</a> ", palavra, palavra);
            }
            else if (palavra.EndsWith(".jpg") || palavra.EndsWith(".png") || palavra.EndsWith(".jpeg"))
            {
                formatado += string.Format("<img src=\"{0}\" alt=\"{1}\" \\> ", palavra, palavra);
            }
            else
            {
                formatado += palavra + " ";
            }
        }

        return formatado;
    }

    protected void showTweetsButton_Click(object sender, EventArgs e)
    {
        GetTweets();
        refreshTwitterButton.Visible = true;
    }

    protected void showFollowersButton_Click(object sender, EventArgs e)
    {
        refreshTwitterButton.Visible = false;
        getFollowers();
    }

    protected void showTweetFieldButton_Click(object sender, EventArgs e)
    {
        if (tweetButton.Visible == false)
        {
            tweetFieldTextBox.Visible = true;
            tweetButton.Visible = true;
        }
        else
        {
            tweetFieldTextBox.Visible = false;
            tweetButton.Visible = false;
        }
    }

    protected void callTrendsPageButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("Trends.aspx", false);
    }
}