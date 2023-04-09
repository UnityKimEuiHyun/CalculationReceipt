using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;


public class PasswordValidator
{
    string pw;
    public bool _okStrength;

    public string Comment
    {
        get {
            if (!CheckChar()) return "PW에 영문자를 조합해주세요.";
            else if (!CheckNum()) return "PW에 숫자를 조합해주세요.";
            else if (!CheckSym()) return "PW에 특수문자를 조합해주세요.";
            else if (!CheckLength()) return "PW는 8자 이상, 15자 이하의 조합이어야 합니다.";
            /*else if (!CheckSimple()) return "입력된 PW는 보안에 너무 취약합니다. 다른 PW를 입력해주세요.";*/
            else
            {
                _okStrength = true;
                return "PW is Confirmed";
            }
        }
    }
    public PasswordValidator(string password)
    {
        pw = password;
        _okStrength = false;
    }
    bool CheckChar()
    {
        Regex enRegex = new Regex(@"[a-zA-Z]");
        return enRegex.IsMatch(pw);
    }
    bool CheckNum()
    {
        Regex numRegex = new Regex(@"[0-9]");
        return numRegex.IsMatch(pw);
    }
    bool CheckSym()
    {
        Regex specialRegex = new Regex(@"[~!@\#$%^&*\()\=+|\\/:;?""<>']");
        return specialRegex.IsMatch(pw);
    }
    bool CheckLength()
    {
        return pw.Length > 7 && pw.Length < 16;
    }
    /*
    bool CheckSimple()
    {
        return !passwordExists(pw);
    }
    private bool passwordExists(string password)
    {
        IEnumerable<string> lines = File.ReadLines("Assets\\Scripts\\PasswordValidator\\millionpassword.txt");
        foreach (string line in lines)
        {
            if (password == line)
            {
                return true; 
            }
        }
        return false;
    }*/
}

