using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Actor", menuName = "Dialogue/Actor")]
public class ActorSO : ScriptableObject
{
    public Actor[] Actors;
}
