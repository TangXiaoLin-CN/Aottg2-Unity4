﻿using ApplicationManagers;
using Cameras;
using Characters;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicCameraBuiltin: CustomLogicBaseBuiltin
    {
        public CustomLogicCameraBuiltin(): base("Camera")
        {
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            var camera = (InGameCamera)SceneLoader.CurrentCamera;
            if (name == "SetManual")
            {
                CustomLogicManager.ManualCamera = true;
            }
            else if (name == "SetPosition")
            {
                var vectorBuiltin = (CustomLogicVector3Builtin)parameters[0];
                CustomLogicManager.CameraPosition = vectorBuiltin.Value;
                camera.SyncCustomPosition();
            }
            else if (name == "SetRotation")
            {
                var vectorBuiltin = (CustomLogicVector3Builtin)parameters[0];
                CustomLogicManager.CameraRotation = vectorBuiltin.Value;
                camera.SyncCustomPosition();
            }
            else if (name == "SetVelocity")
            {
                var vectorBuiltin = (CustomLogicVector3Builtin)parameters[0];
                CustomLogicManager.CameraVelocity = vectorBuiltin.Value;
            }
            else if (name == "LookAt")
            {
                var vectorBuiltin = (CustomLogicVector3Builtin)parameters[0];
                camera.Cache.Transform.LookAt(vectorBuiltin.Value);
                CustomLogicManager.CameraRotation = camera.Cache.Transform.rotation.eulerAngles;
            }
            return base.CallMethod(name, parameters);
        }
    }
}
