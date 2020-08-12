//
// NearbyInteraction bindings
//
// Authors:
//	Whitney Schmidt  <whschm@microsoft.com>
//
// Copyright 2020 Microsoft Inc.
//

using ObjCRuntime;
using Foundation;
using CoreFoundation;
using System;
using Vector3 = global::OpenTK.Vector3;

namespace NearbyInteraction {

    [NoWatch, NoTV, NoMac, iOS (14, 0)]
    [Static]
    interface NIErrorDomain
    {
        [Field ("NIErrorDomain")]
        NSString Domain { get; }
    }

    [NoWatch, NoTV, NoMac, iOS (14,0)]
    [BaseType (typeof (NSObject))]
    [DisableDefaultCtor]
    interface NIConfiguration : NSCopying, NSSecureCoding {}

    [NoWatch, NoTV, NoMac, iOS (14,0)]
    [BaseType (typeof (NSObject))]
    [DisableDefaultCtor]
    interface NIDiscoveryToken : NSCopying, NSSecureCoding {}

    [NoWatch, NoTV, NoMac, iOS (14,0)]
    [BaseType (typeof (NIConfiguration))]
    [DisableDefaultCtor]
    interface NINearbyPeerConfiguration
    {
        [Export ("peerDiscoveryToken", ArgumentSemantic.Copy)]
        NIDiscoveryToken PeerDiscoveryToken { get; }

        [Export ("initWithPeerToken:")]
        IntPtr Constructor (NIDiscoveryToken peerToken);
    }

    [NoWatch, NoTV, NoMac, iOS (14,0)]
    [BaseType (typeof (NSObject))]
    [DisableDefaultCtor]
    partial interface NINearbyObject : NSCopying, NSSecureCoding
    {
        [Export ("discoveryToken", ArgumentSemantic.Copy)]
        NIDiscoveryToken DiscoveryToken { get; }

        [Export ("distance")]
        float Distance { get; }

        [Export ("direction")]
        Vector3 Direction {
            [MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
            get;
        }

        [Field ("NINearbyObjectDistanceNotAvailable")]
        float DistanceNotAvailable { get; }

        // TODO: https://github.com/xamarin/maccore/issues/2274
        // We do not have generator support to trampoline Vector3 -> vector_float3 for Fields
        // There is support for Vector3 -> vector_float3 for properties
        // error BI1014: bgen: Unsupported type for Fields: global::OpenTK.Vector3 for 'NearbyInteraction.NINearbyObjectDistance DirectionNotAvailable'.
        // extern simd_float3 NINearbyObjectDirectionNotAvailable __attribute__((availability(ios, introduced=14.0))) __attribute__((availability(macos, unavailable))) __attribute__((availability(watchos, unavailable))) __attribute__((availability(tvos, unavailable))) __attribute__((visibility("default"))) __attribute__((availability(swift, unavailable)));
        // [Unavailable (PlatformName.Swift)]
        // [NoWatch, NoTV, NoMac, iOS (14, 0)]
        // [Field ("NINearbyObjectDirectionNotAvailable")]
        // [unsupported ExtVector: float __attribute__((ext_vector_type(3)))] NINearbyObjectDirectionNotAvailable { get; }
    }

    [NoWatch, NoTV, NoMac, iOS (14,0)]
    [BaseType (typeof (NSObject))]
    interface NISession
    {
        [Static]
        [Export ("supported")]
        bool IsSupported { [Bind ("isSupported")] get; }

        [Wrap ("WeakDelegate")]
        [NullAllowed]
        INISessionDelegate Delegate { get; set; }

        [NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        [NullAllowed, Export ("delegateQueue", ArgumentSemantic.Strong)]
        DispatchQueue DelegateQueue { get; set; }

        [NullAllowed, Export ("discoveryToken", ArgumentSemantic.Copy)]
        NIDiscoveryToken DiscoveryToken { get; }

        [NullAllowed, Export ("configuration", ArgumentSemantic.Copy)]
        NIConfiguration Configuration { get; }

        [Export ("runWithConfiguration:")]
        void Run (NIConfiguration configuration);

        [Export ("pause")]
        void Pause ();

        [Export ("invalidate")]
        void Invalidate ();
    }

    interface INISessionDelegate {}

    [NoWatch, NoTV, NoMac, iOS (14,0)]
    [Protocol]
    [Model (AutoGeneratedName = true)]
    [BaseType (typeof (NSObject))]
    interface NISessionDelegate
    {
        [Export ("session:didUpdateNearbyObjects:")]
        void DidSessionUpdateNearbyObjects (NISession session, NINearbyObject[] nearbyObjects);

        [Export ("session:didRemoveNearbyObjects:withReason:")]
        void DidSessionRemoveNearbyObjects (NISession session, NINearbyObject[] nearbyObjects, NINearbyObjectRemovalReason reason);

        [Export ("sessionWasSuspended:")]
        void SessionWasSuspended (NISession session);

        [Export ("sessionSuspensionEnded:")]
        void SessionSuspensionEnded (NISession session);

        [Export ("session:didInvalidateWithError:")]
        void DidSessionInvalidate (NISession session, NSError error);
    }
}