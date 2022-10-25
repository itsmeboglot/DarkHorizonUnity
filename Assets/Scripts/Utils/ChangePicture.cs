using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePicture : MonoBehaviour
{
    [SerializeField] private GameObject[] pictures;
    private int indexs;

    public void Change()
    {
        if (indexs < pictures.Length-1)
        {
            indexs += 1;
            pictures[indexs - 1].SetActive(false);
            pictures[indexs].SetActive(true);
        }
        else
        {
            pictures[indexs].SetActive(false);
            indexs = 0; 
            pictures[indexs].SetActive(true);
        }
    }
}
