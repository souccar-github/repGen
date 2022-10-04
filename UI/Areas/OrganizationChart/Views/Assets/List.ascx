﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.ValueObjects.Asset>" %>
<% using (Ajax.BeginForm("Save", "Assets", new AjaxOptions { OnComplete = "JsonAdd_OnComplete" }))
   {%>
<script type="text/javascript">
    function JsonAdd_OnComplete(context) {

        var JsonAdd = context.get_response().get_object();
        if (JsonAdd.Success) {
            var url = '<%: Url.Action("Index", "Grade", new { selectedTabOrder=2 }) %>';
            self.location = url;
        } else { 
            $("#ValueObjectsList").html(JsonAdd.PartialViewHtml);
            Toggle("add");
        }
    }
</script>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td align="left">
            <img onclick="cancel()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/90.png") %>"
                title=<%: Resources.Shared.Buttons.Function.Cancel %> alt=<%: Resources.Shared.Buttons.Function.Cancel %> height="24" width="24" align="middle" />
            <script type="text/javascript">
                function cancel() {
                    $('#result').fadeOut('slow', function () {
                        $('#result').empty();
                    });
                }
            </script>
        </td>
        
    </tr>
</table>
<div id="addValueObjectArea" style="width: auto">   
    <% 
       if (ViewData["ValueObjectsList"] != null & ((IEnumerable<HRIS.Domain.OrgChart.ValueObjects.Asset>)ViewData["ValueObjectsList"]).Count() == 0)
       {
           if (Model != null)
           {
               Html.RenderPartial("Edit");
           }
           else
           {
               Html.RenderAction("Load");
           }
       }
       else
       {
           if (Model != null)
           {
               Html.RenderPartial("Edit");
           }
       }
    %>
</div>

<% } %>
<% if (ViewData["ValueObjectsList"] != null)
   {
       Html.RenderPartial("Select");
   } %>
<input id="toggleStatus" style="width: 0; border-width: 0; background-color: White;
    visibility: hidden" />
<input id="toggleFiredBy" style="width: 0; border-width: 0; background-color: White;
    visibility: hidden" />
<script type="text/javascript">

    function add() {
        $('#addValueObjectArea').load('<%: Url.Action("Load", "Assets") %>');

        Toggle("add");
    }

</script>
