using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllAccount
{
    public List<AccountInfo> allAccount;

    public AllAccount()
    {
        allAccount = new List<AccountInfo>();
    }
}
