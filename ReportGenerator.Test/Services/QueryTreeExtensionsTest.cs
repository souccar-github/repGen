#region Using Statements

using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.JobDesc.ValueObjects;
using HRIS.Domain.OrgChart.Indexes;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.ValueObjects;
using Moq;
using NHibernate.Criterion;
using SharpTestsEx;
using Souccar.Domain.PersistenceSupport;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using Souccar.ReportGenerator.Services;
using Xunit;

#endregion

namespace Souccar.ReportGenerator.Test.Services
{
    public class when_dealing_with_query_tree_extensions_class
    {
        public virtual void SetupTest()
        {
        }
    }

    #region Get Orderby String Version Tests

    public class and_calling_get_orderby_query_string_method : when_dealing_with_query_tree_extensions_class
    {
        protected String Actual;
        protected IClassMapping ClassMapping;

        protected String Expected;
        protected QueryTree QueryTree;

        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree = new QueryTree();
            var classMappingMock = new Mock<IClassMapping>();
            classMappingMock.Setup(expr => expr.TableName(It.IsAny<Type>())).Returns((Type type) => type.Name);
            classMappingMock.Setup(expr => expr.ColumnName(typeof (Employee), "FirstName")).Returns("FirstName");
            classMappingMock.Setup(expr => expr.ColumnName(typeof (Employee), "LastName")).Returns("LastName");
            classMappingMock.Setup(expr => expr.ColumnName(typeof (Employee), "BloodType")).Returns("BloodType_id");
            classMappingMock.Setup(expr => expr.ColumnName(typeof (Employee), "Religion")).Returns("Religion_id");
            classMappingMock.Setup(expr => expr.ColumnName(typeof (Employee), "DateOfBirth")).Returns("DateOfBirth");
            classMappingMock.Setup(expr => expr.ColumnName(typeof (Employee), "NoOfChildren")).Returns("NoOfChildren");
            classMappingMock.Setup(expr => expr.ColumnName(typeof (Child), "FirstName")).Returns("FirstName");
            classMappingMock.Setup(expr => expr.ColumnName(typeof (Child), "ResidencyNo")).Returns("ResidencyNo");
            classMappingMock.Setup(expr => expr.ColumnName(typeof (Religion), "Name")).Returns("Name");
            classMappingMock.Setup(expr => expr.ColumnName(typeof (JobDescription), "Summary")).Returns("Summary");
            classMappingMock.Setup(expr => expr.ColumnName(typeof (JobDescription), "JobTitle")).Returns("JobTitle_id");
            classMappingMock.Setup(expr => expr.ColumnName(typeof (Role), "Name")).Returns("Name");
            classMappingMock.Setup(expr => expr.ColumnName(typeof (Responsibility), "Description")).Returns(
                "Description");
            classMappingMock.Setup(expr => expr.ColumnName(typeof (JobTitle), "Name")).Returns("Name");

            ClassMapping = classMappingMock.Object;
        }
    }

    public class and_passing_a_query_tree_with_no_orderby : and_calling_get_orderby_query_string_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         Selected = 1,
                                         IsPrimitive = true,
                                         IsReference = false,
                                         ParentType = typeof (Employee),
                                         PropertyType = typeof (string),
                                         PropertyName = "FirstName",
                                         SortDescriptor =
                                             new SortDescriptor
                                                 {SortDirection = ListSortDirection.Ascending, SortOrder = 0}
                                     });
            Expected = String.Empty;
            Actual = QueryTree.GetOrderByQueryString(ClassMapping);
        }

        [Fact]
        public void then_an_empty_string_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_passing_a_query_tree_with_one_leaf_and_has_order_by : and_calling_get_orderby_query_string_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         Selected = 1,
                                         IsPrimitive = true,
                                         IsReference = false,
                                         ParentType = typeof (Employee),
                                         PropertyType = typeof (string),
                                         PropertyName = "FirstName",
                                         SortDescriptor =
                                             new SortDescriptor
                                                 {SortDirection = ListSortDirection.Ascending, SortOrder = 1}
                                     });
            Expected =
                String.Format("order by {0}.{1}", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), "FirstName")).ToLower();
            Actual = QueryTree.GetOrderByQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_order_by_one_field_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_passing_a_query_tree_with_two_leaf_and_has_order_by : and_calling_get_orderby_query_string_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         Selected = 1,
                                         IsPrimitive = true,
                                         IsReference = false,
                                         ParentType = typeof (Employee),
                                         PropertyType = typeof (string),
                                         PropertyName = "FirstName",
                                         SortDescriptor = new SortDescriptor {SortOrder = 1}
                                     });
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         Selected = 2,
                                         IsPrimitive = true,
                                         IsReference = false,
                                         ParentType = typeof (Employee),
                                         PropertyType = typeof (string),
                                         PropertyName = "LastName",
                                         SortDescriptor = new SortDescriptor {SortOrder = 2}
                                     });
            Expected =
                String.Format("order by {0}.{1},{0}.{2}", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), "FirstName"),
                              ClassMapping.ColumnName(typeof (Employee), "LastName")).ToLower();
            Actual = QueryTree.GetOrderByQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_order_by_two_field_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_passing_a_query_tree_with_one_leaf_and_has_desc_order_by :
        and_calling_get_orderby_query_string_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         Selected = 1,
                                         IsPrimitive = true,
                                         IsReference = false,
                                         ParentType = typeof (Employee),
                                         PropertyType = typeof (string),
                                         PropertyName = "FirstName",
                                         SortDescriptor =
                                             new SortDescriptor
                                                 {SortOrder = 1, SortDirection = ListSortDirection.Descending}
                                     });
            Expected =
                String.Format("order by {0}.{1} desc", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), "FirstName")).ToLower();
            Actual = QueryTree.GetOrderByQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_order_by_one_field_desc_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_passing_a_query_tree_with_two_leaves_with_random_sort_order :
        and_calling_get_orderby_query_string_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         Selected = 1,
                                         IsPrimitive = true,
                                         IsReference = false,
                                         ParentType = typeof (Employee),
                                         PropertyType = typeof (string),
                                         PropertyName = "FirstName",
                                         SortDescriptor = new SortDescriptor {SortOrder = 2}
                                     });
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         Selected = 2,
                                         IsPrimitive = true,
                                         IsReference = false,
                                         ParentType = typeof (Employee),
                                         PropertyType = typeof (string),
                                         PropertyName = "LastName",
                                         SortDescriptor = new SortDescriptor {SortOrder = 1}
                                     });

            Expected =
                String.Format("order by {0}.{2},{0}.{1}", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), "FirstName"),
                              ClassMapping.ColumnName(typeof (Employee), "LastName")).ToLower();
            Actual = QueryTree.GetOrderByQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_order_by_two_fields_in_the_correct_order_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_passing_a_query_tree_with_two_leaves_and_one_node_with_two_leaves_and_has_order_by :
        and_calling_get_orderby_query_string_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         ParentType = typeof (Employee),
                                         PropertyType = typeof (string),
                                         PropertyName = "FirstName",
                                         SortDescriptor = new SortDescriptor {SortOrder = 1}
                                     });
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         ParentType = typeof (Employee),
                                         PropertyType = typeof (string),
                                         PropertyName = "LastName",
                                         SortDescriptor = new SortDescriptor {SortOrder = 2}
                                     });
            var childrenQueryTree = new QueryTree {Type = typeof (Child)};
            childrenQueryTree.Leaves.Add(new QueryLeaf
                                             {
                                                 ParentType = typeof (Child),
                                                 PropertyType = typeof (string),
                                                 PropertyName = "FirstName",
                                                 SortDescriptor = new SortDescriptor {SortOrder = 1}
                                             });
            childrenQueryTree.Leaves.Add(new QueryLeaf
                                             {
                                                 ParentType = typeof (Child),
                                                 PropertyType = typeof (string),
                                                 PropertyName = "ResidencyNo",
                                                 SortDescriptor = new SortDescriptor {SortOrder = 2}
                                             });
            QueryTree.Nodes.Add(childrenQueryTree);
            Expected = String.Format("order by {0}.{1},{0}.{2},{3}.{4},{3}.{5}",
                                     ClassMapping.TableName(typeof (Employee)),
                                     ClassMapping.ColumnName(typeof (Employee), "FirstName"),
                                     ClassMapping.ColumnName(typeof (Employee), "LastName"),
                                     ClassMapping.TableName(typeof (Child)),
                                     ClassMapping.ColumnName(typeof (Child), "FirstName"),
                                     ClassMapping.ColumnName(typeof (Child), "ResidencyNo")).ToLower();
            Actual = QueryTree.GetOrderByQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_order_by_four_fields_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_passing_a_query_tree_with_one_leaves_and_one_leaf_in_level_two_and_has_order_by :
        and_calling_get_orderby_query_string_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            var responsibilityQueryTree = new QueryTree {Type = typeof (Responsibility)};

            responsibilityQueryTree.Leaves.Add(new QueryLeaf
                                                   {
                                                       ParentType = typeof (Responsibility),
                                                       PropertyType = typeof (string),
                                                       PropertyName = "Description",
                                                       SortDescriptor = new SortDescriptor {SortOrder = 1}
                                                   });
            var roleQueryTree = new QueryTree {Type = typeof (Role)};
            roleQueryTree.Leaves.Add(new QueryLeaf
                                         {
                                             ParentType = typeof (Role),
                                             PropertyType = typeof (string),
                                             PropertyName = "Name",
                                             SortDescriptor = new SortDescriptor {SortOrder = 1}
                                         });
            roleQueryTree.Nodes.Add(responsibilityQueryTree);

            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         ParentType = typeof (JobDescription),
                                         PropertyType = typeof (string),
                                         PropertyName = "Summary",
                                         SortDescriptor = new SortDescriptor {SortOrder = 1}
                                     });

            QueryTree.Nodes.Add(roleQueryTree);
            Expected = String.Format("order by {0}.{1},{2}.{3},{4}.{5}",
                                     ClassMapping.TableName(typeof (JobDescription)),
                                     ClassMapping.ColumnName(typeof (JobDescription), "Summary"),
                                     ClassMapping.TableName(typeof (Role)),
                                     ClassMapping.ColumnName(typeof (Role), "Name"),
                                     ClassMapping.TableName(typeof (Responsibility)),
                                     ClassMapping.ColumnName(typeof (Responsibility), "Description")).ToLower();
            Actual = QueryTree.GetOrderByQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_order_by_three_fields_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_passing_a_query_tree_with_two_leaves_one_of_them_without_sort :
        and_calling_get_orderby_query_string_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         ParentType = typeof (JobDescription),
                                         PropertyType = typeof (string),
                                         PropertyName = "Summary",
                                         SortDescriptor = new SortDescriptor {SortOrder = 2}
                                     });
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         IsReference = true,
                                         ParentType = typeof (JobTitle),
                                         PropertyType = typeof (string),
                                         PropertyName = "JobTitle.Name",
                                     });
            Expected = String.Format("order by {0}.{1}",
                                     ClassMapping.TableName(typeof (JobDescription)),
                                     ClassMapping.ColumnName(typeof (JobDescription), "Summary")).ToLower();
            Actual = QueryTree.GetOrderByQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_order_by_one_field_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    #endregion

    #region Get Filter Expression Tree Tests

    public class and_calling_get_filter_expression_tree_method : when_dealing_with_query_tree_extensions_class
    {
        protected AbstractCriterion Actual;
        protected Dictionary<string, string> Aliases;
        protected AbstractCriterion Expected;
        protected QueryTree QueryTree;

        public override void SetupTest()
        {
            base.SetupTest();
            Aliases = new Dictionary<string, string>();
            QueryTree = new QueryTree();
        }
    }

    public class and_passing_a_query_tree_with_no_tree_leaves_and_no_nodes :
        and_calling_get_filter_expression_tree_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Type = typeof (Employee);
            Actual =
                QueryTree.GetFilterExpressionTree(Aliases);
        }

        [Fact]
        public void then_a_null_criteria_should_be_returned()
        {
            SetupTest();
            Actual.Should().Be.Null();
        }
    }

    public class and_passing_a_query_tree_with_node_and_leaf_with_no_filters :
        and_calling_get_filter_expression_tree_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Type = typeof (JobDescription);
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         ParentType = typeof (JobDescription),
                                         PropertyName = "Summary",
                                         IsReference = false,
                                         IsPrimitive = true,
                                         PropertyType = typeof (string)
                                     });
            QueryTree.Nodes.Add(new QueryTree {Type = typeof (Role), FullClassPath = "JobDescription.Roles"});
            QueryTree.Nodes[0].Leaves.Add(new QueryLeaf
                                              {
                                                  ParentType = typeof (Role),
                                                  PropertyName = "Name",
                                                  PropertyType = typeof (string),
                                                  IsPrimitive = true,
                                                  IsReference = false
                                              });
            Actual = QueryTree.GetFilterExpressionTree(Aliases);
        }

        [Fact]
        public void then_a_null_expression_should_be_returned()
        {
            SetupTest();
            Actual.Should().Be.Null();
        }
    }

    public class and_passing_a_query_tree_with_one_tree_leaf_and_no_filters :
        and_calling_get_filter_expression_tree_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Type = typeof (Employee);
            QueryTree.Leaves.Add(new QueryLeaf());
            Actual =
                QueryTree.GetFilterExpressionTree(Aliases);
        }

        [Fact]
        public void then_a_null_expression_should_be_returned()
        {
            SetupTest();
            Actual.Should().Be.Null();
        }
    }

    public class and_passing_a_query_tree_with_one_tree_leaf_and_one_filter :
        and_calling_get_filter_expression_tree_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Type = typeof (Employee);
            QueryTree.FullClassPath = "Employee";
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         ParentType = typeof (Employee),
                                         PropertyName = "NoOfChildren",
                                         PropertyType = typeof (int)
                                     });
            QueryTree.Leaves[0].AddFilter(FilterOperator.IsGreaterThanOrEqualTo, 1);
            Aliases.Add("Employee", "employee");
            Actual = QueryTree.GetFilterExpressionTree(Aliases);
            Expected = Restrictions.Ge("employee.NoOfChildren", 1);
        }

        [Fact]
        public void then_a_greater_than_or_equal_criteria_should_be_returned()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_passing_a_query_tree_with_two_tree_leaves_and_one_filter_each :
        and_calling_get_filter_expression_tree_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Type = typeof (Employee);
            QueryTree.FullClassPath = "Employee";
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         ParentType = typeof (Employee),
                                         PropertyName = "NoOfChildren",
                                         PropertyType = typeof (int)
                                     });
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         ParentType = typeof (Employee),
                                         PropertyName = "DateOfBirth",
                                         PropertyType = typeof (DateTime)
                                     });
            QueryTree.Leaves[0].AddFilter(FilterOperator.IsGreaterThanOrEqualTo, 1);
            QueryTree.Leaves[1].AddFilter(FilterOperator.IsEqualTo, new DateTime(1988, 11, 30));
            Aliases.Add("Employee", "employee");
            Actual =
                QueryTree.GetFilterExpressionTree(Aliases);
            Expected = Restrictions.And(Restrictions.Ge("employee.NoOfChildren", 1),
                                        Restrictions.Eq("employee.DateOfBirth", new DateTime(1988, 11, 30)));
        }

        [Fact]
        public void then_a_criteria_expression_with_an_and_operation_should_be_returned()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_passing_a_query_tree_with_one_tree_node_and_one_leaf_with_filter :
        and_calling_get_filter_expression_tree_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Type = typeof (Employee);
            QueryTree.FullClassPath = "Employee";
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         ParentType = typeof (Employee),
                                         PropertyName = "NoOfChildren",
                                         PropertyType = typeof (int)
                                     });
            QueryTree.Leaves[0].AddFilter(FilterOperator.IsGreaterThan, 1);
            QueryTree.Nodes.Add(new QueryTree {Type = typeof (Child), FullClassPath = "Employee.Children"});
            Aliases.Add("Employee", "employee");
            Aliases.Add("Employee.Children", "child");
        }
    }

    public class and_the_tree_node_has_one_query_leaf :
        and_passing_a_query_tree_with_one_tree_node_and_one_leaf_with_filter
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Nodes[0].Leaves.Add(new QueryLeaf
                                              {
                                                  ParentType = typeof (Child),
                                                  PropertyType = typeof (string),
                                                  PropertyName = "FirstName",
                                              });
        }
    }

    public class and_the_query_leaf_has_one_filter : and_the_tree_node_has_one_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Nodes[0].Leaves[0].AddFilter(FilterOperator.Contains, "hello");
            Actual =
                QueryTree.GetFilterExpressionTree(Aliases);
            Expected = Restrictions.And(Restrictions.Gt("employee.NoOfChildren", 1),
                                        Restrictions.InsensitiveLike("child.FirstName", "hello", MatchMode.Anywhere));
        }

        [Fact]
        public void then_a_criteria_with_and_between_greater_than_and_insensitive_like_expression_should_be_returned()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_the_query_leaf_has_two_filter : and_the_tree_node_has_one_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Nodes[0].Leaves[0].AddFilter(FilterOperator.Contains, "hello");
            QueryTree.Nodes[0].Leaves[0].AddFilter(FilterOperator.StartsWith, "m");
            Actual =
                QueryTree.GetFilterExpressionTree(Aliases);
            Expected = Restrictions.And(Restrictions.Gt("employee.NoOfChildren", 1),
                                        Restrictions.And(
                                            Restrictions.InsensitiveLike("child.FirstName", "hello", MatchMode.Anywhere),
                                            Restrictions.InsensitiveLike("child.FirstName", "m", MatchMode.Start)));
        }

        [Fact]
        public void then_a_criteria_with_and_between_greater_than_and_an_and_should_be_returned()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_passing_a_query_tree_with_two_tree_leaves_with_one_filter_each_and_two_tree_nodes :
        and_calling_get_filter_expression_tree_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Type = typeof (Employee);
            QueryTree.FullClassPath = "Employee";
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         PropertyName = "FirstName",
                                         PropertyType = typeof (string),
                                         IsPrimitive = true,
                                         IsReference = false,
                                         ParentType = typeof (Employee)
                                     });
            QueryTree.Leaves.Add(new QueryLeaf
                                     {
                                         PropertyName = "LastName",
                                         PropertyType = typeof (string),
                                         IsPrimitive = true,
                                         IsReference = false,
                                         ParentType = typeof (Employee)
                                     });
            QueryTree.Nodes.Add(new QueryTree {Type = typeof (Education), FullClassPath = "Employee.Educations"});
            QueryTree.Nodes.Add(new QueryTree {Type = typeof (Child), FullClassPath = "Employee.Children"});
            QueryTree.Nodes[0].Leaves.Add(new QueryLeaf
                                              {
                                                  PropertyName = "Comments",
                                                  PropertyType = typeof (string),
                                                  IsPrimitive = true,
                                                  IsReference = false,
                                                  ParentType = typeof (Education)
                                              });
            QueryTree.Nodes[1].Leaves.Add(new QueryLeaf
                                              {
                                                  PropertyName = "FirstName",
                                                  PropertyType = typeof (string),
                                                  IsPrimitive = true,
                                                  IsReference = false,
                                                  ParentType = typeof (Child)
                                              });
            QueryTree.Leaves.First(leaf => leaf.PropertyName == "FirstName").AddFilter(FilterOperator.IsEqualTo, "hello");
            QueryTree.Leaves.First(leaf => leaf.PropertyName == "LastName").AddFilter(FilterOperator.IsEqualTo, "hello");
            Aliases.Add("Employee", "employee");
            Aliases.Add("Employee.Children", "child");
            Aliases.Add("Employee.Educations", "education");
            Actual =
                QueryTree.GetFilterExpressionTree(Aliases);
            Expected = Restrictions.And(Restrictions.InsensitiveLike("employee.FirstName", "hello", MatchMode.Exact),
                                        Restrictions.InsensitiveLike("employee.LastName", "hello", MatchMode.Exact));
        }

        [Fact]
        public void then_to_string_should_be_and_expression_between_two_insensitive_like()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_passing_and_passing_a_query_tree_no_tree_leaves_and_one_tree_node :
        and_calling_get_filter_expression_tree_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Type = typeof (Employee);
            QueryTree.FullClassPath = "Employee";
            QueryTree.Nodes.Add(new QueryTree {FullClassPath = "Employee.Children", Type = typeof (Child)});
            Aliases.Add("Employee", "employee");
            Aliases.Add("Employee.Children", "Child");
        }
    }

    public class and_the_tree_node_has_one_query_leaf_with_filter :
        and_passing_and_passing_a_query_tree_no_tree_leaves_and_one_tree_node
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Nodes[0].Leaves.Add(new QueryLeaf
                                              {
                                                  ParentType = typeof (Child),
                                                  IsReference = false,
                                                  IsPrimitive = true,
                                                  PropertyName = "FirstName",
                                                  PropertyType = typeof (string)
                                              });
            QueryTree.Nodes[0].Leaves[0].AddFilter(FilterOperator.Contains, "hello");
            Actual = QueryTree.GetFilterExpressionTree(Aliases);
            Expected = Restrictions.InsensitiveLike("Child.FirstName", "hello", MatchMode.Anywhere);
        }

        [Fact]
        public void then_a_single_filter_criteria_should_be_returned()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_the_query_tree_has_one_aggregate_filter : and_calling_get_filter_expression_tree_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree = QueryTreeFactory.Create(typeof (Employee));
            QueryTree.AddAggregateFilter(new AggregateFilterDescriptor
                                             {
                                                 AggregateFunction = AggregateFunction.Count,
                                                 FilterOperator = FilterOperator.IsGreaterThan,
                                                 PropertyName = "Children",
                                                 Value = 2
                                             });
            Expected = Subqueries.Exists(DetachedCriteria.For<Contact>()
                                             .SetProjection(Projections.RowCount())
                                             .Add(Restrictions.EqProperty("Employee", "employee.Id"))
                                             .Add(Restrictions.Gt(Projections.RowCount(), 2)));
            Aliases.Add("Employee", "employee");
            Actual = QueryTree.GetFilterExpressionTree(Aliases);
        }

        [Fact]
        public void then_an_exists_subquery_should_be_returned()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }


    public class and_the_query_tree_has_two_aggregate_filters : and_calling_get_filter_expression_tree_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree = QueryTreeFactory.Create(typeof (Employee));
            QueryTree.AddAggregateFilter(new AggregateFilterDescriptor
                                             {
                                                 AggregateFunction = AggregateFunction.Count,
                                                 FilterOperator = FilterOperator.IsGreaterThan,
                                                 PropertyName = "Children",
                                                 Value = 2
                                             });
            QueryTree.AddAggregateFilter(new AggregateFilterDescriptor
                                             {
                                                 AggregateFunction = AggregateFunction.Count,
                                                 FilterOperator = FilterOperator.IsGreaterThan,
                                                 PropertyName = "Educations",
                                                 Value = 1
                                             });
            Expected = Restrictions.And(Subqueries.Exists(DetachedCriteria.For<Child>()
                                                              .SetProjection(Projections.RowCount())
                                                              .Add(Restrictions.EqProperty("Employee", "employee.Id"))
                                                              .Add(Restrictions.Gt(Projections.RowCount(), 2))),
                                        Subqueries.Exists(DetachedCriteria.For<Education>()
                                                              .SetProjection(Projections.RowCount())
                                                              .Add(Restrictions.EqProperty("Employee", "employee.Id"))
                                                              .Add(Restrictions.Gt(Projections.RowCount(), 1))));
            Aliases.Add("Employee", "employee");
            Actual = QueryTree.GetFilterExpressionTree(Aliases);
        }

        [Fact]
        public void then_an_and_between_two_exists_subquery_should_be_returned()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_passing_a_query_tree_with_an_aggregate_filter_and_query_leaf_with_one_filter :
        and_calling_get_filter_expression_tree_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree = QueryTreeFactory.Create(typeof (Employee));
            QueryTree.AddAggregateFilter(new AggregateFilterDescriptor
                                             {
                                                 AggregateFunction = AggregateFunction.Count,
                                                 FilterOperator = FilterOperator.IsGreaterThan,
                                                 PropertyName = "Children",
                                                 Value = 2
                                             });
            QueryTree.Leaves.Single(x => x.PropertyName == "FirstName").AddFilter(FilterOperator.StartsWith, "m");
            Expected = Restrictions.And(Subqueries.Exists(DetachedCriteria.For<Child>()
                                                              .SetProjection(Projections.RowCount())
                                                              .Add(Restrictions.EqProperty("Employee", "employee.Id"))
                                                              .Add(Restrictions.Gt(Projections.RowCount(), 2))),
                                        Restrictions.InsensitiveLike("employee.FirstName", "m", MatchMode.Start));
            Aliases.Add("Employee", "employee");
            Actual = QueryTree.GetFilterExpressionTree(Aliases);
        }

        [Fact]
        public void then_an_and_between_subquery_and_start_with_should_be_returned()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    #endregion
}