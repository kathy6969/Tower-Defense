using UnityEngine;

public class GridSwitcher : MonoBehaviour
{
    public GameObject[] grids;   // Grid1, Grid2, Grid3
    private int currentIndex = 0;

    void Start()
    {
        // Ban đầu chỉ bật Grid đầu tiên
        for (int i = 0; i < grids.Length; i++)
            grids[i].SetActive(i == 0);
    }

    public void NextPage()
    {
        int next = currentIndex + 1;
        if (next >= grids.Length) return;

        SwitchTo(next);
    }

    public void PrevPage()
    {
        int prev = currentIndex - 1;
        if (prev < 0) return;

        SwitchTo(prev);
    }

    void SwitchTo(int newIndex)
    {
        grids[currentIndex].SetActive(false);
        grids[newIndex].SetActive(true);

        currentIndex = newIndex;
    }
}
