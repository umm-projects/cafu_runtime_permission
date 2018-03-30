using UniRx;
using UnityEngine;

namespace CAFU.RuntimePermission.Domain.UseCase.RuntimePermissionHandler {

    // ReSharper disable once InconsistentNaming
    public class iOS : IRuntimePermissionHandler {

        public bool HasPermission(UserAuthorization userAuthorization) {
            return Application.HasUserAuthorization(userAuthorization);
        }

        public IObservable<bool> RequestPermission(UserAuthorization userAuthorization) {
            if (this.HasPermission(userAuthorization)) {
                return Observable.Return(true);
            }
            Application.RequestUserAuthorization(userAuthorization);
            return Observable.EveryApplicationFocus().Where(x => true).Take(1).Select(_ => this.HasPermission(userAuthorization));
        }

    }

}