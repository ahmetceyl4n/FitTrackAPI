using System.Collections.Generic;
using System.Linq;

namespace FitnessApp.Application.Common.Models
{
    public class ServiceResult<T>
    {
        public bool Succeeded { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> Errors { get; set; }

        public static ServiceResult<T> Success(T data)
        {
            return new ServiceResult<T> { Succeeded = true, Data = data };
        }

        public static ServiceResult<T> Failure(string errorMessage)
        {
            return new ServiceResult<T> 
            { 
                Succeeded = false, 
                ErrorMessage = errorMessage, 
                Errors = new List<string> { errorMessage } 
            };
        }

        public static ServiceResult<T> Failure(IEnumerable<string> errors)
        {
            var errorList = errors.ToList();
            return new ServiceResult<T> 
            { 
                Succeeded = false, 
                Errors = errorList, 
                ErrorMessage = string.Join(", ", errorList) 
            };
        }
    }
}
