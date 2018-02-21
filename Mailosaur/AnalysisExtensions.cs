// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Mailosaur
{
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for Analysis.
    /// </summary>
    public static partial class AnalysisExtensions
    {
            /// <summary>
            /// Perform a spam test
            /// </summary>
            /// <remarks>
            /// Perform spam testing on the specified email
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='email'>
            /// The identifier of the email to be analyzed.
            /// </param>
            public static SpamAnalysisResult Spam(this IAnalysis operations, System.Guid email)
            {
                return operations.SpamAsync(email).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Perform a spam test
            /// </summary>
            /// <remarks>
            /// Perform spam testing on the specified email
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='email'>
            /// The identifier of the email to be analyzed.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<SpamAnalysisResult> SpamAsync(this IAnalysis operations, System.Guid email, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.SpamWithHttpMessagesAsync(email, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
