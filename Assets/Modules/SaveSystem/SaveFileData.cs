using System;
using System.Security.Cryptography;
using System.Linq;
using Cardinals.Enums;
using Cardinals.Game;
using Unity.VisualScripting;

namespace Cardinals {
    [Serializable]
    public class SaveFileData {
        // Savefile Info
        public string fileName;
        public DateTime saveTime;

        // Player Info
        public BlessType[] BlessList;
        public PotionType[] PotionList;
        public int Coin;
        public int MaxHP;
        public int HP;

        // Dice Info
        public DiceSaveData[] DiceList;

        // Stage Info
        public StageEventList[][] StageEventSequence;
        public int ClearedStageIndex;
        public int ClearedStageEventIndex;

        // Savefile Validation
        public string ValidationCode;

        public bool Validate() {
            return ValidationCode == GenerateValidationCode();
        }

        public void SetData(string fileName, Player player, DiceManager diceManager, Stage[] stage, int clearedStageIndex) {
            SetPlayerData(player);
            SetDiceData(diceManager);
            SetStageData(stage, clearedStageIndex);
            SetSaveFileInfo(fileName);

            ValidationCode = GenerateValidationCode();
        }

        [Serializable]
        public struct DiceSaveData {
            public int[] DiceNumbers;
            public DiceType DiceType; 
        }

        private void SetPlayerData(Player player) {
            var playerInfo = player.PlayerInfo;

            BlessList = playerInfo.BlessList.ToArray();
            PotionList = playerInfo.PotionList.ConvertAll(potion => potion.PotionType).ToArray();
            Coin = playerInfo.Gold;
            MaxHP = player.MaxHp;
            HP = player.Hp;
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

        private void SetStageData(Stage[] stage, int clearedStageIndex) {
            ClearedStageIndex = clearedStageIndex;
            ClearedStageEventIndex = stage[clearedStageIndex].Index - 1;

            StageEventSequence = new StageEventList[stage.Length][];
            for (int i = 0; i < stage.Length; i++) {
                StageEventSequence[i] = stage[i].EventNames;
            }
        }

        private void SetSaveFileInfo(string fileName) {
            this.fileName = fileName;
            saveTime = DateTime.Now;
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
            
            // dice info
            validationString += string.Join("", DiceList);
            
            // stage info
            for (int i = 0; i < StageEventSequence.Length; i++) {
                validationString += string.Join("", StageEventSequence[i]);
            }
            validationString += ClearedStageIndex;
            validationString += ClearedStageEventIndex;

            // generate validation code
            var tmpSource = System.Text.Encoding.UTF8.GetBytes(validationString);
            var tmpHash = new SHA512Managed().ComputeHash(tmpSource);

            return Convert.ToBase64String(tmpHash);
        }
    }
}