using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] Image img;

    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    void Start()
    {
        
    }

    public void RefreshLife(float current, float max)
    {
        img.fillAmount = current / max;

        txt.text = current.ToString() + " / " + max.ToString();
    }

    public void ShowStateWinScreen(bool win)
    {

    }
}
