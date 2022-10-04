#region Using Statements

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.JobDesc.Indexes;
using HRIS.Domain.JobDesc.ValueObjects;
using HRIS.Domain.OrgChart.Indexes;
using Souccar.ReportGenerator.Services;
using Xunit;

#endregion

namespace Souccar.ReportGenerator.Test.Services
{
    public class when_dealing_with_the_multi_level_distinct_entity_transformer_class
    {
        protected Dictionary<Type, List<string>> FetchedProperties;
        protected MultiLevelDistinctEntityTransformer Transformer;

        public virtual void SetupTest()
        {
            FetchedProperties = new Dictionary<Type, List<string>>();
            Transformer = new MultiLevelDistinctEntityTransformer(FetchedProperties);
        }
    }

    public class and_calling_transform_list_method : when_dealing_with_the_multi_level_distinct_entity_transformer_class
    {
        protected IList Actual;
        protected IList Data;
        protected IList Expected;
    }

    public class and_passing_an_empty_list : and_calling_transform_list_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            IList data = new ArrayList();
            Actual = Transformer.TransformList(data);
        }

        [Fact]
        public void then_an_empty_list_should_be_returned()
        {
            SetupTest();
            Assert.Equal(0, Actual.Count);
        }
    }

    public class and_passing_a_list_that_have_duplication_on_first_level : and_calling_transform_list_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            Data = new ArrayList();
            var jobDescription = new JobDescription
                                     {Id = 1, Summary = "hello", JobTitle = new JobTitle {Id = 1, Name = "JT1"}};
            var jobDescription2 = new JobDescription
                                      {Id = 2, Summary = "Job2", JobTitle = new JobTitle {Id = 1, Name = "JT1"}};
            Data.Add(jobDescription);
            Data.Add(jobDescription);
            Data.Add(jobDescription2);
            Expected = new ArrayList {jobDescription, jobDescription2};
            Actual = Transformer.TransformList(Data);
        }

        [Fact]
        public void then_duplicated_values_should_be_removed_from_the_list()
        {
            SetupTest();
            Assert.Equal(Expected.Count, Actual.Count);
            Assert.Equal(Expected[0], Actual[0]);
            Assert.Equal(Expected[1], Actual[1]);
        }
    }

    public class and_passing_a_list_the_have_duplication_on_second_level : and_calling_transform_list_method
    {
        public override void SetupTest()
        {
            base.SetupTest();
            Data = new ArrayList();
            var jobDescription = new JobDescription
                                     {Id = 1, Summary = "hello", JobTitle = new JobTitle {Id = 1, Name = "JT1"}};
            var authority1 = new Authority
                                 {
                                     Id = 1,
                                     RelatedActions = "RA1",
                                     Title = "Auth1",
                                     JobDescription = jobDescription,
                                     JobTitle = new JobTitle {Id = 1, Name = "JT1"}
                                 };
            var authority2 = new Authority
                                 {
                                     Id = 2,
                                     RelatedActions = "RA2",
                                     Title = "Auth2",
                                     JobDescription = jobDescription,
                                     JobTitle = new JobTitle {Id = 1, Name = "JT1"}
                                 };
            jobDescription.AddAuthority(authority1);
            jobDescription.AddAuthority(authority1);
            jobDescription.AddAuthority(authority2);
            var jobDescription2 = new JobDescription
                                      {Id = 2, Summary = "Job2", JobTitle = new JobTitle {Id = 1, Name = "JT1"}};
            Data.Add(jobDescription);
            Data.Add(jobDescription);
            Data.Add(jobDescription2);
        }
    }

    public class and_the_second_level_is_fetched : and_passing_a_list_the_have_duplication_on_second_level
    {
        public override void SetupTest()
        {
            base.SetupTest();
            FetchedProperties.Add(typeof (JobDescription), new List<string> {"Authorities"});
            Actual = Transformer.TransformList(Data);
        }

        [Fact]
        public void then_the_duplication_from_the_first_level_should_be_removed_from_list()
        {
            SetupTest();
            Assert.Equal(2, Actual.Count);
        }

        [Fact]
        public void then_the_duplication_from_the_second_level_should_be_removed_from_list()
        {
            SetupTest();
            Assert.Equal(2, ((JobDescription) Actual[0]).Authorities.Count);
        }
    }

    public class and_passing_a_list_that_have_duplication_on_fetched_third_level : and_calling_transform_list_method
    {
        private IList<Responsibility> _expectedResponsibilites;
        private IList<Role> _expectedRoles;

        public override void SetupTest()
        {
            base.SetupTest();
            FetchedProperties.Add(typeof (JobDescription), new List<string> {"Roles"});
            FetchedProperties.Add(typeof (Role), new List<string> {"Responsibilities"});
            Data = new ArrayList();
            var jobDescription = new JobDescription
                                     {Id = 1, Summary = "hello", JobTitle = new JobTitle {Id = 1, Name = "JT1"}};
            var role1 = new Role
                            {
                                Id = 1,
                                Name = "Role1",
                                JobDescription = jobDescription,
                                Priority = new Priority {Id = 1, Name = "High"},
                                JobTitle = new JobTitle {Id = 1, Name = "JT1"},
                                Summary = "RoleSummary1",
                                Weight = 50f
                            };
            var responsibility1 = new Responsibility
                                      {
                                          Id = 1,
                                          Description = "resp1",
                                          JobTitle = new JobTitle {Id = 1, Name = "JT1"},
                                          Priority = new Priority {Id = 1, Name = "High"},
                                          Weight = 20f,
                                          Role = role1
                                      };
            var responsibility2 = new Responsibility
                                      {
                                          Id = 2,
                                          Description = "resp2",
                                          JobTitle = new JobTitle {Id = 1, Name = "JT1"},
                                          Priority = new Priority {Id = 1, Name = "High"},
                                          Weight = 20f,
                                          Role = role1
                                      };
            role1.AddResponsibility(responsibility1);
            role1.AddResponsibility(responsibility1);
            role1.AddResponsibility(responsibility2);
            var role2 = new Role
                            {
                                Id = 2,
                                Name = "Role2",
                                JobDescription = jobDescription,
                                Priority = new Priority {Id = 2, Name = "Medium"},
                                JobTitle = new JobTitle {Id = 1, Name = "JT1"},
                                Summary = "RoleSummary2",
                                Weight = 50f
                            };
            jobDescription.AddRole(role1);
            jobDescription.AddRole(role1);
            jobDescription.AddRole(role2);
            var jobDescription2 = new JobDescription
                                      {Id = 2, Summary = "Job2", JobTitle = new JobTitle {Id = 1, Name = "JT1"}};
            Data.Add(jobDescription);
            Data.Add(jobDescription);
            Data.Add(jobDescription2);
            _expectedResponsibilites = new List<Responsibility> {responsibility1, responsibility2};
            _expectedRoles = new List<Role> {role1, role2};
            Expected = new ArrayList {jobDescription, jobDescription2};
            Actual = Transformer.TransformList(Data);
        }

        [Fact]
        public void then_the_duplication_from_the_first_level_should_be_removed_from_list()
        {
            SetupTest();
            Assert.Equal(Expected.Count, Actual.Count);
            Assert.Equal(Expected[0], Actual[0]);
            Assert.Equal(Expected[1], Actual[1]);
        }

        [Fact]
        public void then_the_duplication_from_the_second_level_should_be_removed_from_list()
        {
            SetupTest();
            Assert.Equal(_expectedRoles.Count,
                         ((JobDescription) Actual[0]).Roles.Count);
            Assert.True(
                _expectedRoles.SequenceEqual(((JobDescription) Actual[0]).Roles),
                "Roles are not equal");
        }

        [Fact]
        public void then_the_duplication_from_the_third_level_should_be_removed_from_list()
        {
            SetupTest();
            Assert.Equal(_expectedResponsibilites.Count,
                         ((JobDescription) Actual[0]).Roles[0].Responsibilities.Count);
            Assert.True(
                _expectedResponsibilites.SequenceEqual(((JobDescription) Actual[0]).Roles[0].Responsibilities),
                "Responsibilities are not equal");
        }
    }

    public class and_the_second_level_is_not_fetched : and_passing_a_list_the_have_duplication_on_second_level
    {
        public override void SetupTest()
        {
            base.SetupTest();
            Actual = Transformer.TransformList(Data);
        }

        [Fact]
        public void then_the_duplication_from_the_first_level_should_be_removed_from_list()
        {
            SetupTest();
            Assert.Equal(2, Actual.Count);
        }

        [Fact]
        public void then_the_duplication_from_the_second_level_should_not_be_removed_from_list()
        {
            SetupTest();
            Assert.Equal(3,
                         ((JobDescription) Actual[0]).Authorities.Count);
        }
    }
}