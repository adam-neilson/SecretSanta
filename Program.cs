using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace SecretSanta
{
    class Program
    {
        private const string EmailBodyFormat = @"Hello {0}

You are buying for {1}!

Merry Christmas!";

        private static Random Randomiser = new Random();
        private static string Auditor = "someoneToCheckIt@gmail.com";

        private static Person personA = new Person("PersonA", "personA@gmail.com");
        private static Person personB = new Person("PersonB", "personB@gmail.com");
        private static Person personC = new Person("PersonC", "personC@gmail.com");
        private static Person personD = new Person("PersonD", "personD@gmail.com");

        private static IEnumerable<Person> Persons = new List<Person> { personA, personB, personC, personD };

        private static IEnumerable<Relationship> Relationships = new List<Relationship>
        {
            new Relationship(personC, personB),
            new Relationship(personA, personD),
        };

        static void Main(string[] args)
        {
            var masterPossiblities = Persons.ToDictionary(person => person, person =>
                {
                    var relationships = Relationships.Where(x => x.Left == person || x.Right == person).Select(relationship => relationship.Left == person ? relationship.Right : relationship.Left);

                    return Persons.Except(relationships.Concat(new[] { person }));
                });

            var giftersTodo = Persons.ToList();
            var receiversTodo = Persons.ToList();
            var matches = new List<Tuple<Person, Person>>();

            foreach (var person in Persons)
            {
                giftersTodo.Remove(person);

                // Note: Ensure there aren't any gifters left with no possiblities.
                var possiblities = receiversTodo.Intersect(masterPossiblities[person]).Where(p => !giftersTodo.Any(g => !receiversTodo.Intersect(masterPossiblities[g]).Except(new[] { p }).Any())).ToList();

                var count = possiblities.Count();
                var value = Randomiser.Next(count); // Note, less than the specified maximum, so count is fine.

                var match = possiblities.ElementAt(value);

                receiversTodo.Remove(match);

                matches.Add(Tuple.Create(person, match));
            }

            var smtpClient = new SmtpClient();

            foreach (var match in matches)
            {
                // Paranoid assertion
                if (match.Item1 == match.Item2 || Relationships.Any(x => new[] { x.Left, x.Right }.SequenceEqual(new[] { match.Item1, match.Item2 })))
                {
                    throw new Exception(string.Format("Invalid match {0} - {1}!", match.Item1.Name, match.Item2.Name));
                }

                Console.WriteLine("Sending email to {0}", match.Item1.Email);
                smtpClient.Send("sender@gmail.com", match.Item1.Email, "Your Secret Santa Recipient!", string.Format(EmailBodyFormat, match.Item1.Name, match.Item2.Name));
            }

            string summary = string.Join("\r\n", matches.Select(match => string.Format("{0} is buying for {1}", match.Item1, match.Item2)));
            smtpClient.Send("sender@gmail.com", Auditor, "Secret Santa Outcome", summary);
        }
    }
}
