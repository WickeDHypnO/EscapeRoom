using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PingDissapear : Photon.PunBehaviour {

    public List<GameObject> pingElements;
    bool dissapear;
    float timer;
    public float dissapearTime = 1f;
    public float scaleFactor = 0.25f;
    public PingIndicator indicator;
    public Color firstPlayerColor;
    public Color secondPlayerColor;
    public bool debugSecondPlayer;

    public void Restart()
    {
        photonView.RPC("RpcShowPing", PhotonTargets.All);
    }

    private void OnEnable()
    {
        if(debugSecondPlayer)
        {
            foreach(Image i in indicator.GetComponentsInChildren<Image>(true))
            {
                i.color = secondPlayerColor;
            }
            pingElements[0].GetComponent<Renderer>().sharedMaterial.SetColor("_Color", secondPlayerColor);
        }
        else
        {
            foreach (Image i in indicator.GetComponentsInChildren<Image>(true))
            {
                i.color = firstPlayerColor;
            }
            pingElements[0].GetComponent<Renderer>().sharedMaterial.SetColor("_Color", firstPlayerColor);
        }
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
                foreach (GameObject go in pingElements)
                {
                    go.GetComponent<Renderer>().sharedMaterial.SetFloat("_Alpha", 0);
                }
                dissapear = false;
                gameObject.SetActive(false);
            }
        }
	}

    [PunRPC]
    void RpcShowPing()
    {
        timer = 0;
        foreach (GameObject go in pingElements)
        {
            go.GetComponent<Renderer>().sharedMaterial.SetFloat("_Alpha", 1);
        }
        transform.localScale = Vector3.zero;
        indicator.StartShowing(dissapearTime);
    }
}
