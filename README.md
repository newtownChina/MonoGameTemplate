# MyGame
 一个MonoGame游戏模板
 功能：
 解析Tiled（https://www.mapeditor.org/）地图编辑器绘制的瓦片碰撞形状，支持由顶点构成的多边形碰撞和矩形碰撞。
 瓦片矩形碰撞有三种算法：
 1.瓦片逐点描边算法：根据瓦片边缘生成点状碰撞形状（宽度、高度都是1px）
 2.瓦片逐行绘制矩形算法：根据瓦片边缘生成点状碰撞形状（宽为瓦片宽度，高度是1px）
 3.瓦片逐行绘制矩形合并算法：根据瓦片边缘生成点状碰撞形状，并进一步将Position.X相同，宽度相同的矩形合并成一个大矩形（宽为瓦片宽度，高度是合并后大矩形高度）
 
 模板结构：
![image](https://github.com/newtownChina/MonoGameTemplate/blob/master/Content/3.png)
![image](https://github.com/newtownChina/MonoGameTemplate/blob/master/Content/1.png)
![image](https://github.com/newtownChina/MonoGameTemplate/blob/master/Content/2.png)
