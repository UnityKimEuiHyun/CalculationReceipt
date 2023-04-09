using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Panel_SaveList : PanelBase
{
    [Header("UI_Content")]
    public Transform content;
    [Header("UI_Btn")]
    public Button btn_deleteAll;
    public Button btn_close;
    public Button btn_changeName;
    public Button btn_changeNameComp;
    public Button btn_changeSeq;
    public Button btn_changeSeqComp;
    public Button btn_UpTop, btn_Up, btn_Down, btn_DownBottom;

    public GameObject Panel_Normal;
    public GameObject Panel_ChangeName;
    public GameObject Panel_ChangeSeq;
    
    [Header("UI_Text")]
    public TMP_Text txt_title;
    public TMP_Text txt_comment;


    public List<GameObject> prefabM_savedData;

    public Queue<Prefab_SaveList> que_SelectedData;

    public override void OnInit()
    {
        base.OnInit();
        prefabM_savedData = new List<GameObject>();
        GameManager._panel_saveList = this;
        btn_close.onClick.AddListener(Exit);
        btn_deleteAll.onClick.AddListener(DeleteAll_SaveList);
        btn_changeName.onClick.AddListener(Btn_ChangeName);
        btn_changeNameComp.onClick.AddListener(Btn_ChangeNameComplete);
        btn_changeSeq.onClick.AddListener(Btn_ChangeSeq);
        btn_changeSeqComp.onClick.AddListener(Btn_ChangeSeqComp);
        Panel_Normal.SetActive(true);
        Panel_ChangeName.SetActive(false);
        Panel_ChangeSeq.SetActive(false);
        btn_UpTop.onClick.AddListener(Btn_UptoTop);
        btn_Up.onClick.AddListener(Btn_Up);
        btn_Down.onClick.AddListener(Btn_Down);
        btn_DownBottom.onClick.AddListener(Btn_DowntoBottom);
        txt_title.text = "저장된 데이터 목록";
        txt_comment.text = "저장된 데이터 목록을 클릭하면 데이터를 열 수 있습니다.";
        que_SelectedData = new Queue<Prefab_SaveList>();
        SetList();
    }
    public void SetList()
    {
        ClearChildren(content);
        prefabM_savedData.Clear();
        var saveList = loginInfo.Data_saveList;
        for (int i = 0; i < saveList.Count; i++)
        {
            var asset = Resources.Load<GameObject>("Prefabs/Contents/Prefab_saveList");
            var prefab = GameObject.Instantiate(asset, content);
            var script = prefab.GetComponent<Prefab_SaveList>();
            var saveName = saveList[i].saveDataName;
            var saveTime = saveList[i].saveDataTime;
            script.txt_saveName.text = saveName;
            script.txt_savedTime.text = saveTime;
            var saveData = saveList[i];
            script.btn_saveName.onClick.AddListener(() => Btn_SaveName(saveData));
            var index = i;
            script.index_btn = index;
            script.btn_delete.onClick.AddListener(() => Btn_Delete(index, prefab, saveData));
            script.input_saveName.gameObject.SetActive(false);
            script.btn_select.gameObject.SetActive(false);
            script.btn_select.onClick.AddListener(()=>Btn_Select(index));
            script.saveListData = saveData;
            prefabM_savedData.Add(prefab);
            if (GameManager._panel_costMain.LoadData_SaveList != null
                && saveData.dataCode == GameManager._panel_costMain.LoadData_SaveList.dataCode)
            {
                script.btn_saveName.GetComponent<Image>().color = GameManager._gm.color_btn_complete;
                script.input_saveName.GetComponent<Image>().color = GameManager._gm.color_btn_complete;
            }
        }
    }
    public void Btn_Select(int index_button)
    {
        if(que_SelectedData.Count != 0)
        {
            que_SelectedData.Peek().btn_select.GetComponent<Image>().color = GameManager._gm.color_btn_unselected;
            que_SelectedData.Dequeue();
        }
        que_SelectedData.Enqueue(prefabM_savedData[index_button].GetComponent<Prefab_SaveList>());
        que_SelectedData.Peek().btn_select.GetComponent<Image>().color = GameManager._gm.color_btn_selected;
    }
    public void Btn_UptoTop()
    {
        if (que_SelectedData.Count == 0) return;
        var index_btn = que_SelectedData.Peek().index_btn;
        if (index_btn == 0) return;
        loginInfo.Data_saveList.RemoveAt(index_btn);
        loginInfo.Data_saveList.Insert(0, que_SelectedData.Peek().saveListData);
        que_SelectedData.Dequeue();
        SetList();
        OverWriteJson();
        Btn_ChangeSeq();
        Btn_Select(0);
    }
    public void Btn_Up()
    {
        if (que_SelectedData.Count == 0) return;
        var index_btn = que_SelectedData.Peek().index_btn;
        if (index_btn == 0) return;
        loginInfo.Data_saveList.RemoveAt(index_btn);
        loginInfo.Data_saveList.Insert(index_btn - 1, que_SelectedData.Peek().saveListData);
        que_SelectedData.Dequeue();
        SetList();
        OverWriteJson();
        Btn_ChangeSeq();
        Btn_Select(index_btn - 1);
    }
    public void Btn_Down()
    {
        if (que_SelectedData.Count == 0) return;
        var index_btn = que_SelectedData.Peek().index_btn;
        if (index_btn == prefabM_savedData.Count - 1) return;
        loginInfo.Data_saveList.RemoveAt(index_btn);
        loginInfo.Data_saveList.Insert(index_btn + 1, que_SelectedData.Peek().saveListData);
        que_SelectedData.Dequeue();
        SetList();
        OverWriteJson();
        Btn_ChangeSeq();
        Btn_Select(index_btn + 1);
    }
    public void Btn_DowntoBottom()
    {
        if (que_SelectedData.Count == 0) return;
        var index_btn = que_SelectedData.Peek().index_btn;
        if (index_btn == prefabM_savedData.Count - 1) return;
        loginInfo.Data_saveList.RemoveAt(index_btn);
        loginInfo.Data_saveList.Add(que_SelectedData.Peek().saveListData);
        que_SelectedData.Dequeue();
        SetList();
        OverWriteJson();
        Btn_ChangeSeq();
        Btn_Select(prefabM_savedData.Count-1);
    }
    public void Btn_ChangeName()
    {
        txt_title.text = "저장된 데이터 이름 변경";
        txt_comment.text = "데이터의 이름을 변경한 뒤 완료 버튼을 누르세요.";
        foreach (var gameObj in prefabM_savedData)
        {
            var script = gameObj.GetComponent<Prefab_SaveList>();
            script.input_saveName.gameObject.SetActive(true);
            script.input_saveName.text = script.btn_saveName.GetComponentInChildren<TMP_Text>().text;
        }
        Panel_Normal.SetActive(false);
        Panel_ChangeName.SetActive(true);
        Panel_ChangeSeq.SetActive(false);
    }
    public void Btn_ChangeNameComplete()
    {
        txt_title.text = "저장된 데이터 목록";
        txt_comment.text = "저장된 데이터 목록을 클릭하면 데이터를 열 수 있습니다.";
        for (int i = 0; i < prefabM_savedData.Count; i++)
        {
            var script = prefabM_savedData[i].GetComponent<Prefab_SaveList>();
            script.input_saveName.gameObject.SetActive(false);
            var saveName = script.input_saveName.text;
            script.btn_saveName.GetComponentInChildren<TMP_Text>().text = saveName;
            loginInfo.Data_saveList[i].saveDataName = saveName;
        }
        int index = FindSaveListIndex(GameManager._panel_costMain.LoadData_SaveList.dataCode);
        if(index != 9999)
        GameManager._panel_costMain.LoadData_SaveList.saveDataName = loginInfo.Data_saveList[index].saveDataName;
        SetLoadDataNameToTitle();
        Panel_Normal.SetActive(true);
        Panel_ChangeName.SetActive(false);
        Panel_ChangeSeq.SetActive(false);
        OverWriteJson();
    }
    public void Btn_ChangeSeq()
    {
        txt_title.text = "저장된 데이터 순서 변경";
        txt_comment.text = "순서를 변경할 데이터를 선택한 뒤 위치 변경 버튼을 누르면 위치를 변경할 수 있습니다.";
        foreach (var gameObj in prefabM_savedData)
        {
            var script = gameObj.GetComponent<Prefab_SaveList>();
            script.btn_select.gameObject.SetActive(true);
        }
        Panel_Normal.SetActive(false);
        Panel_ChangeName.SetActive(false);
        Panel_ChangeSeq.SetActive(true);
    }
    public void Btn_ChangeSeqComp()
    {
        txt_title.text = "저장된 데이터 목록";
        txt_comment.text = "저장된 데이터 목록을 클릭하면 데이터를 열 수 있습니다.";
        foreach (var gameObj in prefabM_savedData)
        {
            var script = gameObj.GetComponent<Prefab_SaveList>();
            script.btn_select.gameObject.SetActive(false);
        }
        if (que_SelectedData.Count != 0)
        {
            que_SelectedData.Peek().btn_select.GetComponent<Image>().color = GameManager._gm.color_btn_unselected;
            que_SelectedData.Dequeue();
        }
        Panel_Normal.SetActive(true);
        Panel_ChangeName.SetActive(false);
        Panel_ChangeSeq.SetActive(false);
    }
    
    void Btn_Delete(int index, GameObject btnPrefab, Data_SaveList deleteData)
    {
        if (GameManager._panel_costMain.LoadData_SaveList.dataCode == deleteData.dataCode)
        {
            var asset_comment = Resources.Load<GameObject>("Prefabs/Popups/popup_DeleteLoadData");
            var popup_comment = CreatePanel(asset_comment);
            popup_comment.GetComponent<Popup_DeleteLoadData>().OnInit(deleteData, btnPrefab);
            return;
        }
        SetLoadDataNameToTitle();
        prefabM_savedData.Remove(btnPrefab);
        DeletePrefab_saveList(index, btnPrefab);
    }
    public void Btn_SaveName(Data_SaveList data_saveList)
    {
        if (GameManager._panel_costMain.LoadData_SaveList == null)
        {
            LoadData(data_saveList);
            return;
        }
        if (GameManager._panel_costMain.LoadData_SaveList.dataCode == data_saveList.dataCode)
        {
            var asset_comment1 = Resources.Load<GameObject>("Prefabs/Popups/popup_LoadCaution");
            var popup_comment1 = CreatePanel(asset_comment1);
            popup_comment1.GetComponent<Popup_LoadCaution>().OnInit(data_saveList);
            return;
        }
        GameManager._panel_saveList.LoadData(data_saveList);
    }
    
    public void DeleteAll_SaveList()
    {
        var asset_deleteAll = Resources.Load<GameObject>("Prefabs/Popups/popup_DeleteAll");
        var popup_deleteAll = CreatePanel(asset_deleteAll);
        var script = popup_deleteAll.GetComponent<Popup_DeleteAll>();
        script.OnInit("저장 데이터 전체 삭제", "정말 모든 저장 데이터를 삭제하시겠습니까", content);
        script.btn_yes.onClick.AddListener(deleteAll_loginData_SaveList);
    }
    public void deleteAll_loginData_SaveList()
    {
        loginInfo.Data_saveList.Clear();
        OverWriteJson();
    }
    public void LoadData(Data_SaveList data_saveList)
    {
        if (loginInfo.Data_saveList.Count == 0) return;
        var dataCode = data_saveList.dataCode;
        //일치 saveList 찾기
        var findIndex = FindSaveListIndex(dataCode);
        if (findIndex == 9999) return;
        
        //데이터 정보 로드(열린 데이터 표시)
        GameManager._panel_costMain.LoadData_SaveList = loginInfo.Data_saveList[findIndex].DeepCopy();
        SetLoadDataNameToTitle();

        var saveList = GameManager._panel_costMain.LoadData_SaveList;
        var costMainPanel = GameManager._panel_costMain;
        ClearChildren(costMainPanel.content_member);
        costMainPanel.prefabM_member.Clear();
        ClearChildren(costMainPanel.content_payment);
        costMainPanel.prefabM_payment.Clear();
        foreach (var member in saveList.list_members)
        {
            var asset_member = Resources.Load<GameObject>("Prefabs/Contents/Prefab_member");
            var obj_member = GameObject.Instantiate(asset_member, costMainPanel.content_member);
            var script = obj_member.GetComponent<Prefab_Member>();
            costMainPanel.prefabM_member.Add(obj_member);
            script.btn_delete.onClick.AddListener(() => costMainPanel.DeleteMemberPrefab(member));
            script.input_memberName.text = member.saveMemberName;
            script.input_memberName.onValueChanged.AddListener(x => InputName_ValueChanged(obj_member));
        }
        foreach (var payment in saveList.list_payments)
        {
            var asset_payment = Resources.Load<GameObject>("Prefabs/Contents/Prefab_payment");
            var obj_payment = GameObject.Instantiate(asset_payment, costMainPanel.content_payment);
            var script = obj_payment.GetComponent<Prefab_Payment>();
            costMainPanel.prefabM_payment.Add(obj_payment);
            script.btn_delete.onClick.AddListener(() => DeletePaymentPrefab(payment));
            script.input_payInfo.text = payment.savePaymentInfo;
            script.input_payCost.text = payment.savePaymentCost.ToString();
            script.membersForPay = payment.membersForPay;
            script.input_payInfo.onValueChanged.AddListener(x => InputInfo_ValueChanged(obj_payment));
            script.input_payCost.onValueChanged.AddListener(x => InputCost_ValueChanged(obj_payment));
        }
        Exit();
    }
    public override void Exit()
    {
        base.Exit();
        GameManager._panel_saveList = null;
        Destroy(backG);
    }

}
