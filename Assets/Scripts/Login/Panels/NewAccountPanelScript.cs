using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;


public class NewAccountPanelScript : PanelBase
{
    [Header("UI_Inputfield")]
    public TMP_InputField input_id;
    public TMP_InputField input_pw, input_pwCheck, input_name, input_telNum, input_emailAccount;
    [Header("UI_Dropdown")]
    public TMP_Dropdown dr_emailDomain;
    [Header("UI_Button")]
    public Button btn_idCheck;
    public Button btn_telCheck, btn_cancel, btn_comp;
    [Header("UI_Text")]
    public TMP_Text txt_idComment;
    public TMP_Text txt_pwComment, txt_pwCheckComment, txt_finalComment;
    public string telCode = "1234";
    string _finalId;
    string _finalPw, _finalName, _finalTel, _finalEmail;
    bool _finishId, _finishPw, _finishTel;
    #region Property
    public string FinalID
    {
        get { return _finalId; }
    }
    public bool FinishID
    {
        get { return _finishId; }
        set {
            if (value) _finalId = input_id.text;
            _finishId = value;
        }
    }
    public bool FinishPW
    {
        get { if (input_pw.text == input_pwCheck.text) return _finishPw;
            else return false;
        }
        set {
            if (value) _finalPw = input_pw.text;
            _finishPw = value;
        }
    }
    public bool FinishTEL
    {
        get { return _finishTel; }
        set {
            SetFinalTel(value);
            _finishTel = value;
        }
    }
    public bool FinishEMAIL
    {
        get { if (!string.IsNullOrEmpty(input_emailAccount.text))
            {
                SetFinalEmail();
                return true;
            }
            else return false;
        }
    }
    public bool FinishNAME
    {
        get {
            if (!string.IsNullOrEmpty(input_name.text))
            {
                SetFinalName();
                return true;
            }
            else return false;
        }
    }
    #endregion
    #region Method
    public override void OnInit()
    {
        base.OnInit();
        GameManager._panel_newAccount = this;
        input_id.onValueChanged.AddListener(x=> ChangedIDData(input_id));
        btn_idCheck.onClick.AddListener(Popup_IdCheckPopup);
        input_pw.onValueChanged.AddListener(x=>CheckPWStrength());
        txt_pwComment.text = "8자 이상, 15자 이하의 영문자+숫자+특수문자 조합";
        input_pwCheck.onValueChanged.AddListener(x => IsEqualPWCheck());
        input_telNum.onValueChanged.AddListener(x => PhoneNumValueChange(input_telNum));
        btn_telCheck.onClick.AddListener(Popup_telCheckPopup);
        dr_emailDomain.onValueChanged.AddListener( x => CheckOtherEmail());
        btn_cancel.onClick.AddListener(Exit);
        btn_comp.onClick.AddListener(Complete);
        txt_idComment.text = "ID 입력 후 중복확인을 진행해주세요.";
        txt_finalComment.color = GameManager._gm.color_txt_caution;
    }

    public void Popup_IdCheckPopup()
    {
        string id = input_id.text;
        IDValidator iv = new IDValidator(id);
        if (string.IsNullOrWhiteSpace(input_id.text))
        {
            txt_idComment.color = GameManager._gm.color_txt_caution;
            txt_idComment.text = "ID를 입력해주세요.";
            GetComponent<TabList>().tabList[0].Select();
        }
        else if (!iv.OkChar)
        {
            txt_idComment.color = GameManager._gm.color_txt_caution;
            txt_idComment.text = "ID는 영문자를 포함해야 합니다.";
            TabSwap.stack_tabList.Peek().SelectInputField(0);
        }
        else if(!iv.OkNum)
        {
            txt_idComment.color = GameManager._gm.color_txt_caution;
            txt_idComment.text = "ID는 최소 1개 이상의 숫자를 포함해야 합니다.";
            TabSwap.stack_tabList.Peek().SelectInputField(0);
        }
        else if(!iv.OkLength)
        {
            txt_idComment.color = GameManager._gm.color_txt_caution;
            txt_idComment.text = "ID는 최소 4자 이상의 문자로 구성되어야 합니다.";
            TabSwap.stack_tabList.Peek().SelectInputField(0);
        }
        else if(CheckIdDuplication(id))
        {
            txt_idComment.color = GameManager._gm.color_txt_caution;
            txt_idComment.text = "이미 사용중인 ID입니다.";
            TabSwap.stack_tabList.Peek().SelectInputField(0);
        }
        else
        {
            txt_idComment.text = null;
            var asset = Resources.Load<GameObject>("Prefabs/Popups/popup_idCheck");
            var prefab = CreatePanel(asset);
            prefab.GetComponent<Popup_IDCheck>().OnInit();
            btn_idCheck.GetComponent<Image>().color = GameManager._gm.color_btn_normal;
        }
    }
    
    void ChangedIDData(TMP_InputField input)
    {
        txt_idComment.text = null;
        btn_idCheck.GetComponent<Image>().color = GameManager._gm.color_btn_normal;
        FinishID = false;
    }
    bool CheckIdDuplication(string id)
    {
        
        var file = "AllAccount.json";
        var path = Path.Combine(Application.streamingAssetsPath, file);
        if (!File.Exists(path)) return false;

        string jdata_read = File.ReadAllText(path);
        byte[] bytes_read = System.Convert.FromBase64String(jdata_read);
        string reformat = System.Text.Encoding.UTF8.GetString(bytes_read);
        AllAccount loadInfo = JsonUtility.FromJson<AllAccount>(reformat);
        foreach (var account in loadInfo.allAccount)
        {
            if (account.id == id)
                return true;
        }
        return false;
    }
    void Popup_telCheckPopup()
    {
        if (string.IsNullOrEmpty(input_telNum.text) || input_telNum.text.Length < 12)
        {
            var asset_comment = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
            var popup_comment = CreatePanel(asset_comment);
            popup_comment.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.simpleComment, "부적절한 형식의 전화번호", "전화번호의 형식이 부적절합니다.\n다시 확인해주세요.", -1);
            return;
        }
        var asset_tel = Resources.Load<GameObject>("Prefabs/Popups/popup_telCheck");
        var popup_tel = CreatePanel(asset_tel);
        popup_tel.GetComponent<Popup_TelCheck>().OnInit();

        //임의의 코드 4자리 숫자를 생성한 뒤 문자 전송
        telCode = "1234";
        
    }
    public override void Exit()
    {
        base.Exit();
        TabSwap.stack_tabList.Peek().SelectInputField(0);
        GameManager._panel_newAccount = null;
        Destroy(backG);
    }
    public override void Complete()
    {
        if (!FinishID)
        {
            txt_finalComment.text = "아이디 중복 확인이 완료되지 않았습니다.";
            TabSwap.stack_tabList.Peek().SelectInputField(0);
        }
        else if (!FinishPW)
        {
            txt_finalComment.text = "비밀번호가 적합하지 않습니다.";
            TabSwap.stack_tabList.Peek().SelectInputField(2);
        }
        else if (!FinishNAME)
        {
            txt_finalComment.text = "이름을 입력해주세요.";
            TabSwap.stack_tabList.Peek().SelectInputField(4);
        }
        else if (!FinishTEL)
        {
            txt_finalComment.text = "전화번호 인증이 완료되지 않았습니다.";
            TabSwap.stack_tabList.Peek().SelectInputField(5);
        }
        else if (!FinishEMAIL)
        {
            txt_finalComment.text = "이메일 계정이 입력되지 않았습니다.";
            TabSwap.stack_tabList.Peek().SelectInputField(7);
        }
        else
        {
            string _finalDate = DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_tt");
            CreateNewAccountJson(_finalId, _finalPw, _finalName, _finalTel, _finalEmail, _finalDate);
            var asset = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
            var popup_comment = CreatePanel(asset);
            popup_comment.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.CompleteNewAccount, "계정생성 완료", "계정이 생성되었습니다.");
        }
    }
    void CreateNewAccountJson(string id, string pw, string name, string tel, string email, string date)
    {
        //추후 계정생성 메서드에 추가할 내용(json생성) [NewtonSoft의 외부 라이브러리 사용]
        //오브젝트 -> 데이터
        AccountInfo accountInfo = new AccountInfo(id, pw, name, tel, email, date);
        AddNewAccount(accountInfo);
        JsonCreator jc = new JsonCreator(accountInfo);
        jc.ObjectToJson(id);
    }
    void AddNewAccount(AccountInfo newAccount)
    {
        var file = "AllAccount.json";
        var path = Path.Combine(Application.streamingAssetsPath, file);
        AllAccount loadInfo;
        if (!File.Exists(path))
        {
            loadInfo = new AllAccount();
        }
        else
        {
            string jdata_read = File.ReadAllText(path);
            byte[] bytes_read = System.Convert.FromBase64String(jdata_read);
            string reformat = System.Text.Encoding.UTF8.GetString(bytes_read);
            loadInfo = JsonUtility.FromJson<AllAccount>(reformat);
        }
        loadInfo.allAccount.Add(newAccount);
        
        string jdata_write = JsonUtility.ToJson(loadInfo);
        byte[] bytes_write = System.Text.Encoding.UTF8.GetBytes(jdata_write);
        string format_write = System.Convert.ToBase64String(bytes_write);
        File.WriteAllText(path, format_write);
    }
    void CheckPWStrength()
    {
        string password = input_pw.text;
        PasswordValidator pv = new PasswordValidator(password);
        txt_pwComment.text = pv.Comment;
        FinishPW = false;
        if (pv._okStrength)
        {
            txt_pwCheckComment.text = "PW를 한번 더 입력해주세요.";
            txt_pwCheckComment.color = GameManager._gm.color_txt_normal;
            FinishPW = true;
        }
        IsEqualPWCheck();
    }
    void IsEqualPWCheck()
    {
        if (string.IsNullOrEmpty(input_pwCheck.text)|| !_finishPw) return;

        if (input_pw.text == input_pwCheck.text)
        {
            txt_pwCheckComment.text = "입력한 PW와 일치합니다.";
            txt_pwCheckComment.color = GameManager._gm.color_txt_confirmed;
            FinishPW = true;
        }
        else
        {
            txt_pwCheckComment.text = "입력한 PW와 일치하지 않습니다.";
            txt_pwCheckComment.color = GameManager._gm.color_txt_caution;
        }
    }
    void SetFinalName()
    {
        _finalName = input_name.text;
    }
    void SetFinalTel(bool isFinish)
    {
        if(isFinish)
        {
            btn_telCheck.interactable = false;
            btn_telCheck.GetComponent<Image>().color = GameManager._gm.color_btn_complete;
            input_telNum.interactable = false;
            _finalTel = input_telNum.text;
        }
        else
        {
            btn_telCheck.interactable = true;
            btn_telCheck.GetComponent<Image>().color = GameManager._gm.color_btn_normal;
            input_telNum.interactable = true;
        }
    }
    void SetFinalEmail()
    {
        _finalEmail = $"{input_emailAccount.text}@{dr_emailDomain.captionText.text}";
    }
    void CheckOtherEmail()
    {
        if(dr_emailDomain.captionText.text == "직접 입력")
        {
            var asset = Resources.Load<GameObject>("Prefabs/Popups/popup_otherEmail");
            var popup_otherEmail = CreatePanel(asset);
            popup_otherEmail.GetComponent<Popup_OtherEmail>().OnInit();
        }
    }
    #endregion
}
