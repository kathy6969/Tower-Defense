using UnityEngine;

public class CardSelec : MonoBehaviour
{
    public CardUpgradeUI cardUpgradeUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardUpgradeUI = GetComponentInParent<CardUpgradeUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClick()
    {
        cardUpgradeUI.HideCardUpgradePanel();
    }
}
