using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Entities.Card;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Views.Cards;

namespace Views.WithoutPresenter
{
    public class ShowdownData
    {
        public string YourNickname;
        public string OpponentNickname;
        public Sprite MyCardSprite;
        public Sprite OpponentCardSprite;
        public CardStatData MyCardStatData;
        public CardStatData OpponentCardStatData;
        public CardStatsType StatType;
        public int MyAddedBoosterValue;
        public int OpponentAddedBoosterValue;
        public StatBoosterViewData MyBoosterData;
        public StatBoosterViewData OpponentBoosterData;

        public ShowdownData(string yourNickname, string opponentNickname, Sprite myCardSprite,
            Sprite opponentCardSprite, CardStatData myStatData, CardStatData opponentStatData,
            StatBoosterViewData myBoosterData, StatBoosterViewData opponentBoosterData)
        {
            YourNickname = yourNickname;
            OpponentNickname = opponentNickname;
            MyCardSprite = myCardSprite;
            OpponentCardSprite = opponentCardSprite;
            StatType = opponentStatData.StatType;
            MyBoosterData = myBoosterData;
            OpponentBoosterData = opponentBoosterData;
            MyCardStatData = myStatData;
            OpponentCardStatData = opponentStatData;

            MyAddedBoosterValue = MyBoosterData?.Value ?? 0;
            OpponentAddedBoosterValue = OpponentBoosterData?.Value ?? 0;
        }
    }

    public class ShowdownView : MonoBehaviour
    {
        [SerializeField] private TMP_Text playedStatText;

        [SerializeField] private float cardRotationTime = 0.4f;
        [SerializeField] private float waitTimeBeforeRotate = 2f;
        [SerializeField] private Transform myFighterTransform;
        [SerializeField] private Image myCardImage;
        [SerializeField] private TMP_Text myNickText;
        [SerializeField] private TMP_Text myBoosterDescription;
        [SerializeField] private CanvasGroup myFaceCanvasGroup;
        [SerializeField] private CanvasGroup myBackCanvasGroup;
        [SerializeField] private GameObject myCrown;
        [SerializeField] private GameObject myBooster;
        [SerializeField] private Image myBoosterImage;
        [SerializeField] private StatView myStatView;

        [SerializeField] private Transform opponentFighterTransform;
        [SerializeField] private Image opponentCardImage;
        [SerializeField] private TMP_Text opponentNickText;
        [SerializeField] private TMP_Text opponentBoosterDescription;
        [SerializeField] private CanvasGroup opponentFaceCanvasGroup;
        [SerializeField] private CanvasGroup opponentBackCanvasGroup;
        [SerializeField] private GameObject opponentCrown;
        [SerializeField] private GameObject opponentBooster;
        [SerializeField] private Image opponentBoosterImage;
        [SerializeField] private StatView opponentStatView;

        [Header("Colors")]
        [SerializeField] private Color normalColor;
        [SerializeField] private Color winColor;
        [SerializeField] private Color loseColor;
        [SerializeField] private Color fightColor;
        [SerializeField] private Image playerFaceFrame;
        [SerializeField] private Image playerCardFrame;
        [SerializeField] private Image playerBoosterFrame;
        [SerializeField] private Image opponentFaceFrame;
        [SerializeField] private Image opponentCardFrame;
        [SerializeField] private Image opponentBoosterFrame;

        private TweenerCore<Quaternion, Vector3, QuaternionOptions> _myTween;
        private TweenerCore<Quaternion, Vector3, QuaternionOptions> _opponentTween;
        private int myAddedValueFromBooster, opponentAddedValueFromBooster;
        private bool _isMyWin;
        private float colorDuration = .4f;

        public void SetData(ShowdownData showdownData, bool isMyWin)
        {
            _isMyWin = isMyWin;
            myAddedValueFromBooster = showdownData.MyAddedBoosterValue;
            opponentAddedValueFromBooster = showdownData.OpponentAddedBoosterValue;

            myCardImage.sprite = showdownData.MyCardSprite;
            opponentCardImage.sprite = showdownData.OpponentCardSprite;

            myNickText.text = showdownData.YourNickname;
            opponentNickText.text = showdownData.OpponentNickname;


            myStatView.SetData(showdownData.MyCardStatData);
            myStatView.SetInteractable(false);
            opponentStatView.SetData(showdownData.OpponentCardStatData);
            opponentStatView.SetInteractable(false);
            BoostStatIfNeeds();
            
            myBooster.SetActive(myAddedValueFromBooster > 0);
            opponentBooster.SetActive(opponentAddedValueFromBooster > 0);

            var mySign = myAddedValueFromBooster > 0 ? "+" : "";
            var opponentSign = opponentAddedValueFromBooster > 0 ? "+" : "";
            myBoosterDescription.text = $"{mySign}{myAddedValueFromBooster} {showdownData.StatType}";
            opponentBoosterDescription.text = $"{opponentSign}{opponentAddedValueFromBooster} {showdownData.StatType}";

            myBoosterImage.sprite = showdownData.MyBoosterData.SpriteValue;
            opponentBoosterImage.sprite = showdownData.OpponentBoosterData.SpriteValue;

            playedStatText.text = showdownData.StatType.ToString();

            BeginColoring();
        }

        private void BeginColoring()
        {
            playerFaceFrame.DOColor(fightColor , colorDuration);
            playerBoosterFrame.DOColor(fightColor, colorDuration);
            opponentFaceFrame.DOColor(fightColor, colorDuration);
            opponentBoosterFrame.DOColor(fightColor, colorDuration);
        }

        public void Hide()
        {
            ShowCardsFaces();
            if (_opponentTween == null || _myTween == null)
                return;

            _opponentTween.onComplete = null;
            _myTween.onComplete = null;
            _opponentTween.Kill();
            _myTween.Kill();
        }

        public void Show(Action doNext = null)
        {
            myCrown.SetActive(false);
            opponentCrown.SetActive(false);
            StartCoroutine(RotateCard(doNext));
        }

        private IEnumerator RotateCard(Action callback = null)
        {
            yield return new WaitForSeconds(waitTimeBeforeRotate);

            _opponentTween = opponentFighterTransform.DOLocalRotate(new Vector3(0, 90, 0), cardRotationTime);
            _opponentTween.onComplete = HideOpponentCardsFace;
            _myTween = myFighterTransform.DOLocalRotate(new Vector3(0, 90, 0), cardRotationTime, RotateMode.Fast);
            _myTween.onComplete = HideMyCardsFace;

            callback?.Invoke();
            yield return new WaitForSeconds(cardRotationTime);

            AnimateIfBoosterUsed();
            myCrown.SetActive(_isMyWin);
            opponentCrown.SetActive(!_isMyWin);

            if (_isMyWin)
            {
                playerCardFrame.DOColor(winColor, colorDuration);
                playerBoosterFrame.DOColor(winColor, colorDuration);
                opponentCardFrame.DOColor(loseColor, colorDuration);
            }
            else
            {
                playerCardFrame.DOColor(loseColor, colorDuration);
                playerBoosterFrame.DOColor(loseColor, colorDuration);
                opponentCardFrame.DOColor(winColor, colorDuration);
            }
        }

        private void AnimateIfBoosterUsed()
        {
            if (myAddedValueFromBooster > 0)
            {
                DOTween.Sequence()
                    .Append(myStatView.transform.DOScale(Vector3.one * 1.2f, cardRotationTime / 4))
                    .Append(myStatView.transform.DOScale(Vector3.one, cardRotationTime / 4));
            }

            if (opponentAddedValueFromBooster > 0)
            {
                DOTween.Sequence()
                    .Append(opponentStatView.transform.DOScale(Vector3.one * 1.2f, cardRotationTime / 4))
                    .Append(opponentStatView.transform.DOScale(Vector3.one, cardRotationTime / 4));
            }
        }

        private void HideMyCardsFace()
        {
            myFaceCanvasGroup.alpha = 0;
            myBackCanvasGroup.alpha = 1;
            myFighterTransform.DOLocalRotate(Vector3.zero, cardRotationTime);
        }

        private void HideOpponentCardsFace()
        {
            opponentFaceCanvasGroup.alpha = 0;
            opponentBackCanvasGroup.alpha = 1;
            opponentFighterTransform.DOLocalRotate(Vector3.zero, cardRotationTime);
        }

        private void ShowCardsFaces()
        {
            myFaceCanvasGroup.alpha = 1;
            myBackCanvasGroup.alpha = 0;

            opponentFaceCanvasGroup.alpha = 1;
            opponentBackCanvasGroup.alpha = 0;
        }

        private void BoostStatIfNeeds()
        {
            if (myAddedValueFromBooster != 0)
            {
                myStatView.BoostStat(myAddedValueFromBooster);
            }
            if (opponentAddedValueFromBooster != 0)
            {
                myStatView.BoostStat(myAddedValueFromBooster);
            }
        }
    }
}