using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.Personnel.Entities;
using ReportGenerator.Test.TestClasses;
using SharpTestsEx;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using Souccar.ReportGenerator.Test.TestClasses;
using Xunit;

namespace Souccar.ReportGenerator.Test.Domain
{
    public class when_dealing_with_query_tree_class
    {
        protected QueryTree QueryTree;

        protected virtual void SetupTest()
        {
        }
    }

    public class and_calling_find_by_full_class_path_method : when_dealing_with_query_tree_class
    {
        protected QueryTree Actual;
        protected QueryTree Expected;

        protected override void SetupTest()
        {
            QueryTree = QueryTreeFactory.Create(typeof (EntityClassParent));
        }
    }

    public class and_passing_an_empty_string : and_calling_find_by_full_class_path_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            Actual = QueryTree.FindByFullClassPath("");
            Expected = null;
        }

        [Fact]
        public void then_a_null_query_tree_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_passing_a_full_class_path_that_does_not_exist_in_first_level :
        and_calling_find_by_full_class_path_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            Actual = QueryTree.FindByFullClassPath("Halololololo");
        }

        [Fact]
        public void then_a_null_query_tree_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_passing_a_full_class_path_of_the_query_tree : and_calling_find_by_full_class_path_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            Actual = QueryTree.FindByFullClassPath("EntityClassParent");
            Expected = QueryTree;
        }

        [Fact]
        public void then_the_original_query_tree_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_passing_a_full_class_path_of_a_query_tree_in_first_level :
        and_calling_find_by_full_class_path_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            Actual = QueryTree.FindByFullClassPath("EntityClassParent.Children");
            Expected = new QueryTree
                           {
                               FullClassName = typeof (EntityClassChild).FullName,
                               Type = typeof (EntityClassChild),
                               FullClassPath = "EntityClassParent.Children",
                               DefiningType = typeof (EntityClassParent)
                           };
            Expected.Leaves.Add(new QueryLeaf
                                    {
                                        PropertyType = typeof (string),
                                        PropertyFullPath = String.Format("EntityClassParent.Children.Name"),
                                        ParentType = typeof (EntityClassChild),
                                        DefiningType = typeof (EntityClassChild),
                                        IsPrimitive = true,
                                        IsReference = false,
                                        PropertyName = "Name"
                                    });
        }

        [Fact]
        public void then_the_matching_query_tree_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected.FullClassPath, Actual.FullClassPath);
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_passing_a_full_class_path_of_a_query_tree_in_second_level :
        and_calling_find_by_full_class_path_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            QueryTree = QueryTreeFactory.Create(typeof (EntityClassWithTwoLevels));
            Actual = QueryTree.FindByFullClassPath("EntityClassWithTwoLevels.EntityClassLevel1s.EntityClassLevel2S");
            Expected = new QueryTree
                           {
                               FullClassName = typeof (EntityClassLevel2).FullName,
                               Type = typeof (EntityClassLevel2),
                               FullClassPath = "EntityClassWithTwoLevels.EntityClassLevel1s.EntityClassLevel2S"
                           };
            Expected.Leaves.Add(new QueryLeaf
                                    {
                                        PropertyType = typeof (string),
                                        PropertyFullPath =
                                            "EntityClassWithTwoLevels.EntityClassLevel1s.EntityClassLevel2S.Name",
                                        PropertyName = "Name",
                                        ParentType = typeof (EntityClassLevel2),
                                        DefiningType = typeof (EntityClassLevel2),
                                        IsPrimitive = true,
                                        IsReference = false,
                                    });
        }

        [Fact]
        public void then_the_matching_query_tree_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected.FullClassPath, Actual.FullClassPath);
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_calling_find_leaf_by_property_full_path_method : when_dealing_with_query_tree_class
    {
        protected QueryLeaf Actual;
        protected QueryLeaf Expected;
        protected QueryTree QueryTree;

        protected override void SetupTest()
        {
            QueryTree = QueryTreeFactory.Create(typeof (EntityClassParent));
        }
    }

    public class and_passing_an_empty_string_to_property_full_name : and_calling_find_leaf_by_property_full_path_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            Expected = null;
            Actual = QueryTree.FindLeafByPropertyFullPath("");
        }

        [Fact]
        public void then_a_null_query_leaf_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_passing_a_property_full_name_that_does_not_exists :
        and_calling_find_leaf_by_property_full_path_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            QueryTree = new QueryTree();
            QueryTree.Leaves.Add(new QueryLeaf {PropertyFullPath = "Entity1.Property1"});
            Expected = null;
            Actual = QueryTree.FindLeafByPropertyFullPath("Entity1.Property2");
        }

        [Fact]
        public void then_a_null_query_leaf_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_passing_a_property_full_path_that_exists : and_calling_find_leaf_by_property_full_path_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            QueryTree = new QueryTree();
            QueryTree.Leaves.Add(new QueryLeaf
                                     {PropertyFullPath = "EntityClass1.Property1", ParentType = typeof (EntityClass1)});
            QueryTree.Leaves.Add(new QueryLeaf
                                     {PropertyFullPath = "EntityClass1.Property2", ParentType = typeof (EntityClass1)});
            Expected = QueryTree.Leaves[0];
            Actual = QueryTree.FindLeafByPropertyFullPath("EntityClass1.Property1");
        }

        [Fact]
        public void then_the_correct_query_leaf_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected.PropertyFullPath, Actual.PropertyFullPath);
            Assert.Equal(Expected.PropertyType, Actual.PropertyType);
        }
    }

    public class and_calling_display_name_property : when_dealing_with_query_tree_class
    {
        protected string Actual;
        protected string Expected;

        protected override void SetupTest()
        {
            base.SetupTest();
        }
    }

    public class and_the_query_tree_defining_type_is_null : and_calling_display_name_property
    {
        protected override void SetupTest()
        {
            base.SetupTest();
        }
    }

    public class and_the_query_tree_type_has_no_metadata : and_the_query_tree_defining_type_is_null
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            QueryTree = new QueryTree
                            {
                                Type = typeof (EntityClass1),
                                FullClassPath = "EntityClass1",
                                FullClassName = typeof (EntityClass1).FullName
                            };
            Actual = QueryTree.DisplayName;
            Expected = "EntityClass1";
        }

        [Fact]
        public void then_the_name_of_the_class_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_query_tree_type_has_metadata_with_class_localization_attribute :
        and_the_query_tree_defining_type_is_null
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            QueryTree = new QueryTree
                            {
                                Type = typeof (IndexClass1),
                                FullClassName = typeof (IndexClass1).FullName,
                                FullClassPath = "IndexClass1"
                            };
            Actual = QueryTree.DisplayName;
            Expected = "Blood Group";
        }

        [Fact]
        public void then_the_localized_name_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_query_tree_type_metadata_has_no_localization_attribute :
        and_the_query_tree_defining_type_is_null
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            QueryTree = new QueryTree
                            {
                                Type = typeof (ClassWithPrivateSetter),
                                FullClassName = typeof (ClassWithPrivateSetter).FullName,
                                FullClassPath = "ClassWithPrivateSetter"
                            };
            Actual = QueryTree.DisplayName;
            Expected = "ClassWithPrivateSetter";
        }

        [Fact]
        public void then_the_class_name_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_query_tree_defining_type_is_not_null : and_calling_display_name_property
    {
    }

    public class and_the_query_tree_defining_type_has_localization_attribute_for_the_collection_property :
        and_the_query_tree_defining_type_is_not_null
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            QueryTree = QueryTreeFactory.Create(typeof (EntityClassWithTwoLevels));
            Actual =
                QueryTree.Nodes.Find(node => node.Type == typeof (EntityClassLevel1)).Nodes.Find(
                    node => node.Type == typeof (ClassWithIndexes)).DisplayName;
            Expected = "Children";
        }

        [Fact]
        public void then_the_localized_name_of_collection_property_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_query_tree_defining_type_has_no_localization_attribute_for_the_collection_property :
        and_the_query_tree_defining_type_is_not_null
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            QueryTree = QueryTreeFactory.Create(typeof (EntityClassWithTwoLevels));
            Actual =
                QueryTree.Nodes.Find(node => node.Type == typeof (EntityClassLevel1)).Nodes.Find(
                    node => node.Type == typeof (EntityClassLevel2)).DisplayName;
            Expected = "EntityClassLevel2S";
        }

        [Fact]
        public void then_the_name_of_collection_property_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }


    public class and_attempting_to_get_the_available_aggregate_functions : when_dealing_with_query_tree_class
    {
        private Dictionary<AggregateFunction, string> _actual;
        private Dictionary<AggregateFunction, string> _expected;

        protected override void SetupTest()
        {
            base.SetupTest();
            QueryTree = new QueryTree();
            _expected = new Dictionary<AggregateFunction, string> {{AggregateFunction.Count, "Count"}};
            _actual = QueryTree.GetAvailableAggregateFilters();
        }

        [Fact]
        public void then_a_dictionary_with_count_function_should_be_returned()
        {
            SetupTest();
            _actual.Keys.Should().Have.SameSequenceAs(_expected.Keys);
            _actual.Values.Should().Have.SameSequenceAs(_expected.Values);
        }
    }

    public class and_attempting_to_change_order_of_nodes_in_query_tree : when_dealing_with_query_tree_class
    {
    }

    public class and_the_source_index_is_less_than_zero : and_attempting_to_change_order_of_nodes_in_query_tree
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            QueryTree = new QueryTree();
        }

        [Fact]
        public void then_an_index_out_of_range_exception_should_be_thrown()
        {
            SetupTest();
            Assert.Throws<IndexOutOfRangeException>(() => QueryTree.ChangeOrder(-1, 1));
        }
    }

    public class and_the_destination_index_is_less_than_zero : and_attempting_to_change_order_of_nodes_in_query_tree
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            QueryTree = new QueryTree();
        }

        [Fact]
        public void then_an_index_out_of_range_exception_should_be_thrown()
        {
            SetupTest();
            Assert.Throws<IndexOutOfRangeException>(() => QueryTree.ChangeOrder(1, -1));
        }
    }

    public class and_the_source_index_is_less_than_destination_index_by_one :
        and_attempting_to_change_order_of_nodes_in_query_tree
    {
        private QueryTree _oldNode1;
        private QueryTree _oldNode2;

        protected override void SetupTest()
        {
            base.SetupTest();
            QueryTree = QueryTreeFactory.Create(typeof (Employee));
            _oldNode1 = QueryTree.Nodes.Single(node => node.SelectOrder == 1);
            _oldNode2 = QueryTree.Nodes.Single(node => node.SelectOrder == 2);
            QueryTree.ChangeOrder(1, 2);
        }

        [Fact]
        public void then_the_select_order_of_the_corresponding_nodes_should_be_swapped()
        {
            SetupTest();
            _oldNode1.SelectOrder.Should().Be.EqualTo(2);
            _oldNode2.SelectOrder.Should().Be.EqualTo(1);
        }
    }

    public class and_the_source_index_is_less_than_destination_index_by_two :
        and_attempting_to_change_order_of_nodes_in_query_tree
    {
        private QueryTree _oldNode1;
        private QueryTree _oldNode2;
        private QueryTree _oldNode3;

        protected override void SetupTest()
        {
            base.SetupTest();
            QueryTree = QueryTreeFactory.Create(typeof (Employee));
            _oldNode1 = QueryTree.Nodes.Single(node => node.SelectOrder == 1);
            _oldNode2 = QueryTree.Nodes.Single(node => node.SelectOrder == 2);
            _oldNode3 = QueryTree.Nodes.Single(node => node.SelectOrder == 3);
            QueryTree.ChangeOrder(1, 3);
        }

        [Fact]
        public void then_the_select_order_of_the_nodes_between_source_and_destination_should_be_decreased_by_one()
        {
            SetupTest();
            _oldNode1.SelectOrder.Should().Be.EqualTo(3);
            _oldNode2.SelectOrder.Should().Be.EqualTo(1);
            _oldNode3.SelectOrder.Should().Be.EqualTo(2);
        }
    }

    public class and_the_source_index_is_greater_than_destination_index_by_two :
        and_attempting_to_change_order_of_nodes_in_query_tree
    {
        private QueryTree _oldNode1;
        private QueryTree _oldNode2;
        private QueryTree _oldNode3;

        protected override void SetupTest()
        {
            base.SetupTest();
            QueryTree = QueryTreeFactory.Create(typeof (Employee));
            _oldNode1 = QueryTree.Nodes.Single(node => node.SelectOrder == 1);
            _oldNode2 = QueryTree.Nodes.Single(node => node.SelectOrder == 2);
            _oldNode3 = QueryTree.Nodes.Single(node => node.SelectOrder == 3);
            QueryTree.ChangeOrder(3, 1);
        }

        [Fact]
        public void then_the_select_order_of_the_nodes_between_source_and_destination_should_be_increased_by_one()
        {
            SetupTest();
            _oldNode1.SelectOrder.Should().Be.EqualTo(2);
            _oldNode2.SelectOrder.Should().Be.EqualTo(3);
            _oldNode3.SelectOrder.Should().Be.EqualTo(1);
        }
    }

    public class and_the_source_index_is_greater_than_destination_index_by_three :
        and_attempting_to_change_order_of_nodes_in_query_tree
    {
        private QueryTree _oldNode1;
        private QueryTree _oldNode2;
        private QueryTree _oldNode3;
        private QueryTree _oldNode4;

        protected override void SetupTest()
        {
            base.SetupTest();
            QueryTree = QueryTreeFactory.Create(typeof (Employee));
            _oldNode1 = QueryTree.Nodes.Single(node => node.SelectOrder == 2);
            _oldNode2 = QueryTree.Nodes.Single(node => node.SelectOrder == 3);
            _oldNode3 = QueryTree.Nodes.Single(node => node.SelectOrder == 4);
            _oldNode4 = QueryTree.Nodes.Single(node => node.SelectOrder == 5);
            QueryTree.ChangeOrder(5, 2);
        }

        [Fact]
        public void then_the_select_order_of_the_nodes_between_source_and_destination_should_be_increased_by_one()
        {
            SetupTest();
            _oldNode1.SelectOrder.Should().Be.EqualTo(3);
            _oldNode2.SelectOrder.Should().Be.EqualTo(4);
            _oldNode3.SelectOrder.Should().Be.EqualTo(5);
            _oldNode4.SelectOrder.Should().Be.EqualTo(2);
        }
    }
}