using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup_OtherEmail : PanelBase
{
    [Header("UI_Button")]
    public Button btn_Okay;
    public Button btn_No;
    [Header("UI_Inputfield")]
    public TMP_InputField input_otherEmailDomain;
    [Header("UI_Text")]
    public TMP_Text txt_otherEmailID;
    public TMP_Text txt_otherEmailComment;

    public override void OnInit()
    {
        base.OnInit();
        btn_Okay.onClick.AddListener(Complete);
        btn_No.onClick.AddListener(Exit);
        txt_otherEmailID.text = $"{GameManager._panel_newAccount.input_emailAccount.text}  @";
        txt_otherEmailComment.text = null;
        txt_otherEmailComment.color = GameManager._gm.color_txt_caution;
    }
    public override void Exit()
    {
        base.Exit();
        var dr = GameManager._panel_newAccount.dr_emailDomain;
        dr.value = 0;
        dr.captionText.text = dr.options[0].text;
        Destroy(backG);
    }
    public override void Complete()
    {
        base.Complete();
        if (string.IsNullOrEmpty(input_otherEmailDomain.text))
        {
            txt_otherEmailComment.text = "도메인 주소를 입력해주세요.";
        }
        else
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = input_otherEmailDomain.text;
            var dr = GameManager._panel_newAccount.dr_emailDomain;
            bool isDupl = false;
            for (int i = 0; i < dr.options.Count; i++)
            {
                if (dr.options[i].text == option.text)
                {
                    isDupl = true;
                    dr.value = i;
                    break;
                }
            }

            if (!isDupl)
            {
                dr.options.Insert(dr.options.Count - 1, option);
            }
            dr.captionText.text = option.text;
            
            Destroy(backG);
        }
    }
}
