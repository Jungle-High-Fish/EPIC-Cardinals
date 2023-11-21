using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Cardinals.Enums;

namespace Cardinals
{
    public class PlayerInfo 
    {
        public int Gold { get; private set; }
        
        private List<Potion> _potions;
        public Action<int, Potion> AddPotionEvent { get; set; }

        private List<Artifact> _artifacts;
        public Action<Artifact> AddArtifactEvent { get; set; }
        public PlayerInfo()
        {
            _potions = new();
            for(int i=0; i < Constants.GameSetting.Player.MaxPotionCapacity; i++)
            {
                _potions.Add(null);
            }

            _artifacts = new();
            for(int i=0; i < Constants.GameSetting.Player.MaxArtifactCapacity; i++)
            {
                _artifacts.Add(null);
            }
        }
        private bool _isBlessFire1;
        public bool IsBlessFire1 // 그을린 상처
        {
            get => _isBlessFire1;
            set
            {
                _isBlessFire1 = value;
            }
        }
        private bool _isBlessFire2;
        public bool IsBlessFire2 // 메테오
        {
            get => _isBlessFire2;
            set
            {
                _isBlessFire2 = value;
            }
        }
        private bool _isBlessWater1;
        public bool IsBlessWater1 // 잔잔한 수면
        {
            get => _isBlessWater1;
            set
            {
                _isBlessWater1 = value;
            }
        }
        private bool _isBlessWater2;
        public bool IsBlessWater2 // 범람
        {
            get => _isBlessWater2;
            set
            {
                _isBlessWater2 = value;
            }
        }
        private bool _isBlessEarth1;
        public bool IsBlessEarth1 // 바위 잔해
        {
            get => _isBlessEarth1;
            set
            {
                _isBlessEarth1 = value;
            }
        }
        private bool _isBlessEarth2;
        public bool IsBlessEarth2 // 깨지지 않는 바위
        {
            get => _isBlessEarth2;
            set
            {
                _isBlessEarth2 = value;
            }
        }

       
        public void AddPotion(PotionType potionType)
        {
            for (int i = 0; i < Constants.GameSetting.Player.MaxPotionCapacity; i++)
            {
                if(_potions[i] == null)
                {
                    Potion potion = EnumHelper.GetPotion(potionType);
                    _potions[i] = potion;
                    AddPotionEvent?.Invoke(i, potion);
                    break;
                }
            }
        }


        public void DeletePotion(int index)
        {
            _potions[index].DeletePotionEvent?.Invoke(_potions[index]);
            _potions[index] = null;
        }

        public void UsePotion(int index)
        {
            _potions[index].UsePotion();
            DeletePotion(index);
        }

        public void AddArtifact(ArtifactType artifactType)
        {
            for (int i = 0; i < Constants.GameSetting.Player.MaxArtifactCapacity; i++)
            {
                if (_artifacts[i] == null&&!CheckArtifactExist(artifactType))
                {
                    Artifact artifact = EnumHelper.GetArtifact(artifactType);
                    _artifacts[i] = artifact;
                    AddArtifactEvent?.Invoke(artifact);
                    break;
                }
            }
        }

        public bool CheckArtifactExist(ArtifactType artifactType)
        {
            foreach(Artifact artifact in _artifacts)
            {
                if (artifact == null)
                {
                    continue;
                }
                if(artifact.Type == artifactType)
                {
                    return true;
                }
            }
            return false;
        }

        public void DebugArtifactList()
        {
            foreach(Artifact artifact in _artifacts)
            {
                if (artifact == null)
                    continue;
                Debug.Log(artifact.Name);
            }
        }
    

        public void AddGold(int value)
        {
            Gold += value;
        }

        public void UseGold(int value)
        {
            Gold -= value;
        }
    }
}
