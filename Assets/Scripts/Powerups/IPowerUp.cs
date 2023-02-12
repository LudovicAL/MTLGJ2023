using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerUp
{
    bool IsPassive { get; }
    bool IsAcquired { get; }
    void Acquire();
    void Execute();
    bool CanExecute();
}
