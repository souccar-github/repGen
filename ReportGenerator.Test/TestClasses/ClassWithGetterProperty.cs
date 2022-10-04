namespace Souccar.ReportGenerator.Test.TestClasses
{
    public class ClassWithGetterProperty
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public IndexClass1 Index
        {
            get { return new IndexClass1(); }
        }
    }
}