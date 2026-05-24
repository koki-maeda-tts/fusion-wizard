using GetOnlyPropertyGenerator;
using UnityEngine;

namespace CombinationMagician.ScriptableObjects
{
    /// <summary>
    /// 各シーンの名前
    /// </summary>
    [CreateAssetMenu(fileName = "SceneNames", menuName = "Scriptable Objects/SceneNames")]
    public partial class SceneNames : ScriptableObject
    {
        [SerializeField, GenerateGetOnlyProperty]
        string _titleSceneName;

        [SerializeField, GenerateGetOnlyProperty]
        string _gameSceneName;

        [SerializeField, GenerateGetOnlyProperty]
        string _tutorialSceneName;
    }
}
