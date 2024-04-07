using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Test
{
    public class SquareCircleMosh : MonoBehaviour
    {
        [SerializeField] private Vector3 target;
        private Vector3 _fromPosition;
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float spinSpeed = 8f;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target);
        }

        // Start is called before the first frame update
        void Start()
        {
            _fromPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            Do(transform, ref target, moveSpeed * Time.deltaTime, _fromPosition, spinSpeed * Time.deltaTime);
        }

        private void Do(Transform enemyTransform, ref Vector3 targetPosition, float moveSpeed, Vector3 firstActivePosition, float spinSpeed)
        {
            enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, targetPosition, moveSpeed);
            float radius = Vector3.Distance(firstActivePosition, targetPosition);
            float currentAngle = Mathf.Atan2(targetPosition.y - firstActivePosition.y, targetPosition.x - firstActivePosition.x) * Mathf.Rad2Deg;
            currentAngle += spinSpeed; // 時計回りに角度を増やす
            if (currentAngle >= 360f) currentAngle -= 360f; // 角度が360を超えたらリセット

            // 新しいtarget位置を計算
            float newX = firstActivePosition.x + radius * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
            float newY = firstActivePosition.y + radius * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
            targetPosition = new Vector3(newX, newY, targetPosition.z);
        }
    }
}
