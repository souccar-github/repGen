using System;
using System.Collections.Generic;
using System.Reflection;
using SharpTestsEx;
using Souccar.Domain.Extensions;
using Souccar.ReportGenerator.Test.TestClasses;
using Xunit;

namespace Souccar.ReportGenerator.Test
{
    public class AssemblyExtensionsTest
    {
        [Fact]
        public void GetAggregateClassesTest()
        {
            Assembly a = Assembly.GetAssembly(typeof (EntityClass1));
            Dictionary<Type, string> actual = a.GetAggregateClasses();
            var expected = new Dictionary<Type, string>
                               {
                                   {typeof (EntityClass1), typeof (EntityClass1).Name},
                                   {typeof (EntityClass2), typeof (EntityClass2).Name},
                                   {typeof (EntityClassChild), typeof (EntityClassChild).Name}
                               };
            actual.Count.Should().Be.EqualTo(expected.Count);

            Assert.Equal(expected.Count, actual.Count);
            foreach (var classRecord in expected)
            {
                string actualValue;
                actual.TryGetValue(classRecord.Key, out actualValue);
                Assert.Equal(classRecord.Value, actualValue);
            }
        }
    }
}