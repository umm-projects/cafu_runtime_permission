using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace CAFU.RuntimePermission.Domain.UseCase.RuntimePermissionHandler {

    public class Android : IRuntimePermissionHandler {

            private const string JAVA_CLASS_NAME_UNITY_PLAYER = "com.unity3d.player.UnityPlayer";

            private const string JAVA_CLASS_NAME_PACKAGE_MANAGER = "android.content.pm.PackageManager";

            private const string JAVA_CLASS_NAME_CONTEXT_COMPAT = "android.support.v4.content.ContextCompat";

            private const string JAVA_METHOD_NAME_CHECK_SELF_PERMISSION = "checkSelfPermission";

            private const string JAVA_METHOD_NAME_REQUEST_PERMISSIONS = "requestPermissions";

            private const string JAVA_PROPERTY_NAME_PERMISSION_GRANTED = "PERMISSION_GRANTED";

            private const string JAVA_PROPERTY_NAME_CURRENT_ACTIVITY = "currentActivity";

            private static readonly Dictionary<UserAuthorization, string> PERMISSION_NAME_MAP = new Dictionary<UserAuthorization, string>() {
                { UserAuthorization.Microphone, "android.permission.RECORD_AUDIO" },
                { UserAuthorization.WebCam    , "android.permission.CAMERA" },
            };

            public bool HasPermission(UserAuthorization userAuthorization) {
                using (var packageManager = new AndroidJavaClass(JAVA_CLASS_NAME_PACKAGE_MANAGER))
                using (var activity = GetActivity())
                using (var compat = new AndroidJavaClass(JAVA_CLASS_NAME_CONTEXT_COMPAT)) {
                    return compat.CallStatic<int>(JAVA_METHOD_NAME_CHECK_SELF_PERMISSION, activity, PERMISSION_NAME_MAP[userAuthorization]) == packageManager.GetStatic<int>(JAVA_PROPERTY_NAME_PERMISSION_GRANTED);
                }
            }

            public IObservable<bool> RequestPermission(UserAuthorization userAuthorization) {
                if (this.HasPermission(userAuthorization)) {
                    return Observable.Return(true);
                }
                using (var activity = GetActivity())
                using (var compat = new AndroidJavaClass(JAVA_CLASS_NAME_CONTEXT_COMPAT)) {
                    compat.CallStatic(JAVA_METHOD_NAME_REQUEST_PERMISSIONS, activity, new [] { PERMISSION_NAME_MAP[userAuthorization] }, 0);
                }
                return Observable.EveryApplicationFocus().Where(x => true).Take(1).Select(_ => this.HasPermission(userAuthorization));
            }

            private static AndroidJavaObject GetActivity() {
                using (var unityPlayer = new AndroidJavaClass(JAVA_CLASS_NAME_UNITY_PLAYER)) {
                    return unityPlayer.GetStatic<AndroidJavaObject>(JAVA_PROPERTY_NAME_CURRENT_ACTIVITY);
                }
            }

        }

}