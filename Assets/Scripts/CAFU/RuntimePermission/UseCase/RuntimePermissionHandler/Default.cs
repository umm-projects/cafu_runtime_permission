using UniRx;
using UnityEngine;

namespace CAFU.RuntimePermission.Domain.UseCase.RuntimePermissionHandler {

    public class Default : IRuntimePermissionHandler {

        public bool HasPermission(UserAuthorization userAuthorization) {
            return Application.HasUserAuthorization(userAuthorization);
        }

        public IObservable<bool> RequestPermission(UserAuthorization userAuthorization) {
            return Observable.Return(this.HasPermission(userAuthorization));
        }

    }

}