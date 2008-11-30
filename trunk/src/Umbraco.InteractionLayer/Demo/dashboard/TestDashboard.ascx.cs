using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Umbraco.Generated;
using System.Web.UI.HtmlControls;
using System.Text;
using umbraco.cms.businesslogic.web;

namespace Umbraco.InteractionLayer.Demo.dashboard
{
    public partial class TestDashboard : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            runTextTests.Click += new EventHandler(runTextTests_Click);
        }

        void runTextTests_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            Folder testRun = new Folder();
            testRun.Text = DateTime.Now.ToString();
            testRun.ParentNodeId = int.Parse(ConfigurationManager.AppSettings["TestRootId"]);
            testRun.Save();

            sb.Append("Created a new folder for the tests - " + testRun.Id + "<br />");

            sb.Append("<ul>");
            foreach (var item in Enumerable.Range(0, 100))
            {
                TextPage page = new TextPage()
                {
                    ParentNodeId = testRun.Id,
                    Text = "Text page - " + item,
                    PageHeader = "Text page - " + item,
                    PageTitle = "Text page - " + item
                };

                page.Save();

                sb.Append("<li>Created page: \"" + page.Text + "\" with id: " + page.Id + "</li>");
            }

            sb.Append("</ul>");

            TextPage invalidPage = new TextPage();
            invalidPage.Text = "This page doesn't meet mandatory requirements";
            invalidPage.ParentNodeId = testRun.Id;

            try
            {
                invalidPage.Save();
            }
            catch (ArgumentException ex)
            {
                sb.Append("Wasn't able to completely save this document, it didn't meet the mandatory requirements - " + ex.Message + "<br />");
            }

            try
            {
                ContactPage contactPage = new ContactPage()
                {
                    EmailTo = "not a valid email address!"
                };
            }
            catch (InvalidCastException ex)
            {
                sb.Append("Well that email address wasn't valid for a contact page!<br />");
                sb.Append(ex.Message + "<br />");
            }

            testRun.Publish();
            sb.Append("Just published the test run folder<br />");

            TextPage reopenedPage = new TextPage(testRun.Id + new Random().Next(1, 50));
            reopenedPage.PageTitle += ". Reopened!";
            reopenedPage.bodyText = "The page was reopened and the body now contains stuff!";
            reopenedPage.Save();
            reopenedPage.Publish();

            sb.Append("Page id " + reopenedPage.Id + " was reopened and has now been updated and published!<br />");

            TextPage runResults = new TextPage();
            runResults.ParentNodeId = testRun.Id;
            runResults.PageTitle = runResults.PageHeader = runResults.Text = "Test run results";
            runResults.bodyText = sb.ToString();
            runResults.Save();
            runResults.Publish();

            sb.Append("Test run results have been published into page ID: " + runResults.Id);

            var pages = testRun.TextPages.Take(10);
            foreach (var item in pages)
            {
                item.Description = item.Text;
            }

            sb.Append("There have been " + testRun.TextPages.Where(t => t.IsDirty).Count() + " unsaved changes");

            this.textTestResults.Controls.Add(new LiteralControl(sb.ToString()));

        }
    }
}