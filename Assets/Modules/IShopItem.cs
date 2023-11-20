using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.BoardEvent.Shop
{
    public interface IShopItem
    {
        public string Name { get; }
        public string Description { get; }
        public Sprite Sprite { get; } // Çה! 
        public int Price { get; }

    }
}