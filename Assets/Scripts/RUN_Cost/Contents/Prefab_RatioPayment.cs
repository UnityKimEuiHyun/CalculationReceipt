using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Prefab_RatioPayment : PrefabBase
{
    [Header("UI_Button")]
    public Button btn_payInfo;

    [Header("UI_Text")]
    public TMP_Text txt_costofPayment;
    public TMP_Text txt_numofMember;
}
