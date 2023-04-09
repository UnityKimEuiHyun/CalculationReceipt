using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Data_Members
{
    public string saveMemberName;
    public string dataCode;
   
    public Data_Members(string memberName)
    {
        saveMemberName = memberName;
        SetDataCode();
    }
    public void SetDataCode()
    {
        var time = DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_tt");
        dataCode = $"m{GameManager.loginInfo.dataIndex}_{time}";
        GameManager.loginInfo.dataIndex += 1;
    }
    public Data_Members DeepCopy()
    {
        Data_Members copiedData = new Data_Members(saveMemberName);
        copiedData.dataCode = dataCode;
        return copiedData;
    }
}
