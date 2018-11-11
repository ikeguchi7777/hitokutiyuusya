using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UniRx;

public interface IDamageable : IEventSystemHandler
{
    void Damage(float atk, float cri);
}

public interface IPlayerEvent
{
    Subject<float> RemainHP { get; }
    Subject<float> SpecialGage { get; }
    Subject<float> StrongGage { get; }
}