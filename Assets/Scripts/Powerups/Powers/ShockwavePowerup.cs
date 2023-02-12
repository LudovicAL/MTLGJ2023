using UnityEngine;

public class ShockwavePowerUp : PowerUp
{
    public override void Acquire()
    {
        foreach (var model in models)
        {
            model.SetActive(true);
        }
        Execute();
        base.Acquire();
    }

    public override void Execute()
    {
        
    }

    public override bool CanExecute()
    {
        return true;
    }
}
