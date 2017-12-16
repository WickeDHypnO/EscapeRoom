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
    public Material firstPlayerMaterial;
    public Color secondPlayerColor;
    public Material secondPlayerMaterial;
    public bool debugSecondPlayer;

    public void OnEnable()
    {
        if (photonView.isMine)
        {
            photonView.RPC("RpcShowPing", PhotonTargets.All);
        }
    }

    private void SetColor()
    {
        if(!PhotonNetwork.isMasterClient && !debugSecondPlayer)
        {
            debugSecondPlayer = true;
        }
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
                //gameObject.SetActive(false);
                if(photonView.isMine)
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
	}

    [PunRPC]
    void RpcShowPing()
    {
        Show();
    }

    public void Show()
    {
        indicator = FindObjectOfType<Pinger>().indicator;
        if (photonView.owner.IsMasterClient)
        {
            pingElements[0].GetComponent<Renderer>().sharedMaterial = firstPlayerMaterial;
        }
        else
        {
            pingElements[0].GetComponent<Renderer>().sharedMaterial = secondPlayerMaterial;
        }
        timer = 0;
        foreach (GameObject go in pingElements)
        {
            go.GetComponent<Renderer>().sharedMaterial.SetFloat("_Alpha", 1);
        }
        transform.localScale = Vector3.zero;
        dissapear = true;
        indicator.StartShowing(dissapearTime);
    }
}
