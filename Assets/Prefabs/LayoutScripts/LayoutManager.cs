using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutManager : MonoBehaviour
{
    [SerializeField] bool doArrange;
    [SerializeField] float horizontalSize;
    float spacing;

    Transform[] children;
    int childNum;

    void Awake()
    {
        childNum = transform.childCount;
        children = new Transform[childNum];
        for(int i=0; i<childNum; i++)
        {
            children[i] = transform.GetChild(i);
        }

        Arrange();
    }

    void Update()
    {
        if (doArrange)
            Arrange();
    }

    void Arrange()
    {

        float horizCameraSize = 2 * Camera.main.orthographicSize * Camera.main.aspect;
        /*        
            numSpaces = numElements + 1         // includes spaces on the sides
            totalSize = numElements*sizeElement + numSpaces*sizeSpaces  
            sizeSpaces = (totalSize - numElements*sizeElement)/numSpaces
         */
        spacing = (horizCameraSize - childNum * horizontalSize) / (childNum + 1);

        if (childNum % 2 == 0) // even num. of children
        {
            int mid = (childNum - 1) / 2;  // sub 1 to have  leftChild = mid  (truncates correctly)
            Transform midLeft, midRight;
            midLeft = children[mid];
            midRight = children[mid + 1];

            float cardOffs = spacing + horizontalSize;      // space between center of a card and center of adjacent card
            Vector3 leftPos = new Vector3(-cardOffs / 2, 0, 0);   // halve because the middle is between 2 cards
            Vector3 rightPos = new Vector3(cardOffs / 2, 0, 0);

            midLeft.localPosition = leftPos;
            midRight.localPosition = rightPos;

            for (int i = 1; (mid - i) >= 0; i++) // does each pair outside of the middle 2
            {
                Transform left = children[mid - i];
                Transform right = children[mid + i + 1];
                leftPos.x -= cardOffs;
                rightPos.x += cardOffs;
                left.localPosition = leftPos;
                right.localPosition = rightPos;
            }
        }
        else    // odd num. of children
        {
            int midIndex = childNum/2;  // value is truncated correctly (e.g. 3 children {0,1,2}, mid is 1)
            Transform mid = children[midIndex];

            float cardOffs = spacing + horizontalSize;      // space between center of a card and center of adjacent card
            Vector3 leftPos = Vector3.zero;
            Vector3 rightPos = Vector3.zero;
            mid.localPosition = Vector3.zero;

            for (int i = 1; (midIndex - i) >= 0; i++) // does each pair outside of the middle card
            {
                Transform left = children[midIndex - i];
                Transform right = children[midIndex + i];
                leftPos.x -= cardOffs;
                rightPos.x += cardOffs;
                left.localPosition = leftPos;
                right.localPosition = rightPos;
            }
        }
    }
}
