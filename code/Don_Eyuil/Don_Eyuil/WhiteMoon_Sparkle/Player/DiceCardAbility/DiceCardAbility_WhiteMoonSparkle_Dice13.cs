using System.Collections.Generic;
using System.Linq;

namespace Don_Eyuil.WhiteMoon_Sparkle.Player.DiceCardAbility
{
    public class DiceCardAbility_WhiteMoonSparkle_Dice13 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]触发目标层数最多的伤害类负面4次(仅限[烧伤], [流血], [妖灵], [腐蚀])";

        public override void OnSucceedAttack(BattleUnitModel target)
        {
            var buf_Burn = target.bufListDetail.GetActivatedBuf(KeywordBuf.Burn);
            var buf_Bleed = target.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding);
            var buf_Fairy = target.bufListDetail.GetActivatedBuf(KeywordBuf.Fairy);
            var buf_Decay = target.bufListDetail.GetActivatedBuf(KeywordBuf.Decay);
            var list = new List<BattleUnitBuf>();
            if (buf_Burn != null)
            {
                list.Add(buf_Burn);
            }
            if (buf_Bleed != null)
            {
                list.Add(buf_Bleed);
            }
            if (buf_Fairy != null)
            {
                list.Add(buf_Fairy);
            }
            if (buf_Decay != null)
            {
                list.Add(buf_Decay);
            }
            list.Sort((x, y) => y.stack - x.stack);
            if (list.Count > 0)
            {
                var buf = list.First();

                for (int i = 0; i < 4; i++)
                {
                    switch (buf.bufType)
                    {
                        case KeywordBuf.Burn: buf.OnRoundEnd(); break;
                        case KeywordBuf.Bleeding: buf.AfterDiceAction(behavior); break;
                        case KeywordBuf.Fairy: buf.AfterDiceAction(behavior); break;
                        case KeywordBuf.Decay: buf.OnTakeDamageByAttack(behavior, 0); break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
