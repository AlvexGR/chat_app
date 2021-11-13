namespace ChatApp.Utilities.Constants
{
    public class EmailTemplates
    {
        public const string ConfirmAccountTitle = "Welcome to my ChatApp. Please confirm your account!";

        public const string ConfirmAccountBody = @"
            <p>Hi <strong>#name#</strong>,</p>
            <p>Welcome to my ChatApp, <br /> please click on the following link to confirm your account: #link#.</p>
            <p>Best regards,</p>
            <i>Nhan Nguyen</i>
        ";

        public const string ForgotPasswordTitle = "Forgot password";

        public const string ForgotPasswordBody = @"
            <p>Hi <strong>#name#</strong>,</p>
            <p>Your new password is: <strong>#password#</strong></p>
            <p>Best regards,</p>
            <i>Nhan Nguyen</i>
        ";
    }
}
