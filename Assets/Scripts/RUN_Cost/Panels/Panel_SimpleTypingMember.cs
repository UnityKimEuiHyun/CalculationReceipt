using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Panel_SimpleTypingMember : PanelBase
{
    [Header("UI_Inputfield")]
    public TMP_InputField input_typingName;

    [Header("UI_Button")]
    public Button btn_addMember;
    public Button btn_cancel;

    [Header("UI_Button")]
    public Transform content;

    public List<GameObject> prefabM_EasyTypingMembers;

    public override void OnInit()
    {
        base.OnInit();
        btn_addMember.onClick.AddListener(Btn_AddMember);
        btn_cancel.onClick.AddListener(Exit);
        SetSavedData();
    }
    public void SetSavedData()
    {
        var members = GameManager._panel_costMain.LoadData_SaveList.list_members;
        foreach (var member in members)
        {
            var asset = Resources.Load<GameObject>("Prefabs/Contents/Prefab_simpleTypingMember");
            var prefab = GameObject.Instantiate(asset, content);
            var script = prefab.GetComponent<Prefab_SimpleTypingMember>();
            script.input_memberName.text = member.saveMemberName;
            script.btn_delete.onClick.AddListener(() => Btn_Delete(member));
            script.input_memberName.onValueChanged.AddListener(x => InputEasyName_ValueChanged(script.input_memberName, member));
            prefabM_EasyTypingMembers.Add(prefab);
        }
    }

    public void Btn_AddMember()
    {
        var typingName = input_typingName.text;
        if (string.IsNullOrEmpty(typingName)) return;
        Btn_AddMember(typingName);
        input_typingName.text = null;
        btn_addMember.Select();
        input_typingName.Select();
    }
    public void Btn_AddMember(string typingName)
    {
        var asset = Resources.Load<GameObject>("Prefabs/Contents/Prefab_simpleTypingMember");
        var prefab = GameObject.Instantiate(asset, content);
        var script_easyMember = prefab.GetComponent<Prefab_SimpleTypingMember>();
        var data_member = new Data_Members(typingName);
        script_easyMember.btn_delete.onClick.AddListener(() => Btn_Delete(data_member));
        script_easyMember.input_memberName.text = typingName;
        script_easyMember.input_memberName.onValueChanged.AddListener(x => InputEasyName_ValueChanged(script_easyMember.input_memberName, data_member));
        prefabM_EasyTypingMembers.Add(prefab);

        var panel_CostMain = GameManager._panel_costMain;
        var asset_member = Resources.Load<GameObject>("Prefabs/Contents/Prefab_member");
        var content_member = GameManager._panel_costMain.content_member;
        var obj_member = GameObject.Instantiate(asset_member, content_member);
        var script_member = obj_member.GetComponent<Prefab_Member>();
        script_member.input_memberName.text = typingName;
        panel_CostMain.prefabM_member.Add(obj_member);
        
        panel_CostMain.LoadData_SaveList.list_members.Add(data_member);
        script_member.btn_delete.onClick.AddListener(() => panel_CostMain.DeleteMemberPrefab(data_member));
        var data_payments = panel_CostMain.LoadData_SaveList.list_payments;
        foreach (var data_payment in data_payments)
        {
            var data_memberForPay = new Data_MemberForPay(data_member);
            data_payment.membersForPay.Add(data_memberForPay);
        }
        script_member.input_memberName.onValueChanged.AddListener(x => InputName_ValueChanged(obj_member));
    }
    public void InputEasyName_ValueChanged(TMP_InputField input_name, Data_Members data_member)
    {
        var index_data = FindIndex_dataMember(data_member);
        GameManager._panel_costMain.LoadData_SaveList.list_members[index_data].saveMemberName = input_name.text;
        GameManager._panel_costMain.prefabM_member[index_data].GetComponent<Prefab_Member>().input_memberName.text = input_name.text;
    }
    public void Btn_Delete(Data_Members data_member)
    {
        int index_member = FindIndex_dataMember(data_member);
        GameManager._panel_costMain.LoadData_SaveList.list_members.RemoveAt(index_member);
        Destroy(GameManager._panel_costMain.prefabM_member[index_member]);
        GameManager._panel_costMain.prefabM_member.RemoveAt(index_member);
        Destroy(prefabM_EasyTypingMembers[index_member]);
        prefabM_EasyTypingMembers.RemoveAt(index_member);
    }
    public int FindIndex_dataMember(Data_Members data_member)
    {
        var members = GameManager._panel_costMain.LoadData_SaveList.list_members;
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i] == data_member)
                return i;
        }
        return 9999;
    }
    public override void Exit()
    {
        base.Exit();
        Destroy(backG);
    }
    public override void Complete()
    {
        Btn_AddMember();
    }
    
}
