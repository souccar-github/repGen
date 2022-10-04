﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Souccar.ReportGenerator.Domain.QueryBuilder.QueryTree>" %>
<%@ Import Namespace="Souccar.ReportGenerator.Domain.QueryBuilder" %>
<%@ Import Namespace="UI.Extensions" %>

<%if (Model != null)
  { %>
<% Html.Telerik().TabStrip().Name("TabStrip")
       .Items(tabstrip =>
                  {
                      tabstrip.Add().Text("Fields").Content(() =>
                                                                {%>
<table>
    <tr>
        <td>
            <%: Html.ListBox("lstAllFields", Model.Leaves.Where(x => x.Selected == 0).ToList()
                                                         .SelectFromList(x => x.PropertyFullPath, y => y.DisplayName),
                                     new {size = 15, style = "width:250px;"}) %>
        </td>
        <td>
            <input type="button" value="<<" onclick="selectClicked('fields','<<')" /><br />
            <input type="button" value="<" onclick="selectClicked('fields','<')" /><br />
            <input type="button" value=">" onclick="selectClicked('fields','>')" /><br />
            <input type="button" value=">>" onclick="selectClicked('fields','>>')" />
        </td>
        <td>
            <%: Html.ListBox(
                    "lstSelectedFields", Model.Leaves.Where(x => x.Selected > 0).OrderBy(z => z.Selected).ToList()
                                             .SelectFromList(x => x.PropertyFullPath, y => y.DisplayName),
                    new {size = 15, style = "width:250px;"}) %>
        </td>
        <td>
            <input type="button" value="Up" onclick="OrderListItems('fields','Up')" /><br />
            <input type="button" value="Down" onclick="OrderListItems('fields','Down')" />
        </td>
    </tr>
</table>
<% });

                      tabstrip.Add().Text("Grouping").Content(() =>
                                                                  {%>
<table>
    <tr>
        <td>
            <%:
                                                                          Html.ListBox("lstAllGroupingFields",
                                                                                       Model.Leaves.Where(
                                                                                           x =>
                                                                                           x.GroupDescriptor.
                                                                                               GroupByOrder == 0).ToList
                                                                                           ()
                                                                                           .SelectFromList(
                                                                                               x => x.PropertyFullPath,
                                                                                               y => y.DisplayName),
                                                                                       new
                                                                                           {
                                                                                               size = 15,
                                                                                               style = "width:250px;"
                                                                                           }) %>
        </td>
        <td>
            <input type="button" value="<<" onclick="selectClicked('Grouping','<<')" /><br />
            <input type="button" value="<" onclick="selectClicked('Grouping','<')" /><br />
            <input type="button" value=">" onclick="selectClicked('Grouping','>')" /><br />
            <input type="button" value=">>" onclick="selectClicked('Grouping','>>')" />
        </td>
        <td>
            <%:
                                                                          Html.ListBox("lstSelectedGroupingFields",
                                                                                       Model.Leaves.Where(
                                                                                           x =>
                                                                                           x.GroupDescriptor.
                                                                                               GroupByOrder > 0).OrderBy
                                                                                           (z =>
                                                                                            z.GroupDescriptor.
                                                                                                GroupByOrder).ToList()
                                                                                           .SelectFromList(
                                                                                               x => x.PropertyFullPath,
                                                                                               y => y.DisplayName),
                                                                                       new
                                                                                           {
                                                                                               size = 15,
                                                                                               style = "width:250px;"
                                                                                           }) %>
        </td>
        <td>
            <input type="button" value="Up" onclick="OrderListItems('Grouping','Up')" /><br />
            <input type="button" value="Down" onclick="OrderListItems('Grouping','Down')" />
        </td>
    </tr>
</table>
<% });
                      tabstrip.Add().Text("Sorting").Content(() =>
                                                                 {%>
<table>
    <tr>
        <td>
            <%:Html.ListBox("lstAllSortingFields",
            Model.Leaves.Where(x =>x.SortDescriptor.SortOrder ==0).ToList().SelectFromList(x => x.PropertyFullPath,y => y.DisplayName), new{size = 15,style = "width:250px;"}) %>
        </td>
        <td>
            <input type="button" value="<<" onclick="selectClicked('Sorting','<<')" /><br />
            <input type="button" value="<" onclick="selectClicked('Sorting','<')" /><br />
            <input type="button" value=">" onclick="selectClicked('Sorting','>')" /><br />
            <input type="button" value=">>" onclick="selectClicked('Sorting','>>')" />
        </td>
        <td>
            <%:Html.ListBox("lstSelectedSortingFields",Model.Leaves.Where(x =>x.SortDescriptor.SortOrder > 0).OrderBy(z =>z.SortDescriptor.SortOrder).ToList()
            .SelectFromList(x => x.PropertyFullPath,y => y.DisplayName + (y.SortDescriptor.SortDirection == ListSortDirection.Ascending?" (Asc)":" (Desc)")),
            new{size = 15,style = "width:250px;"}) %>
        </td>
        <td>
            <input type="button" value="Up" onclick="OrderListItems('Sorting','Up')" /><br />
            <input type="button" value="Down" onclick="OrderListItems('Sorting','Down')" />
            <input type="button" value="Sort Asc/Desc" onclick="OrderListItems('Sorting','Ordering')" /><br />
        </td>
    </tr>
</table>
<% });
                      tabstrip.Add().Text("Filter").Content(() =>
                                                                {%>
<div id="divFilters">
    <%int counter = 0; %>
    <% for (int i = 0; i < Model.Leaves.Count; i++)
       {%>
    <% for (int y = 0; y < Model.Leaves[i].FilterDescriptors.Count; y++)
       {%>
    <div id='divFilterRow_<%:(counter + 1) %>'>
        <table>
            <tr>
                <td>
                    <div class="editor-label-required">
                        <%: Html.Label("Field") %>
                    </div>
                    <%: Html.Telerik().ComboBox()
                           .Name("cmbLeafs_" + (counter + 1))
                    .BindTo((SelectList)ViewData["QueryLeafProperties"])
                    .Value(Model.Leaves[i].PropertyFullPath)
.ClientEvents(e => e.OnChange("LeafChanged"))
                    %>
                </td>
                <td>
                    <div class="editor-label-required">
                        <%: Html.Label("Operator") %>
                    </div>
                    <% if (Model.Leaves[i].PropertyType == typeof(string))
                       {%>
                    <%: Html.Telerik().ComboBox()
                                                                         .Name("cmbOperator_" + (counter + 1))
                                           .BindTo((SelectList)ViewData["StringOperators"])
                    .Value(Model.Leaves[i].FilterDescriptors[y].FilterOperator.ToString())
                    %>
                    <% }
                       else
                       {%>
                    <%: Html.Telerik().ComboBox()
                                                                         .Name("cmbOperator_" + (counter + 1))
                    .BindTo((SelectList)ViewData["OtherOperators"])
                    .Value(Model.Leaves[i].FilterDescriptors[y].FilterOperator.ToString())
                    %>
                    <%}%>
                </td>


                <td>
                    <div class="editor-label-required">
                        <%: Html.Label("Value") %>
                    </div>

                    <% if (Model.Leaves[i].PropertyType == typeof(DateTime))
                       {%>
                    <%: Html.Telerik().DatePicker().Name("txtFilterValue_" + (counter + 1)).HtmlAttributes(new { disabled = true}).Value(DateTime.Parse(Model.Leaves[i].FilterDescriptors[y].Value.ToString()))%>

                    <% }
                       else
                       {%>
                    <%:Html.TextBox("txtFilterValue_" + (counter + 1), Model.Leaves[i].FilterDescriptors[y].Value)%>
                    <%}%>
                
                </td>
                <td>
                    <input type="button" id="RemoveFilterRow_<%:(counter + 1)%>" value="X" onclick="RemoveFilterRow(this)" />
                </td>
            </tr>
        </table>
    </div>
    <%counter++; %>
    <%} %>
    <%} %>
</div>
<script type="text/javascript">

    $(document).ready(function(){
        if(<%:counter %>>0)
        {
            filterCounter = <%:counter %>;
        }



    });

    //function RemoveFilterRow(obj) {
    //    var rowNumber = $(obj).attr('id').split('_')[1];
    //    $("#divFilterRow_" + rowNumber).remove();
    //}
</script>
<input type="button" value="+" onclick="AddFilterRow()" />
<% });










                      tabstrip.Add().Text("AggregateFilter").Content(() =>
                                                                                   {%>
<div id="divAggregateFilters">
    <%int aggregateCounter = 0; %>
    <% for (int i = 0; i < Model.AggregateFilters.Count; i++)
       {%>    
    <div id='divAggregateFilterRow_<%:(aggregateCounter + 1) %>'>
        <table>
            <tr>
                <td>
                    <div class="editor-label-required">
                        <%: Html.Label("Aggregate Fields") %>
                    </div>
                    <%: Html.Telerik().ComboBox()
                           .Name("cmbAggregates_" + (aggregateCounter + 1))
                    .BindTo(Model.Nodes.SelectFromList(x => x.PropertyName, y => y.DisplayName))
                    //.BindTo((SelectList)ViewData["AggregateProperties"])
                    
                    .Value(Model.AggregateFilters[i].PropertyName)%>
                </td>
                
                <td>
                    <div class="editor-label-required">
                        <%: Html.Label("Aggregate Functions") %>
                    </div>
                    <%: Html.Telerik().ComboBox()
                    .Name("cmbAggregateFunction_" + (aggregateCounter + 1))
                    .BindTo(QueryTree.GetAvailableAggregateFilters().ToList().SelectFromList(x=>x.Key.ToString(),y=>y.Value))
                    .Value(Model.AggregateFilters[i].AggregateFunction.ToString())%>
                </td>

                <td>
                    <div class="editor-label-required">
                        <%: Html.Label("Aggregate Operator") %>
                    </div>
                    <%: Html.Telerik().ComboBox()
                    .Name("cmbAggregateOperator_" + (aggregateCounter + 1))
                    .BindTo(QueryTree.GetAvailableFilterOperators().ToList().SelectFromList(x=>x.Key.ToString(),y=>y.Value))
                    .Value(Model.AggregateFilters[i].FilterOperator.ToString())%>
                </td>
                <td>
                    <div class="editor-label-required">
                        <%: Html.Label("Value") %>
                    </div>
                    <%:Html.Telerik().NumericTextBox().Name("txtAggregateFilterValue_" + (aggregateCounter + 1)).Value(double.Parse(Model.AggregateFilters[i].Value.ToString()))%>
                </td>
                <td>
                    <input type="button" id="RemoveAggregateFilterRow_<%:(aggregateCounter + 1)%>" value="X" onclick="RemoveAggregateFilterRow(this)" />
                </td>
            </tr>
        </table>
    </div>
    <%aggregateCounter++; %>
    <%} %>
</div>
<script type="text/javascript">

    $(document).ready(function(){
        if(<%:aggregateCounter %>>0)
        {
            aggregateFilterCounter = <%:aggregateCounter %>;
        }
    });
             
    //function RemoveAggregateFilterRow(obj) {
    //    var rowNumber = $(obj).attr('id').split('_')[1];
    //    $("#divAggregateFilterRow_" + rowNumber).remove();
    //}
</script>
<input type="button" value="+" onclick="AddAggregateFilterRow()" />
<% });









                  }).SelectedIndex(0).Render();%>
<br />
<%--<input type="button" value="Save Node State" onclick="SaveQueryLeaf()" />--%>
<% } %>