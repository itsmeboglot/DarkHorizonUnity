using Darkhorizon.Shared.Player.Dto;

namespace Entities.User
{
    public class UserProfile
    {
        public string NickName;
        public string Bio;
        public string AvatarId;
        public WalletNftCollectionDto _nftCollection;

        public bool IsNew => string.IsNullOrEmpty(NickName);

    }
}
