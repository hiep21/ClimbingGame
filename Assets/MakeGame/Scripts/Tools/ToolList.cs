using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTHiep.Tool
{
    public class ToolList
    {
        public static void SortPositionArray<T>(List<T> list, int row, int col, float spacingX, float spacingY, float distanceX = 0f, float distanceY = 0f) where T : MonoBehaviour
        {
            if (list.Count != row * col)
            {

                Debug.LogWarning($"Kích thước của list không khớp với số hàng và cột.{list.Count},{row}/{col}");
                return;
            }

            // Tính tổng chiều rộng và chiều cao của lưới
            float totalWidth = (col - 1) * spacingX;
            float totalHeight = (row - 1) * spacingY;

            // Đặt vị trí cho từng phần tử theo hàng và cột
            for (int i = 0; i < list.Count; i++)
            {
                int currentRow = i / col;     // Tính hàng hiện tại
                int currentColumn = i % col;  // Tính cột hiện tại

                // Tính toán vị trí ban đầu (chưa căn giữa)
                Vector3 newPosition = new Vector3(currentColumn * spacingX, -currentRow * spacingY, 0);

                // Căn giữa bằng cách trừ đi nửa tổng chiều rộng và chiều cao
                newPosition.x -= totalWidth / 2f;
                newPosition.y += totalHeight / 2f;

                // Thêm khoảng cách di chuyển theo trục X và Y
                newPosition.x += distanceX;
                newPosition.y += distanceY;

                // Đặt vị trí mới cho phần tử
                list[i].transform.position = new Vector3(newPosition.x, newPosition.y);
                list[i].transform.localPosition = new Vector3(list[i].transform.localPosition.x, list[i].transform.localPosition.y, 0);

            }
        }


        public static float ChangeSizeBoxWithScreen(Transform posLimitTop, Transform posLimitBottom, Transform posLimitLeft, Transform posLimitRight, GameObject boardBox, int row, int column, float spacingX, float spacingY)
        {
            float ratio = 0.85f;

            float limitMinVertical = 99f;
            float limitMaxVertical = -99f;
            float limitMinHorizontal = 99f;

            // Lấy kích thước màn hình
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, 0)) * 2;
            // Tính toán kích thước hiện tại của grid (với scale = 1)
            float currentWidth = column * spacingX;
            float currentHeight = row * spacingY;


            //Kiểm tra xem giới hạn là ở đâu
            if (posLimitTop == null)
            {
                limitMinVertical = Mathf.Min(limitMinVertical, screenBounds.y / 2);
                limitMaxVertical = Mathf.Max(limitMaxVertical, screenBounds.y / 2);
            }
            else
            {
                limitMinVertical = Mathf.Min(limitMinVertical, posLimitTop.position.y);
                limitMaxVertical = Mathf.Max(limitMaxVertical, posLimitTop.position.y);
            }

            if (posLimitBottom == null)
            {
                limitMinVertical = Mathf.Min(limitMinVertical, screenBounds.y / 2);
                limitMaxVertical = Mathf.Max(limitMaxVertical, screenBounds.y / 2);
            }
            else
            {
                limitMinVertical = Mathf.Min(limitMinVertical, posLimitBottom.position.y);
                limitMaxVertical = Mathf.Max(limitMaxVertical, posLimitBottom.position.y);
            }

            if (posLimitLeft == null)
            {
                limitMinHorizontal = Mathf.Min(limitMinHorizontal, Mathf.Abs(-screenBounds.x / 2));
            }
            else
            {
                limitMinHorizontal = Mathf.Min(limitMinHorizontal, Mathf.Abs(posLimitLeft.position.x));
            }

            if (posLimitRight == null)
            {
                limitMinHorizontal = Mathf.Min(limitMinHorizontal, Mathf.Abs(screenBounds.x / 2));
            }
            else
            {
                limitMinHorizontal = Mathf.Min(limitMinHorizontal, Mathf.Abs(posLimitRight.position.x));
            }


            float newScale = 1f;
            float newSize;
            float offsetY = 0f;

            newSize = limitMinHorizontal * 2 * ratio;

            // bool prioritizeHeightWithRatioX = CheckLimitWithPrioritizeHeight(areaBoardWithRatioX, limitVertical, limitHorizontal);
            // bool prioritizeHeightWithRatioY = CheckLimitWithPrioritizeHeight(areaBoardWithRatioY, limitVertical, limitHorizontal);

            newScale = newSize / currentWidth;
            float newHeight = newScale * currentHeight;

            if (newHeight > limitMaxVertical - limitMinVertical)
            {
                newSize = (limitMaxVertical + Mathf.Abs(limitMinVertical)) * ratio;
                newScale = newSize / currentHeight;
            }

            offsetY = (limitMaxVertical + limitMinVertical) / 2f;


            // Reset và áp dụng scale mới
            boardBox.transform.localScale = Vector3.one;
            boardBox.transform.localScale = new Vector3(newScale, newScale);
            boardBox.transform.position = new Vector3(0f, offsetY, 0f);


            return newScale;
        }



        public static List<T> ConvertTo1DArray<T>(List<List<T>> nestedList)
        {
            List<T> flattenedList = new List<T>();
            // Duyệt qua từng List con và thêm các phần tử vào List kết quả
            foreach (var subList in nestedList)
            {
                flattenedList.AddRange(subList);
            }
            return flattenedList;
        }
        public static List<List<T>> ConvertTo2DArray<T>(List<T> sourceList, int rows, int columns, int itemsPerBox)
        {
            int currentIndex = 0;
            List<List<T>> result = new List<List<T>>();

            // Tạo rows * columns List con
            for (int i = 0; i < rows * columns; i++)
            {
                List<T> boxItems = new List<T>();

                // Thêm itemsPerBox phần tử vào mỗi List con
                for (int j = 0; j < itemsPerBox; j++)
                {
                    boxItems.Add(sourceList[currentIndex]);
                    currentIndex++;
                }
                result.Add(boxItems);
            }
            return result;
        }
    }

}