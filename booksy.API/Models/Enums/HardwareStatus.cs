using System.Text.Json.Serialization;

namespace booksy.API.Models.Enums
{
    public enum HardwareStatus
    {
        Available = 0,
        [JsonStringEnumMemberName("In Use")]
        InUse = 1,
        Repair = 2,
        Unknown = 3
    }
}
