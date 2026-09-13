namespace Slackbot.Net.Abstractions.Hosting;

/// <summary>
///     Obsolete: renamed to <see cref="ITokenManager"/> since "store" implied a persistence
///     mechanism this interface never actually required. Kept for backwards compatibility -
///     it inherits <see cref="ITokenManager"/>, so any existing implementation of this
///     interface already satisfies the new one.
/// </summary>
[Obsolete("ITokenStore has been renamed to ITokenManager to avoid implying a persistence mechanism. Implement ITokenManager instead. This interface will be removed in a future version.")]
public interface ITokenStore : ITokenManager
{
}
