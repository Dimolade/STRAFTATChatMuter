using System;
using System.IO;
using UnityEngine;
using HarmonyLib;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using System.Collections;
using System.Collections.Generic;
using ComputerysModdingUtilities;
using TMPro;
using HeathenEngineering.DEMO;
using HeathenEngineering.SteamworksIntegration;

[assembly: StraftatMod(isVanillaCompatible: true)]

namespace STRAFTATMod
{
    [BepInPlugin("dimolade.dimolade.ChatMute", "Chat Mute", "1.0.0.0")]
    public class Loader : BaseUnityPlugin
    {
        private void Awake()
        {
            // Initialize Harmony
            var harmony = new Harmony("dimolade.harmony.ChatMute");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(LobbyChatUILogic), "HandleChatMessage")]
    class ChatPatcher
    {
        static bool Prefix(LobbyChatUILogic __instance, ref LobbyChatMsg message)
        {
            if (message.sender.IsMe) return true; // message is from the player (audio source will be mute)
            /*PauseManager.Instance.WriteOfflineLog("Chat received from: "+message.sender.Name);
            PauseManager.Instance.WriteOfflineLog(message.Message);*/
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("ClientInstance"))
            {
                if (gameObject.GetComponent<ClientInstance>().PlayerSteamID == message.sender.SteamId) // the id is the same
                {
                    //PauseManager.Instance.WriteOfflineLog("Message Skipped from "+message.sender.Name);
                    return !gameObject.GetComponent<AudioSource>().mute; // skip if they are muted
                }
            }
            return true; // continue for whatever reason
        }
    }
}
