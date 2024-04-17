using Game.Scripts.Binding;
using Game.Scripts.Score;
using Game.Scripts.Timer;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class Provider : MonoBehaviour
{
    public static IServiceLocator ServiceLocator { get; private set; }

    private void Awake()
    {
        ServiceLocator = new ServiceLocator();
        ServiceLocator.AddService<IScoreboard,Scoreboard>(new Scoreboard());
        ServiceLocator.AddService<ITimer,Timer>(new Timer());
    }
}