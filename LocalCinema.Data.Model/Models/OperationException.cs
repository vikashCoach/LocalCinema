using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LocalCinema.Data.Model
{
    public class OperationException : Exception
    {
        public ICollection<OperationError> Errors {get;} = new List<OperationError>();
        public OperationException() { }

        public OperationException(OperationError error)
        {
            Errors.Add(error);
        }
    }
    public class OperationError
    {
        public string Code {get;set;}
        public string Message {get;set;}

        public dynamic Source {get;set;}
        public OperationError() { }

        public OperationError(string errorCode, string message, dynamic source = null)
        {
            Code = errorCode;
            Message = message;
            Source = source;
        }

    }
    public class OperationResult
    {
        public ICollection<OperationError> Errors { get; }
        public ICollection<ValidationResult> ValidationError { get; }

        public bool IsValid
        {
            get { return !Errors.Any(); }
        }

        public OperationResult() : this(null) { }

        public OperationResult(ICollection<OperationError> errors)
        {
            Errors = errors ?? new List<OperationError>();
        }

        public void AddError(OperationError error)
        {
            Errors.Add(error);
        }

        public void AddErrors(IEnumerable<OperationError> errors)
        {
            foreach (var error in errors)
                Errors.Add(error);
        }
        public void AddErrors(IEnumerable<ValidationResult> errors)
        {
            foreach (var error in errors)
                ValidationError.Add(error);
        }

        public void AddErrorsFromException(OperationException operationException)
        {
            if (operationException.Errors.Count > 0)
                AddErrors(operationException.Errors);
            else
            {
                var error = new OperationError(ErrorCodes.InternalError,
                    "Oops something broke. Please contact us with details of your request so we can investigate further.");

                AddError(error);
            }
        }

        public static OperationResult operator +(OperationResult left, OperationResult right)
        {
            foreach (var error in right.Errors)
                left.AddError(error);

            return left;
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Result { get; set; }

        public OperationResult() : base(null) { }

        public OperationResult(ICollection<OperationError> errors) : base(errors) { }

        public static OperationResult<T> operator +(OperationResult<T> left, OperationResult<T> right)
        {
            foreach (var error in right.Errors)
                left.AddError(error);

            return left;
        }
    }
    public static class ErrorCodes
    {
        public const string Unauthorized = "unauthorized";
        public const string AccountLocked = "account_locked";
        public const string AccountUnVerified = "account_unverified";
        public const string Forbidden = "forbidden";
        public const string InvalidOperation = "invalid_operation";
        public const string NotFound = "not_found";
        public const string ResourceExists = "resource_exists";
        public const string InvalidInput = "invalid_input";
        public const string InvalidState = "invalid_state";
        public const string InternalError = "internal_error";
    }

}