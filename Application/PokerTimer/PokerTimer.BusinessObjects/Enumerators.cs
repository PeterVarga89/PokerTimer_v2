using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTimer.BusinessObjects
{
    public enum eGameType
    {
        [Description("Vyberte si")]
        NotSet = 0,

        [Description("Freeze Out")]
        FreezeOut = 1,

        [Description("Limited Rebuy")]
        RebuyLimited = 2,

        [Description("Unlimited ReBuy")]
        RebuyUnlimited = 3,

        [Description("Double Chance")]
        DoubleChance = 4,

        [Description("Triple Chance")]
        TripleChance = 5,

        [Description("Freeroll")]
        FreeRoll = 6,

        [Description("Cash Game")]
        CashGame = 7,

        [Description("Double Trouble")]
        DoubleTrouble = 8
    }

    public enum eConnectionString
    {
        NotSet = 0,
        [Description(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|PSDB.mdf;Integrated Security=True")]
        Local = 1,

        [Description("Data Source=82.119.117.77;Initial Catalog=PokerSystem;Integrated Security=False;User Id=PokerTimer;Password=Poker1969;MultipleActiveResultSets=True")]
        Online = 2
    }

    public enum ePositions
    {
        [Description("50_30_20_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0")]
        NotSet = 0,

        [Description("50_30_20_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0")]
        p3,

        [Description("45_25_18_12_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0")]
        p4,

        [Description("40_23_16_12_9_0_0_0_0_0_0_0_0_0_0_0_0_0_0_0")]
        p5,

        [Description("38_22_15_11_8_6_0_0_0_0_0_0_0_0_0_0_0_0_0_0")]
        p6,

        [Description("35_21_15_11_8_6_4_0_0_0_0_0_0_0_0_0_0_0_0_0")]
        p7,

        [Description("34_20_14_11_8_6_4_3_0_0_0_0_0_0_0_0_0_0_0_0")]
        p8,

        [Description("33_19_14_11_8_6_4_3_2_0_0_0_0_0_0_0_0_0_0_0")]
        p9,

        [Description("32,00_19,50_13,70_10,00_7,70_5,80_4,50_3,20_2,30_1,30_0,00_0,00_0,00_0,00_0,00_0,00_0,00_0,00_0,00_0,00")]
        p10,

        [Description("31,90_19,50_12,85_9,35_7,25_5,55_4,40_3,30_2,50_1,70_1,70_0,00_0,00_0,00_0,00_0,00_0,00_0,00_0,00_0,00")]
        p11,

        [Description("31,80_19,40_12,75_9,25_6,95_5,45_4,20_3,20_2,40_1,60_1,60_1,40_0,00_0,00_0,00_0,00_0,00_0,00_0,00_0,00")]
        p12,

        [Description("31,80_19,40_12,75_9,25_6,75_5,25_4,00_3,00_2,20_1,50_1,50_1,30_1,30_0,00_0,00_0,00_0,00_0,00_0,00_0,00")]
        p13,

        [Description("31,50_18,90_12,00_9,50_6,75_5,25_4,00_3,00_2,20_1,50_1,50_1,30_1,30_1,30_0,00_0,00_0,00_0,00_0,00_0,00")]
        p14,

        [Description("31,50_18,90_11,50_9,00_6,75_5,25_4,00_3,00_2,20_1,50_1,50_1,30_1,30_1,30_1,00_0,00_0,00_0,00_0,00_0,00")]
        p15,

        [Description("31,00_18,40_11,50_9,00_6,75_5,25_4,00_3,00_2,20_1,50_1,50_1,30_1,30_1,30_1,00_1,00_0,00_0,00_0,00_0,00")]
        p16,

        [Description("31,00_18,40_11,50_8,50_6,50_5,00_4,00_3,00_2,20_1,50_1,50_1,30_1,30_1,30_1,00_1,00_1,00_0,00_0,00_0,00")]
        p17,

        [Description("31,00_18,40_10,50_8,50_6,50_5,00_4,00_3,00_2,20_1,50_1,50_1,30_1,30_1,30_1,00_1,00_1,00_1,00_0,00_0,00")]
        p18,

        [Description("30,00_18,40_10,50_8,50_6,50_5,00_4,00_3,00_2,20_1,50_1,50_1,30_1,30_1,30_1,00_1,00_1,00_1,00_1,00_0,00")]
        p19,

        [Description("30,00_17,40_10,50_8,50_6,50_5,00_4,00_3,00_2,20_1,50_1,50_1,30_1,30_1,30_1,00_1,00_1,00_1,00_1,00_1,00")]
        p20,

    }

    public enum ePositionSeqs
    {
        NotSet = 0,
        p3 = 0,
        p4 = 28,
        p5 = 37,
        p6 = 46,
        p7 = 55,
        p8 = 64,
        p9 = 73,
        p10 = 100,
        p11 = 110,
        p12 = 120,
        p13 = 130,
        p14 = 140,
        p15 = 150,
        p16 = 160,
        p17 = 170,
        p18 = 180,
        p19 = 190,
        p20 = 200
    }
}
