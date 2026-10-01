using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    /// <summary>
    /// ブラスターの移動情報を入力する
    /// </summary>
    public struct MoveInfo
    {
        public float x;
        public float y;
        public Vector3 vector3;
        public int di;

        public MoveInfo(Vector3 vector3, int di)
        {
            this.vector3 = vector3;
            this.di = di;
            this.x = vector3.x;
            this.y = vector3.y;
        }
        public MoveInfo(float x, float y, int di)
        {
            this.x = x;
            this.y = y;
            this.di = di;
            this.vector3 = new Vector3(x, y);
        }
    }
    /// <summary>
    /// ブラスターの初期位置を入力する
    /// </summary>
    public struct StartInfo
    {
        public float x;
        public float y;
        public Vector3 vector3;
        public int di;

        public StartInfo(Vector3 vector3, int di)
        {
            this.vector3 = vector3;
            this.di = di;
            this.x = vector3.x;
            this.y = vector3.y;
        }

        public StartInfo(float x, float y, int di)
        {
            this.x = x;
            this.y = y;
            this.di = di;
            this.vector3 = new Vector3(x, y);
        }
    }
    public enum BlasterColor
    {
        white,
        orange,
        blue

    }
    public enum FlyDirection
    {
        /// <summary>
        /// 上に動く。
        /// </summary>
        Up,
        /// <summary>
        /// 下に動く。
        /// </summary>
        Down,
        /// <summary>
        /// 左に動く。
        /// </summary>
        Left,
        /// <summary>
        /// 右に動く。
        /// </summary>
        Right,
    }

    public enum MessageType
    {
        /// <summary>
        /// 何も表示しない。
        /// </summary>
        None,
        /// <summary>
        /// 文字を表示する。
        /// </summary>
        Text,
        /// <summary>
        /// 画像を表示する。
        /// </summary>
        Image,
        /// <summary>
        /// 選択肢を表示する。
        /// </summary>
        Choice,
        /// <summary>
        /// シーンを切り替える。
        /// </summary>
        SceneChange,
    }

    public enum EnemyMovement
    {
        /// <summary>
        /// 左右に動く。
        /// </summary>
        Horizontal,
        /// <summary>
        /// 上下に動く。
        /// </summary>
        Vertical,
        
    }
}
