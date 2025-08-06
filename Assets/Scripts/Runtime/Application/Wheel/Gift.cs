using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gift : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private int _amount = 0;
    [SerializeField] private TextMeshProUGUI _amountText;

    public int GetCost()
    {
        return _amount;
    }

    public void SetCost(int amount)
    {
        if (amount <= 0)
        {
            amount = 1;
        }

        _amount = amount;
        UpdateCostText();
    }

    public void UpdateCostText()
    {
        _amountText.text = _amount.ToString();
    }

    public RectTransform GetRotation()
    {
        return rectTransform;
    }

    public void SetRotation(float angleZ)
    {
        rectTransform.transform.localRotation = Quaternion.Euler(0, 0, angleZ);
    }
}