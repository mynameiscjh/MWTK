using HarmonyLib;

namespace Don_Eyuil.San_Sora.Player.Buff
{
    public class BattleUnitBuf_Feather : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "血羽\r\n自身速度不低于5时闪避型骰子最小值+1\r\n可用于特殊书页\r\n本状态根据层数获得相应强化(最大值:30层)\r\n达到10层以上转化为血羽II\r\n血羽II\r\n自身速度不低于5时自身所有骰子最大值与最小值+1\r\n可用于特殊书页\r\n本状态根据层数获得相应强化(最大值:30层)\r\n达到10层以上转化为血羽III\r\n血羽III\r\n自身速度不低于5时施加的”流血”层数+1\r\n每有15层本效果便使自身所有骰子最大值与最小值+1 🐟";

        protected override string keywordId => $"BattleUnitBuf_Bloodfeather_{stage}";

        public int stage = 1;

        public override int GetMaxStack()
        {
            return 30;
        }

        public override void OnAddBuf(int addedStack)
        {
            if (this.stack >= 10 && stage == 1)
            {
                stage = 2;
            }
            if (this.stack >= 20 && stage == 2)
            {
                stage = 3;
            }
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks[$"血羽{stage}"]);
        }

        public BattleUnitBuf_Feather(BattleUnitModel model) : base(model)
        {
            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, TKS_BloodFiend_Initializer.ArtWorks["血羽1"]);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (behavior.card.speedDiceResultValue >= 5)
            {
                if (behavior.Detail == LOR_DiceSystem.BehaviourDetail.Evasion && stage == 1)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus() { min = 1 });
                }
                if (stage == 2)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus() { min = 1, max = 1 });
                }
            }
            if (this.stage == 3 && this.stack >= 15)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { min = this.stack / 15, max = this.stack / 15 });
            }
        }

        public override void BeforeAddKeywordBuf(KeywordBuf BufType, ref int Stack)
        {
            if (owner.currentDiceAction == null)
            {
                return;
            }

            if (owner.currentDiceAction.speedDiceResultValue >= 5 && stage == 3 && BufType == KeywordBuf.Bleeding)
            {
                Stack += 1;
            }
        }
        int count = 0;
        public static int UsedStack = 0;
        public override void OnUseBuf(ref int stack)
        {
            UsedStack += stack;
            if (GetBuf<BattleUnitBuf_Lance>(_owner) != null && _owner.currentDiceAction != null && _owner.currentDiceAction == GetBuf<BattleUnitBuf_Lance>(_owner).Card && _owner.currentDiceAction.speedDiceResultValue >= 6)
            {
                GainBuf<BattleUnitBuf_Feather>(_owner, stack / 2);
            }
            if (_owner.personalEgoDetail.GetCardAll().Exists(x => x.GetID() == MyId.Card_桑空派变体硬血术终式_La_Sangre) && UsedStack - count >= 3)
            {
                var card = _owner.personalEgoDetail.GetCardAll().Find(x => x.GetID() == MyId.Card_桑空派变体硬血术终式_La_Sangre);
                card?.AddCoolTime((UsedStack - count) / 3);
                count = UsedStack;
            }
        }

    }
}
