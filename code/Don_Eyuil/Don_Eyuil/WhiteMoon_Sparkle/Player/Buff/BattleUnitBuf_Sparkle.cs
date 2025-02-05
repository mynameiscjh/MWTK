namespace Don_Eyuil.WhiteMoon_Sparkle.Player.Buff
{
    public class BattleUnitBuf_Sparkle : BattleUnitBuf_Don_Eyuil
    {
        public static string Desc = "管理百般武艺";

        public BattleUnitBuf_Don_Eyuil PrimaryWeapon = null;
        public BattleUnitBuf_Don_Eyuil SubWeapon = null;
        static BattleUnitBuf_Sparkle _instance = null;
        public static BattleUnitBuf_Sparkle Instance
        {
            get
            {
                if (_instance == null)
                {
                    foreach (var item in BattleObjectManager.instance.GetAliveList())
                    {
                        if (GetBuf<BattleUnitBuf_Sparkle>(item) != null)
                        {
                            _instance = GetBuf<BattleUnitBuf_Sparkle>(item);
                            break;
                        }
                    }
                    return _instance;
                }
                return _instance;
            }
        }

        public void ReselectPrimaryWeapon()
        {
            MyTools.未实现提醒();
        }

        public void ReselectSubWeapon()
        {
            MyTools.未实现提醒();
        }

        public BattleUnitBuf_Sparkle(BattleUnitModel model) : base(model)
        {
        }
    }

}
