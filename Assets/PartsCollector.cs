using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsCollector : MonoBehaviour {
    public WorkingTableController table;

    void OnTriggerEnter(Collider col)
    {
        if (!table.ladderParts.Contains(col.gameObject) && col.CompareTag("Ladder"))
        {
            table.ladderParts.Add(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (!table.ladderParts.Contains(col.gameObject) && col.CompareTag("Ladder"))
        {
            table.ladderParts.Remove(col.gameObject);
        }
    }
}
