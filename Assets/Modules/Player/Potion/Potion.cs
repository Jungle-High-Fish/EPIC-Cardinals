using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals
{
    public abstract class Potion : IProduct
    {
        private int _money;
        public virtual int Money { get; set; }
        private string _name;
        public virtual string Name { get; set; }

        public Action<Potion> DeletePotionEvent { get; set; }
        public virtual void UsePotion()
        {

        }

        #region IProduct
        public Sprite Sprite { get; }
        public string Description { get; }
        public int Price => Money;
        #endregion
    }
}
