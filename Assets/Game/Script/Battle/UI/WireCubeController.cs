using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace RPG.Battle.UI
{
    public class WireCubeController : MonoBehaviour
    {
        private void Start()
        {
            StartAnimation().Forget();
        }

        private async UniTask StartAnimation()
        {
            while (true)
            {
                var target = new Vector3(
                    UnityEngine.Random.Range(-360, 360), 
                    UnityEngine.Random.Range(-360, 360),
                    UnityEngine.Random.Range(-360, 360));

                await transform.DORotate(target, UnityEngine.Random.Range(1F, 6F))
                    .SetEase(Ease.InOutElastic)
                    .ToUniTask(cancellationToken: destroyCancellationToken);
            }
        }
    }
}
