using UnityEngine;

namespace Cardinals.UI
{
    
    /// <summary>
    /// DescriptionConnector 클래스에서 호출하는 인터페이스로
    /// 출력할 설명 정보를 포함하고 있음
    /// </summary>
    public interface IDescription
    {
        string Name { get; }
        string Description { get; }
        Sprite IconSprite { get; }
        
        Color Color { get; }
        
        string Key { get; }
    }
}