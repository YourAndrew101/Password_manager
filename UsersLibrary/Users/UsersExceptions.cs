using System;

namespace UsersLibrary
{
    public class UsersExceptions
    {
        public class DuplicateMailException : Exception
        {
            public DuplicateMailException() : base("User already exists") { }
        }
        public class NonExistenMailException : Exception
        {
            public NonExistenMailException() : base("User does not exists") { }
        }
        public class IncorrectPasswordException : Exception
        {
            public IncorrectPasswordException() { }
        }
    }
}
