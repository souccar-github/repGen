using System.Collections;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.JobDesc.Indexes;
using HRIS.Domain.JobDesc.ValueObjects;
using HRIS.Domain.OrgChart.Indexes;
using HRIS.Domain.Personnel.Entities;
using SharpTestsEx;
using Souccar.ReportGenerator.Services;
using Xunit;

namespace Souccar.ReportGenerator.Test.Services
{
    public class when_dealing_with_alias_to_projection_entity_transformer
    {
        protected virtual void SetupTest()
        {
        }
    }

    public class and_using_transform_tuple_method : when_dealing_with_alias_to_projection_entity_transformer
    {
        protected object Actual;
        protected object Expected;
    }

    public class and_attempting_to_transform_a_tuple_with_one_value :
        and_using_transform_tuple_method
    {
        protected override void SetupTest()
        {
            var tuple = new object[] {"John"};
            var aliases = new[] {"Employee.FirstName"};
            Expected = new Employee {FirstName = "John"};
            Actual = new AliasProjectionToEntityTransformer(typeof (Employee)).TransformTuple(tuple, aliases);
        }

        [Fact]
        public void then_an_entity_with_its_selected_property_filled_should_be_returned()
        {
            SetupTest();
            ((Employee) Actual).FirstName.Should().Be.EqualTo(((Employee) Expected).FirstName);
        }
    }

    public class and_attempting_to_transform_a_tuple_with_two_value_from_same_entity :
        and_using_transform_tuple_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            var tuple = new object[] {"John", "Smith"};
            var aliases = new[] {"Employee.FirstName", "Employee.LastName"};
            Expected = new Employee {FirstName = "John", LastName = "Smith"};
            Actual = new AliasProjectionToEntityTransformer(typeof (Employee)).TransformTuple(tuple, aliases);
        }

        [Fact]
        public void then_an_entity_with_its_two_selected_properties_filled_should_be_returned()
        {
            SetupTest();
            ((Employee) Actual).FirstName.Should().Be.EqualTo(((Employee) Expected).FirstName);
            ((Employee) Actual).LastName.Should().Be.EqualTo(((Employee) Expected).LastName);
        }
    }

    public class and_attempting_to_transform_a_tuple_with_two_value_from_master_and_refrence :
        and_using_transform_tuple_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            var tuple = new object[] {"Summary1", "JobTitle1"};
            var aliases = new[] {"JobDescription.Summary", "JobDescription.JobTitle.Name"};
            Expected = new JobDescription {Summary = "Summary1", JobTitle = new JobTitle {Name = "JobTitle1"}};
            Actual = new AliasProjectionToEntityTransformer(typeof (JobDescription)).TransformTuple(tuple, aliases);
        }

        [Fact]
        public void then_an_entity_with_its_selected_property_and_refrence_selected_property_filled_should_be_returned()
        {
            SetupTest();
            ((JobDescription) Actual).Summary.Should().Be.EqualTo(((JobDescription) Expected).Summary);
            ((JobDescription) Actual).JobTitle.Name.Should().Be.EqualTo(((JobDescription) Expected).JobTitle.Name);
        }
    }

    public class and_attempting_to_transform_a_tuple_with_one_value_from_master_and_two_value_from_reference :
        and_using_transform_tuple_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            var tuple = new object[] {"Summary1", 1, "JobTitle1"};
            var aliases = new[] {"JobDescription.Summary", "JobDescription.JobTitle.Id", "JobDescription.JobTitle.Name"};
            Expected = new JobDescription {Summary = "Summary1", JobTitle = new JobTitle {Id = 1, Name = "JobTitle1"}};
            Actual = new AliasProjectionToEntityTransformer(typeof (JobDescription)).TransformTuple(tuple, aliases);
        }

        [Fact]
        public void then_an_entity_with_its_selected_property_and_refrence_selected_properties_filled_should_be_returned
            ()
        {
            SetupTest();
            ((JobDescription) Actual).Summary.Should().Be.EqualTo(((JobDescription) Expected).Summary);
            ((JobDescription) Actual).JobTitle.Name.Should().Be.EqualTo(((JobDescription) Expected).JobTitle.Name);
            ((JobDescription) Actual).JobTitle.Id.Should().Be.EqualTo(((JobDescription) Expected).JobTitle.Id);
        }
    }

    public class and_attempting_to_transform_a_tuple_with_one_value_from_master_and_two_value_from_detail :
        and_using_transform_tuple_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            var tuple = new object[] {"Summary1", "Role1", 0.5f};
            var aliases = new[] {"JobDescription.Summary", "JobDescription.Roles.Name", "JobDescription.Roles.Weight"};
            Expected = new JobDescription {Summary = "Summary1"};
            ((JobDescription) Expected).Roles.Add(new Role {Name = "Role1", Weight = 0.5f});
            Actual = new AliasProjectionToEntityTransformer(typeof (JobDescription)).TransformTuple(tuple, aliases);
        }

        [Fact]
        public void then_an_entity_with_its_selected_property_filled_and_one_record_in_its_detail_should_be_returned()
        {
            SetupTest();
            ((JobDescription) Actual).Summary.Should().Be.EqualTo(((JobDescription) Expected).Summary);
            ((JobDescription) Actual).Roles.Count.Should().Be.EqualTo(((JobDescription) Expected).Roles.Count);
            ((JobDescription) Actual).Roles[0].Name.Should().Be.EqualTo(((JobDescription) Expected).Roles[0].Name);
            ((JobDescription) Actual).Roles[0].Weight.Should().Be.EqualTo(((JobDescription) Expected).Roles[0].Weight);
        }
    }

    public class
        and_attempting_to_transform_a_tuple_with_one_value_from_master_and_two_value_from_detail_with_null_detail_list :
            and_using_transform_tuple_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            var tuple = new object[] {"Summary1", "Role1", 0.5f, "Responsibility1", "ResponsibilityKpiValue1"};
            var aliases = new[]
                              {
                                  "JobDescription.Summary", "JobDescription.Roles.Name", "JobDescription.Roles.Weight",
                                  "JobDescription.Roles.Responsibilities.Description",
                                  "JobDescription.Roles.Responsibilities.ResponsibilityKpis.Description"
                              };
            Expected = new JobDescription {Summary = "Summary1"};
            ((JobDescription) Expected).Roles.Add(new Role {Name = "Role1", Weight = 0.5f});
            Actual = new AliasProjectionToEntityTransformer(typeof (JobDescription)).TransformTuple(tuple, aliases);
        }

        [Fact]
        public void
            then_an_entity_with_its_selected_property_filled_and_one_record_in_its_detail_and_the_list_is_initialized_should_be_returned
            ()
        {
            SetupTest();
            ((JobDescription) Actual).Roles[0].Responsibilities[0].ResponsibilityKpis.Should().Not.Be.Null();
        }
    }


    public class
        and_attempting_to_transform_a_tuple_with_one_value_from_master_and_one_value_from_detail_and_one_value_from_detail_of_detail :
            and_using_transform_tuple_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            var tuple = new object[] {"Summary1", "Role1", 0.5f, "Responsibility1"};
            var aliases = new[]
                              {
                                  "JobDescription.Summary", "JobDescription.Roles.Name", "JobDescription.Roles.Weight",
                                  "JobDescription.Roles.Responsibilities.Description"
                              };
            Expected = new JobDescription {Summary = "Summary1"};
            ((JobDescription) Expected).Roles.Add(new Role {Name = "Role1", Weight = 0.5f});
            ((JobDescription) Expected).Roles[0].Responsibilities.Add(new Responsibility
                                                                          {Description = "Responsibility1"});
            Actual = new AliasProjectionToEntityTransformer(typeof (JobDescription)).TransformTuple(tuple, aliases);
        }

        [Fact]
        public void
            then_an_entity_with_its_selected_property_filled_and_one_record_in_its_detail_and_one_record_in_detail_of_detail_should_be_returned
            ()
        {
            SetupTest();
            ((JobDescription) Actual).Summary.Should().Be.EqualTo(((JobDescription) Expected).Summary);
            ((JobDescription) Actual).Roles.Count.Should().Be.EqualTo(((JobDescription) Expected).Roles.Count);
            ((JobDescription) Actual).Roles[0].Name.Should().Be.EqualTo(((JobDescription) Expected).Roles[0].Name);
            ((JobDescription) Actual).Roles[0].Weight.Should().Be.EqualTo(((JobDescription) Expected).Roles[0].Weight);
            ((JobDescription) Actual).Roles[0].Responsibilities.Count.Should().Be.EqualTo(
                ((JobDescription) Expected).Roles[0].Responsibilities.Count);
            ((JobDescription) Actual).Roles[0].Responsibilities[0].Description.Should().Be.EqualTo(
                ((JobDescription) Expected).Roles[0].Responsibilities[0].Description);
        }
    }

    public class and_attempting_to_transform_a_tuple_with_one_value_from_master_and_one_null_value_from_reference :
        and_using_transform_tuple_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            var tuple = new object[] {"Summary1", null};
            var aliases = new[] {"JobDescription.Summary", "JobDescription.JobTitle.Name"};
            Expected = new JobDescription {Summary = "Summary1"};
            Actual = new AliasProjectionToEntityTransformer(typeof (JobDescription)).TransformTuple(tuple, aliases);
        }

        [Fact]
        public void then_an_entity_with_its_selected_property_filled_and_its_reference_property_is_null()
        {
            SetupTest();
            ((JobDescription) Actual).Summary.Should().Be.EqualTo(((JobDescription) Expected).Summary);
            ((JobDescription) Actual).JobTitle.Name.Should().Be.Null();
        }
    }

    public class
        and_attempting_to_transform_a_tuple_with_one_value_from_master_and_one_value_from_detail_and_one_value_from_reference_in_detail_of_detail :
            and_using_transform_tuple_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            var tuple = new object[] {"Summary1", "Role1", 0.5f, "Priority1"};
            var aliases = new[]
                              {
                                  "JobDescription.Summary", "JobDescription.Roles.Name", "JobDescription.Roles.Weight",
                                  "JobDescription.Roles.Responsibilities.Priority.Name"
                              };
            Expected = new JobDescription {Summary = "Summary1"};
            ((JobDescription) Expected).Roles.Add(new Role {Name = "Role1", Weight = 0.5f});
            ((JobDescription) Expected).Roles[0].Responsibilities.Add(new Responsibility
                                                                          {Priority = new Priority {Name = "Priority1"}});
            Actual = new AliasProjectionToEntityTransformer(typeof (JobDescription)).TransformTuple(tuple, aliases);
        }

        [Fact]
        public void
            then_an_entity_with_its_selected_property_filled_and_one_record_in_its_detail_and_one_reference_in_detail_of_detail_should_be_returned
            ()
        {
            SetupTest();
            ((JobDescription) Actual).Summary.Should().Be.EqualTo(((JobDescription) Expected).Summary);
            ((JobDescription) Actual).Roles.Count.Should().Be.EqualTo(((JobDescription) Expected).Roles.Count);
            ((JobDescription) Actual).Roles[0].Name.Should().Be.EqualTo(((JobDescription) Expected).Roles[0].Name);
            ((JobDescription) Actual).Roles[0].Weight.Should().Be.EqualTo(((JobDescription) Expected).Roles[0].Weight);
            ((JobDescription) Actual).Roles[0].Responsibilities.Count.Should().Be.EqualTo(
                ((JobDescription) Expected).Roles[0].Responsibilities.Count);
            ((JobDescription) Actual).Roles[0].Responsibilities[0].Priority.Name.Should().Be.EqualTo(
                ((JobDescription) Expected).Roles[0].Responsibilities[0].Priority.Name);
        }
    }

    public class and_using_transform_list_method : when_dealing_with_alias_to_projection_entity_transformer
    {
        protected IList Actual;
        protected IList Expected;
        protected ArrayList Tuples;
    }

    public class and_attempting_to_transform_an_empty_list : and_using_transform_list_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            Tuples = new ArrayList();
            Actual = new AliasProjectionToEntityTransformer(typeof (JobDescription)).TransformList(Tuples);
        }

        [Fact]
        public void then_an_empty_array_list_should_be_returned()
        {
            SetupTest();
            Actual.Count.Should().Be.EqualTo(0);
        }
    }

    public class and_attempting_to_transform_a_list_with_one_entity : and_using_transform_list_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            var tuple = new object[] {1, "Summary1"};
            var aliases = new[] {"JobDescription.Id", "JobDescription.Summary"};
            Expected = new ArrayList
                           {
                               new JobDescription {Id = 1, Summary = "Summary1"}
                           };
            var aliasProjectionToEntityTransformer = new AliasProjectionToEntityTransformer(typeof (JobDescription));
            Tuples = new ArrayList
                         {
                             aliasProjectionToEntityTransformer.TransformTuple(tuple, aliases)
                         };
            Actual = aliasProjectionToEntityTransformer.TransformList(Tuples);
        }

        [Fact]
        public void then_a_list_with_one_object_of_passed_type_filled_with_data_should_be_returned()
        {
            SetupTest();
            Actual.Count.Should().Be.EqualTo(Expected.Count);
            Actual[0].Should().Be.InstanceOf<JobDescription>();
            ((JobDescription) Actual[0]).Id.Should().Be.EqualTo(((JobDescription) Expected[0]).Id);
            ((JobDescription) Actual[0]).Summary.Should().Be.EqualTo(((JobDescription) Expected[0]).Summary);
        }
    }

    public class and_attempting_to_transform_a_list_with_one_entity_with_one_reference_data :
        and_using_transform_list_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            var aliases = new[]
                              {
                                  "JobDescription.Id", "JobDescription.Summary", "JobDescription.JobTitle.Id",
                                  "JobDescription.JobTitle.Name"
                              };
            Expected = new ArrayList
                           {
                               new JobDescription
                                   {Id = 1, Summary = "Summary1", JobTitle = new JobTitle {Id = 1, Name = "JobTitle1"}}
                           };
            var aliasProjectionToEntityTransformer = new AliasProjectionToEntityTransformer(typeof (JobDescription));
            Tuples = new ArrayList
                         {
                             aliasProjectionToEntityTransformer.TransformTuple(
                                 new object[] {1, "Summary1", 1, "JobTitle1"}, aliases)
                         };
            Actual = aliasProjectionToEntityTransformer.TransformList(Tuples);
        }

        [Fact]
        public void
            then_an_array_list_with_one_object_with_its_simple_properties_and_reference_properties_filled_should_be_returned
            ()
        {
            SetupTest();
            Actual.Count.Should().Be.EqualTo(Expected.Count);
            Actual[0].Should().Be.InstanceOf<JobDescription>();
            ((JobDescription) Actual[0]).Id.Should().Be.EqualTo(((JobDescription) Expected[0]).Id);
            ((JobDescription) Actual[0]).Summary.Should().Be.EqualTo(((JobDescription) Expected[0]).Summary);
            ((JobDescription) Actual[0]).JobTitle.Id.Should().Be.EqualTo(((JobDescription) Expected[0]).JobTitle.Id);
            ((JobDescription) Actual[0]).JobTitle.Name.Should().Be.EqualTo(((JobDescription) Expected[0]).JobTitle.Name);
        }
    }

    public class and_attempting_to_transform_a_list_with_one_duplicated_entity_and_one_detail_record_in_each :
        and_using_transform_list_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            var aliases = new[]
                              {
                                  "JobDescription.Id", "JobDescription.Summary", "JobDescription.Roles.Id",
                                  "JobDescription.Roles.Name"
                              };
            Expected = new ArrayList
                           {
                               new JobDescription
                                   {Id = 1, Summary = "Summary1"}
                           };
            ((JobDescription) Expected[0]).Roles.Add(new Role {Id = 1, Name = "Role1"});
            ((JobDescription) Expected[0]).Roles.Add(new Role {Id = 2, Name = "Role2"});
            var aliasProjectionToEntityTransformer = new AliasProjectionToEntityTransformer(typeof (JobDescription));
            Tuples = new ArrayList
                         {
                             aliasProjectionToEntityTransformer.TransformTuple(
                                 new object[] {1, "Summary1", 1, "Role1"}, aliases),
                             aliasProjectionToEntityTransformer.TransformTuple(
                                 new object[] {1, "Summary1", 2, "Role2"}, aliases)
                         };
            Actual = aliasProjectionToEntityTransformer.TransformList(Tuples);
        }

        [Fact]
        public void
            then_an_array_list_with_one_object_with_its_simple_properties_filled_and_the_collection_property_has_one_item_should_be_returned
            ()
        {
            SetupTest();
            Actual.Count.Should().Be.EqualTo(Expected.Count);
            ((JobDescription) Actual[0]).Id.Should().Be.EqualTo(((JobDescription) Expected[0]).Id);
            ((JobDescription) Actual[0]).Summary.Should().Be.EqualTo(((JobDescription) Expected[0]).Summary);
            ((JobDescription) Actual[0]).Roles.Count.Should().Be.EqualTo(2);
            ((JobDescription) Actual[0]).Roles[0].Id.Should().Be.EqualTo(((JobDescription) Expected[0]).Roles[0].Id);
            ((JobDescription) Actual[0]).Roles[0].Name.Should().Be.EqualTo(((JobDescription) Expected[0]).Roles[0].Name);
            ((JobDescription) Actual[0]).Roles[1].Id.Should().Be.EqualTo(((JobDescription) Expected[0]).Roles[1].Id);
            ((JobDescription) Actual[0]).Roles[1].Name.Should().Be.EqualTo(((JobDescription) Expected[0]).Roles[1].Name);
        }
    }

    public class
        and_attempting_to_transform_a_list_with_one_duplicated_entity_and_one_detail_record_in_each_and_a_reference_in_detail :
            and_using_transform_list_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            var aliases = new[]
                              {
                                  "JobDescription.Id", "JobDescription.Summary", "JobDescription.Roles.Id",
                                  "JobDescription.Roles.Name", "JobDescription.Roles.JobTitle.Name"
                              };
            Expected = new ArrayList
                           {
                               new JobDescription
                                   {Id = 1, Summary = "Summary1"}
                           };
            ((JobDescription) Expected[0]).Roles.Add(new Role
                                                         {
                                                             Id = 1,
                                                             Name = "Role1",
                                                             JobTitle = new JobTitle {Name = "JobTitle1"}
                                                         });
            ((JobDescription) Expected[0]).Roles.Add(new Role {Id = 2, Name = "Role2", JobTitle = new JobTitle()});
            var aliasProjectionToEntityTransformer = new AliasProjectionToEntityTransformer(typeof (JobDescription));
            Tuples = new ArrayList
                         {
                             aliasProjectionToEntityTransformer.TransformTuple(
                                 new object[] {1, "Summary1", 1, "Role1", "JobTitle1"}, aliases),
                             aliasProjectionToEntityTransformer.TransformTuple(
                                 new object[] {1, "Summary1", 2, "Role2", null}, aliases)
                         };
            Actual = aliasProjectionToEntityTransformer.TransformList(Tuples);
        }

        [Fact]
        public void
            then_an_array_list_with_one_object_with_its_simple_properties_filled_and_the_collection_property_has_one_item_with_reference_should_be_returned
            ()
        {
            SetupTest();
            Actual.Count.Should().Be.EqualTo(Expected.Count);
            ((JobDescription) Actual[0]).Id.Should().Be.EqualTo(((JobDescription) Expected[0]).Id);
            ((JobDescription) Actual[0]).Summary.Should().Be.EqualTo(((JobDescription) Expected[0]).Summary);
            ((JobDescription) Actual[0]).Roles.Count.Should().Be.EqualTo(2);
            ((JobDescription) Actual[0]).Roles[0].Id.Should().Be.EqualTo(((JobDescription) Expected[0]).Roles[0].Id);
            ((JobDescription) Actual[0]).Roles[0].Name.Should().Be.EqualTo(((JobDescription) Expected[0]).Roles[0].Name);
            ((JobDescription) Actual[0]).Roles[0].JobTitle.Name.Should().Be.EqualTo(
                ((JobDescription) Expected[0]).Roles[0].JobTitle.Name);
            ((JobDescription) Actual[0]).Roles[1].Id.Should().Be.EqualTo(((JobDescription) Expected[0]).Roles[1].Id);
            ((JobDescription) Actual[0]).Roles[1].Name.Should().Be.EqualTo(((JobDescription) Expected[0]).Roles[1].Name);
            ((JobDescription) Actual[0]).Roles[1].JobTitle.Name.Should().Be.Null();
        }
    }

    public class
        and_attempting_to_transform_a_list_with_one_duplicated_entity_and_one_detail_record_in_each_and_detail_of_detail :
            and_using_transform_list_method
    {
        protected override void SetupTest()
        {
            base.SetupTest();
            var aliases = new[]
                              {
                                  "JobDescription.Id", "JobDescription.Summary", "JobDescription.Roles.Id",
                                  "JobDescription.Roles.Name", "JobDescription.Roles.Responsibilities.Id",
                                  "JobDescription.Roles.Responsibilities.Description"
                              };
            Expected = new ArrayList
                           {
                               new JobDescription
                                   {Id = 1, Summary = "Summary1"}
                           };
            ((JobDescription) Expected[0]).Roles.Add(new Role {Id = 1, Name = "Role1"});
            ((JobDescription) Expected[0]).Roles[0].Responsibilities.Add(new Responsibility
                                                                             {Id = 1, Description = "Responsibility1"});
            ((JobDescription) Expected[0]).Roles[0].Responsibilities.Add(new Responsibility
                                                                             {Id = 2, Description = "Responsibility2"});
            ((JobDescription) Expected[0]).Roles.Add(new Role {Id = 2, Name = "Role2"});
            var aliasProjectionToEntityTransformer = new AliasProjectionToEntityTransformer(typeof (JobDescription));
            Tuples = new ArrayList
                         {
                             aliasProjectionToEntityTransformer.TransformTuple(
                                 new object[] {1, "Summary1", 1, "Role1", 1, "Responsibility1"}, aliases),
                             aliasProjectionToEntityTransformer.TransformTuple(
                                 new object[] {1, "Summary1", 1, "Role1", 2, "Responsibility2"}, aliases),
                             aliasProjectionToEntityTransformer.TransformTuple(
                                 new object[] {1, "Summary1", 2, "Role2", null}, aliases)
                         };
            Actual = aliasProjectionToEntityTransformer.TransformList(Tuples);
        }

        [Fact]
        public void
            then_an_array_list_with_one_object_with_its_simple_properties_filled_and_the_collection_property_has_one_item_with_one_item_in_its_detail_should_be_returned
            ()
        {
            SetupTest();
            Actual.Count.Should().Be.EqualTo(Expected.Count);
            ((JobDescription) Actual[0]).Id.Should().Be.EqualTo(((JobDescription) Expected[0]).Id);
            ((JobDescription) Actual[0]).Summary.Should().Be.EqualTo(((JobDescription) Expected[0]).Summary);
            ((JobDescription) Actual[0]).Roles.Count.Should().Be.EqualTo(2);
            ((JobDescription) Actual[0]).Roles[0].Id.Should().Be.EqualTo(((JobDescription) Expected[0]).Roles[0].Id);
            ((JobDescription) Actual[0]).Roles[0].Name.Should().Be.EqualTo(((JobDescription) Expected[0]).Roles[0].Name);
            ((JobDescription) Actual[0]).Roles[1].Id.Should().Be.EqualTo(((JobDescription) Expected[0]).Roles[1].Id);
            ((JobDescription) Actual[0]).Roles[1].Name.Should().Be.EqualTo(((JobDescription) Expected[0]).Roles[1].Name);
            ((JobDescription) Actual[0]).Roles[0].Responsibilities.Count.Should().Be.EqualTo(2);
            ((JobDescription) Actual[0]).Roles[0].Responsibilities[0].Description.Should().Be.EqualTo(
                ((JobDescription) Expected[0]).Roles[0].Responsibilities[0].Description);
            ((JobDescription) Actual[0]).Roles[0].Responsibilities[1].Description.Should().Be.EqualTo(
                ((JobDescription) Expected[0]).Roles[0].Responsibilities[1].Description);
        }
    }
}