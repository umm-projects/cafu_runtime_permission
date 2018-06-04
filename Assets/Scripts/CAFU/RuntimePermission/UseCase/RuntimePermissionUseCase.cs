using System;
using CAFU.Core.Domain.UseCase;
using CAFU.RuntimePermission.Domain.UseCase.RuntimePermissionHandler;
using UnityEngine;

namespace CAFU.RuntimePermission.Domain.UseCase {

    public interface IRuntimePermissionUserCase : IUseCase {

        bool HasPermission(UserAuthorization userAuthorization);

        IObservable<bool> RequestPermission(UserAuthorization userAuthorization);

    }

    public class RuntimePermissionUseCase : IRuntimePermissionUserCase {

        public class Factory : DefaultUseCaseFactory<RuntimePermissionUseCase> {

            protected override void Initialize(RuntimePermissionUseCase instance) {
                base.Initialize(instance);
                instance.Initialize();
            }

        }

        private IRuntimePermissionHandler RuntimePermissionHandler { get; set; }

        public bool HasPermission(UserAuthorization userAuthorization) {
            return this.RuntimePermissionHandler.HasPermission(userAuthorization);
        }

        public IObservable<bool> RequestPermission(UserAuthorization userAuthorization) {
            return this.RuntimePermissionHandler.RequestPermission(userAuthorization);
        }

        private void Initialize() {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (Application.platform) {
                case RuntimePlatform.IPhonePlayer:
                    this.RuntimePermissionHandler = new iOS();
                    break;
                case RuntimePlatform.Android:
                    this.RuntimePermissionHandler = new Android();
                    break;
                default:
                    this.RuntimePermissionHandler = new Default();
                    break;
            }
        }

    }

}