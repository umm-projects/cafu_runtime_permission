using System;
using UniRx;
using UnityEngine;

namespace CAFU.RuntimePermission.Domain.UseCase {

    public interface IRuntimePermissionHandler {

        bool HasPermission(UserAuthorization userAuthorization);

        UniRx.IObservable<bool> RequestPermission(UserAuthorization userAuthorization);

    }

    public static class RuntimePermissionHandlerExtension {

        public static UniRx.IObservable<bool> CreateRuntimePermissionDialogResultObservable(this IRuntimePermissionHandler runtimePermissionHandler, UserAuthorization userAuthorization) {
            return Observable
                // ReSharper disable once InvokeAsExtensionMethod
                .Amb(
                    // 1秒経ってもアプリケーションのフォーカスが外れなかった場合
                    // ダイアログが出なかったと見なして、その時点でのパーミッションを返す（値は多分偽になる）
                    Observable
                        .Timer(TimeSpan.FromSeconds(1.0))
                        .TakeUntil(
                            Observable.EveryApplicationFocus().Where(x => !x).Take(1)
                        )
                        .Select(_ => runtimePermissionHandler.HasPermission(userAuthorization)),
                    // アプリケーションのフォーカスが外れた後に復帰してきた場合
                    // ダイアログが出たと見なして、その時点でのパーミッションを返す（値はユーザの選択によって変動しうる）
                    Observable
                        .ReturnUnit()
                        .SelectMany(_ => Observable.EveryApplicationFocus().Where(x => !x).Take(1))
                        .SelectMany(_ => Observable.EveryApplicationFocus().Where(x => x).Take(1))
                        .Select(_ => runtimePermissionHandler.HasPermission(userAuthorization))
                );
        }

    }

}