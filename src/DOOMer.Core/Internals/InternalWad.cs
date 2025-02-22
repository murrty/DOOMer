namespace DOOMer.Core;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HashSlingingSlasher;

public sealed class InternalWad {
    #region Internal data
    public static bool ScannedForDuplicates { get; private set; }
    public static InternalWad InvalidIWAD { get; } = new(name: "Invalid IWAD", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "NULL.IWAD", note: "Not an IWAD.", wadType: null);
    public static InternalWad InvalidPWAD { get; } =
        new(name: "Invalid PWAD", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "NULL.PWAD", note: "Not a PWAD.", wadType: null);

    private const string INCORRECTLY_MARKED_IWAD_NOTE = """
        Incorrectly marked as a PWAD.
        This will cause some source ports to produce warnings, or malfunction entirely.
        """;
    private const string MACOSX_NOTE = "Mac OS X version.";
    private const string DOOM1_DEFAULT_NOTE = """
        Cut-down version of 'DOOM.WAD', which contains only the first episode.
        Later on, 'Doom 1" became a retronym for Doom after the release of Doom II.
        """;
    private const string DOOM64_DEFAULT_NOTE = """
        Made compatible with Doom 64 Ex to utilize port-specific features such as mouselook,
        or to play on unsupported platforms such as linux.
        'The Lost Levels' remain incompatible.
        """;
    private const string HERETIC1_DEFAULT_NOTE = "Cut-down version of 'HERETIC.WAD', which contains only the first episode.";
    private const string HEXDD_DEFAULT_NOTE = """
        While technically an IWAD, the only data in the file are extra levels,
        as well as their associated lumps,
        and some small graphics and end message cahnges,
        so the game will not run without a Hexen IWAD.
        """;
    private const string STRIFE0_DEFAULT_NOTE = """
        Cut-down version of 'STRIFE1.WAD', which contains only three levels:
        'MAP32: Sanctuary', 'MAP33: Town', and 'MAP34: Movement Base'.
        'VOICES.WAD' is not present, but necessary voice clips are included in the WAD.
        """;
    private const string SVE_DEFAULT_NOTE = """
        Includes GL nodes and related supplemental data for the original maps;
        four new maps; some new flats and sprites; and several new menu and console fonts.
        Automatically loaded by the game.
        """;
    private const string EXTRAS_DEFAULT_NOTE = """
        Contains additional resources used by the KEX port,
        namely the skull, health bonus, and interrogation mark icons used for the automap statistics,
        the weapon sprites shown on the weapon selector, crosshair graphics, and 'DSSECRET' sound lump.
        """;
    private const string EXTRAS_KEX_DEFAULT_NOTE = """
        Additionally, the KEX version added two different 'Ogg Vorbis' versions of the Doom and Doom II soundrack,
        one being a recording from an SC-55, the other being Andrew Hulshult's IDKFA remix.
        As a result, it is now significantly large.
        """;
    private const string ID1_RES_DEFAULT_NOTE = """
        Contains copies of the textures, sprites, sounds, and MBF21 actor definitions used in the expansion,
        allowing modders to create custom maps with these resources.
        The new weapons are includede separately, so as not to force replacement of the plasma gun and BFG9000.
        """;
    private const string ID1_WEAP_DEFAULT_NOTE = """
        Contains copies of the new weapon code used in the expansion,
        as well as a status bar replacement to show "FUEL" instead of "CELL".
        The actual weapon sprites and sounds are included separately, which must also be loaded.
        """;

    public static IReadOnlyList<InternalWad> DOOM1 { get; } = [
        new(name: "Doom Shareware", version: "1.0", nameSubtext: null,
            md5: "90facab21eede7981be10790e3f82da2", sha1: "fc0359e191bd257b3507863ae412ef3250515866", crc32: "eedae672",
            date: "1993-12-10", size: 4_207_819, entries: 1_270,
            wadName: "DOOM1.WAD", note: DOOM1_DEFAULT_NOTE, wadType: WadType.IWAD),

        new(name: "Doom Shareware", version: "1.1-a", nameSubtext: null,
            md5: "cea4989df52b65f4d481b706234a3dca", sha1: "9a24a7093ea0e78fd85f9923e55c55e79491b6a1", crc32: "289f4d3f",
            date: "1993-12-15", size: 4_274_218, entries: 1_270,
            wadName: "DOOM1.WAD", note: $$"""
            {{DOOM1_DEFAULT_NOTE}}
            Earlier release published on 1993-12-15, quickly superseded by 1.1b released 1993-12-16.
            """, wadType: WadType.IWAD),

        new(name: "Doom Shareware", version: "1.1-b", nameSubtext: null,
            md5: "52cbc8882f445573ce421fa5453513c1", sha1: "d4dc6806abd96bd93570c8df436fb6956e13d910", crc32: "981dcebb",
            date: "1993-12-16", size: 4_274_218, entries: 1_270,
            wadName: "DOOM1.WAD", note: $$"""
            {{DOOM1_DEFAULT_NOTE}}
            Earlier release published on 1993-12-15, quickly superseded by 1.1b released 1993-12-16.
            """, wadType: WadType.IWAD),

        new(name: "Doom Shareware", version: "1.2-a", nameSubtext: null,
            md5: "2a380f28e813fb0989cae5e4762ebb4c", sha1: "eaa66abb5e0b9c22f2f314b6599dcee871f57ed7", crc32: "023c2f84",
            date: "1994-02-04", size: 4_260_146, entries: 1_240,
            wadName: "DOOM1.WAD", note: $$"""
            {{DOOM1_DEFAULT_NOTE}}
            Earlier release published on 1994-02-04 for NeXTSTEP and possibly Mac OS, later superseded by 1.2b, released 1994-02-17.
            """, wadType: WadType.IWAD),

        new(name: "Doom Shareware", version: "1.2-b", nameSubtext: null,
            md5: "30aa5beb9e5ebfbbe1e1765561c08f38", sha1: "77ef34de7f13dc36b792fb82ed6805e9c1dc7afc", crc32: "bc842626",
            date: " 1994-02-17", size: 4_225_504, entries: 1_241,
            wadName: "DOOM1.WAD", note: $$"""
            {{DOOM1_DEFAULT_NOTE}}
            Earlier release published on 1994-02-04 for NeXTSTEP and possibly Mac OS, later superseded by 1.2b, released 1994-02-17.
            """, wadType: WadType.IWAD),

        new(name: "Doom Shareware", version: "1.25", nameSubtext: null,
            md5: "17aebd6b5f2ed8ce07aa526a32af8d99", sha1: "72caf585f7ce56861d25f8580c1cc82bf50abd1b", crc32: "225d7fb1",
            date: " 1994-04-21", size: 4_225_460, entries: 1_243,
            wadName: "DOOM1.WAD", note: DOOM1_DEFAULT_NOTE, wadType: WadType.IWAD),

        new(name: "Doom Shareware", version: "1.4", nameSubtext: null,
            md5: "a21ae40c388cb6f2c3cc1b95589ee693", sha1: "b4a8e93f1f9544210a173035a0b04c19eb283a2a", crc32: "f5c2708d",
            date: "1994-06-28", size: 4_261_144, entries: 1_256,
            wadName: "DOOM1.WAD", note: DOOM1_DEFAULT_NOTE, wadType: WadType.IWAD),

        new(name: "Doom Shareware", version: "1.5", nameSubtext: null,
            md5: "e280233d533dcc28c1acd6ccdc7742d4", sha1: "b559ba93d0a96e242eb6ded9deeedbd6f79d40fc", crc32: "8653b0eb",
            date: "1994-07-08", size: 4_271_324, entries: 1_256,
            wadName: "DOOM1.WAD", note: DOOM1_DEFAULT_NOTE, wadType: WadType.IWAD),

        new(name: "Doom Shareware", version: "1.6", nameSubtext: null,
            md5: "762fd6d4b960d4b759730f01387a50a1", sha1: "1437fc1ac25a17d5b3cef4c9d2f74e40cae3d231", crc32: "f26dcad8",
            date: "1994-08-03", size: 4_211_660, entries: 1_256,
            wadName: "DOOM1.WAD", note: DOOM1_DEFAULT_NOTE, wadType: WadType.IWAD),

        new(name: "Doom Shareware", version: "1.666", nameSubtext: null,
            md5: "c428ea394dc52835f2580d5bfd50d76f", sha1: "81535778d0d4c0c7aa8616fbfd3607dfb3dfd643", crc32: "505fb740",
            date: "1994-08-30", size: 4_234_124, entries: 1_264,
            wadName: "DOOM1.WAD", note: DOOM1_DEFAULT_NOTE, wadType: WadType.IWAD),

        new(name: "Doom Shareware", version: "1.8", nameSubtext: null,
            md5: "5f4eb849b1af12887dec04a2a12e5e62", sha1: "c6612ac5a8ac2e2a1d707f9b2869af820efb7c50", crc32: "331ebf07",
            date: "1994-12-10", size: 4_196_020, entries: 1_264,
            wadName: "DOOM1.WAD", note: DOOM1_DEFAULT_NOTE, wadType: WadType.IWAD),

        new(name: "Doom Shareware", version: "1.9", nameSubtext: null,
            md5: "f0cefca49926d00903cf57551d901abe", sha1: "5b2e249b9c5133ec987b3ea77596381dc0d6bc1d", crc32: "162b696a",
            date: "1995-02-01", size: 4_196_020, entries: 1_264,
            wadName: "DOOM1.WAD", note: DOOM1_DEFAULT_NOTE, wadType: WadType.IWAD),
    ];
    public static IReadOnlyList<InternalWad> DOOM { get; } = [
        new(name: "Doom Alpha", version: "0.2", nameSubtext: null,
            md5: "740901119ba2953e3c7f3764eca6e128", sha1: "89d934616c57fe974b06c2b37a9837853a89dbbc", crc32: "2587d97b",
            date: "1993-02-04", size: 496_250, entries: 198,
            wadName: "DOOM.WAD", note: """
            Referred to as a 'tech demo' and the earliest known alpha version of Doom,
            released to testers outside of id Software on February 4, 1993,
            when only 2 months of work had been done on the game.
            """, wadType: WadType.IWAD),
        new(name: "Doom Alpha", version: "0.3", nameSubtext: null,
            md5: "dae9b1eea1a8e090fdfa5707187f4a43", sha1: "df8ffe821a212d130ae48cf2c23721bd0ee6543b", crc32: "f97fe671",
            date: "1993-02-28", size: 1_901_322, entries: 670,
            wadName: "DOOM.WAD", note: """
            Apparently never released to id's alpha testers.
            Notable features include several maps in their earliest known forms,
            as well as a fully function on-screen automap.
            """, wadType: WadType.IWAD),
        new(name: "Doom Alpha", version: "0.4", nameSubtext: null,
            md5: "b6afa12a8b22e2726a8ff5bd249223de", sha1: "5f78b23fbffc828f5863ecff7e908d556241ff45", crc32: "c8a8b5ea",
            date: "1993-04-03", size: 2_675_669, entries: 859,
            wadName: "DOOM.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom Alpha", version: "0.5", nameSubtext: null,
            md5: "9c877480b8ef33b7074f1f0c07ed6487", sha1: "d3648d720b5324ce3c7bf58cf019e395911d677e", crc32: "8fe33445",
            date: "1993-05-23", size: 3_522_207, entries: 1_099,
            wadName: "DOOM.WAD", note: null, wadType: WadType.IWAD),

        new(name: "Doom Press Release Beta", version: null, nameSubtext: null,
            md5: "049e32f18d9c9529630366cfc72726ea", sha1: "692994db9579be4201730b9ac77797fae2111bde", crc32: "ff9bd960",
            date: "1993-10-04", size: 5_468_456, entries: 1_493,
            wadName: "DOOM.WAD", note: null, wadType: WadType.IWAD),

        new(name: "Doom Registered", version: "1.1", nameSubtext: null,
            md5: "981b03e6d1dc033301aa3095acc437ce", sha1: "df0040ccb29cc1622e74ceb3b7793a2304cca2c8", crc32: "66457ab9",
            date: "1993-12-16", size: 10_396_254, entries: 2_074,
            wadName: "DOOM.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom Registered", version: "1.2", nameSubtext: null,
            md5: "792fd1fea023d61210857089a7c1e351", sha1: "b5f86a559642a2b3bdfb8a75e91c8da97f057fe6", crc32: "a5da8930",
            date: "1994-02-17", size: 10_399_316, entries: 2_045,
            wadName: "DOOM.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom Registered", version: "1.666", nameSubtext: null,
            md5: "54978d12de87f162b9bcc011676cb3c0", sha1: "2e89b86859acd9fc1e552f587b710751efcffa8e", crc32: "f756aab5",
            date: "1994-09-01", size: 11_159_840, entries: 2_194,
            wadName: "DOOM.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom Registered", version: "1.8", nameSubtext: null,
            md5: "11e1cd216801ea2657723abc86ecb01f", sha1: "2c8212631b37f21ad06d18b5638c733a75e179ff", crc32: "8d242df9",
            date: "1995-01-20", size: 11_159_840, entries: 2_194,
            wadName: "DOOM.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom Registered", version: "1.9", nameSubtext: null,
            md5: "1cd63c5ddff1bf8ce844237f580e9cf3", sha1: "7742089b4468a736cadb659a7deca3320fe6dcbd", crc32: "723e60f9",
            date: "1995-02-01", size: 11_159_840, entries: 2_194,
            wadName: "DOOM.WAD", note: null, wadType: WadType.IWAD),

        new(name: "The Ultimate Doom", version: "1.9u", nameSubtext: null,
            md5: "c4fe9fd920207691a9f493668e0a2083", sha1: "9b07b02ab3c275a6a7570c3f73cc20d63a0e3833", crc32: "bf0eaac0",
            date: "1995-05-25", size: 12_408_292, entries: 2_306,
            wadName: "DOOMU.WAD", note: null, wadType: WadType.IWAD),

        new(name: "The Ultimate Doom", version: "1.9u", nameSubtext: "BFG Edition",
            md5: "fb35c4a5a9fd49ec29ab6e900572c524", sha1: "e5ec79505530e151ff0e6f517f3ce1fd65969c46", crc32: "5efa677e",
            date: null, size: 12_487_824, entries: 2_312,
            wadName: "DOOMU.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
        new(name: "The Ultimate Doom", version: "1.9u", nameSubtext: "Xbox Doom 3 bundle",
            md5: "0c8758f102ccafe26a3040bee8ba5021", sha1: "1d1d4f69fe14fa255228d8243470678b1b4efdc5", crc32: "ff1ba733",
            date: """
            Included in Doom 3: Resurrection of Evil expansion.
            """, size: 12_538_385, entries: 2_318,
            wadName: "DOOMU.WAD", note: null, wadType: WadType.IWAD),
        new(name: "The Ultimate Doom", version: "1.9u", nameSubtext: "PSN Classic Complete",
            md5: "e4f120eab6fb410a5b6e11c947832357", sha1: "117015379c529573510be08cf59810aa10bb934e", crc32: "3f646587",
            date: null, size: 12_474_561, entries: 2_307,
            wadName: "DOOMU.WAD", note: null, wadType: WadType.IWAD),
        new(name: "The Ultimate Doom", version: "1.9u", nameSubtext: "Xbox 360 BFG Edition",
            md5: "7912931e44c7d56e021084a256659800", sha1: "d6a9f0172eca101471128ec61be975361f2ad28e", crc32: "6010fd43",
            date: null, size: 12_474_561, entries: 2_307,
            wadName: "DOOMU.WAD", note: null, wadType: WadType.IWAD),
        new(name: "The Ultimate Doom", version: "1.9u", nameSubtext: "XBLA Doom",
            md5: "72286ddc680d47b9138053dd944b2a3d", sha1: "37de4510216eb3ce9a835dd939109443375d10c5", crc32: "b8583cd5",
            date: null, size: 12_475_196, entries: 2_307,
            wadName: "DOOMU.WAD", note: null, wadType: WadType.IWAD),
        new(name: "The Ultimate Doom", version: "1.9u", nameSubtext: "Doom PocketPC",
            md5: "88ce96442d269ef515b39fe34f08a9b7", sha1: "23a3a8bfafcfdc7c481f282cf2a3d03e5f386d43", crc32: "0874d1c3",
            date: null, size: 14_445_628, entries: 2_305,
            wadName: "doom1.wad", note: $$"""
            {{INCORRECTLY_MARKED_IWAD_NOTE}}

            The WAD is modified during installation to include 4 extra bytes at the end of the file as part of the copy protection.
            """, wadType: WadType.IWADIncorrectlyMarked),

        new(name: "Doom I Enhanced", version: "1.0", nameSubtext: "Unity Port",
            md5: "232a79f7121b22d7401905ee0ee1e487", sha1: "f770111ca9eb6d49aead51fcbd398719b462e64b", crc32: "46359dfb",
            date: null, size: 12_468_955, entries: 2_307,
            wadName: "DOOM.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom I Enhanced", version: "1.1", nameSubtext: "Unity Port",
            md5: "21b200688d0fa7c1b6f63703d2bdd455", sha1: "08ab2507e1d525c4c06b6df4f6d5862568a6b009", crc32: "346a4bfd",
            date: null, size: 12_468_955, entries: 2_307,
            wadName: "DOOM.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom I Enhanced", version: "1.3", nameSubtext: "Unity Port",
            md5: "8517c4e8f0eef90b82852667d345eb86", sha1: "2a8a1ce0f29497a2781b2902c76115fd60d8bbf8", crc32: "75c3b7bf",
            date: null, size: 12_702_921, entries: 2_307,
            wadName: "DOOM.WAD", note: null, wadType: WadType.IWAD),

        new(name: "Doom: KEX Edition", version: "1.0", nameSubtext: "Doom + Doom II",
            md5: "2a762516da425645ce21a2119979d235", sha1: "20dbeb1e14c8cef08ac888a1162241757fb10131", crc32: "e441b887",
            date: null, size: 12_727_528, entries: 2_308,
            wadName: "DOOM.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom: KEX Edition", version: "1.1/2", nameSubtext: "Doom + Doom II",
            md5: "4461d4511386518e784c647e3128e7bc", sha1: "997bae5e5a190c5bb3b1fb9e7e3e75b2da88cb27", crc32: "cff03d9f",
            date: "2024-08-06", size: 12_733_492, entries: 2_308,
            wadName: "DOOM.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom: KEX Edition", version: "1.3", nameSubtext: "Doom + Doom II",
            md5: "3b37188f6337f15718b617c16e6e7a9c", sha1: "87651324502044f9a6eed403e48853aa16c93e49", crc32: "d5f8c089",
            date: "2024-10-01", size: 12_996_515, entries: 2_308,
            wadName: "DOOM.WAD", note: null, wadType: WadType.IWAD),
    ];
    public static IReadOnlyList<InternalWad> DOOM2 { get; } = [
        new(name: "Doom II: Hell on Earth", version: "1.666", nameSubtext: null,
            md5: "30e3c2d0350b67bfbf47271970b74b2f", sha1: "6d559b7ceece4f5ad457415049711992370d520a", crc32: "e2a683bd",
            date: "1994-08-29", size: 14_943_400, entries: 2_956,
            wadName: "DOOM2.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom II: Hell on Earth", version: "1.666g", nameSubtext: "German release",
            md5: "d9153ced9fd5b898b36cc5844e35b520", sha1: "a4ce5128d57cb129fdd1441c12b58245be55c8ce", crc32: "c08005f7",
            date: "1994-08-29", size: 14_824_716, entries: 2_934,
            wadName: "DOOM2.WAD", note: "This version was used for the German release.", wadType: WadType.IWAD),
        new(name: "Doom II: Hell on Earth", version: "1.7", nameSubtext: null,
            md5: "ea74a47a791fdef2e9f2ea8b8a9da13b", sha1: "78009057420b792eacff482021db6fe13b370dcc", crc32: "47daeb2e",
            date: "1994-09-21", size: 14_612_688, entries: 2_919,
            wadName: "DOOM2.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom II: Hell on Earth", version: "1.7a", nameSubtext: null,
            md5: "d7a07e5d3f4625074312bc299d7ed33f", sha1: "70192b8d5aba65c7e633a7c7bcfe7e3e90640c97", crc32: "952f6baa",
            date: "1994-10-18", size: 14_612_688, entries: 2_919,
            wadName: "DOOM2.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom II: Hell on Earth", version: "1.8", nameSubtext: null,
            md5: "c236745bb01d89bbb866c8fed81b6f8c", sha1: "79c283b18e61b9a989cfd3e0f19a42ea98fda551", crc32: "31bd3bc0",
            date: "1995-01-20", size: 14_612_688, entries: 2_919,
            wadName: "DOOM2.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom II: Hell on Earth", version: "1.8f", nameSubtext: "French release",
            md5: "3cb02349b3df649c86290907eed64e7b", sha1: "d510c877031bbd5f3d198581a2c8651e09b9861f", crc32: "27eaae69",
            date: "1994-12-01", size: 14_607_420, entries: 2_914,
            wadName: "DOOM2.WAD", note: "This version was used for the French release.", wadType: WadType.IWAD),
        new(name: "Doom II: Hell on Earth", version: "1.9", nameSubtext: null,
            md5: "25e1459ca71d321525f84628f45ca8cd", sha1: "7ec7652fcfce8ddc6e801839291f0e28ef1d5ae7", crc32: "ec8725db",
            date: "1995-02-01", size: 14_604_584, entries: 2_919,
            wadName: "DOOM2.WAD", note: null, wadType: WadType.IWAD),

        new(name: "Doom II: Hell on Earth", version: "1.9", nameSubtext: "PC-98",
            md5: "b96683d113c4f4e9a916e1c7d1d71ffd", sha1: "c78e5f516eb8f26aafd6e487aab3e7323b672fb6", crc32: "dbaa4a2b",
            date: "1995-02-01", size: 14_604_584, entries: 2_919,
            wadName: "DOOM2.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom II: Hell on Earth", version: "1.9", nameSubtext: "BFG Edition",
            md5: "c3bea40570c23e511a7ed3ebcd9865f7", sha1: "a59548125f59f6aa1a41c22f615557d3dd2e85a9", crc32: "927a778a",
            date: null, size: 14_691_821, entries: 2_935,
            wadName: "DOOM2.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
        new(name: "Doom II: Hell on Earth", version: "1.9", nameSubtext: "Xbox Doom 3 bundle",
            md5: "a793ebcdd790afad4a1f39cc39a893bd", sha1: "1c91d86cd8a2f3817227986503a6672a5e1613f0", crc32: "218030c8",
            date: """
            Included in Doom 3: Resurrection of Evil expansion.
            """, size: 14_683_458, entries: 2_931,
            wadName: "DOOM2.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom II: Hell on Earth", version: "1.9", nameSubtext: "PSN Classic Complete",
            md5: "4c3db5f23b145fccd24c9b84aba3b7dd", sha1: "ca8db908a7c9fbac764f34c148f0bcc78d18553e", crc32: "7755acfc",
            date: null, size: 14_599_800, entries: 2_919,
            wadName: "DOOM2.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom II: Hell on Earth", version: "1.9", nameSubtext: "PSN Europe Classic Complete",
            md5: "97573aaf26957099ed45e61d81a0a1a3", sha1: "f1b6ba94352d53f646b67c01d2da88c5c40e3179", crc32: "62fd057f",
            date: null, size: 14_603_212, entries: 2_919,
            wadName: "DOOM2.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
        new(name: "Doom II: Hell on Earth", version: "1.9", nameSubtext: "Xbox 360/PS3 BFG Edition",
            md5: "f617591a6c5d07037eb716dc4863e26b", sha1: "b7ba1c68631023ea1aab1d7b9f7f6e9afc508f39", crc32: "1350e452",
            date: null, size: 14_677_988, entries: 2_931,
            wadName: "DOOM2.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom II: Hell on Earth", version: "1.9", nameSubtext: "XBLA Doom II",
            md5: "43c2df32dc6c740cb11f34dc5ab693fa", sha1: "55e445badd63d8841ebea887910c26c62c7f525e", crc32: "3f2b4852",
            date: null, size: 14_685_034, entries: 2_931,
            wadName: "DOOM2.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom II: Hell on Earth", version: "1.9", nameSubtext: "Tapwave Zodiac",
            md5: "9640fc4b2c8447bbd28f2080725d5c51", sha1: "2cda310805397ae44059bbcaed3cd602f4864a82", crc32: "541a97c2",
            date: null, size: 14_639_397, entries: 2_923,
            wadName: "DOOM2.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),

        new(name: "Doom II Enhanced", version: "1.0", nameSubtext: "Unity Port",
            md5: "e7395bd5e838d58627bd028871efbc14", sha1: "9b39107b5bcfd1f989bcfe46f68dbc1f49222922", crc32: "897339a7",
            date: null, size: 14_668_180, entries: 2_920,
            wadName: "DOOM2.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom II Enhanced", version: "1.1", nameSubtext: "Unity Port",
            md5: "7895d10c281305c45a7e5f01b3f7b1d8", sha1: "b723882122e90b61a1d92a11dcfcf9cbf95a407e", crc32: "22c291c8",
            date: null, size: 14_685_607, entries: 2_920,
            wadName: "DOOM2.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom II Enhanced", version: "1.3", nameSubtext: "Unity Port",
            md5: "8ab6d0527a29efdc1ef200e5687b5cae", sha1: "9574851209c9dfbede56db0dee0660ecd51e6150", crc32: "f1d1ad55",
            date: null, size: 14_802_843, entries: 2_920,
            wadName: "DOOM2.WAD", note: null, wadType: WadType.IWAD),

        new(name: "Doom II: KEX Edition", version: "1.0/1/2", nameSubtext: "Doom + Doom II",
            md5: "9aa3cbf65b961d0bdac98ec403b832e1", sha1: "c745f04a6abc2e6d2a2d52382f45500dd2a260be", crc32: "09b8a6ae",
            date: "2024-08-06", size: 14_802_506, entries: 2_928,
            wadName: "DOOM2.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Doom II: KEX Edition", version: "1.3", nameSubtext: "Doom + Doom II",
            md5: "64a4c88a871da67492aaa2020a068cd8", sha1: "2921cf667359fd3a80aba3c0cf62ab39297e7e9e", crc32: "151b8a96",
            date: "2024-10-01", size: 14_951_361, entries: 2_928,
            wadName: "DOOM2.WAD", note: null, wadType: WadType.IWAD),

        new(name: "No Rest for the Living", version: "1.4", nameSubtext: "Unity Port",
            md5: "53d180803ae34b16a63c5f1c97faf562", sha1: "7bb0734f85b045dd41482c5e95b0ecbe1700fbcb", crc32: "5990e558",
            date: "2022-08-18", size: 15_569_321, entries: 2_666,
            wadName: "nerve_i.wad", note: "IWAD version compatible with The Ultimate Doom and Doom II.", wadType: WadType.IWAD),
    ];
    public static IReadOnlyList<InternalWad> DOOM64 { get; } = [
        new(name: "Doom 64", version: "1.0", nameSubtext: "2020 port",
            md5: "e16e17f59afe7b3297f53ebe7e9de815", sha1: "ae363db8cd5645e1753d9bacc82cc0724e8e7f21", crc32: "cb10a53d",
            date: "2020-03-20", size: 15_103_284, entries: 1_668,
            wadName: "DOOM64.WAD", note: DOOM64_DEFAULT_NOTE, wadType: WadType.IWAD),
        new(name: "Doom 64", version: "1.1", nameSubtext: "2020 port",
            md5: "0aaba212339c72250f8a53a0a2b6189e", sha1: "d041456bea851c173f65ac6ab3f2ee61bb0b8b53", crc32: "65816192",
            date: "2022-02-24", size: 15_103_212, entries: 1_668,
            wadName: "DOOM64.WAD", note: DOOM64_DEFAULT_NOTE, wadType: WadType.IWAD),
    ];
    public static IReadOnlyList<InternalWad> TNT { get; } = [
        new(name: "Final Doom: TNT Evilution", version: "1.9", nameSubtext: null,
            md5: "4e158d9953c79ccf97bd0663244cc6b6", sha1: "9fbc66aedef7fe3bae0986cdb9323d2b8db4c9d3", crc32: "903dcc27",
            date: "1996-06-10", size: 18_195_736, entries: 3_101,
            wadName: "TNT.WAD", note: null, wadType: WadType.IWAD),

        new(name: "Final Doom: TNT Evilution", version: "1.9", nameSubtext: "id Anthology",
            md5: "1d39e405bf6ee3df69a8d2646c8d5c49", sha1: "4a65c8b960225505187c36040b41a40b152f8f3e", crc32: "d4bb05c0",
            date: "1996-11-14", size: 18_654_796, entries: 3_106,
            wadName: "TNT.WAD", note: """
            A slightly different, and rarer, version is found on a later revision of id Anthology as well as the Macintosh version of Final Doom.
            This alternate version of the IWAD is also the one available for purchase from GOG.com.
            This version does not have the yellow keycard bug, and also removed unnecessary secret specials from sectors 57, 58, 59 and 61 in MAP10.
            This map also has its nodes rebuilt, resulting demos recorded using this IWAD to desync on regular version.
            The rest of changes are the presence of '_DEUTEX_' lump and inclusion of 'P1_START'/'P1_END' lumps inside patch section and 'F1_START'/'F1_END' lumps inside flat section.
            Despite of all these changes, this version is also called v1.9.
            """, wadType: WadType.IWAD),

        new(name: "Final Doom: TNT Evilution", version: "1.9", nameSubtext: "PSN Classic Complete",
            md5: "be626c12b7c9d94b1dfb9c327566b4ff", sha1: "139e26d801a64b404b8d898defca10227a61867b", crc32: "7f572c1f",
            date: null, size: 18_222_568, entries: 3_101,
            wadName: "TNT.WAD", note: """
            Changes include pickup sprites of stimpacks, medkits, and berserk packs have been modified
            to replace the red cross with a red and while pill.
            Already includes the fixed verson of MAP31.
            """, wadType: WadType.IWAD),
        new(name: "Final Doom: TNT Evilution", version: "1.9", nameSubtext: "PSN Europe Classic Complete",
            md5: "66841d77eae5424df947c55747824171", sha1: "5066833da047117241cdda05a708b009eb266c91", crc32: "01846edc",
            date: null, size: 18_226_047, entries: 3_101,
            wadName: "TNT.WAD", note: $$"""
            {{INCORRECTLY_MARKED_IWAD_NOTE}}

            Changes include pickup sprites of stimpacks, medkits, and berserk packs have been modified
            to replace the red cross with a red and while pill.
            Already includes the fixed verson of MAP31.

            Additionally, Wolf3D-related patches and sprites have been changed.
            """, wadType: WadType.IWADIncorrectlyMarked),

        new(name: "Final Doom: TNT Evilution", version: "1.1", nameSubtext: "Unity Port",
            md5: "a6685de59ddf2c07f45deeec95296d98", sha1: "503271390606ebded04a2cfaa1a4e249c0313a9d", crc32: "7a0117a3",
            date: "2019-11-27", size: 18_434_452, entries: 3_107,
            wadName: "TNT.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Final Doom: TNT Evilution", version: "1.3", nameSubtext: "Unity Port",
            md5: "f5528f6fd55cf9629141d79eda169630", sha1: "ca0f0495a6c2813b49620202774c56560d6d7621", crc32: "3243c2b4",
            date: "2020-09-03", size: 18_529_884, entries: 3_107,
            wadName: "TNT.WAD", note: null, wadType: WadType.IWAD),

        new(name: "Final Doom: TNT Evilution KEX Edition", version: "1.0/1/2", nameSubtext: "Doom + Doom II",
            md5: "8974e3117ed4a1839c752d5e11ab1b7b", sha1: "9820e2a3035f0cdd87f69a7d57c59a7a267c9409", crc32: "15f18ddb",
            date: "2024-08-08", size: 18_304_630, entries: 3_109,
            wadName: "TNT.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Final Doom: TNT Evilution KEX Edition", version: "1.3", nameSubtext: "Doom + Doom II",
            md5: "ad7885c17a6b9b79b09d7a7634dd7e2c", sha1: "ab4e71e360a1e64e262d77a53e4bb202450d0f6e", crc32: "6d7a8eec",
            date: "2024-10-03", size: 18_507_237, entries: 3_109,
            wadName: "TNT.WAD", note: null, wadType: WadType.IWAD),
    ];
    public static IReadOnlyList<InternalWad> PLUTONIA { get; } = [
        new(name: "Final Doom: The Plutonia Experiment", version: "1.9", nameSubtext: null,
            md5: "75c8cf89566741fa9d22447604053bd7", sha1: "90361e2a538d2388506657252ae41aceeb1ba360", crc32: "48d1453c",
            date: "1996-06-10", size: 17_420_824, entries: 2_984,
            wadName: "PLUTONIA.WAD", note: null, wadType: WadType.IWAD),

        new(name: "Final Doom: The Plutonia Experiment", version: "1.9", nameSubtext: "id Anthology",
            md5: "3493be7e1e2588bc9c8b31eab2587a04", sha1: "f131cbe1946d7fddb3caec4aa258c83399c21e60", crc32: "15cd1448",
            date: "1996-11-21", size: 18_240_172, entries: 2_988,
            wadName: "PLUTONIA.WAD", note: """
            A slightly different, and rarer, version is found on a later revision of id Anthology as well as the Macintosh version of Final Doom.
            This alternate version of the IWAD is also the one available for purchase from GOG.com.
            This version adds the missing deathmatch starts to MAP12 and MAP23.
            Unfortunately, there is no revision change between both versions so it is also called v1.9.
            """, wadType: WadType.IWAD),

        new(name: "Final Doom: The Plutonia Experiment", version: "1.9", nameSubtext: "PSN Classic Complete",
            md5: "b77ca6a809c4fae086162dad8e7a1335", sha1: "327f8c41ebd4138354e9fca63cebbbd1b9489749", crc32: "3b1ca4fd",
            date: null, size: 17_417_800, entries: 2_984,
            wadName: "PLUTONIA.WAD", note: """
            Changes include pickup sprites of stimpacks, medkits, and berserk packs have been modified
            to replace the red cross with a red and while pill.
            Already includes the fixed verson of MAP31.
            """, wadType: WadType.IWAD),
        new(name: "Final Doom: The Plutonia Experiment", version: "1.9", nameSubtext: "PSN Europe Classic Complete",
            md5: "c2a33c7f597b8a47b334a6812bb4284a", sha1: "85c3517434135a5886111b324955f9288c01046c", crc32: "a7102125",
            date: null, size: 17_421_279, entries: 2_984,
            wadName: "PLUTONIA.WAD", note: """
            Changes include pickup sprites of stimpacks, medkits, and berserk packs have been modified
            to replace the red cross with a red and while pill.
            Already includes the fixed verson of MAP31.

            Additionally, Wolf3D-related patches and sprites have been changed.
            """, wadType: WadType.IWAD),

        new(name: "Final Doom: The Plutonia Experiment", version: "1.1", nameSubtext: "Unity Port",
            md5: "0b381ff7bae93bde6496f9547463619d", sha1: "54e27b5791fbc5677bf7e83c1de3a92ea3ef935b", crc32: "7530bd6e",
            date: "2019-11-27", size: 17_421_754, entries: 2_989,
            wadName: "PLUTONIA.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Final Doom: The Plutonia Experiment", version: "1.3", nameSubtext: "Unity Port",
            md5: "ae76c20366ff685d3bb9fab11b148b84", sha1: "20fd23ee410c466b263a741bbd53bbef573ab47d", crc32: "7cc32623",
            date: "2020-09-03", size: 17_517_186, entries: 2_989,
            wadName: "PLUTONIA.WAD", note: null, wadType: WadType.IWAD),

        new(name: "Final Doom: The Plutonia Experiment: KEX Edition", version: "1.0/1/2", nameSubtext: "Doom + Doom II",
            md5: "24037397056e919961005e08611623f4", sha1: "816c7c6b0098f66c299c9253f62bd908456efb63", crc32: "650b998d",
            date: "2024-08-08", size: 17_531_493, entries: 2_991,
            wadName: "PLUTONIA.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Final Doom: The Plutonia Experiment: KEX Edition", version: "1.3", nameSubtext: "Doom + Doom II",
            md5: "e47cf6d82a0ccedf8c1c16a284bb5937", sha1: "fbcc140825b507ab88d844c6b1635c60c4b8bcd6", crc32: "e82fbfd2",
            date: "2024-10-03", size: 17_704_206, entries: 2_991,
            wadName: "PLUTONIA.WAD", note: null, wadType: WadType.IWAD),
    ];
    public static IReadOnlyList<InternalWad> HERETIC1 { get; } = [
        new(name: "Heretic Shareware Beta", version: null, nameSubtext: "Wide Area Beta",
            md5: "fc7eab659f6ee522bb57acc1a946912f", sha1: "e3d56700ecc3fefbd19cafba5bac3b115120d465", crc32: "864ab894",
            date: "1994-12-20", size: 4_095_992, entries: 1_288,
            wadName: "HERETIC1.WAD", note: """
            A 3-level version of the shareware IWAD released on December 20th, 1994 to private testers,
            before the full 9-level 1.0 shareware IWAD was released.
            """, wadType: WadType.IWAD),

        new(name: "Heretic Shareware", version: "1.0", nameSubtext: null,
            md5: "023b52175d2f260c3bdc5528df5d0a8c", sha1: "5ef073366e5ca523b26a529e843f05c4907c23a4", crc32: "884a3e45",
            date: "1994-12-24", size: 5_120_300, entries: 1_357,
            wadName: "HERETIC1.WAD", note: HERETIC1_DEFAULT_NOTE, wadType: WadType.IWAD),
        new(name: "Heretic Shareware", version: "1.2", nameSubtext: null,
            md5: "ae779722390ec32fa37b0d361f7d82f8", sha1: "b4c50ca9bea07f7c35250a1a11906091971c05ae", crc32: "22d3f0ca",
            date: "1995-06-28", size: 5_120_920, entries: 1_374,
            wadName: "HERETIC1.WAD", note: HERETIC1_DEFAULT_NOTE, wadType: WadType.IWAD),
    ];
    public static IReadOnlyList<InternalWad> HERETIC { get; } = [
        new(name: "Heretic Registered", version: "1.0", nameSubtext: null,
            md5: "3117e399cdb4298eaa3941625f4b2923", sha1: "b5a6cc79cde48d97905b44282e82c4c966a23a87", crc32: "77482d1e",
            date: "1994-12-27", size: 11_096_488, entries: 2_415,
            wadName: "HERETIC1.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Heretic Registered", version: "1.2", nameSubtext: null,
            md5: "1e4cb4ef075ad344dd63971637307e04", sha1: "a54c5d30629976a649119c5ce8babae2ddfb1a60", crc32: "54759180",
            date: "1995-06-28", size: 11_095_516, entries: 2_412,
            wadName: "HERETIC1.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Heretic Registered", version: "1.3", nameSubtext: null,
            md5: "66d686b1ed6d35ff103f15dbd30e0341", sha1: "f489d479371df32f6d280a0cb23b59a35ba2b833", crc32: "5b16049e",
            date: "1996-03-22", size: 14_189_976, entries: 2_633,
            wadName: "HERETIC1.WAD", note: "Includes 'Heretic: Shadow of the Serpent Riders'.", wadType: WadType.IWAD),
    ];
    public static IReadOnlyList<InternalWad> HEXEN { get; } = [
        new(name: "Hexen Shareware Beta", version: null, nameSubtext: null,
            md5: "9178a32a496ff5befebfe6c47dac106c", sha1: "f1015d659329733a237c00f2c6208243fa95e2f7", crc32: "c0e5a448",
            date: "1995-10-02", size: 10_615_976, entries: 2_762,
            wadName: "HEXEN.WAD", note: null, wadType: WadType.IWAD),

        new(name: "Hexen Shareware", version: "1.0", nameSubtext: null,
            md5: "876a5a44c7b68f04b3bb9bc7a5bd69d6", sha1: "fa89a2475855e43c7f7e3198d6e4c4bee23bfab9", crc32: "77cd26c4",
            date: "1995-10-18", size: 10_644_136, entries: 2_856,
            wadName: "HEXEN.WAD", note: """
            Includes four levels from 'Hub 1: Seven Portas':
            'Winnowing Hall', 'Seven Portals', 'Guardian of Ice', and 'Guardian of Fire'.
            """, wadType: WadType.IWAD),
        new(name: "Hexen Shareware", version: "1.0", nameSubtext: "Mac Version",
            md5: "925f9f5000e17dc84b0a6a3bed3a6f31", sha1: "c2338e52c06c3a4925b9a9fb2c720252fe917c08", crc32: "8929dbfa",
            date: null, size: 13_596_228, entries: 3_500,
            wadName: "HEXEN.WAD", note: $$"""
            {{MACOSX_NOTE}}

            Includes four levels from 'Hub 1: Seven Portas':
            'Winnowing Hall', 'Seven Portals', 'Guardian of Ice', and 'Guardian of Fire'.
            """, wadType: WadType.IWAD),

        new(name: "Hexen Registered Beta", version: null, nameSubtext: null,
            md5: "c88a2bb3d783e2ad7b599a8e301e099e", sha1: "ae797f5fdce845be24a7a24dd5bfc3e762a17bbe", crc32: "5185684a",
            date: "1995-09-27", size: 20_428_208, entries: 4_177,
            wadName: "HEXEN.WAD", note: null, wadType: WadType.IWAD),

        new(name: "Hexen Registered", version: "1.0", nameSubtext: null,
            md5: "b2543a03521365261d0a0f74d5dd90f0", sha1: "ac129c4331bf26f0f080c4a56aaa40d64969c98a", crc32: "eece0236",
            date: "1995-10-13", size: 20_128_392, entries: 4_249,
            wadName: "HEXEN.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Hexen Registered", version: "1.1", nameSubtext: null,
            md5: "abb033caf81e26f12a2103e1fa25453f", sha1: "4b53832f0733c1e29e5f1de2428e5475e891af29", crc32: "dca9114c",
            date: "1996-03-14", size: 20_083_672, entries: 4_270,
            wadName: "HEXEN.WAD", note: """
            A slightly different, rarer, version is found encrypted 'HEXEN11.MJ3' on some CDs (such as Quake 1.01).
            There are various undocumented changes, and appears to be identical to the common 1.1 version.
            """, wadType: WadType.IWAD),
        new(name: "Hexen Registered", version: "1.1", nameSubtext: "Mac Version",
            md5: "b68140a796f6fd7f3a5d3226a32b93be", sha1: "4343fbe5aef905ef6d077a1517a50c919e5cc906", crc32: "d6379951",
            date: null, size: 21_078_584, entries: 4_567,
            wadName: "HEXEN.WAD", note: $$"""
            {{MACOSX_NOTE}}

            The lumps 'DENMIDI' and 'DMXGUS' are not present in this version,
            the lumps 'HELP1' and 'HELP2' have minor changes to account for
            differences in controls.

            Additionally, the end of the WAD has an additional 299 extra lumps,
            which contain higher resolution of various images and fonts.
            """, wadType: WadType.IWAD),
    ];
    public static IReadOnlyList<InternalWad> HEXDD { get; } = [
        new(name: "Hexen: Deathkings of the Dark Citadel", version: "1.0", nameSubtext: null,
            md5: "1077432e2690d390c256ac908b5f4efa", sha1: "c3065527d62b05a930fe75fe8181a64fb1982976", crc32: "0c20539a",
            date: "1996-03-22", size: 4_429_700, entries: 325,
            wadName: "HEXDD.WAD", note: $$"""
            {{HEXDD_DEFAULT_NOTE}}

            Compared to v1.1, the missing entry is the 'SDINFO' lump,
            which is used to assign MIDI music tracks to the maps.
            Its absence means that no music is played unless audio CD
            is used (since CD tracks are associated to maps in 'MAPINFO' instead).
            """, wadType: WadType.IWAD),
        new(name: "Hexen: Deathkings of the Dark Citadel", version: "1.1", nameSubtext: null,
            md5: "78d5898e99e220e4de64edaa0e479593", sha1: "081f6a2024643b54ef4a436a85508539b6d20a1e", crc32: "fd5eb11d",
            date: "1996-05-09", size: 4_440_584, entries: 326,
            wadName: "HEXDD.WAD", note: HEXDD_DEFAULT_NOTE, wadType: WadType.IWAD),
    ];
    public static IReadOnlyList<InternalWad> STRIFE0 { get; } = [
        new(name: "Strife Shareware", version: "1.0", nameSubtext: null,
            md5: "de2c8dcad7cca206292294bdab524292", sha1: "92a5fcff91fef9f55ce1da00c57e4aa933bd6f8d", crc32: "3a6ab94d",
            date: "1996-02-22", size: 10_493_652, entries: 2_037,
            wadName: "STRIFE0.WAD", note: $$"""
            {{STRIFE0_DEFAULT_NOTE}}

            Contains 2 extra maps compared to newer versions.

            'MAP35' is hypothesised to be a test map.
            'MAP36' is mostly untextured and unremarkable.
            """, wadType: WadType.IWAD),
        new(name: "Strife Shareware", version: "1.1", nameSubtext: null,
            md5: "bb545b9c4eca0ff92c14d466b3294023", sha1: "bc0a110bf27aee89a0b2fc8111e2391ede891b8d", crc32: "93c144dd",
            date: "1996-03-14", size: 9_934_413, entries: 1_977,
            wadName: "STRIFE0.WAD", note: STRIFE0_DEFAULT_NOTE, wadType: WadType.IWAD),
    ];
    public static IReadOnlyList<InternalWad> STRIFE1 { get; } = [
        new(name: "Strife Registered", version: "1.1", nameSubtext: null,
            md5: "8f2d3a6a289f5d2f2f9c1eec02b47299", sha1: "eb0f3e157b35c34d5a598701f775e789ec85b4ae", crc32: "b7581abd",
            date: "1996-04-18", size: 28_372_168, entries: 3_985,
            wadName: "STRIFE1.WAD", note: """
            The marker in demo lumps identifying the engine version is set to '101', a way to indicate version '1.1'.
            Version 1.0 of the engine was only bundled with the teaser demo with an accompanying 'STRIFE0.WAD' file.
            """, wadType: WadType.IWAD),
        new(name: "Strife Registered", version: "1.2/3/31", nameSubtext: null,
            md5: "2fed2031a5b03892106e0f117f17901f", sha1: "64c13b951a845ca7f8081f68138a6181557458d1", crc32: "4234ace5",
            date: "1996-05-23", size: 28_377_364, entries: 3_985,
            wadName: "STRIFE1.WAD", note: null, wadType: WadType.IWAD),

        new(name: "Strife Voices", version: "1.0", nameSubtext: null,
            md5: "082234d6a3f7086424856478b5aa9e95", sha1: "ec6883100d807b894a98f426d024d22c77b63e7f", crc32: "cd12ebcf",
            date: "1996-04-15", size: 27_319_149, entries: 373,
            wadName: "VOICES.WAD", note: """
            Automatically loaded by the game for voices during conversation and cutscenes.
            No known changes have occurred since release.
            """, wadType: WadType.IWAD),

        new(name: "Strife: Veteran Edition", version: "1.0", nameSubtext: null,
            md5: "06a8f99b9b756ac908917c3868b8e3bc", sha1: "4768b23a2d4778862e15a14ef744da121f711d5b", crc32: "5a1ce95e",
            date: null, size: 102_621_358, entries: 1_753,
            wadName: "SVE.WAD", note: SVE_DEFAULT_NOTE, wadType: WadType.PWAD),
        new(name: "Strife: Veteran Edition", version: "1.1", nameSubtext: null,
            md5: "2c0a712d3e39b010519c879f734d79ae", sha1: "08ba56e6171cec65c10c9feff8705124e72421f3", crc32: "e8a1fcfd",
            date: null, size: 102_621_358, entries: 1_753,
            wadName: "SVE.WAD", note: SVE_DEFAULT_NOTE, wadType: WadType.PWAD),
        new(name: "Strife: Veteran Edition", version: "1.2", nameSubtext: null,
            md5: "47958a4fea8a54116e4b51fc155799c0", sha1: "539ff0d7d9971c2c33e663dca8072c629e177dfb", crc32: "0645464b",
            date: null, size: 102_407_969, entries: 1_751,
            wadName: "SVE.WAD", note: SVE_DEFAULT_NOTE, wadType: WadType.PWAD),
        new(name: "Strife: Veteran Edition", version: "1.3", nameSubtext: null,
            md5: "c4d07b9ff4bfeffca1cdb8478c50fd75", sha1: "69d896243ec17059d7ffeb6448118ea5d2621253", crc32: "4226b19b",
            date: null, size: 106_608_109, entries: 1_788,
            wadName: "SVE.WAD", note: SVE_DEFAULT_NOTE, wadType: WadType.PWAD),
        new(name: "Strife: Veteran Edition", version: "1.0", nameSubtext: "Nintendo Switch release",
            md5: "76fe460d9a981fa47d9cc4eb8ac4bb94", sha1: "b24ff7127da6cc44b61e8c6e551eaffacb674751", crc32: "b23c75bc",
            date: null, size: 102_424_681, entries: 1_754,
            wadName: "SVE.WAD", note: SVE_DEFAULT_NOTE, wadType: WadType.PWAD),
    ];
    public static IReadOnlyList<InternalWad> CHEX { get; } = [
        new(name: "Chex Quest Prototype", version: null, nameSubtext: null,
            md5: "f428a9a226f143a01b5782af611a83dd", sha1: "8451c11659b482a2688d206172e45d74180af7d0", crc32: "84948986",
            date: "1996-10-28", size: 14_588_480, entries: 2_220,
            wadName: "DOOM.WAD", note: """
            Prototype version found on the CD ROM located at:
                    - "ChexR Quest/Macintosh/Bonus/Doom.wad" on Mac HFS+
                    - "CHEX_QUEST\MACINTOS\BONUS\DOOM.WAD" on ISO 9660.
            """, wadType: WadType.IWAD),

        new(name: "Chex Quest", version: "1.0", nameSubtext: null,
            md5: "25485721882b050afa96a56e5758dd52", sha1: "eca9cff1014ce5081804e193588d96c6ddb35432", crc32: "298dd5b5",
            date: "1996-10-31", size: 12_361_532, entries: 2_220,
            wadName: "CHEX.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
    ];
    public static IReadOnlyList<InternalWad> CHEX3 { get; } = [
        new(name: "Chex Quest 3", version: "1.0-1", nameSubtext: null,
            md5: "c77ca2e66bc329c34be559a81cbaf4f3", sha1: "f346fa66cc76bdeea6195b8a430b1104ac0453e0", crc32: "a7a0a92c",
            date: "2008-09-06", size: 18_624_673, entries: 2_403,
            wadName: "CHEX3.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
        new(name: "Chex Quest 3", version: "1.0-2", nameSubtext: null,
            md5: "eded68e862383c5e36c2f07bd7f831d6", sha1: "fce88765fd2e5d1a2920cfce1e7c07b102ed1ce7", crc32: "b7c01da2",
            date: "2008-09-10", size: 18_684_697, entries: 2_403,
            wadName: "CHEX3.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
        new(name: "Chex Quest 3", version: "1.0-3", nameSubtext: null,
            md5: "795823d36ea52b77211a49dbb7d03f93", sha1: "5f59e7284eaf05ce890af46ed6da45fdbe2a3d52", crc32: "795b913b",
            date: "2008-09-12", size: 18_697_249, entries: 2_403,
            wadName: "CHEX3.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
        new(name: "Chex Quest 3", version: "1.0-4", nameSubtext: null,
            md5: "4922378002190472a5fa942ccfa2c3f3", sha1: "5204462358e24adfb1290bf310cc6d55b3fc8845", crc32: "f88b0232",
            date: "2008-09-18", size: 18_627_052, entries: 2_281,
            wadName: "CHEX3.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
        new(name: "Chex Quest 3", version: "1.0-5", nameSubtext: null,
            md5: "1715d1ce852dcd0b2239bab626a3c10c", sha1: "cfe235f50b7a7a0c965951dab970b79238e142d7", crc32: "a93e5ad1",
            date: "2008-09-23", size: 18_461_611, entries: 2_215,
            wadName: "CHEX3.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
        new(name: "Chex Quest 3", version: "1.0-6", nameSubtext: null,
            md5: "cb001c34e424687191f299cc1dff4d68", sha1: "e93b73ce5f68e21d9e4fb60f3838b5abddb0aacb", crc32: "84a6c172",
            date: "2008-11-12", size: 18_458_493, entries: 2_215,
            wadName: "CHEX3.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
        new(name: "Chex Quest 3", version: "1.1", nameSubtext: null,
            md5: "f85944f55fff094f2ffbd3ecef3fa255", sha1: "00b1d5e6d1baad22fbcfbc857f6bd6b36859728b", crc32: "2a63580f",
            date: "2009-04-22", size: 18_513_724, entries: 2_215,
            wadName: "CHEX3.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
        new(name: "Chex Quest 3", version: "1.2", nameSubtext: null,
            md5: "26a8998ecdaa983f8e6c363b4b95bf55", sha1: "992cf6491994cdab59879262df74d229b4db9b90", crc32: "9fb99acb",
            date: "2009-05-02", size: 18_708_791, entries: 2_225,
            wadName: "CHEX3.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
        new(name: "Chex Quest 3", version: "1.3", nameSubtext: null,
            md5: "148367e53ff7f4f814e54b5ac9ff0ab3", sha1: "ef27173d01180f7732e2cd74b5f75ba8153aa278", crc32: "77149702",
            date: "2009-06-12", size: 19_258_591, entries: 2_245,
            wadName: "CHEX3.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
        new(name: "Chex Quest 3", version: "1.4", nameSubtext: null,
            md5: "bce163d06521f9d15f9686786e64df13", sha1: "f0581563604543060c1dce2f148fd74412adb8b8", crc32: "34aa2168",
            date: "2009-06-24", size: 19_191_031, entries: 2_240,
            wadName: "CHEX3.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
    ];
    public static IReadOnlyList<InternalWad> HACX { get; } = [
        new(name: "HacX Demo", version: "1.0", nameSubtext: null,
            md5: "4268af0335f9454c489a794058cd02f2", sha1: "126432aef79cf85c7e1134e88174880b38610008", crc32: "5e11e85e",
            date: null, size: 9_745_911, entries: 2_407,
            wadName: "HACXSW.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
        new(name: "HacX", version: "1.1", nameSubtext: null,
            md5: "b7fd2f43f3382cf012dc6b097a3cb182", sha1: "1125a16243d1e1d5259e0f387d955382432f1424", crc32: "b95a03d2",
            date: "1997-09-16", size: 22_102_300, entries: 2_649,
            wadName: "HACX.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
        new(name: "HacX", version: "1.0", nameSubtext: null,
            md5: "1511a7032ebc834a3884cf390d7f186e", sha1: "dc6df745e342eaea325677a40184e10cbc6629d7", crc32: "b9a93237",
            date: "1997-10-09", size: 21_951_805, entries: 2_662,
            wadName: "HACX.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
        new(name: "HacX", version: "1.0", nameSubtext: null,
            md5: "402ca45bb90520bfef0dec6baac5889e", sha1: "52b5cd8ffef0fd4a33743e774a717774c63046ad", crc32: "4f37c580",
            date: "1997-10-08", size: 21_951_805, entries: 2_662,
            wadName: "HACX.WAD", note: null, wadType: WadType.IWAD),
        new(name: "HacX", version: "1.2", nameSubtext: null,
            md5: "65ed74d522bdf6649c2831b13b9e02b4", sha1: "9fd331b9d53006af1884e89eaeb97b17b8572b71", crc32: "72e3b8ac",
            date: null, size: 19_321_722, entries: 2_784,
            wadName: "HACX.WAD", note: null, wadType: WadType.IWAD),
        new(name: "HacX", version: "2.0r58", nameSubtext: null,
            md5: "ad8db94ef3bdb8522f57d4c5b3b92bd7", sha1: "d5cb3d93345b2eb33e9fd4ec45fa6c87e59732ee", crc32: "80b881c3",
            date: null, size: 39_504_485, entries: 3_413,
            wadName: "HACX.WAD", note: null, wadType: WadType.IWAD),
        new(name: "HacX", version: "2.0r61", nameSubtext: null,
            md5: "793f07ebadb3d7353ee5b6b6429d9afa", sha1: "bae5769b52f39e3461cf0fbcaffc4bf1e805107d", crc32: "19d1bb98",
            date: "2012-07-21", size: 51_079_920, entries: 3_882,
            wadName: "HACX.WAD", note: INCORRECTLY_MARKED_IWAD_NOTE, wadType: WadType.IWADIncorrectlyMarked),
    ];
    public static IReadOnlyList<InternalWad> FREEDOOM { get; } = [
        new(name: "Freedoom Demo", version: "0.6.4", nameSubtext: null,
            md5: "104aa68b59098eed45ca6b328fc4a235", sha1: "f1de1b637a9cc77b095e5d2c375949d2bb7c0aa3", crc32: "5edc71b0",
            date: null, size: 5_827_636, entries: 1_359,
            wadName: "'DOOM1.WAD' replacement", note: "Unsupported after v0.7.", wadType: WadType.IWAD),
        new(name: "Freedoom Ultimate", version: "0.6.4", nameSubtext: null,
            md5: "2e1af223cad142e3487c4327cf0ac8bd", sha1: "fa834f8b99326c4b9372e804d68eb493f1dfa5ec", crc32: "900133c0",
            date: null, size: 11_080_240, entries: 2_450,
            wadName: "'DOOMU.WAD' replacement", note: "Later renamed to 'Freedoom: Phase 1'.", wadType: WadType.IWAD),
        new(name: "Freedoom", version: "0.6.4", nameSubtext: null,
            md5: "5292a1275340798acf9cee07081718e8", sha1: "92faca31f0ebe6011dc33c27524a676e5ba41c17", crc32: "5921a309",
            date: null, size: 19_801_320, entries: 3_023,
            wadName: "'DOOM2.WAD' replacement", note: "Later renamed to 'Freedoom: Phase 2'.", wadType: WadType.IWAD),

        new(name: "Freedoom Demo", version: "0.7-rc1", nameSubtext: null,
            md5: "6faedf0ebb5e72fb22eec94402b14572", sha1: "2f53caf70e3e805815425209f9967150e2c87de8", crc32: "8cb84367",
            date: null, size: 6_368_680, entries: 1_366,
            wadName: "'DOOM1.WAD' replacement", note: "Unsupported after v0.7.", wadType: WadType.IWAD),
        new(name: "Freedoom Ultimate", version: "0.7-rc1", nameSubtext: null,
            md5: "10a5ea771980c135be1e9585f2a05e53", sha1: "0ef1ce68ba6a5f9854ac4073332db17f3887aa86", crc32: "c0f42b2a",
            date: null, size: 16_043_348, entries: 2_766,
            wadName: "'DOOMU.WAD' replacement", note: "Later renamed to 'Freedoom: Phase 1'.", wadType: WadType.IWAD),
        new(name: "Freedoom", version: "0.7-rc1", nameSubtext: null,
            md5: "e2db4f21fbcfd0d69e39ae16b0594168", sha1: "52a856ef7e565e7d4c2956f5562407c2d4dc02ef", crc32: "6ff1afa8",
            date: null, size: 27_704_188, entries: 3_332,
            wadName: "'DOOM2.WAD' replacement", note: "Later renamed to 'Freedoom: Phase 2'.", wadType: WadType.IWAD),

        new(name: "Freedoom Demo", version: "0.7", nameSubtext: null,
            md5: "1bc775628ab912cced19007a565cde54", sha1: "23497c0c948c4f672b58b517dc58657319a8c28b", crc32: "674cb7d3",
            date: null, size: 6_422_144, entries: 1_365,
            wadName: "'DOOM1.WAD' replacement", note: "Unsupported after v0.7.", wadType: WadType.IWAD),
        new(name: "Freedoom Ultimate", version: "0.7", nameSubtext: null,
            md5: "7b7720fc9c1a20fb8ebb3e9532c089af", sha1: "a019548b4614641a9f05ce3c09fdc4c226b3acd0", crc32: "ff6ee2b7",
            date: null, size: 16_514_224, entries: 2_764,
            wadName: "'DOOMU.WAD' replacement", note: "Later renamed to 'Freedoom: Phase 1'.", wadType: WadType.IWAD),
        new(name: "Freedoom", version: "0.7", nameSubtext: null,
            md5: "21ea277fa5612267eb7985493b33150e", sha1: "c1dbf1b22fe594615c1ab2c8d6e4943a82b1020f", crc32: "66bfe10d",
            date: null, size: 27_625_596, entries: 3_326,
            wadName: "'DOOM2.WAD' replacement", note: "Later renamed to 'Freedoom: Phase 2'.", wadType: WadType.IWAD),

        new(name: "Freedoom Ultimate", version: "0.8-beta1", nameSubtext: null,
            md5: "2a24722c068d3a74cd16f770797ff198", sha1: "7f3a58b1c43fd1af8d42e9c2d30ab50bc3be43b0", crc32: "d1151404",
            date: null, size: 17_659_828, entries: 2_718,
            wadName: "'DOOMU.WAD' replacement", note: "Later renamed to 'Freedoom: Phase 1'.", wadType: WadType.IWAD),
        new(name: "Freedoom", version: "0.8-beta1", nameSubtext: null,
            md5: "0597b0937e9615a9667b98077332597d", sha1: "7f2df0c39a399c7e395e30902577d69ff230b1ba", crc32: "5ee7d4ce",
            date: null, size: 28_144_744, entries: 3_286,
            wadName: "'DOOM2.WAD' replacement", note: "Later renamed to 'Freedoom: Phase 2'.", wadType: WadType.IWAD),

        new(name: "Freedoom Ultimate", version: "0.8", nameSubtext: null,
            md5: "30095b256dd3a1566bbc30286f72bc47", sha1: "f758519d22359d3e4ed149d08005804956fdb3d7", crc32: "00f7a9fd",
            date: null, size: 21_806_280, entries: 2_755,
            wadName: "'DOOMU.WAD' replacement", note: "Later renamed to 'Freedoom: Phase 1'.", wadType: WadType.IWAD),
        new(name: "Freedoom", version: "0.8", nameSubtext: null,
            md5: "e3668912fc37c479b2840516c887018b", sha1: "583de46dc05c0bf53f13b834354f048af8caeec1", crc32: "560076c9",
            date: null, size: 28_592_816, entries: 3_325,
            wadName: "'DOOM2.WAD' replacement", note: "Later renamed to 'Freedoom: Phase 2'.", wadType: WadType.IWAD),
    ];
    public static IReadOnlyList<InternalWad> FREEDOOM1 { get; } = [
        new(name: "Freedoom: Phase 1", version: "0.9", nameSubtext: null,
            md5: "aca90cf5ac36e996edc58bd0329b979a", sha1: "b510054f277c3ec786648c1b8bcb9065b802b52a", crc32: "0dfc5a77",
            date: null, size: 20_165_884, entries: 2_454,
            wadName: "'DOOMU.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 1", version: "0.10", nameSubtext: null,
            md5: "9b8d72b59fd93b2b3e116149baa1b142", sha1: "b9ea49cac933202f3fd58f90a7510b0807ba05a5", crc32: "562d477f",
            date: null, size: 21_493_744, entries: 2_448,
            wadName: "'DOOMU.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 1", version: "0.10.1", nameSubtext: null,
            md5: "91de79621a393a08c39a9ab2c034b766", sha1: "ffcc7f7b568a7864d1a11a8d724b97127ea3c688", crc32: "d94b737d",
            date: null, size: 21_493_744, entries: 2_448,
            wadName: "'DOOMU.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 1", version: "0.11", nameSubtext: null,
            md5: "21a4707fc25d29edf4b098bd400c5c42", sha1: "9e38dcc0d1e9fbd20382ba19a6bdf11f7a2b0502", crc32: "48aef988",
            date: null, size: 23_463_640, entries: 2_702,
            wadName: "'DOOMU.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 1", version: "0.11.1", nameSubtext: null,
            md5: "35312e99d2473297aabe0602700bee8a", sha1: "f81e8bb84000daecd682abf95357cf55fb57fbb7", crc32: "872e5d9d",
            date: null, size: 23_553_928, entries: 2_703,
            wadName: "'DOOMU.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 1", version: "0.11.2", nameSubtext: null,
            md5: "6d00c49520be26f08a6bd001814a32ab", sha1: "7931a41e6b1c6b7c701ab16d7355e7685f96b1cc", crc32: "922cc8c4",
            date: null, size: 23_559_728, entries: 2_703,
            wadName: "'DOOMU.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 1", version: "0.11.3", nameSubtext: null,
            md5: "ea471a3d38fcee0fb3a69bcd3221e335", sha1: "36db0eb476486fa56c43f24d9b23e9d8afdbbff5", crc32: "81901f03",
            date: null, size: 23_578_720, entries: 2_703,
            wadName: "'DOOMU.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 1", version: "0.12", nameSubtext: null,
            md5: "0c5f8ff45cc3538d368a0f8d8fc11ce3", sha1: "351a207b5fe520ea618bc8659d176f4c39047d20", crc32: "070682b7",
            date: null, size: 27_284_988, entries: 3_081,
            wadName: "'DOOMU.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 1", version: "0.12.1", nameSubtext: null,
            md5: "b36aa44a23045e503c19af4b4c438a78", sha1: "e9bf428b73a04423ea7a0e9f4408f71df85ab175", crc32: "de6ddb27",
            date: null, size: 27_284_988, entries: 3_081,
            wadName: "'DOOMU.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 1", version: "0.13", nameSubtext: null,
            md5: "b93be13d05148dd01614bc205a03648e", sha1: "97bb88094a51457a8dcad98c58be22a2d0fa9a37", crc32: "5b55e156",
            date: null, size: 28_795_076, entries: 3_163,
            wadName: "'DOOMU.WAD' replacement", note: null, wadType: WadType.IWAD),
    ];
    public static IReadOnlyList<InternalWad> FREEDOOM2 { get; } = [
        new(name: "Freedoom: Phase 2", version: "0.9", nameSubtext: null,
            md5: "8fa57dbc7687f84528eba39dde3a20e0", sha1: "c6df51fa16a0502db77b4eb165155bc56e78dde3", crc32: "ec5a525d",
            date: null, size: 30_426_864, entries: 3_333,
            wadName: "'DOOM2.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 2", version: "0.10", nameSubtext: null,
            md5: "c5a4f2d38d78b251d8557cb2d93e40ee", sha1: "f2f03b807057d07adb7091a0e8cdd6290760a8cf", crc32: "fd3019dc",
            date: null, size: 29_182_560, entries: 3_329,
            wadName: "'DOOM2.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 2", version: "0.10.1", nameSubtext: null,
            md5: "dd9c9e73f5f50d3778c85573cd08d9a4", sha1: "cf3b383245f05bd655a1d2f1c177d0d5837aac26", crc32: "bc18778d",
            date: null, size: 29_182_140, entries: 3_329,
            wadName: "'DOOM2.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 2", version: "0.11", nameSubtext: null,
            md5: "b1018017c61b06e33c11102d8bafaad0", sha1: "25110745824a107f80b078cf368a1045661df3b5", crc32: "23997426",
            date: null, size: 29_135_072, entries: 3_578,
            wadName: "'DOOM2.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 2", version: "0.11.1", nameSubtext: null,
            md5: "ec5b38b30ba2b70e278205776af3fbb5", sha1: "8b4385450d3c622ca7aae3d00c7e15829da264cf", crc32: "bbcfea9b",
            date: null, size: 29_091_820, entries: 3_595,
            wadName: "'DOOM2.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 2", version: "0.11.2", nameSubtext: null,
            md5: "90832a872b5bb0aca4ca0b20419aad5d", sha1: "26fba39016c26cf58756aa2cbf715a34f83857d3", crc32: "a758c437",
            date: null, size: 29_093_468, entries: 3_595,
            wadName: "'DOOM2.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 2", version: "0.11.3", nameSubtext: null,
            md5: "984f99af08f085e38070f51095ab7c31", sha1: "0c03d1f754b98c53f292f66bc9505a29f47c6a9f", crc32: "cef01ecb",
            date: null, size: 29_102_220, entries: 3_595,
            wadName: "'DOOM2.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 2", version: "0.12", nameSubtext: null,
            md5: "83560b2963424fa4a2eb971194428bf8", sha1: "18c6ef3f269a80feed16eb46c7577d29d4209098", crc32: "b66d9e8d",
            date: null, size: 28_552_112, entries: 3_649,
            wadName: "'DOOM2.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 2", version: "0.12.1", nameSubtext: null,
            md5: "ca9a4159a7833544a89144c7f5053412", sha1: "51c997f430dc12abba7888c2d83c237a8e0758a7", crc32: "212e1cf9",
            date: null, size: 28_544_132, entries: 3_649,
            wadName: "'DOOM2.WAD' replacement", note: null, wadType: WadType.IWAD),
        new(name: "Freedoom: Phase 2", version: "0.13", nameSubtext: null,
            md5: "cd666466759b5e5f63af93c5f0ffd0a1", sha1: "975f781e6d801c0a23e3caa33f70493efe68a880", crc32: "68a76eb5",
            date: null, size: 28_787_748, entries: 3_610,
            wadName: "'DOOM2.WAD' replacement", note: null, wadType: WadType.IWAD),
    ];
    public static IReadOnlyList<InternalWad> FREEDM { get; } = [
        new(name: "FreeDM", version: "0.6.4", nameSubtext: null,
            md5: "6f49b8557b39c3a863341cadfc3a420f", sha1: "13ef440dba4aaf8e3cbf77d6993bfdbbc8f7531d", crc32: "ac0fdba9",
            date: null, size: 9_799_976, entries: 2_295,
            wadName: "Deathmatch replacement", note: null, wadType: WadType.IWAD),
        new(name: "FreeDM", version: "0.7-rc1", nameSubtext: null,
            md5: "ee1090f12aa2b4a06e33f640a039f00e", sha1: "11747d6e54f1ec24a8750e6d5b389aebb01c6b7e", crc32: "ff25b967",
            date: null, size: 13_222_064, entries: 2_568,
            wadName: "Deathmatch replacement", note: null, wadType: WadType.IWAD),
        new(name: "FreeDM", version: "0.7", nameSubtext: null,
            md5: "da6ca1bab43edf9fe6a4f14ad060a8d8", sha1: "79068841f057f1b403415964b9c84a5759b5faa6", crc32: "de985aa4",
            date: null, size: 13_431_132, entries: 2_568,
            wadName: "Deathmatch replacement", note: null, wadType: WadType.IWAD),
        new(name: "FreeDM", version: "0.8-beta1", nameSubtext: null,
            md5: "5142cbf6a3f862b293d649225737381b", sha1: "eaf7a215461b32d0d39bd10f83e28dc581ad9f66", crc32: "7f9d1cf9",
            date: null, size: 16_572_172, entries: 3_290,
            wadName: "Deathmatch replacement", note: null, wadType: WadType.IWAD),
        new(name: "FreeDM", version: "0.8", nameSubtext: null,
            md5: "05859098bf191899903ef343afba369d", sha1: "2afd5bde28236c974ade385b103d8b82b7677edc", crc32: "8b923349",
            date: null, size: 16_770_008, entries: 3_329,
            wadName: "Deathmatch replacement", note: null, wadType: WadType.IWAD),

        new(name: "FreeDM", version: "0.9", nameSubtext: null,
            md5: "cbb27c5f3c2c44d34843cf63daa627f6", sha1: "aa71fa61170a021de121fdfceeb80ed08b042b32", crc32: "c31a6ad0",
            date: null, size: 18_880_132, entries: 3_348,
            wadName: "Deathmatch replacement", note: null, wadType: WadType.IWAD),
        new(name: "FreeDM", version: "0.10", nameSubtext: null,
            md5: "f37b8b70e1394289a7ec404f67cdec1a", sha1: "f1058e38ee3d02ffbbb3ecd5bbced9035ecd330e", crc32: "c4e83daa",
            date: null, size: 19_128_320, entries: 3_342,
            wadName: "Deathmatch replacement", note: null, wadType: WadType.IWAD),
        new(name: "FreeDM", version: "0.10.1", nameSubtext: null,
            md5: "bd4f359f1963e388beda014c5548b420", sha1: "cffed419b55732a5dd2ec3366c1ae655471b9414", crc32: "a3f21558",
            date: null, size: 19_127_900, entries: 3_342,
            wadName: "Deathmatch replacement", note: null, wadType: WadType.IWAD),
        new(name: "FreeDM", version: "0.11", nameSubtext: null,
            md5: "d76d3973c075b069ecb4e16dc9eacbb4", sha1: "09e952395598de163ecd60d846d1ad8f06567e8e", crc32: "73278a33",
            date: null, size: 21_188_712, entries: 3_587,
            wadName: "Deathmatch replacement", note: null, wadType: WadType.IWAD),
        new(name: "FreeDM", version: "0.11.1", nameSubtext: null,
            md5: "77ba9c0f75c32e4a729490688bb99241", sha1: "884c0c67a062780c40d7264215c3e7c858ea19ef", crc32: "e5b64f1e",
            date: null, size: 21_103_512, entries: 3_604,
            wadName: "Deathmatch replacement", note: null, wadType: WadType.IWAD),
        new(name: "FreeDM", version: "0.11.2", nameSubtext: null,
            md5: "9352b09ae878dc52c6c18aa38acda6eb", sha1: "05348a5c7801d61cce779940f3e7015e16aadcf4", crc32: "a514a6ab",
            date: null, size: 21_103_364, entries: 3_604,
            wadName: "Deathmatch replacement", note: null, wadType: WadType.IWAD),
        new(name: "FreeDM", version: "0.11.3", nameSubtext: null,
            md5: "87ee2494d921633420ce9bdb418127c4", sha1: "7e979209685ff81f4d709e0926ba585a37d2e35a", crc32: "ae55bb4d",
            date: null, size: 21_112_116, entries: 3_604,
            wadName: "Deathmatch replacement", note: null, wadType: WadType.IWAD),
        new(name: "FreeDM", version: "0.12", nameSubtext: null,
            md5: "3250aad8b1d40fb7b25b7df6573eb29f", sha1: "3ea8429890c3fe5455783575598d52ff4a72758d", crc32: "988d338d",
            date: null, size: 21_824_448, entries: 3_655,
            wadName: "Deathmatch replacement", note: null, wadType: WadType.IWAD),
        new(name: "FreeDM", version: "0.12.1", nameSubtext: null,
            md5: "d40c932a9183ded919afa89f4a729668", sha1: "61065a41a391cd2e4bff69488be36773754354ab", crc32: "bd680d11",
            date: null, size: 21_824_448, entries: 3_655,
            wadName: "Deathmatch replacement", note: null, wadType: WadType.IWAD),
        new(name: "FreeDM", version: "0.13", nameSubtext: null,
            md5: "908dfd77a14cc490c4cea94b62d13449", sha1: "42c36e9d0610bf70c710ca79c7098ef3b1b00059", crc32: "e5636b13",
            date: null, size: 22_218_796, entries: 3_615,
            wadName: "Deathmatch replacement", note: null, wadType: WadType.IWAD),
    ];
    public static IReadOnlyList<InternalWad> OTHERS { get; } = [
        new(name: "Harmony", version: "1.0", nameSubtext: null,
            md5: "fe2cce6713ddcf6c6d6f0e8154b0cb38", sha1: "4c77354915015ccb650ae9708eed3c8219730be9", crc32: "4e10e32f",
            date: "2009-12-10", size: 58_398_594, entries: 2_000,
            wadName: "HARM1.WAD", note: null, wadType: WadType.IWAD),
        new(name: "Harmony", version: "1.1", nameSubtext: null,
            md5: "48ebb49b52f6a3020d174dbcc1b9aeaf", sha1: "d7c5f9095fd5f1ebfe14ecd97ca31503e2a02964", crc32: "bf979c23",
            date: "2012-02-17", size: 58_396_393, entries: 2_000,
            wadName: "HARM1.WAD", note: null, wadType: WadType.IWAD),

        // Customs

        new(name: "Wolfenstein 3D TC", version: "1.0", nameSubtext: null,
            md5: "d43bd2c46188a6967cc3cc56a17fc8c3", sha1: "81fbcbe3f2b6417b159e78e75d2b61afc23212e3", crc32: "c31f296c",
            date: null, size: 1_825_456, entries: 658,
            wadName: "Wolf3D.pk3", note: null, wadType: WadType.IPK3),
        new(name: "Wolfenstein 3D TC: Episode 1: Escape from Castle Wolfenstein", version: "1.0", nameSubtext: null,
            md5: "a3cb659d8df185b52a1dd90f641e963b", sha1: "0d267108b4111ed12e591eda0a59f58e839fd6ca", crc32: "c8a534c2",
            date: null, size: 454_061, entries: 192,
            wadName: "Wol3D_E1.pk3", note: null, wadType: WadType.IPK3),
        new(name: "Wolfenstein 3D TC: Episode 2: Operation Eisenfaust", version: "1.0", nameSubtext: null,
            md5: "c29f580b8c898b9d02c56c27480c19ce", sha1: "58c83c79a5024f90d2ea6a2ac2b0572fa9a49cc0", crc32: "41b29d83",
            date: null, size: 686_716, entries: 140,
            wadName: "Wol3D_E2.pk3", note: null, wadType: WadType.IPK3),
        new(name: "Wolfenstein 3D TC: Episode 3: Die, Fuhrer, Die!", version: "1.0", nameSubtext: null,
            md5: "d50fa142c29af87f0ea4c79da5091eef", sha1: "981731465fee6b0fbbf2584e9366ea184bab88f4", crc32: "ef6079ad",
            date: null, size: 369_912, entries: 70,
            wadName: "Wol3D_E3.pk3", note: null, wadType: WadType.IPK3),
        new(name: "Wolfenstein 3D TC: Episode 4: Dark Secret", version: "1.0", nameSubtext: null,
            md5: "e003d2e5ed34d65c848d6445886a9c1c", sha1: "5f2bde256b4d26daa0c3b09d568532196f8190bd", crc32: "542204e1",
            date: null, size: 229_699, entries: 32,
            wadName: "Wol3D_E4.pk3", note: null, wadType: WadType.IPK3),
        new(name: "Wolfenstein 3D TC: Episode 5: Trail of the Madman", version: "1.0", nameSubtext: null,
            md5: "43d917fe88c614a8ca2dafb36a24e5ba", sha1: "d242ec78df9cd15d89062ea450b984adadb4050d", crc32: "902a4949",
            date: null, size: 194_349, entries: 33,
            wadName: "Wol3D_E5.pk3", note: null, wadType: WadType.IPK3),
        new(name: "Wolfenstein 3D TC: Episode 6: Confrontation", version: "1.0", nameSubtext: null,
            md5: "cb8fb1d86b34d8569fb6bc4a1094c3a6", sha1: "45427127afa29f36070b1f5d5f61a7884290d622", crc32: "15a370f3",
            date: null, size: 295_175, entries: 34,
            wadName: "Wol3D_E6.pk3", note: null, wadType: WadType.IPK3),
        new(name: "Wolfenstein 3D TC: Spear of Destiny", version: "1.0", nameSubtext: null,
            md5: "962bd9d8467259a089882db1ad7baece", sha1: "88800ba37a41ce070fa2e1dce13473eff2b2c49a", crc32: "caa9df63",
            date: null, size: 1_633_388, entries: 305,
            wadName: "SoD.pk3", note: null, wadType: WadType.IPK3),
        new(name: "Wolfenstein 3D TC: GL", version: "1.0", nameSubtext: null,
            md5: "504a68bb760540f82be52a6658d967b8", sha1: "7f2a63176d0c9e74af9bdbea57ca540448eb615e", crc32: "1ae037e4",
            date: null, size: 228_116, entries: 70,
            wadName: "Wolf3DGL.pk3", note: null, wadType: WadType.IPK3),

        new(name: "Wolfenstein 3D TC", version: "2.0", nameSubtext: null,
            md5: "08a951694dc4ba5aea00d9ef0f972c57", sha1: "104c27cbf35a8b2675cd5fec4fae20ddbcc816d1", crc32: "94b21e8b",
            date: null, size: 1_399, entries: 3,
            wadName: "Wolf3D.pk7", note: null, wadType: WadType.IPK7),
        new(name: "Wolfenstein 3D TC: Spear of Destiny", version: "2.0", nameSubtext: null,
            md5: "5d3a049eb1db1f43592a1e4413e15168", sha1: "bc58bb979f17f56143b26e5f28a14b6b85348f1c", crc32: "98777237",
            date: null, size: 6_424, entries: 4,
            wadName: "SoD.pk7", note: null, wadType: WadType.IPK7),
        new(name: "Wolfenstein 3D TC: Spear of Destiny, Return to Danger", version: "2.0.0/1/2", nameSubtext: null,
            md5: "5d50e25fa0bb2d8614a986353a6f2d3f", sha1: "e44a45ba4805a685c4b5fb42f5e85c6ca02b38e7", crc32: "4cc2ae28",
            date: null, size: 715, entries: 2,
            wadName: "SoD_RTD.pk7", note: null, wadType: WadType.IPK7),
        new(name: "Wolfenstein 3D TC: Spear of Destiny, Ultimate Challenge", version: "2.0.0/1/2", nameSubtext: null,
            md5: "0c5aacd734e207f0562fc7f8ff285782", sha1: "bb01927b55ef249ec9cf96f75aa26136f5357101", crc32: "fb398084",
            date: null, size: 720, entries: 2,
            wadName: "SoD_TUC.pk7", note: null, wadType: WadType.IPK7),
        new(name: "Wolfenstein 3D TC: Common", version: "2.0", nameSubtext: null,
            md5: "cddabf32772193577cae377e77205243", sha1: "77b2ff888decfcf4ee7aa53fa1514eed0ee2ce22", crc32: "ab4fc98d",
            date: null, size: 1_617_577, entries: 323,
            wadName: "Wolf3D_Common.pk7", note: null, wadType: WadType.IPK7),
        new(name: "Wolfenstein 3D TC: Resources", version: "2.0", nameSubtext: null,
            md5: "a06f445714364bd68589fbf935fea0e7", sha1: "64c4f9469b61d4b4044a41b24c4fd0e5f5f6c5d2", crc32: "4624ba57",
            date: null, size: 3_474_185, entries: 1_627,
            wadName: "Wolf3D_Resources.pk3", note: null, wadType: WadType.IPK3),

        new(name: "Wolfenstein 3D TC", version: "2.0.1/2", nameSubtext: null,
            md5: "A155F1820A3839796AAE27ECF1DE607B", sha1: "DC3FFEB8E2FDD6E1B16ABEA971724ACB2186A2CD", crc32: "755EB4EE",
            date: null, size: 1_398, entries: 3,
            wadName: "Wolf3D.pk7", note: null, wadType: WadType.IPK7),
        new(name: "Wolfenstein 3D TC: Spear of Destiny", version: "2.0.1/2", nameSubtext: null,
            md5: "BC945CEF51E896DB484F5EE63F03EC76", sha1: "E5906B6065EC664A3B89037274F3F5AC80B46153", crc32: "3D323401",
            date: null, size: 6_412, entries: 4,
            wadName: "SoD_TUC.pk7", note: null, wadType: WadType.IPK7),
        new(name: "Wolfenstein 3D TC: Common", version: "2.0.1", nameSubtext: null,
            md5: "F04BE0C59A29CAB146692F492D625ABA", sha1: "349E3AE7D7DBC056A532F130C207038CEB0364C6", crc32: "6947FA2C",
            date: null, size: 1_187_725, entries: 318,
            wadName: "Wolf3D_Common.pk7", note: null, wadType: WadType.IPK7),
        new(name: "Wolfenstein 3D TC: Resources", version: "2.0.1/2", nameSubtext: null,
            md5: "3EF85A6CB10F859127D6CECE7DD3CEA4", sha1: "87F502A3516E935952CB7916FB0F8719966563C1", crc32: "2C98FC9C",
            date: null, size: 3_557_957, entries: 1695,
            wadName: "Wolf3D_Resources.pk3", note: null, wadType: WadType.IPK3),
        new(name: "Wolfenstein 3D TC: High Res", version: "2.0.1/2", nameSubtext: null,
            md5: "5370AA945C56832CA160BA582F801B0E", sha1: "D5EC8CE48F1F3FA558E1A966E0D2A80800C8D376", crc32: "41DD1566",
            date: null, size: 964_256, entries: 280,
            wadName: "Wolf3D_HighRes.pk3", note: null, wadType: WadType.IPK7),

        new(name: "Wolfenstein 3D TC: Common", version: "2.0.2", nameSubtext: null,
            md5: "98732E872752FCCBF873CECBE1B06A6D", sha1: "C01FD6544A4A921D4848695F4A34061AC3205251", crc32: "52D749AD",
            date: null, size: 1_188_965, entries: 318,
            wadName: "Wolf3D_Common.pk7", note: null, wadType: WadType.IPK7),

        new(name: "Wolfenstein 3D TC", version: "3.0-beta1", nameSubtext: null,
            md5: "d272f00cbf5d14a0c82aef66239a4171", sha1: "c9bca066683ead074ac155d831012875ca867152", crc32: "0e6d487d",
            date: null, size: 7_111_941, entries: 2_148,
            wadName: "Wolf3D.ipk3", note: null, wadType: WadType.IPK3),
        new(name: "Wolfenstein 3D TC", version: "3.0-beta2", nameSubtext: null,
            md5: "0582758f4c16df08092a2653ed6a5407", sha1: "5aa3edd4913c044ec79ff92c1b9c94e635936461", crc32: "88c89102",
            date: null, size: 7_414_530, entries: 2_281,
            wadName: "Wolf3D.ipk3", note: null, wadType: WadType.IPK3),
        new(name: "Wolfenstein 3D TC", version: "3.0-beta2a", nameSubtext: null,
            md5: "b311a04651d1a5cc623681fe4a2ce827", sha1: "ae55931e935c87f66bcb8798dc1088a26dde89a1", crc32: "b28e9416",
            date: null, size: 7_414_619, entries: 2_281,
            wadName: "Wolf3D.ipk3", note: null, wadType: WadType.IPK3),
        new(name: "Wolfenstein 3D TC", version: "3.0-beta3a", nameSubtext: null,
            md5: "e061528647a21bc47e156b65e70a979e", sha1: "af693b7d55f5a9dbf20fa76c264034f2c1aa9545", crc32: "c01923bb",
            date: null, size: 7_749_913, entries: 2_595,
            wadName: "Wolf3D.ipk3", note: null, wadType: WadType.IPK3),
        new(name: "Wolfenstein 3D TC", version: "3.0-beta4", nameSubtext: null,
            md5: "0eaea026aa352a52b9ad58da76402919", sha1: "44de791c9f8f1445b7945187f60b5c360369950b", crc32: "6d322938",
            date: null, size: 7_434_805, entries: 2_756,
            wadName: "Wolf3D.ipk3", note: null, wadType: WadType.IPK3),
        new(name: "Wolfenstein 3D TC", version: "3.0-beta5", nameSubtext: null,
            md5: "3265542429b1835f0b263ad84a905b13", sha1: "e9362ba09a25d3a0a64b1f5c515e37cee4220b23", crc32: "5fcb07eb",
            date: null, size: 7_529_572, entries: 2_794,
            wadName: "Wolf3D.ipk3", note: null, wadType: WadType.IPK3),
        new(name: "Wolfenstein 3D TC", version: "3.0", nameSubtext: null,
            md5: "795a4b1b964385af67d98e33ac99ab93", sha1: "29b64976764cd20b8afdd262041074a6a90b6cf0", crc32: "bd79d8dc",
            date: null, size: 8_386_159, entries: 3_515,
            wadName: "Wolf3D.ipk3", note: null, wadType: WadType.IPK3),
        new(name: "Wolfenstein 3D TC", version: "3.1", nameSubtext: null,
            md5: "02654a7c6a016b94af84124baab6da94", sha1: "d1d4906167c0c453dd9d635ac25ca56b63ba0cf8", crc32: "3a0027bb",
            date: null, size: 8_387_658, entries: 3_515,
            wadName: "Wolf3D.ipk3", note: null, wadType: WadType.IPK3),
        new(name: "Wolfenstein 3D TC", version: "3.1", nameSubtext: "Complete",
            md5: "37c2dd526c0bcaf8788387994a87e965", sha1: "27343361ac528005cd84089e77d3d64fafa3aadc", crc32: "89b4b928",
            date: null, size: 8_645_258, entries: 3_520,
            wadName: "Wolf3D.ipk3", note: """
            Wolfenstein Total Conversion
            Not an official Wolf3D build.
            Added required 'GAMEMAPS' files into the 'WOLF3DTC.PK3' with SLADE v3.2.2,
            ordered 'WL3', 'WL6', 'SOD', 'SD2', 'SD3'.
            """, wadType: WadType.IPK3),

        new(name: "Adventures of Square", version: "2.1", nameSubtext: "Windows bundle release",
            md5: "582cbe67ebd506dd55786552482099f8", sha1: "a4bd92bde933fccc1761d749a2a0ee855f436c83", crc32: "a2858888",
            date: null, size: 45_202_919, entries: 6_605,
            wadName: "square1.wad", note: "Adventures of Square", wadType: WadType.IPK3),
        new(name: "Adventures of Square", version: "2.1", nameSubtext: "Mac OS X bundle release",
            md5: "c8b9a7f7a36f3d12294b426907684c24", sha1: "5bb02f57d600ce37812b4db9738266e5e1f96d98", crc32: "2ee2ad02",
            date: null, size: 45_201_465, entries: 6_605,
            wadName: "square1.wad", note: "Adventures of Square", wadType: WadType.IPK3),
        new(name: "Adventures of Square", version: "2.1", nameSubtext: "ZDoom release",
            md5: "f4578097c658ad3c813cd5901ec125e2", sha1: "cf1f2f508766666a88af165968001dcb6680141e", crc32: "be923d55",
            date: null, size: 45_202_919, entries: 6_605,
            wadName: "square1.wad", note: "Adventures of Square", wadType: WadType.IPK3),
    ];
    public static IReadOnlyList<InternalWad> PWADS { get; } = [
        new(name: "Geryon: 6th Canto of Inferno", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "a1efff02df6d873762ebac6b12358bbc", sha1: "2889354437b3814197c5de030e392ed0061f968b", crc32: "521d230f",
            date: null, size: 221_009, entries: 12,
            wadName: "GERYON.WAD", note: "Created by John Anderson (Dr. Sleep)", wadType: WadType.PWAD),
        new(name: "Minos' Judgement: 4th Canto of Inferno", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "aea597159dee96bcc58f3f9e3e586182", sha1: "5fc46a9bd60b9fc2ed00cad3e7f57fe0cc43422b", crc32: "c385f0bc",
            date: null, size: 351_712, entries: 12,
            wadName: "MINOS.WAD", note: "Created by John Anderson (Dr. Sleep)", wadType: WadType.PWAD),
        new(name: "Nessus: 5th Canto of Inferno", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "46f58580e7792f486c747cf1117c4ca1", sha1: "395fcfdf94b9d2514a1d0e7e53f0738a077ecee5", crc32: "f3e1c6cd",
            date: null, size: 158_514, entries: 12,
            wadName: "NESSUS.WAD", note: "Created by John Anderson (Dr. Sleep)", wadType: WadType.PWAD),
        new(name: "Vesperas: 7th Canto of Inferno", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "a49dccebb5f32307246b7f32da121cf7", sha1: "8341faf0e8469e43178b0be3fe7ebb40d5a33b03", crc32: "9b452c7e",
            date: null, size: 235_353, entries: 12,
            wadName: "VESPERAS.WAD", note: "Created by John Anderson (Dr. Sleep)", wadType: WadType.PWAD),
        new(name: "Virgil's Lead: 3rd Canto of Inferno", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "3c0874f2df3c06a002ee2a18aba0f0e8", sha1: "e3de77406aff3b6e5f06c3d450d70b8e6c6a34f9", crc32: "4d0b58e8",
            date: null, size: 171_232, entries: 12,
            wadName: "VIRGIL.WAD", note: "Created by John Anderson (Dr. Sleep)", wadType: WadType.PWAD),
        new(name: "Titan Manor", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "787fa80fe9665c322f853b74838e77cc", sha1: "6698a16408d5d337f13a444d5dc71051ea5fcad4", crc32: "e6243374",
            date: null, size: 245_444, entries: 16,
            wadName: "MANOR.WAD.WAD", note: "Created by Jim Flynn", wadType: WadType.PWAD),
        new(name: "Trapped on Titan", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "8474f6d663f04630de05ecac36b574d1", sha1: "50a68fcb4761f6155febceca947e663d6b442845", crc32: "2fe2a2e4",
            date: null, size: 286_220, entries: 16,
            wadName: "TTRAP.WAD", note: "Created by Jim Flynn", wadType: WadType.PWAD),
        new(name: "The Catwalk", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "e7c273033376824edf95e1328261e7de", sha1: "5dcd681b968098f24b774e3f3cab19a38e40a091", crc32: "120cd525",
            date: null, size: 151_817, entries: 11,
            wadName: "CATWALK.WAD", note: "Created by Christen Klie", wadType: WadType.PWAD),
        new(name: "The Combine", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "77c179948df47a7a613bd1181c959892", sha1: "f94d5e96923b7d148639c471a2700e64f3a6bad1", crc32: "f36f7a65",
            date: null, size: 141_548, entries: 12,
            wadName: "COMBINE.WAD", note: "Created by Christen Klie", wadType: WadType.PWAD),
        new(name: "The Fistula", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "cbf714b499ebdef2682990eaf93fdb5f", sha1: "a7a45f5becf6cb470be3e19ffa46153f156641fe", crc32: "7bdb6497",
            date: null, size: 104_654, entries: 11,
            wadName: "FISTULA.WAD", note: "Created by Christen Klie", wadType: WadType.PWAD),
        new(name: "The Garrison", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "f000701a3ed1f49249ee08550c03dfa5", sha1: "5a07c230ca351c9865f838bda0ece4e46ba0b412", crc32: "24a63b87",
            date: null, size: 116_403, entries: 11,
            wadName: "GARRISON.WAD", note: "Created by Christen Klie", wadType: WadType.PWAD),
        new(name: "Subspace", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "b572d518d564c7d7b6b259a726538cbb", sha1: "485758921ddb3686763c53b5638699d1b65a5ac0", crc32: "311059cb",
            date: null, size: 105780, entries: 11,
            wadName: "SUBSPACE.WAD", note: "Created by Christen Klie", wadType: WadType.PWAD),
        new(name: "Subterra", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "bb417f07804373415a6ed8e533242c3c", sha1: "d91fbab728132dbecb9f7c9cea9212a3dfd795ee", crc32: "ad3094fd",
            date: null, size: 127_480, entries: 11,
            wadName: "SUBTERRA.WAD", note: "Created by Christen Klie", wadType: WadType.PWAD),
        new(name: "Black Tower", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "a421ca18cea00a2696162f8d2a2beeca", sha1: "53cd0db349198a721162282f95d15c2ec407a555", crc32: "e8ec0c8e",
            date: null, size: 199_534, entries: 11,
            wadName: "BLACKTWR.WAD", note: "Created by Sverre André Kvernmo (Cranium)", wadType: WadType.PWAD),
        new(name: "Bloodsea Keep", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "18eb4ffb3094ddb690e62211dc6169a1", sha1: "51a02e02ed92ee22ae8aff0dfeee5b41e55d23a4", crc32: "4e3c30d6",
            date: null, size: 258_942, entries: 11,
            wadName: "BLOODSEA.WAD", note: "Created by Sverre André Kvernmo (Cranium)", wadType: WadType.PWAD),
        new(name: "Mephisto's Maosoleum", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "b4eaf844b135cc2a0058c6e0149b4408", sha1: "6335ff2e8c767f49fd2545d1a0493a7721757385", crc32: "3c9f90af",
            date: null, size: 121_406, entries: 11,
            wadName: "MEPHISTO.WAD", note: "Created by Sverre André Kvernmo (Cranium)", wadType: WadType.PWAD),
        new(name: "The Express Elevator to Hell / Bad Dream", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "65b4abcb74e7a386d5c024cf250d6336", sha1: "5bbf0b9d52b831fe47dc09ac5a51d923bed03e3e", crc32: "9b918d83",
            date: null, size: 190_531, entries: 23,
            wadName: "TEETH.WAD", note: "Created by Sverre André Kvernmo (Cranium)", wadType: WadType.PWAD),
        new(name: "Paradox", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "d560abb6d5719d46ebb47b27d7813a4b", sha1: "26f96ac183be4383ad7f3daed2c16548fa69dd42", crc32: "716050f7",
            date: null, size: 223555, entries: 11,
            wadName: "PARADOX.WAD", note: "Created by Tom Mustaine", wadType: WadType.PWAD),
        new(name: "Attack", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "cb03fd0cd84b10579c2b2b313199d4c1", sha1: "e0b93dcb87875544cc57306088983cb21ca45165", crc32: "b350c720",
            date: null, size: 185_135, entries: 11,
            wadName: "ATTACK.WAD", note: "Created by Tim Willits", wadType: WadType.PWAD),
        new(name: "Canyon", version: "1.0", nameSubtext: "Master Levels for Doom II",
            md5: "33493942592d764e7787fb0ad7d03044", sha1: "33a10a0515a52ce2a017648d121d719adccc3900", crc32: "3d84f23d",
            date: null, size: 157_529, entries: 11,
            wadName: "CANYON.WAD", note: "Created by Tim Willits", wadType: WadType.PWAD),

        new(name: "Master Levels for Doom II", version: "1.0", nameSubtext: "PSN Classic Complete",
            md5: "84cb8640f599c4a17c8eb526f90d2b7a", sha1: "f095f557c9969bafd249710da7675c1bc2e94698", crc32: "6baec89f",
            date: null, size: 3_479_715, entries: 252,
            wadName: "MASTERLEVELS.WAD", note: null, wadType: WadType.PWAD),
        new(name: "Master Levels for Doom II: KEX Edition", version: "1.0/1/2", nameSubtext: "Doom + Doom II",
            md5: "2d0e4fde4c83d90476f3f439bb5f3eea", sha1: "47d5b10aee861791c9272ba4944e8eb1ea1e0819", crc32: "07312a30",
            date: "2024-08-08", size: 3_894_977, entries: 275,
            wadName: "MASTERLEVELS.WAD", note: null, wadType: WadType.PWAD),
        new(name: "Master Levels for Doom II: KEX Edition", version: "1.3", nameSubtext: "Doom + Doom II",
            md5: "ab3ce78e085e50a61f6dff46aabbfaeb", sha1: "32e7937fda52c4cecc8ddc75ddd41b5e0dfe6892", crc32: "d7053e8a",
            date: "2024-10-03", size: 3_920_169, entries: 277,
            wadName: "MASTERLEVELS.WAD", note: null, wadType: WadType.PWAD),

        new(name: "The Lost Episodes of Doom", version: "1.0", nameSubtext: null,
            md5: "LLDMD5", sha1: "e820373ae6bd18282bec8f3aa5b40c71a6105637", crc32: "LLDCRC32",
            date: "", size: 0, entries: 0,
            wadName: "JPTR_V40.WAD", note: null, wadType: WadType.PWAD),
        new(name: "The Lost Episodes of Doom", version: "1.0", nameSubtext: "Self-extracted version",
            md5: "8111f6ea0263a7fcfee5ba3dd06c5554", sha1: "9dd7699086f5519c21f4ff24459069e887bb015e", crc32: "368a45b2",
            date: "09 December 1994 01:31:28", size: 2_313_675, entries: 271,
            wadName: "JPTR_V40.WAD", note: null, wadType: WadType.PWAD),

        new(name: "No Rest for the Living", version: "1,0", nameSubtext: "BFG Edition, XBLA, PSN, Xbox 360+",
            md5: "967d5ae23daf45196212ae1b605da3b0", sha1: "3451288383fb16e196f273d9f85d58c1fda97bf4", crc32: "ad7f9292",
            date: null, size: 3_819_855, entries: 108,
            wadName: "Nerve.demo.wad", note: null, wadType: WadType.PWAD),
        new(name: "No Rest for the Living: KEX Edition", version: "1.0+", nameSubtext: "Doom + Doom II",
            md5: "23422eb42833ac7b0dd59c0c7ae18a6f", sha1: "4522eaec7a13aac24b456e29520e99ac879e6989", crc32: "07d9faab",
            date: null, size: 4_039_441, entries: 117,
            wadName: "NERVE.WAD", note: null, wadType: WadType.PWAD),
        new(name: "No Rest for the Living", version: "1.0", nameSubtext: "Unity Port",
            md5: "4214c47651b63ee2257b1c2490a518c9", sha1: "321e951f6cc6e9d51bc16eac53e0d001f0f3f338", crc32: "ab68447f",
            date: "2019-11-27", size: 3_821_966, entries: 109,
            wadName: "NERVE.WAD", note: null, wadType: WadType.PWAD),
        new(name: "No Rest for the Living", version: "1.1-2", nameSubtext: "Unity Port",
            md5: "3544e1903091c50ba50049db74cd7d25", sha1: "2c5c4aa07eea8820a65dbf282d8b6d8358bb74fa", crc32: "a409d7bb",
            date: "2020-07-15", size: 3_821_885, entries: 109,
            wadName: "NERVE.WAD", note: null, wadType: WadType.PWAD),
        new(name: "No Rest for the Living", version: "1.3", nameSubtext: "Unity Port",
            md5: "9143b392392a7ac870c2ca36ac65af45", sha1: "6ab960ae23430ca2ced7db3edd8075b7e0158f26", crc32: "ca2b615d",
            date: "2020-09-03", size: 4_003_409, entries: 111,
            wadName: "NERVE.WAD", note: null, wadType: WadType.PWAD),

        new(name: "Chex Quest 2", version: "1.0", nameSubtext: null,
            md5: "fdc4ffa57e1983e30912c006284a3e01", sha1: "d5b970834b8ff364d377ef04eb7d12fa6035e10a", crc32: "30aab11e",
            date: null, size: 7_585_664, entries: 1_782,
            wadName: "CHEX2.WAD", note: null, wadType: WadType.PWAD),
        new(name: "Chex Quest 3", version: "1.0", nameSubtext: "Stolen",
            md5: "59c985995db55cd2623c1893550d82b3", sha1: "100fd7b4dec9dd299d7756cc3a6a419a94264926", crc32: "a6a5bf06",
            date: "2001-01-16", size: 9_348_448, entries: 1_722,
            wadName: "chex3.wad", note: "Actually an earlier unofficial PWAD not related to Charles Jacobi's Chex Quest 3.", wadType: WadType.PWAD),

        new(name: "SIGIL", version: "1.0", nameSubtext: null,
            md5: "f53ffc4fb89e966839bb8d20c632819a", sha1: "5e5241f49f0dbb7a6f6df3d46b494c1e163a2c53", crc32: "e74b2233",
            date: null, size: 4_473_686, entries: 142,
            wadName: "SIGIL.WAD", note: null, wadType: WadType.PWAD),
        new(name: "SIGIL", version: "1.1", nameSubtext: null,
            md5: "1fe9daa0e837c7452eb2f91aac2cc983", sha1: "1d95e599a1587d1130bf24356a73b817ac2c1b57", crc32: "5a80c2cd",
            date: null, size: 4_525_740, entries: 145,
            wadName: "SIGIL.WAD", note: null, wadType: WadType.PWAD),
        new(name: "SIGIL", version: "1.2", nameSubtext: null,
            md5: "427ca995600970abcd2efcc684a64c88", sha1: "ca88fa5749e494af5a757866c14008c305428ab6", crc32: "8292a285",
            date: null, size: 4_639_917, entries: 146,
            wadName: "SIGIL.WAD", note: null, wadType: WadType.PWAD),
        new(name: "SIGIL", version: "1.21", nameSubtext: null,
            md5: "743d6323cb2b9be24c258ff0fc350883", sha1: "e2efdf379e1383c4e15c03de89063361897cd459", crc32: "f9216574",
            date: null, size: 4_640_210, entries: 146,
            wadName: "SIGIL.WAD", note: null, wadType: WadType.PWAD),
        new(name: "SIGIL", version: "1.0", nameSubtext: "Compat",
            md5: "a775262ca0e423468196803b71a57a43", sha1: "c7b52480221678f96daf8b8c34d08e05a4591704", crc32: "abaef919",
            date: null, size: 4_468_256, entries: 150,
            wadName: "SIGIL.WAD", note: null, wadType: WadType.PWAD),
        new(name: "SIGIL", version: "1.1", nameSubtext: "Compat",
            md5: "c04912beab6aa82c114a19c976ec8c0d", sha1: "60641c2519ba95565c714de09b1cf1358c4905fd", crc32: "4fa56fdb",
            date: null, size: 4_483_345, entries: 154,
            wadName: "SIGIL.WAD", note: null, wadType: WadType.PWAD),
        new(name: "SIGIL", version: "1.2", nameSubtext: "Compat",
            md5: "9285e9cc2dbd87d238baab37d700c644", sha1: "90e02ee6fd023f6d62a64d71c05c4f00a841e7bd", crc32: "c2e2f76b",
            date: null, size: 4_633_831, entries: 158,
            wadName: "SIGIL.WAD", note: null, wadType: WadType.PWAD),
        new(name: "SIGIL", version: "1.21", nameSubtext: "Compat",
            md5: "573f3f178c76709f512089ed15484391", sha1: "b5e68950820b3a0385375dbace81376e73568207", crc32: "b7679050",
            date: null, size: 4_634_157, entries: 158,
            wadName: "SIGIL.WAD", note: null, wadType: WadType.PWAD),
        new(name: "SIGIL: KEX Edition", version: "1.0/1", nameSubtext: "Doom + Doom II",
            md5: "fe76f36340b7adaaf889f143ed944885", sha1: "53273e526bc77022ddf260825bd0d250e1c72f8f", crc32: "175af9d6",
            date: null, size: 4_658_791, entries: 148,
            wadName: "SIGIL.WAD", note: null, wadType: WadType.PWAD),
        new(name: "SIGIL: KEX Edition", version: "1.2/3", nameSubtext: "Doom + Doom II",
            md5: "08ee05388c137db5f5d7996e89425b95", sha1: "89186ec316fe2ad7e065e685604230b60ee3bb29", crc32: "c1280f17",
            date: "2024-08-08", size: 4_660_156, entries: 149,
            wadName: "SIGIL.WAD", note: null, wadType: WadType.PWAD),
        new(name: "SIGIL Buckethead Soundtrack", version: "1.0", nameSubtext: null,
            md5: "b424dcf46ae55a496c34ac37cce32646", sha1: "efdc3b2255a50b1b058ac609044cbd5e36029bc4", crc32: "cf88488f",
            date: null, size: 162_146_269, entries: 11,
            wadName: "SIGIL.WAD", note: null, wadType: WadType.PWAD),
        new(name: "SIGIL Buckethead Soundtrack", version: "1.0", nameSubtext: "Compat",
            md5: "343faa815928c58faa08939a4502d5d2", sha1: "42a5034d76a4effb4a26194dbb90d6ee859e8374", crc32: "8524df67",
            date: null, size: 162_146_269, entries: 11,
            wadName: "SIGIL.WAD", note: null, wadType: WadType.PWAD),

        new(name: "SIGIL II", version: "1.0", nameSubtext: null,
            md5: "d0442f5a75f2faef3405c09a0c3acc58", sha1: "ad2c6e8367afbeef74e9e09b6b1e4da88c0576b4", crc32: "d210db36",
            date: null, size: 5_632_323, entries: 171,
            wadName: "SIGIL2.WAD", note: null, wadType: WadType.PWAD),
        new(name: "SIGIL II THORR Soundtrack", version: "1.0", nameSubtext: null,
            md5: "732fb8f9c470e857189c206a9279af74", sha1: "f3d9362ab8ff2796c5772071e06a8e6817fcf4be", crc32: "a5b88587",
            date: null, size: 146_563_690, entries: 171,
            wadName: "SIGIL2.WAD", note: null, wadType: WadType.PWAD),
        new(name: "SIGIL II THORR Soundtrack", version: "1.0", nameSubtext: "Compat",
            md5: "579725cb49e5093a84f0d15438c965de", sha1: "7f7616d15eb165ed849d0763672befb0e93c97b1", crc32: "ac56dd7a",
            date: null, size: 84_602_446, entries: 11,
            wadName: "SIGIL2.WAD", note: null, wadType: WadType.PWAD),

        new(name: "DOOM 2019-2024 Reissue Resources", version: "1.0", nameSubtext: null,
            md5: "38e1bb6cdd1e4227eef13d22f6e35209", sha1: "0f2a7139b640e19a08f384562f907cb0734b0512", crc32: "82bccddc",
            date: null, size: 64_587, entries: 22,
            wadName: "extras.wad", note: $$"""
            {{EXTRAS_DEFAULT_NOTE}}

            This version added the crosshair.
            """, wadType: WadType.PWAD),
        new(name: "DOOM 2019-2024 Reissue Resources", version: "1.1", nameSubtext: null,
            md5: "0c7caf25ad1584721ff5ecc38dec97a0", sha1: "55a6a95c99d7fe7b9501d34b0afecd3252a629e1", crc32: "8068229f",
            date: null, size: 64_779, entries: 24,
            wadName: "extras.wad", note: EXTRAS_DEFAULT_NOTE, wadType: WadType.PWAD),
        new(name: "DOOM 2019-2024 Reissue Resources: KEX Edition", version: "1.0/1", nameSubtext: "Doom + Doom II",
            md5: "02b06abfe84ec3c37fb8e739984fb68a", sha1: "bec65aa5eef9a4397c1f2a2bd7fd34e9777a4510", crc32: "1c841611",
            date: null, size: 625_064_339, entries: 165,
            wadName: "extras.wad", note: $$"""
            {{EXTRAS_DEFAULT_NOTE}}

            {{EXTRAS_KEX_DEFAULT_NOTE}}
            """, wadType: WadType.PWAD),
        new(name: "DOOM 2019-2024 Reissue Resources: KEX Edition", version: "1.2", nameSubtext: "Doom + Doom II",
            md5: "b3247939c60f6a819c625036b52a5f53", sha1: "f83b2f84de582fe4ec4a4b2125507f9e48e4a151", crc32: "ec81d166",
            date: "2024-08-08", size: 625_132_400, entries: 169,
            wadName: "extras.wad", note: $$"""
            {{EXTRAS_DEFAULT_NOTE}}
            
            {{EXTRAS_KEX_DEFAULT_NOTE}}
            """, wadType: WadType.PWAD),
        new(name: "DOOM 2019-2024 Reissue Resources: KEX Edition", version: "1.3", nameSubtext: "Doom + Doom II",
            md5: "2e76d93d52ef64fb9db3cee2437c686b", sha1: "9c4fe2ae23b4c94c30cfb74f619ed1011fb8cda1", crc32: "0fa8a647",
            date: "2024-10-03", size: 625_140_164, entries: 170,
            wadName: "extras.wad", note: $$"""
            {{EXTRAS_DEFAULT_NOTE}}
            
            {{EXTRAS_KEX_DEFAULT_NOTE}}
            """, wadType: WadType.PWAD),

        // Move to 'OTHERS'?
        new(name: "DOOM KEX: ID24 standard resources", version: "1.2/3", nameSubtext: "Doom + Doom II",
            md5: "4f0651accebc007b853943ac12aa95b8", sha1: "ff85c552e2335f81811f11141f80a4c7c904cea1", crc32: "6875903f",
            date: null, size: 2_108_038, entries: 530,
            wadName: "id24res.wad", note: """
            Contains the resources needed for the new content added by the ID24 standard,
            including sprites and sounds for new props, monsters, and weapons.

            Contrarily to 'id1.wad', 'id1-res.wad', or 'id-weap.wad',
            it does not contain 'DEHACKED' actor definitions, as these actors are supported
            to be integrated directly in the egine.
            In addition, it does not include any of the Legacy of Rust textures.
            
            The ID24 standard states that compatibly ports should load 'id24.res.wad' before the IWAD,
            allowing its content to be overridden if needed.
            """, wadType: WadType.IWAD),

        new(name: "DOOM KEX: id Deathmatch Pack #1", version: "1.0/1", nameSubtext: "Doom + Doom II",
            md5: "a1ce606446c1c8901fcaca373fded7b7", sha1: "057f0c47a419abcea1c958c80dc843fe4f01e6c5", crc32: "9b40c37e",
            date: null, size: 17_144_597, entries: 1_821,
            wadName: "iddm1.wad", note: null, wadType: WadType.PWAD),
        new(name: "DOOM KEX: id Deathmatch Pack #1", version: "1.2/2", nameSubtext: "Doom + Doom II",
            md5: "5670fd8fe8eb6910ec28f9e27969d84f", sha1: "cb7b39fe4083f8c9d327ac6e51bfcc7fb7cf07f9", crc32: "11fe5048",
            date: "2024-08-08", size: 24_769_102, entries: 2_030,
            wadName: "iddm1.wad", note: null, wadType: WadType.PWAD),

        new(name: "DOOM KEX: Legacy of Rust Expansion", version: "1.0/1", nameSubtext: "Doom + Doom II",
            md5: "aacb9c789bb64ac818a95deb1a972276", sha1: "12cd943bb02d9d53c6e4b0faec892fecb7459865", crc32: "29b97b8d",
            date: null, size: 34_741_757, entries: 2_450,
            wadName: "'id1.wad", note: null, wadType: WadType.PWAD),
        new(name: "DOOM KEX: Legacy of Rust Expansion", version: "1.2", nameSubtext: "Doom + Doom II",
            md5: "681bcea18c1286e8b9986c335034bdd1", sha1: "6905630f20d512844f8c9692d021da4df243b978", crc32: "e2e73e06",
            date: "2024-08-08", size: 34_793_603, entries: 2_450,
            wadName: "id1.wad", note: null, wadType: WadType.PWAD),
        new(name: "DOOM KEX: Legacy of Rust Expansion", version: "1.3", nameSubtext: "Doom + Doom II",
            md5: "95f21547be5e0bff38d412017440f656", sha1: "9848ba3d1726ce738811cef22ad55e2e6af4bbf4", crc32: "3a495080",
            date: "2024-10-03", size: 34_816_611, entries: 2_450,
            wadName: "id1.wad", note: null, wadType: WadType.PWAD),

        new(name: "DOOM KEX: Legacy of Rust Expansion: Resources", version: "1.0/1", nameSubtext: "Doom + Doom II",
            md5: "8fd969d2bafb1f00b53488a41b250981", sha1: "8612609fa5a1f8fb0b5c52f283aaeddc1c5e4bb0", crc32: "63e519b8",
            date: null, size: 18_268_916, entries: 2_207,
            wadName: "id1-res.wad", note: ID1_RES_DEFAULT_NOTE, wadType: WadType.PWAD),
        new(name: "DOOM KEX: Legacy of Rust Expansion: Resources", version: "1.2", nameSubtext: "Doom + Doom II",
            md5: "b6b2370ae8733aaf1377b0ef12351572", sha1: "6ec1646b348a8ca52a5a6ad386668127d87cff4b", crc32: "0b9a1202",
            date: "2024-08-08", size: 18_263_585, entries: 2_206,
            wadName: "id1-res.wad", note: ID1_RES_DEFAULT_NOTE, wadType: WadType.PWAD),
        new(name: "DOOM KEX: Legacy of Rust Expansion: Resources", version: "1.3", nameSubtext: "Doom + Doom II",
            md5: "f8fbab472230bfa090d6a9234d65fae6", sha1: "50bd7932c28c0d936de40529c2f13ca847855420", crc32: "5b7dea04",
            date: "2024-10-03", size: 18_266_239, entries: 2_206,
            wadName: "id1-res.wad", note: ID1_RES_DEFAULT_NOTE, wadType: WadType.PWAD),

        new(name: "DOOM KEX: Legacy of Rust Expansion: New Weapons", version: "1.0/1", nameSubtext: "Doom + Doom II",
            md5: "02e65fc8dd2b1cbf900cb10e65893182", sha1: "f7dbebca89a4ac7018a92945a8b99bc06d811cf4", crc32: "c94d63c3",
            date: null, size: 124_384, entries: 6,
            wadName: "id1-weap.wad", note: ID1_WEAP_DEFAULT_NOTE, wadType: WadType.PWAD),
        new(name: "DOOM KEX: Legacy of Rust Expansion: New Weapons", version: "1.2", nameSubtext: "Doom + Doom II",
            md5: "b50da800b17db51fa06b5191becad82d", sha1: "b877cba17b5306f9201f672a77a1babb615d0480", crc32: "3ade4c54",
            date: "2024-08-08", size: 123_775, entries: 6,
            wadName: "id1-weap.wad", note: ID1_WEAP_DEFAULT_NOTE, wadType: WadType.PWAD),
        new(name: "DOOM KEX: Legacy of Rust Expansion: New Weapons", version: "1.3", nameSubtext: "Doom + Doom II",
            md5: "85d25c8c3d06a05a1283ae4afe749c9f", sha1: "2e4e975208a95202aef3000d44795315197747eb", crc32: "ec52fdb9",
            date: "2024-10-03", size: 133_656, entries: 6,
            wadName: "id1-weap.wad", note: ID1_WEAP_DEFAULT_NOTE, wadType: WadType.PWAD),

        // uwu
        new(name: "BoyKisser Pet (MOD)", version: "1.0", nameSubtext: null,
            md5: "50fb3ba4f99704a33d03aab04272ccb8", sha1: "23f8c0b1cc672fed7bd897299211c7f4b799eeaf", crc32: "94ace071",
            date: null, size: 6_323_033, entries: 124,
            wadName: "owo~", note: "George will never read this, but I love him", wadType: WadType.PWAD),
        new(name: "BoyKisser Pet", version: "2.0", nameSubtext: null,
            md5: "974375e448f2d3aad703d31b189aac9e", sha1: "2f2705f219288dc07e77ab9afa662bfb486d8d8b", crc32: "a6bf7758",
            date: null, size: 8_783_018, entries: 262,
            wadName: "uwu~", note: "uwu", wadType: WadType.PWAD),
    ];
    public static IReadOnlyList<InternalWad> ALL_IWADS { get; } = [
        ..DOOM1, ..DOOM, ..DOOM2, ..DOOM64, ..TNT, ..PLUTONIA, ..HERETIC1,
        ..HERETIC, ..HEXEN, ..HEXDD, ..STRIFE0, ..STRIFE1, ..CHEX, ..CHEX3,
        ..HACX, ..FREEDOOM, ..FREEDOOM1, ..FREEDOOM2, ..FREEDM, ..OTHERS
    ];
    public static IReadOnlyList<InternalWad> COMPLETE { get; } = [
        ..ALL_IWADS, ..PWADS
    ];

    public static IReadOnlyList<InternalWad> PORTS { get; } = [
        new(name: "Doom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "doom.exe", note: null, wadType: null),
        new(name: "Doom II: Hell on Eart", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "doom2.exe", note: null, wadType: null),
        new(name: "Heretic ", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "heretic.exe", note: null, wadType: null),
        new(name: "Hexen", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "hexen.exe", note: null, wadType: null),
        new(name: "Hexen: Deathkings of the Dark Citadel", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "hexendk.exe", note: null, wadType: null),
        new(name: "Strife (Shareware)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "strife.exe", note: null, wadType: null),
        new(name: "Strife", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "strife1.exe", note: null, wadType: null),
        new(name: "WinDoom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "windoom.exe", note: null, wadType: null),
        new(name: "Doom 95", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "doom95.exe", note: null, wadType: null),
        new(name: "ZDoom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "zdoom.exe", note: null, wadType: null),
        new(name: "GZDoom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "gzdoom.exe", note: null, wadType: null),
        new(name: "QZDoom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "qzdoom.exe", note: null, wadType: null),
        new(name: "LDoom LE (Win 9x)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "ldoom98.exe", note: null, wadType: null),
        new(name: "LDoom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "ldoom.exe", note: null, wadType: null),
        new(name: "ZDoom32", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "zdoom32.exe", note: null, wadType: null),
        new(name: "ZDoom32 (MinGW)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "zdoom32_n.exe", note: null, wadType: null),
        new(name: "ZDoom32 (SSE2)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "zdoom32_sse2.exe", note: null, wadType: null),
        new(name: "Skulltag", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "skulltag.exe", note: null, wadType: null),
        new(name: "Doomsday Engine", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "doomsday.exe", note: null, wadType: null),
        new(name: "Doomsday Server", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "doomsday-server.exe", note: null, wadType: null),
        new(name: "Chocolate Doom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "chocolate-doom.exe", note: null, wadType: null),
        new(name: "Chocolate Heretic", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "chocolate-heretic.exe", note: null, wadType: null),
        new(name: "Chocolate Hexen", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "chocolate-hexen.exe", note: null, wadType: null),
        new(name: "Chocolate Strife", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "chocolate-strife.exe", note: null, wadType: null),
        new(name: "CnDoom (Doom)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "cndoom.exe", note: null, wadType: null),
        new(name: "CnDoom (Heretic)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "cnheretic.exe", note: null, wadType: null),
        new(name: "CnDoom (Hexen)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "cnhexen.exe", note: null, wadType: null),
        new(name: "CnDoom (Strife)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "cnstrife.exe", note: null, wadType: null),
        new(name: "CvDoom (Server)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "cnserver.exe", note: null, wadType: null),
        new(name: "ZDaemon", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "zdaemon.exe", note: null, wadType: null),
        new(name: "ZDaemon (Server)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "zserv32.exe", note: null, wadType: null),
        new(name: "PrBoom+", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "prboom-plus.exe", note: null, wadType: null),
        new(name: "PrBoom+ (Server)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "prboom-plus_server.exe", note: null, wadType: null),
        new(name: "PrBoom+ (OpenGL)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "glboom-plus.exe", note: null, wadType: null),
        new(name: "Zandronum", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "zandronum.exe", note: null, wadType: null),
        new(name: "Risen3D", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "risen3d.exe", note: null, wadType: null),
        new(name: "Odamex", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "odamex.exe", note: null, wadType: null),
        new(name: "Odamex (Server)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "odasrv.exe", note: null, wadType: null),
        new(name: "EDGE", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "edge.exe", note: null, wadType: null),
        new(name: "Vavoom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "vavoom.exe", note: null, wadType: null),
        new(name: "Vavoom (Server)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "vavoom-dedicated.exe", note: null, wadType: null),
        new(name: "Eternity Engine", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "eternity.exe", note: null, wadType: null),
        new(name: "Doom Legacy", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "doomlegacy.exe", note: null, wadType: null),
        new(name: "ReMooD", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "remood.exe", note: null, wadType: null),
        new(name: "Doom Retro", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "doomretro.exe", note: null, wadType: null),
        new(name: "PrBoom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "prboom.exe", note: null, wadType: null),
        new(name: "PrBoom (Server)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "prboom_server.exe", note: null, wadType: null),
        new(name: "PrBoom (OpenGL)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "glboom.exe", note: null, wadType: null),
        new(name: "Boom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "boom.exe", note: null, wadType: null),
        new(name: "csDoom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "doomcl.exe", note: null, wadType: null),
        new(name: "Crispy Doom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "crispy-doom.exe", note: null, wadType: null),
        new(name: "Crispy Doom (Server)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "crispy-server.exe", note: null, wadType: null),
        new(name: "jDoom (Doom)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "jdoom.exe", note: null, wadType: null),
        new(name: "jDoom (Heretic)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "jheretic.exe", note: null, wadType: null),
        new(name: "jDoom (Hexen)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "jhexen.exe", note: null, wadType: null),
        new(name: "WDMP", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "wdmp.exe", note: null, wadType: null),
        new(name: "LxDoom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "lxdoom.exe", note: null, wadType: null),
        new(name: "LxDoom (Server)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "lxdoom-game-server.exe", note: null, wadType: null),
        new(name: "ZDoomGL", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "zdoomgl.exe", note: null, wadType: null),
        new(name: "Mocha Doom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "mochadoom.exe", note: null, wadType: null),
        new(name: "Mocha Doom (Win 7)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "mochadoom7.exe", note: null, wadType: null),
        new(name: "Strawberry Doom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "strawberry-doom.exe", note: null, wadType: null),
        new(name: "Strawberry Doom (Server)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "strawberry-server.exe", note: null, wadType: null),
        new(name: "GLDoom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "gldoom.exe", note: null, wadType: null),
        new(name: "Doom3D", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "doom3d.exe", note: null, wadType: null),
        new(name: "Smack My Marine Up (SMMU)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "smmu.exe", note: null, wadType: null),
        new(name: "Doom Plus", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "doomplus.exe", note: null, wadType: null),
        new(name: "ZDaemonGL", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "zdaemongl.exe", note: null, wadType: null),
        new(name: "DoomGL", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "doomgl.exe", note: null, wadType: null),
        new(name: "DOSDoom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "dosdoom.exe", note: null, wadType: null),
        new(name: "TASDoom", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "tasdoom.exe", note: null, wadType: null),
        new(name: "Marine's Best Friend (MBF)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "mbf.exe", note: null, wadType: null),
        new(name: "Linux Doom (X)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "linuxxdoom.exe", note: null, wadType: null),
        new(name: "Linux Doom (SVGAlib)", version: null, nameSubtext: null,
            md5: string.Empty, sha1: string.Empty, crc32: string.Empty,
            date: null, size: 0, entries: 0,
            wadName: "linuxsdoom.exe", note: null, wadType: null),
    ];
    #endregion Internal data

    #region Helpers
    //[Conditional("DEBUG")]
    [Conditional("ENSURE_NO_INTERNAL_WAD_DUPLICATES")]
    public static void EnsureNoDuplicates() {
        Debug.Print("Scanning for duplicate internal wads.");

        for (int i = 0; i < COMPLETE.Count - 1; i++) {
            InternalWad wad = COMPLETE[i];
            for (int x = i + 1; x < COMPLETE.Count; x++) {
                InternalWad wad2 = COMPLETE[x];

                bool equals = wad.EqualsAny(wad2);

                if (!equals) {
                    continue;
                }

                Debug.Assert(!equals);
                Debug.Print($"EQUAL IWAD VALUE FOUND\r\nCurrent: {wad.Name}\r\nDupe: {wad2.Name}");
            }
        }

        ScannedForDuplicates = true;
        Debug.Print("Scan complete");
    }

    public const int WadHeaderLength = 5;

    public static WadType GetWadType(FileInfo file) {
        if (!file.Exists || file.Length < WadHeaderLength) {
            return WadType.Unknown;
        }
        using var fileStream = file.OpenRead();
        Span<byte> bytes = stackalloc byte[WadHeaderLength];
        fileStream.Read(bytes);
        return GetWadType(bytes, file);
    }
    public static WadType GetWadType(ReadOnlySpan<byte> bytes, FileInfo file) {
        return bytes switch {
            [0x49, 0x57, 0x41, 0x44, ..] =>
                file.FullName.EndsWith(".wad", StringComparison.OrdinalIgnoreCase)
                || file.FullName.EndsWith(".iwad", StringComparison.OrdinalIgnoreCase) ? WadType.IWAD : WadType.IWADIncorrectlyMarked,

            [0x50, 0x57, 0x41, 0x44, ..] =>
                file.FullName.EndsWith(".wad", StringComparison.OrdinalIgnoreCase)
                || file.FullName.EndsWith(".pwad", StringComparison.OrdinalIgnoreCase) ? WadType.PWAD : WadType.PWADIncorrectlyMarked,

            [0x50, 0x4B, 0x03, 0x04, ..] =>
                file.FullName.EndsWith(".ipk3", StringComparison.OrdinalIgnoreCase) ? WadType.IPK3 : WadType.PK3,

            [0x37, 0x7A, ..] =>
                file.FullName.EndsWith(".ipk7", StringComparison.OrdinalIgnoreCase) ? WadType.IPK7 :
                (file.FullName.EndsWith(".pk7", StringComparison.OrdinalIgnoreCase) ? WadType.PK7 : WadType.PK7IncorrectlyMarked),

            [0x50, 0x41, 0x43, 0x4B, ..] => WadType.PACK,
            [0x57, 0x41, 0x44, 0x32, ..] => WadType.WAD2,
            [0x57, 0x41, 0x44, 0x33, ..] => WadType.WAD3,
            [0x50, 0x61, 0x74, 0x63, 0x68, ..] => WadType.Dehack,
            _ => WadType.Unknown,
        };
    }

    public static int CountArchiveEntries(ReadOnlySpan<byte> bytes) {
        using MemoryStream ms = new(bytes.ToArray());
        using ZipArchive archive = new(ms, ZipArchiveMode.Read);
        return archive.Entries.Count;
    }
    public static WadEntry[]? EnumerateWadFileEntries(ReadOnlySpan<byte> bytes, FileInfo file, bool scanEntriesCrc32, WadType? wadType) {
        if ((wadType ?? GetWadType(bytes, file)) is not WadType.IWAD and not WadType.PWAD) {
            return null;
        }

        int entryCount = BitConverter.ToInt32(bytes[4..8]);
        int dirIndex = BitConverter.ToInt32(bytes[8..12]);
        List<WadEntry> entries = new(entryCount);

        for (int i = 0; i < entryCount; i++, dirIndex += 16) {
            int location = BitConverter.ToInt32(bytes[dirIndex..(dirIndex + 4)]);
            int size = BitConverter.ToInt32(bytes[(dirIndex + 4)..(dirIndex + 8)]);
            string name = Encoding.ASCII.GetString(bytes[(dirIndex + 8)..(dirIndex + 16)]).Trim().RemoveChar('\0');
            WadEntry newEntry = new(location, size, name, scanEntriesCrc32 ? bytes : []);
            entries.Add(newEntry);
        }

        return entries.Count > 0 ? [.. entries] : [];
    }

    public static InternalWad? FindInternalWad(LoadedInternal loadedWad) {
        if (!loadedWad.File.Exists || loadedWad.File.Length < 5) {
            return null;
        }
        return FindInternalWad(loadedWad.MD5, loadedWad.SHA1, loadedWad.CRC32, loadedWad.File.Length);
    }
    public static InternalWad? FindInternalWad(Hash md5, Hash sha1, Hash crc32, long size) {
        foreach (var wad in ALL_IWADS.Where(x => x.Size == size)) {
            if (md5.ToString().Equals(wad.MD5, StringComparison.OrdinalIgnoreCase)
            || sha1.ToString().Equals(wad.SHA1, StringComparison.OrdinalIgnoreCase)
            || crc32.ToString().Equals(wad.CRC32, StringComparison.OrdinalIgnoreCase)) {
                return wad;
            }
        }

        return null;
    }
    public static InternalWad? FindInternalPWad(LoadedExternal loadedWad) {
        if (!loadedWad.IsFile || !loadedWad.File.Exists || loadedWad.File.Length < 5) {
            return null;
        }
        return FindInternalPWad(loadedWad.MD5, loadedWad.SHA1, loadedWad.CRC32, loadedWad.File.Length);
    }
    public static InternalWad? FindInternalPWad(Hash md5, Hash sha1, Hash crc32, long size) {
        foreach (var wad in PWADS.Where(x => x.Size == size)) {
            if (md5.ToString().Equals(wad.MD5, StringComparison.OrdinalIgnoreCase)
            || sha1.ToString().Equals(wad.SHA1, StringComparison.OrdinalIgnoreCase)
            || crc32.ToString().Equals(wad.CRC32, StringComparison.OrdinalIgnoreCase)) {
                return wad;
            }
        }

        return null;
    }
    public static InternalWad? FindAnyInternalWad(LoadedInternal loadedWad) {
        if (!loadedWad.File.Exists || loadedWad.File.Length < 5) {
            return null;
        }
        return FindAnyInternalWad(loadedWad.MD5, loadedWad.SHA1, loadedWad.CRC32, loadedWad.File.Length);
    }
    public static InternalWad? FindAnyInternalWad(LoadedExternal loadedWad) {
        if (!loadedWad.IsFile || !loadedWad.File.Exists || loadedWad.File.Length < 5) {
            return null;
        }
        return FindAnyInternalWad(loadedWad.MD5, loadedWad.SHA1, loadedWad.CRC32, loadedWad.File.Length);
    }
    public static InternalWad? FindAnyInternalWad(Hash md5, Hash sha1, Hash crc32, long size) {
        return FindInternalWad(md5, sha1, crc32, size) ?? FindInternalPWad(md5, sha1, crc32, size);
    }
    public static InternalWad? FindAnyInternalWad(FileInfo file) {
        file.Refresh();

        if (!file.Exists || file.Length < 5) {
            return null;
        }

        Hash md5 = default, sha1 = default, crc32 = default;
        Task.WaitAll(Task.Run(() => md5 = Hash.Calculate(file, Hash.CalculateMD5)),
            Task.Run(() => sha1 = Hash.Calculate(file, Hash.CalculateSHA1)),
            Task.Run(() => crc32 = Hash.Calculate(file, Hash.CalculateCRC32)));

        return FindAnyInternalWad(md5, sha1, crc32, file.Length);
    }
    public static InternalWad? FindAnyInternalPort(string fileName) {
        for (int i = 0; i < PORTS.Count; i++) {
            var port = PORTS[i];
            if (port.WadName.Equals(fileName, StringComparison.OrdinalIgnoreCase)) {
                return port;
            }
        }

        return null;
    }

    public static bool IsDehack(string fileExtension) {
        // DeHackEd
        return fileExtension.EndsWith(".deh", StringComparison.OrdinalIgnoreCase);
    }
    public static bool IsDehackBoomEx(string fileExtension) {
        // DeHackEd with BOOM Extensions
        return fileExtension.EndsWith(".bex", StringComparison.OrdinalIgnoreCase);
    }
    public static bool IsAnyDehack(string fileExtension) {
        return fileExtension.EndsWith(".deh", StringComparison.OrdinalIgnoreCase)
            || fileExtension.EndsWith(".bex", StringComparison.OrdinalIgnoreCase);
    }
    public static bool IsWadFile(string fileExtension) {
        return fileExtension.EndsWith(".wad", StringComparison.OrdinalIgnoreCase)
            || fileExtension.EndsWith(".iwad", StringComparison.OrdinalIgnoreCase)
            || fileExtension.EndsWith(".zip", StringComparison.OrdinalIgnoreCase)
            || fileExtension.EndsWith(".pk3", StringComparison.OrdinalIgnoreCase)
            || fileExtension.EndsWith(".ipk3", StringComparison.OrdinalIgnoreCase)
            || fileExtension.EndsWith(".pk7", StringComparison.OrdinalIgnoreCase)
            || fileExtension.EndsWith(".ipk7", StringComparison.OrdinalIgnoreCase)
            || fileExtension.EndsWith(".p7z", StringComparison.OrdinalIgnoreCase)
            || fileExtension.EndsWith(".pkz", StringComparison.OrdinalIgnoreCase)
            || fileExtension.EndsWith(".pke", StringComparison.OrdinalIgnoreCase);
    }
    public static bool IsSupportedFile(string extension) {
        return IsWadFile(extension) || IsAnyDehack(extension);
    }
    #endregion Helpers

    private InternalWad(string name, string? version, string? nameSubtext, string md5, string sha1, string crc32, string? date, long size, int entries, string wadName, string? note, WadType? wadType) {
        this.Name = name ?? "Unknown";
        this.Version = version ?? "Unknown";
        this.NameSubtext = nameSubtext;
        this.MD5 = md5 ?? "No MD5 provided";
        this.SHA1 = sha1 ?? "No SHA1 provided";
        this.CRC32 = crc32 ?? "No CRC32 provided";
        this.Date = date;
        this.Size = Math.Max(0 , size);
        this.Entries = Math.Max(0, entries);
        this.Note = note;
        this.WadName = wadName ?? "No WAD name provided";
        this.WadType = wadType ?? WadType.Unknown;
    }

    #region Properties
    public string Name { get; }
    public string? Version { get; }
    public string? NameSubtext { get; }

    public string MD5 { get; }
    public string SHA1 { get; }
    public string CRC32 { get; }

    public string? Date { get; }
    public long Size { get; }
    public int Entries { get; }
    public string? Note { get; }

    public string WadName { get; }
    public WadType WadType { get; }
    #endregion Properties

    #region Methods
    private string CompareEntries(int? entries) {
        return entries > -1 ?
            $"\r\nEntries {(this.Entries == entries ? $"match [{entries:N0}]" : $"mismatch [{entries:N0}]\r\nExpected Entries [{this.Entries}]")}" : string.Empty;
    }
    private string GetNote() {
        return this.Note.IsNullEmptyWhitespace() ? string.Empty : "\r\n\r\nNote: " + this.Note;
    }
    public string GenerateWadComparisonString(Hash md5, Hash sha1, Hash crc32, long length, int? entries) {
        string? wadType = this.WadType switch {
            WadType.IWAD => "Internal WAD (IWAD)",
            WadType.PWAD => "Patch WAD (PWAD)",
            WadType.PACK => "PACK WAD",
            WadType.WAD2 => "WAD2",
            WadType.WAD3 => "WAD3",
            WadType.PK3 => "PK3 archive (PWAD)",
            WadType.IPK3 => "Internal PK3 archive (IWAD)",
            WadType.PK7 => "PK7 / 7z archive (PWAD)",
            WadType.IPK7 => "Internal PK7 / 7z archive (IWAD)",
            WadType.IWADIncorrectlyMarked => "Internal WAD (IWAD) (incorrect header)",
            WadType.PWADIncorrectlyMarked => "Patch WAD (PWAD) (incorrect header)",
            WadType.PK3IncorrectlyMarked => "PK3 archive (PWAD) (incorrect header)",
            WadType.IPK3IncorrectlyMarked => "Internal PK3 archive (IWAD) (incorrect header)",
            WadType.PK7IncorrectlyMarked => "PK7 / 7z archive (PWAD) (incorrect header)",
            WadType.IPK7IncorrectlyMarked => "Internal PK7 / 7z archive (IWAD) (incorrect header)",
            _ => null,
        };

        if (wadType is null) {
            return $$"""
                -- Internal WAD information --

                No known WAD match found.

                MD5 [{{md5}}]
                SHA1 [{{sha1}}]
                CRC32 [{{crc32}}]
                Size [{{length:N0}}]
                """ + (entries > -1 ? $"Entries [{entries}]" : string.Empty);
        }

        return $$"""
            -- Internal WAD information --

            {{this.Name}}{{(this.NameSubtext.IsNullEmptyWhitespace() ? string.Empty : $"\r\nSUB: {this.NameSubtext}")}}
            {{wadType}}
            FILE NAME: {{this.WadName}}
            VERSION: {{this.Version}}
            ENTRIES: {{(this.Entries > 0 ? this.Entries.ToString("N0") : "N/A")}}{{(this.Date.IsNullEmptyWhitespace() ? string.Empty : $"\r\nDATE: {this.Date}")}}

            MD5 {{(md5.ToString().Equals(this.MD5, StringComparison.InvariantCultureIgnoreCase) ? $"match [{md5}]" : $"mismatch [{md5}]\r\nExpectd MD5 [{this.MD5.ToUpperInvariant()}]")}}
            SHA1 {{(sha1.ToString().Equals(this.SHA1, StringComparison.InvariantCultureIgnoreCase) ? $"match [{sha1}]" : $"mismatch [{sha1}]\r\nExpectd SHA1 [{this.SHA1.ToUpperInvariant()}]")}}
            CRC32 {{(crc32.ToString().Equals(this.CRC32, StringComparison.InvariantCultureIgnoreCase) ? $"match [{crc32}]" : $"mismatch [{crc32}]\r\nExpectd CRC32 [{this.CRC32.ToUpperInvariant()}]")}}
            Size {{(this.Size == length ? $"match [{length:N0}]" : $"mismatch [{length:N0}]\r\nExpected Size [{this.Size}]")}}
            """ + this.CompareEntries(entries)
                + this.GetNote();
    }

    public string GetFullString() {
        if (string.IsNullOrWhiteSpace(this.Version)) {
            if (string.IsNullOrWhiteSpace(this.NameSubtext)) {
                return $"{this.Name}";
            }
            return $"{this.Name} ({this.NameSubtext})";
        }

        if (string.IsNullOrWhiteSpace(this.NameSubtext)) {
            return $"{this.Name} v{this.Version}";
        }

        return $"{this.Name} v{this.Version} ({this.NameSubtext})";
    }

    public bool EqualsAny(InternalWad other) {
        return other.MD5.Equals(this.MD5, StringComparison.InvariantCultureIgnoreCase)
            || other.SHA1.Equals(this.SHA1, StringComparison.InvariantCultureIgnoreCase)
            || other.CRC32.Equals(this.CRC32, StringComparison.InvariantCultureIgnoreCase);
    }
    public bool FullyEquals(InternalWad other) {
        return other.MD5.Equals(this.MD5, StringComparison.InvariantCultureIgnoreCase)
            && other.SHA1.Equals(this.SHA1, StringComparison.InvariantCultureIgnoreCase)
            && other.CRC32.Equals(this.CRC32, StringComparison.InvariantCultureIgnoreCase);
    }

    public override bool Equals(object? obj) {
        return obj is InternalWad other && this.EqualsAny(other);
    }
    public override int GetHashCode() {
        return this.Name.GetHashCode();
    }
    public override string ToString() {
        return this.GetFullString();
    }
    #endregion Methods
}
