using System;
using System.Linq;

namespace UseCases.Menu
{
  public static class ProfileCreationValidator
  {
    private const string NicknameErrorMessage = "nickname can't be empty";
    private const string BioErrorMessage = "";
    private const string AvatarErrorMessage = "avatar can't be empty";


    public static void ValidateProfileProperties(string nickname, string bio, string selectedAvatar, Action<bool ,string> onSuccess)
    {
      string errorMessage = "";
      bool result = true;
      
      if (!VerifyNickname(nickname))
      {
        result = false;
        errorMessage += NicknameErrorMessage + "\n";
      }

      if (!VerifyBio(bio))
      {
        result = false;
        errorMessage += BioErrorMessage + "\n";
      }

      if (!VerifyAvatar(selectedAvatar))
      {
        result = false;
        errorMessage += AvatarErrorMessage + "\n";
      }

      onSuccess?.Invoke(result, errorMessage);
    }

    public static string RemoveWhitespaceFromValue(string value) => 
      String.Concat(value.Where(c => !Char.IsWhiteSpace(c)));
    
    private static bool VerifyNickname(string nickname) =>
      nickname.Length > 3;
    
    private static bool VerifyBio(string bio) => 
      true;

    private static bool VerifyAvatar(string avatar) =>
      !string.IsNullOrEmpty(avatar);
  }
}