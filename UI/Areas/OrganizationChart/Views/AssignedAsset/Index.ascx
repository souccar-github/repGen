﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%: Resources.Areas.OrgChart.ValueObjects.AssignedGrade.PositionGrade.PositionGradeModel.AssetsTitle %></legend>
    <div id="ValueObjectsList">
        <% Html.RenderPartial("List"); %>
    </div>
</fieldset>
