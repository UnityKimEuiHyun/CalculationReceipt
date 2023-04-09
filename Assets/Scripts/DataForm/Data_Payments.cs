using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Data_Payments
{
    public string savePaymentInfo;
    public int savePaymentCost;
    public List<Data_MemberForPay> membersForPay;
    public string dataCode;

    public Data_Payments(string paymentInfo, int paymentCost)
    {
        savePaymentInfo = paymentInfo;
        savePaymentCost = paymentCost;
        membersForPay = new List<Data_MemberForPay>();
        SetDataCode();
    }
    public void SetDataCode()
    {
        var time = DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_tt");
        dataCode = $"p{GameManager.loginInfo.dataIndex}_{time}";
        GameManager.loginInfo.dataIndex += 1;
    }
    public Data_Payments DeepCopy()
    {
        Data_Payments copiedData = new Data_Payments(savePaymentInfo, savePaymentCost);
        for (int i = 0; i < membersForPay.Count; i++)
        {
            copiedData.membersForPay.Add(membersForPay[i].DeepCopy());
        }
        copiedData.dataCode = dataCode;
        return copiedData;
    }
}

