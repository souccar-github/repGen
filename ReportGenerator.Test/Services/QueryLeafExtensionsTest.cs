#region Using Statements

using System;
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
    public class when_dealing_with_query_leaf_extensions_class
    {
        public virtual void SetupTest()
        {
        }
    }

    #region Construct Filter Operations Tests

    public class and_calling_construct_filter_operation_method : when_dealing_with_query_leaf_extensions_class
    {
        protected String Actual;
        protected IClassMapping ClassMapping;
        protected String Expected;
        protected QueryLeaf QueryLeaf;

        public override void SetupTest()
        {
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

    #region String Query Leaf

    public class and_passing_a_string_query_leaf : and_calling_construct_filter_operation_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf = new QueryLeaf
                            {
                                Selected = 1,
                                IsPrimitive = true,
                                IsReference = false,
                                ParentType = typeof (Employee),
                                PropertyType = typeof (string),
                                PropertyName = "FirstName"
                            };
        }
    }

    public class and_the_operation_is_string_equal_to : and_passing_a_string_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsEqualTo, "hello");
            Expected =
                String.Format("{0}.{1}='hello'", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_string_equal_to_filter_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_operation_is_string_not_equal_to : and_passing_a_string_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsNotEqualTo, "hello");
            Expected =
                String.Format("{0}.{1}<>'hello'", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_string_not_equal_to_filter_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_operation_is_string_startswith : and_passing_a_string_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.StartsWith, "hello");
            Expected =
                String.Format("{0}.{1} like 'hello%'", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_string_startswith_filter_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_operation_is_string_endswith : and_passing_a_string_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.EndsWith, "hello");
            Expected =
                String.Format("{0}.{1} like '%hello'", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_string_endswith_filter_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_operation_is_string_contains : and_passing_a_string_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.Contains, "hello");
            Expected =
                String.Format("{0}.{1} like '%hello%'", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_string_contains_filter_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_passing_two_filters : and_passing_a_string_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.StartsWith, "h");
            QueryLeaf.AddFilter(FilterOperator.EndsWith, "o");
            Expected =
                String.Format("{0}.{1} like 'h%' and {0}.{1} like '%o'", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_two_filters_and_an_and_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_passing_three_filters : and_passing_a_string_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.StartsWith, "h");
            QueryLeaf.AddFilter(FilterOperator.EndsWith, "o");
            QueryLeaf.AddFilter(FilterOperator.Contains, "l");
            Expected =
                String.Format("{0}.{1} like 'h%' and {0}.{1} like '%o' and {0}.{1} like '%l%'",
                              ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_three_filters_and_two_ands_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    #endregion

    #region Numeric Query Leaf

    public class and_passing_a_numeric_query_leaf : and_calling_construct_filter_operation_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf = new QueryLeaf
                            {
                                Selected = 1,
                                IsPrimitive = true,
                                IsReference = false,
                                ParentType = typeof (Employee),
                                PropertyType = typeof (int),
                                PropertyName = "NoOfChildren"
                            };
        }
    }

    public class and_the_operation_is_numeric_equal_to : and_passing_a_numeric_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsEqualTo, 10);
            Expected =
                String.Format("{0}.{1}=10", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_numeric_equal_to_filter_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_operation_is_numeric_not_equal_to : and_passing_a_numeric_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsNotEqualTo, 10);
            Expected =
                String.Format("{0}.{1}<>10", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_numeric_not_equal_to_filter_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_operation_is_numeric_greater_than : and_passing_a_numeric_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsGreaterThan, 10);
            Expected =
                String.Format("{0}.{1}>10", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_numeric_greater_than_filter_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_operation_is_numeric_greater_than_or_equal_to : and_passing_a_numeric_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsGreaterThanOrEqualTo, 10);
            Expected =
                String.Format("{0}.{1}>=10", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_numeric_greater_than_or_equal_to_filter_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_operation_is_numeric_less_than : and_passing_a_numeric_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsLessThan, 10);
            Expected =
                String.Format("{0}.{1}<10", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_numeric_less_than_filter_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_operation_is_numeric_less_than_or_equal_to : and_passing_a_numeric_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsLessThanOrEqualTo, 10);
            Expected =
                String.Format("{0}.{1}<=10", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_numeric_less_than_or_equal_to_filter_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    #endregion

    #region Datetime Query Leaf

    public class and_passing_a_datetime_query_leaf : and_calling_construct_filter_operation_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf = new QueryLeaf
                            {
                                Selected = 1,
                                IsPrimitive = true,
                                IsReference = false,
                                ParentType = typeof (Employee),
                                PropertyType = typeof (DateTime),
                                PropertyName = "DateOfBirth"
                            };
        }
    }

    public class and_the_operation_is_datetime_less_than : and_passing_a_datetime_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsLessThan, new DateTime(1988, 11, 30));
            Expected =
                String.Format("{0}.{1}<'11/30/1988'", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_datetime_less_than_filter_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_operation_is_datetime_less_than_or_equal_to : and_passing_a_datetime_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsLessThanOrEqualTo, new DateTime(1988, 11, 30));
            Expected =
                String.Format("{0}.{1}<='11/30/1988'", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_datetime_less_than_or_equal_to_filter_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_operation_is_datetime_greater_than : and_passing_a_datetime_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsGreaterThan, new DateTime(1988, 11, 30));
            Expected =
                String.Format("{0}.{1}>'11/30/1988'", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_datetime_greater_than_filter_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_operation_is_datetime_greater_than_or_equal_to : and_passing_a_datetime_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsGreaterThanOrEqualTo, new DateTime(1988, 11, 30));
            Expected =
                String.Format("{0}.{1}>='11/30/1988'", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_datetime_greater_than_or_equal_to_filter_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_operation_is_datetime_equal_to : and_passing_a_datetime_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsEqualTo, new DateTime(1988, 11, 30));
            Expected =
                String.Format("{0}.{1}='11/30/1988'", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_datetime_equal_to_filter_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    public class and_the_operation_is_datetime_not_equal_to : and_passing_a_datetime_query_leaf
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsNotEqualTo, new DateTime(1988, 11, 30));
            Expected =
                String.Format("{0}.{1}<>'11/30/1988'", ClassMapping.TableName(typeof (Employee)),
                              ClassMapping.ColumnName(typeof (Employee), QueryLeaf.PropertyName)).ToLower();
            Actual = QueryLeaf.GetFilterQueryString(ClassMapping).ToLower();
        }

        [Fact]
        public void then_a_string_with_datetime_not_equal_to_filter_should_be_returned()
        {
            SetupTest();
            Assert.Equal(Expected, Actual);
        }
    }

    #endregion

    #endregion

    #region Get Filter Expression Tests

    public class and_calling_get_filter_expression : when_dealing_with_query_leaf_extensions_class
    {
        protected AbstractCriterion Actual;
        protected AbstractCriterion Expected;
        protected QueryLeaf QueryLeaf;

        public override void SetupTest()
        {
            base.SetupTest();
        }
    }

    public class and_passing_a_leaf_with_no_filters : and_calling_get_filter_expression
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf = new QueryLeaf {PropertyName = "Property1", IsPrimitive = true, IsReference = false};
            Actual = QueryLeaf.GetFilterExpressionTree("");
        }

        [Fact]
        public void then_a_null_expression_should_be_returned()
        {
            SetupTest();
            Actual.Should().Be.Null();
        }
    }

    #region Single Filter Tests

    public class and_passing_a_primitive_and_non_reference_leaf_with_one_filter : and_calling_get_filter_expression
    {
        public override void SetupTest()
        {
            base.SetupTest();
        }
    }

    public class and_the_leaf_type_is_numeric : and_passing_a_primitive_and_non_reference_leaf_with_one_filter
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf = new QueryLeaf
                            {ParentType = typeof (Employee), PropertyName = "NoOfChildren", PropertyType = typeof (int)};
        }
    }

    public class and_the_operator_is_greater_than : and_the_leaf_type_is_numeric
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsGreaterThan, 1);
            Actual = QueryLeaf.GetFilterExpressionTree("");
            Expected = Restrictions.Gt("NoOfChildren", 1);
        }

        [Fact]
        public void then_to_string_should_be_greater_than_expression()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_the_operator_is_greater_than_or_equal_to : and_the_leaf_type_is_numeric
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsGreaterThanOrEqualTo, 1);
            Actual = QueryLeaf.GetFilterExpressionTree("");
            Expected = Restrictions.Ge("NoOfChildren", 1);
        }

        [Fact]
        public void then_to_string_should_be_greater_than_or_equal_to_expression()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_the_leaf_type_is_datetime : and_passing_a_primitive_and_non_reference_leaf_with_one_filter
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf = new QueryLeaf
                            {
                                ParentType = typeof (Employee),
                                PropertyName = "DateOfBirth",
                                PropertyType = typeof (DateTime)
                            };
        }
    }

    public class and_the_operator_is_less_than_datetime : and_the_leaf_type_is_datetime
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsLessThan, new DateTime(1988, 11, 30));
            Actual = QueryLeaf.GetFilterExpressionTree("");
            Expected = Restrictions.Lt("DateOfBirth", new DateTime(1988, 11, 30));
        }

        [Fact]
        public void then_to_string_should_be_less_than_datetime_expression()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_the_operator_is_less_than_and_the_value_is_string_datetime : and_the_leaf_type_is_datetime
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsLessThan, "30/11/1988");
            Actual = QueryLeaf.GetFilterExpressionTree("");
            Expected = Restrictions.Lt("DateOfBirth", new DateTime(1988, 11, 30));
        }

        [Fact]
        public void then_the_to_string_should_be_equal_to_datetime_less_than()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_the_operator_is_less_than_or_equal_to : and_the_leaf_type_is_datetime
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsLessThanOrEqualTo, new DateTime(1988, 11, 30));
            Actual = QueryLeaf.GetFilterExpressionTree("");
            Expected = Restrictions.Le("DateOfBirth", new DateTime(1988, 11, 30));
        }

        [Fact]
        public void then_to_string_should_be_less_than_or_equal_datetime_expression()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_the_operator_is_equal_to : and_the_leaf_type_is_datetime
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsEqualTo, new DateTime(1988, 11, 30));
            Actual = QueryLeaf.GetFilterExpressionTree("");
            Expected = Restrictions.Eq("DateOfBirth", new DateTime(1988, 11, 30));
        }

        [Fact]
        public void then_to_string_should_be_equal_to_expression()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_the_operator_is_not_equal_to : and_the_leaf_type_is_datetime
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsNotEqualTo, new DateTime(1988, 11, 30));
            Actual = QueryLeaf.GetFilterExpressionTree("");
            Expected = Restrictions.Not(Restrictions.Eq("DateOfBirth", new DateTime(1988, 11, 30)));
        }

        [Fact]
        public void then_to_string_should_be_not_equal_to_expression()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_the_leaf_type_is_string : and_passing_a_primitive_and_non_reference_leaf_with_one_filter
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf = new QueryLeaf
                            {ParentType = typeof (Employee), PropertyName = "FirstName", PropertyType = typeof (string)};
        }
    }

    public class and_the_operator_is_string_equal_to : and_the_leaf_type_is_string
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsEqualTo, "hello");
            Actual = QueryLeaf.GetFilterExpressionTree("");
            Expected = Restrictions.InsensitiveLike("FirstName", "hello", MatchMode.Exact);
        }

        [Fact]
        public void then_to_string_should_like_exact_string_expression()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_the_operator_is_string_not_equal_to : and_the_leaf_type_is_string
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.IsNotEqualTo, "hello");
            Actual = QueryLeaf.GetFilterExpressionTree("");
            Expected = Restrictions.Not(Restrictions.InsensitiveLike("FirstName", "hello", MatchMode.Exact));
        }

        [Fact]
        public void then_to_string_should_be_not_like_exact_string_expression()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_the_operator_is_contains : and_the_leaf_type_is_string
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.Contains, "hello");
            Actual = QueryLeaf.GetFilterExpressionTree("");
            Expected = Restrictions.InsensitiveLike("FirstName", "hello", MatchMode.Anywhere);
        }

        [Fact]
        public void then_to_string_should_be_like_anywhere_string_expression()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_the_operator_is_startswith : and_the_leaf_type_is_string
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.StartsWith, "hello");
            Actual = QueryLeaf.GetFilterExpressionTree("");
            Expected = Restrictions.InsensitiveLike("FirstName", "hello", MatchMode.Start);
        }

        [Fact]
        public void then_to_string_should_be_like_startswith_string_expression()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_the_operator_is_endswith : and_the_leaf_type_is_string
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.EndsWith, "hello");
            Actual = QueryLeaf.GetFilterExpressionTree("");
            Expected = Restrictions.InsensitiveLike("FirstName", "hello", MatchMode.End);
        }

        [Fact]
        public void then_to_string_should_be_endswith_expression()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_the_operator_is_endswith_with_an_alias : and_the_leaf_type_is_string
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf.AddFilter(FilterOperator.EndsWith, "hello");
            Actual = QueryLeaf.GetFilterExpressionTree("employee");
            Expected = Restrictions.InsensitiveLike("employee.FirstName", "hello", MatchMode.End);
        }

        [Fact]
        public void then_to_string_should_be_endswith_string_expression_with_alias()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    #endregion

    public class and_passing_a_primitive_and_reference_leaf_with_one_filter : and_calling_get_filter_expression
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf = new QueryLeaf
                            {
                                PropertyName = "Gender.Name",
                                IsPrimitive = true,
                                IsReference = true,
                                ParentType = typeof (Gender),
                                PropertyType = typeof (string)
                            };
            QueryLeaf.AddFilter(FilterOperator.StartsWith, "m");
            Actual = QueryLeaf.GetFilterExpressionTree("gender");
            Expected = Restrictions.InsensitiveLike("gender.Name", "m", MatchMode.Start);
        }

        [Fact]
        public void then_to_string_should_be_like_startswith_and_a_reference_expression()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_passing_a_primitive_and_non_reference_leaf_with_two_filters : and_calling_get_filter_expression
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf = new QueryLeaf
                            {
                                PropertyName = "FirstName",
                                IsPrimitive = true,
                                IsReference = false,
                                ParentType = typeof (Gender),
                                PropertyType = typeof (string)
                            };
            QueryLeaf.AddFilter(FilterOperator.StartsWith, "m");
            QueryLeaf.AddFilter(FilterOperator.EndsWith, "r");
            Actual = QueryLeaf.GetFilterExpressionTree("");
            Expected = Restrictions.And(Restrictions.InsensitiveLike("FirstName", "m", MatchMode.Start),
                                        Restrictions.InsensitiveLike("FirstName", "r", MatchMode.End));
        }

        [Fact]
        public void then_to_string_should_be_startswith_and_endswith_expression()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    public class and_passing_a_primitive_and_non_reference_leaf_with_three_filters : and_calling_get_filter_expression
    {
        public override void SetupTest()
        {
            base.SetupTest();
            QueryLeaf = new QueryLeaf
                            {
                                PropertyName = "FirstName",
                                IsPrimitive = true,
                                IsReference = false,
                                ParentType = typeof (Gender),
                                PropertyType = typeof (string)
                            };
            QueryLeaf.AddFilter(FilterOperator.StartsWith, "m");
            QueryLeaf.AddFilter(FilterOperator.EndsWith, "r");
            QueryLeaf.AddFilter(FilterOperator.Contains, "a");
            Actual = QueryLeaf.GetFilterExpressionTree("");
            Expected =
                Restrictions.And(Restrictions.And(Restrictions.InsensitiveLike("FirstName", "m", MatchMode.Start),
                                                  Restrictions.InsensitiveLike("FirstName", "r", MatchMode.End)),
                                 Restrictions.InsensitiveLike("FirstName", "a", MatchMode.Anywhere));
        }

        [Fact]
        public void then_to_string_should_be_startswith_and_endswith_expression()
        {
            SetupTest();
            Actual.ToString().Should().Be.EqualTo(Expected.ToString());
        }
    }

    #endregion
}