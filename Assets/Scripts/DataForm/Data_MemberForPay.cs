using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Data_MemberForPay
{
    public Data_Members data_member;
    public float ratio;
    public int costResult;
    public string dataCode;
   
    public Data_MemberForPay(Data_Members data_member)
    {
        this.data_member = data_member;
        ratio = 1;
        costResult = 0;
        SetDataCode();
    }
    public void SetDataCode()
    {
        var time = DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_tt");
        dataCode = $"mp{GameManager.loginInfo.dataIndex}_{time}";
        GameManager.loginInfo.dataIndex += 1;
    }
    public Data_MemberForPay DeepCopy()
    {
        Data_MemberForPay copiedData = new Data_MemberForPay(data_member);
        copiedData.ratio = ratio;
        copiedData.dataCode = dataCode;
        return copiedData;
    }
}
