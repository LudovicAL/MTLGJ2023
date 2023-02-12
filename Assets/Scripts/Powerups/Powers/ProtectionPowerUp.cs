using UnityEngine;

public class ProtectionPowerUp : PowerUp
{
    [SerializeField] private int hpBoost = 6;

    public override void Acquire()
    {
        Execute();
        base.Acquire();
    }

    public override void Execute()
    {
        PlayerData.Instance.Hp.AddMax(hpBoost);
    }

    public override bool CanExecute()
    {
        return true;
    }
}
