using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static ArmConverter.Parser.Assemble;
using static ArmConverter.Utilities;

namespace ArmConverter {
    public static class Assembler {
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