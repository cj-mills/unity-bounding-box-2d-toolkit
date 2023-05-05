# Bounding Box 2D Toolkit
Bounding Box 2D Toolkit is a Unity package that provides an easy-to-use and customizable solution to work with and visualize 2D bounding boxes on a Unity canvas.

## Demo Video
https://user-images.githubusercontent.com/9126128/230750644-6f234dfc-27dc-40e2-a354-286b1e38dcf6.mp4



## Demo Projects

| GitHub Repository                                            | Description                                                |
| ------------------------------------------------------------ | ---------------------------------------------------------- |
| [barracuda-inference-yolox-demo](https://github.com/cj-mills/barracuda-inference-yolox-demo) | Perform object detection using YOLOX models.               |



## Code Walkthrough

* [Code Walkthrough: Unity Bounding Box 2D Toolkit Package](https://christianjmills.com/posts/unity-bounding-box-2d-toolkit-walkthrough/)



## Features

- Display 2D bounding boxes with customizable transparency and colors
- Display labels and label backgrounds associated with bounding boxes
- Easily toggle the visibility of bounding boxes, labels, and backgrounds
- Automatically manage and update UI elements based on provided bounding box data
- Compatible with Unity UI and TextMeshPro



## Getting Started

### Prerequisites

- Unity game engine

### Installation

You can install the Bounding Box 2D Toolkit package using the Unity Package Manager:

1. Open your Unity project.
2. Go to Window > Package Manager.
3. Click the "+" button in the top left corner, and choose "Add package from git URL..."
4. Enter the GitHub repository URL: `https://github.com/cj-mills/unity-bounding-box-2d-toolkit.git`
5. Click "Add". The package will be added to your project.

For Unity versions older than 2021.1, add the Git URL to the `manifest.json` file in your project's `Packages` folder as a dependency:

```json
{
  "dependencies": {
    "com.cj-mills.unity-bounding-box-2d-toolkit": "https://github.com/cj-mills/unity-bounding-box-2d-toolkit.git",
    // other dependencies...
  }
}

```







## License

This project is licensed under the MIT License. See the [LICENSE](Documentation~/LICENSE) file for details.
