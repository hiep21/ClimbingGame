using DG.Tweening;
using NTHiep.MiniOdin;
using UnityEngine;
using UnityEngine.UI;

namespace NTHiep.Tool
{
    enum TypePopup
    {
        Normal = 0,
    }
    public class PopupUI : MonoBehaviour
    {
        [ColorHeader("<color=blue>Action Popup")]
        [SerializeField] GameObject _panel;
        [SerializeField] GameObject _dimmer;
        [SerializeField] TypePopup _typePopup;
        [SerializeField] float _durationShowPopup = 0.5f;
        [SerializeField] float _durationHidePopup = 0.5f;

        public virtual void ShowPopup(System.Action onComplete = null)
        {
            switch (_typePopup)
            {
                case TypePopup.Normal:
                    _dimmer.SetActive(true);
                    _panel.SetActive(true);

                    _dimmer.GetComponent<Image>().DOFade(0.6f, _durationShowPopup);
                    _panel.transform.DOScale(1f, _durationShowPopup).OnComplete(() =>
                    {
                        onComplete?.Invoke();
                    });
                    break;
            }


        }
        public virtual void HidePopup(System.Action onComplete = null)
        {
            switch (_typePopup)
            {
                case TypePopup.Normal:
                    _dimmer.GetComponent<Image>().DOFade(0f, _durationHidePopup).OnComplete(() =>
                    {
                        _dimmer.SetActive(false);
                    });
                    _panel.transform.DOScale(0f, _durationHidePopup).OnComplete(() =>
                    {
                        _panel.SetActive(false);
                        onComplete?.Invoke();
                    });
                    break;
            }

        }
    }
}