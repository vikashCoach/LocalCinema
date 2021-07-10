using System;
using System.Collections.Generic;

namespace LocalCinema.Data.Model
{
    public class OperationException : Exception
    {

        public ICollection<OperationError> Errors {get;} = new List<OperationError>();
    }
    public class OperationError
    {
        public string Code {get;set;}
        public string Message {get;set;}

        public dynamic Source {get;set;}
        
    }
}