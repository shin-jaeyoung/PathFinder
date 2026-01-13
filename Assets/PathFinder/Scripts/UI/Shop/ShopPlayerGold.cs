using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopPlayerGold : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    public void RefreshUI(int value)
    {
        goldText.text = value.ToString();
    }
}
