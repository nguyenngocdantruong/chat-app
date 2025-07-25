namespace ChatApp.Domain.Exceptions.Auth
{
    public class AccountLockedException: AppException
    {
        public override int StatusCode => 423; // Locked
        public override string ErrorCode => "account_locked";
        public AccountLockedException() : base(423, "The account is locked due to too many failed login attempts.")
        {
        }
        public AccountLockedException(string message) : base(423, message)
        {
        }
    }
}
