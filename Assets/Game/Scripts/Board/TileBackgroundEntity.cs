using UnityEngine;

namespace Game.Scripts.Board
{
    public class TileBackgroundEntity : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        public void SetTransform(Vector3 position, Vector3 scale)
        {
            _transform.position = position;
            _transform.localScale = scale;
        }
    }
}