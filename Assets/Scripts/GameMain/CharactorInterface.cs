using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public interface IDamageable : IEventSystemHandler
{
    void Damage(float atk, float cri);
}