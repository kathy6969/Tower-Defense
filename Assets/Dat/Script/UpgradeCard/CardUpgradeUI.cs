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
    public UpgradeCard[] availableCards;

    [Header("Tween Settings")]
    private float duration = 1f;
    private float screenHeight = 1080f;
    public Ease easeType = Ease.OutSine;

    private bool isTransitioning = false;
    private List<GameObject> activeCards = new List<GameObject>();

    public Button rerollButton;
    private bool hasRerolled = false;
    public void ShowCardUpgradePanel()
    {
        if (isTransitioning) return;
        isTransitioning = true;
        Time.timeScale = 0f;

        hasRerolled = false; // reset trạng thái reroll
        rerollButton.interactable = true;  // đảm bảo nút sáng lại

        AddCardToContainer();

        // đảm bảo anchor đúng
        CardUpgradePanel.anchorMin = new Vector2(0.5f, 0.5f);
        CardUpgradePanel.anchorMax = new Vector2(0.5f, 0.5f);
        CardUpgradePanel.pivot = new Vector2(0.5f, 0.5f);

        // đặt vị trí bắt đầu ở ngoài màn hình trên
        CardUpgradePanel.anchoredPosition = new Vector2(0, screenHeight);

        // tween panel xuống
        Sequence seq = DOTween.Sequence().SetUpdate(true); // 👈 unscaled time
        seq.Join(CardUpgradePanel.DOAnchorPosY(0, duration)
            .SetEase(easeType));
        seq.OnComplete(() => isTransitioning = false);
    }

    public void HideCardUpgradePanel()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        Sequence seq = DOTween.Sequence().SetUpdate(true); // 👈 unscaled time
        seq.Join(CardUpgradePanel.DOAnchorPosY(screenHeight, duration)
            .SetEase(easeType));
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

        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        List<UpgradeCard> selectedCards = new List<UpgradeCard>();
        while (selectedCards.Count < 3)
        {
            var card = availableCards[Random.Range(0, availableCards.Length)];
            if (!selectedCards.Contains(card) && !excludeCards.Contains(card))
            {
                selectedCards.Add(card);
            }
        }

        foreach (UpgradeCard card in selectedCards)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);
            activeCards.Add(cardObj);

            cardObj.transform.Find("UpgradeName").GetComponent<TextMeshProUGUI>().text = card.UpgradeName;
            cardObj.transform.Find("UpgradeDescription").GetComponent<TextMeshProUGUI>().text = card.UpgradeDescription;
            cardObj.transform.Find("Image").GetComponent<Image>().sprite = card.UpgradeImage;
        }
    }

    public void RerollCards()
    {
        // if (hasRerolled) return;
        // hasRerolled = true;

        // rerollButton.interactable = false;

        // Lưu lại danh sách UpgradeCard đang hiển thị
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

            // Hiệu ứng xoay 180° + mờ dần
            Tween t = child.DOLocalRotate(new Vector3(0, 180, 0), 0.5f, RotateMode.FastBeyond360)
                .SetEase(Ease.InBack)
                .SetUpdate(true);

            CanvasGroup cg = child.GetComponent<CanvasGroup>();
            if (cg == null) cg = child.gameObject.AddComponent<CanvasGroup>();
            cg.DOFade(0f, 0.5f).SetEase(Ease.InOutQuad).SetUpdate(true);

            seq.Join(t);
        }

        seq.AppendCallback(() =>
        {
            // Sinh thẻ mới khác hoàn toàn với thẻ cũ
            AddCardToContainer(currentCards);

            // Cho thẻ mới xuất hiện bằng hiệu ứng xoay ngược + hiện dần
            foreach (Transform child in cardContainer)
            {
                if (!oldCards.Contains(child.gameObject))
                {
                    child.localRotation = Quaternion.Euler(0, 180, 0);

                    CanvasGroup cg = child.GetComponent<CanvasGroup>();
                    if (cg == null) cg = child.gameObject.AddComponent<CanvasGroup>();
                    cg.alpha = 0f;

                    child.DOLocalRotate(Vector3.zero, 0.35f, RotateMode.FastBeyond360)
                        .SetEase(Ease.OutBack)
                        .SetUpdate(true);

                    cg.DOFade(1f, 0.35f).SetEase(Ease.OutQuad).SetUpdate(true);
                }
            }

            // Sau 0.5 giây mới xóa hẳn thẻ cũ
            StartCoroutine(DestroyAfterDelay(oldCards, 0.5f));
        });
    }

    private IEnumerator DestroyAfterDelay(List<GameObject> cards, float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var card in cards)
        {
            if (card != null)
            {
                DOTween.Kill(card.transform); // Hủy tween liên quan object này
                Destroy(card);
            }
        }
    }


    void RemoveCard()
    {
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
