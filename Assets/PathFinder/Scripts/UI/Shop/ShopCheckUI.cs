using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCheckUI : MonoBehaviour
{
    [Header("Check Text")]
    public TextMeshProUGUI checkText;

    [Header("Input")]
    public TMP_InputField inputField;
    public TextMeshProUGUI inputText;
    [Header("Button")]
    public Button yes;
    public Button no;

    private void Start()
    {
        yes.onClick.RemoveAllListeners();
        no.onClick.RemoveAllListeners();
        yes.onClick.AddListener(() =>
        {
            //판매로직
            //ShopManager.instance.Sell((int)inputText) 이렇게?
        });
        no.onClick.AddListener(() =>
        {
            inputField.gameObject.SetActive(false);
            gameObject.SetActive(false);
        });
    }

    public void RefreshBuyUI()
    {

    }
    public void RefreshSellUI(int count =1)
    {
        if(count > 1)
        {
            inputField.gameObject.SetActive(true);
        }
        checkText.text = "Sell?";
    }
}
