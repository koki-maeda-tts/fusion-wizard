using CombinationMagician.EditorExtensions;
using GetOnlyPropertyGenerator;
using UnityEngine;

namespace CombinationMagician.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "PlayerParameters",
        menuName = "Scriptable Objects/PlayerParameters"
    )]
    public partial class PlayerParameters : ScriptableObject
    {
        [SerializeField, Header("初期HP"), GenerateGetOnlyProperty]
        float _initHp;

        [SerializeField, Header("移動速さ"), GenerateGetOnlyProperty]
        float _moveSpeed;

        [SerializeField, Header("初期投射速さ"), GenerateGetOnlyProperty]
        float _initProjectionSpd;

        [SerializeField, Header("追加の投射速さ"), GenerateGetOnlyProperty]
        float _addendProjectionSpd;

        [SerializeField, Header("投射制限角度(度)"), GenerateGetOnlyProperty]
        float _limitAngle;

        [SerializeField, Header("魔法"), GenerateGetOnlyProperty]
        GameObject[] _magics;

        [SerializeField, Header("攻撃したあとのクールタイム(秒)"), GenerateGetOnlyProperty]
        float _coolSeconds;

        [SerializeField, Tag, Header("ボタンのタグ"), GenerateGetOnlyProperty]
        string _buttonUiTag;

        [SerializeField, Header("ダメージを受けたときのSE"), GenerateGetOnlyProperty]
        AudioClip _damageSE;
    }
}
