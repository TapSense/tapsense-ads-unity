using UnityEngine;
using System;
using System.Collections;


public class DemoControl : MonoBehaviour, TapSense.TapSenseInterstitialListener, TapSense.TapSenseVideoListener, TapSense.TapSenseAdViewListener
{
	public Texture2D pauseIcon, menuBackground, resumeButton, restartButton, fullscreenButton, muteButton, quitButton;
	
	private const float cornerTextureSize = 300.0f;
	private const float menuWidth = 400.0f, menuHeight = 450.0f, menuHeaderHeight = 26.0f, buttonWidth = 350.0f, buttonHeight = 60.0f;
	
	private bool fullScreenAvailable = false, quitEnabled = true, directKeyQuit = true;

#if UNITY_ANDROID || UNITY_IPHONE
	private static TapSense.TapSenseInterstitial mInterstitial;
	private static TapSense.TapSenseAdView mBanner;
#endif

#if UNITY_ANDROID
	private static readonly string INTERSTITIAL_AD_UNIT_ID = "53c9a1c6e4b0645225e57055";
	private static readonly string BANNER_AD_UNIT_ID = "53c9a222e4b0645225e5705f";
#endif

#if UNITY_IPHONE
	private static string INTERSTITIAL_AD_UNIT_ID = "53d03865e4b004068fab1c9d";
	private static string BANNER_AD_UNIT_ID = "53d038c5e4b004068fab1ca0";
#endif

	public static void Restart ()
	{
		DemoControl instance = (DemoControl)FindObjectOfType (typeof (DemoControl));
		if (instance != null)
		{
			Destroy (instance.gameObject);
		}
		Time.timeScale = 1.0f;
		Application.LoadLevel (0);
	}
	
	
	public bool AudioEnabled
	{
		get
		{
			return PlayerPrefs.GetInt ("Play audio", 1) != 0;
		}
		set
		{
			PlayerPrefs.SetInt ("Play audio", value ? 1 : 0);
			UpdateAudio ();
		}
	}
	
	
	void Start ()
	{
		UpdateAudio ();
		
		switch (Application.platform)
		{
			case RuntimePlatform.OSXWebPlayer:
			case RuntimePlatform.WindowsWebPlayer:
			case RuntimePlatform.NaCl:
				fullScreenAvailable = true;
				quitEnabled = false;
				directKeyQuit = false;
			break;
			case RuntimePlatform.FlashPlayer:
				fullScreenAvailable = false;
				quitEnabled = false;
				directKeyQuit = false;
			break;
			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.WindowsPlayer:
				fullScreenAvailable = true;
				directKeyQuit = false;
			break;
		}
		
	#if UNITY_ANDROID || UNITY_IPHONE
		if (mInterstitial == null || mBanner == null) {
			TapSense.setTestMode();
			TapSense.setShowDebugLog();

        	mInterstitial = new TapSense.TapSenseInterstitial(INTERSTITIAL_AD_UNIT_ID);
			mInterstitial.setListener(this);
			mInterstitial.setVideoListener(this);

        	mBanner = new TapSense.TapSenseAdView(BANNER_AD_UNIT_ID,
		 	                                      TapSense.BannerPosition.TOP,
			                                      TapSense.AdSize.Banner);
			mBanner.setListener(this);
			mBanner.loadAd();
		}
		mBanner.setVisibility(false);
	#endif
	}
	
	
	void UpdateAudio ()
	{
		AudioListener.volume = AudioEnabled ? 1.0f : 0.0f;
	}
	
	
	public void FlipFullscreen ()
	{
		Screen.fullScreen = !Screen.fullScreen;
	}
	
	
	public void FlipMute ()
	{
		AudioEnabled = !AudioEnabled;
	}
	
	
	public void FlipPause ()
	{
		Time.timeScale = Time.timeScale == 0.0f ? 1.0f : 0.0f;

	#if UNITY_ANDROID || UNITY_IPHONE
		if (Time.timeScale == 0.0f)
			mInterstitial.showAd();

		//Show the banner only when the game is paused
		mBanner.setVisibility(Time.timeScale == 0.0f);
	#endif
	}
	
	
	void Update ()
	{
		if (directKeyQuit)
		{
			if (Input.GetKeyDown (KeyCode.Escape))
			{
				Application.Quit ();
			}
			else if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.Menu))
			{
				Time.timeScale = 0.0f;
			}
		}
	}
	
	
	void OnGUI ()
	{
		Rect rightRect = new Rect (Screen.width - cornerTextureSize, 0.0f, cornerTextureSize, cornerTextureSize);
		
		switch (Event.current.type)
		{
			case EventType.Repaint:
				GUI.DrawTexture (rightRect, pauseIcon);
			break;
			case EventType.MouseDown:
				if (rightRect.Contains (Event.current.mousePosition))
				{
					FlipPause ();
					Event.current.Use ();
				}
			break;
		}
		
		if (Time.timeScale != 0.0f)
		{
			return;
		}
		
		Rect menuRect = new Rect (
			(Screen.width - menuWidth) * 0.5f,
			(Screen.height - menuHeight) * 0.5f,
			menuWidth,
			menuHeight
		);
		
		GUI.DrawTexture (menuRect, menuBackground);
		
		GUILayout.BeginArea (menuRect);
			GUILayout.Space (menuHeaderHeight);
		
			GUILayout.FlexibleSpace ();
			
			if (MenuButton (resumeButton))
			{
				FlipPause();
        	}
        
			if (fullScreenAvailable)
			{
				GUILayout.FlexibleSpace ();
				if (MenuButton (fullscreenButton))
				{
					FlipFullscreen ();
				}
			}
			
			#if !UNITY_FLASH
				GUILayout.FlexibleSpace ();
			
				if (MenuButton (muteButton))
				{
					FlipMute ();
				}
			#endif
			
			GUILayout.FlexibleSpace ();
			
			if (MenuButton (restartButton))
			{
				Restart ();
			}
			
			if (quitEnabled)
			{
				GUILayout.FlexibleSpace ();
				if (MenuButton (quitButton))
				{
					Application.Quit ();
				}
			}
			GUILayout.FlexibleSpace ();
		GUILayout.EndArea ();
	}
	
	
	bool MenuButton (Texture2D icon)
	{
		bool wasPressed = false;
		
		GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
		
			Rect rect = GUILayoutUtility.GetRect (buttonWidth, buttonHeight, GUILayout.Width (buttonWidth), GUILayout.Height (buttonHeight));
		
			switch (Event.current.type)
			{
				case EventType.MouseUp:
					if (rect.Contains (Event.current.mousePosition))
					{
						wasPressed = true;
					}
				break;
				case EventType.Repaint:
					GUI.DrawTexture (rect, icon);
				break;
			}
		
			GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();
		
		return wasPressed;
	}

#if UNITY_ANDROID || UNITY_IPHONE
	public void onInterstitialLoaded(TapSense.TapSenseInterstitial interstitial) {
		Debug.Log ("In DemoControl.onInterstitialLoaded()");
	}
	public void onInterstitialFailedToLoad(TapSense.TapSenseInterstitial interstitial) {
		Debug.Log ("In DemoControl.onInterstitialFailedToLoad()");
	}
	public void onInterstitialShown(TapSense.TapSenseInterstitial interstitial) {
		Debug.Log ("In DemoControl.onInterstitialShown()");
	}
	public void onInterstitialDismissed(TapSense.TapSenseInterstitial interstitial) {
		Debug.Log ("In DemoControl.onInterstitialDismissed()");
	}

	public void onInterstitialCompletedVideo(TapSense.TapSenseInterstitial interstitial) {
		Debug.Log ("In DemoControl.onInterstitialCompletedVideo()");
	}
	public void onInterstitialSkippedVideo(TapSense.TapSenseInterstitial interstitial) {
		Debug.Log ("In DemoControl.onInterstitialSkippedVideo()");
	}
	
	public void onAdViewLoaded(TapSense.TapSenseAdView banner) {
		Debug.Log ("In DemoControl.onAdViewLoaded");
	}
	public void onAdViewFailedToLoad(TapSense.TapSenseAdView banner) {
		Debug.Log ("In DemoControl.onAdViewFailedToLoad");
	}
	public void onAdViewExpanded(TapSense.TapSenseAdView banner) {
		Debug.Log ("In DemoControl.onAdViewExpanded");
	}
	public void onAdViewCollapsed(TapSense.TapSenseAdView banner) {
		Debug.Log ("In DemoControl.onAdViewCollapsed");
	}
#endif
}
