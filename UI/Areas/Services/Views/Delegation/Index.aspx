﻿<%@ Page Inherits="System.Web.Mvc.ViewPage<UI.Areas.Services.DTO.ViewModels.DelegationViewModel>" 
Language="C#" MasterPageFile="~/Areas/Services/Views/Shared/Services.master" Title="" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%  Html.Telerik().Splitter().Name("SplitterTree")
            .Orientation(SplitterOrientation.Vertical)
            .HtmlAttributes(new { style = "height: 500px;" })
            .Panes(hPanes =>
            {
                hPanes.Add()
                    .Size("490px")
                    .Resizable(false)
                    .Collapsible(false)
                    .Scrollable(false)
                    .Collapsed(true)
                    .Content(() =>
                    {
                    });

                hPanes.Add()
                    .Resizable(true)
                    .Scrollable(true)
                    .Content(() => Html.RenderPartial("MasterGrid"));
            })
            .Render();
    %>
    <br />
    <div id="result" style="display: none">
    </div>
</asp:Content>
