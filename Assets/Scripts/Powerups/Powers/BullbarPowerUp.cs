using UnityEngine;

public class BullbarPowerUp : PowerUp
{
    [SerializeField] private float murderSpeedBonus = 2.5f;

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
        PlayerData.Instance.murderSpeed -= murderSpeedBonus;
    }

    public override bool CanExecute()
    {
        return true;
    }
}
