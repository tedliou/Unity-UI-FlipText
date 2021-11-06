using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace BlondieUtils {
    public class FlipText :MonoBehaviour {
        [Header("Setting")]
        public Mode mode = Mode.AutoFlip;

        public enum Mode {
            AutoFlip = 0,
            TargetFlip = 1
        }

        [Range(0f, 1f)] public float animateDuration = .5f;
        [Range(0f, 1f)] public float delayDuration = 1f;

        public string flipString = "0123456789";

        [Header("Component")]
        public TextMeshProUGUI fixedTop;
        public TextMeshProUGUI last;
        public TextMeshProUGUI next;
        public TextMeshProUGUI current;
        public GameObject currentObj;

        private string _currentValue;
        private Coroutine _work;

        private void Start () {
            _currentValue = flipString[0].ToString();
            current.text = _currentValue;
            last.text = flipString[0].ToString();
            next.text = flipString[1].ToString();
            fixedTop.text = flipString[1].ToString();

            if (mode == Mode.AutoFlip) _work = StartCoroutine(FlipWork());
        }

        private void Flip (string currentValue, string nextValue) {
            currentObj.transform.rotation = Quaternion.Euler(Vector3.zero);
            currentObj.transform.DOKill();
            currentObj.transform.DORotate(new Vector3(-180, 0, 0), animateDuration).OnKill(() => {
                currentObj.transform.rotation = Quaternion.Euler(Vector3.zero);

                current.text = last.text = currentValue;
                next.text = fixedTop.text = nextValue;
            });
        }

        public void TargetFlip(string target) {
            if (_work != null) StopCoroutine(_work);
            _work = StartCoroutine(FlipWork(target));
        }

        private IEnumerator FlipWork (string target = "") {
            yield return new WaitForSeconds(delayDuration);

            var index = 1;
            while (true)
            {
                if (mode == Mode.TargetFlip)
                    if (_currentValue == target) break;

                _currentValue = flipString[index].ToString();

                var nextIndex = 0;
                if (( index + 1 ) < flipString.Length) nextIndex = index + 1;
                Flip(_currentValue, flipString[nextIndex].ToString());

                index++;
                if (index == flipString.Length) index = 0;
                yield return new WaitForSeconds(delayDuration);
            }
        }
    }
}