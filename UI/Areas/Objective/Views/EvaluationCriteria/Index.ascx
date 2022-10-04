﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%:Resources.Areas.Objective.ValueObjects.EvaluationCriteria.EvaluationCriteriaModel.EvaluationCriteria %></legend>
    <div id="ValueObjectsList">
        <% Html.RenderPartial("ExpiredRules"); %>
        <% Html.RenderPartial("List"); %>
    </div>
</fieldset>
