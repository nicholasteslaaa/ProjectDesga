using TMPro;
using UnityEngine;

public class CurrencyCounter : MonoBehaviour
{
    public TextMeshProUGUI currencyText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int[] scores = SaveSystem.loadSaveFile().levelScore;
        int totalCur = 0;

        for (int i = 0; i < scores.Length; i++)
        {
            totalCur += scores[i];
        }
        currencyText.text = $"Currency: Rp.{totalCur*100000}.00";
    }
}
