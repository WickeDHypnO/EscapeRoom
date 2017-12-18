using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PingDissapear : Photon.PunBehaviour, IPunObservable
{

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
    public Vector3 lineStartPosition;

    public void OnEnable()
    {
        if (photonView.isMine)
        {
            photonView.RPC("RpcShowPing", PhotonTargets.All);
        }
    }

    private void SetColor()
    {
        if (!PhotonNetwork.isMasterClient && !debugSecondPlayer)
        {
            debugSecondPlayer = true;
        }
        dissapear = true;
    }

    void Update()
    {
        if (dissapear)
        {
            timer += Time.deltaTime;
            foreach (GameObject go in pingElements)
            {
                go.GetComponent<Renderer>().sharedMaterial.SetFloat("_Alpha", 1 - timer / dissapearTime);
            }
            transform.localScale = new Vector3(timer / dissapearTime * scaleFactor, timer / dissapearTime * scaleFactor, timer / dissapearTime * scaleFactor);
            if (timer / dissapearTime >= 1)
            {
                foreach (GameObject go in pingElements)
                {
                    go.GetComponent<Renderer>().sharedMaterial.SetFloat("_Alpha", 0);
                }
                dissapear = false;
                //gameObject.SetActive(false);
                if (photonView.isMine)
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
        LineRenderer line = GetComponent<LineRenderer>();
        if (!photonView.isMine)
        {
            line.SetPosition(0, lineStartPosition);
            line.SetPosition(1, transform.position);
        }
        indicator = FindObjectOfType<Pinger>().indicator;
        if (photonView.owner.IsMasterClient)
        {
            foreach (GameObject go in pingElements)
            {
                go.GetComponent<Renderer>().sharedMaterial = firstPlayerMaterial;
            }
            line.material = firstPlayerMaterial;
        }
        else
        {
            foreach (GameObject go in pingElements)
            {
                go.GetComponent<Renderer>().sharedMaterial = secondPlayerMaterial;
            }
            line.material = secondPlayerMaterial;
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(lineStartPosition);
        }
        else
        {
            lineStartPosition = (Vector3)stream.ReceiveNext();
            if (lineStartPosition != Vector3.zero && !photonView.isMine)
            {
                LineRenderer line = GetComponent<LineRenderer>();
                if (line.GetPosition(0) != lineStartPosition)
                {
                    line.SetPosition(0, lineStartPosition);
                    line.enabled = true;
                }
            }
        }
    }
}
