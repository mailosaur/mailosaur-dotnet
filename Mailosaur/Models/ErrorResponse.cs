namespace Mailosaur.Models
{
    using System.Collections.Generic;

    public class ErrorResponse
    {
        public IList<Error> Errors { get; set; }
    }

    public class Error
    {
        public string Field { get; set; }
        public IList<ErrorDetail> Detail { get; set; }
    }

    public class ErrorDetail
    {
        public string Description { get; set; }
    }
}
