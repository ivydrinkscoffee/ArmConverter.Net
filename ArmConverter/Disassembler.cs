using System.Collections.Generic;
using System.Net;
using static ArmConverter.Parser.Disassemble;
using static ArmConverter.Utilities;

namespace ArmConverter {
    public static class Disassembler {
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

        public static string[] MultiDisassemble (string[] hex, ArchSelection archSelection = ArchSelection.AArch64, int? offset = null) {
            var assembly = new List<string> ();

            foreach (string line in hex) {
                var webClient = new WebClient ();
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

                string json = $"{{\"hex\":\"{line}\",\"offset\":\"0x{offset ?? 0}\",\"arch\":\"{ArchToString(archSelection)}\"}}";
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