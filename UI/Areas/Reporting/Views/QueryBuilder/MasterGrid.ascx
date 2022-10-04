﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="Souccar.ReportGenerator.Domain.QueryBuilder" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%: Resources.Areas.ReportGenerator.Domain.Entities.Report.ReportModel.ReportGridTitle %></legend>
    <table width="100%">
        <tr>
            <td>
                <%
                    Html.Telerik().Grid<Report>("reports")
                        .Name("Grid")
                        .DataKeys(k => k.Add(o => o.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "QueryBuilder"))
                        .DataBinding(dataBinding => dataBinding.Server().Insert("Insert", "QueryBuilder"))
                        .Scrollable(builder => builder.Height(350))
                        .ToolBar(toolBar => toolBar.Template(() =>
                        { 
                %>
                <table class="GridToolBar">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <a href="<%:Url.Action("Insert", "QueryBuilder")%>">
                                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>" title="<%: Resources.Shared.Buttons.Function.Add %>"
                                    alt="<%: Resources.Shared.Buttons.Function.Add %>" height="36" width="36" align="middle" />
                            </a>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <% 
                        }))
                        .Filterable()
                        .Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                        .Columns(c =>
                                     {
                                         c.Bound(b => b.Id).Width(35).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                         c.Bound(o => o.Name).Sortable(true).Title(Resources.Areas.ReportGenerator.Domain.Entities.Report.ReportModel.Name);
                                         c.Bound(o => o.Template.Name).Sortable(true).Title(Resources.Areas.ReportGenerator.Domain.Entities.Report.ReportModel.ReportTemplate);
                                         c.Bound(o => o.Id).Title("")
                                                .Format(Html.ActionLink(Resources.Shared.Buttons.Function.ViewReport, "Index", "DynamicReport", new { reportId = "{0}", Area = "Reporting" }, new { target = "_blank" }).ToHtmlString())
                                                .Encoded(false)
                                                .Width("5%").Sortable(false).Filterable(false);
                                         c.Command(s =>
                                                       {
                                                           s.Custom("EditCommand").ButtonType(GridButtonType.Image).Ajax(false)
                                                                        .Action("Insert", "QueryBuilder")
                                                                        .HtmlAttributes(new { @class = "t-edit" });
                                                           if (ViewData["CanDelete"] != null &&
                                                               (bool)ViewData["CanDelete"])
                                                           {
                                                               s.Delete().ButtonType(GridButtonType.Image);
                                                           }
                                                       }).Width("8%");

                                     })
                       .Pageable(p => p.PageSize(5))
                       .Render();

                %>
            </td>
        </tr>
    </table>
</fieldset>
