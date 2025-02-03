namespace Don_Eyuil.San_Sora.Player.DiceCardAbility
{
    public class DiceCardAbility_SanSora_Dice10 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]重复触发自身与目标情感等级之和次[流血]若目标因本效果死亡则对所有敌方角色施加目标此时拥有的[流血]";

        public override void OnSucceedAttack(BattleUnitModel target)
        {
            var bleed = target.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding);

            if (bleed == null) { return; }

            for (int i = 0; i < target.emotionDetail.EmotionLevel + owner.emotionDetail.EmotionLevel; i++)
            {
                if (target.IsDead())
                {
                    foreach (var item in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
                    {
                        item.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, bleed.stack, owner);
                    }
                }
                bleed.AfterDiceAction(behavior);
            }
        }
    }
}
