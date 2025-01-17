using System.Collections.Generic;

namespace Don_Eyuil.Buff
{
    public class BattleUnitBuf_Blade : BattleUnitBuf
    {
        public static string Desc = "自身命中处于混乱状态的目标时将触发目标\"流血\"(每张书页至多1次)\r\n一幕中敌方每受到20点\"流血\"伤害便使自身抽取一张书页(每幕至多触发1次)\r\n";

        public List<BattlePlayingCardDataInUnitModel> cards = new List<BattlePlayingCardDataInUnitModel>();

        public override void OnRoundStart()
        {
            foreach (var item in BattleObjectManager.instance.GetAliveList_opponent(_owner.faction))
            {
                BattleUnitBuf_BleedCount.T39 = _owner;
                var buf = new BattleUnitBuf_BleedCount();
                item.bufListDetail.AddBuf(buf);
            }
        }

        public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
        {
            cards.Add(card);
        }

        public override void OnSuccessAttack(BattleDiceBehavior behavior)
        {
            if (cards.Exists(x => x == behavior.card))
            {
                if (behavior.card.target.IsBreakLifeZero())
                {
                    var buf = behavior.card.target.bufListDetail.GetFieldValue<List<BattleUnitBuf>>("_bufList").Find(x => x is BattleUnitBuf_bleeding) as BattleUnitBuf_bleeding;
                    if (buf != null)
                    {
                        var temp = behavior.card.CopyDiceBehaviour(behavior);
                        buf.AfterDiceAction(temp);
                    }
                }
            }
        }

        public class BattleUnitBuf_BleedCount : BattleUnitBuf
        {
            public int count = 0;
            public bool fl = false;
            public static BattleUnitModel T39 = null;

            public override void OnRoundStart()
            {
                count = 0;
                fl = false;
            }

            public override float DmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None)
            {
                if (keyword == KeywordBuf.Bleeding)
                {
                    count += dmg;
                }

                if (count > 20 && T39 != null && !fl)
                {
                    T39.allyCardDetail.DrawCards(1);
                    fl = true;
                    count -= 20;
                }

                return base.DmgFactor(dmg, type, keyword);
            }

            public override void OnRoundEnd()
            {
                this.Destroy();
            }

        }

    }
}
