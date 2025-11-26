using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class CardUpgradeUI : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform CardUpgradePanel;
    public Transform cardContainer;
    public GameObject cardPrefab;

    [Header("Database")]
    public UpgradeCardDatabase cardDatabase;
    private UpgradeCard[] availableCards;

    [Header("Tween Settings")]
    private float duration = 1f;
    private float screenHeight = 1080f;
    public Ease easeType = Ease.OutSine;

    [Header("Else Settings")]
    public GameObject Tower;
    public UpgradeCardManager upgradeCardManager;
    private bool isTransitioning = false;
    private List<GameObject> activeCards = new List<GameObject>();
    public Button rerollButton;
    private bool hasRerolled = false;

    void Start()
    {
        // Load all cards from the database
        availableCards = cardDatabase.allCards.ToArray();
    }

    public void ShowCardUpgradePanel()
    {
        if (isTransitioning) return;
        isTransitioning = true;
        Time.timeScale = 0f;

        hasRerolled = false;
        rerollButton.interactable = true;

        AddCardToContainer();

        CardUpgradePanel.anchorMin = new Vector2(0.5f, 0.5f);
        CardUpgradePanel.anchorMax = new Vector2(0.5f, 0.5f);
        CardUpgradePanel.pivot = new Vector2(0.5f, 0.5f);
        CardUpgradePanel.anchoredPosition = new Vector2(0, screenHeight);

        Sequence seq = DOTween.Sequence().SetUpdate(true);
        seq.Join(CardUpgradePanel.DOAnchorPosY(0, duration).SetEase(easeType));
        seq.OnComplete(() => isTransitioning = false);
    }

    public void HideCardUpgradePanel()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        Sequence seq = DOTween.Sequence().SetUpdate(true);
        seq.Join(CardUpgradePanel.DOAnchorPosY(screenHeight, duration).SetEase(easeType));
        seq.OnComplete(() =>
        {
            isTransitioning = false;
            Time.timeScale = 1f;
            RemoveCard();
        });
    }

    public void AddCardToContainer(List<UpgradeCard> excludeCards = null)
    {
        excludeCards ??= new List<UpgradeCard>();
        List<string> ownedIDs = upgradeCardManager.ownedCardIDs;

        foreach (Transform child in cardContainer)
            Destroy(child.gameObject);

        List<UpgradeCard> selectedCards = new List<UpgradeCard>();
        int safety = 0;

        while (selectedCards.Count < 3 && safety < 100)
        {
            safety++;

            var card = availableCards[Random.Range(0, availableCards.Length)];

            if (selectedCards.Contains(card)) continue;
            if (excludeCards.Contains(card)) continue;
            if (upgradeCardManager.HasUpgrade(card)) continue;

            if (!IsCardUnlocked(card)) continue;

            selectedCards.Add(card);
        }

        foreach (UpgradeCard card in selectedCards)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);
            activeCards.Add(cardObj);

            CardSelec cardSelec = cardObj.GetComponent<CardSelec>();
            cardSelec.Setup(card, Tower);

            cardObj.transform.Find("UpgradeName").GetComponent<TextMeshProUGUI>().text = card.UpgradeName;
            cardObj.transform.Find("UpgradeLever").GetComponent<TextMeshProUGUI>().text = "LV." + card.UpgradeLevel;
            cardObj.transform.Find("UpgradeDescription").GetComponent<TextMeshProUGUI>().text = card.UpgradeDescription;
            cardObj.transform.Find("Image").GetComponent<Image>().sprite = card.UpgradeImage;
        }
    }

    public void RerollCards()
    {
        List<UpgradeCard> currentCards = new List<UpgradeCard>();
        foreach (Transform child in cardContainer)
        {
            string name = child.Find("UpgradeName").GetComponent<TextMeshProUGUI>().text;
            foreach (var card in availableCards)
            {
                if (card.UpgradeName == name)
                {
                    currentCards.Add(card);
                    break;
                }
            }
        }

        Sequence seq = DOTween.Sequence().SetUpdate(true);
        List<GameObject> oldCards = new List<GameObject>();

        foreach (Transform child in cardContainer)
        {
            oldCards.Add(child.gameObject);
            child.localRotation = Quaternion.identity;

            Tween t = child.DOLocalRotate(new Vector3(0, 180, 0), 0.5f, RotateMode.FastBeyond360)
                .SetEase(Ease.InBack).SetUpdate(true);

            CanvasGroup cg = child.GetComponent<CanvasGroup>();
            if (cg == null) cg = child.gameObject.AddComponent<CanvasGroup>();
            cg.DOFade(0f, 0.5f).SetEase(Ease.InOutQuad).SetUpdate(true);

            seq.Join(t);
        }

        seq.AppendCallback(() =>
        {
            AddCardToContainer(currentCards);

            foreach (Transform child in cardContainer)
            {
                if (!oldCards.Contains(child.gameObject))
                {
                    child.localRotation = Quaternion.Euler(0, 180, 0);

                    CanvasGroup cg = child.GetComponent<CanvasGroup>();
                    if (cg == null) cg = child.gameObject.AddComponent<CanvasGroup>();
                    cg.alpha = 0f;

                    child.DOLocalRotate(Vector3.zero, 0.35f, RotateMode.FastBeyond360)
                        .SetEase(Ease.OutBack).SetUpdate(true);

                    cg.DOFade(1f, 0.35f).SetEase(Ease.OutQuad).SetUpdate(true);
                }
            }

            StartCoroutine(DestroyAfterDelay(oldCards, 0.5f));
        });
    }

    private bool IsCardUnlocked(UpgradeCard card)
    {
        if (card.previousLevel == null)
            return true;

        return upgradeCardManager.HasUpgrade(card.previousLevel);
    }

    private IEnumerator DestroyAfterDelay(List<GameObject> cards, float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var card in cards)
        {
            if (card != null)
            {
                DOTween.Kill(card.transform);
                Destroy(card);
            }
        }
    }

    void RemoveCard()
    {
        foreach (Transform child in cardContainer)
            Destroy(child.gameObject);
    }
}
