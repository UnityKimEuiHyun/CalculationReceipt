using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Panel_SimpleTypingPayment : PanelBase
{
    [Header("UI_Inputfield")]
    public TMP_InputField input_typingInfo;
    public TMP_InputField input_typingCost;

    [Header("UI_Button")]
    public Button btn_add;
    public Button btn_onClickTyping;
    public Button btn_cancel;

    [Header("UI_Button")]
    public Transform content;

    public List<GameObject> prefabM_simplePayments;

    public override void OnInit()
    {
        base.OnInit();
        btn_add.onClick.AddListener(Btn_Add);
        btn_cancel.onClick.AddListener(Exit);
        btn_onClickTyping.onClick.AddListener(Btn_OneClickTyping);
        SetSavedData();
    }
    public void SetSavedData()
    {
        var payments = GameManager._panel_costMain.LoadData_SaveList.list_payments;
        foreach (var payment in payments)
        {
            var asset = Resources.Load<GameObject>("Prefabs/Contents/Prefab_simpleTypingPayment");
            var prefab = GameObject.Instantiate(asset, content);
            var script = prefab.GetComponent<Prefab_SimpleTypingPayment>();
            script.input_PaymentInfo.text = payment.savePaymentInfo;
            script.input_PaymentCost.text = payment.savePaymentCost.ToString();
            script.btn_delete.onClick.AddListener(() => Btn_Delete_Payment(payment));
            script.input_PaymentInfo.onValueChanged.AddListener(x => InputEasyInfo_ValueChanged(script.input_PaymentInfo, payment));
            script.input_PaymentCost.onValueChanged.AddListener(x => InputEasyCost_ValueChanged(script.input_PaymentCost, payment));
            prefabM_simplePayments.Add(prefab);
        }
    }
    public void InputEasyInfo_ValueChanged(TMP_InputField input_info, Data_Payments data_payment)
    {
        var index_data = FindIndex_dataPayment(data_payment);
        GameManager._panel_costMain.LoadData_SaveList.list_payments[index_data].savePaymentInfo = input_info.text;
        GameManager._panel_costMain.prefabM_payment[index_data].GetComponent<Prefab_Payment>().input_payInfo.text = input_info.text;
    }
    public void InputEasyCost_ValueChanged(TMP_InputField input_cost, Data_Payments data_payment)
    {
        var index_data = FindIndex_dataPayment(data_payment);
        GameManager._panel_costMain.LoadData_SaveList.list_payments[index_data].savePaymentCost = int.Parse(input_cost.text);
        GameManager._panel_costMain.prefabM_payment[index_data].GetComponent<Prefab_Payment>().input_payCost.text = input_cost.text;
    }
    public void Btn_Delete_Payment(Data_Payments data_payment)
    {
        int index_payment = FindIndex_dataPayment(data_payment);
        GameManager._panel_costMain.LoadData_SaveList.list_payments.RemoveAt(index_payment);
        Destroy(GameManager._panel_costMain.prefabM_payment[index_payment]);
        GameManager._panel_costMain.prefabM_payment.RemoveAt(index_payment);
        Destroy(prefabM_simplePayments[index_payment]);
        prefabM_simplePayments.RemoveAt(index_payment);
    }
    public int FindIndex_dataPayment(Data_Payments data_payment)
    {
        var payments = GameManager._panel_costMain.LoadData_SaveList.list_payments;
        for (int i = 0; i < payments.Count; i++)
        {
            if (payments[i] == data_payment)
                return i;
        }
        return 9999;
    }
    public override void Exit()
    {
        base.Exit();
        Destroy(backG);
    }
    public void Btn_Add()
    {
        var typingInfo = input_typingInfo.text;
        var typingCost = int.Parse(input_typingCost.text);

        if (string.IsNullOrEmpty(typingInfo)|| string.IsNullOrEmpty(input_typingCost.text)) return;
        Btn_Add(typingInfo, typingCost);
        input_typingInfo.text = null;
        input_typingCost.text = null;
        input_typingInfo.Select();
    }
    public void Btn_Add(string info, int cost)
    {
        var asset = Resources.Load<GameObject>("Prefabs/Contents/Prefab_simpleTypingPayment");
        var prefab = GameObject.Instantiate(asset, content);
        var script_easypay = prefab.GetComponent<Prefab_SimpleTypingPayment>();
        var data_payment = new Data_Payments(info, cost);
        script_easypay.btn_delete.onClick.AddListener(() => Btn_Delete_Payment(data_payment));
        script_easypay.input_PaymentInfo.text = info;
        script_easypay.input_PaymentCost.text = cost.ToString();
        prefabM_simplePayments.Add(prefab);
        script_easypay.input_PaymentInfo.onValueChanged.AddListener(x => InputInfo_ValueChanged2(prefab, prefabM_simplePayments));
        script_easypay.input_PaymentCost.onValueChanged.AddListener(x => InputCost_ValueChanged2(prefab, prefabM_simplePayments));

        var costMain = GameManager._panel_costMain;
        var asset_payment = Resources.Load<GameObject>("Prefabs/Contents/Prefab_payment");
        var obj_payment = GameObject.Instantiate(asset_payment, costMain.content_payment);
        var script_payment = obj_payment.GetComponent<Prefab_Payment>();
        costMain.LoadData_SaveList.list_payments.Add(data_payment);
        costMain.prefabM_payment.Add(obj_payment);
        script_payment.btn_delete.onClick.AddListener(() => costMain.DeletePaymentPrefab(data_payment));
        script_payment.membersForPay = new List<Data_MemberForPay>();
        script_payment.input_payInfo.text = info;
        script_payment.input_payCost.text = cost.ToString();
        foreach (var data_member in costMain.LoadData_SaveList.list_members)
        {
            var memberForPay = new Data_MemberForPay(data_member);
            data_payment.membersForPay.Add(memberForPay);
        }
        script_payment.input_payInfo.onValueChanged.AddListener(x => InputInfo_ValueChanged(obj_payment));
        script_payment.input_payCost.onValueChanged.AddListener(x => InputCost_ValueChanged(obj_payment));
    }
    public void Btn_OneClickTyping()
    {
        
        foreach (var prefab in prefabM_simplePayments)
        {
            var script = prefab.GetComponent<Prefab_SimpleTypingPayment>();

            if (!string.IsNullOrEmpty(input_typingInfo.text))
            {
                script.input_PaymentInfo.text = input_typingInfo.text;
            }
            if (!string.IsNullOrEmpty(input_typingCost.text))
            {
                script.input_PaymentCost.text = input_typingCost.text;
            }
        }
        foreach (var prefab_costMain in GameManager._panel_costMain.prefabM_payment)
        {
            var script = prefab_costMain.GetComponent<Prefab_Payment>();
            if (!string.IsNullOrEmpty(input_typingInfo.text))
            {
                script.input_payInfo.text = input_typingInfo.text;
            }
            if (!string.IsNullOrEmpty(input_typingCost.text))
            {
                script.input_payCost.text = input_typingCost.text;
            }
        }
    }
    public override void Complete()
    {
        Btn_Add();
    }
}
