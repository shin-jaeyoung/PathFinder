using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopExplainUI : MonoBehaviour
{
    public TextMeshProUGUI explainText;

    public void RefreshUI(string text)
    {
        explainText.text = text;
    }
}
