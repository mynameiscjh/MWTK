using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Don_Eyuil
{
    public class DiceCardAbility_DonEyuil_02 : DiceCardAbilityBase
    {
        public static string Desc = "[命中时]对双方施加3层[流血]";
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3, owner);
            if(target != null)
            {
                target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3, owner);
            }

        }
    }

}
