// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Mailosaur.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class Link
    {
        /// <summary>
        /// Initializes a new instance of the Link class.
        /// </summary>
        public Link()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the Link class.
        /// </summary>
        public Link(string href = default(string), string text = default(string))
        {
            Href = href;
            Text = text;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

    }
}