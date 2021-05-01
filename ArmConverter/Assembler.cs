using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static ArmConverter.Parser.Assemble;
using static ArmConverter.Utilities;

namespace ArmConverter {
    /// <summary>
    /// The main <c>Assembler</c> class.
    /// Contains methods for assembling ARM assembly code into hex code.
    /// <list type="bullet">
    /// <item>
    /// <term>Assemble</term>
    /// <description>Synchronous assembling method</description>
    /// </item>
    /// <item>
    /// <term>MultiAssemble</term>
    /// <description>Synchronous multi-assembling method</description>
    /// </item>
    /// <item>
    /// <term>AssembleAsync</term>
    /// <description>Asynchronous assembling method</description>
    /// </item>
    /// <item>
    /// <term>MultiAssembleAsync</term>
    /// <description>Asynchronous multi-assembling method</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>This class can assemble all types of ARM assembly code.</para>
    /// <para>These methods are performed on strings and string arrays only.</para>
    /// </remarks>
    public static class Assembler {
        #region Synchronous Methods
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
        /// <exception cref="System.FormatException">
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
                    string aArch64Hex = AArch64Json.FromJson (result).Hex.AArch64[1].ToString ().Replace ("### ", string.Empty);

                    if (ExceptionMessageList.Contains (aArch64Hex)) {
                        throw new FormatException (aArch64Hex);
                    }

                    return aArch64Hex;
                case ArchSelection.AArch64BigEndian:
                    string aArch64BigEndianHex = AArch64BigEndianJson.FromJson (result).Hex.AArch64BigEndian[1].ToString ().Replace ("### ", string.Empty);

                    if (ExceptionMessageList.Contains (aArch64BigEndianHex)) {
                        throw new FormatException (aArch64BigEndianHex);
                    }

                    return aArch64BigEndianHex;
                case ArchSelection.AArch32:
                    string aArch32Hex = AArch32Json.FromJson (result).Hex.AArch32[1].ToString ().Replace ("### ", string.Empty);

                    if (ExceptionMessageList.Contains (aArch32Hex)) {
                        throw new FormatException (aArch32Hex);
                    }

                    return aArch32Hex;
                case ArchSelection.AArch32BigEndian:
                    string aArch32BigEndianHex = AArch32BigEndianJson.FromJson (result).Hex.AArch32BigEndian[1].ToString ().Replace ("### ", string.Empty);

                    if (ExceptionMessageList.Contains (aArch32BigEndianHex)) {
                        throw new FormatException (aArch32BigEndianHex);
                    }

                    return aArch32BigEndianHex;
                case ArchSelection.Thumb:
                    string thumbHex = ThumbJson.FromJson (result).Hex.Thumb[1].ToString ().Replace ("### ", string.Empty);

                    if (ExceptionMessageList.Contains (thumbHex)) {
                        throw new FormatException (thumbHex);
                    }

                    return thumbHex;
                case ArchSelection.ThumbBigEndian:
                    string thumbBigEndianHex = ThumbBigEndianJson.FromJson (result).Hex.ThumbBigEndian[1].ToString ().Replace ("### ", string.Empty);

                    if (ExceptionMessageList.Contains (thumbBigEndianHex)) {
                        throw new FormatException (thumbBigEndianHex);
                    }

                    return thumbBigEndianHex;
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
        /// <exception cref="System.FormatException">
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
                        string aArch64Hex = AArch64Json.FromJson (result).Hex.AArch64[1].ToString ().Replace ("### ", string.Empty);

                        if (ExceptionMessageList.Contains (aArch64Hex)) {
                            throw new FormatException (aArch64Hex);
                        }

                        hex.Add (aArch64Hex);
                        break;
                    case ArchSelection.AArch64BigEndian:
                        string aArch64BigEndianHex = AArch64BigEndianJson.FromJson (result).Hex.AArch64BigEndian[1].ToString ().Replace ("### ", string.Empty);

                        if (ExceptionMessageList.Contains (aArch64BigEndianHex)) {
                            throw new FormatException (aArch64BigEndianHex);
                        }

                        hex.Add (aArch64BigEndianHex);
                        break;
                    case ArchSelection.AArch32:
                        string aArch32Hex = AArch32Json.FromJson (result).Hex.AArch32[1].ToString ().Replace ("### ", string.Empty);

                        if (ExceptionMessageList.Contains (aArch32Hex)) {
                            throw new FormatException (aArch32Hex);
                        }

                        hex.Add (aArch32Hex);
                        break;
                    case ArchSelection.AArch32BigEndian:
                        string aArch32BigEndianHex = AArch32BigEndianJson.FromJson (result).Hex.AArch32BigEndian[1].ToString ().Replace ("### ", string.Empty);

                        if (ExceptionMessageList.Contains (aArch32BigEndianHex)) {
                            throw new FormatException (aArch32BigEndianHex);
                        }

                        hex.Add (aArch32BigEndianHex);
                        break;
                    case ArchSelection.Thumb:
                        string thumbHex = ThumbJson.FromJson (result).Hex.Thumb[1].ToString ().Replace ("### ", string.Empty);

                        if (ExceptionMessageList.Contains (thumbHex)) {
                            throw new FormatException (thumbHex);
                        }

                        hex.Add (thumbHex);
                        break;
                    case ArchSelection.ThumbBigEndian:
                        string thumbBigEndianHex = ThumbBigEndianJson.FromJson (result).Hex.ThumbBigEndian[1].ToString ().Replace ("### ", string.Empty);

                        if (ExceptionMessageList.Contains (thumbBigEndianHex)) {
                            throw new FormatException (thumbBigEndianHex);
                        }

                        hex.Add (thumbBigEndianHex);
                        break;
                    default:
                        return null;
                }
            }

            return hex.ToArray ();
        }
        #endregion

        #region Asynchronous Methods
        /// <summary>
        /// Asynchronously assembles the specified assembly code <paramref name="assembly"/> and returns the result, this method does not block the calling thread.
        /// </summary>
        /// <returns>The resulting hex code from the assembled assembly code.</returns>
        /// <example>
        /// <code>
        /// string hex = await Assembler.AssembleAsync ("mov w0, #0");
        /// Console.WriteLine (hex);
        /// </code>
        /// </example>
        /// <exception cref="System.FormatException">
        /// Thrown when an error occurs after attempting to assemble the assembly code.
        /// </exception>
        /// See <see cref="Assembler.MultiAssembleAsync(string[], ArchSelection, int?)"/> to assemble an array of lines of assemble code asynchronously.
        /// <param name="assembly">The assembly code string to assemble.</param>
        /// <param name="archSelection">The architecture that the assembly code <paramref name="assembly"/> corresponds to.</param>
        /// <param name="offset">The offset integer that the resulting hex code should be shifted by.</param>
        public static async Task<string> AssembleAsync (string assembly, ArchSelection archSelection = ArchSelection.AArch64, int? offset = null) {
            var webClient = new WebClient ();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

            string json = $"{{\"asm\":\"{assembly}\",\"offset\":\"0x{offset ?? 0}\",\"arch\":\"{ArchToString(archSelection)}\"}}";
            string result = await webClient.UploadStringTaskAsync ("https://armconverter.com/api/convert", json);

            switch (archSelection) {
                case ArchSelection.AArch64:
                    var aArch64Json = await AArch64Json.FromJsonAsync (result);
                    string aArch64Hex = aArch64Json.Hex.AArch64[1].ToString ().Replace ("### ", string.Empty);

                    if (ExceptionMessageList.Contains (aArch64Hex)) {
                        throw new FormatException (aArch64Hex);
                    }

                    return aArch64Hex;
                case ArchSelection.AArch64BigEndian:
                    var aArch64BigEndianJson = await AArch64BigEndianJson.FromJsonAsync (result);
                    string aArch64BigEndianHex = aArch64BigEndianJson.Hex.AArch64BigEndian[1].ToString ().Replace ("### ", string.Empty);

                    if (ExceptionMessageList.Contains (aArch64BigEndianHex)) {
                        throw new FormatException (aArch64BigEndianHex);
                    }

                    return aArch64BigEndianHex;
                case ArchSelection.AArch32:
                    var aArch32Json = await AArch32Json.FromJsonAsync (result);
                    string aArch32Hex = aArch32Json.Hex.AArch32[1].ToString ().Replace ("### ", string.Empty);

                    if (ExceptionMessageList.Contains (aArch32Hex)) {
                        throw new FormatException (aArch32Hex);
                    }

                    return aArch32Hex;
                case ArchSelection.AArch32BigEndian:
                    var aArch32BigEndianJson = await AArch32BigEndianJson.FromJsonAsync (result);
                    string aArch32BigEndianHex = aArch32BigEndianJson.Hex.AArch32BigEndian[1].ToString ().Replace ("### ", string.Empty);

                    if (ExceptionMessageList.Contains (aArch32BigEndianHex)) {
                        throw new FormatException (aArch32BigEndianHex);
                    }

                    return aArch32BigEndianHex;
                case ArchSelection.Thumb:
                    var thumbJson = await ThumbJson.FromJsonAsync (result);
                    string thumbHex = thumbJson.Hex.Thumb[1].ToString ().Replace ("### ", string.Empty);

                    if (ExceptionMessageList.Contains (thumbHex)) {
                        throw new FormatException (thumbHex);
                    }

                    return thumbHex;
                case ArchSelection.ThumbBigEndian:
                    var thumbBigEndianJson = await ThumbBigEndianJson.FromJsonAsync (result);
                    string thumbBigEndianHex = thumbBigEndianJson.Hex.ThumbBigEndian[1].ToString ().Replace ("### ", string.Empty);

                    if (ExceptionMessageList.Contains (thumbBigEndianHex)) {
                        throw new FormatException (thumbBigEndianHex);
                    }

                    return thumbBigEndianHex;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Asynchronously assembles the specified assembly code array <paramref name="assembly"/> and returns the result, this method does not block the calling thread.
        /// </summary>
        /// <returns>The resulting hex code array from the assembled assembly code.</returns>
        /// <example>
        /// <code>
        /// string[] assembly = { "mov w0, #0", "ret", "ldr w3, [x1]", "and w3, w3, #0xff", "cmp w3, #0x61", "b.ne #0x1c", "adr x1, #0x24", "sub sp, sp, #0x60", "b #0xfffffffffffcbff4" };
        /// string[] hex = await Assembler.MultiAssembleAsync (assembly);
        /// Console.WriteLine (string.Join ('\n', hex));
        /// </code>
        /// </example>
        /// <exception cref="System.FormatException">
        /// Thrown when an error occurs after attempting to assemble one of the lines of the assembly code.
        /// </exception>
        /// See <see cref="Assembler.AssembleAsync(string, ArchSelection, int?)"/> to assemble a single line of assemble code asynchronously.
        /// <param name="assembly">The assembly code string array to assemble.</param>
        /// <param name="archSelection">The architecture that the assembly code <paramref name="assembly"/> corresponds to.</param>
        /// <param name="offset">The offset integer that the resulting hex code should be shifted by.</param>
        public static async Task<string[]> MultiAssembleAsync (string[] assembly, ArchSelection archSelection = ArchSelection.AArch64, int? offset = null) {
            var hex = new List<string> ();

            foreach (string line in assembly) {
                var webClient = new WebClient ();
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

                string json = $"{{\"asm\":\"{line}\",\"offset\":\"0x{offset ?? 0}\",\"arch\":\"{ArchToString(archSelection)}\"}}";
                string result = await webClient.UploadStringTaskAsync ("https://armconverter.com/api/convert", json);

                switch (archSelection) {
                    case ArchSelection.AArch64:
                        var aArch64Json = await AArch64Json.FromJsonAsync (result);
                        string aArch64Hex = aArch64Json.Hex.AArch64[1].ToString ().Replace ("### ", string.Empty);

                        if (ExceptionMessageList.Contains (aArch64Hex)) {
                            throw new FormatException (aArch64Hex);
                        }

                        hex.Add (aArch64Hex);
                        break;
                    case ArchSelection.AArch64BigEndian:
                        var aArch64BigEndianJson = await AArch64BigEndianJson.FromJsonAsync (result);
                        string aArch64BigEndianHex = aArch64BigEndianJson.Hex.AArch64BigEndian[1].ToString ().Replace ("### ", string.Empty);

                        if (ExceptionMessageList.Contains (aArch64BigEndianHex)) {
                            throw new FormatException (aArch64BigEndianHex);
                        }

                        hex.Add (aArch64BigEndianHex);
                        break;
                    case ArchSelection.AArch32:
                        var aArch32Json = await AArch32Json.FromJsonAsync (result);
                        string aArch32Hex = aArch32Json.Hex.AArch32[1].ToString ().Replace ("### ", string.Empty);

                        if (ExceptionMessageList.Contains (aArch32Hex)) {
                            throw new FormatException (aArch32Hex);
                        }

                        hex.Add (aArch32Hex);
                        break;
                    case ArchSelection.AArch32BigEndian:
                        var aArch32BigEndianJson = await AArch32BigEndianJson.FromJsonAsync (result);
                        string aArch32BigEndianHex = aArch32BigEndianJson.Hex.AArch32BigEndian[1].ToString ().Replace ("### ", string.Empty);

                        if (ExceptionMessageList.Contains (aArch32BigEndianHex)) {
                            throw new FormatException (aArch32BigEndianHex);
                        }

                        hex.Add (aArch32BigEndianHex);
                        break;
                    case ArchSelection.Thumb:
                        var thumbJson = await ThumbJson.FromJsonAsync (result);
                        string thumbHex = thumbJson.Hex.Thumb[1].ToString ().Replace ("### ", string.Empty);

                        if (ExceptionMessageList.Contains (thumbHex)) {
                            throw new FormatException (thumbHex);
                        }

                        hex.Add (thumbHex);
                        break;
                    case ArchSelection.ThumbBigEndian:
                        var thumbBigEndianJson = await ThumbBigEndianJson.FromJsonAsync (result);
                        string thumbBigEndianHex = thumbBigEndianJson.Hex.ThumbBigEndian[1].ToString ().Replace ("### ", string.Empty);

                        if (ExceptionMessageList.Contains (thumbBigEndianHex)) {
                            throw new FormatException (thumbBigEndianHex);
                        }

                        hex.Add (thumbBigEndianHex);
                        break;
                    default:
                        return null;
                }
            }

            return hex.ToArray ();
        }
        #endregion
    }
}