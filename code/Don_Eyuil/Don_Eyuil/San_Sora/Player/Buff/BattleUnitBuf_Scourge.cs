using LOR_BattleUnit_UI;

namespace Don_Eyuil.San_Sora.Player.Buff
{
    public class BattleUnitBuf_Scourge : BattleUnitBuf_SanHardBlood
    {
        public static string Desc = "本速度骰子中的书页对自身施加”流血”时将同时对两名敌方角色施加一次\r\n自身在一幕中若至少承受了15点”流血”伤害则在这一幕结束时恢复1点光芒";

        public BattleUnitBuf_Scourge(SpeedDiceUI dice) : base(dice)
        {
            //dice.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = TKS_BloodFiend_Initializer.ArtWorks["血鞭骰子"];
        }

        int count = 0;
        public override void AfterTakeBleedingDamage(int Dmg)
        {
            count += Dmg;
        }
        public override void OnRoundStart()
        {
            count = 0;
        }
        public override void OnRoundEnd()
        {
            if (count >= 15)
            {
                _owner.cardSlotDetail.RecoverPlayPoint(1);
            }
        }

        public override void BeforeOtherUnitAddKeywordBuf(KeywordBuf BufType, BattleUnitModel Target, ref int Stack)
        {
            if (_owner.currentDiceAction != Card)
            {
                return;
            }

            if (Target != Card.target)
            {
                return;
            }

            if (BufType != KeywordBuf.Bleeding)
            {
                return;
            }

            var list = BattleObjectManager.instance.GetAliveList_opponent(_owner.faction);
            list.Remove(Target);
            for (int i = 0; i < 2; i++)
            {
                if (list.Count <= 0)
                {
                    return;
                }
                var luckyDog = RandomUtil.SelectOne(list);
                luckyDog.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, Stack);
            }
        }
    }
}
