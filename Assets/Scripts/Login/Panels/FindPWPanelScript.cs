using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.IO;

public class FindPWPanelScript : PanelBase
{
    [Header("UI_Input")]
    public TMP_InputField input_userId;
    public TMP_InputField input_userName;
    public TMP_InputField input_userTel;
    [Header("UI_Button")]
    public Button btn_findPW;
    public Button btn_cancel;

    public override void OnInit()
    {
        base.OnInit();
        GameManager._panel_findPW = this;
        btn_findPW.onClick.AddListener(Btn_FindPW);
        btn_cancel.onClick.AddListener(Exit);
        input_userTel.onValueChanged.AddListener(x => PhoneNumValueChange(input_userTel));
    }
    void Btn_FindPW()
    {
        var asset = Resources.Load<GameObject>("Prefabs/Popups/popup_resultofFindPW");
        var prefab = CreatePanel(asset);
        var result = FindPW();
        if(result == null)
            prefab.GetComponent<Popup_ResultofFindPW>().OnInit(input_userId.text);
        else prefab.GetComponent<Popup_ResultofFindPW>().OnInit(result);

    }
    AccountInfo FindPW()
    {
        var file = "AllAccount.json";
        var path = Path.Combine(Application.streamingAssetsPath, file);
        string jdata_read = File.ReadAllText(path);
        byte[] bytes_read = System.Convert.FromBase64String(jdata_read);
        string reformat = System.Text.Encoding.UTF8.GetString(bytes_read);
        AllAccount loadInfo = JsonUtility.FromJson<AllAccount>(reformat);
        foreach (var account in loadInfo.allAccount)
        {
            if (account.id == input_userId.text && account.userName == input_userName.text && account.tel == input_userTel.text)
                return account;
        }
        return null;
    }
    public override void Complete()
    {
        Btn_FindPW();
    }
    public override void Exit()
    {
        base.Exit();
        GameManager._panel_findPW = null;
        TabSwap.stack_tabList.Peek().SelectInputField(0);
        Destroy(backG);
    }
}
