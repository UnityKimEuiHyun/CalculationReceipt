using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Prefab_Payment : PrefabBase
{
    [Header("UI_Inputfield")]
    public TMP_InputField input_payInfo;
    public TMP_InputField input_payCost;
    public List<Data_MemberForPay> membersForPay;
    
    
   
}
