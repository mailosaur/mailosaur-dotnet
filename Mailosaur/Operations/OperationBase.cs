namespace Mailosaur.Operations
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Mailosaur.Models;
    using Newtonsoft.Json;

    public class OperationBase
    {
        public async Task ThrowExceptionAsync(HttpResponseMessage response)
        {
            var message = string.Format("Operation returned an invalid status code '{0}'", response.StatusCode);
            var ex = new MailosaurException(message);

            try
            {
                var content = await response.Content.ReadAsStringAsync();
                var mailosaurError = JsonConvert.DeserializeObject<MailosaurError>(content);

                if (mailosaurError != null)
                    ex.MailosaurError = mailosaurError;
            }
            catch (Exception) 
            { 
                // Ignore the exception
            }

            throw ex;
        }
    }
}