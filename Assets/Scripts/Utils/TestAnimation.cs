using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Tweens;

public class TestAnimation : MonoBehaviour
{
    private Vector2 rect;
    private void Start()
    {
        StartCoroutine(ExampleCoroutine());

        //AnimationHelper.CardFight(gameObject.transform, new Vector2(600, 600), 2);
        //AnimationHelper.CardFight(gameObject.transform, startPosition, 1);
    }

    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(1);
        rect = GetComponent<RectTransform>().transform.position;
    }

    public void StartFight()
    {
        AnimationHelper.Fight(gameObject.transform, new Vector2(600, 600), 2);
    }

    public void StopFight()
    {
        AnimationHelper.Fight(gameObject.transform, rect, 1);
    }

}
