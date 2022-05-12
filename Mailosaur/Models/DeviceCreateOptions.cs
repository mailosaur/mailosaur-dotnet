namespace Mailosaur.Models
{
    public class DeviceCreateOptions
    {
        /// <summary>
        /// Initializes a new instance of the ServerCreateOptions class.
        /// </summary>
        /// <param name="name">A name used to identify the device.</param>
        /// <param name="sharedSecret">The base32-encoded shared secret for this device.</param>
        public DeviceCreateOptions(string name, string sharedSecret)
        {
            Name = name;
            SharedSecret = sharedSecret;
        }

        /// <summary>
        /// A name used to identify the device.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The base32-encoded shared secret for this device.
        /// </summary>
        public string SharedSecret { get; set; }

    }
}
