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
            if (!CheckChar()) return "PW�� �����ڸ� �������ּ���.";
            else if (!CheckNum()) return "PW�� ���ڸ� �������ּ���.";
            else if (!CheckSym()) return "PW�� Ư�����ڸ� �������ּ���.";
            else if (!CheckLength()) return "PW�� 8�� �̻�, 15�� ������ �����̾�� �մϴ�.";
            /*else if (!CheckSimple()) return "�Էµ� PW�� ���ȿ� �ʹ� ����մϴ�. �ٸ� PW�� �Է����ּ���.";*/
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

