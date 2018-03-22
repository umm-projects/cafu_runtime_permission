using UniRx;
using UnityEngine;

namespace CAFU.RuntimePermission.Domain.UseCase {

    public interface IRuntimePermissionHandler {

        bool HasPermission(UserAuthorization userAuthorization);

        IObservable<bool> RequestPermission(UserAuthorization userAuthorization);

    }

}