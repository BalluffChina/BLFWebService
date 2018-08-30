using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.Services;
using System.Text;
using System.Web;
using System.Configuration;
using Microsoft.SharePoint.Utilities;

namespace I.BLF.WebServices.Layouts.I.BLF.WebServices
{
    public partial class QueryMaterials : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static string GetMaterials(string keyword)
        {
            StringBuilder materials = new StringBuilder();
            string queryMaterials = string.Empty;
            try
            {
                string siteUrl = ConfigurationManager.AppSettings["crmPriceApprovalUrl"].ToString();
                using (SPSite site = new SPSite(siteUrl))
                using (SPWeb web = site.OpenWeb())
                {
                    SPList listMaterials = web.Lists["Materials"];
                    SPQuery query = new SPQuery();
                    string strQuery = @"<Where>
										<Contains>
											<FieldRef Name='Title' />
											<Value Type='Text'>{0}</Value>
										</Contains>
									</Where>";
                    query.RowLimit = 10;
                    query.Query = string.Format(strQuery, keyword);
                    SPListItemCollection colMaterials = listMaterials.GetItems(query);
                    if (colMaterials != null && colMaterials.Count > 0)
                    {
                        queryMaterials = colMaterials.Xml;
                    }

                }
            }
            catch(Exception ex)
            {
                queryMaterials = ex.ToString();
            }
            return queryMaterials;
        }
    }
}
