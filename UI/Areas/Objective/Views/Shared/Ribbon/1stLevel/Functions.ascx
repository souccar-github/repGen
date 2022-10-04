﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table width="80%">
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        <div class="box" style="z-index: 1000">
                            <ul id="ldd_menu" class="ldd_menu">
                                <li><span>
                                    <%: Resources.Areas.Objective.Views.Shared.Ribbon._1stLevel.Functions.OrganizationalObjective %></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Objective.Views.Shared.Ribbon._1stLevel.Functions.OrganizationalObjective %></li>
                                            <li><a href="#">
                                                <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "StrategicObjective",
                                              new {selectedTabOrder = 0, ribbon = true}, null)%></a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span>
                                    <%: Resources.Areas.Objective.Views.Shared.Ribbon._1stLevel.Functions.Objective %></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">Objective</li>
                                            <li><a href="#">
                                                <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Objective",
                                              new {selectedTabOrder = 0, ribbon = true}, null)%></a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Objective.Views.Shared.Ribbon._1stLevel.Functions.OrganizationalObjective %></li>
                                            <li>
                                                <%
                                                    if (Session["CurrentSecondLevelRowId"] != null)
                                                    {
                                                        if ((int)Session["CurrentSecondLevelRowId"] != 0)
                                                        {%>
                                                <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToObjective",
                                                                                              "Objective",
                                                                                              new {selectedTabOrder = 11, ribbon = false},
                                                                                              null)%>
                                                <%
                                                        }
                                                        else
                                                        {
                                                %>
                                                <%: Resources.Areas.Objective.Views.Shared.RibbonLinks.Add.ChooseObjective %>
                                                <%
                                                        }
                                                    }
                                                    else
                                                    {
                                                %>
                                                <%: Resources.Areas.Objective.Views.Shared.RibbonLinks.Add.ChooseObjective %>
                                                <%
                                                    }
                                                %>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Objective.Views.Shared.Ribbon._1stLevel.Functions.ActionPlan %></li>
                                            <li>
                                                <%
                                                    if (Session["CurrentSecondLevelRowId"] != null)
                                                    {
                                                        if ((int)Session["CurrentSecondLevelRowId"] != 0)
                                                        {%>
                                                <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToObjective",
                                                                                              "Objective",
                                                                                              new {selectedTabOrder = 1, ribbon = false},
                                                                                              null)%>
                                                <%
                                                        }
                                                        else
                                                        {
                                                %>
                                               <%: Resources.Areas.Objective.Views.Shared.RibbonLinks.Add.ChooseObjective %>
                                                <%
                                                        }
                                                    }
                                                    else
                                                    {
                                                %>
                                                <%: Resources.Areas.Objective.Views.Shared.RibbonLinks.Add.ChooseObjective %>
                                                <%
                                                    }
                                                %>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Objective.Views.Shared.Ribbon._1stLevel.Functions.ObjectiveKPI %></li>
                                            <li>
                                                <%
                                                    if (Session["CurrentSecondLevelRowId"] != null)
                                                    {
                                                        if ((int)Session["CurrentSecondLevelRowId"] != 0)
                                                        {%>
                                                <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToObjective",
                                                                                              "Objective",
                                                                                              new {selectedTabOrder = 2, ribbon = false},
                                                                                              null)%>
                                                <%
                                                        }
                                                        else
                                                        {
                                                %>
                                              <%: Resources.Areas.Objective.Views.Shared.RibbonLinks.Add.ChooseObjective %>
                                                <%
                                                        }
                                                    }
                                                    else
                                                    {
                                                %>
                                                <%: Resources.Areas.Objective.Views.Shared.RibbonLinks.Add.ChooseObjective %>
                                                <%
                                                    }
                                                %>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Objective.Views.Shared.Ribbon._1stLevel.Functions.EvaluationCriteria %></li>
                                            <li>
                                                <%
                                                    if (Session["CurrentSecondLevelRowId"] != null)
                                                    {
                                                        if ((int)Session["CurrentSecondLevelRowId"] != 0)
                                                        {%>
                                                <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToObjective",
                                                                                              "Objective",
                                                  new { selectedTabOrder = 3, ribbon = false },
                                                                                              null)%>
                                                <%
                                                        }
                                                        else
                                                        {
                                                %>
                                               <%: Resources.Areas.Objective.Views.Shared.RibbonLinks.Add.ChooseObjective %>
                                                <%
                                                        }
                                                    }
                                                    else
                                                    {
                                                %>
                                                <%: Resources.Areas.Objective.Views.Shared.RibbonLinks.Add.ChooseObjective %>
                                                <%
                                                    }
                                                %>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Objective.Views.Shared.Ribbon._1stLevel.Functions.ObjectiveConstraints %></li>
                                            <li>
                                                <%
                                                    if (Session["CurrentSecondLevelRowId"] != null)
                                                    {
                                                        if ((int)Session["CurrentSecondLevelRowId"] != 0)
                                                        {%>
                                                <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToObjective",
                                                                                              "Objective",
                                                 new { selectedTabOrder = 4, ribbon = false },
                                                                                              null)%>
                                                <%
                                                        }
                                                        else
                                                        {
                                                %>
                                              <%: Resources.Areas.Objective.Views.Shared.RibbonLinks.Add.ChooseObjective %>
                                                <%
                                                        }
                                                    }
                                                    else
                                                    {
                                                %>
                                                <%: Resources.Areas.Objective.Views.Shared.RibbonLinks.Add.ChooseObjective %>
                                                <%
                                                    }
                                                %>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Objective.Views.Shared.Ribbon._1stLevel.Functions.SharedWith %></li>
                                            <li>
                                                <%
                                                    if (Session["CurrentSecondLevelRowId"] != null)
                                                    {
                                                        if ((int)Session["CurrentSecondLevelRowId"] != 0)
                                                        {%>
                                                <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToObjective",
                                                                                              "Objective",
                                                new { selectedTabOrder = 5, ribbon = false },
                                                                                              null)%>
                                                <%
                                                        }
                                                        else
                                                        {
                                                %>
                                                <%: Resources.Areas.Objective.Views.Shared.RibbonLinks.Add.ChooseObjective %>
                                                <%
                                                        }
                                                    }
                                                    else
                                                    {
                                                %>
                                                <%: Resources.Areas.Objective.Views.Shared.RibbonLinks.Add.ChooseObjective %>
                                                <%
                                                    }
                                                %>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Objective.Views.Shared.Ribbon._1stLevel.Functions.Evaluation %></li>
                                            <li>
                                                <%
                                                    if (Session["CurrentSecondLevelRowId"] != null)
                                                    {
                                                        if ((int)Session["CurrentSecondLevelRowId"] != 0)
                                                        {%>
                                                <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToObjective",
                                                                                              "Objective",
                                                new { selectedTabOrder = 6, ribbon = false },
                                                                                              null)%>
                                                <%
                                                        }
                                                        else
                                                        {
                                                %>
                                             <%: Resources.Areas.Objective.Views.Shared.RibbonLinks.Add.ChooseObjective %>
                                                <%
                                                        }
                                                    }
                                                    else
                                                    {
                                                %>
                                              <%: Resources.Areas.Objective.Views.Shared.RibbonLinks.Add.ChooseObjective %>
                                                <%
                                                    }
                                                %>
                                            </li>
                                        </ul>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<script type="text/javascript">
    $(function () {
        /**
        * the menu
        */
        var $menu = $('#ldd_menu');

        /**
        * for each list element,
        * we show the submenu when hovering and
        * expand the span element (title) to 510px
        */
        $menu.children('li').each(function () {
            var $this = $(this);
            var $span = $this.children('span');
            $span.data('width', $span.width());

            $this.bind('mouseenter', function () {
                $menu.find('.ldd_submenu').stop(true, true).hide();
                $span.stop().animate({ 'width': '100px' }, 300, function () {
                    $this.find('.ldd_submenu').slideDown(300);
                });
            }).bind('mouseleave', function () {
                $this.find('.ldd_submenu').stop(true, true).hide();
                $span.stop().animate({ 'width': $span.data('width') + 'px' }, 300);
            });
        });
    });
</script>
