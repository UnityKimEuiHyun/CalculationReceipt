using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class Panel_Result : PanelBase
{
    [Header("UI_Text")]
    public TMP_Text txt_title;
    public TMP_Text txt_totalMember;
    public TMP_Text txt_totalCost;
    public TMP_Text txt_totalCostCount;

    [Header("UI_Content")]
    public Transform content_Member;
    public Transform content_Payment;

    [Header("UI_Inputfield")]
    public TMP_InputField input_etc;
    public TMP_InputField input_UserComment;

    [Header("UI_Button")]
    public Button btn_Back;
    public Button btn_screenShot;
    public Button btn_hideMember;
    public Button btn_hidePayment;
    public Button btn_hideUserComment;

    [Header("UI_ButtonPanel")]
    public GameObject Panel_memberResult;
    public GameObject Panel_paymentResult;
    public HorizontalLayoutGroup sizeRearange;
    public GameObject Btns;
    public GameObject gameobject_UserComment;

    [Header("UI_RectTransform")]
    public RectTransform sizeOfResult;

    public Data_SaveList _LoadData;

    [Header("UI_Scrollbar Vertical")]
    public Scrollbar scrollbar_V;

    [Header("UI_Slider")]
    public Slider slider_resultSize;

    public override void OnInit()
    {
        base.OnInit();
        _LoadData = GameManager._panel_costMain.LoadData_SaveList;
        btn_Back.onClick.AddListener(Exit);
        btn_screenShot.onClick.AddListener(Btn_ScreenShoot);
        input_UserComment.onValueChanged.AddListener(x => OnValueChange_userComment());
        input_etc.onValueChanged.AddListener(x => OnValueChange_etc());
        btn_hideMember.onClick.AddListener(Btn_HideMember);
        btn_hidePayment.onClick.AddListener(Btn_HidePayment);
        btn_hideUserComment.onClick.AddListener(Btn_HideUserComment);
        slider_resultSize.onValueChanged.AddListener(x => OnValueChange_ResultSizeSlider());
        

        SetInfo();
    }
    public void Btn_ScreenShoot()
    {
        Btns.SetActive(false);
        StartCoroutine(Screenshot());
    }
    IEnumerator Screenshot()
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        string name = $"Screenshot_{System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.png";

        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + name, bytes);
        Destroy(texture);

        Btns.SetActive(true);
        yield return new WaitForEndOfFrame();

        var asset_comment_m = Resources.Load<GameObject>("Prefabs/Popups/popup_Comment");
        var popup_comment_m = CreatePanel(asset_comment_m);
        popup_comment_m.GetComponent<Popup_Comment>().OnInit(Popup_Comment.CommentType.simpleComment, "스크린샷 완료", $"[C:/Users/[user name]/AppData/LocalLow/EHCompany\n위 경로에 {name} 스크린샷이 저장되었습니다.");
    }
    void Btn_HideMember()
    {
        if(Panel_memberResult.activeSelf)
        {
            Panel_memberResult.SetActive(false);
            btn_hideMember.GetComponent<Image>().color = GameManager._gm.color_btn_complete;
            var btn_text = btn_hideMember.GetComponentInChildren<TMP_Text>();
            btn_text.color = GameManager._gm.color_txt_normal;
            btn_text.text = "인원별 정산 숨기기 취소";
        }
        else
        {
            Panel_memberResult.SetActive(true);
            btn_hideMember.GetComponent<Image>().color = GameManager._gm.color_btn_caution;
            var btn_text = btn_hideMember.GetComponentInChildren<TMP_Text>();
            btn_text.color = GameManager._gm.color_txt_white;
            btn_text.text = "인원별 정산 숨기기";
        }
        StartCoroutine(SetRearange());
    }
    void Btn_HidePayment()
    {
        if (Panel_paymentResult.activeSelf)
        {
            Panel_paymentResult.SetActive(false);
            btn_hidePayment.GetComponent<Image>().color = GameManager._gm.color_btn_complete;
            var btn_text = btn_hidePayment.GetComponentInChildren<TMP_Text>();
            btn_text.color = GameManager._gm.color_txt_normal;
            btn_text.text = "지출별 정산 숨기기 취소";
        }
        else
        {
            Panel_paymentResult.SetActive(true);
            btn_hidePayment.GetComponent<Image>().color = GameManager._gm.color_btn_caution;
            var btn_text = btn_hidePayment.GetComponentInChildren<TMP_Text>();
            btn_text.color = GameManager._gm.color_txt_white;
            btn_text.text = "지출별 정산 숨기기";
        }
        StartCoroutine(SetRearange());
    }
    void Btn_HideUserComment()
    {
        if (gameobject_UserComment.activeSelf)
        {
            gameobject_UserComment.SetActive(false);
            btn_hideUserComment.GetComponent<Image>().color = GameManager._gm.color_btn_complete;
            var btn_text = btn_hideUserComment.GetComponentInChildren<TMP_Text>();
            btn_text.color = GameManager._gm.color_txt_normal;
            btn_text.text = "특이사항 숨기기 취소";
        }
        else
        {
            gameobject_UserComment.SetActive(true);
            btn_hideUserComment.GetComponent<Image>().color = GameManager._gm.color_btn_caution;
            var btn_text = btn_hideUserComment.GetComponentInChildren<TMP_Text>();
            btn_text.color = GameManager._gm.color_txt_white;
            btn_text.text = "특이사항 정산 숨기기";
            StartCoroutine(ScrollToBottom());
        }
    }
    IEnumerator ScrollToBottom()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        scrollbar_V.GetComponent<Scrollbar>().value = 0;
    }
    IEnumerator SetRearange()
    {
        sizeRearange.childAlignment = TextAnchor.UpperLeft;
        yield return new WaitForEndOfFrame();
        sizeRearange.childAlignment = TextAnchor.UpperCenter;
        yield return null;
    }
    public void SetInfo()
    {
        txt_title.text = _LoadData.saveDataName;
        txt_totalMember.text = _LoadData.list_members.Count.ToString();
        txt_totalCost.text = SUM_Cost().ToString();
        txt_totalCostCount.text = _LoadData.list_payments.Count.ToString();
        if(_LoadData.list_members.Count==0)
        {
            
            var asset_member = Resources.Load<GameObject>("Prefabs/Result/prefab_ResultofMember");
            var obj_member = GameObject.Instantiate(asset_member, content_Member);
            var script_member = obj_member.GetComponent<Prefab_ResultOfMember>();
            script_member.txt_index.text = "0";
            script_member.txt_memberName.text = "인원 없음";
            script_member.txt_Cost.text = "-";
        }
        else
        {
            int sum_member = 0;
            for (int i = 0; i < _LoadData.list_members.Count; i++)
            {
                var member = _LoadData.list_members[i];
                var asset_member = Resources.Load<GameObject>("Prefabs/Result/prefab_ResultofMember");
                var obj_member = GameObject.Instantiate(asset_member, content_Member);
                var script_member = obj_member.GetComponent<Prefab_ResultOfMember>();
                int index = i;
                script_member.txt_index.text = index.ToString();
                script_member.txt_memberName.text = member.saveMemberName;
                script_member.txt_Cost.text = SUM_CostByMember(member).ToString();
                sum_member += SUM_CostByMember(member);
            }
            var asset_endMember = Resources.Load<GameObject>("Prefabs/Result/prefab_EndofMember");
            var obj_endMember = GameObject.Instantiate(asset_endMember, content_Member);
            var script_endMember = obj_endMember.GetComponent<Prefab_EndOfMember>();
            script_endMember.txt_sumMember.text = sum_member.ToString();
        }
        if(_LoadData.list_payments.Count == 0 )
        {
            var asset_payment = Resources.Load<GameObject>("Prefabs/Result/prefab_ResultofPayment");
            var obj_payment = GameObject.Instantiate(asset_payment, content_Payment);
            var script_pay = obj_payment.GetComponent<Prefab_ResultOfPayment>();
            script_pay.txt_payInfo.text = "지출 정보 없음";
            script_pay.txt_payCost.text = "-";

            var asset_memberForPay = Resources.Load<GameObject>("Prefabs/Result/prefab_MemberForPay");
            var obj_memberForPay = GameObject.Instantiate(asset_memberForPay, script_pay.content_MemberForPay);
            var script_memberForPay = obj_memberForPay.GetComponent<Prefab_MemberForPay>();
            script_memberForPay.txt_index.text = "0";
            script_memberForPay.txt_memberName.text = "인원 없음";
            script_memberForPay.txt_divCost.text = "-";
        }
        else
        {
            int sum_paymentCost = 0;
            for (int i = 0; i < _LoadData.list_payments.Count; i++)
            {
                var payment = _LoadData.list_payments[i];
                var asset_payment = Resources.Load<GameObject>("Prefabs/Result/prefab_ResultofPayment");
                var obj_payment = GameObject.Instantiate(asset_payment, content_Payment);
                var script_pay = obj_payment.GetComponent<Prefab_ResultOfPayment>();
                script_pay.txt_payInfo.text = payment.savePaymentInfo;
                script_pay.txt_payCost.text = payment.savePaymentCost.ToString();
                if(payment.membersForPay.Count==0)
                {
                    var asset_memberForPay = Resources.Load<GameObject>("Prefabs/Result/prefab_MemberForPay");
                    var obj_memberForPay = GameObject.Instantiate(asset_memberForPay, script_pay.content_MemberForPay);
                    var script_memberForPay = obj_memberForPay.GetComponent<Prefab_MemberForPay>();
                    script_memberForPay.txt_index.text = "0";
                    script_memberForPay.txt_memberName.text = "인원 없음";
                    script_memberForPay.txt_divCost.text = "-";
                }
                else
                {
                    for (int j = 0; j < payment.membersForPay.Count; j++)
                    {
                        var memberForPay = payment.membersForPay[j];
                        var asset_memberForPay = Resources.Load<GameObject>("Prefabs/Result/prefab_MemberForPay");
                        var obj_memberForPay = GameObject.Instantiate(asset_memberForPay, script_pay.content_MemberForPay);
                        var script_memberForPay = obj_memberForPay.GetComponent<Prefab_MemberForPay>();
                        script_memberForPay.txt_index.text = j.ToString();
                        script_memberForPay.txt_memberName.text = memberForPay.data_member.saveMemberName;
                        script_memberForPay.txt_divCost.text = memberForPay.costResult.ToString();
                        sum_paymentCost += memberForPay.costResult;
                        if (script_memberForPay.txt_divCost.text == "0")
                        Destroy(obj_memberForPay);
                    }
                }
                if (script_pay.txt_payCost.text == "0")
                    Destroy(obj_payment);
            }
            
            var asset_endPayment = Resources.Load<GameObject>("Prefabs/Result/prefab_EndofPayment");
            var obj_endPayment = GameObject.Instantiate(asset_endPayment, content_Payment);
            var script_endPay = obj_endPayment.GetComponent<Prefab_EndOfPayment>();
            script_endPay.txt_sumPayment.text = sum_paymentCost.ToString();
        }
        input_UserComment.text = _LoadData.userComment;
        input_etc.text = _LoadData.etc;
    }
    public int SUM_CostByMember(Data_Members data_member)
    {
        int sum_CostByMember = 0;
        foreach (var payment in _LoadData.list_payments)
        {
            foreach (var member in payment.membersForPay)
            {
                if(data_member == member.data_member)
                {
                    sum_CostByMember += member.costResult;
                }
            }
        }
        return sum_CostByMember;
    }
    public int SUM_Cost()
    {
        int sum_Cost = 0;
        foreach (var payment in _LoadData.list_payments)
        {
            sum_Cost += payment.savePaymentCost;
        }
        return sum_Cost;
    }
    public override void Exit()
    {
        base.Exit();
        Destroy(backG);
    }
    public void OnValueChange_etc()
    {
        _LoadData.userComment = input_etc.text;
    }
    public void OnValueChange_userComment()
    {
        if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            StartCoroutine(MoveToEndOfText());
        }
        _LoadData.userComment = input_UserComment.text;
    }
    IEnumerator MoveToEndOfText()
    {
        input_UserComment.MoveTextEnd(true);
        yield return new WaitForSecondsRealtime(0.1f);
        scrollbar_V.GetComponent<Scrollbar>().value = 0;
    }
    public void OnValueChange_ResultSizeSlider()
    {
        var sliderValue = new Vector2(slider_resultSize.value, slider_resultSize.value);
        sizeOfResult.localScale = sliderValue;
    }
}
