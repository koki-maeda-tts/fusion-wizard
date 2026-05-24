using System;
using CombinationMagician.Utility;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CombinationMagician.Players.Weapons
{
    /// <summary>
    /// 合体魔法を生成するクラス
    /// </summary>
    public class CombinationMagicGenerator : SceneSingleton<CombinationMagicGenerator>
    {
        [SerializeField, Header("隕石魔法")]
        GameObject _meteorite;

        [SerializeField, Header("氷魔法")]
        GameObject _ice;

        [SerializeField, Header("竜巻魔法")]
        GameObject _tornado;

        [Header("魔法のダメージ倍率")]
        [SerializeField]
        float _meteoDamageMultiplier;

        [SerializeField]
        float _iceDamageMultiplier;

        [SerializeField]
        float _tornadoDamageMultiplier;

        [SerializeField, Header("隕石を生成する数")]
        int _numOfCreateMeteo;

        [SerializeField, Header("隕石生成時のx軸オフセット")]
        float _offsetXOfMeteo;

        [SerializeField, Header("隕石生成の間隔(秒)")]
        float _waitSecondsOfMeteo;

        [SerializeField, Header("氷を生成する数")]
        int _numOfCreateIce;

        [SerializeField, Header("氷生成時のx軸オフセット")]
        float _offsetXOfIce;

        [SerializeField, Header("氷生成の間隔(秒)")]
        float _waitSecondsOfIce;

        /// <summary>
        /// これがtrueのときのみ合体魔法を生成する
        /// </summary>
        public bool IsActive { get; set; } = true;

        void CreateMagic(float posX, float damage, GameObject gameObject, float damageMultiplier)
        {
            if (!IsActive)
            {
                return;
            }
            var obj = Instantiate(gameObject);
            var magic = obj.GetComponent<ICombinationMagic>();
            magic.Init(posX, damage * damageMultiplier);
        }

        async UniTask CreateMagics(Action<int> magicCreater, int createCount, float waitSeconds)
        {
            for (int i = 0; i < createCount; i++)
            {
                magicCreater(i);
                await UniTask.WaitForSeconds(
                    waitSeconds,
                    cancellationToken: destroyCancellationToken
                );
            }
        }

        public void CreateMeteorite(float posX, float damage) =>
            CreateMagics(
                    i =>
                        CreateMagic(
                            posX + i * _offsetXOfMeteo,
                            damage,
                            _meteorite,
                            _meteoDamageMultiplier
                        ),
                    _numOfCreateMeteo,
                    _waitSecondsOfMeteo
                )
                .Forget();

        public void CreateIce(float posX, float damage) =>
            CreateMagics(
                    i => CreateMagic(posX + i * _offsetXOfIce, damage, _ice, _iceDamageMultiplier),
                    _numOfCreateIce,
                    _waitSecondsOfIce
                )
                .Forget();

        public void CreateTornado(float posX, float damage) =>
            CreateMagic(posX, damage, _tornado, _tornadoDamageMultiplier);
    }
}
