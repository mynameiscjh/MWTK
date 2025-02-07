using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.Buff
{
    public class BattleUnitBuf_Blood : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "埃尤尔之血（本buff仅能从堂埃尤尔获取）[中立buff]:\r\n本buff视为一把已装备的副武器 但不占用副武器位置\r\n自身每张书页首次命中目标时对目标造成一次目标当前流血层数50%的流血伤害\r\n自身命中带有流血的目标时在下一幕对其施加当前流血层数20%的流血\r\n";

        List<BattlePlayingCardDataInUnitModel> list = new List<BattlePlayingCardDataInUnitModel>();

        public override void OnSuccessAttack(BattleDiceBehavior behavior)
        {
            if (!behavior.card.target.bufListDetail.HasBuf<BattleUnitBuf_bleeding>())
            {
                return;
            }

            behavior.card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, behavior.card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding).stack / 5, _owner);

            if (list.Contains(behavior.card))
            {
                return;
            }
            list.Add(behavior.card);
            behavior.card.target.TakeDamage(behavior.card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding).stack / 2);
            BattleUnitBuf_Don_Eyuil.OnTakeBleedingDamagePatch.Trigger_BleedingDmg_After(_owner, behavior.card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding).stack / 2, KeywordBuf.Bleeding);
        }

        public override void OnRoundStart()
        {
            list.Clear();
        }

        public BattleUnitBuf_Blood(BattleUnitModel model) : base(model)
        {
        }
    }
}
