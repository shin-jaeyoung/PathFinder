using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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

    private int count;
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
            string input = inputText.text;
            if(inputField.gameObject.activeSelf)
            {
                if (int.TryParse(input, out count))
                {
                    Debug.Log("저장 성공: " + count);
                    ShopManager.instance.count = count;
                }
                else
                {
                        Debug.LogError("숫자만 입력해주세요!");
                }
            }
            if (ShopManager.instance.isSell)
            {
                ShopManager.instance.Sell();
            }
            else
            {
                ShopManager.instance.Buy();
            }
            //ShopManager.instance.Sell((int)inputText) 이렇게?
            
            inputField.gameObject.SetActive(false);
            gameObject.SetActive(false);
        });
        no.onClick.AddListener(() =>
        {
            inputField.gameObject.SetActive(false);
            gameObject.SetActive(false);
        });
    }
    private void OnEnable()
    {
        
        inputField.gameObject.SetActive(false);
    }
    public void RefreshBuyUI()
    {
        checkText.text = "Buy?";
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
