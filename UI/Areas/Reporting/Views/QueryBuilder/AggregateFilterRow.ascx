﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Souccar.ReportGenerator.Domain.QueryBuilder.QueryTree>" %>
<%@ Import Namespace="Souccar.ReportGenerator.Domain.QueryBuilder" %>
<%@ Import Namespace="UI.Extensions" %>
<table>
    <tr>
        <td>
            <div class="editor-label-required">
                <%: Html.Label("Aggregate Fields") %>
            </div>
            <div class="editor-field">
                <%: Html.Telerik().ComboBox()
                    .Name("cmbAggregates")
                    //.BindTo((SelectList)ViewData["AggregateProperties"])
                    .BindTo(Model.Nodes.SelectFromList(x => x.PropertyName, y => y.DisplayName))
                    %>
            </div>
        </td>
        <td>
            <div class="editor-label-required">
                <%: Html.Label("Aggregate Functions") %>
            </div>
            <div class="editor-field">
                <%: Html.Telerik().ComboBox().Name("cmbAggregateFunction")
                .BindTo(QueryTree.GetAvailableAggregateFilters().ToList().SelectFromList(x=>x.Key.ToString(),y=>y.Value))%>
            </div>
        </td>
        <td>
            <div class="editor-label-required">
                <%: Html.Label("Aggregate Operator") %>
            </div>
            <div class="editor-field">
                <%: Html.Telerik().ComboBox().Name("cmbAggregateOperator")
            .BindTo(QueryTree.GetAvailableFilterOperators().ToList().SelectFromList(x=>x.Key.ToString(),y=>y.Value))%>
            </div>
        </td>
        <td>
            <div class="editor-label-required">
                <%: Html.Label("Value") %>
            </div>
            <div class="editor-field">
                <div id="divAggregateFilterValue">
                    <%:Html.Telerik().NumericTextBox().Name("txtAggregateFilterValue")%>
                </div>
            </div>
        </td>
        <td>
            <input type="button" id="RemoveAggregateFilterRow" value="X" onclick="RemoveAggregateFilterRow(this)" />
        </td>
    </tr>
</table>
