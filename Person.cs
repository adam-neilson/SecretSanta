using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SecretSanta
{
    [DebuggerDisplay("{Name} - {Email}")]
    public class Person
    {
        public Person(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }

        protected bool Equals(Person other)
        {
            return other != null && string.Equals(ToString(), other.ToString());
        }

        public override bool Equals(object obj)
        {
            return obj is Person && this.Equals((Person)obj);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Email);
        }
    }
}
