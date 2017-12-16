using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingIndicator : MonoBehaviour
{
    public Transform ping;
    public GameObject indicator;
    public Vector2 cameraFrustum;
    public float minAngle;
    public float safeEdgesWidth;
    float timer = 0;
    float showingDuration;

    public void StartShowing(float duration)
    {
        showingDuration = duration;
        timer = duration;
        enabled = true;
    }

    void Start()
    {
        cameraFrustum = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
       // StartCoroutine(GetOtherPlayerPing());
    }

    IEnumerator GetOtherPlayerPing()
    {
        yield return new WaitForSeconds(1f);
        foreach(PingDissapear pd in FindObjectsOfType<PingDissapear>())
        {
            if(!pd.photonView.isMine)
            {
                ping = pd.transform;
            }
        }
    }

    void Update()
    {
        if (!ping)
            return;
        timer -= Time.deltaTime;
        indicator.GetComponent<CanvasGroup>().alpha = timer/showingDuration;
        if(timer <= 0)
        {
            indicator.GetComponent<CanvasGroup>().alpha = 0;
            enabled = false;
            return;
        }
        Vector3 targetDir = ping.position - transform.parent.position;
        Vector3 pos = Camera.main.WorldToViewportPoint(ping.position);
        Vector3 playerAngle = new Vector3(transform.parent.forward.x, 0.0f, transform.parent.forward.z);
        Vector3 cameraAngle = new Vector3(targetDir.x, 0.0f, targetDir.z);
        float horizDiffAngle = Vector3.SignedAngle(playerAngle, cameraAngle, Vector3.up);
        if (Mathf.Abs(horizDiffAngle) < minAngle)
        {
            if (indicator.activeInHierarchy)
                indicator.SetActive(false);
            return;
        }
        else
        {
            if (!indicator.activeInHierarchy)
            {
                indicator.SetActive(true);
            }
            if (horizDiffAngle > 0)
            {
                indicator.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
                pos.x = cameraFrustum.x - safeEdgesWidth;
            }
            else if (horizDiffAngle < 0)
            {
                indicator.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 180);
                pos.x = safeEdgesWidth;
            }
        }
        indicator.GetComponent<RectTransform>().position = new Vector3(pos.x , cameraFrustum.y/2, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.parent.parent.forward * 2);
    }
}
