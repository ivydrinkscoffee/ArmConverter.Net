using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static ArmConverter.Parser.Assemble;
using static ArmConverter.Utilities;

namespace ArmConverter {
    /// <summary>
    /// The main <c>Assembler</c> class.
    /// Contains methods for assembling ARM assembly code into hex code.
    /// <list type="bullet">
    /// <item>
    /// <term>Assemble</term>
    /// <description>Main assembling method</description>
    /// </item>
    /// <item>
    /// <term>MultiAssemble</term>
    /// <description>Main multi-assembling method</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>This class can assemble all types of ARM assembly code.</para>
    /// <para>These methods are performed on strings and string arrays only.</para>
    /// </remarks>
    public static class Assembler {
        /// <summary>
        /// Assembles the specified assembly code <paramref name="assembly"/> and returns the result.
        /// </summary>
        /// <returns>The resulting hex code from the assembled assembly code.</returns>
        /// <example>
        /// <code>
        /// string hex = Assembler.Assemble ("mov w0, #0");
        /// Console.WriteLine (hex);
        /// </code>
        /// </example>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when an error occurs after attempting to assemble the assembly code.
        /// </exception>
        /// See <see cref="Assembler.MultiAssemble(string[], ArchSelection, int?)"/> to assemble an array of lines of assemble code.
        /// <param name="assembly">The assembly code string to assemble.</param>
        /// <param name="archSelection">The architecture that the assembly code <paramref name="assembly"/> corresponds to.</param>
        /// <param name="offset">The offset integer that the resulting hex code should be shifted by.</param>
        public static string Assemble (string assembly, ArchSelection archSelection = ArchSelection.AArch64, int? offset = null) {
            var webClient = new WebClient ();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

            string json = $"{{\"asm\":\"{assembly}\",\"offset\":\"0x{offset ?? 0}\",\"arch\":\"{ArchToString(archSelection)}\"}}";
            string result = webClient.UploadString ("https://armconverter.com/api/convert", json);

            switch (archSelection) {
                case ArchSelection.AArch64:
                    string aarch64Hex = AArch64Json.FromJson (result).Hex.AArch64[1].ToString ().Replace ("### ", string.Empty);

                    if (ExceptionMessageList.Contains (aarch64Hex)) {
                        throw new InvalidOperationException (aarch64Hex);
                    }

                    return aarch64Hex;
                case ArchSelection.AArch32:
                    string aarch32Hex = AArch32Json.FromJson (result).Hex.AArch32[1].ToString ().Replace ("### ", string.Empty);

                    if (ExceptionMessageList.Contains (aarch32Hex)) {
                        throw new InvalidOperationException (aarch32Hex);
                    }

                    return aarch32Hex;
                case ArchSelection.Thumb:
                    string thumbHex = ThumbJson.FromJson (result).Hex.Thumb[1].ToString ().Replace ("### ", string.Empty);

                    if (ExceptionMessageList.Contains (thumbHex)) {
                        throw new InvalidOperationException (thumbHex);
                    }

                    return thumbHex;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Assembles the specified assembly code array <paramref name="assembly"/> and returns the result.
        /// </summary>
        /// <returns>The resulting hex code array from the assembled assembly code.</returns>
        /// <example>
        /// <code>
        /// string[] assembly = { "mov w0, #0", "ret", "ldr w3, [x1]", "and w3, w3, #0xff", "cmp w3, #0x61", "b.ne #0x1c", "adr x1, #0x24", "sub sp, sp, #0x60", "b #0xfffffffffffcbff4" };
        /// string[] hex = Assembler.MultiAssemble (assembly);
        /// Console.WriteLine (string.Join ('\n', hex));
        /// </code>
        /// </example>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when an error occurs after attempting to assemble one of the lines of the assembly code.
        /// </exception>
        /// See <see cref="Assembler.Assemble(string, ArchSelection, int?)"/> to assemble a single line of assemble code.
        /// <param name="assembly">The assembly code string array to assemble.</param>
        /// <param name="archSelection">The architecture that the assembly code <paramref name="assembly"/> corresponds to.</param>
        /// <param name="offset">The offset integer that the resulting hex code should be shifted by.</param>
        public static string[] MultiAssemble (string[] assembly, ArchSelection archSelection = ArchSelection.AArch64, int? offset = null) {
            var hex = new List<string> ();

            foreach (string line in assembly) {
                var webClient = new WebClient ();
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

                string json = $"{{\"asm\":\"{line}\",\"offset\":\"0x{offset ?? 0}\",\"arch\":\"{ArchToString(archSelection)}\"}}";
                string result = webClient.UploadString ("https://armconverter.com/api/convert", json);

                switch (archSelection) {
                    case ArchSelection.AArch64:
                        string aarch64Hex = AArch64Json.FromJson (result).Hex.AArch64[1].ToString ().Replace ("### ", string.Empty);

                        if (ExceptionMessageList.Contains (aarch64Hex)) {
                            throw new InvalidOperationException (aarch64Hex);
                        }

                        hex.Add (aarch64Hex);
                        break;
                    case ArchSelection.AArch32:
                        string aarch32Hex = AArch32Json.FromJson (result).Hex.AArch32[1].ToString ().Replace ("### ", string.Empty);

                        if (ExceptionMessageList.Contains (aarch32Hex)) {
                            throw new InvalidOperationException (aarch32Hex);
                        }

                        hex.Add (aarch32Hex);
                        break;
                    case ArchSelection.Thumb:
                        string thumbHex = ThumbJson.FromJson (result).Hex.Thumb[1].ToString ().Replace ("### ", string.Empty);

                        if (ExceptionMessageList.Contains (thumbHex)) {
                            throw new InvalidOperationException (thumbHex);
                        }

                        hex.Add (thumbHex);
                        break;
                    default:
                        return null;
                }
            }

            return hex.ToArray ();
        }
    }
}