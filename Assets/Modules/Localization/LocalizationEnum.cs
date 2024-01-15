namespace Cardinals {
    public enum LocalizationEnum {
        // Common
        UI_MAIN_CONTINUE,
        UI_MAIN_NEW_START,
        UI_MAIN_START,
        UI_MAIN_BACK,
        UI_MAIN_DICE_SELECT,
        UI_GAMESETTING_TITLE,
        UI_GAMESETTING_FULLSCREEN,
        UI_GAMESETTING_RESOLUTION,
        UI_GAMESETTING_BGM,
        UI_GAMESETTING_LANGUAGE,
        UI_GAMESETTING_LANGUAGE_WARN,
        UI_GAMESETTING_SFX,
        UI_CLOSE,
        UI_EXIT_GAME,
        UI_INIT_DICE_SELECT,
        UI_ROULETTE_SPIN,
        UI_TILE_SEALED,
        UI_TILE_SEALED_INFO,
        UI_TILE_CURSE,
        UI_TILE_CURSE_ABOUT,
        STAGE_SUSPICIOUS_MEADOW,
        TILE_MAGIC_ATTACK_NAME,
        TILE_MAGIC_ATTACK_EXPLAIN,
        TILE_MAGIC_DEFENSE_NAME,
        TILE_MAGIC_DEFENSE_EXPLAIN,
        TILE_MAGIC_FIRE_NAME,
        TILE_MAGIC_FIRE_EXPLAIN,
        TILE_MAGIC_WATER_NAME,
        TILE_MAGIC_WATER_EXPLAIN,
        TILE_MAGIC_EARTH_NAME,
        TILE_MAGIC_EARTH_EXPLAIN,
        BUFF_BURN_NAME,
        BUFF_BURN_EXPLAIN,
        BUFF_WEAK_NAME,
        BUFF_WEAK_EXPLAIN,
        BUFF_ELECTRICSHOCK_NAME,
        BUFF_ELECTRICSHOCK_EXPLAIN,
        BUFF_POISON_NAME,
        BUFF_POISON_EXPLAIN,
        BUFF_SLOW_NAME,
        BUFF_SLOW_EXPLAIN,
        BUFF_HEAL_NAME,
        BUFF_HEAL_EXPLAIN,
        BUFF_CONFUSION_NAME,
        BUFF_CONFUSION_EXPLAIN,
        BUFF_DOLL_NAME,
        BUFF_DOLL_EXPLAIN,
        BUFF_ROTATIONRATE_NAME,
        BUFF_ROTATIONRATE_EXPLAIN,
        BUFF_POWERLESS_NAME,
        BUFF_POWERLESS_EXPLAIN,
        BUFF_BERSERK_NAME,
        BUFF_BERSERK_EXPLAIN,
        BUFF_GROWTH_NAME,
        BUFF_GROWTH_EXPLAIN, 
        BUFF_GROWTH_EXPLAIN2,
        POTION_JUMP_NAME,
        POTION_JUMP_EXPLAIN,
        POTION_BACK_NAME,
        POTION_BACK_EXPLAIN,
        POTION_NUM4_NAME,
        POTION_NUM4_EXPLAIN,
        POTION_NUM3_NAME,
        POTION_NUM3_EXPLAIN,
        POTION_NUM2_NAME,
        POTION_NUM2_EXPLAIN,
        POTION_NUM1_NAME,
        POTION_NUM1_EXPLAIN,
        POTION_HEAL_NAME,
        POTION_HEAL_EXPLAIN,
        POTION_GROWTH_NAME,
        POTION_GROWTH_EXPLAIN,
        OBJECT_POTIONGOBLIN_NAME,
        OBJECT_POTIONGOBLIN_EXPLAIN,
        OBJECT_DICEEVENT_NAME,
        OBJECT_DICEEVENT_EXPLAIN,
        OBJECT_ROULETTE_NAME,
        OBJECT_ROULETTE_EXPLAIN,
        OBJECT_RYUKA_NAME,
        OBJECT_RYUKA_EXPLAIN,
        OBJECT_FIREBALL_NAME,
        OBJECT_FIREBALL_EXPLAIN,
        OBJECT_ALCHEMY_NAME,
        OBJECT_ALCHEMY_EXPLAIN,
        DICE_NORMAL_NAME,
        DICE_NORMAL_DESCRIPTION,
        DICE_FIRE_NAME,
        DICE_FIRE_DESCRIPTION,
        DICE_WATER_NAME,
        DICE_WATER_DESCRIPTION,
        DICE_EARTH_NAME,
        DICE_EARTH_DESCRIPTION,
        TILE_CURSE_THUNDERBOLT_NAME,
        TILE_CURSE_THUNDERBOLT_EXPLAIN,
        TILE_CURSE_LOCK_NAME,
        TILE_CURSE_LOCK_EXPLAIN,
        TILE_CURSE_EMBER_NAME,
        TILE_CURSE_EMBER_EXPLAIN,
        BLESS_EARTH1_NAME,
        BLESS_EARTH1_EXPLAIN,
        BLESS_EARTH2_NAME,
        BLESS_EARTH2_EXPLAIN,
        BLESS_FIRE1_NAME,
        BLESS_FIRE1_EXPLAIN,
        BLESS_FIRE2_NAME,
        BLESS_FIRE2_EXPLAIN,
        BLESS_WATER1_NAME,
        BLESS_WATER1_EXPLAIN,
        BLESS_WATER2_NAME,
        BLESS_WATER2_EXPLAIN,
        BLESS_WIND1_NAME,
        BLESS_WIND1_EXPLAIN,
        BLESS_WIND2_NAME,
        BLESS_WIND2_EXPLAIN,
        EVENT_DICEEVENT_WARNING,
        PLAYER_SCRIPT_START,
        PLAYER_SCRIPT_ATTACK,
        PLAYER_SCRIPT_HIT,
        PLAYER_SCRIPT_WIN,
        PLAYER_SCRIPT_SLOW,
        PLAYER_SCRIPT_ELECTRICSHOCK,
        PLAYER_SCRIPT_LOCK,
        PLAYER_SCRIPT_GAMEOVER,
        PLAYER_SCRIPT_TUTORIAL,
        PLAYER_SCRIPT_REROLL,
        PLAYER_SCRIPT_TUTORIAL1,
        PLAYER_SCRIPT_CONFUSE,
        PLAYER_SCRIPT_POTION,
        ENEMY_ACTION_ATTACK_NAME,
        ENEMY_ACTION_ATTACK_EXPLAIN,
        ENEMY_ACTION_DEFENSE_NAME,
        ENEMY_ACTION_DEFENSE_EXPLAIN,
        ENEMY_ACTION_TILECURSE_NAME,
        ENEMY_ACTION_TILECURSE_EXPLAIN,
        ENEMY_ACTION_TILEDEBUFF_NAME,
        ENEMY_ACTION_TILEDEBUFF_EXPLAIN,
        ENEMY_ACTION_USERDEBUFF_NAME,
        ENEMY_ACTION_USERDEBUFF_EXPLAIN,
        ENEMY_ACTION_SLEEP_NAME,
        ENEMY_ACTION_SLEEP_EXPLAIN,
        ENEMY_ACTION_SPAWN_NAME,
        ENEMY_ACTION_SPAWN_EXPLAIN,
        ENEMY_ACTION_AREAATTACK_NAME,
        ENEMY_ACTION_AREAATTACK_EXPLAIN,
        ENEMY_ACTION_CONFUSION_NAME,
        ENEMY_ACTION_CONFUSION_EXPLAIN,
        ENEMY_ACTION_NOACTION_NAME,
        ENEMY_ACTION_NOACTION_EXPLAIN,
        TUOTORIAL_QUEST1_TITLE,
        TUOTORIAL_QUEST1_DES1,
        TUOTORIAL_QUEST1_DES2,
        TUOTORIAL_QUEST1_DES3,
        TUOTORIAL_QUEST1_DES4,
        TUOTORIAL_QUEST2_TITLE,
        TUOTORIAL_QUEST2_DES1,
        TUOTORIAL_QUEST2_DES2,
        TUOTORIAL_QUEST2_DES3,
        TUOTORIAL_QUEST2_DES4,
        TUOTORIAL_QUEST3_TITLE,
        TUOTORIAL_QUEST3_DES1,
        TUOTORIAL_QUEST3_DES2,
        TUOTORIAL_QUEST3_DES3,
        TUOTORIAL_QUEST3_DES4,
        TUOTORIAL_QUEST3_DES5,
        TUOTORIAL_QUEST3_DES6,
        TUOTORIAL_QUEST4_TITLE,
        TUOTORIAL_QUEST4_DES1,
        TUOTORIAL_QUEST4_DES2,
        TUOTORIAL_QUEST5_TITLE,
        TUOTORIAL_QUEST5_DES1,
        TUOTORIAL_QUEST5_DES2,
        TUOTORIAL_END1,
        TUOTORIAL_END2,
        UI_INGAME_BLESS,
        UI_INGAME_POTION,
        UI_INGAME_CURRENTTILE,
        UI_INGAME_TURN,
        UI_INGAME_LAP,
        UI_INGAME_ENDTURN,
        UI_INGAME_NEXTEVENT,
        UI_TURN_AN1,
        UI_TURN_AN2,
        UI_DICE_REROLL,
        EVENT_ALCHEMY_ALLTILE,
        EVENT_ALCHEMY_DAMAGE,
        EVENT_ALCHEMY_MONEY,
        EVENT_ALCHEMY_POTION,
        EVENT_ALCHEMY_HP,
        EVENT_DICE_MONEY,
        EVENT_DICE_HEAL,
        EVENT_ROULETTE_DAM,
        EVENT_ROULETTE_DICE,
        EVENT_ROULETTE_MONEY,
        EVENT_ROULETTE_POTION,
        EVENT_ROULETTE_TILE,
        EVENT_ROULETTE_HP,
        EVENT_DICE_MONEY1,
        EVENT_DICE_HEAL2,
        EVENT_ALCHEMY_TITLE,
        EVENT_ALCHEMY_DES,
        EVENT_ALCHEMY_ROLL,
        GAMEOVER_NUM_ROUND,
        GAMEOVER_NUM_DICE,
        GAMEOVER_NUM_MONSTER,
        GAMEOVER_NUM_PLAYTIME,
        GAMEOVER_BUTTON_RE,
        GAMEOVER_BUTTON_TITLE,
        MAGIC_LEVELUP_TITLE1,
        MAGIC_LEVELUP_TITLE2,
        MAGIC_LEVELUP_BT1,
        MAGIC_LEVELUP_BT2,
        MAGIC_LEVELUP_BT3,
        OPTION_TITLE,
        OPTION_CONTINUE,
        OPTION_SETTING,
        OPTION_MAINMENU,
        OPTION_QUIT,
        UI_ITEM_USE,
        UI_ITEM_REMOVE,
        UI_ITEM_NO_MORE_ITEM,
        UI_GETDICE,
        UI_DICE_TITLE,
        UI_DICE_TRADE,
        UI_TUTORIAL_SKIP,
        MONSTER_THEROCK,
        MONSTER_POPO,
        MONSTER_PIPI,
        MONSTER_DOLDOL,
        MONSTER_CHARRRRRRRRRRRRRUK,
        MONSTER_NULLUNULLU,
        MONSTER_TWETWE,
        MONSTER_MUMU,
        MONSTER_PAZIZIZIZIC,
        MONSTER_KROL,
        MONSTER_PICPIC,
        UI_LOAD_TITLE,
        UI_LOAD_LOAD,
        REWARD_GOLD,
        REWARD_DICE,
        REWARD_CLEAR,

    }
}