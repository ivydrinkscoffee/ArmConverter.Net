using System.Collections.Generic;

namespace ArmConverter {
    /// <summary>
    /// The main <c>Utilities</c> class.
    /// Contains an enum for aiding with architecture choices.
    /// <list type="bullet">
    /// <item>
    /// <term>ArchSelection</term>
    /// <description>Main architecture enum</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>This class can handle choices for every architecture that the API offers.</para>
    /// </remarks>
    public static class Utilities {
        /// <summary>
        /// An enum for handling the architecture choice given by the main methods.
        /// </summary>
        public enum ArchSelection {
            /// <summary>
            /// Resembles the AArch64 or ARM64 architecture.
            /// </summary>
            AArch64,
            /// <summary>
            /// Resembles the AArch32 or ARM architecture.
            /// </summary>
            AArch32,
            /// <summary>
            /// Resembles the AArch32 or ARM architecture as big-endian.
            /// </summary>
            AArch32BigEndian,
            /// <summary>
            /// Resembles the Thumb, Thumb-2 or T32 architecture extension.
            /// </summary>
            Thumb,
            /// <summary>
            /// Resembles the Thumb, Thumb-2 or T32 architecture extension as big-endian.
            /// </summary>
            ThumbBigEndian
        }

        internal static string ArchToString (ArchSelection archSelection) {
            switch (archSelection) {
                case ArchSelection.AArch64:
                    return "arm64";
                case ArchSelection.AArch32:
                    return "arm";
                case ArchSelection.AArch32BigEndian:
                    return "armbe";
                case ArchSelection.Thumb:
                    return "thumb";
                case ArchSelection.ThumbBigEndian:
                    return "thumbbe";
                default:
                    return null;
            }
        }

        internal static IEnumerable<string> ExceptionMessageList = new List<string> () {
            "Invalid mnemonic",
            "Invalid operand",
            "Unknown token in expression",
            "Unknown error",
            "Unexpected token at start of statement",
            "Literal value out of range for directive",
            "Unexpected token in directive"
        };
    }
}