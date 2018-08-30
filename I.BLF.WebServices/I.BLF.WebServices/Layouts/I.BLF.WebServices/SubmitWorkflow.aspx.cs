using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace I.BLF.WebServices.Layouts.I.BLF.WebServices
{
    public partial class SubmitWorkflow : LayoutsPageBase
    {
        protected SPList list;
        protected SPListItem item;
        protected void Page_Load(object sender, EventArgs e)
        {
            SPSite curSite = SPContext.Current.Site;
            SPWeb curWeb = SPContext.Current.Web;
            string listId = Request.QueryString["listId"].ToString();
            string itemIds = Request.QueryString["ID"].ToString();
            if(!string.IsNullOrEmpty(listId))
            {
                list = curWeb.Lists[new Guid(listId)];
            }
            if (list != null && !string.IsNullOrEmpty(itemIds))
            {

                SPSecurity.RunWithElevatedPrivileges(delegate() {
                    curWeb.AllowUnsafeUpdates = true;
                    var idArr = itemIds.Split(',');
                    foreach (var id in idArr)
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            int itemid = System.Convert.ToInt32(id);
                            try
                            {
                                item = list.GetItemById(itemid);
                                foreach (SPWorkflowAssociation wf in list.WorkflowAssociations)
                                {
                                    if (wf.Enabled == true)
                                    {
                                        var wfinstances = item.Workflows.Cast<SPWorkflow>().Where(wfInstance => wfInstance.IsCompleted == false);
                                        if (!wfinstances.Any())
                                        {
                                            curSite.WorkflowManager.StartWorkflow(item, wf, wf.AssociationData);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                                throw new SPException("Invalid URL: " + ex.Message);
                            }

                        }
                    }
                    curWeb.AllowUnsafeUpdates = false;
                });
                
                Redirect();
            }
        }

        private void Redirect()
        {
            if (!string.IsNullOrEmpty(Request["Source"].ToString()))
            {
                SPUtility.Redirect(Request["Source"].ToString(), SPRedirectFlags.UseSource, Context);
            }
            else
            {
                SPUtility.Redirect(list.DefaultViewUrl, SPRedirectFlags.UseSource, Context);
            }
        }
    }
}
