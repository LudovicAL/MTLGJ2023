using UnityEngine;

public class JerricanPowerUp : PowerUp
{
    [SerializeField] private int fuelBoost = 25;

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
