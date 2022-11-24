using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Labyrinth.Helpers
{
    public class MovimentAnimationHelper : MonoBehaviour
    {
        [Header("References")]
        public GameObject target;

        [Tooltip("Set all positions that object will moviment")]
        public List<Transform> positions;
        public Transform startPosition;

        [Space()]
        [Header("Callback")]
        public UnityEvent<int, Transform> onEnterPosition;
        
        [Space()]
        [Header("Settings")]
        public float duration = 1f;
        public bool isRandom = false;
        public bool isLoop = true;

        private int _index = 0;
        private Coroutine _coroutine;

        public void NextPosition()
        {
            if (isRandom)
            {
                _index = Random.Range(0, positions.Count);
                return;
            }

            _index++;

            if (_index >= positions.Count)
            {
                if (!isLoop) // if loop has false, stop movement when are last position
                    StopAnimation();
                else
                    _index = 0;
            }

            onEnterPosition?.Invoke(_index, positions[_index]);
        }

        private void StopAnimation()
        {
            StopCoroutine(_coroutine);
        }

        private void Start()
        {
            target.transform.localPosition = startPosition.localPosition;
            _coroutine = StartCoroutine(StartMoviment());
        }

        IEnumerator StartMoviment()
        {
            float time = 0;

            while (true)
            {
                var currentPosition = target.transform.localPosition;

                while (time < duration)
                {
                    target.transform.localPosition = Vector3.Lerp(currentPosition, positions[_index].transform.localPosition, (time / duration));
                    
                    time += Time.deltaTime;
                    yield return null;
                }

                NextPosition();

                time = 0;

                yield return null;

            }
        }
    }
}
