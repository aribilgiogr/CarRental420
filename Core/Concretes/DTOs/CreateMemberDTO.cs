namespace Core.Concretes.DTOs.Member
{
    public class CreateMemberDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? PhoneNumber2 { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? DriverLicenseNumber { get; set; }
        public DateTime? DriverLicenseExpiryDate { get; set; }
        public string? NationalIdNumber { get; set; }
        public string Password { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}