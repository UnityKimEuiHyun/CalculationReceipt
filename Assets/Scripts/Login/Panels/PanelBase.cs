using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.IO;

public class PanelBase : MonoBehaviour
{
    public GameObject backG;
    public AccountInfo loginInfo;

    public virtual void OnInit()
    {
        var asset_login = Resources.Load<GameObject>("Prefabs/Panels/panel_backG");
        backG = GameObject.Instantiate(asset_login, GameManager._panels);
        transform.SetParent(backG.GetComponent<Transform>());
        TabList tabListScript;
        if (TryGetComponent<TabList>(out tabListScript))
        {
            tabListScript.tabList.Clear();
            Transform childTrans = GetComponent<Transform>();
            for (int i = 0; i < childTrans.childCount; i++)
            {
                TMP_InputField selectable_input;
                Button selectable_btn;
                if (childTrans.GetChild(i).TryGetComponent<TMP_InputField>(out selectable_input))
                {
                    tabListScript.tabList.Add(selectable_input);
                    tabListScript.tabList_onlyInput.Add(selectable_input);
                    selectable_input.onSelect.RemoveAllListeners();
                    selectable_input.onSelect.AddListener(x => OnClickInputfield(selectable_input));
                }
                else if (childTrans.GetChild(i).TryGetComponent<Button>(out selectable_btn))
                {
                    tabListScript.tabList.Add(selectable_btn);
                    tabListScript.tabList_onlyBtn.Add(selectable_btn);
                }
            }
            TabSwap.stack_tabList.Push(tabListScript);
            var tempList = new Stack<TabList>(TabSwap.stack_tabList);
            int count = tempList.Count;
            for (int i = 0; i < count - 1; i++)
            {
                tempList.Pop().GetComponent<PanelBase>().backG.GetComponent<Image>().color = GameManager._gm.color_backG_A0;
            }
            tabListScript.inputSelected = 0;
            if(tabListScript.tabList.Count!=0)
            tabListScript.tabList[0].Select();
        }
        GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        if(GameManager.loginInfo != null) loginInfo = GameManager.loginInfo;
    }
    public bool CheckCost_Under10()
    {
        var prefabM_p = GameManager._panel_costMain.prefabM_payment;
        foreach (var prefab in prefabM_p)
        {
            string cost = prefab.GetComponent<Prefab_Payment>().input_payCost.text;
            if (cost[cost.Length - 1] != '0')
                return false;
        }
        return true;
    }
    public bool CheckNull_Member()
    {
        var prefabM_m = GameManager._panel_costMain.prefabM_member;
        foreach (var prefab_member in prefabM_m)
        {
            if (string.IsNullOrEmpty(prefab_member.GetComponent<Prefab_Member>().input_memberName.text))
                return true;
        }
        return false;
    }
    public bool CheckNull_PaymentInfo()
    {
        var prefabM_p = GameManager._panel_costMain.prefabM_payment;
        foreach (var prefab_payment in prefabM_p)
        {
            if (string.IsNullOrEmpty(prefab_payment.GetComponent<Prefab_Payment>().input_payInfo.text))
                return true;
        }
        return false;
    }
    public bool CheckNull_PaymentCost()
    {
        var prefabM_p = GameManager._panel_costMain.prefabM_payment;
        foreach (var prefab_payment in prefabM_p)
        {
            if (string.IsNullOrEmpty(prefab_payment.GetComponent<Prefab_Payment>().input_payCost.text))
                return true;
        }
        return false;
    }
    public bool CheckNull_PaymentZero()
    {
        int sum = 0;
        foreach (var payment in GameManager._panel_costMain.LoadData_SaveList.list_payments)
        {
            sum += payment.savePaymentCost;
        }
        if (sum < 10) return true;
        return false;
    }
    public int CheckReady()
    {
        if (CheckNull_Member()) return 1;
        if (CheckNull_PaymentInfo()) return 2;
        if (CheckNull_PaymentCost()) return 3;
        if (CheckNull_PaymentZero()) return 4;
        return 0;
    }
    public void CheckCostResult()
    {
        var payments = GameManager._panel_costMain.LoadData_SaveList.list_payments;
        for (int i = 0; i < payments.Count; i++)
        {
            var payment = payments[i];
            float ratioTotal = 0;
            foreach (var memberForPay1 in payment.membersForPay)
            {
                ratioTotal += memberForPay1.ratio;
            }
            int CostResultSum = 0;
            for (int j = 0; j < payment.membersForPay.Count; j++)
            {
                var memberForPay = payment.membersForPay[j];
                var cost = int.Parse(payment.savePaymentCost.ToString());
                var ratio = memberForPay.ratio;
                if (ratio == 0)
                    memberForPay.costResult = 0;
                else if (j == payment.membersForPay.Count - 1)
                {
                    memberForPay.costResult = cost - CostResultSum;
                    if (memberForPay.ratio == 0)
                    {
                        payment.membersForPay[j - 1].costResult += memberForPay.costResult;
                        memberForPay.costResult = 0;
                    }
                }
                else
                {
                    memberForPay.costResult = IntRound(cost * ratio / ratioTotal, -1);
                    CostResultSum += memberForPay.costResult;
                }
            }
        }
        ChangeData(GameManager._panel_costMain.LoadData_SaveList);
    }
    public GameObject CreatePanel(GameObject _asset)
    {
        var prefab_login = GameObject.Instantiate(_asset, GameManager._panels);
        return prefab_login;
    }
    public void ClearChildren(Transform t)
    {
        var children = t.Cast<Transform>().ToArray();

        foreach (var child in children)
        {
            UnityEngine.Object.DestroyImmediate(child.gameObject);
        }
    }
    public void InputName_ValueChanged(GameObject prefab)
    {
        var prefabM_member = GameManager._panel_costMain.prefabM_member;
        for (int i = 0; i < prefabM_member.Count; i++)
        {
            if (prefab == prefabM_member[i])
            {
                GameManager._panel_costMain.LoadData_SaveList.list_members[i].saveMemberName = prefab.GetComponent<Prefab_Member>().input_memberName.text;
                return;
            }
        }
    }
    public void InputInfo_ValueChanged(GameObject prefab)
    {
        var prefabM_payment = GameManager._panel_costMain.prefabM_payment;
        for (int i = 0; i < prefabM_payment.Count; i++)
        {
            if (prefab == prefabM_payment[i])
            {
                GameManager._panel_costMain.LoadData_SaveList.list_payments[i].savePaymentInfo = prefab.GetComponent<Prefab_Payment>().input_payInfo.text;
                return;
            }
        }
    }
    public void InputCost_ValueChanged(GameObject prefab)
    {
        var prefabM_payment = GameManager._panel_costMain.prefabM_payment;
        for (int i = 0; i < prefabM_payment.Count; i++)
        {
            if (prefab == prefabM_payment[i])
            {
                if (string.IsNullOrEmpty(prefab.GetComponent<Prefab_Payment>().input_payCost.text))
                    GameManager._panel_costMain.LoadData_SaveList.list_payments[i].savePaymentCost = 0;
                else GameManager._panel_costMain.LoadData_SaveList.list_payments[i].savePaymentCost = int.Parse(prefab.GetComponent<Prefab_Payment>().input_payCost.text);
                return;
            }
        }
    }
    public void InputInfo_ValueChanged2(GameObject prefab, List<GameObject> prefabM)
    {
        for (int i = 0; i < prefabM.Count; i++)
        {
            if (prefab == prefabM[i])
            {
                GameManager._panel_costMain.LoadData_SaveList.list_payments[i].savePaymentInfo = prefab.GetComponent<Prefab_SimpleTypingPayment>().input_PaymentInfo.text;
                return;
            }
        }
    }
    public void InputCost_ValueChanged2(GameObject prefab, List<GameObject> prefabM)
    {
        var prefabM_payment = GameManager._panel_costMain.prefabM_payment;
        for (int i = 0; i < prefabM.Count; i++)
        {
            if (prefab == prefabM[i])
            {
                if (string.IsNullOrEmpty(prefab.GetComponent<Prefab_SimpleTypingPayment>().input_PaymentCost.text))
                    GameManager._panel_costMain.LoadData_SaveList.list_payments[i].savePaymentCost = 0;
                else GameManager._panel_costMain.LoadData_SaveList.list_payments[i].savePaymentCost = int.Parse(prefab.GetComponent<Prefab_SimpleTypingPayment>().input_PaymentCost.text);
                return;
            }
        }
    }
    public void OnClickInputfield(TMP_InputField input)
    {
        TabList tabListScript = TabSwap.stack_tabList.Peek();
        for (int i = 0; i < tabListScript.tabList.Count; i++)
        {
            if(tabListScript.tabList[i].gameObject == input.gameObject)
            {
                tabListScript.inputSelected = i;
                break;
            }
        }
    }
    public void PhoneNumValueChange(TMP_InputField input_phoneNum)
    {
        if (Input.anyKey)
        {
            StartCoroutine(changeToPhoneNum(input_phoneNum));
        }
    }
    IEnumerator changeToPhoneNum(TMP_InputField input_phoneNum)
    {
        string inputTxt = input_phoneNum.text;
        string strNum = Regex.Replace(inputTxt, @"\D", "");

        StringBuilder sb = new StringBuilder(strNum);
        if (7 > strNum.Length && strNum.Length > 3) sb.Insert(3, '-');
        else if (11 > strNum.Length && strNum.Length >= 7)
        {
            sb.Insert(3, '-');
            sb.Insert(7, '-');
        }
        else if (strNum.Length >= 11)
        {
            sb.Insert(3, '-');
            sb.Insert(8, '-');
        }

        input_phoneNum.text = sb.ToString();
        yield return new WaitForEndOfFrame();
        input_phoneNum.MoveToEndOfLine(false, false);
    }
    public void SaveData(string saveDataName)
    {
        var time = DateTime.Now.ToString(("yyyy-MM-dd_HH_mm_ss_tt"));
        var loadData = GameManager._panel_costMain.LoadData_SaveList;
        loadData.saveDataName = saveDataName;
        loadData.saveDataTime = time;
        loadData.DataCodeSet();
        loginInfo.Data_saveList.Add(loadData.DeepCopy());
        SetLoadDataNameToTitle();
        OverWriteJson();
    }
    
    public void ChangeData(int index)
    {
        var time = DateTime.Now.ToString(("yyyy-MM-dd_HH_mm_ss_tt"));
        var loadData = GameManager._panel_costMain.LoadData_SaveList;
        loadData.saveDataTime = time;
        loadData.DataCodeSet();
        loginInfo.Data_saveList[index] = loadData.DeepCopy();
        SetLoadDataNameToTitle();
        OverWriteJson();
    }
    public void ChangeData(Data_SaveList data_savelist)
    {
        int index_saveList = FindSaveListIndex(data_savelist.dataCode);
        var time = DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_tt");
        var loadData = GameManager._panel_costMain.LoadData_SaveList;
        loadData.saveDataTime = time;
        loadData.DataCodeSet();
        if(index_saveList != 9999)
            loginInfo.Data_saveList[index_saveList] = loadData.DeepCopy();
        SetLoadDataNameToTitle();
        OverWriteJson();
    }
    public void ChangeData(int index, string newSaveName)
    {
        var time = DateTime.Now.ToString(("yyyy-MM-dd_HH_mm_ss_tt"));
        var loadData = GameManager._panel_costMain.LoadData_SaveList;
        loadData.saveDataTime = time;
        loadData.saveDataName = newSaveName;
        loadData.DataCodeSet();
        loginInfo.Data_saveList[index] = loadData.DeepCopy();
        GameManager._panel_costMain.LoadData_SaveList = loadData;
        SetLoadDataNameToTitle();
        OverWriteJson();
    }
    public virtual void Exit()
    {
        TabSwap.stack_tabList.Pop();
        TabSwap.stack_tabList.Peek().GetComponent<PanelBase>().backG.GetComponent<Image>().color = GameManager._gm.color_backG_A60;
    }
    
    public virtual void Complete()
    {
        if (TabSwap.stack_tabList.Count > 1)
        {
            TabSwap.stack_tabList.Pop();
        }
        TabSwap.stack_tabList.Peek().GetComponent<PanelBase>().backG.GetComponent<Image>().color = GameManager._gm.color_backG_A60;
    }

    public void OverWriteJson()
    {
        loginInfo.ObjectToJson();
    }
    
    public virtual void DeletePrefab_saveList(int index, GameObject btnPrefab)
    {
        loginInfo.Data_saveList.RemoveAt(index);
        Destroy(btnPrefab);
        OverWriteJson();
    }
    public virtual void DeleteMemberPrefab(Data_Members data_member)
    {
        int index_member = FindMemberDataIndex(data_member.dataCode);
        var loadData = GameManager._panel_costMain.LoadData_SaveList;
        loadData.list_members.RemoveAt(index_member);
        foreach (var payment in loadData.list_payments)
        {
            payment.membersForPay.RemoveAt(index_member);
        }
        Destroy(GameManager._panel_costMain.prefabM_member[index_member]);
        GameManager._panel_costMain.prefabM_member.RemoveAt(index_member);
    }
    public virtual void DeletePaymentPrefab(Data_Payments data_payment)
    {
        int index_payment = FindPaymentDataIndex(data_payment.dataCode);
        var loadData = GameManager._panel_costMain.LoadData_SaveList;
        loadData.list_payments.RemoveAt(index_payment);
        Destroy(GameManager._panel_costMain.prefabM_payment[index_payment]);
        GameManager._panel_costMain.prefabM_payment.RemoveAt(index_payment);
    }
    
    public int FindSaveListIndex(string _dataCode)
    {
        for (int i = 0; i < loginInfo.Data_saveList.Count; i++)
        {
            if (loginInfo.Data_saveList[i].dataCode == _dataCode)
            {
                return i;
            }
        }
        return 9999;
    }
    public int FindMemberDataIndex(string _dataCode)
    {
        var members = GameManager._panel_costMain.LoadData_SaveList.list_members;
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i].dataCode == _dataCode)
            {
                return i;
            }
        }
        return 9999;
    }
    public int FindPaymentDataIndex(string _dataCode)
    {
        var payments = GameManager._panel_costMain.LoadData_SaveList.list_payments;
        for (int i = 0; i < payments.Count; i++)
        {
            if (payments[i].dataCode == _dataCode)
            {
                return i;
            }
        }
        return 9999;
    }
    public void SetLoadDataNameToTitle()
    {
        if (GameManager._panel_costMain.LoadData_SaveList == null)
        {
            GameManager._panel_costMain.txt_loadDataName.text = "Not Saved Data";
            return;
        }
        GameManager._panel_costMain.txt_loadDataName.text = GameManager._panel_costMain.LoadData_SaveList.saveDataName;
    }
    public int IntRound(float Value, int Digit)
    {
        double Temp = Math.Pow(10.0, Digit);
        return (int)(Math.Round(Value * Temp) / Temp);
    }
}

