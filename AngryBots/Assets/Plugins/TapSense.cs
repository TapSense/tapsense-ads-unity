using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class TapSense : MonoBehaviour {

	public enum BannerPosition {TOP, BOTTOM};

	public enum Gender {MALE, FEMALE, UNKOWN};

	public class AdSize {
		public int width { get; private set; }
		public int height { get; private set; }

		/** Banner: 320x50 */
		public static readonly AdSize Banner = new AdSize(320, 50);
		/** MediumRect: 300x250 */
		public static readonly AdSize MediumRect = new AdSize(300, 250);
		/** Leaderboard: 728x90 */
		public static readonly AdSize Leaderboard = new AdSize(728, 90);
		/** iPadBanner: 768x66 */
		public static readonly AdSize iPadBanner = new AdSize(768, 66);
		
		AdSize(int width, int height) {
			this.width = width;
			this.height = height;
		}
    }

	private static bool showLog = false;
    
	public static void log(string msg) {
		if (showLog)
			Debug.Log(msg);
	}

	// ========================================
	// Callback support
	// ========================================
	public interface TapSenseInterstitialListener {
		#if UNITY_ANDROID || UNITY_IPHONE
		void onInterstitialLoaded(TapSenseInterstitial interstitial);
		void onInterstitialFailedToLoad(TapSenseInterstitial interstitial);
		void onInterstitialShown(TapSenseInterstitial interstitial);
		void onInterstitialDismissed(TapSenseInterstitial interstitial);
		#endif
	}
	
	public interface TapSenseVideoListener {
		#if UNITY_ANDROID || UNITY_IPHONE
		void onInterstitialCompletedVideo(TapSenseInterstitial interstitial);
		void onInterstitialSkippedVideo(TapSenseInterstitial interstitial);
		#endif
	}

	public interface TapSenseAdViewListener {
		#if UNITY_ANDROID || UNITY_IPHONE
		void onAdViewLoaded(TapSenseAdView banner);
		void onAdViewFailedToLoad(TapSenseAdView banner);
		void onAdViewExpanded(TapSenseAdView banner);
		void onAdViewCollapsed(TapSenseAdView banner);
		#endif
	}

#if UNITY_ANDROID || UNITY_IPHONE
	private static Dictionary<int, TapSenseInterstitial> interstitials =
		new Dictionary<int, TapSenseInterstitial>();

	private static Dictionary<int, TapSenseAdView> banners =
		new Dictionary<int, TapSenseAdView>();

	void Awake()
	{
		// Set the GameObject name to the class name for easy access from native code
		gameObject.name = this.GetType().ToString();
		DontDestroyOnLoad(this);
	}

	// ========================================
	// Interstitial callbacks from native code
	// ========================================

	public void onInterstitialLoaded(string msg) {
		int id = int.Parse(msg);
		log ("onInterstitialLoaded("+msg+")");

		if (interstitials.ContainsKey(id) && interstitials[id] != null) {
			TapSenseInterstitialListener l = interstitials[id].getListener();
			if (l != null) l.onInterstitialLoaded(interstitials[id]);
		}
	}

	public void onInterstitialFailedToLoad(string msg) {
		int id = int.Parse(msg);
		log ("onInterstitialFailedToLoad("+msg+")");

		if (interstitials.ContainsKey(id) && interstitials[id] != null) {
			TapSenseInterstitialListener l = interstitials[id].getListener();
			if (l != null) l.onInterstitialFailedToLoad(interstitials[id]);
		}
	}

	public void onInterstitialShown(string msg) {
		int id = int.Parse(msg);
		log ("onInterstitialShown("+msg+")");
		
		if (interstitials.ContainsKey(id) && interstitials[id] != null) {
			TapSenseInterstitialListener l = interstitials[id].getListener();
			if (l != null) l.onInterstitialShown(interstitials[id]);
		}
	}

	public void onInterstitialDismissed(string msg) {
		int id = int.Parse(msg);
		log ("onInterstitialDismissed("+msg+")");
		
		if (interstitials.ContainsKey(id) && interstitials[id] != null) {
			TapSenseInterstitialListener l = interstitials[id].getListener();
			if (l != null) l.onInterstitialDismissed(interstitials[id]);
		}
	}
	
	// ========================================
	// Video callbacks from native code
	// ========================================
	
	public void onInterstitialCompletedVideo(string msg) {
		int id = int.Parse(msg);
		log ("onInterstitialCompletedVideo("+msg+")");
		
		if (interstitials.ContainsKey(id) && interstitials[id] != null) {
			TapSenseVideoListener l = interstitials[id].getVideoListener();
			if (l != null) l.onInterstitialCompletedVideo(interstitials[id]);
		}
	}
	
	public void onInterstitialSkippedVideo(string msg) {
		int id = int.Parse(msg);
		log ("onInterstitialSkippedVideo("+msg+")");
		
		if (interstitials.ContainsKey(id) && interstitials[id] != null) {
			TapSenseVideoListener l = interstitials[id].getVideoListener();
			if (l != null) l.onInterstitialSkippedVideo(interstitials[id]);
		}
	}

	// ========================================
	// Banner callbacks from native code
	// ========================================

	public void onAdViewLoaded(string msg) {
		int id = int.Parse(msg);
		log ("onAdViewLoaded("+msg+")");
		
		if (banners.ContainsKey(id) && banners[id] != null) {
			TapSenseAdViewListener l = banners[id].getListener();
			if (l != null) l.onAdViewLoaded(banners[id]);
		}
	}

	public void onAdViewFailedToLoad(string msg) {
		int id = int.Parse(msg);
		log ("onAdViewFailedToLoad("+msg+")");
		
		if (banners.ContainsKey(id) && banners[id] != null) {
			TapSenseAdViewListener l = banners[id].getListener();
			if (l != null) l.onAdViewFailedToLoad(banners[id]);
		}
	}

	public void onAdViewExpanded(string msg) {
		int id = int.Parse(msg);
		log ("onAdViewExpanded("+msg+")");
		
		if (banners.ContainsKey(id) && banners[id] != null) {
			TapSenseAdViewListener l = banners[id].getListener();
			if (l != null) l.onAdViewExpanded(banners[id]);
		}
	}

	public void onAdViewCollapsed(string msg) {
		int id = int.Parse(msg);
		log ("onAdViewCollapsed("+msg+")");
		
		if (banners.ContainsKey(id) && banners[id] != null) {
			TapSenseAdViewListener l = banners[id].getListener();
			if (l != null) l.onAdViewCollapsed(banners[id]);
		}
	}
#endif


#if UNITY_IPHONE
	private static bool _safeSetup() {
		return Application.platform != RuntimePlatform.OSXEditor;
	}

	///////////// externs

	/////// Static methods
	
	[DllImport ("__Internal")]
	public static extern void _setTestMode();
	public static void setTestMode()
	{
		if (!_safeSetup()) return;

		_setTestMode();
	}
	
	[DllImport ("__Internal")]
	public static extern void _setShowDebugLog();
	public static void setShowDebugLog()
	{
		if (!_safeSetup()) return;

		_setShowDebugLog();
		showLog = true;
	}

	[DllImport ("__Internal")]
	public static extern void _trackForAdUnitId(string adUnitId);
	public static void trackForAdUnitId(string adUnitId) {
		if (!_safeSetup()) return;

		_trackForAdUnitId(adUnitId);
	}

	/////// TapSenseInterstitital

	[DllImport ("__Internal")]
	private static extern int _initInterstitialWithAdUnitIdShouldAutoRequestAdKeywordMap(string adUnitId, bool autoRequestAd, int keywordMapIndex);

	[DllImport ("__Internal")]
	private static extern int _initInterstitialWithAdUnitId(string adUnitId);

	[DllImport ("__Internal")]
	private static extern int _initInterstitialWithAdUnitIdShouldAutoRequestAd(string adUnitId, bool autoRequestAd);

	[DllImport ("__Internal")]
	private static extern int _initInterstitialWithAdUnitIdKeywordMap(string adUnitId, int keywordMapIndex);
	
	[DllImport ("__Internal")]
	private static extern bool _showInterstitial(int index);

	[DllImport ("__Internal")]
	private static extern bool _isInterstitialReady(int index);

	[DllImport ("__Internal")]
	private static extern bool _requestInterstitial(int index);

	/////// TapSenseAdView

	[DllImport ("__Internal")]
	private static extern int _initAdView(string adUnitId, int bannerPosition, int width, int height);
	
	[DllImport ("__Internal")]
	private static extern void _setShouldAutoRefresh(int index, bool shouldAutoRefresh);
	
	[DllImport ("__Internal")]
	private static extern void _setKeywordMap(int index, int keywordMapIndex);
	
	[DllImport ("__Internal")]
	private static extern void _loadAd(int index);

	[DllImport ("__Internal")]
	private static extern void _setVisibility(int index, bool visible);

	[DllImport ("__Internal")]
	private static extern void _destroyBanner(int index);
    
	/////// KeywordsMap
	
	[DllImport ("__Internal")]
	private static extern int _newKeywordsMapBuilder();
	
	[DllImport ("__Internal")]
	private static extern void _keywordsMapBuilderSetGender(int ptr, int gender);
	
	[DllImport ("__Internal")]
	private static extern void _keywordsMapBuilderSetBirthday(int ptr, string birthday);
	
	[DllImport ("__Internal")]
	private static extern void _keywordsMapBuilderSetLocation(int ptr, float latitude, float longitude);
	
	[DllImport ("__Internal")]
	private static extern void _keywordsMapBuilderSetValueForKey(int ptr, string value, string key);
	
	[DllImport ("__Internal")]
	private static extern int _newKeywordsMap(int index);

	///////////// end externs

	public class TapSenseInterstitial {
		private int id;
		private TapSenseInterstitialListener listener;
		private TapSenseVideoListener videoListener;

		public TapSenseInterstitial(string adUnitId, bool autoRequestAd, TSKeywordMap map) {
			if (!_safeSetup()) {
				id = 0;
				return;
			}

			int mapIndex = (map != null) ? map.getId() : -1;
			id = _initInterstitialWithAdUnitIdShouldAutoRequestAdKeywordMap(adUnitId, autoRequestAd, mapIndex);
			listener = null;
			
			registerForCallbacks();
			TapSense.log("new Interstitial(): "+id+" registered");
		}

		public TapSenseInterstitial(string adUnitId) {
			if (!_safeSetup()) {
				id = 0;
				return;
			}
			id = _initInterstitialWithAdUnitId(adUnitId);
			listener = null;
			
			registerForCallbacks();
			TapSense.log("new Interstitial(): "+id+" registered");
		}

		public TapSenseInterstitial(string adUnitId, bool autoRequestAd) {
			if (!_safeSetup()) {
				id = 0;
				return;
			}
			id = _initInterstitialWithAdUnitIdShouldAutoRequestAd(adUnitId, autoRequestAd);
			listener = null;
			
			registerForCallbacks();
			TapSense.log("new Interstitial(): "+id+" registered");
		}

		public TapSenseInterstitial(string adUnitId, TSKeywordMap map) {
			if (!_safeSetup()) {
				id = 0;
				return;
			}
			int mapIndex = (map != null) ? map.getId() : -1;
			id = _initInterstitialWithAdUnitIdKeywordMap(adUnitId, mapIndex);
			listener = null;
			
			registerForCallbacks();
			TapSense.log("new Interstitial(): "+id+" registered");
		}

		public void destroy() {
		}

		private void registerForCallbacks() {
			if (TapSense.interstitials.ContainsKey(id))
				TapSense.interstitials[id] = this;
			else
				TapSense.interstitials.Add(id, this);
        }
        
        public void showAd() {
			if (!_safeSetup()) return;
			_showInterstitial(id);
		}

		public bool isReady() {
			if (!_safeSetup()) {
				return false;
			}
			return _isInterstitialReady(id);
		}

		public void requestAd() {
			if (!_safeSetup()) return;
			_requestInterstitial(id);
		}
		
		public void setListener(TapSenseInterstitialListener listener) {
			this.listener = listener;
		}
		
		public void setVideoListener(TapSenseVideoListener listener) {
			this.videoListener = listener;
		}
		
		public TapSenseInterstitialListener getListener() {
			return listener;
		}
		
		public TapSenseVideoListener getVideoListener() {
			return videoListener;
		}
	}

	public class TapSenseAdView {
		private int id;
		private TapSenseAdViewListener listener;
		
		public TapSenseAdView(string adUnitId, BannerPosition position, AdSize adSize) {
			if (!_safeSetup()) {
				id = 0;
				return;
			}
			id = _initAdView(adUnitId, (int) position, adSize.width, adSize.height);
			listener = null;
			
			registerForCallbacks();
			TapSense.log("new AdView(): "+id+" registered");
		}
		
		private void registerForCallbacks() {
			if (TapSense.banners.ContainsKey(id))
				TapSense.banners[id] = this;
            else
                TapSense.banners.Add(id, this);
        }
        
        public void setListener(TapSenseAdViewListener listener) {
			this.listener = listener;
		}
		
		public TapSenseAdViewListener getListener() {
			return listener;
		}

		public void setAutoRefresh(bool shouldAutoRefresh) {
			if (!_safeSetup()) return;
			_setShouldAutoRefresh(id, shouldAutoRefresh);
		}

		public void setKeywordMap(TSKeywordMap map) {
			if (!_safeSetup()) return;

			int mapIndex = (map != null) ? map.getId() : -1;
			_setKeywordMap(id, mapIndex);
        }
        
        public void loadAd() {
			if (!_safeSetup()) return;
			_loadAd(id);
		}

		public void setVisibility(bool visible) {
			if (!_safeSetup()) return;
            _setVisibility(id, visible);
		}
        
        public void destroy() {
			if (!_safeSetup()) return;
			_destroyBanner(id);
		}
	}
	
	public class TSKeywordMap {
		
		public class Builder {
			private int id;
			
			public Builder() {
				if (!_safeSetup()) {
					id = 0;
					return;
				}
				id = _newKeywordsMapBuilder();
			}
			
			public Builder setGender(Gender gender) {
				if (!_safeSetup()) return this;
				_keywordsMapBuilderSetGender(id, (int) gender);
				return this;
			}
			
			public Builder setBirthday(DateTime birthday) {
				if (!_safeSetup()) return this;
				//Format date as SQL date for easy parsing
				_keywordsMapBuilderSetBirthday(id, birthday.ToString("yyyy-MM-dd"));
				return this;
			}
			
			public Builder setLocation(LocationInfo location) {
				if (!_safeSetup()) return this;
				_keywordsMapBuilderSetLocation(id, location.latitude, location.longitude);
				return this;
			}
			
			public Builder setValueForKey(string value, string key) {
				if (!_safeSetup()) return this;
				_keywordsMapBuilderSetValueForKey(id, value, key);
				return this;
			}
			
			public TSKeywordMap build() {
				return new TSKeywordMap(id);
			}
		}
		
		private int id;

		private TSKeywordMap(int builderId) {
			if (!_safeSetup()) return;
			id = _newKeywordsMap(builderId);
		}
		
		public int getId() {
			return id;
		}
	}

#endif

#if UNITY_ANDROID

	// ========================================
	// Android TSKeywordMap Support Class
	// ========================================

	public class TSKeywordMap {

		public class Builder {
			private static readonly string DATE_FORMAT = "yyyy-MM-dd";

			private int id;
			
			public Builder() {
				if (!_safeSetup()) {
					id = 0;
					return;
				}
				id = _bridge.Call<int>("newKeywordMapBuilder");
			}

			~Builder() {
				if (!_safeSetup()) return;
				//_bridge.Call("destructKeywordMapBuilder", id);
			}

			public Builder setGender(Gender gender) {
				if (!_safeSetup()) return this;
				_bridge.Call("keywordMapBuilderSetGender", id, Convert.ChangeType(gender, typeof(int)));
				return this;
			}

			public Builder setBirthday(DateTime birthday) {
				if (!_safeSetup()) return this;
				_bridge.Call("keywordMapBuilderSetBirthday", id, DATE_FORMAT, birthday.ToString(DATE_FORMAT));
				return this;
			}

			public Builder setLocation(LocationInfo location) {
				if (!_safeSetup()) return this;
				_bridge.Call("keywordMapBuilderSetLocation", id, location.latitude, location.longitude);
				return this;
			}

			public Builder setValueForKey(string value, string key) {
				if (!_safeSetup()) return this;
				_bridge.Call("keywordMapBuilderSetValueForKey", id, value, key);
				return this;
			}

			public TSKeywordMap build() {
				return new TSKeywordMap(id);
			}
		}

		private int id;
		
		private TSKeywordMap() {}
		
		private TSKeywordMap(int builderId) {
			if (!_safeSetup()) return;
			id = _bridge.Call<int>("newKeywordMap", builderId);
		}
		
		~TSKeywordMap() {
			if (!_safeSetup()) return;
			//_bridge.Call("destructKeywordMap", id);
		}
		
		public int getId() {
			int copy = id;
			return copy;
		}

		public string toString() {
			if (!_safeSetup()) return "";
			return _bridge.Call<string>("keywordMapToString", id);
		}
	}

	// ========================================
	// Android Interstitial Support Class
	// ========================================

	public class TapSenseInterstitial {
		private int id;
		private TapSenseInterstitialListener listener;
		private TapSenseVideoListener videoListener;

		public TapSenseInterstitial(string adUnitId) {
			TapSense.log("new Interstitial(): start");
			if (!_safeSetup()) {
				id = 0;
				return;
			}
			id = _bridge.Call<int>("initInterstitial", adUnitId);
			TapSense.log("new Interstitial(): "+id);

			listener = null;

			registerForCallbacks();
			TapSense.log("new Interstitial(): "+id+" registered");
		}

		public TapSenseInterstitial(string adUnitId, bool autoRequestAd) {
			TapSense.log("new Interstitial(): start");
			if (!_safeSetup()) {
				id = 0;
				return;
			}
			id = _bridge.Call<int>("initInterstitial", adUnitId, autoRequestAd);
			TapSense.log("new Interstitial(): "+id);
			
			listener = null;
			
			registerForCallbacks();
			TapSense.log("new Interstitial(): "+id+" registered");
		}

		public TapSenseInterstitial(string adUnitId, TSKeywordMap userInfo) {
			TapSense.log("new Interstitial(): start");
			if (!_safeSetup()) {
				id = 0;
				return;
			}
			id = _bridge.Call<int>("initInterstitial", adUnitId, userInfo.getId());
			TapSense.log("new Interstitial(): "+id);
			
			listener = null;
			
			registerForCallbacks();
			TapSense.log("new Interstitial(): "+id+" registered");
		}
		
		public TapSenseInterstitial(string adUnitId, bool autoRequestAd, TSKeywordMap userInfo) {
			TapSense.log("new Interstitial(): start");
			if (!_safeSetup()) {
				id = 0;
				return;
			}
			id = _bridge.Call<int>("initInterstitial", adUnitId, autoRequestAd, userInfo.getId());
			TapSense.log("new Interstitial(): "+id);
			
			listener = null;
			
			registerForCallbacks();
			TapSense.log("new Interstitial(): "+id+" registered");
		}

		private void registerForCallbacks() {
			if (TapSense.interstitials.ContainsKey(id))
				TapSense.interstitials[id] = this;
			else
				TapSense.interstitials.Add(id, this);
		}

		public void showAd() {
			if (!_safeSetup()) return;
			_bridge.Call("interstitialShowAd", id);
		}

		public void requestAd() {
			if (!_safeSetup()) return;
			_bridge.Call("interstitialRequestAd", id);
		}

		public bool isReady() {
			if (!_safeSetup()) return false;
			return _bridge.Call<bool>("interstitialIsReady", id);
		}
		
		public void setListener(TapSenseInterstitialListener listener) {
			this.listener = listener;
		}
		
		public void setVideoListener(TapSenseVideoListener listener) {
			this.videoListener = listener;
		}
		
		public TapSenseInterstitialListener getListener() {
			return listener;
		}
		
		public TapSenseVideoListener getVideoListener() {
			return videoListener;
		}

		public void destroy() {
			TapSense.log("destroyInterstitial("+id+"): start");
			if (!_safeSetup()) return;
			
			if (TapSense.interstitials[id] == this)
				TapSense.interstitials[id] = null;
			_bridge.Call("destructInterstitial", id);
			
			TapSense.log("destroyInterstitial("+id+"): return");
		}
	}

	// ========================================
	// Android AdView Support Class
	// ========================================

	public class TapSenseAdView {
		private int id;
		private TapSenseAdViewListener listener;
		
		public TapSenseAdView(string adUnitId, BannerPosition pos, AdSize adSize) {
			TapSense.log("new AdView(): start");
			if (!_safeSetup()) {
				id = 0;
				return;
			}
			int posInt = (int)Convert.ChangeType(pos, typeof(int));
			id = _bridge.Call<int>("initAdView", posInt, adUnitId);
			TapSense.log("new AdView(): "+id);

			listener = null;
			
			registerForCallbacks();
			TapSense.log("new AdView(): "+id+" registered");
		}

		private void registerForCallbacks() {
			if (TapSense.banners.ContainsKey(id))
				TapSense.banners[id] = this;
			else
				TapSense.banners.Add(id, this);
		}

		public void setListener(TapSenseAdViewListener listener) {
			this.listener = listener;
		}
		
		public TapSenseAdViewListener getListener() {
			return listener;
		}

		public void setVisibility(bool visible) {
			if (!_safeSetup()) return;
			_bridge.Call("adViewSetVisibility", id, visible);
		}

		public void setKeywordMap(TSKeywordMap map) {
			if (!_safeSetup()) return;
			_bridge.Call("adViewSetKeywordMap", id, map.getId());
		}

		public void setAutoRefresh(bool autoRefresh) {
			if (!_safeSetup()) return;
			_bridge.Call("adViewSetAutoRefresh", id, autoRefresh);
		}
		
		public void loadAd() {
			if (!_safeSetup()) return;
			_bridge.Call("adViewLoadAd", id);
		}

		public void destroy() {
			TapSense.log("destroyAdView("+id+"): start");
			if (!_safeSetup()) return;

			if (TapSense.banners[id] == this)
				TapSense.banners[id] = null;
			_bridge.Call("destructAdView", id);

			TapSense.log("destroyAdView("+id+"): return");
		}
	}

	// ========================================
	// Android Specific TapSense Class Data and Methods
	// ========================================

	private static AndroidJavaObject _bridge = null;

	private static bool _safeSetup() {
		if (Application.platform != RuntimePlatform.Android) {
			return false;
		}

		if (_bridge == null) {
			using (AndroidJavaClass pluginCls = new AndroidJavaClass("com.tapsense.android.publisher.TSUnityBridge")) {
				_bridge = pluginCls.CallStatic<AndroidJavaObject>("getInstance");
			}
		}
		return true;
	}

	public static void setTestMode() {
		if (!_safeSetup()) return;
		_bridge.Call("setTestMode");
	}

	public static void setShowDebugLog() {
		if (!_safeSetup()) return;
		_bridge.Call("setShowDebugLog");
		showLog = true;
	}

	public static void trackForAdUnitId(string adUnitId) {
		if (!_safeSetup()) return;
		_bridge.Call("trackForAdUnitId", adUnitId);
	}
#endif
}
