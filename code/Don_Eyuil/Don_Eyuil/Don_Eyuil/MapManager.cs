using CustomMapUtility;
using UnityEngine;

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

            if (currentEnemyTheme(SingletonBehavior<BattleSoundManager>.Instance).clip == MyTools.CMH.GetAudioClip("PastMoonLight_2_1.mp3") && !currentEnemyTheme(SingletonBehavior<BattleSoundManager>.Instance).isPlaying)
            {
                currentEnemyTheme(SingletonBehavior<BattleSoundManager>.Instance).clip = MyTools.CMH.GetAudioClip("PastMoonLight_2_2.wav");
                currentEnemyTheme(SingletonBehavior<BattleSoundManager>.Instance).Play();
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
