namespace Mailosaur.Models
{
    using System.Collections.Generic;

    public class DeviceListResult
    {
        /// <summary>
        /// Gets or sets the individual devices forming the result.
        /// </summary>
        public IList<Device> Items { get; set; }

    }
}
