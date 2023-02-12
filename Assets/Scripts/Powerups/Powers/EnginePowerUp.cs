using UnityEngine;

public class EnginePowerUp : PowerUp
{
    [SerializeField] private int fuelBoost = 10;

    public override void Acquire()
    {
        Execute();
        base.Acquire();
    }

    public override void Execute()
    {
        PlayerData.Instance.Fuel.AddMax(fuelBoost);
    }

    public override bool CanExecute()
    {
        return true;
    }
}
