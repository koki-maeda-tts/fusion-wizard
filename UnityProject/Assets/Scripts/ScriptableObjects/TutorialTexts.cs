using System;
using GetOnlyPropertyGenerator;
using UnityEngine;
using static CombinationMagician.Tutorial.TutorialManager;

namespace CombinationMagician.ScriptableObjects
{
    /// <summary>
    /// チュートリアルで表示するテキストの情報
    /// </summary>
    [CreateAssetMenu(fileName = "TutorialTexts", menuName = "Scriptable Objects/TutorialTexts")]
    public partial class TutorialTexts : ScriptableObject
    {
        [Serializable]
        public class Info
        {
            /// <summary>
            /// 表示するテキスト
            /// </summary>
            [SerializeField]
            string _text;
            public string Text => _text;

            /// <summary>
            /// 次のテキストに進むための条件
            /// </summary>
            [SerializeField]
            TransitionConditions _condition;
            public TransitionConditions Condition => _condition;
        }

        [SerializeField, GenerateGetOnlyProperty]
        Info[] _infos;
    }
}
