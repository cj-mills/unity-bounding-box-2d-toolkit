using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace BBox2DToolkit
{
    /// <summary>
    /// A struct that represents a 2D bounding box.
    /// </summary>
    public struct BBox2D
    {
        public float x0;
        public float y0;
        public float width;
        public float height;
        public int index;
        public float prob;

        /// <summary>
        /// Initializes a new instance of the BBox2D struct.
        /// </summary>
        /// <param name="x0">The x-coordinate of the top-left corner.</param>
        /// <param name="y0">The y-coordinate of the top-left corner.</param>
        /// <param name="width">The width of the bounding box.</param>
        /// <param name="height">The height of the bounding box.</param>
        /// <param name="index">The class index of the object.</param>
        /// <param name="prob">The probability of the object belonging to the given class.</param>
        public BBox2D(float x0, float y0, float width, float height, int index, float prob)
        {
            this.x0 = x0;
            this.y0 = y0;
            this.width = width;
            this.height = height;
            this.index = index;
            this.prob = prob;
        }
    }

    /// <summary>
    /// A struct for 2D bounding box information.
    /// </summary>
    public struct BBox2DInfo
    {
        public BBox2D bbox;
        public string label;
        public Color color;

        /// <summary>
        /// Initializes a new instance of the BBox2DInfo struct.
        /// </summary>
        /// <param name="boundingBox">The 2D bounding box.</param>
        /// <param name="label">The class label.</param>
        /// <param name="width">The bounding box color.</param>
        public BBox2DInfo(BBox2D boundingBox, string label = "", Color color = new Color())
        {
            this.bbox = boundingBox;
            this.label = label;
            this.color = color;
        }
    }

    public static class BBox2DUtility
    {
        /// <summary>
        /// Calculates the union area between two bounding boxes.
        /// </summary>
        /// <param name="a">The first bounding box.</param>
        /// <param name="b">The second bounding box.</param>
        /// <returns>The union area between the two bounding boxes.</returns>
        public static float CalcUnionArea(BBox2D a, BBox2D b)
        {
            // Calculate the coordinates and dimensions of the union area
            float x = Mathf.Min(a.x0, b.x0);
            float y = Mathf.Min(a.y0, b.y0);
            float w = Mathf.Max(a.x0 + a.width, b.x0 + b.width) - x;
            float h = Mathf.Max(a.y0 + a.height, b.y0 + b.height) - y;

            // Calculate the union area of two bounding boxes
            return w * h;
        }

        /// <summary>
        /// Calculates the intersection area between two bounding boxes.
        /// </summary>
        /// <param name="a">The first bounding box.</param>
        /// <param name="b">The second bounding box.</param>
        /// <returns>The intersection area between the two bounding boxes.</returns>
        public static float CalcInterArea(BBox2D a, BBox2D b)
        {
            // Calculate the coordinates and dimensions of the intersection area
            float x = Mathf.Max(a.x0, b.x0);
            float y = Mathf.Max(a.y0, b.y0);
            float w = Mathf.Min(a.x0 + a.width, b.x0 + b.width) - x;
            float h = Mathf.Min(a.y0 + a.height, b.y0 + b.height) - y;

            // Calculate the intersection area of two bounding boxes
            return w * h;
        }

        /// <summary>
        /// Performs Non-Maximum Suppression (NMS) on a sorted list of bounding box proposals.
        /// </summary>
        /// <param name="proposals">A sorted list of BBox2D objects representing the bounding box proposals.</param>
        /// <param name="nms_thresh">The NMS threshold for filtering proposals (default is 0.45).</param>
        /// <returns>A list of integers representing the indices of the retained proposals.</returns>
        public static List<int> NMSSortedBoxes(List<BBox2D> proposals, float nms_thresh = 0.45f)
        {
            // Iterate through the proposals and perform non-maximum suppression
            List<int> proposal_indices = new List<int>();

            for (int i = 0; i < proposals.Count; i++)
            {
                // Calculate the intersection and union areas
                BBox2D a = proposals[i];
                bool keep = proposal_indices.All(j =>
                {
                    BBox2D b = proposals[j];
                    float inter_area = CalcInterArea(a, b);
                    float union_area = CalcUnionArea(a, b);
                    // Keep the proposal if its IoU with all previous proposals is below the NMS threshold
                    return inter_area / union_area <= nms_thresh;
                });

                // If the proposal passes the NMS check, add its index to the list
                if (keep) proposal_indices.Add(i);
            }

            return proposal_indices;
        }

        /// <summary>
        /// Scales and optionally mirrors the bounding box of a detected object to match the in-game screen and display resolutions.
        /// </summary>
        /// <param name="boundingBox">A BBox2D object containing the bounding box information for a detected object.</param>
        /// <param name="inputDims">The dimensions of the input image used for object detection.</param>
        /// <param name="screenDims">The dimensions of the in-game screen where the bounding boxes will be displayed.</param>
        /// <param name="offset">An offset to apply to the bounding box coordinates when scaling.</param>
        /// <param name="mirrorScreen">A boolean flag to indicate if the bounding boxes should be mirrored horizontally (default is false).</param>
        public static BBox2D ScaleBoundingBox(BBox2D boundingBox, Vector2Int inputDims, Vector2 screenDims, Vector2Int offset, bool mirrorScreen)
        {
            // The smallest dimension of the screen
            float minScreenDim = Mathf.Min(screenDims.x, screenDims.y);
            // The smallest input dimension
            int minInputDim = Mathf.Min(inputDims.x, inputDims.y);
            // Calculate the scale value between the in-game screen and input dimensions
            float minImgScale = minScreenDim / minInputDim;
            // Calculate the scale value between the in-game screen and display
            float displayScale = Screen.height / screenDims.y;

            // Scale bounding box to in-game screen resolution and flip the bbox coordinates vertically
            float x0 = (boundingBox.x0 + offset.x) * minImgScale;
            float y0 = (inputDims.y - (boundingBox.y0 - offset.y)) * minImgScale;
            float width = boundingBox.width * minImgScale;
            float height = boundingBox.height * minImgScale;

            // Mirror bounding box across screen
            if (mirrorScreen)
            {
                x0 = screenDims.x - x0 - width;
            }

            // Scale bounding boxes to display resolution
            boundingBox.x0 = x0 * displayScale;
            boundingBox.y0 = y0 * displayScale;
            boundingBox.width = width * displayScale;
            boundingBox.height = height * displayScale;

            // Offset the bounding box coordinates based on the difference between the in-game screen and display
            boundingBox.x0 += (Screen.width - screenDims.x * displayScale) / 2;

            return boundingBox;
        }
    }
}
