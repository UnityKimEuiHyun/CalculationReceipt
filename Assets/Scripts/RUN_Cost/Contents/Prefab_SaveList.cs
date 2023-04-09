using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Prefab_SaveList : PrefabBase
{
    [Header("UI_Button")]
    public Button btn_saveName;
    public Button btn_select;

    [Header("UI_Text")]
    public TMP_Text txt_saveName;
    public TMP_Text txt_savedTime;

    [Header("UI_Inputfield")]
    public TMP_InputField input_saveName;

    public Data_SaveList saveListData;
    public int index_btn;
    

    
}
