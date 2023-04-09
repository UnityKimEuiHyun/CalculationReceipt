using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup_Comment : PanelBase
{
    public enum CommentType { simpleComment, CompleteNewAccount, LoginFailure};
    [Header("UI_Text")]
    public TMP_Text txt_title;
    public TMP_Text txt_Comment;
    [Header("UI_Button")]
    public Button btn_Okay;
    string title;
    string comment;
    int moveNum;
    CommentType commentType;

    public void OnInit(CommentType _commentType, string _title, string _comment) 
    {
        base.OnInit();
        title = _title;
        comment = _comment;
        commentType = _commentType;
        SetText();
        btn_Okay.onClick.AddListener(Complete);
        moveNum = 0;
    }
    public void OnInit(CommentType _commentType, string _title, string _comment, int _moveNum)
    {
        OnInit(_commentType, _title, _comment);
        moveNum = _moveNum;
    }
    void SetText()
    {
        txt_title.text = title;
        txt_Comment.text = comment;
        switch (commentType)
        {
            case CommentType.CompleteNewAccount:
                txt_Comment.color = GameManager._gm.color_txt_normal;
                break;
            case CommentType.simpleComment:
                txt_Comment.color = GameManager._gm.color_txt_caution;
                break;
            case CommentType.LoginFailure:
                txt_Comment.color = GameManager._gm.color_txt_caution;
                break;
            default: break;
        }
    }
    public override void Exit()
    {
        Complete();
    }

    public override void Complete()
    {
        base.Complete();
        switch (commentType)
        {
            case CommentType.simpleComment:
                TabSwap.stack_tabList.Peek().ChangeInputNumAndSelect(moveNum);
                break;
            case CommentType.CompleteNewAccount:
                GameManager._loginPanel.input_loginId.text = GameManager._panel_newAccount.FinalID;
                GameManager._panel_newAccount.Exit();
                TabSwap.stack_tabList.Peek().SelectInputField(1);
                break;
            case CommentType.LoginFailure:
                TabSwap.stack_tabList.Peek().SelectInputField(0);
                break;
            default: break;
        }
        Destroy(backG);
    }
}
