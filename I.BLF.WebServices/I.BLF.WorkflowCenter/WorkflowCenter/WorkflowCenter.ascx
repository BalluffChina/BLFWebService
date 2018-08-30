<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkflowCenter.ascx.cs" Inherits="I.BLF.WorkflowCenter.WorkflowCenter.WorkflowCenter" %>


<script src="../_layouts/15/I.BLF.WorkflowCenter/Script/jquery-1.11.1.min.js" type="text/javascript"></script>

<script type="text/javascript">
    function ShowAdminWorkflows() {;
        HideAllWorkflows();
        $("div[id*=AdminDept]").css("display", "block");
    }

    function ShowFinanceWorkflows() {;
        HideAllWorkflows();
        $("div[id*=FinanceDept]").css("display", "block");
    }

    function ShowHRWorkflows() {;
        HideAllWorkflows();
        $("div[id*=HRDept]").css("display", "block");
    }

    function ShowSCMWorkflows() {;
        HideAllWorkflows();
        $("div[id*=SCMDept]").css("display", "block");
    }

    function ShowITWorkflows() {;
        HideAllWorkflows();
        $("div[id*=ITDept]").css("display", "block");
    }

    function ShowMOWorkflows() {;
        HideAllWorkflows();
        $("div[id*=MODept]").css("display", "block");
    }

    function HideAllWorkflows() {
        $(".cnt").css("display", "none");
    }

</script>
<link href="../_layouts/15/I.BLF.WorkflowCenter/CSS/style.css" rel="stylesheet" type="text/css" />


<html xmlns="http://www.w3.org/1999/xhtml">
<body>
    <table>
        <tr>
            <td>
                <div id="wrapper">
                    <div style="color: rgb(68, 68, 68); padding-left: 20px; font-size: 19px;">Workflow Center</div>
                <!-- top-nav -->
		            <nav class="top-nav">
			            <div class="shell">
				            <a href="#" class="nav-btn">HOMEPAGE<span></span></a>
				            <span class="top-nav-shadow"></span>
				            <ul>
					            <li id="AdminTab" onmouseover="ShowAdminWorkflows()" runat="server" visible="false"><span><a href="#">Admin Dept.</a></span></li>
					            <li id="FITab" onmouseover="ShowFinanceWorkflows()" runat="server" visible="false"><span><a href="#">Finance Dept.</a></span></li>
					            <li id="HRTab" onmouseover="ShowHRWorkflows()" runat="server" visible="false"><span><a href="#">HR Dept.</a></span></li>
                                <li id="ITTab" onmouseover="ShowITWorkflows()" runat="server" ><span><a href="#">IT Dept.</a></span></li>
					            <li id="SCMTab" onmouseover="ShowSCMWorkflows()" runat="server" visible="false"><span><a href="#">SCM Dept.</a></span></li>
                                <li id="MOTab" onmouseover="ShowMOWorkflows()" runat="server" visible="false"><span><a href="#">MO Dept.</a></span></li>
					            <%--<li><span><a href="#">jobs</a></span></li>
					            <li><span><a href="#">blog</a></span></li>
					            <li><span><a href="#">contacts</a></span></li>--%>
				            </ul>
			            </div>
		            </nav>
                </div>
            </td>          
        </tr>
        <tr>
            <td>
                <div class="content" >
	                <div id="FinanceDept"  runat="server" style="display:none" class="cnt">
		                <h3>Finance Dept.</h3>
		                <ul>
			                <%--<li><a id="BPLink" runat="server" href="#">Bank Payment 付款凭证</a><asp:HyperLink ID="BPTasks" Text="" ForeColor="Red" runat="server" NavigateUrl="#"></asp:HyperLink></li>
			                <li><a id="CCRLink" runat="server" href="#">Customer Credit Application and Check Form 客户信贷申请核查表</a><asp:HyperLink ID="CCRTasks" Text="" ForeColor="Red" runat="server" NavigateUrl="#"></asp:HyperLink></li>
                            <li><a id="RRLink" runat="server" href="#">Reimbursement 费用报销</a><asp:HyperLink ID="RRTasks" Text="" ForeColor="Red" runat="server" NavigateUrl="#"></asp:HyperLink></li>--%>
		                    <%=FIHtml%>
                        </ul>
	                </div>
                    <div id="SCMDept" style="display:none" runat="server" class="cnt">
		                <h3>SCM Dept.</h3>
		                <ul>
			                <%=SCMHtml%>
		                </ul>
	                </div>
                    <div id="AdminDept" style="display:none" runat="server" class="cnt">
		                <h3>Admin Dept.</h3>
		                <ul>
			                <%=AdminHtml%>
		                </ul>
	                </div>
                    <div id="HRDept" style="display:none" runat="server" class="cnt">
		                <h3>HR Dept.</h3>
		                <ul>
			                <%=HRHtml%>
		                </ul>
	                </div>
                    <div id="ITDept" style="display:none" runat="server" class="cnt">
		                <h3>IT Dept.</h3>
		                <ul>
			                <li><a id="ITSTLink" runat="server" href="#">IT Service Ticket System 信息技术支持</a><asp:HyperLink ID="ITSTTasks" Text="" ForeColor="Red" runat="server" NavigateUrl="#"></asp:HyperLink></li>
		                </ul>
	                </div>
                     <div id="MODept" style="display:none" runat="server" class="cnt">
		                <h3>MO Dept.</h3>
		                <ul>
			                 <%=MOHtml%>
		                </ul>
	                </div>
                </div>
            </td>
        </tr>   
    </table>
    <input id="errorMsg" type="text" value="" style="display:none" runat="server" />
</body>
</html>