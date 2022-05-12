namespace Mailosaur.Operations
{
    using Models;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Devices operations.
    /// </summary>
    public class Devices : OperationBase
    {
        /// <summary>
        /// Initializes a new instance of the Devices class.
        /// </summary>
        /// <param name='client'>
        /// Reference to the HttpClient.
        /// </param>
        public Devices(HttpClient client) : base(client) { }

        /// <summary>
        /// List all devices
        /// </summary>
        /// <remarks>
        /// Returns a list of your virtual security devices.
        /// </remarks>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public DeviceListResult List()
            => Task.Run(async () => await ListAsync()).UnwrapException<DeviceListResult>();

        /// <summary>
        /// List all devices
        /// </summary>
        /// <remarks>
        /// Returns a list of your virtual security devices.
        /// </remarks>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Task<DeviceListResult> ListAsync()
            => ExecuteRequest<DeviceListResult>(HttpMethod.Get, $"api/devices");

        /// <summary>
        /// Create a device
        /// </summary>
        /// <remarks>
        /// Creates a new virtual security device and returns it.
        /// </remarks>
        /// <param name='deviceCreateOptions'>
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Device Create(DeviceCreateOptions deviceCreateOptions)
            => Task.Run(async () => await CreateAsync(deviceCreateOptions)).UnwrapException<Device>();

        /// <summary>
        /// Create a device
        /// </summary>
        /// <remarks>
        /// Creates a new virtual security device and returns it.
        /// </remarks>
        /// <param name='deviceCreateOptions'>
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Task<Device> CreateAsync(DeviceCreateOptions deviceCreateOptions)
            => ExecuteRequest<Device>(HttpMethod.Post, "api/devices", deviceCreateOptions);

        /// <summary>
        /// Retrieve the current one-time password.
        /// </summary>
        /// <remarks>
        /// Retrieves the current one-time password for a saved device, or given base32-encoded shared secret.
        /// </remarks>
        /// <param name='query'>
        /// Either the unique identifier of the device, or a base32-encoded shared secret.
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public OtpResult Otp(string query)
            => Task.Run(async () => await OtpAsync(query)).UnwrapException<OtpResult>();

        /// <summary>
        /// Retrieve the current one-time password.
        /// </summary>
        /// <remarks>
        /// Retrieves the current one-time password for a saved device, or given base32-encoded shared secret.
        /// </remarks>
        /// <param name='query'>
        /// Either the unique identifier of the device, or a base32-encoded shared secret.
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Task<OtpResult> OtpAsync(string query)
        {
            if (query.Contains("-"))
            {
                return ExecuteRequest<OtpResult>(HttpMethod.Get, $"api/devices/{query}/otp");
            }

            return ExecuteRequest<OtpResult>(HttpMethod.Post, "api/devices/otp", new { SharedSecret = query });
        }

        /// <summary>
        /// Delete a device
        /// </summary>
        /// <remarks>
        /// Permanently deletes a device. This operation cannot be undone.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the device to be deleted.
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public void Delete(string id)
            => Task.Run(async () => await DeleteAsync(id)).WaitAndUnwrapException();

        /// <summary>
        /// Delete a device
        /// </summary>
        /// <remarks>
        /// Permanently deletes a device. This operation cannot be undone.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the device to be deleted.
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Task DeleteAsync(string id)
            => ExecuteRequest(HttpMethod.Delete, $"api/devices/{id}");
    }
}
