﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.IEnumerable<Souccar.ReportGenerator.Domain.QueryBuilder.QueryTree>>" %>
Aggregate Tree:
<%: Html.Telerik().TreeView()
                .ClientEvents(e => e.OnSelect("TreeNodeSelected"))
                .ClientEvents(e => e.OnLoad("TreeLoaded"))
                .ClientEvents(e => e.OnNodeDrop("onNodeDrop"))
                .DragAndDrop(true)
            .Name("TreeView")
            .ExpandAll(true)
                .BindTo(Model, map => map.For<Souccar.ReportGenerator.Domain.QueryBuilder.QueryTree>(bin => bin
		    .ItemDataBound((item, node) => 
            { 
                item.Text = node.DisplayName; 
                item.Value = node.FullClassPath;
                item.Selected = node.FullClassPath==(string)TempData["lastTreeNodeSelectedValue"];
            })
            .Children(node2 => node2.Nodes.OrderBy(o => o.SelectOrder))))%>
<script type="text/javascript">
    function TreeLoaded(e) {
        var lastSelectedNode = '<%:TempData["lastTreeNodeSelectedValue"]%>';
        if (lastSelectedNode != "") {
            var treeview = $('#TreeView').data('tTreeView');
            var selectedNode = treeview.findByValue(lastSelectedNode);
            e.item = selectedNode;
            TreeNodeSelected(e);
        }
    }

    function IsDragAndDropValid(e) {
        var treeview = $('#TreeView').data('tTreeView');
        var parentSourceNode = treeview.getItemText($(e.item).parent().closest(".t-item"));
        var parentDestinationNode = treeview.getItemText($(e.destinationItem).parent().closest(".t-item"));
        if (parentSourceNode != parentDestinationNode || e.dropPosition == "over") {
            return false;
        }
        return true;
    }

    function onNodeDrop(e) {
        if (IsDragAndDropValid(e) == false) {
            e.preventDefault();
        }
        else {
            var treeview = $('#TreeView').data('tTreeView');
            var dropPosition = e.dropPosition;
            var destinationNodeId = treeview.getItemValue(e.destinationItem);
            var sourceNodeId = treeview.getItemValue(e.item);
            var parentNodeId = treeview.getItemValue($(e.item).parent().closest(".t-item"));

            $.ajax({
                url: '<%:Url.Action("ReorderNodes", "QueryBuilder", new { area = "Reporting"})%>/',
                type: "POST",
                async: false,
                traditional: true,
                data: {
                    sourceNodeId: sourceNodeId,
                    destinationNodeId: destinationNodeId,
                    parentNodeId: parentNodeId,
                    dropPosition: dropPosition
                },
                success: function (result) {
                    if (result.Success == false) {
                        alert('Error While Reorder Please Try Again!');
                    }
                }
            });
        }
    }
</script>
