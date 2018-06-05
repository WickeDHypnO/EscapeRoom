using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkingTableController : MonoBehaviour {

    private List<GameObject> ladderParts = new List<GameObject>();
    public GameObject LadderStep;

	void OnTriggerEnter(Collider col)
    {
        if(!ladderParts.Contains(col.gameObject) && col.CompareTag("Ladder"))
        {
            ladderParts.Add(col.gameObject);
        }
    }

    public void ObjectReleased(GameObject obj)
    {
        if (obj.name.Equals("DraggableHammer"))
        {
            if (ladderParts.Count == 3)
            {
                foreach (GameObject part in ladderParts)
                {
                    Destroy(part);
                }
                ladderParts.Clear();
                Instantiate(LadderStep, this.transform.position, Quaternion.identity);
            }

        }
    }

}
