namespace Whimsy.Shared.Identity
{
    public static class IdentityErrorCodes
    {
        private const int Start = 1100;

        public const int AccountNotExists = Start + 1;
        public const int SocialApiError = Start + 2;
    }
}