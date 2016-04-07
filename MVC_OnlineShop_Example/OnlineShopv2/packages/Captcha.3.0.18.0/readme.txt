The BotDetect ASP.NET CAPTCHA NuGet package has been added to your project.

    1. Captcha integration instructions
    2. ASP.NET Captcha code samples download
    3. BotDetect Captcha documentation


-------------------------------------------------------------------------------
 1. CAPTCHA INTEGRATION INSTRUCTIONS
-------------------------------------------------------------------------------

To show a Captcha challenge on the form and check user input during form submission:

ASP.NET MVC
-------------------------------------------------------------------------------
In View code: import the BotDetect namespace, include BotDetect styles in page <head>, create a MvcCaptcha object and pass it to the Captcha HtmlHelper:

        @using BotDetect.Web.UI.Mvc;

          [...]

          <link href="@BotDetect.Web.CaptchaUrls.Absolute.LayoutStyleSheetUrl" rel="stylesheet" type="text/css" />
        </head>

          [...]

        @{ MvcCaptcha sampleCaptcha = new MvcCaptcha("SampleCaptcha"); }
        @Html.Captcha(sampleCaptcha)
        @Html.TextBox("CaptchaCode")

Exclude BotDetect paths from ASP.NET MVC Routing:

    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        // BotDetect requests must not be routed
        routes.IgnoreRoute("{*botdetect}",  new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

Mark the protected Controller action with the CaptchaValidation attribute to include Captcha validation in ModelState.IsValid checks:

    using BotDetect.Web.UI.Mvc;

    [HttpPost]
    [AllowAnonymous]
    [CaptchaValidation("CaptchaCode", "SampleCaptcha", "Incorrect CAPTCHA code!")]
    public ActionResult SampleAction(SampleModel model)
    {
        if (!ModelState.IsValid)
        {
            // TODO: Captcha validation failed, show error message
        }
        else
        {
            // TODO: Captcha validation passed, proceed with protected action
        }

Detailed ASP.NET MVC integration instructions can be found at http://captcha.com/doc/aspnet/howto/asp.net-mvc-captcha.html.


ASP.NET WebForms
-------------------------------------------------------------------------------
On the ASP.NET form you want to protect against bots, add:

    <BotDetect:Captcha ID="SampleCaptcha" runat="server" />
    <asp:TextBox ID="CaptchaCodeTextBox" runat="server" />

When the form is submitted, the Captcha validation result must be checked:

    if (IsPostBack)
    {
        // validate the Captcha to check we're not dealing with a bot
        bool isHuman = SampleCaptcha.Validate(CaptchaCodeTextBox.Text);

        CaptchaCodeTextBox.Text = null; // clear previous user input

        if (!isHuman)
        {
            // TODO: Captcha validation failed, show error message
        }
        else
        {
            // TODO: Captcha validation passed, proceed with protected action
        }
    }

Detailed ASP.NET WebForms integration instructions can be found at http://captcha.com/doc/aspnet/howto/asp.net-webforms-captcha.html.


-------------------------------------------------------------------------------
 2. ASP.NET CAPTCHA CODE SAMPLES DOWNLOAD
-------------------------------------------------------------------------------

CAPTCHA code samples (ASP.NET WebForms integration samples, ASP.NET MVC integration samples, BotDetect configuration samples - with both C# and VB.NET and ASPX/Razor variants), as well as  equivalents for older .NET framework versions and additional resources are included in the BotDetect setup package which can be downloaded from:

http://captcha.com/captcha-download.html?version=aspnet.


-------------------------------------------------------------------------------
 3. BOTDETECT CAPTCHA DOCUMENTATION
-------------------------------------------------------------------------------

The full index of available BotDetect ASP.NET CAPTCHA documentation can be found at:

http://captcha.com/documentation.html#aspnet_doc


