using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingDissapear : MonoBehaviour {

    public List<GameObject> pingElements;
    bool dissapear;
    float timer;
    public float dissapearTime = 1f;
    public float scaleFactor = 0.25f;

    public void Restart()
    {
        timer = 0;
        foreach (GameObject go in pingElements)
        {
            go.GetComponent<Renderer>().sharedMaterial.SetFloat("_Alpha", 1);
        }
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        Restart();
        dissapear = true;
    }

    void Update () {
		if(dissapear)
        {
            timer += Time.deltaTime;
            foreach (GameObject go in pingElements)
            {
                go.GetComponent<Renderer>().sharedMaterial.SetFloat("_Alpha", 1 - timer/dissapearTime);
            }
            transform.localScale = new Vector3(timer / dissapearTime * scaleFactor, timer / dissapearTime * scaleFactor, timer / dissapearTime * scaleFactor);
            if (timer/dissapearTime >= 1)
            {
                dissapear = false;
                enabled = false;
            }
        }
	}
}
