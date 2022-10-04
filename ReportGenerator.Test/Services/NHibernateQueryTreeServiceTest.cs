using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.JobDesc.ValueObjects;
using HRIS.Domain.Personnel.Entities;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using Souccar.ReportGenerator.Services;
using Souccar.ReportGenerator.Test.TestClasses;
using Xunit;

namespace Souccar.ReportGenerator.Test.Services
{
    public class when_dealing_with_nhibernate_query_tree_service_class
    {
        protected IQueryTreeService QueryTreeService;

        public virtual void SetupTest()
        {
            NHibernateProfiler.Initialize();
            QueryTreeService = new NHibernateQueryTreeService();
        }
    }

    public class and_calling_the_execute_query_method : when_dealing_with_nhibernate_query_tree_service_class
    {
        protected object Actual;
        protected object Expected;
        protected QueryTree QueryTree;

        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree = new QueryTree();
        }
    }

    public class and_passing_a_query_tree_with_one_selected_field : and_calling_the_execute_query_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree = QueryTreeFactory.Create(typeof (JobDescription));
            QueryTree.Leaves.First(prop => prop.PropertyName == "Summary").Selected = 1;
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public void then_an_iqueryable_of_the_querytree_type_should_be_returned()
        {
            SetupTest();
            Assert.Equal(typeof (List<>).MakeGenericType(QueryTree.Type), Actual.GetType());
        }
    }

    public class and_passing_a_query_tree_with_two_selected_fields_one_reference_and_one_non_reference :
        and_calling_the_execute_query_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree = QueryTreeFactory.Create(typeof (JobDescription));
            QueryTree.Leaves.First(prop => prop.PropertyName == "Summary").Selected = 1;
            QueryTree.Leaves.First(prop => prop.PropertyName == "JobTitle.Name").Selected = 2;
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public void then_an_iqueryable_of_the_querytree_type_should_be_returned()
        {
            SetupTest();
            Assert.Equal(typeof (List<>).MakeGenericType(QueryTree.Type), Actual.GetType());
        }
    }

    public class and_has_a_selected_field_from_detail : and_calling_the_execute_query_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree = QueryTreeFactory.Create(typeof (JobDescription));
            QueryTree.Leaves.First(prop => prop.PropertyName == "Summary").Selected = 1;
            QueryTree.Leaves.First(prop => prop.PropertyName == "JobTitle.Name").Selected = 2;
            QueryTree.Nodes.First(node => node.Type == typeof (Role)).Leaves.First(prop => prop.PropertyName == "Name").
                Selected = 1;
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public void then_an_iqueryable_of_the_querytree_type_should_be_returned()
        {
            SetupTest();
            Assert.Equal(typeof (List<>).MakeGenericType(QueryTree.Type), Actual.GetType());
        }
    }

    public class and_has_a_selected_reference_field_from_detail_of_detail : and_calling_the_execute_query_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            NHibernateProfiler.Initialize();
            QueryTree = QueryTreeFactory.Create(typeof (JobDescription));
            QueryTree.Leaves.First(prop => prop.PropertyName == "Summary").Selected = 1;
            QueryTree.Leaves.First(prop => prop.PropertyName == "JobTitle.Name").Selected = 2;
            QueryTree.Nodes.First(node => node.Type == typeof (Role)).Leaves.First(prop => prop.PropertyName == "Name").
                Selected = 1;
            QueryTree.Nodes.First(node => node.Type == typeof (Role)).Nodes.First(
                node => node.Type == typeof (Responsibility)).Leaves.First(prop => prop.PropertyName == "Priority.Name")
                .Selected = 1;
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public void then_an_iqueryable_of_the_querytree_type_should_be_returned()
        {
            SetupTest();
            Assert.Equal(typeof (List<>).MakeGenericType(QueryTree.Type), Actual.GetType());
        }
    }

    public class and_passing_a_query_tree_with_two_selected_field : and_calling_the_execute_query_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree = QueryTreeFactory.Create(typeof (Employee));
            QueryTree.Leaves.First(prop => prop.PropertyName == "FirstName").Selected = 1;
            QueryTree.Leaves.First(prop => prop.PropertyName == "LastName").Selected = 2;
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public void then_an_iqueryable_of_the_querytree_type_should_be_returned()
        {
            SetupTest();
            Assert.Equal(typeof (List<>).MakeGenericType(QueryTree.Type), Actual.GetType());
        }
    }

    public class and_the_leaves_have_order_by_asc : and_calling_the_execute_query_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree = QueryTreeFactory.Create(typeof (Employee));
            QueryTree.Leaves.First(prop => prop.PropertyName == "FirstName").Selected = 1;
            QueryTree.Leaves.First(prop => prop.PropertyName == "LastName").Selected = 2;
            QueryTree.Leaves.First(prop => prop.PropertyName == "FirstName").SortDescriptor = new SortDescriptor
                                                                                                  {
                                                                                                      SortDirection =
                                                                                                          ListSortDirection
                                                                                                          .Ascending,
                                                                                                      SortOrder = 1
                                                                                                  };
            QueryTree.Leaves.First(prop => prop.PropertyName == "LastName").SortDescriptor = new SortDescriptor
                                                                                                 {
                                                                                                     SortDirection =
                                                                                                         ListSortDirection
                                                                                                         .Ascending,
                                                                                                     SortOrder = 2
                                                                                                 };
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public void then_an_iqueryable_of_the_querytree_type_should_be_returned()
        {
            SetupTest();
            Assert.Equal(typeof (List<>).MakeGenericType(QueryTree.Type), Actual.GetType());
        }
    }

    public class and_the_leaves_have_order_by_desc : and_passing_a_query_tree_with_two_selected_field
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Leaves.First(prop => prop.PropertyName == "FirstName").SortDescriptor = new SortDescriptor
                                                                                                  {
                                                                                                      SortDirection =
                                                                                                          ListSortDirection
                                                                                                          .Descending,
                                                                                                      SortOrder = 1
                                                                                                  };
            QueryTree.Leaves.First(prop => prop.PropertyName == "LastName").SortDescriptor = new SortDescriptor
                                                                                                 {
                                                                                                     SortDirection =
                                                                                                         ListSortDirection
                                                                                                         .Descending,
                                                                                                     SortOrder = 2
                                                                                                 };
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public new void then_an_iqueryable_of_the_querytree_type_should_be_returned()
        {
            SetupTest();
            Assert.Equal(typeof (List<>).MakeGenericType(QueryTree.Type), Actual.GetType());
        }
    }

    public class and_the_leaf_is_reference_and_not_selected_with_order_by_asc : and_calling_the_execute_query_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree = QueryTreeFactory.Create(typeof (Employee));
            QueryTree.Leaves.First(prop => prop.PropertyName == "FirstName").Selected = 1;
            QueryTree.Leaves.First(prop => prop.PropertyName == "LastName").Selected = 2;
            QueryTree.Leaves.First(prop => prop.PropertyName == "FirstName").SortDescriptor = new SortDescriptor
                                                                                                  {
                                                                                                      SortDirection =
                                                                                                          ListSortDirection
                                                                                                          .Ascending,
                                                                                                      SortOrder = 1
                                                                                                  };
            QueryTree.Leaves.First(prop => prop.PropertyName == "LastName").SortDescriptor = new SortDescriptor
                                                                                                 {
                                                                                                     SortDirection =
                                                                                                         ListSortDirection
                                                                                                         .Ascending,
                                                                                                     SortOrder = 2
                                                                                                 };
            QueryTree.Leaves.First(prop => prop.PropertyName == "Gender.Name").SortDescriptor = new SortDescriptor
                                                                                                    {
                                                                                                        SortDirection =
                                                                                                            ListSortDirection
                                                                                                            .Ascending,
                                                                                                        SortOrder = 3
                                                                                                    };
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public void then_an_iqueryable_of_the_querytree_type_should_be_returned()
        {
            SetupTest();
            Assert.Equal(typeof (List<>).MakeGenericType(QueryTree.Type), Actual.GetType());
        }
    }


    public class and_the_leaf_has_order_by_asc : and_passing_a_query_tree_with_one_selected_field
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Leaves.First(prop => prop.PropertyName == "Summary").SortDescriptor = new SortDescriptor
                                                                                                {
                                                                                                    SortOrder = 1,
                                                                                                    SortDirection =
                                                                                                        ListSortDirection.
                                                                                                        Ascending
                                                                                                };
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public new void then_an_iqueryable_of_the_querytree_type_should_be_returned()
        {
            SetupTest();
            Assert.Equal(typeof (List<>).MakeGenericType(QueryTree.Type), Actual.GetType());
        }
    }

    public class and_the_leaf_has_order_by_desc : and_passing_a_query_tree_with_one_selected_field
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Leaves.First(prop => prop.PropertyName == "Summary").SortDescriptor = new SortDescriptor
                                                                                                {
                                                                                                    SortOrder = 1,
                                                                                                    SortDirection =
                                                                                                        ListSortDirection.
                                                                                                        Descending
                                                                                                };
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public new void then_an_iqueryable_of_the_querytree_type_should_be_returned()
        {
            SetupTest();
            Assert.Equal(typeof (List<>).MakeGenericType(QueryTree.Type), Actual.GetType());
        }
    }

    public class and_the_leaf_has_one_filter : and_passing_a_query_tree_with_one_selected_field
    {
    }

    public class and_the_leaf_has_two_filters : and_passing_a_query_tree_with_one_selected_field
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Leaves.First(prop => prop.PropertyName == "Summary").AddFilter(FilterOperator.Contains, "h");
            QueryTree.Leaves.First(prop => prop.PropertyName == "Summary").AddFilter(FilterOperator.StartsWith, "h");
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public new void then_an_iqueryable_of_the_querytree_type_should_be_returned()
        {
            SetupTest();
            Assert.Equal(typeof (List<>).MakeGenericType(QueryTree.Type), Actual.GetType());
        }
    }

    public class and_filter_type_is_contains : and_passing_a_query_tree_with_one_selected_field
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Leaves.First(prop => prop.PropertyName == "Summary").AddFilter(FilterOperator.Contains, "h");
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public new void then_an_iqueryable_of_the_querytree_type_should_be_returned()
        {
            SetupTest();
            Assert.Equal(typeof (List<>).MakeGenericType(QueryTree.Type), Actual.GetType());
        }
    }

    public class and_filter_type_is_startswith : and_passing_a_query_tree_with_one_selected_field
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Leaves.First(prop => prop.PropertyName == "Summary").AddFilter(FilterOperator.StartsWith, "h");
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public new void then_an_iqueryable_of_the_querytree_type_should_be_returned()
        {
            SetupTest();
            Assert.Equal(typeof (List<>).MakeGenericType(QueryTree.Type), Actual.GetType());
        }
    }

    public class and_filter_type_is_endswith : and_passing_a_query_tree_with_one_selected_field
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Leaves.First(prop => prop.PropertyName == "Summary").AddFilter(FilterOperator.EndsWith, "h");
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public new void then_an_iqueryable_of_the_querytree_type_should_be_returned()
        {
            SetupTest();
            Assert.Equal(typeof (List<>).MakeGenericType(QueryTree.Type), Actual.GetType());
        }
    }

    public class and_filter_type_is_equal_to : and_passing_a_query_tree_with_one_selected_field
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Leaves.First(prop => prop.PropertyName == "Summary").AddFilter(FilterOperator.IsEqualTo, "h");
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public new void then_an_iqueryable_of_the_querytree_type_should_be_returned()
        {
            SetupTest();
            Assert.Equal(typeof (List<>).MakeGenericType(QueryTree.Type), Actual.GetType());
        }
    }

    public class and_filter_type_is_not_equal_to : and_passing_a_query_tree_with_one_selected_field
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Leaves.First(prop => prop.PropertyName == "Summary").AddFilter(FilterOperator.IsNotEqualTo, "h");
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public new void then_an_iqueryable_of_the_querytree_type_should_be_returned()
        {
            SetupTest();
            Assert.Equal(typeof (List<>).MakeGenericType(QueryTree.Type), Actual.GetType());
        }
    }

    public class and_passing_an_empty_query_tree : and_calling_the_execute_query_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Type = typeof (EntityClass1);
            Expected = null;
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public void then_a_null_object_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_passing_a_query_tree_with_no_fields_selected : and_calling_the_execute_query_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree.Type = typeof (EntityClass1);
            QueryTree.Leaves.Add(new QueryLeaf {PropertyName = "Property1", Selected = 0});
            QueryTree.Leaves.Add(new QueryLeaf {PropertyName = "Property2", Selected = 0});
            Expected = null;
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public void then_a_null_object_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_passing_a_query_tree_with_an_aggregate_filter : and_calling_the_execute_query_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryTree = QueryTreeFactory.Create(typeof (Employee));
            QueryTree.AddAggregateFilter(new AggregateFilterDescriptor
                                             {
                                                 AggregateFunction = AggregateFunction.Count,
                                                 FilterOperator = FilterOperator.IsGreaterThanOrEqualTo,
                                                 PropertyName = "Children",
                                                 Value = 2
                                             });
            QueryTree.FindLeafByPropertyFullPath("Employee.FirstName").Selected = 1;
            QueryTree.FindLeafByPropertyFullPath("Employee.LastName").Selected = 2;
            QueryTree.FindLeafByPropertyFullPath("Employee.DateOfBirth").Selected = 3;
            QueryTree.FindByFullClassPath("Employee.Children").FindLeafByPropertyFullPath("Employee.Children.FirstName")
                .Selected = 1;
            QueryTree.FindByFullClassPath("Employee.Children").FindLeafByPropertyFullPath("Employee.Children.LastName").
                Selected = 2;
            Actual = QueryTreeService.ExecuteQuery(QueryTree);
        }

        [Fact]
        public void then_a_list_of_the_query_tree_type_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Actual.GetType(), typeof (List<>).MakeGenericType(QueryTree.Type));
        }
    }
}