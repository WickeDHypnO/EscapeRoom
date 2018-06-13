using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LoadingScreenCanvas : MonoBehaviour {

	public Slider loadingSlider;
	public void StartLoading () {
		GetComponent<CanvasGroup>().DOFade(1, 1f);
	}
	
	public void FinishLoading () {
		GetComponent<CanvasGroup>().DOFade(0, 1f);
	}
}
