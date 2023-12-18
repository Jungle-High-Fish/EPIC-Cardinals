using System;
using System.Security.Cryptography;
using System.Linq;
using Cardinals.Enums;
using Cardinals.Game;
using Unity.VisualScripting;
using System.Collections.Generic;
using Cardinals.Board;

namespace Cardinals {
    [Serializable]
    public class SaveFileData {
        // Savefile Info
        public string fileName;
        public DateTime saveTime;
        public bool isCloudSave;

        // Player Info
        public BlessType[] BlessList;
        public PotionType[] PotionList;
        public int Coin;
        public int MaxHP;
        public int HP;
        public int OnTileIndex;

        // Dice Info
        public DiceSaveData[] DiceList;

        // Stage Info
        public List<StageEventListWrapper> StageEventSequence;
        public int CurrentStageIndex;
        public int ClearedStageEventIndex;
        public List<TileSaveData> TileSaveDataList;

        // Player Result Data
        public int TurnCount;
        public int DiceRollingCount;
        public int ExecuteEnemyCount;
        public ulong PlayTime;
        
        // Savefile Validation
        public string ValidationCode;

        public bool Validate() {
            return ValidationCode == GenerateValidationCode();
        }

        public void SetData(bool isCloudSave, string fileName, Player player, DiceManager diceManager, Stage[] stage, int clearedStageIndex) {
            SetPlayerData(player);
            SetDiceData(diceManager);
            SetStageData(stage, clearedStageIndex);
            SetTileData();
            SetGameData(GameManager.I);
            SetSaveFileInfo(fileName, isCloudSave);

            ValidationCode = GenerateValidationCode();
        }

        public void UpdateData(Player player, DiceManager diceManager, Stage currentStage, int currentStageIndex) {
            SetPlayerData(player);
            SetDiceData(diceManager);
            SetGameData(GameManager.I);
            CurrentStageIndex = currentStageIndex;
            ClearedStageEventIndex = currentStage.Index - 1;
            SetTileData();

            ValidationCode = GenerateValidationCode();
        }

        public List<(int[], DiceType)> GetDiceList() {
            var diceList = new List<(int[], DiceType)>();

            foreach (var dice in DiceList) {
                diceList.Add((dice.DiceNumbers, dice.DiceType));
            }

            return diceList;
        }

        [Serializable]
        public struct DiceSaveData {
            public int[] DiceNumbers;
            public DiceType DiceType; 
        }

        private void SetPlayerData(Player player) {
            var playerInfo = player.PlayerInfo;

            BlessList = playerInfo.BlessList.ToArray();
            List<PotionType> potionList = new List<PotionType>();
            foreach (var potion in playerInfo.PotionList) {
                if (potion != null) {
                    potionList.Add(potion.PotionType);
                }
            }
            PotionList = potionList.ToArray();
            Coin = playerInfo.Gold;
            MaxHP = player.MaxHp;
            HP = player.Hp;
            OnTileIndex = GameManager.I.Stage.Board.GetTileIndex(player.OnTile);
        }

        private void SetDiceData(DiceManager diceManager) {
            DiceList = diceManager.Dices.ConvertAll(dice => {
                var diceSaveData = new DiceSaveData() {
                    DiceNumbers = dice.DiceNumbers.ToArray(),
                    DiceType = dice.DiceType
                };

                return diceSaveData;
            }).ToArray();
        }

        private void SetStageData(Stage[] stage, int currentStageIndex) {
            CurrentStageIndex = currentStageIndex;
            ClearedStageEventIndex = stage[currentStageIndex].Index - 1;
            if (ClearedStageEventIndex < -1) {
                ClearedStageEventIndex = -1;
            }

            StageEventSequence = new List<StageEventListWrapper>();
            for (int i = 0; i < stage.Length; i++) {
                StageEventSequence.Add(new StageEventListWrapper(stage[i].EventNames));
            }
        }

        private void SetTileData() {
            Board.Board board = GameManager.I.Stage.Board;
            TileSaveDataList = new List<TileSaveData>();
            for (int i = 0; i < board.TileSequence.Count; i++) {
                var tile = board.TileSequence[i];
                var tileSaveData = new TileSaveData() {
                    TileMagicType = tile.TileMagic.Type,
                    Level = tile.TileMagic.Level,
                    Exp = tile.TileMagic.Exp
                };

                TileSaveDataList.Add(tileSaveData);
            }
        }

        private void SetGameData(GameManager game)
        {
            TurnCount = game.TurnCount;
            DiceRollingCount = game.DiceRollingCount;
            ExecuteEnemyCount = game.ExecuteEnemyCount;
            PlayTime = game.PlayTime;
        }

        private void SetSaveFileInfo(string fileName, bool isCloudSave) {
            this.fileName = fileName;
            saveTime = DateTime.Now;
            this.isCloudSave = isCloudSave;
        }

        private string GenerateValidationCode() {
            string validationString = "";

            // savefile info
            validationString += fileName;
            validationString += saveTime.ToString();
            
            // player info
            validationString += string.Join("", BlessList);
            validationString += string.Join("", PotionList);
            validationString += Coin;
            validationString += MaxHP;
            validationString += HP;
            validationString += OnTileIndex;
            
            // dice info
            validationString += string.Join("", DiceList);
            
            // stage info
            for (int i = 0; i < StageEventSequence.Count; i++) {
                validationString += string.Join("", StageEventSequence[i]);
            }
            validationString += CurrentStageIndex;
            validationString += ClearedStageEventIndex;
            
            // Play Data
            validationString += TurnCount;
            validationString += DiceRollingCount;
            validationString += ExecuteEnemyCount;
            validationString += PlayTime;

            // generate validation code
            var tmpSource = System.Text.Encoding.UTF8.GetBytes(validationString);
            var tmpHash = new SHA512Managed().ComputeHash(tmpSource);

            return Convert.ToBase64String(tmpHash);
        }

        [Serializable]
        public struct StageEventListWrapper {
            public StageEventList[] StageEventList;

            public StageEventListWrapper(StageEventList[] stageEventList) {
                StageEventList = stageEventList;
            }
        }

        [Serializable]
        public struct TileSaveData {
            public TileMagicType TileMagicType;
            public int Level;
            public int Exp;
        }
    }
}