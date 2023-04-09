using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

public class IDValidator
{
    string id;
    public bool OkChar
    {
        get
        {
            Regex enRegex = new Regex(@"[a-zA-Z]");
            if (enRegex.IsMatch(id))
                return true;
            else return false;
        }
    }
    public bool OkNum
    {
        get
        {
            Regex numRegex = new Regex(@"[0-9]");
            if (numRegex.IsMatch(id))
                return true;
            else return false;
        }
    }
    public bool OkLength
    {
        get
        {
            if (id.Length > 3) //4ÀÚ ÀÌ»ף
                return true;
            else return false;
        }
    }
    public IDValidator(string text_id)
    {
        id = text_id;
    }
}
