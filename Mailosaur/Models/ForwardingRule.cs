namespace Mailosaur.Models
{
    public class ForwardingRule
    {
        /// <summary>
        /// Gets or sets possible values include: 'from', 'to', 'subject'
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'endsWith', 'startsWith',
        /// 'contains'
        /// </summary>
        public string Operator { get; set; }

        public string Value { get; set; }

        public string ForwardTo { get; set; }

    }
}
