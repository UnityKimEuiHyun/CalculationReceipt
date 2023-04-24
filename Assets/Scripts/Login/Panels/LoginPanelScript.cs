using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class LoginPanelScript : PanelBase
{
    [Header("UI_Inputfield")]
    public TMP_InputField input_loginId;
    public TMP_InputField input_loginPw;
    [Header("UI_Button")]
    public Button btn_login;
    public Button btn_newAcc;
    public Button btn_findId, btn_findPw;
    public Button btn_quit;

    public string loginId, loginPw;

    public override void OnInit()
    {
        base.OnInit();
        GameManager._loginPanel = this;
        btn_login.onClick.AddListener(Complete);
        btn_findId.onClick.AddListener(Btn_FindIDPanel);
        btn_findPw.onClick.AddListener(Btn_FindPWPanel);
        btn_newAcc.onClick.AddListener(Btn_NewAccountPanel);
        btn_quit.onClick.AddListener(OnClick_Quit);
    }
    public override void Complete()
    {
        if (!TryLogin())
        {
            LoginFailure();
        }
        else
        {
            base.Complete();
            input_loginId.text = null;
            input_loginPw.text = null;
            GameManager._gm.SetHomePanel();
        }
    }

    bool TryLogin()
    {
        loginId = input_loginId.text;
        loginPw = input_loginPw.text;
        if (string.IsNullOrEmpty(loginId) || string.IsNullOrEmpty(loginPw)) return false;

        var file = loginId + ".json";
        var path = Path.Combine(Application.streamingAssetsPath, file);



        if (File.Exists(path))
        {
            string jdata_read = File.ReadAllText(path);
            byte[] bytes_read = System.Convert.FromBase64String(jdata_read);
            string reformat = System.Text.Encoding.UTF8.GetString(bytes_read);
            AccountInfo loadInfo = JsonUtility.FromJson<AccountInfo>(reformat);
            
            if (loadInfo.pw == loginPw)
            {
                GameManager.loginInfo = loadInfo;
                return true;
            }
        }
        GameManager.loginInfo = null;
        return false;
    }
    
    void LoginFailure()
    {
        var asset_comment = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
        var popup_comment = CreatePanel(asset_comment);
        popup_comment.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.LoginFailure, "로그인 실패", "계정 또는 비밀번호가 일치하지 않습니다.\n다시 확인해주세요.", -1);
    }
    void Btn_FindIDPanel()
    {
        var asset = Resources.Load<GameObject>("Prefabs/Panels/panel_FindID");
        var prefab = CreatePanel(asset);
        prefab.GetComponent<FindIDPanelScript>().OnInit();
    }
    void Btn_FindPWPanel()
    {
        var asset = Resources.Load<GameObject>("Prefabs/Panels/panel_FindPW");
        var prefab = CreatePanel(asset);
        prefab.GetComponent<FindPWPanelScript>().OnInit();
    }
    void Btn_NewAccountPanel()
    {
        var asset = Resources.Load<GameObject>("Prefabs/Panels/panel_NewAccount");
        var prefab = CreatePanel(asset);
        prefab.GetComponent<NewAccountPanelScript>().OnInit();
    }
    void OnClick_Quit()
    {
        Application.Quit();
    }
}
