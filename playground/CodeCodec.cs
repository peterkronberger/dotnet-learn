using System.Text;

namespace Playground;

/// <summary>Reads code from a Base64url-encoded URL fragment (UTF-8).</summary>
internal static class CodeCodec
{
    public static string? TryDecode(string encoded)
    {
        try
        {
            var base64 = encoded.Replace('-', '+').Replace('_', '/');
            base64 = base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        }
        catch (FormatException)
        {
            return null;
        }
    }
}
