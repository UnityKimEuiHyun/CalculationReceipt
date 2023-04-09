using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Data_SaveList
{
    public string saveDataName;
    public string saveDataTime;
    public List<Data_Members> list_members;
    public List<Data_Payments> list_payments;
    public string dataCode;
    public string userComment;
    public string etc;

    public Data_SaveList(string name, string time)
    {
        saveDataName = name;
        saveDataTime = time;
        list_members = new List<Data_Members>();
        list_payments = new List<Data_Payments>();
        userComment = "";
        etc = "";
        DataCodeSet();
    }
    public void Clear()
    {
        list_members.Clear();
        list_payments.Clear();
    }
    public Data_SaveList DeepCopy()
    {
        Data_SaveList CopiedData = new Data_SaveList(saveDataName, saveDataTime);
        for (int i = 0; i < list_members.Count; i++)
        {
            CopiedData.list_members.Add(list_members[i].DeepCopy());
        }
        for (int i = 0; i < list_payments.Count; i++)
        {
            CopiedData.list_payments.Add(list_payments[i].DeepCopy());
            for (int j = 0; j < CopiedData.list_members.Count; j++)
            {
                CopiedData.list_payments[i].membersForPay[j].data_member = CopiedData.list_members[j];
            }
        }
        CopiedData.dataCode = dataCode;
        return CopiedData;
    }
    public void DataCodeSet()
    {
        dataCode = $"s{GameManager.loginInfo.dataIndex}_{saveDataTime}";
        GameManager.loginInfo.dataIndex += 1;
    }
}
