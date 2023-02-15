using Souccar.ReportGenerator.Domain.QueryBuilder;
using Syncfusion.RDL.DOM;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Linq;
using Souccar.Infrastructure.Core;
using System.Text.RegularExpressions;
using Reporting.Extensions;
using System.Collections.Generic;

namespace Reporting.RDL
{
    public class RdlReport
    {
        private Syncfusion.RDL.DOM.ReportDefinition _report;
        private Syncfusion.RDL.DOM.DataSource _dataSource;
        private Syncfusion.RDL.DOM.DataSet _dataSet;
        //private Syncfusion.RDL.DOM.ReportParameter _reportParameter;
        private QueryTree _queryTree;
        private string _connectionString;
        private string _reportPath;
        private int _reportId;

        public RdlReport(Report report, string reportPath, string connectionString)
        {
            _reportId = report.Id;
            _queryTree = report.QueryTreesList.FirstOrDefault();
            _connectionString = connectionString;
            _reportPath = reportPath;

            _report = new ReportDefinition();
            _report.DataSources = new DataSources();
            _report.DataSets = new DataSets();
            _report.ReportSections = new ReportSections();
            _report.ReportParameters = new ReportParameters();

            _dataSource = new Syncfusion.RDL.DOM.DataSource();
            _dataSet = new Syncfusion.RDL.DOM.DataSet();
        }

        public void Create()
        {
            InitialReport();
            SaveSerialize();
        }

        private void InitialReport()
        {
            //Data Source
            var dataSource = CreateDataSource();
            _report.DataSources.Add(dataSource);

            //Data Set
            var dataSets = CreateDataSet(dataSource);
            _report.DataSets.AddRange(dataSets);
            _dataSet = dataSets.FirstOrDefault(x => x.Name == "MainDataSet");

            //ReportParameters
            var parameters = CreateReportParameter();
            _report.ReportParameters.AddRange(parameters);

            //Report Parameters Layout
            _report.ReportParametersLayout = CreateReportParameterLayout(_queryTree);

            //Sections
            _report.ReportSections = new ReportSections();
            var reportSection = new ReportSection();
            _report.ReportSections.Add(reportSection);
            reportSection.Body = CreateBody();


            reportSection.Page = new Syncfusion.RDL.DOM.Page();
            //reportSection.Page.PageHeader = CreateHeader();
            //reportSection.Page.PageFooter = CreateFooter();
            //reportSection.Page.Style = AddStyle();
            reportSection.Page.PageHeight = "5in";
            var pageWidth = GetPageWidth(_queryTree);
            reportSection.Page.PageWidth = pageWidth + "in";
            reportSection.Page.LeftMargin = "0.5in";
            reportSection.Page.RightMargin = "0.5in";
            reportSection.Page.TopMargin = "0.5in";
            reportSection.Page.BottomMargin = "0.5in";

            reportSection.Width = pageWidth + "in";
            _report.ReportUnitType = "Inch";
            _report.RDLType = RDLType.RDL2016;
        }

        private Syncfusion.RDL.DOM.ReportParametersLayout CreateReportParameterLayout(QueryTree queryTree)
        {
            Syncfusion.RDL.DOM.CellDefinitions cellDefs = new CellDefinitions();
            var cellDefinitions = GetCellDefinitions(_queryTree, cellDefs, 0, 0);

            int rowIndex = 0, columnIndex = 0;

            foreach (var cellDef in cellDefinitions)
            {
                cellDef.RowIndex = rowIndex;
                cellDef.ColumnIndex = columnIndex;
                if (columnIndex >= 3)
                {
                    columnIndex = 0;
                    rowIndex++;
                }
                else
                    columnIndex++;
            }


            var RepParamLayout = new ReportParametersLayout()
            {
                GridLayoutDefinition = new GridLayoutDefinition()
                {
                    NumberOfRows = GetRowsAndColumnsNumber(true),
                    NumberOfColumns = GetRowsAndColumnsNumber(false),
                    CellDefinitions = cellDefs
                },
            };
            return RepParamLayout;
        }
        private Syncfusion.RDL.DOM.CellDefinitions GetCellDefinitions(QueryTree queryTree, CellDefinitions cellDefs, int rowIndex, int columnIndex)
        {

            foreach (var leave in queryTree.Leaves.Where(x => x.IsSelected && x.HasFilters))
            {
                var cellDef = new CellDefinition()
                {
                    RowIndex = rowIndex,
                    ColumnIndex = columnIndex,
                    ParameterName = leave.PropertyName
                };
                cellDefs.Add(cellDef);
                //if (columnIndex >= 3)
                //{
                //    columnIndex = 0;
                //    rowIndex++;
                //}
                //else
                //    columnIndex++;
            }
            foreach (var node in queryTree.Nodes.Where(x => x.HasSelectedFields && x.HasFilters))
            {
                GetCellDefinitions(node, cellDefs, rowIndex, columnIndex);
            }

            return cellDefs;
        }

        private int GetRowsAndColumnsNumber(bool isRow)
        {
            if (isRow)
            { return 2; }
            TablixMembers tablixMembers = new TablixMembers();
            return GetColumnHierarchyMembers(tablixMembers, _queryTree).Count;
        }

        private Syncfusion.RDL.DOM.DataSource CreateDataSource()
        {
            return new RdlDataSource(_connectionString).Create();
        }

        private Syncfusion.RDL.DOM.DataSets CreateDataSet(Syncfusion.RDL.DOM.DataSource dataSource)
        {
            return new RdlDataSet(dataSource, _queryTree).Create();
        }

        private Syncfusion.RDL.DOM.ReportParameters CreateReportParameter()
        {
            return new RdlParameter(_queryTree).Create(_queryTree);
        }

        Body CreateBody()
        {
            var body = new Body();
            body.Height = new Syncfusion.RDL.DOM.Size("2.03333in");
            //body.Style = AddStyle();
            body.ReportItems = new ReportItems();
            var textBox = CreateTextBox("TextBoxBodyTitle", "Body Title", "1in", "0in", "6.27in", "0.5in");
            body.ReportItems.Add(textBox);

            body.ReportItems.Add(CreateTable());

            return body;
        }

        private Tablix CreateTable()
        {
            var table = new Tablix();
            table.Name = _queryTree.GetTableName() + "Table";
            table.DataSetName = _dataSet.Name;
            table.Top = "1in";
            table.Left = "1in";
            table.Height = "0.5in";
            table.Width = GetPageWidth(_queryTree) + "in";
            table.LayoutDirection = Direction.LTR;

            #region Body
            var tablixBody = new TablixBody();
            //  tablixBody.TablixColumns = CreateTableColumns(_queryTree);

            ///Edits Amer And Walaa
            TablixColumns tabCols = new TablixColumns();
            tablixBody.TablixColumns = CreateChildTablixColumns(tabCols, _queryTree);
            //Endd


            var tablixRows = new TablixRows();
            tablixBody.TablixRows = GetTablixRows(tablixRows, _queryTree);

            table.TablixBody = tablixBody;
            #endregion

            #region Tablix Hierarchy

            //Column
            table.TablixColumnHierarchy = GetColumnHierarchy(_queryTree);
            //Row
            table.TablixRowHierarchy = GetRowHierarchy(_queryTree);

            //var tablixRowHierarchy = new TablixRowHierarchy();
            //tablixRowHierarchy.TablixMembers = new TablixMembers();
            //tablixRowHierarchy.TablixMembers.Add(new TablixMember()
            //{
            //    KeepWithGroup = KeepWithGroup.After,
            //    KeepTogether = true
            //});
            //tablixRowHierarchy.TablixMembers.Add(new TablixMember()
            //{
            //    DataElementName = "Detail_Collection",
            //    DataElementOutput = DataElementOutputs.Output,
            //    KeepTogether = true,
            //    Group = new Syncfusion.RDL.DOM.Group()
            //    {
            //        Name = "Table1_Details_Group",
            //        DataElementName = "Detail",
            //        GroupExpressions = GetGroupExpressions("=Fields!EmployeeId.Value")
            //    },
            //    TablixMembers = CreateChildTablixMember()
            //});

            //table.TablixRowHierarchy = tablixRowHierarchy;
            #endregion
            return table;
        }

        private TablixRowHierarchy GetRowHierarchy(QueryTree queryTree)
        {
            var tablixRowHierarchy = new TablixRowHierarchy();
            tablixRowHierarchy.TablixMembers = GetRowHierarchyMembers(queryTree);

        //    tablixRowHierarchy.TablixMembers.AddRange(GetGroupMembers(queryTree));

            return tablixRowHierarchy;
        }

        private TablixMembers GetRowHierarchyMembers(QueryTree queryTree)
        {
            var rowTablixMembers = new TablixMembers();
            //rowTablixMembers.Add(new TablixMember()
            //{
            //    KeepWithGroup = KeepWithGroup.After,
            //    //KeepTogether = true
            //});
            //rowTablixMembers.Add(new TablixMember()
            //{
            //    Group = new Syncfusion.RDL.DOM.Group()
            //    {
            //        Name = "Detail"
            //    }
            //});
            var leaves = queryTree.Leaves.Where(x => x.IsSelected && x.GroupDescriptor.GroupByOrder != 0);

            if (leaves != null && leaves.Any())
            {

                //List
                foreach (QueryLeaf leave in leaves)
                {

                    rowTablixMembers.Add(new TablixMember()
                    {
                        KeepWithGroup = KeepWithGroup.After,
                        KeepTogether = true
                    });
                    var name = leave.DisplayName;
                    var parent = queryTree.GetTableName();

                    TablixMembers Childrens = new TablixMembers();

                    //Childrens.Add(new TablixMember()
                    //{
                    //    Group = new Syncfusion.RDL.DOM.Group()
                    //    {
                    //        Name = "Detail"
                    //    }
                    //});

                    Childrens.Add(new TablixMember()
                    {
                        //DataElementName = name + "_Collection",
                        //DataElementOutput = DataElementOutputs.Output,
                        //KeepTogether = true,

                        Group = new Syncfusion.RDL.DOM.Group()
                        {
                            Name = "Detail",
                            DataElementName = "Detail",
                            //GroupExpressions = GetGroupExpressions($"=Fields!{name}.Value")
                        },
                        // TablixMembers = GetChildRowHierarchyMembers(leave.QueryTree)
                    });

                    rowTablixMembers.Add(new TablixMember()
                    {
                        DataElementName = name + "_Collection",
                        DataElementOutput = DataElementOutputs.Output,
                        KeepTogether = true,
                        Group = new Syncfusion.RDL.DOM.Group()
                        {
                            Name = name + "_Details_Group",
                            DataElementName = name + "Detail",
                            GroupExpressions = GetGroupExpressions($"=Fields!{parent}{name}.Value")
                        },
                        TablixMembers = Childrens,
                    });

                }

            }

            //var name = queryTree.GetTableName();
            //rowTablixMembers.Add(new TablixMember()
            //{
            //    DataElementName = name + "_Collection",
            //    DataElementOutput = DataElementOutputs.Output,
            //    KeepTogether = true,
            //    Group = new Syncfusion.RDL.DOM.Group()
            //    {
            //        Name = name + "_Details_Group",
            //        DataElementName = queryTree.GetTableName() + "Detail",
            //        GroupExpressions = GetGroupExpressions($"=Fields!{name}Id.Value")
            //    },
            //    TablixMembers = GetChildRowHierarchyMembers(queryTree)
            //});

            return rowTablixMembers;
        }

        private TablixMembers GetGroupMembers(QueryTree queryTree)
        {
            var rowTablixMembers = new TablixMembers();

            var leaves = queryTree.Leaves.Where(x => x.IsSelected && x.GroupDescriptor.GroupByOrder != 0);

            if (leaves != null && leaves.Any())
            {
                rowTablixMembers.Add(new TablixMember());
                rowTablixMembers.Add(new TablixMember());

                //List
                foreach (QueryLeaf leave in leaves)
                {
                    
                        rowTablixMembers.Add(new TablixMember()
                        {
                            KeepWithGroup = KeepWithGroup.After,
                            KeepTogether = true
                        });
                         var name = leave.DisplayName;
                         var parent = queryTree.GetTableName();

                    rowTablixMembers.Add(new TablixMember()
                        {
                            DataElementName = name + "_Collection",
                            DataElementOutput = DataElementOutputs.Output,
                            KeepTogether = true,
                            Group = new Syncfusion.RDL.DOM.Group()
                            {
                                Name = name + "_Details_Group",
                                DataElementName = name + "Detail",
                                GroupExpressions = GetGroupExpressions($"=Fields!{parent}{name}.Value")
                            },
                            TablixMembers = GetChildRowHierarchyMembers(leave.QueryTree)
                        });
                    
                }

            }
            return rowTablixMembers;
        }

        private TablixMembers GetChildRowHierarchyMembers(QueryTree queryTree)
        {
            var rowTablixMembers = new TablixMembers();

            var nodes = queryTree.Nodes.Where(x => x.HasSelectedFields).OrderBy(x => x.SelectOrder)
                .Where(x => queryTree.Type.GetProperty(x.DisplayName).PropertyType.GetInterface("IEnumerable") != null);

            if (nodes != null && nodes.Any())
            {
                rowTablixMembers.Add(new TablixMember());
                rowTablixMembers.Add(new TablixMember());

                //List
                foreach (QueryTree node in queryTree.Nodes.Where(x => x.HasSelectedFields).OrderBy(x => x.SelectOrder))
                {
                    if (queryTree.Type.GetProperty(node.DisplayName)
                       .PropertyType
                       .GetInterface("IEnumerable") != null)
                    {
                        rowTablixMembers.Add(new TablixMember()
                        {
                            //KeepWithGroup = KeepWithGroup.After,
                            KeepTogether = true
                        });
                        var name = node.GetTableName();

                        rowTablixMembers.Add(new TablixMember()
                        {
                            DataElementName = name + "_Collection",
                            DataElementOutput = DataElementOutputs.Output,
                            KeepTogether = true,
                            Group = new Syncfusion.RDL.DOM.Group()
                            {
                                Name = name + "_Details_Group",
                                DataElementName = name + "Detail",
                                GroupExpressions = GetGroupExpressions($"=Fields!{name}Id.Value")
                            },
                            TablixMembers = GetChildRowHierarchyMembers(node)
                        });
                    }
                }

            }
            return rowTablixMembers;
        }

        private TablixColumnHierarchy GetColumnHierarchy(QueryTree queryTree)
        {
            var tablixColumnHierarchy = new TablixColumnHierarchy();
            var columnTablixMembers = new TablixMembers();
            tablixColumnHierarchy.TablixMembers = GetColumnHierarchyMembers(columnTablixMembers, queryTree);
            return tablixColumnHierarchy;
        }

        private TablixMembers GetColumnHierarchyMembers(TablixMembers columnTablixMembers, QueryTree queryTree)
        {
            //var columnTablixMembers = new TablixMembers();
            foreach (var leaf in queryTree.Leaves.Where(x => x.IsSelected))
            {
                columnTablixMembers.Add(new TablixMember());
            }
            foreach (QueryTree supQueryTree in queryTree.Nodes.Where(x => x.HasSelectedFields))
            {
                GetColumnHierarchyMembers(columnTablixMembers, supQueryTree);
            }

            return columnTablixMembers;
        }

        private GroupExpressions GetGroupExpressions(string v)
        {
            var groupExpressions = new GroupExpressions();

            var groupExpression = new GroupExpression()
            {
                Value = v
            };

            groupExpressions.Add(groupExpression);
            return groupExpressions;
        }

        private TablixRows GetTablixRows(TablixRows tablixRows, QueryTree queryTree)
        {
            tablixRows.Add(CreateHeaderRow(queryTree));
            tablixRows.Add(CreateDetailRow(queryTree));
            return tablixRows;
        }

        private TablixRow CreateHeaderRow(QueryTree queryTree)
        {
            var tablixRow = new TablixRow();
            tablixRow.Height = "0.25in";
            tablixRow.TablixCells = new TablixCells();
            CreateHeaderRowTablixCells(tablixRow, queryTree);
            return tablixRow;
        }
        //Edit Walaa 26
        private TablixRow CreateHeaderRowTablixCells(TablixRow tablixRow, QueryTree queryTree)
        {
            foreach (var leaf in queryTree.Leaves.Where(x => x.IsSelected))
            {
                string labelName = GetLabelName(queryTree, leaf);
                tablixRow.TablixCells.Add(CreateHeaderCell(labelName, GetLabelText(queryTree, leaf)));
            }
            foreach (QueryTree supQueryTree in queryTree.Nodes.Where(x => x.HasSelectedFields))
            {
                CreateHeaderRowTablixCells(tablixRow, supQueryTree);
            }
            return tablixRow;
        }

        //End
        private TablixCell CreateHeaderCell(string name, string value)
        {
            var style = GetTableHeaderStyle();

            var textRun = new TextRun();
            textRun.Value = value;

            var paragraph = new Syncfusion.RDL.DOM.Paragraph();
            paragraph.Style = GetParagraphStyle();
            paragraph.TextRuns = new TextRuns();
            paragraph.TextRuns.Add(textRun);

            var textBox = new Syncfusion.RDL.DOM.TextBox();
            textBox.Name = name;
            textBox.Style = style;
            textBox.KeepTogether = true;
            textBox.HideDuplicates = _dataSet.Name;
            textBox.Paragraphs = new Paragraphs();
            textBox.Paragraphs.Add(paragraph);

            var cellContent = new CellContents();
            cellContent.ReportItem = textBox;

            var tablixCell = new TablixCell();
            tablixCell.CellContents = cellContent;

            return tablixCell;
        }

        private TablixRow CreateDetailRow(QueryTree queryTree)
        {
            var tablixRow = new TablixRow();
            tablixRow.Height = "0.25in";
            tablixRow.TablixCells = new TablixCells();

            CreateDetailRowTablixCells(tablixRow, queryTree);


            return tablixRow;
        }
        //Edit Walaa 26
        private TablixRow CreateDetailRowTablixCells(TablixRow tablixRow, QueryTree queryTree)
        {
            foreach (var leaf in queryTree.Leaves.Where(x => x.IsSelected))
            {
                string name = queryTree.GetTableName() + leaf.DisplayName;
                if (leaf.PropertyType.IsEnum)
                {
                    
                    tablixRow.TablixCells.Add(CreateEnumCell(queryTree.GetTableName(),leaf.PropertyType));
                }
                else
                {
                    tablixRow.TablixCells.Add(CreateCell(name));
                }
            }
            foreach (QueryTree supQueryTree in queryTree.Nodes.Where(x => x.HasSelectedFields))
            {
                CreateDetailRowTablixCells(tablixRow, supQueryTree);
            }
            return tablixRow;
        }
        //End 
        private TablixCell CreateCell(string name)
        {
            var style = GetTableBodyStyle();

            var textRun = new TextRun();
            textRun.Value = "=Fields!" + name + ".Value";

            var paragraph = new Syncfusion.RDL.DOM.Paragraph();
            paragraph.Style = GetParagraphStyle();
            paragraph.TextRuns = new TextRuns();
            paragraph.TextRuns.Add(textRun);

            var textBox = new Syncfusion.RDL.DOM.TextBox();
            textBox.Name = name;
            textBox.Style = style;
            textBox.KeepTogether = true;
            textBox.HideDuplicates = _dataSet.Name;
            textBox.Paragraphs = new Paragraphs();
            textBox.Paragraphs.Add(paragraph);

            var cellContent = new CellContents();
            cellContent.ReportItem = textBox;

            var tablixCell = new TablixCell();
            tablixCell.CellContents = cellContent;


            return tablixCell;
        }

        private TablixCell CreateEnumCell(string tableName,System.Type name)
        {
            var style = GetTableBodyStyle();

            var textRun = new TextRun();
            string SwitchContent = "";
            int counter = 0;
            foreach (var nameOfEnum in Enum.GetNames(name))
            {
                if (counter == 0)
                {
                    SwitchContent += "Fields!" + tableName + name.Name + ".Value = " + counter + $@",""{nameOfEnum}""";
                }
                else
                {
                    SwitchContent += ",Fields!" + tableName + name.Name + ".Value = " + counter + $@",""{nameOfEnum}""";
                }
                counter++;
            }
            textRun.Value = $"=Switch({SwitchContent})";

            var paragraph = new Syncfusion.RDL.DOM.Paragraph();
            paragraph.Style = GetParagraphStyle();
            paragraph.TextRuns = new TextRuns();
            paragraph.TextRuns.Add(textRun);

            var textBox = new Syncfusion.RDL.DOM.TextBox();
            textBox.Name = name.Name;
            textBox.Style = style;
            textBox.KeepTogether = true;
            textBox.HideDuplicates = _dataSet.Name;
            textBox.Paragraphs = new Paragraphs();
            textBox.Paragraphs.Add(paragraph);

            var cellContent = new CellContents();
            cellContent.ReportItem = textBox;

            var tablixCell = new TablixCell();
            tablixCell.CellContents = cellContent;


            return tablixCell;
        }

        private TablixColumns CreateTableColumns(QueryTree queryTree)
        {
            var tablixColumns = new TablixColumns();

            foreach (var leaf in queryTree.Leaves.Where(x => x.IsSelected))
            {
                var tablixColumn = new TablixColumn();
                tablixColumn.Width = "2in";
                tablixColumns.Add(tablixColumn);
            }

            return tablixColumns;
        }

        //  Amer And Walaa Edits
        private TablixColumns CreateChildTablixColumns(TablixColumns tablixColumns, QueryTree queryTree)
        {
            foreach (var leaf in queryTree.Leaves.Where(x => x.IsSelected))
            {
                var tablixColumn = new TablixColumn();
                tablixColumn.Width = "2in";
                tablixColumns.Add(tablixColumn);
            }

            foreach (QueryTree supQueryTree in queryTree.Nodes.Where(x => x.HasSelectedFields))
            {
                CreateChildTablixColumns(tablixColumns, supQueryTree);
            }

            return tablixColumns;
        }

        Syncfusion.RDL.DOM.Style AddStyle()
        {
            Syncfusion.RDL.DOM.Style style = new Syncfusion.RDL.DOM.Style();
            style.Border = new Syncfusion.RDL.DOM.Border();
            style.Border.Width = new Syncfusion.RDL.DOM.Size("1pt");
            style.Border.Style = "Solid";
            style.Border.Color = "Black";
            return style;
        }

        PageHeader CreateHeader()
        {
            PageHeader pageHeader = new PageHeader();
            pageHeader.Height = new Syncfusion.RDL.DOM.Size("0.59167in");
            pageHeader.Style = AddStyle();
            pageHeader.PrintOnFirstPage = true;
            pageHeader.PrintOnLastPage = true;
            pageHeader.ReportItems = new ReportItems();
            var textBox = CreateTextBox("TextBox2", "Header", "1.61333in", "0.13833in", "2.2in", "0.34167in");
            pageHeader.ReportItems.Add(textBox);
            return pageHeader;
        }

        PageFooter CreateFooter()
        {
            PageFooter pageFooter = new PageFooter();
            pageFooter.Style = AddStyle();
            pageFooter.Height = new Syncfusion.RDL.DOM.Size("0.59167in");
            pageFooter.ReportItems = new ReportItems();
            pageFooter.PrintOnFirstPage = true;
            pageFooter.PrintOnLastPage = true;
            var textBox = CreateTextBox("TextBox3", "Footer", "1.61333in", "0.05416in", "2.2in", "0.34167in");
            pageFooter.ReportItems.Add(textBox);
            return pageFooter;
        }

        Syncfusion.RDL.DOM.TextBox CreateTextBox(string name, string text, string left, string top, string width, string height)
        {
            var textBox = new Syncfusion.RDL.DOM.TextBox();
            textBox.Name = name;
            textBox.Height = new Syncfusion.RDL.DOM.Size(height);
            textBox.Width = new Syncfusion.RDL.DOM.Size(width);
            textBox.Left = new Syncfusion.RDL.DOM.Size(left);
            textBox.Top = new Syncfusion.RDL.DOM.Size(top);
            textBox.Style = new Syncfusion.RDL.DOM.Style();
            textBox.Style.TextAlign = "Center";
            textBox.Style.VerticalAlign = "Middle";

            textBox.Paragraphs = new Paragraphs();
            Syncfusion.RDL.DOM.Paragraph paragraph = new Syncfusion.RDL.DOM.Paragraph();
            TextRuns runs = new TextRuns();
            TextRun run = new TextRun();
            run.Style = new Syncfusion.RDL.DOM.Style();
            run.Style.FontStyle = "Default";
            run.Style.TextAlign = "Center";
            run.Style.FontFamily = "Arial";
            run.Style.FontSize = new Syncfusion.RDL.DOM.Size("10pt");
            run.Value = text;
            runs.Add(run);
            paragraph.Style = new Syncfusion.RDL.DOM.Style();
            paragraph.Style.VerticalAlign = "Top";
            paragraph.Style.TextAlign = "Center";
            paragraph.TextRuns = runs;
            textBox.Paragraphs.Add(paragraph);

            return textBox;
        }

        Syncfusion.RDL.DOM.Style GetTableBodyStyle()
        {
            var style = new Syncfusion.RDL.DOM.Style();
            style = new Syncfusion.RDL.DOM.Style();
            style.TextAlign = "Center";
            style.VerticalAlign = "Middle";
            style.FontSize = new Syncfusion.RDL.DOM.Size("11pt");
            style.FontFamily = "tahoma";
            style.Border = new Border()
            {
                Color = "LightGrey",
                Style = "Solid"
            };
            return style;
        }

        Syncfusion.RDL.DOM.Style GetTableHeaderStyle()
        {
            var style = new Syncfusion.RDL.DOM.Style();
            style = new Syncfusion.RDL.DOM.Style();
            style.TextAlign = "Center";
            style.VerticalAlign = "Middle";
            style.FontSize = new Syncfusion.RDL.DOM.Size("12pt");
            style.FontWeight = "Bold";
            style.BackgroundColor = "LightGrey";
            style.FontFamily = "tahoma";
            style.Border = new Border()
            {
                Color = "LightGrey",
                Style = "Solid"
            };
            return style;
        }

        Syncfusion.RDL.DOM.Style GetParagraphStyle()
        {
            var style = new Syncfusion.RDL.DOM.Style();
            style.TextAlign = "Center";
            style.VerticalAlign = "Middle";

            return style;
        }

        private double GetPageWidth(QueryTree queryTree)
        {
            var count = queryTree.Leaves.Where(x => x.IsSelected).Count();
            return count * 2 + 1;
        }


        private void SaveSerialize()
        {
            string nameSpace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition";

            if (_report.RDLType == RDLType.RDL2010)
            {
                nameSpace = "http://schemas.microsoft.com/sqlserver/reporting/2010/01/reportdefinition";
            }
            else if (_report.RDLType == RDLType.RDL2016)
            {
                nameSpace = "http://schemas.microsoft.com/sqlserver/reporting/2016/01/reportdefinition";
            }

            XmlSerializer xs2 = new XmlSerializer(typeof(ReportDefinition), nameSpace);
            XmlSerializerNamespaces xs = new XmlSerializerNamespaces();
            xs.Add("rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");

            if (_report.RDLType == RDLType.RDL2016)
            {
                xs.Add("df", "http://schemas.microsoft.com/sqlserver/reporting/2016/01/reportdefinition/defaultfontfamily");
            }

            try
            {
                TextWriter ws2 = new StreamWriter(_reportPath + "\\ReportGenerator_" + _reportId + ".rdl");
                xs2.Serialize(ws2, _report, xs);
                ws2.Close();
            }
            catch (Exception ex) { }
        }

        private string GetLabelText(QueryTree queryTree, QueryLeaf leaf)
        {
            var resourceName = queryTree.FullClassName + "." + leaf.DisplayName;
            var resourceValue = ServiceFactory.LocalizationService.GetResource(resourceName);
            if (resourceValue != null)
                return resourceValue;

            return Regex.Replace(leaf.DisplayName, @"(?<a>[a-z])(?<b>[A-Z0-9])", @"${a} ${b}");
        }

        private string GetLabelName(QueryTree queryTree, QueryLeaf leaf)
        {
            var sections = queryTree.FullClassName.Split('.');
            if (sections != null && sections.Length >= 3)
            {
                return $"Label{sections[2]}0{queryTree.GetTableName()}0{leaf.DisplayName}";
            }

            return $"Label{leaf.DisplayName}";
        }
    }
}


