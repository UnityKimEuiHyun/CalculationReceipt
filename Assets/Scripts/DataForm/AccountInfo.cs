using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AccountInfo
{
    public string id, pw, userName, tel, email, registerDate;
    public List<Data_SaveList> Data_saveList;
    public int dataIndex;

    public AccountInfo(string id, string pw, string name, string tel, string email, string date)
    {
        this.id = id;
        this.pw = pw;
        userName = name;
        this.tel = tel;
        this.email = email;
        registerDate = date;
        Data_saveList = new List<Data_SaveList>();
        dataIndex = 0;
    }
    public void ObjectToJson()
    {
        JsonCreator js = new JsonCreator(this);
        js.ObjectToJson(id);
    }
    public void Clear()
    {
        foreach (var item in Data_saveList)
        {
            item.Clear();
        }
        Data_saveList.Clear();
    }
}
