﻿namespace Codixa.Core.Custom_Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message) { }

    }
}
