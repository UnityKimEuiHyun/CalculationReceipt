using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class JsonCreator
{
    AccountInfo accountInfo;
    public AccountInfo loadAccountInfo;
    public JsonCreator(AccountInfo _object)
    {
        accountInfo = _object;
    }
    public JsonCreator()
    {
        accountInfo = null;
    }
    public void ObjectToJson(string ID)
    {
        var file = ID + ".json";
        var path = Path.Combine(Application.streamingAssetsPath, file);
        string jdata = JsonUtility.ToJson(accountInfo);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);
        string format = System.Convert.ToBase64String(bytes);
        File.WriteAllText(path, format);
    }
    public AccountInfo JsonToObject(string ID)
    {
        var file = ID + ".json";
        var path = Path.Combine(Application.streamingAssetsPath, file);
        string jdata = File.ReadAllText(path);
        byte[] bytes = System.Convert.FromBase64String(jdata);
        string reformat = System.Text.Encoding.UTF8.GetString(bytes);
        var loadInfo = JsonUtility.FromJson<AccountInfo>(reformat);
        return loadInfo;
    }
    
}
