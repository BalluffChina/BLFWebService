using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace I.BLF.WorkflowCenter.WorkflowCenter
{
    [ToolboxItemAttribute(false)]
    public partial class WorkflowCenter : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        string formatHtml = "<li><a href='{0}'>{1}</a><a href='{2}' style='color:red'>{3}</a>";

        public string FIHtml;
        public string AdminHtml;
        public string SCMHtml;
        public string HRHtml;
        public string MOHtml;

        public WorkflowCenter()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //QueryFinance();
                QueryIT();
                QueryWorkflows();

            }
            catch (Exception ex)
            {
                errorMsg.Value = ex.ToString();
            }
        }
        //public void QueryFinance()
        //{
            
        //    //Guid id = new Guid("{1034991e-1489-4a16-991a-388273a3dd2a}");
        //    //string ss = "http://i.balluff/";
        //    //// Http://i.balluff/FI/BP
        //    SPUserToken userToken = SPContext.Current.Web.CurrentUser.UserToken;
        //    string rootUrl = SPContext.Current.Site.RootWeb.Url;
        //    //Bankpayment
        //    using (SPSite site = new SPSite(rootUrl + "/FI/BP", userToken))
        //    using (SPWeb web = site.OpenWeb())
        //    {
        //        BPLink.HRef = rootUrl + "/FI/BP/SitePages/Home.aspx";
        //        SPListItemCollection col = GetCollection(web);
        //        if (col != null && col.Count > 0)
        //        {
        //            BPTasks.Text = "(" + col.Count + ")";
        //            BPTasks.NavigateUrl = rootUrl + "/FI/BP/WorkflowTasks/MyItems.aspx";
        //        }

        //    }

        //    //Custom Credit Request
        //    using (SPSite site = new SPSite(rootUrl + "/FI/CCAACF", userToken))
        //    using (SPWeb web = site.OpenWeb())
        //    {
        //        CCRLink.HRef = rootUrl + "/FI/CCAACF/SitePages/Home.aspx";
        //        SPListItemCollection col = GetCollection(web);
        //        if (col != null && col.Count > 0)
        //        {
        //            CCRTasks.Text = "(" + col.Count + ")";
        //            CCRTasks.NavigateUrl = rootUrl + "/FI/CCAACF/WorkflowTasks/MyItems.aspx";
        //        }

        //    }

            
        //    //Reimbursement
        //    using (SPSite site = new SPSite(rootUrl + "/FI/Reimbursement", userToken))
        //    using (SPWeb web = site.OpenWeb())
        //    {
        //        RRLink.HRef = rootUrl + "/FI/Reimbursement/SitePages/Home.aspx";
        //        SPListItemCollection col = GetCollection(web);
        //        if (col != null && col.Count > 0)
        //        {
        //            RRTasks.Text = "(" + col.Count + ")";
        //            RRTasks.NavigateUrl = rootUrl + "/FI/Reimbursement/WorkflowTasks/MyItems.aspx";
        //        }

        //    }
        //}
        public void QueryIT()
        {

            //Guid id = new Guid("{1034991e-1489-4a16-991a-388273a3dd2a}");
            //string ss = "http://i.balluff/";
            //// Http://i.balluff/FI/BP
            SPUserToken userToken = SPContext.Current.Web.CurrentUser.UserToken;
            string rootUrl = SPContext.Current.Site.RootWeb.Url;
            //Bankpayment
            using (SPSite site = new SPSite(rootUrl + "/IT/ISTS", userToken))
            using (SPWeb web = site.OpenWeb())
            {
                ITSTLink.HRef = rootUrl + "/IT/ISTS/SitePages/Home.aspx";
                SPListItemCollection col = GetCollectionIT(web);
                if (col != null && col.Count > 0)
                {
                    ITSTTasks.Text = "(" + col.Count + ")";
                    ITSTTasks.NavigateUrl = rootUrl + "/IT/ISTS/Lists/IT%20Service%20Requests/MyItems.aspx";
                }
            }
        }

        SPListItemCollection GetCollection(SPWeb web)
        {
            SPList oList = web.Lists["Workflow Tasks"];
            SPQuery oQuery = new SPQuery();
            string strQuery = @"<Where>
                                      <And>
                                         <Eq>
                                            <FieldRef Name='AssignedTo' />
                                            <Value Type='User'>{0}</Value>
                                         </Eq>
                                         <Neq>
                                            <FieldRef Name='Status' />
                                            <Value Type='Choice'>Completed</Value>
                                         </Neq>
                                      </And>
                                   </Where>";
            oQuery.Query = string.Format(strQuery, SPContext.Current.Web.CurrentUser.Name);
            SPListItemCollection col = oList.GetItems(oQuery);
            return col;
        }

        SPListItemCollection GetCollectionIT(SPWeb web)
        {
            SPList oList = web.Lists["IT Service Requests"];
            SPQuery oQuery = new SPQuery();
            string strQuery = @"<Where>
                                      <And>
                                         <Eq>
                                            <FieldRef Name='AssignedTo' />
                                            <Value Type='User'>{0}</Value>
                                         </Eq>
                                         <Eq>
                                            <FieldRef Name='Status' />
                                            <Value Type='Choice'>In progress</Value>
                                         </Eq>
                                      </And>
                                   </Where>";
            oQuery.Query = string.Format(strQuery, SPContext.Current.Web.CurrentUser.Name);
            SPListItemCollection col = oList.GetItems(oQuery);
            return col;
        }

        void QueryWorkflows()
        {
            //Finance
            string department = "FI";
            SPListItemCollection col = GetWorkflowCenterConfigByDept(department);
            if (col.Count > 0)
            {
                FITab.Visible = true;
                foreach (SPListItem item in col)
                {
                    string relativePath = item["Relative Path"] == null ? string.Empty : item["Relative Path"].ToString();
                    string workflowName = item["Workflow Name"] == null ? string.Empty : item["Workflow Name"].ToString();
                    string remainingTasks = string.Empty;
                    if (!string.IsNullOrEmpty(relativePath))
                    {
                        remainingTasks = GetRemainingTasks(relativePath);
                    }
                    FIHtml += string.Format(formatHtml, relativePath + "/SitePages/Home.aspx", workflowName, relativePath + "/WorkflowTasks/MyItems.aspx", remainingTasks);
                }
            }

            //Admin
            department = "AD";
            col = GetWorkflowCenterConfigByDept(department);
            if (col.Count > 0)
            {
                AdminTab.Visible = true;
                foreach (SPListItem item in col)
                {
                    string relativePath = item["Relative Path"] == null ? string.Empty : item["Relative Path"].ToString();
                    string workflowName = item["Workflow Name"] == null ? string.Empty : item["Workflow Name"].ToString();
                    string remainingTasks = string.Empty;
                    if (!string.IsNullOrEmpty(relativePath))
                    {
                        remainingTasks = GetRemainingTasks(relativePath);
                    }
                    AdminHtml += string.Format(formatHtml, relativePath + "/SitePages/Home.aspx", workflowName, relativePath + "/WorkflowTasks/MyItems.aspx", remainingTasks);
                }
            }

            //HR
            department = "HR";
            col = GetWorkflowCenterConfigByDept(department);
            if (col.Count > 0)
            {
                HRTab.Visible = true;
                foreach (SPListItem item in col)
                {
                    string relativePath = item["Relative Path"] == null ? string.Empty : item["Relative Path"].ToString();
                    string workflowName = item["Workflow Name"] == null ? string.Empty : item["Workflow Name"].ToString();
                    string remainingTasks = string.Empty;
                    if (!string.IsNullOrEmpty(relativePath))
                    {
                        remainingTasks = GetRemainingTasks(relativePath);
                    }
                    HRHtml += string.Format(formatHtml, relativePath + "/SitePages/Home.aspx", workflowName, relativePath + "/WorkflowTasks/MyItems.aspx", remainingTasks);
                }
            }

            //SCM
            department = "SCM";
            col = GetWorkflowCenterConfigByDept(department);
            if (col.Count > 0)
            {
                SCMTab.Visible = true;
                foreach (SPListItem item in col)
                {
                    string relativePath = item["Relative Path"] == null ? string.Empty : item["Relative Path"].ToString();
                    string workflowName = item["Workflow Name"] == null ? string.Empty : item["Workflow Name"].ToString();
                    string remainingTasks = string.Empty;
                    if (!string.IsNullOrEmpty(relativePath))
                    {
                        remainingTasks = GetRemainingTasks(relativePath);
                    }
                    SCMHtml += string.Format(formatHtml, relativePath + "/SitePages/Home.aspx", workflowName, relativePath + "/WorkflowTasks/MyItems.aspx", remainingTasks);
                }
            }

            //MO
            department = "MO";
            col = GetWorkflowCenterConfigByDept(department);
            if (col.Count > 0)
            {
                MOTab.Visible = true;
                foreach (SPListItem item in col)
                {
                    string relativePath = item["Relative Path"] == null ? string.Empty : item["Relative Path"].ToString();
                    string workflowName = item["Workflow Name"] == null ? string.Empty : item["Workflow Name"].ToString();
                    string remainingTasks = string.Empty;
                    if (!string.IsNullOrEmpty(relativePath))
                    {
                        remainingTasks = GetRemainingTasks(relativePath);
                    }
                    MOHtml += string.Format(formatHtml, relativePath + "/SitePages/Home.aspx", workflowName, relativePath + "/WorkflowTasks/MyItems.aspx", remainingTasks);
                }
            }
        }


        SPListItemCollection GetWorkflowCenterConfigByDept(string department)
        {
            SPUserToken userToken = SPContext.Current.Web.CurrentUser.UserToken;
            string rootUrl = SPContext.Current.Site.RootWeb.Url;
            using (SPSite site = new SPSite(rootUrl, userToken))
            using (SPWeb web = site.OpenWeb())
            {
                SPList oList = web.Lists["WorkflowCenterConfig"];
                SPQuery oQuery = new SPQuery();
                string strQuery = @" <Where>
                                      <And>
                                         <Eq>
                                            <FieldRef Name='Title' />
                                            <Value Type='Text'>{0}</Value>
                                         </Eq>
                                         <Eq>
                                            <FieldRef Name='Active' />
                                            <Value Type='Boolean'>1</Value>
                                         </Eq>
                                      </And>
                                   </Where>";
                oQuery.Query = string.Format(strQuery, department);
                SPListItemCollection col = oList.GetItems(oQuery);
                return col;
            }
        }

        string GetRemainingTasks(string relativePath)
        {
            string remainingTasks = string.Empty;
            SPUserToken userToken = SPContext.Current.Web.CurrentUser.UserToken;
            string rootUrl = SPContext.Current.Site.RootWeb.Url;
            //Bankpayment
            using (SPSite site = new SPSite(rootUrl + relativePath, userToken))
            using (SPWeb web = site.OpenWeb())
            {
               
                SPListItemCollection col = GetCollection(web);
                if (col != null && col.Count > 0)
                {
                    remainingTasks = "(" + col.Count + ")";
                }

            }
            return remainingTasks;
        }
    
    }
}
