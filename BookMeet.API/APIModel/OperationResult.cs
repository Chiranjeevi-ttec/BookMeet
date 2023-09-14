using System.ComponentModel.DataAnnotations;
using System.Net;


namespace BookMeet.API.APIModel
{
    /// <summary>
    /// Encapsulates the result of a API action
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OperationResult<T>
    {
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Error Message
        /// </summary>
        [DataType(DataType.Text)]
        public string Error { get; set; }

        /// <summary>
        /// Http Status Code
        /// </summary>
        public HttpStatusCode HttpStatus { get; set; }

    }
}

