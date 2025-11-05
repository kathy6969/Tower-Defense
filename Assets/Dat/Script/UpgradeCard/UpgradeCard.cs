using UnityEngine;
[CreateAssetMenu(fileName = "UpgradeCard", menuName = "Tower/UpgradeCard")]
public class UpgradeCard : ScriptableObject
{
    public string UpgradeName;
    [TextArea]public string UpgradeDescription;
    public Sprite UpgradeImage;
}
