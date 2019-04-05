using Harmony;
using RoR2.Mods;
using RoR2;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace HuntressSprint
{
    public class HuntressSprint
    {
        [ModEntry("Huntress Sprint", "1.0.0", "Meepen")]
        public static void Init()
        {
            var harmony = HarmonyInstance.Create("dev.meepen.huntress-sprint");
            var method = typeof(PlayerCharacterMasterController).GetMethod("FixedUpdate", BindingFlags.Instance | BindingFlags.NonPublic);
            var prefix = typeof(SprintPatch).GetMethod("Prefix", BindingFlags.Static | BindingFlags.NonPublic);
            var postfix = typeof(SprintPatch).GetMethod("Postfix", BindingFlags.Static | BindingFlags.NonPublic);
            harmony.Patch(method, new HarmonyMethod(prefix), new HarmonyMethod(postfix));
        }
    }

    public class SprintPatch
    {
        static void Prefix(ref Vector3 __state, ref PlayerCharacterMasterController __instance)
        {
            var body = __instance.master.GetBodyObject().GetComponent<CharacterBody>();
            var inputs = __instance.master.GetBodyObject().GetComponent<InputBankTest>();
            __state = inputs.moveVector;

            if (body.baseNameToken == "HUNTRESS_BODY_NAME")
            {

                Vector3 aimDirection = inputs.aimDirection;
                aimDirection.y = 0f;
                aimDirection.Normalize();
                Vector3 moveVector = __state;
                moveVector.y = 0f;
                moveVector.Normalize();
                if (Vector3.Dot(aimDirection, moveVector) > -0.8f)
                    // Force dot to be 1
                    inputs.moveVector = inputs.aimDirection;
            }
        }
        
        static void Postfix(ref Vector3 __state, ref PlayerCharacterMasterController __instance)
        {
            var body = __instance.master.GetBodyObject().GetComponent<CharacterBody>();
            var inputs = __instance.master.GetBodyObject().GetComponent<InputBankTest>();
            inputs.moveVector = __state;
        }
    }
}