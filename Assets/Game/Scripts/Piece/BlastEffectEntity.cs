using UnityEngine;

namespace Game.Scripts.Piece
{
    public class BlastEffectEntity : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private Transform _transform;
        
        public void SetEffect(EPiece pieceType)
        {
            _particleSystem.Clear();
            _particleSystem.Simulate(0f);
            _particleSystem.Play();
        }
    }
}