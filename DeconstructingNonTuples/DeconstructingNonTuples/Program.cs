using System;

namespace DeconstructingNonTuples
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = new User
            {
                FirstName = "Joe",
                LastName = "Bloggs",
                Email = "joe.bloggs@example.com",
                Age = 23
            };


            (var firstName, var lastName, var age) = user;

            (var id, var name) = GetUser();
        }

        static (int id, string name) GetUser()
        {
            return (123, "andrewlock");
        }

        public class User
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
            public string Email { get; set; }

            public void Deconstruct(out string firstName, out string lastName)
            {
                firstName = FirstName;
                lastName = LastName;
            }

            public void Deconstruct(out string firstName, out string lastName, out int age)
            {
                firstName = FirstName;
                lastName = LastName;
                age = Age;
            }
            
        }
    }
}