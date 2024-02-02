using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NovelGameEditor
{
    public record CommandData
    {
        public TMP_Text NovelTMP { get; }
        public Image[] CharaImages { get; }
        public Image[] Diff { get; }
        public Image BackgroundImage { get; }
        public Image BackgronndDiff { get; }
        public MonoBehaviour MonoBehaviour { get; }

        public CommandData(TMP_Text novelTMP, Image[] charaImage, Image[] charaDiff,Image background, Image backgronndDiff, MonoBehaviour monoBehaviour)
        {
            NovelTMP = novelTMP;
            CharaImages = charaImage;
            Diff = charaDiff;
            BackgroundImage = background;
            BackgronndDiff = backgronndDiff;
            MonoBehaviour = monoBehaviour;
        }
    }
}
