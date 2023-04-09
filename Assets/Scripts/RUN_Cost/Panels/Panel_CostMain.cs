using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Panel_CostMain : PanelBase
{
    [Header("UI_Button")]
    public Button btn_plusPeople;
    public Button btn_plusOnePerson, btn_plusPayList, btn_plusOnePayment, btn_setRatio, btn_openSaveList, btn_newSave, btn_btnExistedSave, btn_otherDataSave;
    public Button btn_deleteAllMember, btn_deleteAllPayment;
    public Button btn_result, btn_home, btn_logOut;

    [Header("UI_Content")]
    public Transform content_member;
    public Transform content_payment;

    [Header("UI_PrefabList")]
    public List<GameObject> prefabM_member;
    public List<GameObject> prefabM_payment;

    [Header("UI_Text")]
    public TMP_Text txt_loadDataName;
    public Data_SaveList LoadData_SaveList;

    public override void OnInit()
    {
        base.OnInit();
        btn_openSaveList.onClick.AddListener(Btn_OpenSaveList);
        btn_newSave.onClick.AddListener(Btn_Save);
        btn_btnExistedSave.onClick.AddListener(Btn_ExistedSave);
        btn_otherDataSave.onClick.AddListener(Btn_OtherOverwrite);
        btn_plusOnePerson.onClick.AddListener(Btn_OneMember);
        btn_plusOnePayment.onClick.AddListener(Btn_OnePayment);
        btn_deleteAllMember.onClick.AddListener(DeleteAllMember);
        btn_deleteAllPayment.onClick.AddListener(DeleteAllPayment);
        btn_plusPeople.onClick.AddListener(Btn_EasyTypingMember);
        btn_plusPayList.onClick.AddListener(Btn_EasyTypingPayment);
        btn_setRatio.onClick.AddListener(Btn_SetRatio);
        btn_result.onClick.AddListener(Btn_Result);
        btn_home.onClick.AddListener(Btn_Home);
        btn_logOut.onClick.AddListener(Exit);
        GameManager._panel_costMain = this;
        prefabM_member = new List<GameObject>();
        prefabM_payment = new List<GameObject>();
        LoadData_SaveList = null;
        SetLoadData_Initiation();
    }
    public void SetLoadData_Initiation()
    {
        LoadData_SaveList = new Data_SaveList("Not Saved Data", null);
    }
    public void Btn_Save()
    {
        var asset_saveSet = Resources.Load<GameObject>("Prefabs/Popups/popup_SaveList_NameSet");
        var popup_saveSet = CreatePanel(asset_saveSet);
        popup_saveSet.GetComponent<Popup_SaveListNameSet>().OnInit();
    }
    void Btn_OpenSaveList()
    {
        var asset_saveList = Resources.Load<GameObject>("Prefabs/Panels/panel_saveList");
        var popup_saveList = CreatePanel(asset_saveList);
        popup_saveList.GetComponent<Panel_SaveList>().OnInit();
    }
    public void DeleteAllMember()
    {
        prefabM_member.Clear();
        LoadData_SaveList.list_members.Clear();
        foreach (var payment in LoadData_SaveList.list_payments)
        {
            payment.membersForPay.Clear();
        }
        ClearChildren(content_member);
    }
    public void DeleteAllPayment()
    {
        prefabM_payment.Clear();
        LoadData_SaveList.list_payments.Clear();
        ClearChildren(content_payment);
    }
    void Btn_OneMember()
    {
        var asset_member = Resources.Load<GameObject>("Prefabs/Contents/Prefab_member");
        var obj_member = GameObject.Instantiate(asset_member, content_member);
        var script = obj_member.GetComponent<Prefab_Member>();
        prefabM_member.Add(obj_member);
        var data_member = new Data_Members("");
        LoadData_SaveList.list_members.Add(data_member);
        script.btn_delete.onClick.AddListener(() => DeleteMemberPrefab(data_member));
        var data_payments = LoadData_SaveList.list_payments;
        foreach (var data_payment in data_payments)
        {
            var data_memberForPay = new Data_MemberForPay(data_member);
            data_payment.membersForPay.Add(data_memberForPay);
        }
        script.input_memberName.onValueChanged.AddListener(x => InputName_ValueChanged(obj_member));
    }

    public override void DeleteMemberPrefab(Data_Members data_member)
    {
        base.DeleteMemberPrefab(data_member);
    }

    void Btn_OnePayment()
    {
        var asset_payment = Resources.Load<GameObject>("Prefabs/Contents/Prefab_payment");
        var obj_payment = GameObject.Instantiate(asset_payment, content_payment);
        var script = obj_payment.GetComponent<Prefab_Payment>();
        var data_payment = new Data_Payments("", 0);
        LoadData_SaveList.list_payments.Add(data_payment);
        prefabM_payment.Add(obj_payment);
        script.btn_delete.onClick.AddListener(() => DeletePaymentPrefab(data_payment));
        script.membersForPay = new List<Data_MemberForPay>();
        foreach (var data_member in LoadData_SaveList.list_members)
        {
            var memberForPay = new Data_MemberForPay(data_member);
            data_payment.membersForPay.Add(memberForPay);
        }
        script.input_payInfo.onValueChanged.AddListener(x => InputInfo_ValueChanged(obj_payment));
        script.input_payCost.onValueChanged.AddListener(x => InputCost_ValueChanged(obj_payment));
    }
    void Btn_EasyTypingMember()
    {
        var asset_simpleTyping = Resources.Load<GameObject>("Prefabs/Panels/panel_SimpleTypingMembers");
        var panel_simpleTyping = CreatePanel(asset_simpleTyping);
        panel_simpleTyping.GetComponent<Panel_SimpleTypingMember>().OnInit();
    }
    void Btn_EasyTypingPayment()
    {
        var asset_simpleTyping = Resources.Load<GameObject>("Prefabs/Panels/panel_SimpleTypingPayment");
        var panel_simpleTyping = CreatePanel(asset_simpleTyping);
        panel_simpleTyping.GetComponent<Panel_SimpleTypingPayment>().OnInit();
    }
    void Btn_SetRatio()
    {
        switch (CheckReady())
        {
            case 1:
                var asset_comment_m = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
                var popup_comment_m = CreatePanel(asset_comment_m);
                popup_comment_m.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.simpleComment, "인원 설정 오류", "이름이 입력되지 않은 인원이 있습니다.\n모든 인원의 이름을 입력해주세요.");
                return;

            case 2:
                var asset_comment_pi = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
                var popup_comment_pi = CreatePanel(asset_comment_pi);
                popup_comment_pi.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.simpleComment, "금액 설정 오류", "입력되지 않은 지출 정보가 있습니다.\n모든 지출에 정보를 입력해주세요.");
                return;
            case 3:
                var asset_comment_pc = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
                var popup_comment_pc = CreatePanel(asset_comment_pc);
                popup_comment_pc.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.simpleComment, "금액 설정 오류", "입력되지 않은 지출 비용이 있습니다.\n모든 지출에 비용를 입력해주세요.");
                return;
            case 4:
                var asset_comment_atLeast = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
                var popup_comment_atLeast = CreatePanel(asset_comment_atLeast);
                popup_comment_atLeast.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.simpleComment, "금액 설정 오류", "최소 1개 이상의 지출이 입력되어야 합니다.");
                return;
        }

        var asset_setRatio = Resources.Load<GameObject>("Prefabs/Panels/panel_SetRatio");
        var panel_setRatio = CreatePanel(asset_setRatio);
        var script_setRatio = panel_setRatio.GetComponent<Panel_SetRatio>();
        script_setRatio.OnInit();
    }
    
    public void Btn_ExistedSave()
    {
        var loadData = GameManager._panel_costMain.LoadData_SaveList;
        int index_saveList = FindSaveListIndex(loadData.dataCode);
        if (string.IsNullOrEmpty(loadData.saveDataTime))
        {
            var asset_comment = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
            var popup_comment = CreatePanel(asset_comment);
            popup_comment.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.simpleComment, "열린 데이터 없음", "현재 열린 데이터가 없습니다.\n새로 저장해주세요.");
            return;
        }
        if (index_saveList == 9999)
        {
            var asset_comment = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
            var popup_comment = CreatePanel(asset_comment);
            popup_comment.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.simpleComment, "저장된 데이터 없음", "덮어쓰기 할 저장된 데이터가 없습니다.\n새로 저장해주세요.");
            return;
        }
        var dataName = loadData.saveDataName;
        var index = FindSaveListIndex(loadData.dataCode);
        if(index != 9999)
        {
            var asset = Resources.Load<GameObject>("Prefabs/Popups/popup_Overwrite_FinalCheck");
            var prefab = CreatePanel(asset);
            var script = prefab.GetComponent<Popup_OverwriteFinalCheck>();
            script.OnInit(dataName, index);
        }
    }
    public void Btn_OtherOverwrite()
    {
        var asset_overwrite = Resources.Load<GameObject>("Prefabs/Popups/popup_saveListOverwrite");
        var popup_overwrite = CreatePanel(asset_overwrite);
        popup_overwrite.GetComponent<Popup_SaveListOverwrite>().OnInit();
    }
    public void Btn_Result()
    {
        switch (CheckReady())
        {
            case 1:
                var asset_comment_m = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
                var popup_comment_m = CreatePanel(asset_comment_m);
                popup_comment_m.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.simpleComment, "인원 설정 오류", "이름이 입력되지 않은 인원이 있습니다.\n모든 인원의 이름을 입력해주세요.");
                return;

            case 2:
                var asset_comment_pi = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
                var popup_comment_pi = CreatePanel(asset_comment_pi);
                popup_comment_pi.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.simpleComment, "금액 설정 오류", "입력되지 않은 지출 정보가 있습니다.\n모든 지출에 정보를 입력해주세요.");
                return;
            case 3:
                var asset_comment_pc = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
                var popup_comment_pc = CreatePanel(asset_comment_pc);
                popup_comment_pc.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.simpleComment, "금액 설정 오류", "입력되지 않은 지출 비용이 있습니다.\n모든 지출에 비용를 입력해주세요.");
                return;
            case 4:
                var asset_comment_atLeast = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
                var popup_comment_atLeast = CreatePanel(asset_comment_atLeast);
                popup_comment_atLeast.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.simpleComment, "금액 설정 오류", "최소 1개 이상의 지출이 입력되어야 합니다.");
                return;
        }

        CheckCostResult();
        var asset_result = Resources.Load<GameObject>("Prefabs/Panels/panel_Result");
        var popup_result = CreatePanel(asset_result);
        popup_result.GetComponent<Panel_Result>().OnInit();
    }
    public override void Exit()
    {
        //로그아웃
        base.Exit();
        GameManager.loginInfo = null;
        TabSwap.stack_tabList.Peek().tabList_onlyInput[0].Select();
        Destroy(backG);
    }
    public void Btn_Home()
    {
        base.Exit();
        GameManager._gm.SetHomePanel();
        Destroy(backG);
    }
}

