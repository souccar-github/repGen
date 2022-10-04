﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.ProjectKpi>" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<HRIS.Domain.ProjectManagment.ValueObjects.ProjectKpi>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Id) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Id)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Type) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Type.Name)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Value)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Value)%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Weight)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Weight)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Description) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Description, new ReadOnlyTextBox(true, "MultiLine"))%>
            </div>
        </td>
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <input type="button" value="<%: Resources.Shared.Buttons.Function.Edit %>" onclick="ShowEditUserControl()" class="EditButton" />
            <script type="text/javascript">
                function ShowEditUserControl() {
                    $('#addValueObjectArea').load('<%:Url.Action("JsonEdit", "ProjectKpi", new { })%>');
                }
            </script>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>
