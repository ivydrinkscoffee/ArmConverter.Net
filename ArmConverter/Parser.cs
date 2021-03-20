using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArmConverter {
    internal static class Parser {
        internal static class Assemble {
            #region AArch64
            internal partial class AArch64Json {
                [JsonPropertyName ("hex")]
                public AArch64Hex Hex { get; set; }

                [JsonPropertyName ("counter")]
                public long Counter { get; set; }
            }

            internal partial class AArch64Hex {
                [JsonPropertyName ("arm64")]
                public object[] AArch64 { get; set; }
            }

            internal partial class AArch64Json {
                public static AArch64Json FromJson (string json) => JsonSerializer.Deserialize<AArch64Json> (json);
            }
            #endregion

            #region AArch32
            internal partial class AArch32Json {
                [JsonPropertyName ("hex")]
                public AArch32Hex Hex { get; set; }

                [JsonPropertyName ("counter")]
                public long Counter { get; set; }
            }

            internal partial class AArch32Hex {
                [JsonPropertyName ("arm")]
                public object[] AArch32 { get; set; }
            }

            internal partial class AArch32Json {
                public static AArch32Json FromJson (string json) => JsonSerializer.Deserialize<AArch32Json> (json);
            }
            #endregion  

            #region Thumb
            internal partial class ThumbJson {
                [JsonPropertyName ("hex")]
                public ThumbHex Hex { get; set; }

                [JsonPropertyName ("counter")]
                public long Counter { get; set; }
            }

            internal partial class ThumbHex {
                [JsonPropertyName ("thumb")]
                public object[] Thumb { get; set; }
            }

            internal partial class ThumbJson {
                public static ThumbJson FromJson (string json) => JsonSerializer.Deserialize<ThumbJson> (json);
            }
            #endregion
        }

        internal static class Disassemble {
            #region AArch64
            internal partial class AArch64Json {
                [JsonPropertyName ("asm")]
                public AArch64Asm Asm { get; set; }

                [JsonPropertyName ("counter")]
                public long Counter { get; set; }
            }

            internal partial class AArch64Asm {
                [JsonPropertyName ("arm64")]
                public object[] AArch64 { get; set; }
            }

            internal partial class AArch64Json {
                public static AArch64Json FromJson (string json) => JsonSerializer.Deserialize<AArch64Json> (json);
            }
            #endregion

            #region AArch32
            internal partial class AArch32Json {
                [JsonPropertyName ("asm")]
                public AArch32Asm Asm { get; set; }

                [JsonPropertyName ("counter")]
                public long Counter { get; set; }
            }

            internal partial class AArch32Asm {
                [JsonPropertyName ("arm")]
                public object[] AArch32 { get; set; }
            }

            internal partial class AArch32Json {
                public static AArch32Json FromJson (string json) => JsonSerializer.Deserialize<AArch32Json> (json);
            }
            #endregion

            #region Thumb
            internal partial class ThumbJson {
                [JsonPropertyName ("asm")]
                public ThumbAsm Asm { get; set; }

                [JsonPropertyName ("counter")]
                public long Counter { get; set; }
            }

            internal partial class ThumbAsm {
                [JsonPropertyName ("thumb")]
                public object[] Thumb { get; set; }
            }

            internal partial class ThumbJson {
                public static ThumbJson FromJson (string json) => JsonSerializer.Deserialize<ThumbJson> (json);
            }
            #endregion
        }
    }
}