﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.TeamMember>" %>
<%@ Import Namespace="UI.Helpers" %>
<fieldset style="background-color: Yellow; height: auto; width: 300px">
    <%:Html.Telerik().ComboBoxFor(model => model.Position.Id)
                                  .Name("nodePositions")
                                  .AutoFill(true)
                                  .BindTo(DropDownListHelpers.ListOfSelectedNodePosition(0))
                                  .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                                  .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.Contains))
                                  .HighlightFirstMatch(true)
                                  .ClientEvents(events => events.OnClose("nodePositionsEvent"))
    %>
</fieldset>
