using System.Collections.Generic;

namespace ArmConverter {
    public static class Utilities {
        public enum ArchSelection {
            AArch64,
            AArch32,
            Thumb
        }

        internal static string ArchToString (ArchSelection archSelection) {
            switch (archSelection) {
                case ArchSelection.AArch64:
                    return "arm64";
                case ArchSelection.AArch32:
                    return "arm";
                case ArchSelection.Thumb:
                    return "thumb";
                default:
                    return null;
            }
        }

        internal static IEnumerable<string> ExceptionMessageList = new List<string> () { "Invalid mnemonic", "Invalid operand", "Unknown token in expression", "Unknown error", "Unexpected token at start of statement", "Literal value out of range for directive", "Unexpected token in directive" };
    }
}
