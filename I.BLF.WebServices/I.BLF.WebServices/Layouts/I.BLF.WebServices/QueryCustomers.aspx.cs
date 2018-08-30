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
    public partial class QueryCustomers : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //GetCustomers("公司");
            //string customer = ("Acct_1_XXX");
            //GetInteractions(customer);
        }

        [WebMethod]
        public static string GetCustomers(string keyword)
        {
            StringBuilder customers = new StringBuilder();
            string queryCustomers = string.Empty;
            string siteUrl = ConfigurationManager.AppSettings["crmUrl"].ToString();
            using (SPSite site = new SPSite(siteUrl))
            using (SPWeb web = site.OpenWeb())
            {
                SPList listAccounts = web.Lists["Accounts"];
                SPQuery query = new SPQuery();
                string strQuery = @"<Where>
										<Contains>
											<FieldRef Name='Title' />
											<Value Type='Text'>{0}</Value>
										</Contains>
									</Where>";
                query.Query = string.Format(strQuery, keyword);
                SPListItemCollection colAccounts = listAccounts.GetItems(query);
                if (colAccounts != null && colAccounts.Count > 0)
                {
                    foreach (SPListItem item in colAccounts)
                    {
                        customers.Append("Acct_" + item.ID + "_" + item.Title + "##@@##");
                    }
                }

                SPList listLeads = web.Lists["Leads"];
                SPListItemCollection colLeads = listLeads.GetItems(query);
                if (colLeads != null && colLeads.Count > 0)
                {
                    foreach (SPListItem item in colLeads)
                    {
                        customers.Append("Lead_" + item.ID + "_" + item.Title + "##@@##");
                    }
                }
                //delete last "##@@##"
                queryCustomers = customers.ToString();
                if (queryCustomers.Length > 0)
                {
                    queryCustomers = queryCustomers.Substring(0, queryCustomers.Length - 6);
                }

            }
            return queryCustomers;
        }

        [WebMethod]
        public static string GetInteractions(string customer)
        {
            StringBuilder interactions = new StringBuilder();
            string strInteractions = string.Empty;
            string type = customer.Split('_')[0];
            string id = customer.Split('_')[1];
            string queryInteractions = string.Empty;
            string siteUrl = ConfigurationManager.AppSettings["crmUrl"].ToString();
            string interactionDays = ConfigurationManager.AppSettings["interactionDays"].ToString();
            using (SPSite site = new SPSite(siteUrl))
            using (SPWeb web = site.OpenWeb())
            {
                SPList list = web.Lists["Interactions"];
                SPQuery query = new SPQuery();
                DateTime CustomDate = DateTime.Now;
                if (type.Equals("Acct"))
                {
                    query.Query = string.Format(@"<Where>
                                                <And>
                                                  <And>
                                                     <Eq>
                                                        <FieldRef Name='CompanyID' />
                                                        <Value Type='Number'>{0}</Value>
                                                     </Eq>
                                                     <Contains>
                                                        <FieldRef Name='Made_x0020_By' />
                                                        <Value Type='UserMulti'>{1}</Value>
                                                     </Contains>
                                                  </And>
                                                    <Geq>
                                                        <FieldRef Name='Created' />
                                                        <Value Type='DateTime'><Today OffsetDays='-{2}'/></Value>
                                                    </Geq>
                                                </And>
                                               </Where>", id, SPContext.Current.Web.CurrentUser.Name, interactionDays);
                }
                else
                {
                    query.Query = string.Format(@"<Where>
                                                    <And>
                                                      <And>
                                                         <And>
                                                            <Eq>
                                                               <FieldRef Name='OwnerID' />
                                                               <Value Type='Number'>{0}</Value>
                                                            </Eq>
                                                            <Eq>
                                                               <FieldRef Name='OwnerModule' />
                                                               <Value Type='Text'>{1}</Value>
                                                            </Eq>
                                                         </And>
                                                         <Contains>
                                                            <FieldRef Name='Made_x0020_By' />
                                                            <Value Type='UserMulti'>{2}</Value>
                                                         </Contains>
                                                      </And>
                                                     <Geq>
                                                        <FieldRef Name='Created' />
                                                        <Value Type='DateTime'><Today OffsetDays='-{3}'/></Value>
                                                    </Geq>
                                                </And>
                                                   </Where>", id, "Leads", SPContext.Current.Web.CurrentUser.Name, interactionDays);
                }

                SPListItemCollection col = list.GetItems(query);
                if (col != null && col.Count > 0)
                {
                    foreach (SPListItem item in col)
                    {
                        string interactionDate = string.Format("{0:M/d/yyyy}", DateTime.Parse(item["Interaction Date"].ToString()));
                        string summary = item["Summary"].ToString();
                        interactions.Append(item.ID + "_" + interactionDate + "_" + summary + "##@@##");
                    }
                }

                //delete last "##@@##"
                strInteractions = interactions.ToString();
                if (strInteractions.Length > 0)
                {
                    strInteractions = strInteractions.Substring(0, strInteractions.Length - 6);
                }

                return strInteractions;

            }
        }

        [WebMethod]
        public static string GetCarAllowanceInteractions(string strStartDate, string strEndDate)
        {
            StringBuilder interactions = new StringBuilder();
            string strInteractions = string.Empty;
            string queryInteractions = string.Empty;
            DateTime startDate = DateTime.Parse(strStartDate);
            DateTime endDate = DateTime.Parse(strEndDate);
            string siteUrl = ConfigurationManager.AppSettings["crmUrl"].ToString();
            using (SPSite site = new SPSite(siteUrl))
            using (SPWeb web = site.OpenWeb())
            {
                SPList list = web.Lists["Interactions"];
                SPQuery query = new SPQuery();

                query.Query = string.Format(@"<Where>
                                            <And>
                                                <And>
                                                    <Leq>
                                                        <FieldRef Name='Created' />
                                                        <Value Type='DateTime' IncludeTimeValue='FALSE'>{0}</Value>
                                                    </Leq>
                                                    <Geq>
                                                        <FieldRef Name='Created' />
                                                        <Value Type='DateTime' IncludeTimeValue='FALSE'>{1}</Value>
                                                    </Geq>
                                                </And>
                                                    <Contains>
                                                        <FieldRef Name='Made_x0020_By' />
                                                        <Value Type='UserMulti'>{2}</Value>
                                                    </Contains>
                                            </And>
                                            </Where>", SPUtility.CreateISO8601DateTimeFromSystemDateTime(endDate), 
                                                     SPUtility.CreateISO8601DateTimeFromSystemDateTime(startDate), 
                                                     SPContext.Current.Web.CurrentUser.Name);
                SPListItemCollection col = list.GetItems(query);
                double mileage = 0;
                if (col != null && col.Count > 0)
                {
                    foreach (SPListItem item in col)
                    {
                        mileage += item["Mileage"] == null ? 0 : (double)item["Mileage"];
                        
                    }
                }

                ////delete last "##@@##"
                //strInteractions = interactions.ToString();
                //if (strInteractions.Length > 0)
                //{
                //    strInteractions = strInteractions.Substring(0, strInteractions.Length - 6);
                //}

                return mileage.ToString();
            }
        }


    }
}
