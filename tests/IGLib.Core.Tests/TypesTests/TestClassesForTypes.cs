
#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IGLib.Commands;


namespace IGLib.Types.Tests
{


    public class TestClassBase
    {

        public TestClassBase(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = $"Object of class {ClassName}, No. {Id}";
            }
            Name = name!;
        }

        protected static int _lastId = 0;

        protected static object _lock = new object();

        protected static int NextId()
        {
            return _lastId++;
        }

        public string ClassName => $"{GetType().Name}";

        public virtual int Id { get; } = NextId();

        public virtual string Name { get; init; }

    }

    public class TestClassDerived: TestClassBase
    {

        public TestClassDerived(string? name, string? description): base(name)
        {
            if (string.IsNullOrEmpty(description))
            {
                description = $"This is an object of class {ClassName}, ID = {Id}.";
            }
            Description = description;
        }

        public virtual string? Description { get; init; }


    }




}

