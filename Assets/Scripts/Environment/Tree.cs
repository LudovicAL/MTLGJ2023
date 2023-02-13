using UnityEngine;

public class Tree : MonoBehaviour
{
   private bool _isDead ;
   private void OnCollisionEnter(Collision collision)
   {
      if (!_isDead && collision.transform.CompareTag("Player") && PlayerData.Instance.envCollisionDoDamage)
      {
         _isDead = true;
         PlayerData.Instance.Hp.Add(-1);
      }
   }

   private void OnBecameInvisible()
   {
      if (_isDead)
      {
         this.gameObject.SetActive(false);
      }
   }
}
