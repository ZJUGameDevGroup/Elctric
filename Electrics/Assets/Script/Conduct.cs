using UnityEngine;

public interface IConduct : IAttachedBy<ICharacter>
{
    float Movable { get; set; }
    float Electric { get; set; }
    
}