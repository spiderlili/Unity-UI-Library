using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//lays out a grid relative to the number of children, supports spacing & padding & support different types of layouts (FitType)

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        Uniform, //scales cells uniformly based on the number of children, often creates a lot of empty space after a certain number of children
        Width, //scales cells by prioritising width to make use of more space
        Height, //scales cells by prioritising height to make use of more space
        FixedRows, //limit the grid to a fixed number of rows
        FixedColumns //limit the grid to a fixed number of columns
    }

    public FitType fitType;

    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;

    public bool fitX;
    public bool fitY;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {
            fitX = true;
            fitY = true;
            rows = Mathf.CeilToInt(transform.childCount / (float) columns);
            //calculate the number of rows & columns by finding the square root of the number of children in the transform
            float squareRoot = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(squareRoot);
            columns = Mathf.CeilToInt(squareRoot);
        }

        if (fitType == FitType.Width || fitType == FitType.FixedColumns || fitType == FitType.Uniform)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float) columns);
        }

        if (fitType == FitType.Height || fitType == FitType.FixedRows || fitType == FitType.Uniform)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float) rows);
        }

        //get the width & height of container so it knows how much space are there to work with
        float parentContainerWidth = rectTransform.rect.width;
        float parentContainerHeight = rectTransform.rect.height;

        //calculate a size for each child based on the info available. reduce width & height of each child cell relative to the spacing added => prevent overflow
        //support padding: offset x & y position of cells relative to the padding values
        float cellWidth = (parentContainerWidth / (float) columns) - (spacing.x / (float) columns * 2) - (padding.left / (float) columns) - (padding.right / (float) columns);
        float cellHeight = (parentContainerHeight / (float) rows) - (spacing.y / (float) rows * 2) - (padding.left / (float) rows) - (padding.right / (float) rows);

        //assign the calculated width & height values to the x & y position of cellSize
        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        //keep a count of column & row indexes as it lays out everything
        int columnCount = 0;
        int rowCount = 0;

        //iterate over all children in the rect transform & find current row/column index
        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            //get a reference to child object's x & y position 
            //reduce the width & height of cells relative to the padding to prevent overflow
            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            //call SetChildAlongAxis for each axis
            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
    }

    public override void CalculateLayoutInputVertical()
    {
        //base.CalculateLayoutInputHorizontal();
    }

    public override void SetLayoutHorizontal()
    {
        //base.SetLayoutHorizontal();
    }

    public override void SetLayoutVertical()
    {
        //base.SetLayoutVertical();
    }

}