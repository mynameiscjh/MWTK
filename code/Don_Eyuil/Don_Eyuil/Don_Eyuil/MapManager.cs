using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;
using System.Reflection.Emit;
using HyperCard;
using UnityEngine;
using Don_Eyuil.Buff;
using static Don_Eyuil.PassiveAbility_DonEyuil_01;
using static Don_Eyuil.PassiveAbility_DonEyuil_02.HardBloodArtPair;
using static UnityEngine.UI.GridLayoutGroup;
using Don_Eyuil.Don_Eyuil.DiceCardSelfAbility;
using TMPro;
using CustomMapUtility;

namespace Don_Eyuil
{
    public class DonEyuilPhase1MapManager : CustomMapManager
    {
        protected override string[] CustomBGMs
        {
            get
            {
                return new string[] { "PastMoonLight_1.mp3" };
            }
        }
    }
    public class DonEyuilPhase2MapManager : CustomMapManager
    {
        protected override string[] CustomBGMs
        {
            get
            {
                return new string[] { "PastMoonLight_2_1.mp3" };
            }
        }
        public AudioSource currentEnemyTheme(BattleSoundManager BSM)
        {
            return BSM.GetFieldValue<AudioSource>("_currentEnemyTheme");
        }
        public AudioClip[] enemyThemeSound(BattleSoundManager BSM)
        {
            return BSM.GetFieldValue<AudioClip[]>("enemyThemeSound");
        }
        protected override void LateUpdate()
        {
            if(currentEnemyTheme(SingletonBehavior<BattleSoundManager>.Instance).clip == MyTools.CMH.GetAudioClip("PastMoonLight_2_1.mp3"))
            {
                currentEnemyTheme(SingletonBehavior<BattleSoundManager>.Instance).loop = false;
            }
            if (currentEnemyTheme(SingletonBehavior<BattleSoundManager>.Instance).clip == MyTools.CMH.GetAudioClip("PastMoonLight_2_1.mp3") && !currentEnemyTheme(SingletonBehavior<BattleSoundManager>.Instance).isPlaying)
            {
                currentEnemyTheme(SingletonBehavior<BattleSoundManager>.Instance).clip = MyTools.CMH.GetAudioClip("PastMoonLight_2_2.wav");
                currentEnemyTheme(SingletonBehavior<BattleSoundManager>.Instance).Play();
                currentEnemyTheme(SingletonBehavior<BattleSoundManager>.Instance).loop = true;
            }
        }
    }
    public class DonEyuilPhase3MapManager : CustomMapManager
    {
        protected override string[] CustomBGMs
        {
            get
            {
                return new string[] { "PastMoonLight_3.mp3" };
            }
        }
    }
}
