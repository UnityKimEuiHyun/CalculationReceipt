using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Panel_SetRatio : PanelBase
{
    [Header("UI_Transform")]
    public Transform content_payment;
    public Transform content_member;

    [Header("UI_Button")]
    public Button btn_Okay;

    [Header("UI_Text")]
    public TMP_Text txt_selectedPayment;
    public TMP_Text txt_title;

    public List<GameObject> prefabM_payments;
    public List<GameObject> prefabM_members;

    public Queue<Data_Payments> queue_dataPayment;

    public override void OnInit()
    {
        base.OnInit();
        btn_Okay.onClick.AddListener(Complete);
        CheckCostResult();
        SetPaymentData();
        queue_dataPayment = new Queue<Data_Payments>();
    }
    void SetPaymentData()
    {
        var listPayment = GameManager._panel_costMain.LoadData_SaveList.list_payments;
        for (int i = 0; i < listPayment.Count; i++)
        {
            var asset = Resources.Load<GameObject>("Prefabs/Contents/Prefab_RatioPayment");
            var prefab = GameObject.Instantiate(asset, content_payment);
            var script = prefab.GetComponent<Prefab_RatioPayment>();
            var index_pay = i;
            script.btn_payInfo.onClick.AddListener(()=> LoadMemberForPay(index_pay));
            script.btn_payInfo.GetComponentInChildren<TMP_Text>().text = listPayment[index_pay].savePaymentInfo;
            script.txt_costofPayment.text = listPayment[index_pay].savePaymentCost.ToString();
            script.txt_numofMember.text = listPayment[index_pay].membersForPay.Count.ToString();
            prefabM_payments.Add(prefab);
        }
    }
    void LoadMemberForPay(int index_pay)
    {
        var loadData = GameManager._panel_costMain.LoadData_SaveList;
        if (loadData.list_members.Count==0)
        {
            var asset_comment = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
            var popup_comment = CreatePanel(asset_comment);
            popup_comment.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.simpleComment, "인원 없음", "입력된 인원이 없습니다.\n인원 정보를 입력해주세요.");
            return;
        }

        if(queue_dataPayment.Count != 0)
        {
            if (CheckAllZero())
            {
                var asset_comment = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
                var popup_comment = CreatePanel(asset_comment);
                popup_comment.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.simpleComment, "배분 비율 오류", "배분 비율이 모두 0으로 설정할 수 없습니다.");
                return;
            }
            queue_dataPayment.Dequeue();
        }
        txt_title.text = $"선택된 지출 내용:[{loadData.list_payments[index_pay].savePaymentInfo}]";
        var Payment = loadData.list_payments[index_pay];
        prefabM_members.Clear();
        ClearChildren(content_member);
        for (int i = 0; i < Payment.membersForPay.Count; i++)
        {
            var member = Payment.membersForPay[i];
            var asset = Resources.Load<GameObject>("Prefabs/Contents/Prefab_RatioMember");
            var prefab = GameObject.Instantiate(asset, content_member);
            var script = prefab.GetComponent<Prefab_RatioMember>();
            int index_memberforpay = i;
            script.btn_memberName.GetComponentInChildren<TMP_Text>().text = member.data_member.saveMemberName;
            script.btn_memberName.onClick.AddListener(()=>RatioZero(script));
            script.input_ratio.text = member.ratio.ToString();
            script.txt_costResult.text = member.costResult.ToString();
            script.input_ratio.onValueChanged.AddListener(x => ValueChanged_Ratio(script.input_ratio, index_memberforpay, index_pay));
            prefabM_members.Add(prefab);
        }
        queue_dataPayment.Enqueue(Payment);
    }
    public bool CheckAllZero()
    {
        foreach (var prefab in prefabM_members)
        {
            if (int.Parse(prefab.GetComponent<Prefab_RatioMember>().input_ratio.text) != 0)
                return false;
        }
        return true;
    }
    void RatioZero(Prefab_RatioMember btnScript)
    {
        btnScript.input_ratio.text = "0";
    }
    void ValueChanged_Ratio(TMP_InputField input_ratio, int index_memberforpay, int index_pay)
    {
        var Payment = GameManager._panel_costMain.LoadData_SaveList.list_payments[index_pay];
        if (input_ratio.text == null)
            Payment.membersForPay[index_memberforpay].ratio = 0;
        else { Payment.membersForPay[index_memberforpay].ratio = float.Parse(input_ratio.text); 
        }
        CheckCostResult();
        for (int i = 0; i < prefabM_members.Count; i++)
        {
            var script = prefabM_members[i].GetComponent<Prefab_RatioMember>();
            script.txt_costResult.text = Payment.membersForPay[i].costResult.ToString();
        }
    }

    public override void Complete()
    {
        if (queue_dataPayment.Count != 0 && CheckAllZero())
        {
            var asset_comment = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
            var popup_comment = CreatePanel(asset_comment);
            popup_comment.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.simpleComment, "배분 비율 오류", "배분 비율이 모두 0으로 설정할 수 없습니다.");
            return;
        }
        base.Complete();
        var LoadData = GameManager._panel_costMain.LoadData_SaveList;
        ChangeData(LoadData);
        Destroy(backG);
    }
    public override void Exit()
    {
        base.Exit();
        Destroy(backG);
    }
}
