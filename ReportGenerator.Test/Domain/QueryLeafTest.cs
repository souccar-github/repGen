using System;
using System.Collections.Generic;
using System.Linq;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using Souccar.ReportGenerator.Test.TestClasses;
using Xunit;

namespace Souccar.ReportGenerator.Test.Domain
{
    public class when_dealing_with_query_leaf_class
    {
        protected virtual void SetupTest()
        {
        }
    }

    public class and_calling_get_display_name : when_dealing_with_query_leaf_class
    {
        protected string Actual;
        protected string Expected;
        protected QueryLeaf QueryLeaf;
    }

    public class and_the_property_parent_type_has_no_metadata_attribute : and_calling_get_display_name
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf = new QueryLeaf();
        }
    }

    public class and_the_property_is_reference : and_calling_get_display_name
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf = new QueryLeaf
                            {
                                PropertyName = "Index2.Name",
                                PropertyType = typeof (string),
                                PropertyFullPath = typeof (ClassWithIndexes).Name + "Index2.Name",
                                ParentType = typeof (IndexClass2),
                                DefiningType = typeof (ClassWithIndexes),
                                IsPrimitive = true,
                                IsReference = true
                            };
            Expected = "Last Name->First Name";
            Actual = QueryLeaf.DisplayName;
        }

        [Fact]
        public void then_the_localized_property_name_from_the_defining_class_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_property_has_no_localization_attribute : and_the_property_parent_type_has_no_metadata_attribute
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.PropertyName = "Property1";
            QueryLeaf.PropertyType = typeof (string);
            QueryLeaf.PropertyFullPath = "EntityClass1.Property1";
            QueryLeaf.ParentType = typeof (EntityClass1);
            QueryLeaf.DefiningType = typeof (EntityClass1);
            QueryLeaf.IsPrimitive = true;
            QueryLeaf.IsReference = false;
            Expected = "Property1";
            Actual = QueryLeaf.DisplayName;
        }

        [Fact]
        public void then_the_property_name_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_property_has_localization_attribute : and_the_property_parent_type_has_no_metadata_attribute
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.PropertyName = "Name";
            QueryLeaf.PropertyType = typeof (string);
            QueryLeaf.PropertyFullPath = "IndexClass2.Name";
            QueryLeaf.ParentType = typeof (IndexClass2);
            QueryLeaf.DefiningType = typeof (IndexClass2);
            QueryLeaf.IsPrimitive = true;
            QueryLeaf.IsReference = false;
            Expected = "First Name";
            Actual = QueryLeaf.DisplayName;
        }

        [Fact]
        public void then_the_localized_display_name_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_property_parent_type_has_metadata_attribute : and_calling_get_display_name
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf = new QueryLeaf();
        }
    }

    public class and_the_metadata_has_no_localization_attribute_for_the_property :
        and_the_property_parent_type_has_metadata_attribute
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.PropertyName = "Name";
            QueryLeaf.PropertyType = typeof (string);
            QueryLeaf.PropertyFullPath = "IndexClass1.Name";
            QueryLeaf.ParentType = typeof (IndexClass1);
            QueryLeaf.DefiningType = typeof (IndexClass1);
            QueryLeaf.IsPrimitive = true;
            QueryLeaf.IsReference = false;
            Expected = "Name";
            Actual = QueryLeaf.DisplayName;
        }

        [Fact]
        public void then_the_property_name_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_metadata_has_localization_attribute_for_the_property :
        and_the_property_parent_type_has_metadata_attribute
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.PropertyName = "FirstName";
            QueryLeaf.PropertyType = typeof (string);
            QueryLeaf.PropertyFullPath = "ClassWithPrivateSetter.FirstName";
            QueryLeaf.ParentType = typeof (ClassWithPrivateSetter);
            QueryLeaf.DefiningType = typeof (ClassWithPrivateSetter);
            QueryLeaf.IsPrimitive = true;
            QueryLeaf.IsReference = false;
            Expected = "First Name";
            Actual = QueryLeaf.DisplayName;
        }

        [Fact]
        public void then_the_localized_display_name_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    #region GetAvailableFilterOperatorsTests

    public class and_calling_get_available_filter_operators_method : when_dealing_with_query_leaf_class
    {
        protected Dictionary<FilterOperator, string> _actual;
        protected Dictionary<FilterOperator, string> _expected;

        protected override void SetupTest()
        {
            _expected = new Dictionary<FilterOperator, string>();
            base.SetupTest();
        }
    }

    public class and_query_leaf_type_is_string : and_calling_get_available_filter_operators_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            _expected.Add(FilterOperator.IsEqualTo, "Is equal to");
            _expected.Add(FilterOperator.IsNotEqualTo, "Is not equal to");
            _expected.Add(FilterOperator.StartsWith, "Starts with");
            _expected.Add(FilterOperator.Contains, "Contains");
            _expected.Add(FilterOperator.EndsWith, "Ends with");

            var queryLeaf = new QueryLeaf {PropertyType = typeof (string)};
            _actual = queryLeaf.GetAvailableFilterOperators();
        }

        [Fact]
        public void then_a_list_of_string_operators_should_be_returned()
        {
            SetupTest();
            Assert.True(_expected.SequenceEqual(_actual));
        }
    }

    #region Numeric Query Leaf Filters Test

    public class and_query_leaf_type_is_number : and_calling_get_available_filter_operators_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            _expected.Add(FilterOperator.IsEqualTo, "Is equal to");
            _expected.Add(FilterOperator.IsNotEqualTo, "Is not equal to");
            _expected.Add(FilterOperator.IsGreaterThan, "Is greater than");
            _expected.Add(FilterOperator.IsGreaterThanOrEqualTo, "Is greater than or equal to");
            _expected.Add(FilterOperator.IsLessThan, "Is less than");
            _expected.Add(FilterOperator.IsLessThanOrEqualTo, "Is less than or equal to");
        }
    }

    public class and_number_type_is_int : and_query_leaf_type_is_number
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            _actual = new QueryLeaf {PropertyType = typeof (int)}.GetAvailableFilterOperators();
        }

        [Fact]
        public void then_a_list_of_number_operators_should_be_returned()
        {
            SetupTest();
            Assert.True(_expected.SequenceEqual(_actual));
        }
    }

    public class and_number_type_is_short : and_query_leaf_type_is_number
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            _actual = new QueryLeaf {PropertyType = typeof (short)}.GetAvailableFilterOperators();
        }

        [Fact]
        public void then_a_list_of_number_operators_should_be_returned()
        {
            SetupTest();
            Assert.True(_expected.SequenceEqual(_actual));
        }
    }

    public class and_number_type_is_long : and_query_leaf_type_is_number
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            _actual = new QueryLeaf {PropertyType = typeof (long)}.GetAvailableFilterOperators();
        }

        [Fact]
        public void then_a_list_of_number_operators_should_be_returned()
        {
            SetupTest();
            Assert.True(_expected.SequenceEqual(_actual));
        }
    }

    public class and_number_type_is_unsigned_long : and_query_leaf_type_is_number
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            _actual = new QueryLeaf {PropertyType = typeof (ulong)}.GetAvailableFilterOperators();
        }

        [Fact]
        public void then_a_list_of_number_operators_should_be_returned()
        {
            SetupTest();
            Assert.True(_expected.SequenceEqual(_actual));
        }
    }

    public class and_number_type_is_unsigned_int : and_query_leaf_type_is_number
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            _actual = new QueryLeaf {PropertyType = typeof (uint)}.GetAvailableFilterOperators();
        }

        [Fact]
        public void then_a_list_of_number_operators_should_be_returned()
        {
            SetupTest();
            Assert.True(_expected.SequenceEqual(_actual));
        }
    }

    public class and_number_type_is_unsigned_short : and_query_leaf_type_is_number
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            _actual = new QueryLeaf {PropertyType = typeof (ushort)}.GetAvailableFilterOperators();
        }

        [Fact]
        public void then_a_list_of_number_operators_should_be_returned()
        {
            SetupTest();
            Assert.True(_expected.SequenceEqual(_actual));
        }
    }

    public class and_number_type_is_float : and_query_leaf_type_is_number
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            _actual = new QueryLeaf {PropertyType = typeof (float)}.GetAvailableFilterOperators();
        }

        [Fact]
        public void then_a_list_of_number_operators_should_be_returned()
        {
            SetupTest();
            Assert.True(_expected.SequenceEqual(_actual));
        }
    }

    public class and_number_type_is_double : and_query_leaf_type_is_number
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            _actual = new QueryLeaf {PropertyType = typeof (double)}.GetAvailableFilterOperators();
        }

        [Fact]
        public void then_a_list_of_number_operators_should_be_returned()
        {
            SetupTest();
            Assert.True(_expected.SequenceEqual(_actual));
        }
    }

    public class and_number_type_is_decimal : and_query_leaf_type_is_number
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            _actual = new QueryLeaf {PropertyType = typeof (decimal)}.GetAvailableFilterOperators();
        }

        [Fact]
        public void then_a_list_of_number_operators_should_be_returned()
        {
            SetupTest();
            Assert.True(_expected.SequenceEqual(_actual));
        }
    }

    #endregion

    public class and_query_leaf_type_is_datetime : and_calling_get_available_filter_operators_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            _expected.Add(FilterOperator.IsEqualTo, "Is equal to");
            _expected.Add(FilterOperator.IsNotEqualTo, "Is not equal to");
            _expected.Add(FilterOperator.IsGreaterThan, "Is greater than");
            _expected.Add(FilterOperator.IsGreaterThanOrEqualTo, "Is greater than or equal to");
            _expected.Add(FilterOperator.IsLessThan, "Is less than");
            _expected.Add(FilterOperator.IsLessThanOrEqualTo, "Is less than or equal to");
            var queryLeaf = new QueryLeaf {PropertyType = typeof (DateTime)};
            _actual = queryLeaf.GetAvailableFilterOperators();
        }

        [Fact]
        public void then_a_list_of_datetime_operators_should_be_returned()
        {
            SetupTest();
            Assert.True(_expected.SequenceEqual(_actual));
        }
    }

    #endregion

    #region AddFilterTests

    public class and_calling_add_filter_method : when_dealing_with_query_leaf_class
    {
        protected QueryLeaf _queryLeaf;

        protected override void SetupTest()
        {
            _queryLeaf = new QueryLeaf();
        }
    }

    public class and_passing_a_null_value_for_filter_value : and_calling_add_filter_method
    {
        [Fact]
        public void then_an_invalid_argument_exception_should_be_thrown()
        {
            SetupTest();
            Assert.Throws<ArgumentNullException>(() => _queryLeaf.AddFilter(FilterOperator.IsEqualTo, null));
        }
    }

    public class and_passing_a_non_numeric_filter_operator_to_a_numeric_query_leaf : and_calling_add_filter_method
    {
        private FilterOperator _filterOperator;

        protected override void SetupTest()
        {
            base.SetupTest();
            _queryLeaf.PropertyType = typeof (int);
            _filterOperator = FilterOperator.Contains;
        }

        [Fact]
        public void then_an_argument_exception_is_thrown()
        {
            SetupTest();
            Assert.Throws<ArgumentException>(() => _queryLeaf.AddFilter(_filterOperator, 10));
        }
    }

    public class and_passing_a_non_string_filter_operator_to_a_string_query_leaf : and_calling_add_filter_method
    {
        private FilterOperator _filterOperator;

        protected override void SetupTest()
        {
            base.SetupTest();
            _queryLeaf.PropertyType = typeof (string);
            _filterOperator = FilterOperator.IsGreaterThanOrEqualTo;
        }

        [Fact]
        public void then_an_argument_exception_is_thrown()
        {
            SetupTest();
            Assert.Throws<ArgumentException>(() => _queryLeaf.AddFilter(_filterOperator, "hello"));
        }
    }

    public class and_passing_a_non_datetime_filter_operator_to_a_datetime_query_leaf : and_calling_add_filter_method
    {
        private FilterOperator _filterOperator;

        protected override void SetupTest()
        {
            base.SetupTest();
            _queryLeaf.PropertyType = typeof (DateTime);
            _filterOperator = FilterOperator.Contains;
        }

        [Fact]
        public void then_an_argument_exception_is_thrown()
        {
            SetupTest();
            Assert.Throws<ArgumentException>(() => _queryLeaf.AddFilter(_filterOperator, DateTime.Now));
        }
    }

    public class and_passing_a_non_datetime_value_to_a_datetime_query_leaf : and_calling_add_filter_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            _queryLeaf.PropertyType = typeof (DateTime);
        }

        [Fact]
        public void then_an_argument_exception_should_be_thrown()
        {
            SetupTest();
            Assert.Throws<ArgumentException>(() => _queryLeaf.AddFilter(FilterOperator.IsGreaterThan, "Hello"));
        }
    }

    public class and_passing_a_non_string_value_to_a_string_query_leaf : and_calling_add_filter_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            _queryLeaf.PropertyType = typeof (string);
        }

        [Fact]
        public void then_an_argument_exception_should_be_thrown()
        {
            SetupTest();
            Assert.Throws<ArgumentException>(() => _queryLeaf.AddFilter(FilterOperator.Contains, 10));
        }
    }

    public class and_passing_a_valid_value_and_a_valid_filter_operator : and_calling_add_filter_method
    {
        private FilterOperator _filterOperator;
        private object _value;

        protected override void SetupTest()
        {
            base.SetupTest();
            _filterOperator = FilterOperator.Contains;
            _value = "Hello World";
            _queryLeaf.PropertyType = typeof (string);
            _queryLeaf.AddFilter(_filterOperator, _value);
        }

        [Fact]
        public void then_a_new_filter_descriptor_should_be_added_to_the_filters_list()
        {
            SetupTest();
            Assert.Equal(1, _queryLeaf.FilterDescriptors.Count);
            Assert.Equal(_filterOperator, _queryLeaf.FilterDescriptors[0].FilterOperator);
            Assert.Equal(_value, _queryLeaf.FilterDescriptors[0].Value);
        }
    }

    #endregion

    #region Remove Filter Tests

    public class and_calling_remove_filter_method : when_dealing_with_query_leaf_class
    {
        protected QueryLeaf _queryLeaf;

        protected override void SetupTest()
        {
            _queryLeaf = new QueryLeaf {PropertyType = typeof (int)};
            _queryLeaf.AddFilter(FilterOperator.IsEqualTo, 1);
            _queryLeaf.AddFilter(FilterOperator.IsNotEqualTo, 10);
        }
    }

    public class and_passing_a_null_value_for_filter_descriptor : and_calling_remove_filter_method
    {
        [Fact]
        public void then_an_invalid_argument_exception_should_be_thrown()
        {
            SetupTest();
            Assert.Throws<ArgumentNullException>(() => _queryLeaf.RemoveFilter(null));
        }
    }

    public class and_passing_a_filter_descriptor_that_does_not_exists : and_calling_remove_filter_method
    {
        [Fact]
        public void then_an_argument_out_of_range_exception_should_be_thrown()
        {
            SetupTest();
            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                _queryLeaf.RemoveFilter(new FilterDescriptor {FilterOperator = FilterOperator.IsGreaterThan, Value = 5}));
        }
    }

    public class and_passing_a_filter_descriptor_that_exists : and_calling_remove_filter_method
    {
        private int _actual;
        private int _expected;

        protected override void SetupTest()
        {
            base.SetupTest();
            _expected = _queryLeaf.FilterDescriptors.Count - 1;
            _queryLeaf.RemoveFilter(new FilterDescriptor {FilterOperator = FilterOperator.IsEqualTo, Value = 1});
            _actual = _queryLeaf.FilterDescriptors.Count;
        }

        [Fact]
        public void then_the_number_of_filters_should_be_decreased()
        {
            SetupTest();
            Assert.Equal(_expected, _actual);
        }
    }

    #endregion
}