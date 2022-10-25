
using UnityEngine;

public class OpponentCardView : MonoBehaviour
{
    [SerializeField] private GameObject usedImageObj;

    public bool IsUsed => _isUsed;
    private bool _isUsed;
    
    public void UseCard(bool value)
    {
        _isUsed = value;
        usedImageObj.SetActive(value);
    }
    
}
