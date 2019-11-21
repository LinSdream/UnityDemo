using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class MoveBase : MonoBehaviour
    {
        #region Fields
        public float MoveTime = 0.1f;
        public LayerMask BlockingLayer;

        BoxCollider2D _collider;
        Rigidbody2D _body;
        float _inverseMoveTime;
        #endregion

        #region MonoBehaviour Callbacks
        protected void Start()
        {
            _collider = GetComponent<BoxCollider2D>();
            _body = GetComponent<Rigidbody2D>();

            _inverseMoveTime = 1f / MoveTime;
            Init();
        }

        #endregion

        #region Protected Methods
        protected bool Move(float xDir,float yDir,out RaycastHit2D hit)
        {
            Vector2 start = transform.position;
            Vector2 end = start + new Vector2(xDir, yDir);

            _collider.enabled = false;//目的在于防止射线射到自己的collider上
            hit = Physics2D.Linecast(start, end, BlockingLayer);
            _collider.enabled = true;
            //如果无hit，说明是开放空间，可以进行移动
            if (hit.transform == null)
            {
                StartCoroutine(SmoothMovement(end));//进行移动
                return true;
            }
            return false;
        }

        protected virtual void AttemptMove<T>(float xDir, float yDir)
            where T : Component
        {
            RaycastHit2D hit;
            bool canMove = Move(xDir, yDir, out hit);
            if (hit.transform == null)//如果是开放空间，进行移动(协程移动)
                return;

            //如果不是开放空间，调用OnCantMove<T>(T component)进行互动（主要互动内容在于：Player-Wall，Enemy-Player）
            T hitComponent = hit.transform.GetComponent<T>();
            if (!canMove && hitComponent != null)
                OnCantMove<T>(hitComponent);
        }

        /// <summary> Start方法的初始化</summary>
        protected virtual void Init() { }
        #endregion

        #region Coroutines
        protected IEnumerator SmoothMovement(Vector3 end)
        {

            float sqrRemainingDistnace = (transform.position - end).sqrMagnitude;
            while (sqrRemainingDistnace > float.Epsilon)
            {
                Vector3 newPos = Vector3.MoveTowards(_body.position, end, _inverseMoveTime*Time.deltaTime);
                _body.MovePosition(newPos);
                sqrRemainingDistnace = (transform.position - end).sqrMagnitude;
                yield return null;
            }
        }
        #endregion

        #region Abstract Methods
        //这里采用泛型设计在于，player和enemy都具有move特性，
        //因此在cant move的时候需要对不能移动的网格进行互动，无法确定要进行互动的component是谁的，所以进行泛型设计
        protected abstract void OnCantMove<T>(T component)
            where T : Component;
        #endregion
    }

}