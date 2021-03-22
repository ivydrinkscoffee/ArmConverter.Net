using System.Collections.Generic;
using System.Net;
using static ArmConverter.Parser.Disassemble;
using static ArmConverter.Utilities;

namespace ArmConverter {
    /// <summary>
    /// The main <c>Disassembler</c> class.
    /// Contains methods for disassembling ARM hex code into assembly code.
    /// <list type="bullet">
    /// <item>
    /// <term>Disassemble</term>
    /// <description>Main disassembling method</description>
    /// </item>
    /// <item>
    /// <term>MultiDisassemble</term>
    /// <description>Main multi-disassembling method</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>This class can disassemble all types of ARM hex code.</para>
    /// <para>These methods are performed on strings and string arrays only.</para>
    /// </remarks>
    public static class Disassembler {
        /// <summary>
        /// Disassembles the specified hex code <paramref name="hex"/> and returns the result.
        /// </summary>
        /// <returns>The resulting assembly code from the disassembled hex code.</returns>
        /// <example>
        /// <code>
        /// string assembly = Disassembler.Disassemble ("00008052");
        /// Console.WriteLine (assembly);
        /// </code>
        /// </example>
        /// <exception cref="System.Net.WebException">
        /// Thrown when the hex code is invalid.
        /// </exception>
        /// See <see cref="Disassembler.MultiDisassemble(string[], ArchSelection, int?)"/> to disassemble an array of elements of hex code.
        /// <param name="hex">The hex code string to disassemble.</param>
        /// <param name="archSelection">The architecture that the hex code <paramref name="hex"/> corresponds to.</param>
        /// <param name="offset">The offset integer that the hex code <paramref name="hex"/> is shifted by.</param>
        public static string Disassemble (string hex, ArchSelection archSelection = ArchSelection.AArch64, int? offset = null) {
            var webClient = new WebClient ();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

            string json = $"{{\"hex\":\"{hex}\",\"offset\":\"0x{offset ?? 0}\",\"arch\":\"{ArchToString(archSelection)}\"}}";
            string result = webClient.UploadString ("https://armconverter.com/api/convert", json);

            switch (archSelection) {
                case ArchSelection.AArch64:
                    string aarch64Assembly = AArch64Json.FromJson (result).Asm.AArch64[1].ToString ();
                    return aarch64Assembly;
                case ArchSelection.AArch32:
                    string aarch32Assembly = AArch32Json.FromJson (result).Asm.AArch32[1].ToString ();
                    return aarch32Assembly;
                case ArchSelection.Thumb:
                    string thumbAssembly = ThumbJson.FromJson (result).Asm.Thumb[1].ToString ();
                    return thumbAssembly;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Disassembles the specified hex code array <paramref name="hex"/> and returns the result.
        /// </summary>
        /// <returns>The resulting assembly code array from the disassembled hex code.</returns>
        /// <example>
        /// <code>
        /// string[] hex = { "00008052", "C0035FD6", "230040B9", "631C0012", "7F840171", "E1000054", "21010010", "FF8301D1", "FD2FFF17" };
        /// string[] assembly = Disassembler.MultiDisassemble (hex);
        /// Console.WriteLine (string.Join ('\n', assembly));
        /// </code>
        /// </example>
        /// <exception cref="System.Net.WebException">
        /// Thrown when one of the elements of the hex code is invalid.
        /// </exception>
        /// See <see cref="Disassembler.Disassemble(string, ArchSelection, int?)"/> to disassemble a single line of hex code.
        /// <param name="hex">The hex code string array to disassemble.</param>
        /// <param name="archSelection">The architecture that the hex code <paramref name="hex"/> corresponds to.</param>
        /// <param name="offset">The offset integer that the hex code <paramref name="hex"/> is shifted by.</param>
        public static string[] MultiDisassemble (string[] hex, ArchSelection archSelection = ArchSelection.AArch64, int? offset = null) {
            var assembly = new List<string> ();

            foreach (string element in hex) {
                var webClient = new WebClient ();
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

                string json = $"{{\"hex\":\"{element}\",\"offset\":\"0x{offset ?? 0}\",\"arch\":\"{ArchToString(archSelection)}\"}}";
                string result = webClient.UploadString ("https://armconverter.com/api/convert", json);

                switch (archSelection) {
                    case ArchSelection.AArch64:
                        string aarch64Assembly = AArch64Json.FromJson (result).Asm.AArch64[1].ToString ();
                        assembly.Add (aarch64Assembly);
                        break;
                    case ArchSelection.AArch32:
                        string aarch32Assembly = AArch32Json.FromJson (result).Asm.AArch32[1].ToString ();
                        assembly.Add (aarch32Assembly);
                        break;
                    case ArchSelection.Thumb:
                        string thumbAssembly = ThumbJson.FromJson (result).Asm.Thumb[1].ToString ();
                        assembly.Add (thumbAssembly);
                        break;
                    default:
                        return null;
                }
            }

            return assembly.ToArray ();
        }
    }
}
