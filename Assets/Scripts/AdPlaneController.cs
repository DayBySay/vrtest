using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class AdPlaneController : VRInteractiveItem {

	const string MOVIE_TEXTURE = "Video/AQUARIUS";
	const string BANNER_TEXTURE = "Materials/AQUARIUS";
	private MovieTexture movieTexture;
	private Texture bannerTexture;
	private VRInteractiveItem m_InteractiveItem;   
	private bool startPlaying = false;
	public GameObject progress;

	void Awake () {
		m_InteractiveItem = GetComponent<VRInteractiveItem> ();

		this.movieTexture = Resources.Load<MovieTexture> (MOVIE_TEXTURE);
		this.bannerTexture = Resources.Load<Texture> (BANNER_TEXTURE);
	}

	void OnDisable () {
		m_InteractiveItem.OnClick -= didTouchUpAd;
	}

	void OnBecameVisible () {
		if (!this.startPlaying) {
			this.progress.SetActive (true);
			Invoke ("videoPlayIfPossible", 2);
			return;
		}

		videoPlayIfPossible ();
	}

	void OnBecameInvisible () {
		if (!this.startPlaying) {
			this.progress.SetActive (false);
			CancelInvoke ();

			return;
		}

		videoPauseIfPossible ();
	}

	void didTouchUpAd() {
		if (!this.startPlaying) {
			return;
		}

		// 何かしらのアクション
	}

	void videoPlayOrPause() {
		bool playing = ((MovieTexture)GetComponent<Renderer> ().material.mainTexture).isPlaying;
		videoPauseWithPlaying (playing);
	}

	void videoPauseWithPlaying(bool playing) {
		if (playing) {
			videoPauseIfPossible ();
			return;
		}

		videoPlayIfPossible ();
	}

	void videoPlayIfPossible() {
		if (!this.startPlaying) {
			this.progress.SetActive (false);
			this.startPlaying = true;
			m_InteractiveItem.OnClick += didTouchUpAd;
			GetComponent<Renderer> ().material.mainTexture = this.movieTexture;
			Invoke ("showBannerTexture", this.movieTexture.duration);
		}

		((MovieTexture)GetComponent<Renderer> ().material.mainTexture).Play ();
	}

	void showBannerTexture () {
		GetComponent<Renderer> ().material.mainTexture = this.bannerTexture;
	}

	void videoPauseIfPossible() {
		((MovieTexture)GetComponent<Renderer> ().material.mainTexture).Pause ();
	}
}
