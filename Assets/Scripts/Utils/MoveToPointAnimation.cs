using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Tweens;

/// <summary>
/// Move children of {cardContainer} to point in {timeToSqwash} and ba
/// </summary>
public class MoveToPointAnimation : MonoBehaviour
{
    [SerializeField] private int timeToSqwash;
    [SerializeField] private int timeToShow;
    [SerializeField] private Transform[] targetStartPositions;
    [SerializeField] private Transform point;

    [SerializeField] private Transform cardContainer;
    
    // private IEnumerator Start()
    // {
    //     yield return new WaitForEndOfFrame();
    //     yield return new WaitForEndOfFrame();
    //     var children = cardContainer.GetComponentsInChildren<Transform>();
    //     
    //     StartCoroutine(MoveTargets(children));
    // }

    IEnumerator MoveTargets (Transform[] targets)
    {
        yield return new WaitForSeconds(timeToSqwash);
        AnimationHelper.MoveCardsOnFight(targets, point.position);
        yield return new WaitForSeconds(timeToShow);
        for (int i = 0; i < targets.Length; i++)
        {
            Debug.Log(targetStartPositions[i].position);
            AnimationHelper.Move(targets[i], targetStartPositions[i].position);
        }      
    }
}
