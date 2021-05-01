using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
                public static async Task<AArch64Json> FromJsonAsync (string json) => await JsonSerializer.DeserializeAsync<AArch64Json> (new MemoryStream (Encoding.UTF8.GetBytes (json)));
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
                public static async Task<AArch32Json> FromJsonAsync (string json) => await JsonSerializer.DeserializeAsync<AArch32Json> (new MemoryStream (Encoding.UTF8.GetBytes (json)));
            }
            #endregion

            #region AArch32BigEndian
            internal partial class AArch32BigEndianJson {
                [JsonPropertyName ("hex")]
                public AArch32BigEndianHex Hex { get; set; }

                [JsonPropertyName ("counter")]
                public long Counter { get; set; }
            }

            internal partial class AArch32BigEndianHex {
                [JsonPropertyName ("armbe")]
                public object[] AArch32BigEndian { get; set; }
            }

            internal partial class AArch32BigEndianJson {
                public static AArch32BigEndianJson FromJson (string json) => JsonSerializer.Deserialize<AArch32BigEndianJson> (json);
                public static async Task<AArch32BigEndianJson> FromJsonAsync (string json) => await JsonSerializer.DeserializeAsync<AArch32BigEndianJson> (new MemoryStream (Encoding.UTF8.GetBytes (json)));
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
                public static async Task<ThumbJson> FromJsonAsync (string json) => await JsonSerializer.DeserializeAsync<ThumbJson> (new MemoryStream (Encoding.UTF8.GetBytes (json)));
            }
            #endregion

            #region ThumbBigEndian
            internal partial class ThumbBigEndianJson {
                [JsonPropertyName ("hex")]
                public ThumbBigEndianHex Hex { get; set; }

                [JsonPropertyName ("counter")]
                public long Counter { get; set; }
            }

            internal partial class ThumbBigEndianHex {
                [JsonPropertyName ("thumbbe")]
                public object[] ThumbBigEndian { get; set; }
            }

            internal partial class ThumbBigEndianJson {
                public static ThumbBigEndianJson FromJson (string json) => JsonSerializer.Deserialize<ThumbBigEndianJson> (json);
                public static async Task<ThumbBigEndianJson> FromJsonAsync (string json) => await JsonSerializer.DeserializeAsync<ThumbBigEndianJson> (new MemoryStream (Encoding.UTF8.GetBytes (json)));
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
                public static async Task<AArch64Json> FromJsonAsync (string json) => await JsonSerializer.DeserializeAsync<AArch64Json> (new MemoryStream (Encoding.UTF8.GetBytes (json)));
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
                public static async Task<AArch32Json> FromJsonAsync (string json) => await JsonSerializer.DeserializeAsync<AArch32Json> (new MemoryStream (Encoding.UTF8.GetBytes (json)));
            }
            #endregion

            #region AArch32BigEndian
            internal partial class AArch32BigEndianJson {
                [JsonPropertyName ("asm")]
                public AArch32BigEndianAsm Asm { get; set; }

                [JsonPropertyName ("counter")]
                public long Counter { get; set; }
            }

            internal partial class AArch32BigEndianAsm {
                [JsonPropertyName ("armbe")]
                public object[] AArch32BigEndian { get; set; }
            }

            internal partial class AArch32BigEndianJson {
                public static AArch32BigEndianJson FromJson (string json) => JsonSerializer.Deserialize<AArch32BigEndianJson> (json);
                public static async Task<AArch32BigEndianJson> FromJsonAsync (string json) => await JsonSerializer.DeserializeAsync<AArch32BigEndianJson> (new MemoryStream (Encoding.UTF8.GetBytes (json)));
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
                public static async Task<ThumbJson> FromJsonAsync (string json) => await JsonSerializer.DeserializeAsync<ThumbJson> (new MemoryStream (Encoding.UTF8.GetBytes (json)));
            }
            #endregion

            #region ThumbBigEndian
            internal partial class ThumbBigEndianJson {
                [JsonPropertyName ("asm")]
                public ThumbBigEndianAsm Asm { get; set; }

                [JsonPropertyName ("counter")]
                public long Counter { get; set; }
            }

            internal partial class ThumbBigEndianAsm {
                [JsonPropertyName ("thumbbe")]
                public object[] ThumbBigEndian { get; set; }
            }

            internal partial class ThumbBigEndianJson {
                public static ThumbBigEndianJson FromJson (string json) => JsonSerializer.Deserialize<ThumbBigEndianJson> (json);
                public static async Task<ThumbBigEndianJson> FromJsonAsync (string json) => await JsonSerializer.DeserializeAsync<ThumbBigEndianJson> (new MemoryStream (Encoding.UTF8.GetBytes (json)));
            }
            #endregion
        }
    }
}