using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSanta
{
    public class Relationship
    {
        public Relationship(Person left, Person right)        
        {
            Left = left;
            Right = right;
        }

        public Person Left { get; private set; }
        public Person Right { get; private set; }
    }
}
