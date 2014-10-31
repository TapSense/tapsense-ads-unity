//
//  TapSense.m
//  Unity-iPhone
//
//  Created by Steven Silver on 10/15/14.
//
//
#import "TapSense.h"


void UnitySendMessage( const char * className, const char * methodName, const char * param );

UIViewController *UnityGetGLViewController();

@interface TSUnityPlugin () <TapSenseAdViewDelegate, TapSenseInterstitialDelegate>
@property (nonatomic, strong) NSMutableArray *interstitialArray;
@property (nonatomic, strong) NSMutableArray *bannerArray;
@property (nonatomic, strong) NSMutableArray *keywordMapArray;
@end

@implementation TSUnityPlugin

typedef enum {
    TOP, BOTTOM
} BannerPostition;

+ (TSUnityPlugin *) sharedInstance {
    static TSUnityPlugin *instance = nil;
    
    if (!instance)
    {
        instance = [[TSUnityPlugin alloc] init];
    }
    
    return instance;
}

# pragma mark - TapSenseInterstitital

- (NSUInteger) initInterstitialWithAdUnitId:(NSString *)adUnitId
                  shouldAutoRequestAd:(BOOL) autoRequest
                           keywordMap:(TSKeywordMap *)map
{
    TapSenseInterstitial *interstitial = [[TapSenseInterstitial alloc] initWithAdUnitId:adUnitId
                                                  shouldAutoRequestAd:autoRequest
                                                           keywordMap:map];
    interstitial.delegate = self;
    
    if (!self.interstitialArray) {
        self.interstitialArray = [[NSMutableArray alloc] init];
    }
    [self.interstitialArray addObject:interstitial];
    return [self.interstitialArray count] - 1;
}

- (NSUInteger) initInterstitialWithAdUnitId:(NSString *)adUnitId
{
    TapSenseInterstitial *interstitial = [[TapSenseInterstitial alloc] initWithAdUnitId:adUnitId];
    interstitial.delegate = self;
    
    if (!self.interstitialArray) {
        self.interstitialArray = [[NSMutableArray alloc] init];
    }
    [self.interstitialArray addObject:interstitial];
    return [self.interstitialArray count] - 1;
}

- (NSUInteger) initInterstitialWithAdUnitId:(NSString *)adUnitId
                        shouldAutoRequestAd:(BOOL) autoRequest
{
    TapSenseInterstitial *interstitial = [[TapSenseInterstitial alloc] initWithAdUnitId:adUnitId
                                                                    shouldAutoRequestAd:autoRequest];
    interstitial.delegate = self;
    
    if (!self.interstitialArray) {
        self.interstitialArray = [[NSMutableArray alloc] init];
    }
    [self.interstitialArray addObject:interstitial];
    return [self.interstitialArray count] - 1;
}

- (NSUInteger) initInterstitialWithAdUnitId:(NSString *)adUnitId
                        keywordMap:(TSKeywordMap *)map
{
    TapSenseInterstitial *interstitial = [[TapSenseInterstitial alloc] initWithAdUnitId:adUnitId
                                                                             keywordMap:map];
    interstitial.delegate = self;
    
    if (!self.interstitialArray) {
        self.interstitialArray = [[NSMutableArray alloc] init];
    }
    [self.interstitialArray addObject:interstitial];
    return [self.interstitialArray count] - 1;
}

- (BOOL) showInterstitial:(int) index
{
    TapSenseInterstitial *interstitial = [self.interstitialArray objectAtIndex:index];
    return [interstitial showAdFromViewController:UnityGetGLViewController()];
}

- (BOOL) isInterstitialReady:(int) index
{
    TapSenseInterstitial *interstitial = [self.interstitialArray objectAtIndex:index];
    return [interstitial isReady];
}

- (BOOL) requestInterstitial:(int) index
{
    TapSenseInterstitial *interstitial = [self.interstitialArray objectAtIndex:index];
    return [interstitial requestAd];
}

- (void) destroyInterstitial:(int) index
{
    TapSenseInterstitial *interstitial = [self.interstitialArray objectAtIndex:index];
    interstitial = nil;
}

# pragma mark - TapSenseAdView

- (NSUInteger) initAdViewWithAdUnitId:(NSString *) adUnitId
                       bannerPosition:(BannerPostition) bannerPosition
                                width:(CGFloat) width
                               height:(CGFloat) height

{
    UIViewController *uvc = UnityGetGLViewController();
    
    TapSenseAdView *adView = [[TapSenseAdView alloc] initWithAdUnitId:adUnitId];
    
    // Set adView.frame
    CGRect adViewFrame;
    if (bannerPosition == BOTTOM) {
        adViewFrame = CGRectMake(0, uvc.view.bounds.size.height - height, width, height);
        adView.autoresizingMask = (UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleTopMargin);
    } else {
        // default TOP
        adViewFrame = CGRectMake(0, 0, width, height);
        adView.autoresizingMask = (UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleBottomMargin);
    }
    // Center ad view
    adViewFrame.origin.x = (uvc.view.bounds.size.width / 2) - (width / 2);
    adView.frame = adViewFrame;
    
    adView.rootViewController = uvc;
    [uvc.view addSubview:adView];
    adView.delegate = self;
    
    if (!self.bannerArray) {
        self.bannerArray = [[NSMutableArray alloc] init];
    }
    [self.bannerArray addObject:adView];
    return [self.bannerArray count] - 1;
}

- (void) setAdViewAtIndex:(int) index withShouldAutoRefresh:(BOOL) shouldAutoRefresh
{
    TapSenseAdView *adView = [self.bannerArray objectAtIndex:index];
    adView.shouldAutoRefresh = shouldAutoRefresh;
}

- (void) setAdViewAtIndex:(int) index withKeywordMap:(TSKeywordMap *) map
{
    TapSenseAdView *adView = [self.bannerArray objectAtIndex:index];
    adView.keywordMap = map;
}

- (void) loadAdView:(int) index
{
    TapSenseAdView *adView = [self.bannerArray objectAtIndex:index];
    [adView loadAd];
}

- (void) refreshAdView:(int) index
{
    TapSenseAdView *adView = [self.bannerArray objectAtIndex:index];
    [adView refreshAd];
}

- (void) setAdViewAtIndex:(int) index withVisibility:(BOOL) visible
{
    TapSenseAdView *adView = [self.bannerArray objectAtIndex:index];
    [adView setHidden:!visible];
}

- (void) destroyAdView:(int) index
{
    TapSenseAdView *adView = [self.bannerArray objectAtIndex:index];
    [adView removeFromSuperview];
    [self.bannerArray setObject:[NSNull null] atIndexedSubscript:index];
}

# pragma mark - TSKeywordMap

- (NSUInteger) addNewKeywordsMap
{
    if (!self.keywordMapArray) {
        self.keywordMapArray = [[NSMutableArray alloc] init];
    }
    [self.keywordMapArray addObject:[[TSKeywordMap alloc] init]];
    return [self.keywordMapArray count] - 1;
}

- (TSKeywordMap *) getKeywordsMap:(int) ptr {
    return [self.keywordMapArray objectAtIndex:ptr];
}

# pragma mark - TapSenseInterstitialDelegate

- (void) interstitialDidLoad:(TapSenseInterstitial *)interstitial
{
    UnitySendMessage("TapSense", "onInterstitialLoaded", [self getInterstitialIndex:interstitial]);
}

- (void) interstitialDidFailToLoad:(TapSenseInterstitial *)interstitial
                         withError:(NSError *)error
{
    // [self getCString:[error localizedDescription]]
    UnitySendMessage("TapSense", "onInterstitialFailedToLoad", [self getInterstitialIndex:interstitial]);
}

- (void) interstitialWillAppear:(TapSenseInterstitial *)interstitial
{
    UnitySendMessage("TapSense", "onInterstitialShown", [self getInterstitialIndex:interstitial]);
}

- (void) interstitialDidDisappear:(TapSenseInterstitial *)interstitial
{
    UnitySendMessage("TapSense", "onInterstitialDismissed", [self getInterstitialIndex:interstitial]);
}

# pragma mark - TapSenseAdViewDelegate

- (void)adViewDidLoadAd:(TapSenseAdView *)adView
{
    UnitySendMessage("TapSense", "onAdViewLoaded", [self getAdViewIndex:adView]);
}

- (void)adViewWillPresentModalView:(TapSenseAdView *)adView
{
    UnitySendMessage("TapSense", "onAdViewFailedToLoad", [self getAdViewIndex:adView]);
}

- (void)adViewDidFailToLoad:(TapSenseAdView *)adView withError:(NSError *)error
{
    // [self getCString:[error localizedDescription]]
    UnitySendMessage("TapSense", "onAdViewExpanded", [self getAdViewIndex:adView]);
}

- (void) adViewDidDismissModalView:(TapSenseAdView *)adView
{
    UnitySendMessage("TapSense", "onAdViewCollapsed", [self getAdViewIndex:adView]);
}

# pragma mark - Utility methods

- (const char *) getCString:(NSString *)string {
    return [string cStringUsingEncoding:NSASCIIStringEncoding];
}

- (const char *) getInterstitialIndex:(TapSenseInterstitial *) interstitial {
    NSInteger index = [self.interstitialArray indexOfObject:interstitial];
    NSString *indexString = [NSString stringWithFormat:@"%ld", (long)index];
    
    return [self getCString:indexString];
}

- (const char *) getAdViewIndex:(TapSenseAdView *) adView {
    NSInteger index = [self.bannerArray indexOfObject:adView];
    NSString *indexString = [NSString stringWithFormat:@"%ld", (long)index];
    
    return [self getCString:indexString];
}

@end


// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
    if (string)
        return [NSString stringWithUTF8String: string];
    else
        return [NSString stringWithUTF8String: ""];
}

# pragma mark - TapSenseAds C interface

void _setTestMode() {
    [TapSenseAds setTestMode];
}

void _setShowDebugLog() {
    [TapSenseAds setShowDebugLog];
}

void _trackForAdUnitId(const char * adUnitId) {
    [TapSenseAds trackForAdUnitId:CreateNSString(adUnitId)];
}

# pragma mark - TapSenseInterstitial C interface

int _initInterstitialWithAdUnitIdShouldAutoRequestAdKeywordMap(const char * adUnitId,
                                                               bool autoRequestAd,
                                                               int keywordMapIndex)
{
    TSKeywordMap *map = [[TSUnityPlugin sharedInstance] getKeywordsMap:keywordMapIndex];
    return (int) [[TSUnityPlugin sharedInstance] initInterstitialWithAdUnitId:CreateNSString(adUnitId)
                                                       shouldAutoRequestAd:autoRequestAd
                                                                keywordMap:map];
}

int _initInterstitialWithAdUnitId(const char * adUnitId)
{
    return (int) [[TSUnityPlugin sharedInstance] initInterstitialWithAdUnitId:CreateNSString(adUnitId)];
}

int _initInterstitialWithAdUnitIdShouldAutoRequestAd(const char * adUnitId,
                                                     bool autoRequestAd)
{
    return (int) [[TSUnityPlugin sharedInstance] initInterstitialWithAdUnitId:CreateNSString(adUnitId)
                                                       shouldAutoRequestAd:autoRequestAd];
}

int _initInterstitialWithAdUnitIdKeywordMap(const char * adUnitId,
                                            int keywordMapIndex)
{
    TSKeywordMap *map = [[TSUnityPlugin sharedInstance] getKeywordsMap:keywordMapIndex];
    return (int) [[TSUnityPlugin sharedInstance] initInterstitialWithAdUnitId:CreateNSString(adUnitId)
                                                                keywordMap:map];
}

bool _showInterstitial(int index)
{
    return [[TSUnityPlugin sharedInstance] showInterstitial:index];
}

bool _isInterstitialReady(int index) {
    return [[TSUnityPlugin sharedInstance] isInterstitialReady:index];
}

bool _requestInterstitial(int index) {
    return [[TSUnityPlugin sharedInstance] requestInterstitial:index];
}

# pragma mark - TapSenseAdView C interface

int _initAdView(const char * adUnitId, BannerPostition bannerPosition, int width, int height)
{
    return (int) [[TSUnityPlugin sharedInstance] initAdViewWithAdUnitId:CreateNSString(adUnitId)
                                                      bannerPosition:bannerPosition
                                                               width:width
                                                              height:height];
}

void _setShouldAutoRefresh(int index, bool shouldAutoRefresh)
{
    [[TSUnityPlugin sharedInstance] setAdViewAtIndex:index
                            withShouldAutoRefresh:shouldAutoRefresh];
}

void _setKeywordMap(int index, int keywordMapIndex)
{
    TSKeywordMap *map = [[TSUnityPlugin sharedInstance] getKeywordsMap:keywordMapIndex];
    [[TSUnityPlugin sharedInstance] setAdViewAtIndex:index withKeywordMap:map];
}

void _loadAd(int index)
{
    [[TSUnityPlugin sharedInstance] loadAdView:index];
}

void _refreshAd(int index)
{
    [[TSUnityPlugin sharedInstance] refreshAdView:index];
}

void _setVisibility(int index, bool visible)
{
    [[TSUnityPlugin sharedInstance] setAdViewAtIndex:index withVisibility:visible];
}

void _destroyBanner(int index)
{
    [[TSUnityPlugin sharedInstance] destroyAdView:index];
}

# pragma mark - TSKeywordMap C interface

int _newKeywordsMapBuilder()
{
    return (int)[[TSUnityPlugin sharedInstance] addNewKeywordsMap];
}

void _keywordsMapBuilderSetGender(int ptr, int gender) {
    TSKeywordMap *map = [[TSUnityPlugin sharedInstance] getKeywordsMap:ptr];
    TSGender tsGender;
    // This is necessary because the enum values are different between TS iOS and Android SDK
    if (gender == 0) {
        tsGender = kTSGenderMale;
    } else if (gender == 1) {
        tsGender = kTSGenderFemale;
    } else {
        tsGender = kTSGenderUnknown;
    }
    [map setGender:tsGender];
}

void _keywordsMapBuilderSetBirthday(int ptr, const char * birthday) {
    NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
    [formatter setDateFormat:@"yyyy-MM-dd"];
    NSDate *date = [formatter dateFromString:CreateNSString(birthday)];
    
    TSKeywordMap *map = [[TSUnityPlugin sharedInstance] getKeywordsMap:ptr];
    [map setBirthday:date];
}

void _keywordsMapBuilderSetLocation(int ptr, float latitude, float longitude) {
    TSKeywordMap *map = [[TSUnityPlugin sharedInstance] getKeywordsMap:ptr];
    [map setLocation:[[CLLocation alloc] initWithLatitude:latitude longitude:longitude]];
}

void _keywordsMapBuilderSetValueForKey(int ptr, const char * value, const char * key) {
    TSKeywordMap *map = [[TSUnityPlugin sharedInstance] getKeywordsMap:ptr];
    [map setValue:CreateNSString(value) forKey:CreateNSString(key)];
}

int _newKeywordsMap(int ptr) {
    return ptr;
}
