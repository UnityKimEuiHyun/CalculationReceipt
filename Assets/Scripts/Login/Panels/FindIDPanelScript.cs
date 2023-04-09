using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.IO;

public class FindIDPanelScript : PanelBase
{
    [Header("UI_Input")]
    public TMP_InputField input_userName;
    public TMP_InputField input_userTel;
    [Header("UI_Button")]
    public Button btn_findID;
    public Button btn_cancel;
    

    public override void OnInit()
    {
        base.OnInit();
        GameManager._panel_findID = this;
        btn_findID.onClick.AddListener(Btn_FindID);
        btn_cancel.onClick.AddListener(Exit);
        input_userTel.onValueChanged.AddListener(x => PhoneNumValueChange(input_userTel));
    }
    void Btn_FindID()
    {
        var file = "AllAccount.json";
        var path = Path.Combine(Application.streamingAssetsPath, file);
        string jdata_read = File.ReadAllText(path);
        byte[] bytes_read = System.Convert.FromBase64String(jdata_read);
        string reformat = System.Text.Encoding.UTF8.GetString(bytes_read);
        AllAccount loadInfo = JsonUtility.FromJson<AllAccount>(reformat);
        List<AccountInfo> findAccountList = new List<AccountInfo>();
        
        var asset = Resources.Load<GameObject>("Prefabs/Popups/popup_resultofFindID");
        var prefab = CreatePanel(asset);
        foreach (var account in loadInfo.allAccount)
        {
            Debug.Log($"account.ID = {account.id}, account.tel = {account.tel}");
            if (account.userName == input_userName.text && account.tel == input_userTel.text)
                findAccountList.Add(account);
        }
        
        if(findAccountList.Count == 0)
        prefab.GetComponent<Popup_ResultofFindID>().OnInit();
        else prefab.GetComponent<Popup_ResultofFindID>().OnInit(input_userName.text, findAccountList);
    }

    
    public override void Complete()
    {
        Btn_FindID();
    }
    public override void Exit()
    {
        base.Exit();
        GameManager._panel_findID = null;
        TabSwap.stack_tabList.Peek().SelectInputField(0);
        Destroy(backG);
    }
}
