var CurrentFiltersFieldsNames = new Array();
var FiltersStringOperators = new Array();
var FiltersNumericAndDateOperators = new Array();
var FielterIdCounter = 0;
var IsFilterAddMode = 0;
var OriginalEditedFilterModel;

var CurrentAggregateFiltersFieldsNames = new Array();
var AggregateFilterOperators = new Array();
var AggregateFilterFunctions = new Array();
var AggregateFielterIdCounter = 0;
var IsAggregateFilterAddMode = 0;
var OriginalEditedAggregateFilterModel;

var dateFormat = "dd/MM/yyyy";
var SelectedTreeNode;
var Report;
var TreeNodeSelectedFirstTime = 1;

function SubmitReport() {
    var treeview = $("#tree").data("kendoTreeView");
    var root = $('.k-item:first');
    treeview.select(root);
    treeview.trigger('select', { node: root });
    treeview.select($());
    Report.QueryTree = SelectedTreeNode;

    $.ajax(
        {
            url: window.applicationpath + "ReportGenerator/ReportBuilder/SaveReport",
            type: "POST",
            data: JSON.stringify(Report),
            contentType: 'application/json',
            success: function () {
            }
        });
}

function treeview_databound(e) {
    debugger;
    var treeview = e.sender;
    var root = $('.k-item:first');
    treeview.select(root);
    treeview.trigger('select', { node: root });
    treeview.select($());
}

function Aggregates_OnChange() {
    var aggregate = $("#Aggregates").val();
    $.ajax(
            {
                url: window.applicationpath + "ReportGenerator/ReportBuilder/GetDataSources",
                type: "POST",
                data: { aggregateClass: aggregate },
                success: function (response) {
                    Report = response;
                    var treeData = new Array();
                    treeData.push(Report.QueryTree);
                    var treeDataSource = new kendo.data.HierarchicalDataSource({
                        data: treeData,
                        schema: {
                            model: {
                                id: "FullClassPath",
                                children: "Nodes",
                                hasChildren: "Nodes.length>0"
                            }
                        }
                    });

                    if ($("#tree").data("kendoTreeView") != null) {
                        $("#tree").data("kendoTreeView").setDataSource(treeDataSource);
                    } else {
                        $("#tree").kendoTreeView({
                            dataSource: treeDataSource,
                            dataTextField: ["DisplayName", "DisplayName"],
                            select: onSelectNode,
                            dataBound: treeview_databound
                        });
                    }

                }
            }
        );


}

$(document).ready(function () {

    $.ajax(
            {
                url: window.applicationpath + "ReportGenerator/ReportBuilder/GetAggregates",
                type: "POST",
                data: {},
                success: function (response) {
                    var data = response;

                    $("#Aggregates").kendoDropDownList({
                        dataTextField: "text",
                        dataValueField: "value",
                        dataSource: data,
                        index: 0,
                        change: Aggregates_OnChange
                    });
                }
            }
        );


    $.ajax(
        {
            url: window.applicationpath + "ReportGenerator/ReportBuilder/GetAvailableFilterOperators",
            type: "POST",
            data: {},
            success: function (response) {
                FiltersStringOperators = response.stringOperators;
                FiltersNumericAndDateOperators = response.numericAndDateOperators;
                AggregateFilterOperators = response.aggregateOperators;
                AggregateFilterFunctions = response.aggregateFunctions;
            }
        }
    );


    $("#tabstrip").kendoTabStrip({
        animation: {
            open: {
                effects: "fadeIn"
            }
        }
    });

    $("#tabstrip").hide();
    $("#removeAllSelected").click(function () {
        var item = SelectedTreeNode;
        item.Leaves.forEach(function (leaf) {
            leaf.Selected = 0;
        });
        refreshSelectList(item);
    });

    $("#removeSelected").click(function () {
        var item = SelectedTreeNode;
        var selectedItems = $("#selectedFieldList  :selected");
        for (var i = 0; i < selectedItems.length; i++) {
            _.findWhere(item.Leaves, { PropertyName: selectedItems[i].value }).Selected = 0;
        }
        var ordreList = _.sortBy(item.Leaves, function (leaf) { return leaf.Selected; });
        var count = 1;
        for (var j = 0; j < ordreList.length; j++) {
            if (ordreList[j].Selected != 0)
                ordreList[j].Selected = count++;
        }
        refreshSelectList(item);
        var temp = [];
        for (i = 0; i < selectedItems.length; i++) {
            temp[i] = selectedItems[i].value;
        }
        $("#fieldList").val(temp);
    });

    $("#addSelected").click(function () {
        var item = SelectedTreeNode;
        var maxSelected = _.max(item.Leaves, function (leaf) { return leaf.Selected; }).Selected;
        var selectedItems = $("#fieldList  :selected");
        for (var i = 0; i < selectedItems.length; i++) {
            _.findWhere(item.Leaves, { PropertyName: selectedItems[i].value }).Selected = ++maxSelected;
        }
        refreshSelectList(item);
        var temp = [];
        for (i = 0; i < selectedItems.length; i++) {
            temp[i] = selectedItems[i].value;
        }
        $("#selectedFieldList").val(temp);
    });

    $("#selectAll").click(function () {
        var item = SelectedTreeNode;
        var maxSelected = _.max(item.Leaves, function (leaf) { return leaf.Selected; }).Selected;
        var selectedItems = $("#fieldList  option");
        for (var i = 0; i < selectedItems.length; i++) {
            _.findWhere(item.Leaves, { PropertyName: selectedItems[i].value }).Selected = ++maxSelected;
        }
        refreshSelectList(item);
    });

    $("#moveSelectUp").click(function () {
        var item = SelectedTreeNode;
        var selectedItems = $("#selectedFieldList  :selected");
        for (var i = 0; i < selectedItems.length; i++) {
            if (selectedItems[i].selected) {
                var leaf = _.findWhere(item.Leaves, { PropertyName: selectedItems[i].value });
                if (leaf.Selected == 1) {
                    continue;
                }
                var prevLeaf = _.findWhere(item.Leaves, { Selected: leaf.Selected - 1 });
                leaf.Selected--;
                prevLeaf.Selected++;
            }
        }
        refreshSelectList(item);
        var temp = [];
        for (i = 0; i < selectedItems.length; i++) {
            temp[i] = selectedItems[i].value;
        }
        $("#selectedFieldList").val(temp);
    });

    $("#moveSelectDown").click(function () {
        var item = SelectedTreeNode;
        var maxSelected = _.max(item.Leaves, function (l) { return l.Selected; }).Selected;
        var selectedItems = $("#selectedFieldList  :selected");
        for (var i = 0; i < selectedItems.length; i++) {
            if (selectedItems[i].selected) {
                var leaf = _.findWhere(item.Leaves, { PropertyName: selectedItems[selectedItems.length - (i + 1)].value });
                if (leaf.Selected == maxSelected) {
                    continue;
                }
                var prevLeaf = _.findWhere(item.Leaves, { Selected: leaf.Selected + 1 });
                leaf.Selected++;
                prevLeaf.Selected--;
            }
        }
        refreshSelectList(item);
        var temp = [];
        for (i = 0; i < selectedItems.length; i++) {
            temp[i] = selectedItems[i].value;
        }
        $("#selectedFieldList").val(temp);
    });

    $("#removeAllGroups").click(function () {
        var item = SelectedTreeNode;
        item.Leaves.forEach(function (leaf) {
            leaf.GroupDescriptor.GroupByOrder = 0;
        });
        refreshGroupList(item);
    });

    $("#removeGroup").click(function () {
        var item = SelectedTreeNode;
        var selectedItems = $("#selectedGroupList  :selected");
        for (var i = 0; i < selectedItems.length; i++) {
            _.findWhere(item.Leaves, { PropertyName: selectedItems[i].value }).GroupDescriptor.GroupByOrder = 0;
        }
        var ordreList = _.sortBy(item.Leaves, function (leaf) { return leaf.GroupDescriptor.GroupByOrder; });
        var count = 1;
        for (var j = 0; j < ordreList.length; j++) {
            if (ordreList[j].GroupDescriptor.GroupByOrder != 0)
                ordreList[j].GroupDescriptor.GroupByOrder = count++;
        }
        refreshGroupList(item);
        var temp = [];
        for (i = 0; i < selectedItems.length; i++) {
            temp[i] = selectedItems[i].value;
        }
        $("#groupList").val(temp);
    });

    $("#addGroup").click(function () {
        var item = SelectedTreeNode;
        var maxGroupOrder = _.max(item.Leaves, function (leaf) { return leaf.GroupDescriptor.GroupByOrder; }).GroupDescriptor.GroupByOrder;
        var selectedItems = $("#groupList  :selected");
        for (var i = 0; i < selectedItems.length; i++) {
            _.findWhere(item.Leaves, { PropertyName: selectedItems[i].value }).GroupDescriptor.GroupByOrder = ++maxGroupOrder;
        }
        refreshGroupList(item);
        var temp = [];
        for (i = 0; i < selectedItems.length; i++) {
            temp[i] = selectedItems[i].value;
        }
        $("#selectedGroupList").val(temp);
    });

    $("#addAllGroups").click(function () {
        var item = SelectedTreeNode;
        var maxGroupOrder = _.max(item.Leaves, function (leaf) { return leaf.GroupDescriptor.GroupByOrder; }).GroupDescriptor.GroupByOrder;
        var selectedItems = $("#groupList  option");
        for (var i = 0; i < selectedItems.length; i++) {
            _.findWhere(item.Leaves, { PropertyName: selectedItems[i].value }).GroupDescriptor.GroupByOrder = ++maxGroupOrder;
        }
        refreshGroupList(item);
    });

    $("#moveGroupUp").click(function () {
        var item = SelectedTreeNode;
        var selectedItems = $("#selectedGroupList  :selected");
        for (var i = 0; i < selectedItems.length; i++) {
            var leaf = _.findWhere(item.Leaves, { PropertyName: selectedItems[i].value });
            if (leaf.GroupDescriptor.GroupByOrder == 1) {
                continue;
            }
            var prevLeaf;
            for (var j = 0; j < item.Leaves.length; j++) {
                if (item.Leaves[j].GroupDescriptor.GroupByOrder == leaf.GroupDescriptor.GroupByOrder - 1) {
                    prevLeaf = item.Leaves[j];
                    break;
                }
            }
            leaf.GroupDescriptor.GroupByOrder--;
            prevLeaf.GroupDescriptor.GroupByOrder++;

        }
        refreshGroupList(item);
        var temp = [];
        for (i = 0; i < selectedItems.length; i++) {
            temp[i] = selectedItems[i].value;
        }
        $("#selectedGroupList").val(temp);
    });

    $("#moveGroupDown").click(function () {
        var item = SelectedTreeNode;
        var maxSelected = _.max(item.Leaves, function (l) { return l.GroupDescriptor.GroupByOrder; }).GroupDescriptor.GroupByOrder;
        var selectedItems = $("#selectedGroupList  :selected");
        for (var i = 0; i < selectedItems.length; i++) {

            var leaf = _.findWhere(item.Leaves, { PropertyName: selectedItems[selectedItems.length - (i + 1)].value });
            if (leaf.GroupDescriptor.GroupByOrder == maxSelected) {
                continue;
            }
            var prevLeaf;
            for (var j = 0; j < item.Leaves.length; j++) {
                if (item.Leaves[j].GroupDescriptor.GroupByOrder == leaf.GroupDescriptor.GroupByOrder + 1) {
                    prevLeaf = item.Leaves[j];
                    break;
                }
            }
            leaf.GroupDescriptor.GroupByOrder++;
            prevLeaf.GroupDescriptor.GroupByOrder--;
        }

        refreshGroupList(item);
        var temp = [];
        for (i = 0; i < selectedItems.length; i++) {
            temp[i] = selectedItems[i].value;
        }
        $("#selectedGroupList").val(temp);
    });

    $("#removeAllSorts").click(function () {
        var item = SelectedTreeNode;
        item.Leaves.forEach(function (leaf) {
            leaf.SortDescriptor.SortOrder = 0;
            leaf.SortDescriptor.SortDirection = "";
        });
        refreshSortList(item);
    });

    $("#removeSort").click(function () {
        var item = SelectedTreeNode;
        var selectedItems = $("#selectedSortList  :selected");
        for (var i = 0; i < selectedItems.length; i++) {
            _.findWhere(item.Leaves, { PropertyName: selectedItems[i].value }).SortDescriptor.SortOrder = 0;
            _.findWhere(item.Leaves, { PropertyName: selectedItems[i].value }).SortDescriptor.SortDirection = "";
        }
        var ordreList = _.sortBy(item.Leaves, function (leaf) { return leaf.SortDescriptor.SortOrder; });
        var count = 1;
        for (var j = 0; j < ordreList.length; j++) {
            if (ordreList[j].SortDescriptor.SortOrder != 0)
                ordreList[j].SortDescriptor.SortOrder = count++;
        }
        refreshSortList(item);
        var temp = [];
        for (i = 0; i < selectedItems.length; i++) {
            temp[i] = selectedItems[i].value;
        }
        $("#sortList").val(temp);
    });

    $("#addSort").click(function () {
        var item = SelectedTreeNode;
        var maxSortOrder = _.max(item.Leaves, function (leaf) { return leaf.SortDescriptor.SortOrder; }).SortDescriptor.SortOrder;
        var selectedItems = $("#sortList  :selected");
        for (var i = 0; i < selectedItems.length; i++) {
            _.findWhere(item.Leaves, { PropertyName: selectedItems[i].value }).SortDescriptor.SortOrder = ++maxSortOrder;
            _.findWhere(item.Leaves, { PropertyName: selectedItems[i].value }).SortDescriptor.SortDirection = "Asc";
        }
        refreshSortList(item);
        var temp = [];
        for (i = 0; i < selectedItems.length; i++) {
            temp[i] = selectedItems[i].value;
        }
        $("#selectedSortList").val(temp);
    });

    $("#addAllSorts").click(function () {
        var item = SelectedTreeNode;
        var maxSortOrder = _.max(item.Leaves, function (leaf) { return leaf.SortDescriptor.SortOrder; }).SortDescriptor.SortOrder;
        var selectedItems = $("#sortList  option");
        for (var i = 0; i < selectedItems.length; i++) {
            _.findWhere(item.Leaves, { PropertyName: selectedItems[i].value }).SortDescriptor.SortOrder = ++maxSortOrder;
            _.findWhere(item.Leaves, { PropertyName: selectedItems[i].value }).SortDescriptor.SortDirection = "Asc";
        }
        refreshSortList(item);
    });

    $("#moveSortUp").click(function () {
        var item = SelectedTreeNode;
        var selectedItems = $("#selectedSortList  :selected");
        for (var i = 0; i < selectedItems.length; i++) {
            var leaf = _.findWhere(item.Leaves, { PropertyName: selectedItems[i].value });
            if (leaf.SortDescriptor.SortOrder == 1) {
                continue;
            }
            var prevLeaf;
            for (var j = 0; j < item.Leaves.length; j++) {
                if (item.Leaves[j].SortDescriptor.SortOrder == leaf.SortDescriptor.SortOrder - 1) {
                    prevLeaf = item.Leaves[j];
                    break;
                }
            }
            leaf.SortDescriptor.SortOrder--;
            prevLeaf.SortDescriptor.SortOrder++;

        }
        refreshSortList(item);
        var temp = [];
        for (i = 0; i < selectedItems.length; i++) {
            temp[i] = selectedItems[i].value;
        }
        $("#selectedSortList").val(temp);
    });

    $("#moveSortDown").click(function () {
        var item = SelectedTreeNode;
        var maxSelected = _.max(item.Leaves, function (l) { return l.SortDescriptor.SortOrder; }).SortDescriptor.SortOrder;
        var selectedItems = $("#selectedSortList  :selected");
        for (var i = 0; i < selectedItems.length; i++) {

            var leaf = _.findWhere(item.Leaves, { PropertyName: selectedItems[selectedItems.length - (i + 1)].value });
            if (leaf.SortDescriptor.SortOrder == maxSelected) {
                continue;
            }
            var prevLeaf;
            for (var j = 0; j < item.Leaves.length; j++) {
                if (item.Leaves[j].SortDescriptor.SortOrder == leaf.SortDescriptor.SortOrder + 1) {
                    prevLeaf = item.Leaves[j];
                    break;
                }
            }
            leaf.SortDescriptor.SortOrder++;
            prevLeaf.SortDescriptor.SortOrder--;
        }

        refreshSortList(item);
        var temp = [];
        for (i = 0; i < selectedItems.length; i++) {
            temp[i] = selectedItems[i].value;
        }
        $("#selectedSortList").val(temp);
    });

    $("#_sortDirection").click(function () {
        var item = SelectedTreeNode;
        var selectedItems = $("#selectedSortList  :selected");
        for (var i = 0; i < selectedItems.length; i++) {
            var temp = _.findWhere(item.Leaves, { PropertyName: selectedItems[i].value });
            if (temp.SortDescriptor.SortDirection == "Asc") {
                temp.SortDescriptor.SortDirection = "Desc";
            } else {
                temp.SortDescriptor.SortDirection = "Asc";
            }
        }
        refreshSortList(item);
        var temp = [];
        for (i = 0; i < selectedItems.length; i++) {
            temp[i] = selectedItems[i].value;
        }
        $("#selectedSortList").val(temp);
    });
});

function refreshSelectList(item) {
    var template = kendo.template($("#fieldListTemplate").html());
    var ordreList = _.sortBy(item.Leaves, function (leaf) { return leaf.Selected; });
    $("#fields").html(template(ordreList));
    template = kendo.template($("#selectedFieldTemplate").html());
    $("#selectedFields").html(template(ordreList));
}

function refreshGroupList(item) {
    var template = kendo.template($("#groupListTemplate").html());
    var ordreList = _.sortBy(item.Leaves, function (leaf) { return leaf.GroupDescriptor.GroupByOrder; });
    $("#groups").html(template(ordreList));
    template = kendo.template($("#selectedGroupTemplate").html());
    $("#selectedGroups").html(template(ordreList));
}

function refreshSortList(item) {
    var template = kendo.template($("#sortListTemplate").html());
    var ordreList = _.sortBy(item.Leaves, function (leaf) { return leaf.SortDescriptor.SortOrder; });
    $("#sorts").html(template(ordreList));
    template = kendo.template($("#selectedSortTemplate").html());
    $("#selectedSorts").html(template(ordreList));
}

function onSelectNode(e) {
    SelectedTreeNode = this.dataItem(e.node);
    if (TreeNodeSelectedFirstTime == 1) {
        TreeNodeSelectedFirstTime = 0;
        SelectedTreeNode.Nodes = Report.QueryTree.Nodes;
        return;
    }
    var item = SelectedTreeNode;
    refreshSelectList(item);
    refreshGroupList(item);
    refreshSortList(item);
    $("#tabstrip").show();

    IntializeFiltersDataSource(item);
    IntializeAggregateFiltersDataSource(item);
}

function GetFilterFieldNameById(id) {
    for (var i = 0; i < CurrentFiltersFieldsNames.length; i++) {
        if (CurrentFiltersFieldsNames[i].value == id) {
            return CurrentFiltersFieldsNames[i].text;
        }
    }
    return id;
}

function GetFilterOperatorNameById(id) {
    for (var i = 0; i < FiltersStringOperators.length; i++) {
        if (FiltersStringOperators[i].value == id) {
            return FiltersStringOperators[i].text;
        }
    }

    for (var x = 0; x < FiltersNumericAndDateOperators.length; x++) {
        if (FiltersNumericAndDateOperators[x].value == id) {
            return FiltersNumericAndDateOperators[x].text;
        }
    }

    return id;
}

function GetAggregateFilterFieldNameById(id) {
    for (var i = 0; i < CurrentAggregateFiltersFieldsNames.length; i++) {
        if (CurrentAggregateFiltersFieldsNames[i].value == id) {
            return CurrentAggregateFiltersFieldsNames[i].text;
        }
    }
    return id;
}

function GetAggregateFilterOperatorNameById(id) {
    for (var i = 0; i < AggregateFilterOperators.length; i++) {
        if (AggregateFilterOperators[i].value == id) {
            return AggregateFilterOperators[i].text;
        }
    }
    return id;
}

function GetAggregateFunctionsNameById(id) {
    for (var i = 0; i < AggregateFilterFunctions.length; i++) {
        if (AggregateFilterFunctions[i].value == id) {
            return AggregateFilterFunctions[i].text;
        }
    }
    return id;
}

function IntializeFiltersDataSource(selectedTreeNodeData) {
    debugger;
    var nodeFiltersDataSource = new Array();
    CurrentFiltersFieldsNames = new Array();
    FielterIdCounter = 0;
    IsFilterAddMode = 0;

    var type = "";
    for (var i = 0; i < selectedTreeNodeData.Leaves.length; i++) {
        if (selectedTreeNodeData.Leaves[i].PropertyType.indexOf("String") != -1) {
            type = "String";
        }
        else if (selectedTreeNodeData.Leaves[i].PropertyType.indexOf("Int") != -1) {
            type = "Integer";
        }
        else if (selectedTreeNodeData.Leaves[i].PropertyType.indexOf("DateTime") != -1) {
            type = "DateTime";
        }

        var fieldName = {
            text: selectedTreeNodeData.Leaves[i].DisplayName,
            value: selectedTreeNodeData.Leaves[i].PropertyFullPath + "-" + type
        };
        CurrentFiltersFieldsNames.push(fieldName);

        for (var x = 0; x < selectedTreeNodeData.Leaves[i].FilterDescriptors.length; x++) {
            var filter = {
                "ID": ++FielterIdCounter,
                "Field": selectedTreeNodeData.Leaves[i].PropertyFullPath + "-" + type,
                "Operator": selectedTreeNodeData.Leaves[i].FilterDescriptors[x].FilterOperator,
                "Value": type == "DateTime" ? kendo.toString(new Date(selectedTreeNodeData.Leaves[i].FilterDescriptors[x].Value), dateFormat) : selectedTreeNodeData.Leaves[i].FilterDescriptors[x].Value,
                "Type": type
            };
            nodeFiltersDataSource.push(filter);
        }
    }


    dataSource = new kendo.data.DataSource({
        data: nodeFiltersDataSource,
        pageSize: 4,
        schema: {
            model: {
                id: "ID",
                fields: {
                    ID: { type: "string", editable: false, nullable: true },
                    Field: { type: "string" },
                    Operator: { type: "string" },
                    Value: { type: "string" },
                    Type: { type: "string" }
                }
            }
        }
    });

    if ($("#filtersPager").data("kendoPager") != null) {
        $("#filtersPager").data("kendoPager").setDataSource(dataSource);
        
        $("#filtersListView").data("kendoListView").setDataSource(dataSource);
    } else {
        $("#filtersPager").kendoPager({
            dataSource: dataSource
        });

        var listView = $("#filtersListView").kendoListView({
            dataSource: dataSource,
            editable: true,
            template: kendo.template($("#filtersDisplayModeTemplate").html()),
            editTemplate: kendo.template($("#filtersEditModeTemplate").html()),
            edit: function (e) {
                OriginalEditedFilterModel = jQuery.extend({}, e.model);
                if (e.model.ID == null) {
                    IsFilterAddMode = 1;
                } else {
                    IsFilterAddMode = 0;
                }
                FilterFieldName_Changed();
            },
            save: function (e) {
                FilterFieldName_Save(e);
            },
            remove: function (e) {
                FilterFieldName_Remove(e);
            }
        }).data("kendoListView");


        $(".k-add-button").click(function (e) {
            listView.add();
            e.preventDefault();
        });
    }
}

function IntializeAggregateFiltersDataSource(selectedTreeNodeData) {
    var nodeAggregateFiltersDataSource = new Array();
    CurrentAggregateFiltersFieldsNames = new Array();
    AggregateFielterIdCounter = 0;
    IsAggregateFilterAddMode = 0;

    for (var i = 0; i < selectedTreeNodeData.Nodes.length; i++) {
        var fieldName = {
            text: selectedTreeNodeData.Nodes[i].DisplayName,
            value: selectedTreeNodeData.Nodes[i].PropertyName
        };
        CurrentAggregateFiltersFieldsNames.push(fieldName);
    }

    for (var x = 0; x < selectedTreeNodeData.AggregateFilters.length; x++) {
        var filter = {
            "ID": ++AggregateFielterIdCounter,
            "Field": selectedTreeNodeData.AggregateFilters[x].PropertyName,
            "AggregateFunction": selectedTreeNodeData.AggregateFilters[x].AggregateFunction,
            "Operator": selectedTreeNodeData.AggregateFilters[x].FilterOperator,
            "Value": selectedTreeNodeData.AggregateFilters[x].Value
        };
        nodeAggregateFiltersDataSource.push(filter);
    }


    dataSource = new kendo.data.DataSource({
        data: nodeAggregateFiltersDataSource,
        pageSize: 4,
        schema: {
            model: {
                id: "ID",
                fields: {
                    ID: { type: "string", editable: false, nullable: true },
                    Field: { type: "string" },
                    AggregateFunction: { type: "string" },
                    Operator: { type: "string" },
                    Value: { type: "string" }
                }
            }
        }
    });

    if ($("#aggregatesPager").data("kendoPager") != null) {
        $("#aggregatesPager").data("kendoPager").setDataSource(dataSource);
        $("#aggregatesListView").data("kendoListView").setDataSource(dataSource);
    } else {
        $("#aggregatesPager").kendoPager({
            dataSource: dataSource
        });

        var listView = $("#aggregatesListView").kendoListView({
            dataSource: dataSource,
            editable: true,
            template: kendo.template($("#aggregateFilterDisplayModeTemplate").html()),
            editTemplate: kendo.template($("#aggregateFilterEditModeTemplate").html()),
            edit: function (e) {
                OriginalEditedAggregateFilterModel = jQuery.extend({}, e.model);
                if (e.model.ID == null) {
                    IsAggregateFilterAddMode = 1;
                } else {
                    IsAggregateFilterAddMode = 0;
                }
            },
            save: function (e) {
                AggregateFilterFieldName_Save(e);
            },
            remove: function (e) {
                AggregateFilterFieldName_Remove(e);
            }
        }).data("kendoListView");


        $(".k-add-button").click(function (e) {
            listView.add();
            e.preventDefault();
        });
    }
}

function FilterFieldChanged() {

    var fieldType = $("#FilterFieldName").data("kendoDropDownList").value().split("-")[1];
    if (fieldType == "String") {
        $("#FilterFieldOperator").data("kendoDropDownList").setDataSource(FiltersStringOperators);
    }
    else
        if (fieldType == "Integer") {
            $("#FilterFieldValue").kendoNumericTextBox();
            $("#FilterFieldOperator").data("kendoDropDownList").setDataSource(FiltersNumericAndDateOperators);
        }
        else
            if (fieldType == "DateTime") {
                $("#FilterFieldValue").kendoDatePicker({ format: dateFormat });
                $("#FilterFieldOperator").data("kendoDropDownList").setDataSource(FiltersNumericAndDateOperators);
            }
}

function FilterFieldName_Changed() {
    FilterFieldChanged();
    $("#FilterFieldName").bind("change", function () {
        $("#FilterFieldValue").parents("dd")
            .html("<input type='text' class='k-textbox' data-bind='value:Value' id='FilterFieldValue' required='required' validationMessage='required' /><span data-for='FilterFieldValue' class='k-invalid-msg'></span>");
        FilterFieldChanged();
    });
}

function FilterFieldName_Save(e) {
    FielterIdCounter++;

    e.model.ID = IsFilterAddMode == 1 ? FielterIdCounter : e.model.ID;
    e.model.Type = $("#FilterFieldName").data("kendoDropDownList").value().split("-")[1];
    e.model.Field = $("#FilterFieldName").data("kendoDropDownList").value();
    e.model.Operator = $("#FilterFieldOperator").data("kendoDropDownList").value();
    e.model.Value = $("#FilterFieldValue").val();

    SaveFielterChangesToTreeDataSource(e);
}

function FilterFieldName_Remove(e) {
    var item = SelectedTreeNode;
    item.Leaves.forEach(function (leaf) {
        if (leaf.PropertyFullPath == e.model.Field.substring(0, e.model.Field.indexOf("-"))) {

            leaf.FilterDescriptors = $.grep(leaf.FilterDescriptors, function (par) {
                return par.FilterOperator != e.model.Operator || par.Value != e.model.Value;
            });
        }
    });
}

function SaveFielterChangesToTreeDataSource(e) {
    var item = SelectedTreeNode;

    if (OriginalEditedFilterModel.Field != e.model.Field && IsFilterAddMode == 0) {
        item.Leaves.forEach(function (leaf) {
            if (leaf.PropertyFullPath == OriginalEditedFilterModel.Field.substring(0, OriginalEditedFilterModel.Field.indexOf("-"))) {

                leaf.FilterDescriptors = $.grep(leaf.FilterDescriptors, function (par) {
                    return par.FilterOperator != OriginalEditedFilterModel.Operator || par.Value != OriginalEditedFilterModel.Value;
                });
            }
            else if (leaf.PropertyFullPath == e.model.Field.substring(0, e.model.Field.indexOf("-"))) {
                var filter = {
                    "FilterOperator": e.model.Operator,
                    "Value": e.model.Value,
                };
                leaf.FilterDescriptors[leaf.FilterDescriptors.length] = filter;
                leaf.FilterDescriptors.length++;
            }
        });
    } else {
        item.Leaves.forEach(function (leaf) {
            if (leaf.PropertyFullPath == e.model.Field.substring(0, e.model.Field.indexOf("-"))) {
                if (IsFilterAddMode == 1) {
                    var filter = {
                        "FilterOperator": e.model.Operator,
                        "Value": e.model.Value,
                    };
                    leaf.FilterDescriptors[leaf.FilterDescriptors.length] = filter;
                    leaf.FilterDescriptors.length++;
                } else {
                    leaf.FilterDescriptors.forEach(function (filterDescriptor) {
                        if (OriginalEditedFilterModel.Operator == filterDescriptor.FilterOperator &&
                            OriginalEditedFilterModel.Value == filterDescriptor.Value) {
                            filterDescriptor.FilterOperator = e.model.Operator;
                            filterDescriptor.Value = e.model.Value;
                        }
                    });
                }
            }
        });
    }
}

function AggregateFilterFieldName_Save(e) {
    AggregateFielterIdCounter++;
    e.model.ID = IsAggregateFilterAddMode == 1 ? FielterIdCounter : e.model.ID;
    e.model.Field = $("#AggregateFieldName").data("kendoDropDownList").value();
    e.model.AggregateFunction = $("#AggregateFunction").data("kendoDropDownList").value();
    e.model.Operator = $("#AggregateFieldOperator").data("kendoDropDownList").value();
    e.model.Value = $("#AggregateFieldValue").val();

    SaveAggregateFielterChangesToTreeDataSource(e);
}

function SaveAggregateFielterChangesToTreeDataSource(e) {
    var item = SelectedTreeNode;

    if (OriginalEditedAggregateFilterModel.Field != e.model.Field && IsAggregateFilterAddMode == 0) {
        item.AggregateFilters = $.grep(item.AggregateFilters, function (par) {
            return !(par.PropertyName == OriginalEditedAggregateFilterModel.Field && par.AggregateFunction == OriginalEditedAggregateFilterModel.AggregateFunction &&
                par.FilterOperator == OriginalEditedAggregateFilterModel.Operator && par.Value == OriginalEditedAggregateFilterModel.Value);
        });
        var newFilter = {
            "PropertyName": e.model.Field,
            "FilterOperator": e.model.Operator,
            "AggregateFunction": e.model.AggregateFunction,
            "Value": e.model.Value,
        };
        item.AggregateFilters[item.AggregateFilters.length] = newFilter;
    } else if (IsAggregateFilterAddMode == 1) {
        var filter = {
            "PropertyName": e.model.Field,
            "FilterOperator": e.model.Operator,
            "AggregateFunction": e.model.AggregateFunction,
            "Value": e.model.Value,
        };
        item.AggregateFilters[item.AggregateFilters.length] = filter;
        item.AggregateFilters.length++;
    } else {
        item.AggregateFilters.forEach(function (aggregateFilter) {
            if (OriginalEditedAggregateFilterModel.Field == aggregateFilter.PropertyName &&
                OriginalEditedAggregateFilterModel.AggregateFunction == aggregateFilter.AggregateFunction &&
                OriginalEditedAggregateFilterModel.Operator == aggregateFilter.FilterOperator &&
                OriginalEditedAggregateFilterModel.Value == aggregateFilter.Value) {
                aggregateFilter.PropertyName = e.model.Field;
                aggregateFilter.FilterOperator = e.model.Operator;
                aggregateFilter.AggregateFunction = e.model.AggregateFunction;
                aggregateFilter.Value = e.model.Value;
            }
        });
    }
}

function AggregateFilterFieldName_Remove(e) {
    var item = SelectedTreeNode;
    item.AggregateFilters = $.grep(item.AggregateFilters, function (par) {
        return !(par.PropertyName == e.model.Field && par.AggregateFunction == e.model.AggregateFunction &&
            par.FilterOperator == e.model.Operator && par.Value == e.model.Value);
    });
}