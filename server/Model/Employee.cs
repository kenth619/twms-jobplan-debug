using System.Text.Json.Serialization;
using TWMSServer.Model.Enum;

namespace TWMSServer.Model
{
    public class Employee
    {
        public string Username { get; set; } = string.Empty;
        public string EmployeeNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string EmployeeStatus { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public int Rank { get; set; }
        public int? Extension { get; set; }
        public string JobName { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";
        public string Label => $"{FirstName} {LastName} ({EmployeeNumber}) - Dept: {Department}";

        [JsonIgnore]
        public byte[] Signature { get; set; } = [];
        [JsonIgnore]
        public string SignatureType { get; set; } = string.Empty;
        [JsonIgnore]
        public string EncodedSignature => DatabaseImage.EncodedImage(Signature, SignatureType);
        [JsonIgnore]
        public bool HasSignature => !string.IsNullOrEmpty(EncodedSignature);
    }
}
