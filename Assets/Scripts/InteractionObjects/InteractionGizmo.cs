using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionGizmo : MonoBehaviour
{
    private float radius = 3f;         // 부채꼴의 반지름
    private float angle = 90f;         // 부채꼴의 각도 (중앙에서 양쪽으로 펼쳐지는 각도)
    private int numSteps = 10;         // 부채꼴을 그릴 때 사용되는 분할 수 (원 그릴 때의 점들)

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;  // 기즈모 색상 설정

        // 부채꼴 그리기
        DrawFan(transform.position, transform.forward, radius, angle, numSteps);
    }

    void DrawFan(Vector3 position, Vector3 direction, float radius, float angle, int numSteps)
    {
        // 부채꼴의 시작 각도와 끝 각도 계산
        float startAngle = -angle / 2;
        float endAngle = angle / 2;

        // 부채꼴의 바닥 원을 그리기 위한 각도 계산
        Vector3[] points = new Vector3[numSteps + 1];

        // 부채꼴의 끝 점들 계산 (각도에 따른 점들 계산)
        for (int i = 0; i <= numSteps; i++)
        {
            float stepAngle = Mathf.Lerp(startAngle, endAngle, i / (float)numSteps) * Mathf.Deg2Rad;
            float x = Mathf.Cos(stepAngle) * radius;
            float z = Mathf.Sin(stepAngle) * radius;
            points[i] = position + transform.forward * x + transform.right * z;
        }

        // 부채꼴의 바닥 원을 그리기 위한 점을 연결하여 부채꼴 형태 그리기
        for (int i = 0; i < numSteps; i++)
        {
            Gizmos.DrawLine(points[i], points[i + 1]);
        }

        // 부채꼴의 중심에서 바닥 원의 각 점들을 연결하여 부채꼴의 범위 그리기
        foreach (var point in points)
        {
            Gizmos.DrawLine(position, point);
        }
    }
}
