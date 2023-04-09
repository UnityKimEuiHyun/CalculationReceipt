using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Prefab_ResultOfPayment : MonoBehaviour
{
    [Header("UI_Text")]
    public TMP_Text txt_payInfo;
    public TMP_Text txt_payCost;

    [Header("UI_Content")]
    public Transform content_MemberForPay;
}
