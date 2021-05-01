using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static ArmConverter.Parser.Disassemble;
using static ArmConverter.Utilities;

namespace ArmConverter {
    /// <summary>
    /// The main <c>Disassembler</c> class.
    /// Contains methods for disassembling ARM hex code into assembly code.
    /// <list type="bullet">
    /// <item>
    /// <term>Disassemble</term>
    /// <description>Synchronous disassembling method</description>
    /// </item>
    /// <item>
    /// <term>MultiDisassemble</term>
    /// <description>Synchronous multi-disassembling method</description>
    /// </item>
    /// <item>
    /// <term>DisassembleAsync</term>
    /// <description>Asynchronous disassembling method</description>
    /// </item>
    /// <item>
    /// <term>MultiDisassembleAsync</term>
    /// <description>Asynchronous multi-disassembling method</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>This class can disassemble all types of ARM hex code except for ARM64 big-endian.</para>
    /// <para>These methods are performed on strings and string arrays only.</para>
    /// </remarks>
    public static class Disassembler {
        #region Synchronous Methods
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
        /// <exception cref ="System.InvalidOperationException">
        /// Thrown when trying to convert the ARM64 big-endian hex code to assembly code.
        /// </exception>
        /// <exception cref="System.Net.WebException">
        /// Thrown when the hex code is invalid.
        /// </exception>
        /// See <see cref="Disassembler.MultiDisassemble(string[], ArchSelection, int?)"/> to disassemble an array of elements of hex code.
        /// <param name="hex">The hex code string to disassemble.</param>
        /// <param name="archSelection">The architecture that the hex code <paramref name="hex"/> corresponds to.</param>
        /// <param name="offset">The offset integer that the hex code <paramref name="hex"/> is shifted by.</param>
        public static string Disassemble (string hex, ArchSelection archSelection = ArchSelection.AArch64, int? offset = null) {
            if (archSelection == ArchSelection.AArch64BigEndian) {
                throw new InvalidOperationException ("Cannot convert ARM64 big-endian hex code to assembly code");
            } else {
                var webClient = new WebClient ();
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

                string json = $"{{\"hex\":\"{hex}\",\"offset\":\"0x{offset ?? 0}\",\"arch\":\"{ArchToString(archSelection)}\"}}";
                string result = webClient.UploadString ("https://armconverter.com/api/convert", json);

                switch (archSelection) {
                    case ArchSelection.AArch64:
                        string aArch64Assembly = AArch64Json.FromJson (result).Asm.AArch64[1].ToString ();
                        return aArch64Assembly;
                    case ArchSelection.AArch32:
                        string aArch32Assembly = AArch32Json.FromJson (result).Asm.AArch32[1].ToString ();
                        return aArch32Assembly;
                    case ArchSelection.AArch32BigEndian:
                        string aArch32BigEndianAssembly = AArch32BigEndianJson.FromJson (result).Asm.AArch32BigEndian[1].ToString ();
                        return aArch32BigEndianAssembly;
                    case ArchSelection.Thumb:
                        string thumbAssembly = ThumbJson.FromJson (result).Asm.Thumb[1].ToString ();
                        return thumbAssembly;
                    case ArchSelection.ThumbBigEndian:
                        string thumbBigEndianAssembly = ThumbBigEndianJson.FromJson (result).Asm.ThumbBigEndian[1].ToString ();
                        return thumbBigEndianAssembly;
                    default:
                        return null;
                }
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
        /// <exception cref ="System.InvalidOperationException">
        /// Thrown when trying to convert one of the ARM64 big-endian hex code elements to assembly code.
        /// </exception>
        /// <exception cref="System.Net.WebException">
        /// Thrown when one of the elements of the hex code is invalid.
        /// </exception>
        /// See <see cref="Disassembler.Disassemble(string, ArchSelection, int?)"/> to disassemble a single line of hex code.
        /// <param name="hex">The hex code string array to disassemble.</param>
        /// <param name="archSelection">The architecture that the hex code <paramref name="hex"/> corresponds to.</param>
        /// <param name="offset">The offset integer that the hex code <paramref name="hex"/> is shifted by.</param>
        public static string[] MultiDisassemble (string[] hex, ArchSelection archSelection = ArchSelection.AArch64, int? offset = null) {
            if (archSelection == ArchSelection.AArch64BigEndian) {
                throw new InvalidOperationException ("Cannot convert ARM64 big-endian hex code to assembly code");
            } else {
                var assembly = new List<string> ();

                foreach (string element in hex) {
                    var webClient = new WebClient ();
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

                    string json = $"{{\"hex\":\"{element}\",\"offset\":\"0x{offset ?? 0}\",\"arch\":\"{ArchToString(archSelection)}\"}}";
                    string result = webClient.UploadString ("https://armconverter.com/api/convert", json);

                    switch (archSelection) {
                        case ArchSelection.AArch64:
                            string aArch64Assembly = AArch64Json.FromJson (result).Asm.AArch64[1].ToString ();
                            assembly.Add (aArch64Assembly);
                            break;
                        case ArchSelection.AArch32:
                            string aArch32Assembly = AArch32Json.FromJson (result).Asm.AArch32[1].ToString ();
                            assembly.Add (aArch32Assembly);
                            break;
                        case ArchSelection.AArch32BigEndian:
                            string aArch32BigEndianAssembly = AArch32BigEndianJson.FromJson (result).Asm.AArch32BigEndian[1].ToString ();
                            assembly.Add (aArch32BigEndianAssembly);
                            break;
                        case ArchSelection.Thumb:
                            string thumbAssembly = ThumbJson.FromJson (result).Asm.Thumb[1].ToString ();
                            assembly.Add (thumbAssembly);
                            break;
                        case ArchSelection.ThumbBigEndian:
                            string thumbBigEndianAssembly = ThumbBigEndianJson.FromJson (result).Asm.ThumbBigEndian[1].ToString ();
                            assembly.Add (thumbBigEndianAssembly);
                            break;
                        default:
                            return null;
                    }
                }

                return assembly.ToArray ();
            }
        }
        #endregion

        #region Asynchronous Methods
        /// <summary>
        /// Asynchronously disassembles the specified hex code <paramref name="hex"/> and returns the result, this method does not block the calling thread.
        /// </summary>
        /// <returns>The resulting assembly code from the disassembled hex code.</returns>
        /// <example>
        /// <code>
        /// string assembly = await Disassembler.DisassembleAsync ("00008052");
        /// Console.WriteLine (assembly);
        /// </code>
        /// </example>
        /// <exception cref ="System.InvalidOperationException">
        /// Thrown when trying to convert the ARM64 big-endian hex code to assembly code.
        /// </exception>
        /// <exception cref="System.Net.WebException">
        /// Thrown when the hex code is invalid.
        /// </exception>
        /// See <see cref="Disassembler.MultiDisassembleAsync(string[], ArchSelection, int?)"/> to disassemble an array of elements of hex code asynchronously.
        /// <param name="hex">The hex code string to disassemble.</param>
        /// <param name="archSelection">The architecture that the hex code <paramref name="hex"/> corresponds to.</param>
        /// <param name="offset">The offset integer that the hex code <paramref name="hex"/> is shifted by.</param>
        public static async Task<string> DisassembleAsync (string hex, ArchSelection archSelection = ArchSelection.AArch64, int? offset = null) {
            if (archSelection == ArchSelection.AArch64BigEndian) {
                throw new InvalidOperationException ("Cannot convert ARM64 big-endian hex code to assembly code");
            } else {
                var webClient = new WebClient ();
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

                string json = $"{{\"hex\":\"{hex}\",\"offset\":\"0x{offset ?? 0}\",\"arch\":\"{ArchToString(archSelection)}\"}}";
                string result = await webClient.UploadStringTaskAsync ("https://armconverter.com/api/convert", json);

                switch (archSelection) {
                    case ArchSelection.AArch64:
                        var aArch64Json = await AArch64Json.FromJsonAsync (result);
                        string aArch64Assembly = aArch64Json.Asm.AArch64[1].ToString ();
                        return aArch64Assembly;
                    case ArchSelection.AArch32:
                        var aArch32Json = await AArch32Json.FromJsonAsync (result);
                        string aArch32Assembly = aArch32Json.Asm.AArch32[1].ToString ();
                        return aArch32Assembly;
                    case ArchSelection.AArch32BigEndian:
                        var aArch32BigEndianJson = await AArch32BigEndianJson.FromJsonAsync (result);
                        string aArch32BigEndianAssembly = aArch32BigEndianJson.Asm.AArch32BigEndian[1].ToString ();
                        return aArch32BigEndianAssembly;
                    case ArchSelection.Thumb:
                        var thumbJson = await ThumbJson.FromJsonAsync (result);
                        string thumbAssembly = thumbJson.Asm.Thumb[1].ToString ();
                        return thumbAssembly;
                    case ArchSelection.ThumbBigEndian:
                        var thumbBigEndianJson = await ThumbBigEndianJson.FromJsonAsync (result);
                        string thumbBigEndianAssembly = thumbBigEndianJson.Asm.ThumbBigEndian[1].ToString ();
                        return thumbBigEndianAssembly;
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// Asynchronously disassembles the specified hex code array <paramref name="hex"/> and returns the result, this method does not block the calling thread.
        /// </summary>
        /// <returns>The resulting assembly code array from the disassembled hex code.</returns>
        /// <example>
        /// <code>
        /// string[] hex = { "00008052", "C0035FD6", "230040B9", "631C0012", "7F840171", "E1000054", "21010010", "FF8301D1", "FD2FFF17" };
        /// string[] assembly = await Disassembler.MultiDisassembleAsync (hex);
        /// Console.WriteLine (string.Join ('\n', assembly));
        /// </code>
        /// </example>
        /// <exception cref ="System.InvalidOperationException">
        /// Thrown when trying to convert one of the ARM64 big-endian hex code elements to assembly code.
        /// </exception>
        /// <exception cref="System.Net.WebException">
        /// Thrown when one of the elements of the hex code is invalid.
        /// </exception>
        /// See <see cref="Disassembler.DisassembleAsync(string, ArchSelection, int?)"/> to disassemble a single line of hex code asynchronously.
        /// <param name="hex">The hex code string array to disassemble.</param>
        /// <param name="archSelection">The architecture that the hex code <paramref name="hex"/> corresponds to.</param>
        /// <param name="offset">The offset integer that the hex code <paramref name="hex"/> is shifted by.</param>
        public static async Task<string[]> MultiDisassembleAsync (string[] hex, ArchSelection archSelection = ArchSelection.AArch64, int? offset = null) {
            if (archSelection == ArchSelection.AArch64BigEndian) {
                throw new InvalidOperationException ("Cannot convert ARM64 big-endian hex code to assembly code");
            } else {
                var assembly = new List<string> ();

                foreach (string element in hex) {
                    var webClient = new WebClient ();
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

                    string json = $"{{\"hex\":\"{element}\",\"offset\":\"0x{offset ?? 0}\",\"arch\":\"{ArchToString(archSelection)}\"}}";
                    string result = await webClient.UploadStringTaskAsync ("https://armconverter.com/api/convert", json);

                    switch (archSelection) {
                        case ArchSelection.AArch64:
                            var aArch64Json = await AArch64Json.FromJsonAsync (result);
                            string aArch64Assembly = aArch64Json.Asm.AArch64[1].ToString ();
                            assembly.Add (aArch64Assembly);
                            break;
                        case ArchSelection.AArch32:
                            var aArch32Json = await AArch32Json.FromJsonAsync (result);
                            string aArch32Assembly = aArch32Json.Asm.AArch32[1].ToString ();
                            assembly.Add (aArch32Assembly);
                            break;
                        case ArchSelection.AArch32BigEndian:
                            var aArch32BigEndianJson = await AArch32BigEndianJson.FromJsonAsync (result);
                            string aArch32BigEndianAssembly = aArch32BigEndianJson.Asm.AArch32BigEndian[1].ToString ();
                            assembly.Add (aArch32BigEndianAssembly);
                            break;
                        case ArchSelection.Thumb:
                            var thumbJson = await ThumbJson.FromJsonAsync (result);
                            string thumbAssembly = thumbJson.Asm.Thumb[1].ToString ();
                            assembly.Add (thumbAssembly);
                            break;
                        case ArchSelection.ThumbBigEndian:
                            var thumbBigEndianJson = await ThumbBigEndianJson.FromJsonAsync (result);
                            string thumbBigEndianAssembly = thumbBigEndianJson.Asm.ThumbBigEndian[1].ToString ();
                            assembly.Add (thumbBigEndianAssembly);
                            break;
                        default:
                            return null;
                    }
                }

                return assembly.ToArray ();
            }
        }
        #endregion
    }
}