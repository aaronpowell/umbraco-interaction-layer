using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Umbraco.InteractionLayer.Web.dashboard
{
    public partial class CodeGen : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var tmp = new umbraco.cms.businesslogic.datatype.controls.Factory();

            if (!IsPostBack)
            {
#if v4
                this.rptDocTypes.DataSource = umbraco.cms.businesslogic.web.DocumentType.GetAllAsList(); 
#endif
#if v3
                this.rptDocTypes.DataSource = umbraco.cms.businesslogic.web.DocumentType.GetAll; 
#endif
                this.rptDocTypes.DataBind();
            }
        }

        protected void GenerateClasses(object sender, EventArgs e)
        {
            this.Page.Validate("ns");
            if (!this.Page.IsValid) return;

            List<int> ids = new List<int>();

            foreach (RepeaterItem item in this.rptDocTypes.Items)
            {
                if (((CheckBox)item.FindControl("docType")).Checked)
                {
                    ids.Add(int.Parse(((HiddenField)item.FindControl("docTypeId")).Value));
                }
            }
            CodeBuilder.CodeCreator cc = new Umbraco.InteractionLayer.CodeBuilder.CodeCreator();
            cc.DocTypesToGen = ids.ToArray();
            if (this.languageType.SelectedValue == "VB")
            {
                cc.GenerationLanaguage = Umbraco.InteractionLayer.CodeBuilder.Language.VB;
            }
            else
            {
                cc.GenerationLanaguage = Umbraco.InteractionLayer.CodeBuilder.Language.CSharp;
            }

            cc.Namespace = this.generateNamespace.Text;

            cc.GetDocTypes();

            Page.ClientScript.RegisterStartupScript(this.GetType(), "saved", "top.umbSpeechBubble('info','Umbraco Interaction Layer','Code generation complete.');", true);
        }
    }
}