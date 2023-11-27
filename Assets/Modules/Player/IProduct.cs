using UnityEngine;

namespace Cardinals
{
    public interface IProduct
    {
        public string Name { get; }
        public Sprite Sprite { get; }
        public string Description { get; }
        public int Price { get; }
    }
}