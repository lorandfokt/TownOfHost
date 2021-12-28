using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using System;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnhollowerBaseLib;
using TownOfHost;
using Hazel;

namespace TownOfHost {
    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    class DebugManager {
        private static Il2CppSystem.Collections.Generic.List<PlayerControl> bots = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
        static System.Random random = new System.Random();
        public static void Postfix(KeyboardJoystick __instance) {
            if(Input.GetKeyDown(KeyCode.L) && Input.GetKey(KeyCode.LeftShift) && AmongUsClient.Instance.AmHost) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EndGame, Hazel.SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.EndGame();
            }
            if(Input.GetKeyDown(KeyCode.X) && AmongUsClient.Instance.GameMode == GameModes.FreePlay) {
                PlayerControl.LocalPlayer.Data.Object.SetKillTimer(0f);
            }
            if(Input.GetKeyDown(KeyCode.Y)) {
                main.SyncCustomSettingsRPC();
            }

            if(main.OptionControllerIsEnable) {
                if(Input.GetKeyDown(KeyCode.UpArrow)) CustomOptionController.Up();
                if(Input.GetKeyDown(KeyCode.DownArrow)) CustomOptionController.Down();
                if(Input.GetKeyDown(KeyCode.RightArrow)) {
                    CustomOptionController.Enter();
                }
                if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                    CustomOptionController.Return();
                }
            }
            if(Input.GetKeyDown(KeyCode.Tab) && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Joined) {
                main.OptionControllerIsEnable = !main.OptionControllerIsEnable;
                CustomOptionController.currentPage = OptionPages.basepage;
                CustomOptionController.currentCursor = 0;
            } 
        }
    }
}