using System.Threading.Tasks;
using UnityEngine;

public interface IPlayer  
{
     public Transform transform { get; }
     public void SetTargetPosition(Vector3 targetPos);
}

