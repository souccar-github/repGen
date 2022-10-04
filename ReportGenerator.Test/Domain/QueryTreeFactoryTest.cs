using System;
using System.Linq;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.OrgChart.ValueObjects;
using HRIS.Domain.PMS.RootEntities;
using ReportGenerator.Test.TestClasses;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using Souccar.ReportGenerator.Test.TestClasses;
using Xunit;

namespace Souccar.ReportGenerator.Test.Domain
{
    public class when_dealing_with_the_query_tree_factory_class
    {
        protected QueryTree _actual;
        protected QueryTree _expected;
    }

    public class and_calling_the_create_query_tree_method : when_dealing_with_the_query_tree_factory_class
    {
    }

    public class and_passing_the_jobdescription_class : and_calling_the_create_query_tree_method
    {
        [Fact]
        private void OutputJDQueryTreeToFile()
        {
            DateTime d1 = DateTime.Now;
            QueryTree actual = QueryTreeFactory.Create(typeof(JobDescription));
            DateTime d2 = DateTime.Now;
            TimeSpan t = d2 - d1;
        }
    }

    public class and_passing_a_class : and_calling_the_create_query_tree_method
    {
        private string _expectedName;

        private void SetupTest()
        {
            _actual = QueryTreeFactory.Create(typeof (EntityClass1));
            _expectedName = typeof (EntityClass1).FullName;
        }

        [Fact]
        public void then_full_class_path_of_the_query_tree_should_equal_type_name()
        {
            SetupTest();
            Assert.Equal(typeof (EntityClass1).Name, _actual.FullClassPath);
        }

        [Fact]
        public void then_full_class_name_of_the_query_tree_should_equal_type_full_name()
        {
            SetupTest();
            Assert.Equal(typeof (EntityClass1).FullName, _actual.FullClassName);
        }

        [Fact]
        public void then_type_of_the_query_tree_should_equal_type()
        {
            SetupTest();
            Assert.Equal(typeof (EntityClass1), _actual.Type);
        }

        [Fact]
        public void then_the_defining_type_of_query_tree_should_equal_null()
        {
            SetupTest();
            Assert.Equal(null, _actual.DefiningType);
        }
    }

    public class and_passing_an_aggregate_class_with_simple_properties : and_calling_the_create_query_tree_method
    {
        private void SetupTest()
        {
            _actual = QueryTreeFactory.Create(typeof (EntityClass1));
            _expected = new QueryTree {FullClassName = typeof (EntityClass1).FullName, FullClassPath = "EntityClass1"};
            _expected.Leaves.Add(new QueryLeaf
                                     {
                                         PropertyType = typeof (int),
                                         ParentType = typeof (EntityClass1),
                                         IsPrimitive = true,
                                         PropertyName = "Property1",
                                         DefiningType = typeof (EntityClass1),
                                         PropertyFullPath = typeof (EntityClass1).Name + ".Property1"
                                     });
            _expected.Leaves.Add(new QueryLeaf
                                     {
                                         PropertyType = typeof (string),
                                         ParentType = typeof (EntityClass1),
                                         IsPrimitive = true,
                                         PropertyName = "Property2",
                                         DefiningType = typeof (EntityClass1),
                                         PropertyFullPath = typeof (EntityClass1).Name + ".Property2"
                                     });
            _expected.Leaves.Add(new QueryLeaf
                                     {
                                         PropertyType = typeof (DateTime),
                                         ParentType = typeof (EntityClass1),
                                         IsPrimitive = true,
                                         PropertyName = "Property3",
                                         DefiningType = typeof (EntityClass1),
                                         PropertyFullPath = typeof (EntityClass1).Name + ".Property3"
                                     });
        }

        [Fact]
        public void then_a_query_tree_with_primitive_leaves_only_should_be_returned()
        {
            SetupTest();
            Assert.Equal(0, _actual.Nodes.Count);
            Assert.Equal(_expected.Leaves.Count, _actual.Leaves.Count);
            Assert.True(_expected.Leaves.SequenceEqual(_actual.Leaves));
        }

        [Fact]
        public void then_the_leaves_should_be_is_primitive_and_not_is_reference()
        {
            SetupTest();
            Assert.True(_actual.Leaves.All(leaf => leaf.IsPrimitive && !leaf.IsReference));
        }

        [Fact]
        public void then_the_full_class_path_of_the_query_tree_equals_name_of_class()
        {
            SetupTest();
            Assert.Equal(_expected.FullClassPath, _actual.FullClassPath);
        }

        [Fact]
        public void then_the_leave_full_property_path_should_be_equal_to_class_name_dot_propert_name()
        {
            SetupTest();
            Assert.True(
                _expected.Leaves.Select(prop => prop.PropertyFullPath).SequenceEqual(
                    _actual.Leaves.Select(prop => prop.PropertyFullPath)));
        }
    }

    public class and_passing_a_class_with_Id_property : and_calling_the_create_query_tree_method
    {
        public void SetupTest()
        {
            _actual = QueryTreeFactory.Create(typeof (IndexClass1));
            _expected = new QueryTree();
            _expected.Leaves.Add(new QueryLeaf
                                     {
                                         PropertyType = typeof (string),
                                         PropertyFullPath = String.Format("{0}.Name", typeof (IndexClass1).Name),
                                         ParentType = typeof (IndexClass1),
                                         IsPrimitive = true,
                                         IsReference = false,
                                         PropertyName = "Name",
                                         DefiningType = typeof (IndexClass1)
                                     });
        }

        [Fact]
        public void then_the_id_property_should_not_be_available_in_the_query_tree()
        {
            SetupTest();
            Assert.True(
                _actual.Leaves.Find(leaf => leaf.PropertyFullPath == String.Format("{0}.Id", typeof (IndexClass1).Name)) ==
                null);
        }
    }

    public class and_passing_an_aggregate_class_with_references : and_calling_the_create_query_tree_method
    {
        private void SetupTest()
        {
            _actual = QueryTreeFactory.Create(typeof (ClassWithIndexes));
            _expected = new QueryTree();
            _expected.Leaves.Add(new QueryLeaf
                                     {
                                         PropertyType = typeof (string),
                                         ParentType = typeof (IndexClass1),
                                         IsPrimitive = true,
                                         IsReference = true,
                                         PropertyName = "Index1.Name",
                                         DefiningType = typeof (ClassWithIndexes),
                                         PropertyFullPath = "ClassWithIndexes.Index1.Name"
                                     });
            _expected.Leaves.Add(new QueryLeaf
                                     {
                                         PropertyType = typeof (string),
                                         ParentType = typeof (IndexClass2),
                                         IsPrimitive = true,
                                         IsReference = true,
                                         PropertyName = "Index2.Name",
                                         DefiningType = typeof (ClassWithIndexes),
                                         PropertyFullPath = "ClassWithIndexes.Index2.Name"
                                     });
        }

        [Fact]
        public void then_the_leaves_should_be_is_primitive_and_is_reference()
        {
            SetupTest();
            Assert.True(_actual.Leaves.All(leaf => leaf.IsPrimitive && leaf.IsReference));
        }

        [Fact]
        public void then_a_query_tree_with_reference_leaves_only_should_be_returned()
        {
            SetupTest();
            Assert.Equal(0, _actual.Nodes.Count);
            Assert.Equal(_expected.Leaves.Count, _actual.Leaves.Count);
            Assert.True(_expected.Leaves.SequenceEqual(_actual.Leaves));
        }
    }

    public class and_passing_an_aggregate_class_with_details : and_calling_the_create_query_tree_method
    {
        private void SetupTest()
        {
            _actual = QueryTreeFactory.Create(typeof (EntityClassParent));
            var entityClassChildQueryTree = new QueryTree
                                                {
                                                    FullClassName = typeof (EntityClassChild).FullName,
                                                    DefiningType = typeof (EntityClassParent),
                                                    Type = typeof (EntityClassChild),
                                                    FullClassPath = "EntityClassParent.Children"
                                                };
            entityClassChildQueryTree.Leaves.Add(new QueryLeaf
                                                     {
                                                         PropertyType = typeof (string),
                                                         ParentType = typeof (EntityClassChild),
                                                         IsPrimitive = true,
                                                         IsReference = false,
                                                         PropertyName = "Name",
                                                         DefiningType = typeof (EntityClassChild),
                                                         PropertyFullPath = "EntityClassParent.Children.Name"
                                                     });
            _expected = new QueryTree
                            {FullClassName = typeof (EntityClassParent).FullName, FullClassPath = "EntityClassParent"};
            _expected.Nodes.Add(entityClassChildQueryTree);
        }

        [Fact]
        public void then_a_query_tree_with_nodes_should_be_returned()
        {
            SetupTest();
            Assert.Equal(0, _actual.Leaves.Count);
            Assert.Equal(_expected.Nodes.Count, _actual.Nodes.Count);
            Assert.True(_expected.Nodes.SequenceEqual(_actual.Nodes));
        }

        [Fact]
        public void then_the_type_of_the_detail_query_tree_should_be_equal_to_the_child_type()
        {
            SetupTest();
            Assert.True(_actual.Nodes.SingleOrDefault(node => node.Type == typeof (EntityClassChild)) != null);
        }

        [Fact]
        public void then_the_full_class_path_should_equal_to_parent_class_name_dot_child_class_name()
        {
            SetupTest();
            Assert.Equal("EntityClassParent.Children",
                         _actual.Nodes.SingleOrDefault(node => node.Type == typeof (EntityClassChild)).FullClassPath);
        }

        [Fact]
        public void then_the_defining_type_of_the_child_node_should_equal_to_the_parent_type()
        {
            SetupTest();
            Assert.Equal(typeof (EntityClassParent), _actual.Nodes[0].DefiningType);
        }

        [Fact]
        public void then_the_full_property_path_should_be_equal_query_tree_full_class_path_dot_property_name()
        {
            SetupTest();
            Assert.True(
                _expected.Nodes[0].Leaves.Select(leaf => leaf.PropertyFullPath).SequenceEqual(
                    _actual.Nodes[0].Leaves.Select(leaf => leaf.PropertyFullPath)));
        }
    }

    public class and_passing_a_class_with_two_details_and_one_with_another_level :
        and_calling_the_create_query_tree_method
    {
        private string _actualClassPath;
        private string _expectedClassPath;

        private void SetupTest()
        {
            _actual = QueryTreeFactory.Create(typeof (EntityClassWithTwoLevels));
            _actualClassPath =
                _actual.Nodes.First(tree => tree.Type == typeof (EntityClassLevel1)).Nodes.First(
                    tree => tree.Type == typeof (EntityClassLevel2)).FullClassPath;
            _expectedClassPath = "EntityClassWithTwoLevels.EntityClassLevel1s.EntityClassLevel2S";
        }

        [Fact]
        public void then_the_computer_skill_call_path_should_be_jd_spec_computerskills()
        {
            SetupTest();
            Assert.Equal(_expectedClassPath, _actualClassPath);
        }
    }

    public class and_passing_a_class_with_some_properties_have_only_get_method :
        and_calling_the_create_query_tree_method
    {
        public void SetupTest()
        {
            _actual = QueryTreeFactory.Create(typeof (ClassWithGetterProperty));
            _expected = new QueryTree
                            {
                                FullClassPath = "ClassWithGetterProperty",
                                FullClassName = typeof (ClassWithGetterProperty).FullName
                            };
            _expected.Leaves.Add(new QueryLeaf
                                     {
                                         PropertyType = typeof (string),
                                         PropertyFullPath =
                                             String.Format("{0}.FirstName", typeof (ClassWithGetterProperty).Name),
                                         ParentType = typeof (ClassWithGetterProperty),
                                         IsPrimitive = true,
                                         PropertyName = "FirstName",
                                         DefiningType = typeof (ClassWithGetterProperty)
                                     });
            _expected.Leaves.Add(new QueryLeaf
                                     {
                                         PropertyType = typeof (string),
                                         PropertyFullPath =
                                             String.Format("{0}.LastName", typeof (ClassWithGetterProperty).Name),
                                         ParentType = typeof (ClassWithGetterProperty),
                                         IsPrimitive = true,
                                         PropertyName = "LastName",
                                         DefiningType = typeof (ClassWithGetterProperty)
                                     });
        }

        [Fact]
        public void then_the_getter_property_should_not_be_added_in_the_query_tree()
        {
            SetupTest();
            Assert.Equal(0, _actual.Nodes.Count);
            Assert.Equal(_expected.Leaves.Count, _actual.Leaves.Count);
            Assert.True(_expected.Leaves.SequenceEqual(_actual.Leaves), "Leaves are not the same");
        }
    }

    public class and_passing_a_class_with_some_properties_have_only_get_method_in_the_reference :
        and_calling_the_create_query_tree_method
    {
        public void SetupTest()
        {
            _actual = QueryTreeFactory.Create(typeof (ClassReferencesClassWithGetterProperty));
            _expected = new QueryTree
                            {
                                FullClassPath = "ClassWithGetterProperty",
                                FullClassName = typeof (ClassReferencesClassWithGetterProperty).FullName
                            };
            _expected.Leaves.Add(new QueryLeaf
                                     {
                                         PropertyType = typeof (string),
                                         PropertyFullPath = "ClassReferencesClassWithGetterProperty.Name",
                                         ParentType = typeof (ClassReferencesClassWithGetterProperty),
                                         IsPrimitive = true,
                                         PropertyName = "Name",
                                         DefiningType = typeof (ClassReferencesClassWithGetterProperty),
                                         IsReference = false
                                     });
            _expected.Leaves.Add(new QueryLeaf
                                     {
                                         PropertyType = typeof (string),
                                         PropertyFullPath =
                                             "ClassReferencesClassWithGetterProperty.ClassWithGetterProperty.FirstName",
                                         ParentType = typeof (ClassWithGetterProperty),
                                         IsPrimitive = true,
                                         PropertyName = "ClassWithGetterProperty.FirstName",
                                         DefiningType = typeof (ClassReferencesClassWithGetterProperty),
                                         IsReference = true
                                     });
            _expected.Leaves.Add(new QueryLeaf
                                     {
                                         PropertyType = typeof (string),
                                         PropertyFullPath =
                                             "ClassReferencesClassWithGetterProperty.ClassWithGetterProperty.LastName",
                                         ParentType = typeof (ClassWithGetterProperty),
                                         IsPrimitive = true,
                                         PropertyName = "ClassWithGetterProperty.LastName",
                                         DefiningType = typeof (ClassReferencesClassWithGetterProperty),
                                         IsReference = true
                                     });
        }

        [Fact]
        public void then_the_getter_property_should_not_be_added_in_the_query_tree()
        {
            SetupTest();
            Assert.Equal(0, _actual.Nodes.Count);
            Assert.Equal(_expected.Leaves.Count, _actual.Leaves.Count);
            Assert.True(_expected.Leaves.SequenceEqual(_actual.Leaves), "Leaves are not the same");
        }
    }

    public class and_passing_a_class_with_a_private_setter : and_calling_the_create_query_tree_method
    {
        public void SetupTest()
        {
            _actual = QueryTreeFactory.Create(typeof (ClassWithPrivateSetter));
            _expected = new QueryTree
                            {
                                FullClassPath = "ClassWithPrivateSetter",
                                FullClassName = typeof (ClassWithPrivateSetter).FullName
                            };
            _expected.Leaves.Add(new QueryLeaf
                                     {
                                         PropertyType = typeof (string),
                                         PropertyFullPath = "ClassWithPrivateSetter.FirstName",
                                         ParentType = typeof (ClassWithPrivateSetter),
                                         IsPrimitive = true,
                                         PropertyName = "FirstName",
                                         DefiningType = typeof (ClassWithPrivateSetter)
                                     });
        }

        [Fact]
        public void then_the_private_setter_property_should_be_returned()
        {
            SetupTest();
            Assert.Equal(0, _actual.Nodes.Count);
            Assert.Equal(_expected.Leaves.Count, _actual.Leaves.Count);
            Assert.True(_expected.Leaves.SequenceEqual(_actual.Leaves), "Leaves are not the same");
        }
    }
}