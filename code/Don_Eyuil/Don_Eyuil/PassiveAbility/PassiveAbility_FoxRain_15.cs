namespace Don_Eyuil
{



    public class PassiveAbility_FoxRain_15 : PassiveAbilityBase
    {
        public override string debugDesc => "堂埃尤尔派硬血术 0费 特殊\r\n自身拥有一套额外的卡组可设置\"硬血术\"书页\r\n情感等级达到0/2/4时可以选择激活设置的\"硬血术\"书页情感等级达到4级后每有一名角色因流血死亡则可额外激活一次设置的\"硬血术\"书页\r\n（这里的选择激活硬血术界面用选EGO的那个levelup的UI做)\r\n（实现上面，基本就是正常的多写一个卡组就可以了)\r\n\r\n自身可使用个人书页\"堂埃尤尔派硬血术终式\"且无法使用楼层E.G.O书页\r\n";

        public override void OnRoundStart()
        {
            if (owner.emotionDetail.EmotionLevel == 0 || owner.emotionDetail.EmotionLevel == 2 || owner.emotionDetail.EmotionLevel == 4)
            {

            }
        }
    }
}
