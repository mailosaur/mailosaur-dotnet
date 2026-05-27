namespace Mailosaur.Operations
{
    using Models;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Operations for managing virtual security devices and retrieving their current one-time
    /// passwords (OTPs), used to automate testing of app-based multi-factor authentication.
    /// Accessed via <c>client.Devices</c>.
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
        /// <returns>
        /// A <see cref="DeviceListResult"/> containing your devices.
        /// </returns>
        public DeviceListResult List()
            => Task.Run(async () => await ListAsync()).UnwrapException<DeviceListResult>();

        /// <summary>
        /// List all devices
        /// </summary>
        /// <remarks>
        /// Returns a list of your virtual security devices.
        /// </remarks>
        /// <returns>
        /// A task resolving to a <see cref="DeviceListResult"/> containing your devices.
        /// </returns>
        public Task<DeviceListResult> ListAsync()
            => ExecuteRequest<DeviceListResult>(HttpMethod.Get, $"api/devices");

        /// <summary>
        /// Create a device
        /// </summary>
        /// <remarks>
        /// Creates a new virtual security device and returns it.
        /// </remarks>
        /// <param name='deviceCreateOptions'>
        /// Options used to create a new Mailosaur virtual security device.
        /// </param>
        /// <returns>
        /// The newly-created <see cref="Device"/>.
        /// </returns>
        public Device Create(DeviceCreateOptions deviceCreateOptions)
            => Task.Run(async () => await CreateAsync(deviceCreateOptions)).UnwrapException<Device>();

        /// <summary>
        /// Create a device
        /// </summary>
        /// <remarks>
        /// Creates a new virtual security device and returns it.
        /// </remarks>
        /// <param name='deviceCreateOptions'>
        /// Options used to create a new Mailosaur virtual security device.
        /// </param>
        /// <returns>
        /// A task resolving to the newly-created <see cref="Device"/>.
        /// </returns>
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
        /// <returns>
        /// An <see cref="OtpResult"/> containing the current one-time password.
        /// </returns>
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
        /// <returns>
        /// A task resolving to an <see cref="OtpResult"/> containing the current one-time password.
        /// </returns>
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
        /// <returns>
        /// A task that completes once the device has been deleted.
        /// </returns>
        public Task DeleteAsync(string id)
            => ExecuteRequest(HttpMethod.Delete, $"api/devices/{id}");
    }
}
