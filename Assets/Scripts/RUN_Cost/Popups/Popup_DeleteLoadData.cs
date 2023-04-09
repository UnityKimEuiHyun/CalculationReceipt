using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup_DeleteLoadData : PanelBase
{
    [Header("UI_Text")]
    public TMP_Text txt_comment;
    [Header("UI_Button")]
    public Button btn_yes;
    public Button btn_no;

    public void OnInit(Data_SaveList deleteData, GameObject prefab)
    {
        base.OnInit();
        txt_comment.text = $"현재 열려있는 데이터입니다.\n삭제하시겠습니까";
        txt_comment.color = GameManager._gm.color_txt_caution;
        var saveName = deleteData.saveDataName;
        var saveTime = deleteData.saveDataTime;
        btn_yes.onClick.AddListener(() => Complete(prefab));
        btn_no.onClick.AddListener(Exit);
    }
    public void Complete(GameObject prefab)
    {
        base.Complete();
        int index_saveList = FindSaveListIndex(GameManager._panel_costMain.LoadData_SaveList.dataCode);
        if (index_saveList != 9999)
        {
            GameManager._panel_costMain.LoadData_SaveList = GameManager._panel_costMain.LoadData_SaveList.DeepCopy();
            GameManager._panel_saveList.prefabM_savedData.Remove(prefab);
            DeletePrefab_saveList(index_saveList, prefab);
        }
        Destroy(backG);
    }

    public override void Exit()
    {
        base.Exit();
        Destroy(backG);
    }
}
