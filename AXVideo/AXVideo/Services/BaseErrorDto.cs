namespace AXVideo.Services
{
    public class BaseErrorDto
    {
        public bool IsSucceed { get; set; }
        public bool SignOut { get; set; }
        public bool TenancyRequired { get; set; }
        public bool QuizFailed { get; set; }
        public int AuthType { get; set; }
        public bool ValidEmail { get; set; }
        public bool ValidPhone { get; set; }
        public bool EmailInDomain { get; set; }
        public bool AuthRequired { get; set; }
        public string ErrorMessage { get; set; }
    }
}
