using TMPro;
using UnityEngine;

public class TestMetamask : MonoBehaviour
{
    public static TestMetamask Instance;

    public TMP_Text Text;

    private void Awake()
    {
        Instance = this;
    }

    public void SetSignature(string value)
    {
        Text.text = value;
    }
}
